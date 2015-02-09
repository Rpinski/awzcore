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
using System.Reflection;
using System.IO;
using awzcore.Interfaces;
using awzcore.Metainfo;
using awzcore.Lifecycle;

namespace awzcore.Registration
{
	/// <summary>
	/// Registration implementation that gets all needed information
	/// about components and services from class attributes.
	/// </summary>
	public class AttributeBasedRegistration : IRegistration
	{
		ICallParameterHandler _callParameterHandler;
		ILogger _logger;

		string _componentPath = String.Empty;

		List<string> _loadedAssemblies = new List<string>();

		readonly Dictionary<string, IComponentInfo> _componentInfos = new Dictionary<string, IComponentInfo>();
		readonly Dictionary<Type, IServiceInfo> _serviceInfos = new Dictionary<Type, IServiceInfo>();
		readonly Dictionary<Type, Type> _substitutions = new Dictionary<Type, Type>();
		readonly Dictionary<object, IList<Type>> _serviceLists = new Dictionary<object, IList<Type>>();

		bool _loadedData = false;

		public AttributeBasedRegistration(ICallParameterHandler callParameterHandler, ILogger logger)
		{
			if (callParameterHandler == null)
				throw new ArgumentNullException("callParameterHandler");
			if (logger == null)
				throw new ArgumentNullException("logger");

			_callParameterHandler = callParameterHandler;
			_logger = logger;
		}

		#region IRegistration implementation

		public void LoadComponentData(IEnumerable<Assembly> assemblies)
		{
			if (_loadedData)
				return;

			// Iterate through all loaded assemblies searching for NQ components
			Dictionary<Assembly, ComponentInfo> componentAssemblies = new Dictionary<Assembly, ComponentInfo>();
			Dictionary<string, List<Assembly>> componentPartAssemblies = new Dictionary<string, List<Assembly>>();
			foreach (Assembly a in assemblies)
			{
				if (AttributeComponentInfo.IsComponent(a))
				{
					// Create component info and save it
					AttributeComponentInfo componentInfo = new AttributeComponentInfo(a);

					if (!componentInfo.IsComponentPart)
					{
						// Add component definition to internal list
						if (!_componentInfos.ContainsKey(componentInfo.Name))
						{
							_componentInfos.Add(componentInfo.Name, componentInfo);
						}
						else
						{
							// Component is already known
							_logger.Write(LogType.Error, "Multiple definition of component " + componentInfo.Name + ".");
							throw new ComponentDefinitionException(ErrorReason.ComponentMultiplyDefined, "Multiple definition of component " + componentInfo.Name + ".");
						}
					}
					else
					{
						// Add assembly to temporary parts list
						if (!componentPartAssemblies.ContainsKey(componentInfo.Name))
						{
							componentPartAssemblies.Add(componentInfo.Name, new List<Assembly>());
						}
						componentPartAssemblies[componentInfo.Name].Add(a);
					}

					// Save assembly and ComponentInfo reference in hashtable for later iteration
					componentAssemblies.Add(a, componentInfo);
				}
			}

			// Convert part assembly lists to arrays
			foreach (KeyValuePair<string, List<Assembly>> partList in componentPartAssemblies)
			{
				((ComponentInfo) ComponentInfos[partList.Key]).PartAssemblies = partList.Value;
			}

			// Now iterate through all well-defined NQ components and search for services inside
			foreach (KeyValuePair<Assembly, ComponentInfo> nqAssembly in componentAssemblies)
			{
				ComponentInfo compInfo = nqAssembly.Value;

				Type[] classes = nqAssembly.Key.GetTypes();
				if (classes != null)
				{
					foreach (Type comptype in classes)
					{
						if (AttributeServiceInfo.IsValidServiceClass(comptype))
						{
							// Firstly create a NQServiceInfo, we'll get all other information from it later
							AttributeServiceInfo serviceInfo = new AttributeServiceInfo(comptype, compInfo.Name);

							if (!ServiceInfos.ContainsKey(serviceInfo.ServiceInterface))
							{
								ServiceInfos.Add(serviceInfo.ServiceInterface, serviceInfo);
							}
							else
							{
								throw new ServiceDefinitionException(ErrorReason.ServiceMultiplyDefined,
									"Service \"" + serviceInfo.ServiceInterface + "\" is multiply defined.");
							}
						}
					}
				}
			}

			_loadedData = true;
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

		public string GetComponentPath()
		{
			return _componentPath;
		}
	}
}
