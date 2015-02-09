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
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using awzcore.Interfaces;
using awzcore.Metainfo;

namespace awzcore.Serialization
{
	/// <summary>
	/// Helper class for serialization and deserialization between
	/// <see cref="IComponentInfo"/>-
	/// and <see cref="IServiceInfo"/>-based
	/// objects and XML structures.
	/// </summary>
	public class ComponentInfoXmlSerializer
	{
		private Encoding _encoding;
		private ILogger _logger;

		/// <summary>
		/// Creates a new instance of the serializer class.
		/// </summary>
		public ComponentInfoXmlSerializer()
			: this(DummyLogger.Instance, Encoding.UTF8)
		{
		}

		/// <summary>
		/// Creates a new instance of the serializer class and sets its output encoding.
		/// </summary>
		/// <param name="encoding">Encoding for the XML output.</param>
		public ComponentInfoXmlSerializer(Encoding encoding)
			: this(DummyLogger.Instance, encoding)
		{
		}

		/// <summary>
		/// Creates a new instance of the serializer class and sets its output encoding.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="encoding">Encoding for the XML output.</param>
		public ComponentInfoXmlSerializer(ILogger logger, Encoding encoding)
		{
			_encoding = encoding;
			_logger = logger;
		}

		/// <summary>
		/// Reads XML component definition from a stream and creates
		/// <see cref="IComponentInfo"/> and
		/// <see cref="IServiceInfo"/>-based
		/// objects from it.
		/// </summary>
		/// <param name="sourceStream">Source stream with XML definition data.</param>
		/// <param name="componentInfo">Receives a reference to an info object describing the component.</param>
		/// <param name="serviceInfos">Receives a reference to an array of service information objects.</param>
		/// <returns><c>True</c>, if the component information could be read successfully, <c>false</c> otherwise.</returns>
		public bool ReadComponentInfo(Stream sourceStream, out IComponentInfo componentInfo, out IServiceInfo[] serviceInfos)
		{
			ComponentInfo tempCompInfo = new ComponentInfo();
			List<IServiceInfo> servInfoTempArray = new List<IServiceInfo>();

			// We don't catch exceptions here, let the caller do that
			//try
			//{
			XmlSerializer serializer = new XmlSerializer(typeof(XmlStub.DefinitionRootType));
			XmlStub.DefinitionRootType compDef = (XmlStub.DefinitionRootType) serializer.Deserialize(sourceStream);

			tempCompInfo.Name = compDef.name;
			tempCompInfo.DisplayName = compDef.displayname;
			tempCompInfo.DisplayVersion = compDef.displayversion;
//			tempCompInfo.UpdaterURL = compDef.updaterurl;

			// Resource definitions
			if (compDef.resources != null)
			{
				if (compDef.resources.nontranslatedresource != null)
				{
					XmlStub.ResourceType resource = compDef.resources.nontranslatedresource;
//					tempCompInfo.NonTranslatedResKey = resource.key;
//					tempCompInfo.NonTranslatedRes = resource.@namespace;
//					tempCompInfo.NonTranslatedPath = resource.path;
				}
				if (compDef.resources.translatedresource != null)
				{
					XmlStub.ResourceType resource = compDef.resources.translatedresource;
//					tempCompInfo.TranslatedResKey = resource.key;
//					tempCompInfo.TranslatedRes = resource.@namespace;
				}
			}

			// Requirements
			if (compDef.requirements != null)
			{
				foreach (var req in compDef.requirements)
				{
					ComponentRequirement compreq = new ComponentRequirement();
					compreq.ComponentName = req.name;
					compreq.Version = new Version(req.version);
					compreq.Condition = this.GetConditionFromXml(req.@operator);
				}
			}

			// Compatibility information
			if (compDef.compatibility != null)
			{
				foreach (var comp in compDef.compatibility)
				{
					ComponentRequirement compCompat = new ComponentRequirement();
					compCompat.Version = new Version(comp.version);
					if (comp.operatorSpecified)
					{
						compCompat.Condition = this.GetConditionFromXml(comp.@operator);
					}
					else
					{
						compCompat.Condition = Condition.Equal;
					}
				}
			}

			// Assembly information
			if (compDef.assemblies != null)
			{
				List<string> partAssemblyNames = new List<string>();
				//foreach (var assemblyDef in compDef.assemblies)
				//{
				//  // Collect assembly full names
				//  if (assemblyDef.ismainSpecified && assemblyDef.ismain)
				//  {
				//    tempCompInfo.MainAssemblyFullName = Path.GetFileNameWithoutExtension(assemblyDef.file);
				//  }
				//  else
				//  {
				//    partAssemblyNames.Add(Path.GetFileNameWithoutExtension(assemblyDef.file));
				//  }
				//}

				foreach (var assemblyDef in compDef.assemblies)
				{
					// Collect assembly full names
					AssemblyName assemblyName = null;
					string assemblyFullName = String.Empty;
					try
					{
						assemblyName = AssemblyName.GetAssemblyName(assemblyDef.file);
						assemblyFullName = assemblyName.Name;
					}
					catch (Exception)
					{
						_logger.Error("NQComponentInfoSerializer: Assembly " + assemblyDef + " not found or not accessible.");
						continue;
					}

					if (assemblyDef.ismainSpecified && assemblyDef.ismain)
					{
						tempCompInfo.MainAssemblyFullName = assemblyFullName;
					}
					else
					{
						partAssemblyNames.Add(assemblyFullName);
					}

					foreach (var serviceDef in assemblyDef.services)
					{
						ServiceInfo servInfo = new ServiceInfo();
						servInfo.ServiceInterfaceName = serviceDef.name;
						servInfo.ServiceTypeName = serviceDef.@class;
						servInfo.ParentComponent = tempCompInfo.Name;
						// TODO How to handle this in future?
//						if (serviceDef.categorySpecified)
//						{
//							servInfo.ServiceCategory = this.GetHostModeFromXml(serviceDef.category);
//						}
//						else
//						{
//							servInfo.ServiceCategory = HostMode.General;
//						}
						// TODO How to handle this in future?
//						servInfo.SingleInstance = (serviceDef.singleinstanceSpecified && serviceDef.singleinstance);

						// Read invokable service settings
						// TODO How to handle this in future?
//						servInfo.InvokeMethodName = serviceDef.invokemethod;
//						servInfo.QuitMethodName = serviceDef.quitmethod;

						// Handle substitutes
						if (serviceDef.substitutions != null)
						{
							servInfo.SubstituteNames = serviceDef.substitutions;
						}

						// Handle AttachLists
						if (serviceDef.attachedto != null)
						{
							servInfo.MemberOfLists = serviceDef.attachedto;
						}

						// Handle requirements
						if (serviceDef.requirements != null)
						{
							foreach (var req in serviceDef.requirements)
							{
								ComponentRequirement compreq = new ComponentRequirement();
								compreq.ComponentName = req.name;
								compreq.Version = new Version(req.version);
								compreq.Condition = this.GetConditionFromXml(req.@operator);
							}
						}

						// Handle auto-injection
						if (serviceDef.autoinjection != null)
						{
							var autoInjectionDefCollection = serviceDef.autoinjection;
							if (autoInjectionDefCollection.Items != null)
							{
								List<IAutoInjection> autoInjections = new List<IAutoInjection>();

								for (int i = 0; i < autoInjectionDefCollection.Items.Length; i++)
								{
									var autoInjectionDef = autoInjectionDefCollection.Items[i];
									AutoInjection autoInjection = new AutoInjection();

									// Injection source depends on the name of the XML element (<list> or <service>)
									if (autoInjectionDefCollection.ItemsElementName[i] == XmlStub.ItemsChoiceType.list)
									{
										autoInjection.InjectFromAttachList = true;
										autoInjection.BoundAttachList = autoInjectionDef.name;
									}
									else
									{
										autoInjection.InjectFromAttachList = false;
										autoInjection.BoundServiceName = autoInjectionDef.name;
									}

									autoInjection.InjectAsDependent = (autoInjectionDef.asdependentSpecified && autoInjectionDef.asdependent);
									autoInjection.Overridable = (autoInjectionDef.overridableSpecified && autoInjectionDef.overridable);
									autoInjection.ServiceInterfaceName = autoInjectionDef.type;

									autoInjections.Add(autoInjection);
								}

								servInfo.AutoInjections = servInfo.AutoInjections;
							}
						}

						servInfoTempArray.Add(servInfo);
					}
				}

				if (partAssemblyNames.Count > 0)
				{
					tempCompInfo.PartAssembliesFullNames = new string[partAssemblyNames.Count];
					partAssemblyNames.CopyTo(tempCompInfo.PartAssembliesFullNames);
				}
			}

			componentInfo = tempCompInfo;
			serviceInfos = new IServiceInfo[servInfoTempArray.Count];
			servInfoTempArray.CopyTo(serviceInfos);
			return true;
			//}
			//catch (Exception)
			//{
			//  // Component information could not be retrieved from stream
			//  componentInfo = null;
			//  serviceInfos = null;
			//  return false;
			//}
		}

