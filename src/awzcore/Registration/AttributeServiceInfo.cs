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
// 	Copyright (C) 2014 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using awzcore.Metainfo;
using awzcore.Interfaces;
using System.Reflection;
using System.Linq;
using awzcore.Lifecycle;

namespace awzcore.Registration
{
	internal class AttributeServiceInfo : ServiceInfo
	{
		internal AttributeServiceInfo(Type serviceType, string compName)
		{
			List<Type> nqInterfaces = new List<Type>();

			this.ParentComponent = compName;
			this.ServiceType = serviceType;
			ExportedServiceAttribute exportedServiceAttr =
				(ExportedServiceAttribute) Attribute.GetCustomAttribute(serviceType, typeof(ExportedServiceAttribute));
			ServiceSubstituteAttribute serviceSubstAttr =
				(ServiceSubstituteAttribute) Attribute.GetCustomAttribute(serviceType, typeof(ServiceSubstituteAttribute));
			ServiceAttachmentAttribute serviceAttachmentAttr =
				(ServiceAttachmentAttribute) Attribute.GetCustomAttribute(serviceType, typeof(ServiceAttachmentAttribute));
			ComponentRequirementAttribute[] compRequirementAttr =
				(ComponentRequirementAttribute[]) Attribute.GetCustomAttributes(serviceType, typeof(ComponentRequirementAttribute));
			AutoInjectionAttribute[] autoInjectionAttr = (AutoInjectionAttribute[]) Attribute.GetCustomAttributes(serviceType, typeof(AutoInjectionAttribute));
			if (exportedServiceAttr != null)
			{
				// Save component information
				this.Lifecycle = exportedServiceAttr.SingleInstance ? (ILifecycle) new SingletonLifecycle() : (ILifecycle) new MultiLifecycle();
				// TODO Set appropriate initializer
				if (serviceSubstAttr != null)
				{
					this.Substitutes = serviceSubstAttr.ServiceList;
				}
				if (serviceAttachmentAttr != null)
				{
					this.MemberOfLists = serviceAttachmentAttr.ServiceList;
				}
				if (compRequirementAttr != null)
				{
					Requires = compRequirementAttr.Select(a => a.ComponentRequirement);
				}

				// Get the registered interface for the service
				Type[] types = serviceType.GetInterfaces();
				if ((types != null) && (types.Length > 0))
				{
					// Take the first interface, that is marked with NQServiceInterfaceAttribute
					foreach (Type interfaceType in serviceType.GetInterfaces())
					{
						ServiceInterfaceAttribute serviceInterfaceAttr =
							(ServiceInterfaceAttribute) Attribute.GetCustomAttribute(interfaceType, typeof(ServiceInterfaceAttribute));
						if (serviceInterfaceAttr != null)
						{
							this.ServiceInterface = interfaceType;
							break;
						}
					}
				}

				if (this.ServiceInterface == null)
				{
					// We use the type implementing the service as its own service interface
					this.ServiceInterface = serviceType;
				}

				// List all interfaces that are implemented by the registered service interface
				foreach (Type interfaceType in this.ServiceInterface.GetInterfaces())
				{
					//NQServiceInterfaceAttribute typesia = (NQServiceInterfaceAttribute) Attribute.GetCustomAttribute(interfaceType, typeof(NQServiceInterfaceAttribute));
					//if (typesia != null)
					//{
					nqInterfaces.Add(interfaceType);
					//}
				}
				this.InterfaceTypes = nqInterfaces;

				// Look for invokable service methods
				// TODO How to handle this in future?
//				MethodInfo[] methods = serviceType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
//				if (methods != null)
//				{
//					for (int i = 0; i < methods.Length; i++)
//					{
//						ServiceInvokeMethodAttribute invokeMethodAttr = (ServiceInvokeMethodAttribute) Attribute.GetCustomAttribute(methods[i], typeof(ServiceInvokeMethodAttribute));
//						ServiceQuitMethodAttribute quitMethodAttr = (ServiceQuitMethodAttribute) Attribute.GetCustomAttribute(methods[i], typeof(ServiceQuitMethodAttribute));
//
//						if (invokeMethodAttr != null)
//						{
//							// We have found an InvokeMethod
//							this.InvokeMethod = methods[i];
//							this.IsInvokable = true;
//						}
//						if (quitMethodAttr != null)
//						{
//							// We have found a QuitMethod
//							this.QuitMethod = methods[i];
//							this.IsInvokable = true;
//						}
//					}
//				}

				// Now check for auto-injection entries
				if (autoInjectionAttr != null)
				{
					this.AutoInjections = autoInjectionAttr.Select(
						a => new AutoInjection() {
							BoundService = a.BoundService,
							BoundAttachList = a.BoundAttachList,
							Interface = a.ServiceInterface,
							InjectAsDependent = a.InjectAsDependent,
							InjectFromAttachList = a.InjectFromAttachList,
							Overridable = a.Overridable
						});
				}
			}
			else
			{
				// Erroneous service definition
				throw new ServiceDefinitionException(ErrorReason.ServiceNameUnspecified, "Service name unspecified");
			}
		}

		public static bool IsValidServiceClass(Type serviceType)
		{
			ExportedServiceAttribute esa = (ExportedServiceAttribute) Attribute.GetCustomAttribute(serviceType, typeof(ExportedServiceAttribute));
			return (esa != null);
		}
	}
}

