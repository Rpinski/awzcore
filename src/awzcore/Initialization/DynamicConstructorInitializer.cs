// 	------------------------------------------------------------------------
// 	 awzcore Library
//
// 	 Homepage: http://www.awzhome.de/
// 	------------------------------------------------------------------------
//
// 	This Source Code Form is subject to the terms of the Mozilla Public
// 	License, v. 2.0. If a copy of the MPL was not distributed with this
// 	file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// 	The Original Code is code of awzcore Library.
//
// 	The Initial Developer of the Original Code is Andreas Weizel.
// 	Portions created by the Initial Developer are
// 	Copyright (C) 2012-2014 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using awzcore.Interfaces;
//using System.Configuration;
using System.Threading.Tasks;

namespace awzcore.Initialization
{
	/// <summary>
	/// Initializer that can inject any number of parameters into a service's constructor dynamically.
	/// </summary>
	/// <remarks>
	/// This creator works by analyzing the given service using reflection
	/// and then compiling code fragments, which later create requested
	/// service instances. Compiling is performed "lazily" on demand.
	/// </remarks>
	public class DynamicConstructorInitializer : IInitializer
	{
		private Dictionary<string, Func<IList<object>, object>> _parametrizedCreators =
			new Dictionary<string, Func<IList<object>, object>>();
		private IServiceInfo _serviceInfo;
		private ILogger _logger;

		public DynamicConstructorInitializer(ILogger logger)
		{
			if (logger == null)
				throw new ArgumentNullException("logger");
				
			_logger = logger;
		}

		#region IInitializer implementation

		public object Create(InitializationData data)
		{
			_serviceInfo = data.ServiceInfo;
			Func<IList<object>, object> compiledCreator = this.GenerateCompiledCreator(data.DynamicParameters.Select(t => t.Item1).ToList());
			if (compiledCreator != null)
			{
				return compiledCreator(data.DynamicParameters.Select(t => t.Item2).ToList());
			}

			return null;
		}

		public bool SupportsDynamicParameters
		{
			get
			{
				return true;
			}
		}

		#endregion

