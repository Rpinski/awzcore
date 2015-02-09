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
using System.IO;
using System.Reflection;
using awzcore.Interfaces;
using awzcore.Serialization;
using awzcore.Metainfo;

namespace awzcore.Registration
{
	/// <summary>
	/// Uses external XML component definitions
	/// to retrieve the component and service information.
	/// </summary>
	public class XmlBasedRegistration : IRegistration, IComponentAssemblyProvider, IBootstrapRegistration
	{
		ICallParameterHandler _callParameterHandler;
		ILogger _logger;
		ComponentInfoXmlSerializer _serializer;

		readonly Dictionary<string, IComponentInfo> _componentInfos = new Dictionary<string, IComponentInfo>();
		readonly Dictionary<Type, IServiceInfo> _serviceInfos = new Dictionary<Type, IServiceInfo>();
		readonly Dictionary<Type, Type> _substitutions = new Dictionary<Type, Type>();
		readonly Dictionary<object, IList<Type>> _serviceLists = new Dictionary<object, IList<Type>>();
		readonly Dictionary<Type, ILifecycle> _lifecycles = new Dictionary<Type, ILifecycle>();

		string _componentPath = String.Empty;
		string _definitionPath = String.Empty;

		bool _loadedData = false;

		/// <summary>
		/// Creates a new instance of <see cref="XmlBasedRegistration" />.
		/// </summary>
		public XmlBasedRegistration(ICallParameterHandler callParameterHandler, ILogger logger)
		{
			if (callParameterHandler == null)
				throw new ArgumentNullException("callParameterHandler");
			if (logger == null)
				throw new ArgumentNullException("logger");

			_callParameterHandler = callParameterHandler;
			_logger = logger;
			_serializer = new ComponentInfoXmlSerializer();
		}

		#region IBootstrapRegistration implementation

		public void Register(IBootstrapServiceContainer serviceContainer)
		{
			if (serviceContainer == null)
				throw new ArgumentNullException("serviceContainer");
			serviceContainer.Register<IRegistration>(this);
			serviceContainer.Register<IComponentAssemblyProvider>(this);
		}

		#endregion

		#region IComponentAssemblyProvider

		public IEnumerable<Assembly> GetAssemblies()
		{
			if (_loadedData)
			{
				// Trying to load data twice!?
				throw new InvalidOperationException("Trying to load component assemblies twice.");
			}

			List<Assembly> loadedAssemblies = new List<Assembly>();

			// Get directory with component definition files
			string[] cddParam = _callParameterHandler["cdd"];
			if ((cddParam != null) && (cddParam.Length > 0))
			{
				// Use given directory
				_definitionPath = cddParam[0];
			}
			else
			{
				// Use "components" as default subdirectory
				_definitionPath = Path.Combine(Environment.CurrentDirectory, "components");
			}

			// Iterate through all XML component files and try to deserialize them
			string fileMask = "*.xml";
			string[] fileMaskCmd = _callParameterHandler["cdfm"];
			if ((fileMaskCmd != null) && (fileMaskCmd.Length > 0))
			{
				fileMask = fileMaskCmd[0];
			}

			foreach (string xmlFile in Directory.GetFiles(_definitionPath, fileMask))
			{
				using (FileStream fileStream = new FileStream(xmlFile, FileMode.Open))
				{
					IComponentInfo deserializedCompInfo;
					IServiceInfo[] deserializedServInfos;
					_serializer.ReadComponentInfo(fileStream, out deserializedCompInfo, out deserializedServInfos);

					// Add component and services to internal list
					ComponentInfos.Add(deserializedCompInfo.Name, deserializedCompInfo);
					if (deserializedServInfos != null)
					{
						foreach (var serviceInfo in deserializedServInfos)
						{
							ServiceInfos[serviceInfo.ServiceType] = serviceInfo;
						}
					}
				}
			}

			loadedAssemblies.AddRange(LoadComponentAssemblies());

			_loadedData = true;
			return loadedAssemblies;
		}

		#endregion

		#region IRegistration implementation

		public void LoadComponentData(IEnumerable<Assembly> assemblies)
		{
			if (_loadedData)
				return;

		}

		public IDictionary<string, IComponentInfo> ComponentInfos
		{
			get
			{
				return _componentInfos;
			}
		}

		public IDictionary<Type, IServiceInfo> ServiceInfos
		{
			get
			{
				return _serviceInfos;
			}
		}

		public IDictionary<Type, Type> Substitutions
		{
			get
			{
				return _substitutions;
			}
		}

		public IDictionary<object, IList<Type>> ServiceLists
		{
			get
			{
				return _serviceLists;
			}
		}

		#endregion

