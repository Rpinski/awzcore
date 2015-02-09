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
// 	The Original Code is code of NQ Core Library.
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
using System.Reflection;
using awzcore.Interfaces;
using System.Linq;

namespace awzcore
{
	/// <summary>
	/// Validates a list of <see cref="IServiceInfo"/>
	/// objects and generates information about faulty service definitions.
	/// </summary>
	public class ServiceInfoValidator
	{
		private IDictionary<Type, IServiceInfo> _serviceInfos;
		private IDictionary<Type, Type> _substitutions;
		private IDictionary<string, IList<Type>> _attachLists;

		/// <summary>
		/// Initializes a new validator instance.
		/// </summary>
		/// <param name="serviceInfos">Dictionary of <see cref="IServiceInfo"/> objects, keyed by service interfaces.</param>
		/// <param name="substitutions">Key/value pairs for a service substitution list.</param>
		/// <param name="attachLists">List of AttachLists (= string lists), keyed by their names.</param>
		public ServiceInfoValidator(IDictionary<Type, IServiceInfo> serviceInfos, IDictionary<Type, Type> substitutions, IDictionary<string, IList<Type>> attachLists)
		{
			if (serviceInfos != null)
			{
				_serviceInfos = serviceInfos;
			}
			else
			{
				_serviceInfos = new Dictionary<Type, IServiceInfo>();
			}
			if (substitutions != null)
			{
				_substitutions = substitutions;
			}
			else
			{
				_substitutions = new Dictionary<Type, Type>();
			}
			if (attachLists != null)
			{
				_attachLists = attachLists;
			}
			else
			{
				_attachLists = new Dictionary<string, IList<Type>>();
			}
		}

		/// <summary>
		/// Container for information about faulty service definitions.
		/// </summary>
		public class FaultyServiceInfo
		{
			/// <summary>
			/// Initializes a
			/// <see cref="ServiceInfoValidator.FaultyServiceInfo"/>
			/// instance.
			/// </summary>
			/// <param name="reason">Reason for the definition error as constant.</param>
			/// <param name="message">Reason for the definition error as textual description.</param>
			public FaultyServiceInfo(ErrorReason reason, string message)
			{
				this.Reason = reason;
				this.Message = message;
			}

			/// <summary>
			/// Reason for definition error, represented by an
			/// <see cref="ErrorReason"/> value.
			/// </summary>
			public ErrorReason Reason
			{
				get;
				private set;
			}

			/// <summary>
			/// Textual description of the reason for the service definition error.
			/// </summary>
			public string Message
			{
				get;
				private set;
			}
		}

		/// <summary>
		/// Constants for the result of a circularity check.
		/// </summary>
		private enum CircularityResult
		{
			/// <summary>
			/// No errors.
			/// </summary>
			NoErrors,
			/// <summary>
			/// Circular references found.
			/// </summary>
			Circularity,
			/// <summary>
			/// A reference could not be resolved.
			/// </summary>
			NotFoundError
		}