		/// <summary>
		/// Generates and compiles a new creator lambda expression for the given service.
		/// </summary>
		/// <param name="cctorParamTypes">Types of the parameters passed to the service directly from code.</param>
		private Func<IList<object>, object> GenerateCompiledCreator(IList<Type> cctorParamTypes)
		{
			IServiceInfo servInfo = _serviceInfo;
			string generatorKey = this.CreateParametrizedCreatorKey(cctorParamTypes);
			if (_parametrizedCreators.ContainsKey(generatorKey))
			{
				return _parametrizedCreators[generatorKey];
			}

			_logger.Write(LogType.Debug, "Begin compiling creator for " + servInfo.ServiceInterface.ToString());

			if (((servInfo.AutoInjections == null) || servInfo.AutoInjections.Any())
			    && (((cctorParamTypes == null) || (cctorParamTypes.Count == 0))))
			{
				// Check whether we have an empty constructor in created class
				ConstructorInfo cctor = _serviceInfo.ServiceType.GetConstructor(new Type[] { });
				if (cctor == null)
				{
					// No suitable constructor in service implementation class
					string message = String.Format("Service implementation for {0} does not offer a constructor for given parameters.",
						                 _serviceInfo.ServiceInterface.Name);
					_logger.Write(LogType.Error, "Unsuitable constructor: " + message);
					throw new ServiceLoadingException(ErrorReason.NoConstructor, message);
				}

				// We need a constructor without parameters, we can get it without dynamic compiling.
				MethodInfo getSimpleCreatorMethodGeneric =
					typeof(DynamicConstructorInitializer).GetMethod("GetSimpleCreator", BindingFlags.Instance | BindingFlags.NonPublic);
				MethodInfo getSimpleCreatorMethod =
					getSimpleCreatorMethodGeneric.MakeGenericMethod(new Type[] { servInfo.ServiceType });
				Func<IList<object>, object> simpleCreator = (Func<IList<object>, object>) getSimpleCreatorMethod.Invoke(this, new object[] { });
				_parametrizedCreators.Add(generatorKey, simpleCreator);

				_logger.Write(LogType.Debug, "End compiling (simple) creator for " + servInfo.ServiceInterface.ToString());

				return simpleCreator;
			}
			else
			{
				// Complex constructor of service class: Use DLR functionality to compile a creator delegate for it
				List<Expression> expressionList = new List<Expression>();
				List<ParameterExpression> declarationList = new List<ParameterExpression>();
				List<ParameterExpression> paramExpressions = new List<ParameterExpression>();

				/* Construct following dynamic method:
				 * delegate(ServiceManager manager, object cctorParam1, ..., object cctorParamN)
				 * {
				 *		var autoInjected1 = manager.GetAttachedServices(autoInjection.BoundAttachList), true, false);
				 *			...
				 *		var autoInjected[n] = manager.CreateService(autoInjection.BoundService);
				 *		return new [ServiceType]([params]);
				 *	}
				 */

				// TODO Delegate parameter, there is no static instance of ServiceManager!
				ParameterExpression managerVarExpr = Expression.Parameter(typeof(ServiceManager), "manager");
				declarationList.Add(
					managerVarExpr
				);
				expressionList.Add(
					Expression.Assign(managerVarExpr, Expression.Property(null, typeof(ServiceManager).GetProperty("Instance")))
				);

				ParameterExpression creatorParameterExpr = Expression.Parameter(typeof(object[]), "dynamicParams");

				if ((cctorParamTypes != null) && (cctorParamTypes.Count > 0))
				{
					// Create ParameterExpressions from the given dynamic parameter types
					int dynamicParamCounter = 0;
					paramExpressions.AddRange(
						cctorParamTypes.Select<Type, ParameterExpression>(
							type => Expression.Parameter(type, "cctorParam" + (dynamicParamCounter++).ToString())
						)
					);

					for (int i = 0; i < cctorParamTypes.Count; i++)
					{
						// Create an assigment of parameter
						expressionList.Add(
							Expression.IfThenElse(
								Expression.And(
									Expression.LessThan(
										Expression.Constant(i),
										Expression.Property(creatorParameterExpr, typeof(object[]).GetProperty("Length"))
									),
									Expression.TypeIs(
										Expression.ArrayIndex(
											creatorParameterExpr,
											Expression.Constant(i)
										),
										cctorParamTypes[i]
									)
								),
								Expression.Assign(
									paramExpressions[i],
									Expression.Convert(
										Expression.ArrayIndex(
											creatorParameterExpr,
											Expression.Constant(i)
										),
										cctorParamTypes[i]
									)
								),
								Expression.Assign(
									paramExpressions[i],
									Expression.Constant(null, cctorParamTypes[i])
								)
							)
						);
					}
				}

				// Collect auto-injected services
				if (servInfo.AutoInjections != null)
				{
					int j = 0;
					foreach (var autoInjection in servInfo.AutoInjections)
					{
						if (autoInjection.Interface != null)
						{
							if (autoInjection.InjectFromAttachList)
							{
								// TODO Check ServiceManager method names!
								MethodInfo getAttachedServicesMethodGeneric = typeof(ServiceManager).GetMethod("GetAttachedServices",
									                                              new Type[] { typeof(string), typeof(bool), typeof(bool) });
								MethodInfo getAttachedServicesMethod =
									getAttachedServicesMethodGeneric.MakeGenericMethod(new Type[] { autoInjection.Interface });

								// Create parameter expressions
								ParameterExpression instancesExpr =
									Expression.Parameter(autoInjection.Interface.MakeArrayType(), "autoInjected" + j.ToString());
								paramExpressions.Add(instancesExpr);
								expressionList.Add(
									Expression.Assign(
										instancesExpr,
										Expression.Call(
											managerVarExpr,
											getAttachedServicesMethod,
											new Expression[] {
												Expression.Constant(autoInjection.BoundAttachList, typeof(string)),
												Expression.Constant(true, typeof(bool)),
												Expression.Constant(false, typeof(bool))
											}
										)
									)
								);
							}
							else
							{
								if (!autoInjection.InjectAsDependent)
								{
									if (autoInjection.Overridable && (paramExpressions.Count > 0))
									{
										// When overriding, use the first of the passed dependent instances instead of creating one
										paramExpressions.Add(paramExpressions[0]);
										paramExpressions.RemoveAt(0);
									}
									else
									{
										// Find appropriate CreateService() using reflection, because we don't know the
										// generic type parameter at compile time.
										// TODO Check ServiceManager method names!
										MethodInfo[] createDependentServiceGenericMethods = typeof(ServiceManager).GetMethods();
										for (int i = 0; i < createDependentServiceGenericMethods.Length; i++)
										{
											if ((createDependentServiceGenericMethods[i].Name == "CreateService")
											    && createDependentServiceGenericMethods[i].IsGenericMethod)
											{
												MethodInfo createDependentServiceMethod =
													createDependentServiceGenericMethods[i].MakeGenericMethod(
														new Type[] { autoInjection.Interface });

												// Create parameter expressions
												ParameterExpression instancesExpr =
													Expression.Parameter(autoInjection.Interface, "autoInjected" + j.ToString());
												paramExpressions.Add(instancesExpr);
												expressionList.Add(
													Expression.Assign(
														instancesExpr,
														Expression.Call(
															managerVarExpr,
															createDependentServiceMethod,
															Expression.Constant(autoInjection.BoundService, typeof(Type))
														)
													)
												);
											}
										}
									}
								}
							}
						}
					}

					j++;
				}

				// Add the expression for creation of the main instance
				ConstructorInfo cctor = servInfo.ServiceType.GetConstructor(
					                        paramExpressions.Select<ParameterExpression, Type>(p => p.Type).ToArray()
				                        );

				if (cctor != null)
				{
					expressionList.Add(
						Expression.New(cctor, paramExpressions.ToArray())
					);

					// Compile the expression
					List<ParameterExpression> allDeclarations = new List<ParameterExpression>();
					allDeclarations.AddRange(paramExpressions);
					allDeclarations.AddRange(declarationList);
					List<Expression> blockExpressions = new List<Expression>();
					//blockExpressions.AddRange(dynamicParamValueAssignments);
					blockExpressions.AddRange(expressionList);
					BlockExpression creatorBlock = Expression.Block(allDeclarations.ToArray(), blockExpressions);
					Func<IList<object>, object> compiledCreator =
						Expression.Lambda<Func<IList<object>, object>>(creatorBlock, creatorParameterExpr).Compile();
					_parametrizedCreators.Add(generatorKey, compiledCreator);

					_logger.Write(LogType.Debug, "End compiling creator for " + servInfo.ServiceInterface.ToString());

					return compiledCreator;
				}
				else
				{
					// No suitable constructor in service implementation class
					string message = String.Format("Service implementation for {0} does not offer a constructor for given parameters.",
						                 _serviceInfo.ServiceInterface.Name);
					_logger.Write(LogType.Error, "Unsuitable constructor: " + message);
					throw new ServiceLoadingException(ErrorReason.NoConstructor, message);
				}
			}
		}

		/// <summary>
		/// Creates a key for the hash table of creators from an
		/// <see cref="IServiceInfo"/> and the list of the
		/// passed parameter types.
		/// </summary>
		/// <param name="cctorParamTypes">Array of types of the passed parameters.</param>
		/// <returns>Created key as string.</returns>
		private string CreateParametrizedCreatorKey(IList<Type> cctorParamTypes)
		{
			string key = _serviceInfo.ServiceInterface.FullName + "|";
			if ((cctorParamTypes != null) && (cctorParamTypes.Count > 0))
			{
				key += String.Join("|", cctorParamTypes.Select<Type, string>(t => t.FullName));
			}

			return key;
		}

		private Func<IList<object>, object> GetSimpleCreator<T>()
			where T : new()
		{
			return (a) => new T();
		}
	}
}