		IEnumerable<Assembly> LoadComponentAssemblies()
		{
			List<Assembly> loadedAssemblies = new List<Assembly>();

			foreach (IComponentInfo componentInfo in ComponentInfos.Values)
			{
				// Try to load all assemblies of the given component
				ComponentInfo serComponentInfo = componentInfo as ComponentInfo;
				if (serComponentInfo != null)
				{
					try
					{
						// Fill the Assembly references
						int startIndex = 0;
						if (String.IsNullOrEmpty(serComponentInfo.MainAssemblyFullName))
						{
							if ((serComponentInfo.PartAssembliesFullNames != null) && (serComponentInfo.PartAssembliesFullNames.Length > 0))
							{
								// No main assembly is defined, use first part assembly instead
								serComponentInfo.MainAssemblyFullName = serComponentInfo.PartAssembliesFullNames[0];
								startIndex = 1;
							}
							else
							{
								// There are no assembly definitions at all, exit
								return loadedAssemblies;
							}
						}

						List<Assembly> partAssemblyList = new List<Assembly>();
						AssemblyName asmName = new AssemblyName(serComponentInfo.MainAssemblyFullName);
						Assembly mainAssembly = AppDomain.CurrentDomain.Load(asmName);
						serComponentInfo.MainAssembly = mainAssembly;

						if (serComponentInfo.PartAssembliesFullNames != null)
						{
							for (int i = startIndex; i < serComponentInfo.PartAssembliesFullNames.Length; i++)
							{
								asmName = new AssemblyName(serComponentInfo.PartAssembliesFullNames[i]);
								Assembly partAssembly = AppDomain.CurrentDomain.Load(asmName);
								partAssemblyList.Add(partAssembly);
							}

							serComponentInfo.PartAssemblies = partAssemblyList;
						}

						// Fill type objects as well
						var componentServiceInfos =
							from sInfo in _serviceInfos
							where (sInfo.Value.ParentComponent == componentInfo.Name) && (sInfo.Value is ServiceInfo)
							select sInfo.Value as ServiceInfo;
						foreach (ServiceInfo item in componentServiceInfos)
						{
							item.ServiceType = this.FindTypeInComponent(serComponentInfo, item.ServiceTypeName);
							item.ServiceInterface = this.FindServiceInterfaceInType(item.ServiceType, item.ServiceInterfaceName);

							// Get type values for the substitutes
							if (item.SubstituteNames != null)
							{
								item.Substitutes =
									(from s in item.SubstituteNames
								  select Type.GetType(s, false)).ToArray<Type>();
							}

							// Get type values for the auto-injection definitions
							if (item.AutoInjections != null)
							{
								foreach (var autoInjection in item.AutoInjections)
								{
									if (autoInjection is AutoInjection)
									{
										((AutoInjection) autoInjection).Interface =
											Type.GetType(((AutoInjection) autoInjection).ServiceInterfaceName);
									}
								}
							}

							// Get MethodInfos for invoke and quit methods
							// TODO How to handle this in future?
//							if (item.InvokeMethodName != null)
//							{
//								try
//								{
//									item.InvokeMethod = item.ServiceType.GetMethod(item.InvokeMethodName, new Type[] { });
//								}
//								finally
//								{
//								}
//							}
//							if (item.QuitMethodName != null)
//							{
//								try
//								{
//									item.QuitMethod = item.ServiceType.GetMethod(item.QuitMethodName, new Type[] { });
//								}
//								finally
//								{
//								}
//							}
						}
					}
					catch (Exception ex)
					{
						// We could not load this assembly
						_logger.Error(ex, "Couldn't load assembly for component '{0}'.", componentInfo.Name);
					}
				}
			}

			return loadedAssemblies;
		}

		public string GetComponentPath()
		{
			return _componentPath;
		}

		private Type FindTypeInComponent(IComponentInfo componentInfo, string typeName)
		{
			List<Assembly> allAssemblies = new List<Assembly>();
			allAssemblies.Add(componentInfo.MainAssembly);
			if ((componentInfo.PartAssemblies != null) && componentInfo.PartAssemblies.Any())
			{
				allAssemblies.AddRange(componentInfo.PartAssemblies);
			}

			// Search for the type in all modules of all assemblies belonging to the passed component
			foreach (Assembly assembly in allAssemblies)
			{
				foreach (Module module in assembly.GetModules(false))
				{
					Type foundType = module.GetType(typeName, false, false);
					if (foundType != null)
					{
						// We have found the type
						return foundType;
					}
				}
			}

			return null;
		}

		private Type FindServiceInterfaceInType(Type serviceType, string serviceInterfaceName)
		{
			Type resultType = serviceType;

			if (serviceType != null)
			{
				// Iterate through all interfaces implemented by the service type
				foreach (Type interfaceType in serviceType.GetInterfaces())
				{
					if (interfaceType.FullName == serviceInterfaceName)
					{
						// Found it!
						resultType = interfaceType;
						break;
					}
				}
			}

			return resultType;
		}
	}
}