		/// <summary>
		/// Checks the consistency of all service definitions and returns a list
		/// of faulty services with the error information that have been found for every service.
		/// </summary>
		/// <returns>
		/// Array of <see cref="ServiceInfoValidator.FaultyServiceInfo"/>
		/// objects, that contain information about faulty service definitions.
		/// </returns>
		public FaultyServiceInfo[] CheckServiceConsistency()
		{
			List<FaultyServiceInfo> faultyServices = new List<FaultyServiceInfo>();
			//try
			//{
			foreach (IServiceInfo serviceInfo in _serviceInfos.Values)
			{
				if ((serviceInfo.AutoInjections != null) && serviceInfo.AutoInjections.Any())
				{
					// Search for circular injections (Service2 is injected into Service1, Service1 is injected into Service2 etc.)
					List<Type> chainList = new List<Type>();
					CircularityResult circularityResult = this.FindCircularAutoInjection(serviceInfo.ServiceInterface, chainList);
					if (circularityResult == CircularityResult.Circularity)
					{
						// There is a circular reference somewhere
						faultyServices.Add(new FaultyServiceInfo(ErrorReason.CircularAutoInjection,
							"The auto-injection definition for " + serviceInfo.ServiceInterface + " contains circular references.")
						);
					}
					else if (circularityResult == CircularityResult.NotFoundError)
					{
						// There is a circular reference somewhere
						faultyServices.Add(new FaultyServiceInfo(ErrorReason.UnresolvedAutoInjection,
							"The auto-injection definition for " + serviceInfo.ServiceInterface + " contains unresolved references.")
						);
					}

					// Test constructor of implementing type
					List<Type> ctorParams = new List<Type>();
					foreach (IAutoInjection injection in serviceInfo.AutoInjections)
					{
						if (injection.InjectFromAttachList)
						{
							ctorParams.Add(injection.Interface.MakeArrayType());
						}
						else
						{
							ctorParams.Add(injection.Interface);
						}
					}

					// Run through all constructors of the implementing type and look for a fitting one
					bool foundCtor = false;
					ConstructorInfo[] ctors = serviceInfo.ServiceType.GetConstructors();
					if (ctors != null)
					{
						for (int i = 0; i < ctors.Length; i++)
						{
							ParameterInfo[] parameters = ctors[i].GetParameters();
							int minParamCount = 0;
							if (ctorParams.Count < parameters.Length)
							{
								minParamCount = ctorParams.Count;
							}
							else
							{
								minParamCount = parameters.Length;
							}
							for (int j = minParamCount - 1; j >= 0; j--)
							{
								if (ctorParams[j].IsAssignableFrom(parameters[j].ParameterType))
								{
									foundCtor = true;
								}
								else
								{
									foundCtor = false;
									break;
								}
							}

							if (foundCtor)
							{
								// We have found a fitting constructor, no more constructors are checked
								break;
							}
						}
					}

					if (!foundCtor)
					{
						// Could not find an appropriate constructor for the auto-injection definition
						faultyServices.Add(new FaultyServiceInfo(ErrorReason.NoConstructor,
							"The class implementing service " + serviceInfo.ServiceInterface.Name + " does not contain a constructor, that is needed to auto-inject other services.")
						);
					}
				}
			}

			// Check all substitutes for integrity
			foreach (KeyValuePair<Type, Type> substEntry in _substitutions)
			{
				IServiceInfo newServInfo = this.GetServiceInfo(substEntry.Value);
				IServiceInfo oldServInfo = this.GetServiceInfo(substEntry.Key);
				if ((oldServInfo != null) && (newServInfo != null))
				{
					//if (!oldServInfo.ServiceInterface.IsAssignableFrom(newServInfo.ServiceInterface))
					//{
					//  // The subtitutor does not implement the service interface of the substituted service
					//  faultyServices.Add(new FaultyServiceInfo(NQErrorReason.SubstitutorInterfaceMismatch,
					//      "The substituting service " + substEntry.Value + " does not implement the interface of the subtituted service " + substEntry.Key + ".")
					//      );
					//}

					Type[] oldInterfaces = oldServInfo.ServiceType.GetInterfaces();
					if ((oldInterfaces == null) || (oldInterfaces.Length == 0))
					{
						oldInterfaces = new Type[] { oldServInfo.ServiceType };
					}

					foreach (Type ifc in oldInterfaces)
					{
						if (!ifc.IsAssignableFrom(newServInfo.ServiceType))
						{
							// The subtitutor does not implement a service interface of the substituted servicefe
							faultyServices.Add(new FaultyServiceInfo(ErrorReason.SubstitutorInterfaceMismatch,
								"The substituting service " + substEntry.Value + " does not implement all interfaces of the subtituted service " + substEntry.Key + ".")
							);
						}
					}
				}
			}

			// Check all substitution chains for cyclic references
			foreach (KeyValuePair<Type, Type> substEntry in _substitutions)
			{
				List<Type> chainList = new List<Type>();
				CircularityResult circularityResult = this.FindCircularSubstitution(substEntry.Value, chainList);
				if (circularityResult == CircularityResult.Circularity)
				{
					// There is a circular reference somewhere
					faultyServices.Add(new FaultyServiceInfo(ErrorReason.CircularSubstitution,
						"The substitution chain for " + substEntry.Value + " contains circular references.")
					);
				}
			}

			//}
			//catch (Exception ex)
			//{
			//  string test = ex.Message;
			//}

			FaultyServiceInfo[] faultyServicesArray = new FaultyServiceInfo[faultyServices.Count];
			faultyServices.CopyTo(faultyServicesArray, 0);
			return faultyServicesArray;
		}