		/// <summary>
		/// Writes XML component definition created from
		/// <see cref="IComponentInfo"/>-
		/// and <see cref="IServiceInfo"/>-based
		/// objects.
		/// </summary>
		/// <param name="destStream">Stream to write the XML data to.</param>
		/// <param name="componentInfo">Component definition to serialize.</param>
		/// <param name="serviceInfos">Service definitions to serialize.</param>
		/// <returns><c>True</c>, if successful, <c>false</c> otherwise.</returns>
		/// <remarks>
		/// The method only serializes the services, that belong to the passed component
		/// (see <see cref="IServiceInfo.ComponentName"/> property).
		/// </remarks>
		public bool WriteComponentInfo(Stream destStream, IComponentInfo componentInfo, IServiceInfo[] serviceInfos)
		{
			if ((componentInfo == null) || (serviceInfos == null))
			{
				return false;
			}

			try
			{
				XmlStub.DefinitionRootType xmlCompInfo = new awzcore.Serialization.XmlStub.DefinitionRootType();

				xmlCompInfo.name = componentInfo.Name;
				xmlCompInfo.displayname = componentInfo.DisplayName;
				xmlCompInfo.displayversion = componentInfo.DisplayVersion;
//				xmlCompInfo.updaterurl = componentInfo.UpdaterURL;

				if ((componentInfo.Requires != null) && componentInfo.Requires.Any())
				{
					// Serialize requirements
					xmlCompInfo.requirements = componentInfo.Requires.Select(
						req =>
						new awzcore.Serialization.XmlStub.RequirementType() {
							name = req.ComponentName,
							@operator = this.GetXmlFromCondition(req.Condition),
							operatorSpecified = true,
							version = req.Version.ToString()
						}
					).ToArray();
				}

				if ((componentInfo.Compatibility != null) && componentInfo.Compatibility.Any())
				{
					// Serialize compatibility settings
					xmlCompInfo.compatibility = componentInfo.Compatibility.Select(
						compat =>
						new awzcore.Serialization.XmlStub.CompatibilityConditionType() {
							@operator = this.GetXmlFromCondition(compat.Condition),
							operatorSpecified = true,
							version = compat.Version.ToString()
						}
					).ToArray();
				}

				// Serialize resources
//				xmlCompInfo.resources = new XmlStub.DefinitionRootTypeResources();
//				if (!String.IsNullOrEmpty(componentInfo.NonTranslatedResKey))
//				{
//					XmlStub.ResourceType nonTranslatedResource = new XmlStub.ResourceType();
//					nonTranslatedResource.key = componentInfo.NonTranslatedResKey;
//					nonTranslatedResource.@namespace = componentInfo.NonTranslatedRes;
//					nonTranslatedResource.path = componentInfo.NonTranslatedPath;
//					xmlCompInfo.resources.nontranslatedresource = nonTranslatedResource;
//				}
//				if (!String.IsNullOrEmpty(componentInfo.TranslatedResKey))
//				{
//					XmlStub.ResourceType translatedResource = new XmlStub.ResourceType();
//					translatedResource.key = componentInfo.TranslatedResKey;
//					translatedResource.@namespace = componentInfo.TranslatedRes;
//					xmlCompInfo.resources.translatedresource = translatedResource;
//				}

				// We save the assemblies of this component in a hash map
				Dictionary<string, XmlStub.AssemblyType> assemblyTagList = new Dictionary<string, XmlStub.AssemblyType>();
				Dictionary<string, List<XmlStub.ServiceType>> serviceTagLists = new Dictionary<string, List<XmlStub.ServiceType>>();

				// Get assembly information from component metadata
				List<Assembly> tempAllAssembliesList = new List<Assembly>();
				if (componentInfo.MainAssembly != null)
				{
					tempAllAssembliesList.Add(componentInfo.MainAssembly);
				}
				if ((componentInfo.PartAssemblies != null) && componentInfo.PartAssemblies.Any())
				{
					tempAllAssembliesList.AddRange(componentInfo.PartAssemblies);
				}

				for (int i = 0; i < tempAllAssembliesList.Count; i++)
				{
					Assembly servAssembly = tempAllAssembliesList[i];
					string hashkey = servAssembly.FullName;
					if (!assemblyTagList.ContainsKey(hashkey))
					{
						XmlStub.AssemblyType xmlAssembly = new XmlStub.AssemblyType();
						xmlAssembly.file = Path.GetFileName(servAssembly.Location);
						xmlAssembly.ismain = (componentInfo.MainAssembly.Location == servAssembly.Location);
						xmlAssembly.ismainSpecified = true;
						assemblyTagList.Add(hashkey, xmlAssembly);
						serviceTagLists.Add(hashkey, new List<XmlStub.ServiceType>());
					}
				}

				// Run through all passed services
				foreach (var servInfo in serviceInfos)
				{
					if (servInfo.ParentComponent == componentInfo.Name)
					{
						// Create a new assembly tag, if not already done
						Assembly servAssembly = servInfo.ServiceType.Assembly;
						string hashkey = servAssembly.FullName;

						// Create the service structure
						XmlStub.ServiceType xmlService = new XmlStub.ServiceType();
						xmlService.name = servInfo.ServiceInterface.FullName;
						xmlService.@class = servInfo.ServiceType.FullName;
						// TODO How to handle this in future?
//						xmlService.category = this.GetXmlFromHostMode(servInfo.ServiceCategory);
//						xmlService.categorySpecified = true;
						// TODO How to persist single instance status in future?
//						xmlService.singleinstance = servInfo.SingleInstance;
//						xmlService.singleinstanceSpecified = true;

						// Invoke and Quit methods
						// TODO How to handle this in future?
//						if (servInfo.IsInvokable)
//						{
//							if (servInfo.InvokeMethod != null)
//							{
//								xmlService.invokemethod = servInfo.InvokeMethod.Name;
//							}
//							if (servInfo.QuitMethod != null)
//							{
//								xmlService.quitmethod = servInfo.QuitMethod.Name;
//							}
//						}

						// Handle substitutions and service attachments
						if (servInfo.Substitutes != null)
						{
							xmlService.substitutions = servInfo.Substitutes
								.Select<Type, string>(t => t.FullName + ", " + t.Assembly.GetName().Name)
								.ToArray<string>();
						}
						xmlService.attachedto = servInfo.MemberOfLists.ToArray();

						// Handle requirements
						if ((servInfo.Requires != null) && servInfo.Requires.Any())
						{
							xmlService.requirements = servInfo.Requires.Select(
								req =>
								new awzcore.Serialization.XmlStub.RequirementType() {
									name = req.ComponentName,
									@operator = this.GetXmlFromCondition(req.Condition),
									operatorSpecified = true,
									version = req.Version.ToString()
								}
							).ToArray();
						}

						// Handle auto-injections settings
						if ((servInfo.AutoInjections != null) && servInfo.AutoInjections.Any())
						{
							XmlStub.ServiceTypeAutoinjection xmlAutoInjectionRoot = new XmlStub.ServiceTypeAutoinjection();
							xmlAutoInjectionRoot.Items = new XmlStub.AutoInjectionType[servInfo.AutoInjections.Count()];
							xmlAutoInjectionRoot.ItemsElementName = new XmlStub.ItemsChoiceType[servInfo.AutoInjections.Count()];
							int i = 0;
							foreach (var autoInjection in servInfo.AutoInjections)
							{
								XmlStub.AutoInjectionType xmlAutoInjection = new XmlStub.AutoInjectionType();
								if (autoInjection.InjectFromAttachList)
								{
									xmlAutoInjection.name = autoInjection.BoundAttachList;
								}
								else
								{
									xmlAutoInjection.name = autoInjection.BoundService.FullName;
								}
								xmlAutoInjection.overridableSpecified = true;
								xmlAutoInjection.overridable = autoInjection.Overridable;
								xmlAutoInjection.asdependentSpecified = true;
								xmlAutoInjection.asdependent = autoInjection.InjectAsDependent;
								xmlAutoInjection.type = autoInjection.Interface.FullName;

								xmlAutoInjectionRoot.ItemsElementName[i] =
									autoInjection.InjectFromAttachList ? XmlStub.ItemsChoiceType.list : XmlStub.ItemsChoiceType.service;
								xmlAutoInjectionRoot.Items[i] = xmlAutoInjection;

								i++;
							}

							xmlService.autoinjection = xmlAutoInjectionRoot;
						}

						// Now save the XML object instance
						serviceTagLists[hashkey].Add(xmlService);
					}
				}

				// Add assemblies and services to component XML
				xmlCompInfo.assemblies = assemblyTagList.Select(
					kv =>
					{
						kv.Value.services = serviceTagLists[kv.Key].ToArray();
						return kv.Value;
					}
				).ToArray();

				// Now write the XML structure to the stream
				XmlSerializer serializer = new XmlSerializer(typeof(XmlStub.DefinitionRootType));
				serializer.Serialize(destStream, xmlCompInfo);

				return true;
			}
			catch (Exception)
			{
				// Writing XML data was not successful
				return false;
			}
		}

		private Condition GetConditionFromXml(XmlStub.VersionContraintOperatorType operatorType)
		{
			switch (operatorType)
			{
				case XmlStub.VersionContraintOperatorType.equal:
					return Condition.Equal;
				case XmlStub.VersionContraintOperatorType.greater:
					return Condition.Greater;
				case XmlStub.VersionContraintOperatorType.lower:
					return Condition.Lower;
				case XmlStub.VersionContraintOperatorType.greaterorequal:
					return Condition.Greater | Condition.Equal;
				case XmlStub.VersionContraintOperatorType.lowerorequal:
					return Condition.Lower | Condition.Equal;
				default:
					return Condition.Equal;
			}
		}

		private XmlStub.VersionContraintOperatorType GetXmlFromCondition(Condition condition)
		{
			switch (condition)
			{
				case Condition.Equal:
					return XmlStub.VersionContraintOperatorType.equal;
				case Condition.Greater:
					return XmlStub.VersionContraintOperatorType.greater;
				case Condition.Lower:
					return XmlStub.VersionContraintOperatorType.lower;
				case Condition.Equal | Condition.Greater:
					return XmlStub.VersionContraintOperatorType.greaterorequal;
				case Condition.Equal | Condition.Lower:
					return XmlStub.VersionContraintOperatorType.lowerorequal;
				default:
					return XmlStub.VersionContraintOperatorType.equal;
			}
		}