		/// <summary>
		/// Searches for repeating services in a substitution chain.
		/// </summary>
		/// <param name="serviceInterface">Registered interface to start the check on.</param>
		/// <param name="chainList">List of chain elements, that is filled by the method.</param>
		/// <returns><c>True</c>, if cyclic references have been found, <c>false</c> otherwise.</returns>
		private CircularityResult FindCircularSubstitution(Type serviceInterface, IList<Type> chainList)
		{
			if (chainList.Contains(serviceInterface))
			{
				// The list already contains this service -> circular reference
				return CircularityResult.Circularity;
			}
			else
			{
				chainList.Add(serviceInterface);
			}

			if (_substitutions.ContainsKey(serviceInterface))
			{
				Type substitutor = _substitutions[serviceInterface];
				return this.FindCircularSubstitution(substitutor, chainList);
			}
			else
			{
				return CircularityResult.NoErrors;
			}
		}

		/// <summary>
		/// Recursively searches for repeating services in a auto-injection definition.
		/// </summary>
		/// <param name="serviceInterface">Registered interface to start the check on.</param>
		/// <param name="chainList">List of chain elements, that is filled by the method.</param>
		/// <returns><c>True</c>, if cyclic references have been found, <c>false</c> otherwise.</returns>
		private CircularityResult FindCircularAutoInjection(Type serviceInterface, IList<Type> chainList)
		{
			if (chainList.Contains(serviceInterface))
			{
				// The list already contains this service -> circular reference
				return CircularityResult.Circularity;
			}
			else
			{
				chainList.Add(serviceInterface);
			}

			if (_serviceInfos.ContainsKey(serviceInterface))
			{
				IServiceInfo servInfo = _serviceInfos[serviceInterface];
				if (servInfo.AutoInjections != null)
				{
					foreach (IAutoInjection injection in servInfo.AutoInjections)
					{
						List<Type> subChainList = new List<Type>(chainList);
						if (injection.InjectFromAttachList)
						{
							if (_attachLists.ContainsKey(injection.BoundAttachList))
							{
								// Check all services in this AttachList
								foreach (Type attachedService in _attachLists[injection.BoundAttachList])
								{
									CircularityResult circularityResult = this.FindCircularAutoInjection(attachedService, subChainList);
									if (circularityResult != CircularityResult.NoErrors)
									{
										return circularityResult;
									}
								}
							}
						}
						else
						{
							// Check the bound service
							CircularityResult circularityResult = this.FindCircularAutoInjection(injection.BoundService, subChainList);
							if (circularityResult != CircularityResult.NoErrors)
							{
								return circularityResult;
							}
						}
					}
				}
			}
			else
			{
				return CircularityResult.NotFoundError;
			}

			return CircularityResult.NoErrors;
		}

		private IServiceInfo GetServiceInfo(Type serviceInterface)
		{
			// Return INQServiceInfo object
			if (_serviceInfos.ContainsKey(serviceInterface))
			{
				return _serviceInfos[serviceInterface];
			}
			else
			{
				return null;
			}
		}
	}
}