		private HostMode GetHostModeFromXml(XmlStub.HostModeType hostModeType)
		{
			switch (hostModeType)
			{
				case awzcore.Serialization.XmlStub.HostModeType.general:
					return HostMode.General;
				case awzcore.Serialization.XmlStub.HostModeType.gui:
					return HostMode.GUI;
				case awzcore.Serialization.XmlStub.HostModeType.console:
					return HostMode.Console;
				case awzcore.Serialization.XmlStub.HostModeType.winservice:
					return HostMode.WindowsService;
				case awzcore.Serialization.XmlStub.HostModeType.webserver:
					return HostMode.WebServer;
				case awzcore.Serialization.XmlStub.HostModeType.browser:
					return HostMode.Browser;
				default:
					return HostMode.General;
			}
		}

		private XmlStub.HostModeType GetXmlFromHostMode(HostMode hostMode)
		{
			switch (hostMode)
			{
				case HostMode.General:
					return awzcore.Serialization.XmlStub.HostModeType.general;
				case HostMode.GUI:
					return awzcore.Serialization.XmlStub.HostModeType.gui;
				case HostMode.Console:
					return awzcore.Serialization.XmlStub.HostModeType.console;
				case HostMode.WindowsService:
					return awzcore.Serialization.XmlStub.HostModeType.winservice;
				case HostMode.WebServer:
					return awzcore.Serialization.XmlStub.HostModeType.webserver;
				case HostMode.Browser:
					return awzcore.Serialization.XmlStub.HostModeType.browser;
				default:
					return awzcore.Serialization.XmlStub.HostModeType.general;
			}
		}
	}
}
