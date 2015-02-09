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
using System.Linq;
using awzcore.Metainfo;
using System.Reflection;
using awzcore.Interfaces;

namespace awzcore.Registration
{
	internal class AttributeComponentInfo : ComponentInfo
	{
		public AttributeComponentInfo(Assembly assembly)
		{
			ComponentDefinitionAttribute cda = (ComponentDefinitionAttribute) Attribute.GetCustomAttribute(assembly, typeof(ComponentDefinitionAttribute));
			if (cda == null)
			{
				ComponentPartAttribute cdp = (ComponentPartAttribute) Attribute.GetCustomAttribute(assembly, typeof(ComponentPartAttribute));
				if (cdp != null)
				{
					// Retrieve name of real component (this is only a part assembly of it)
					Name = cdp.Name;
					IsComponentPart = true;
				}
				else
				{
					// Somebody has tried to get info from an assembly which is not an NQ component
					throw new ComponentDefinitionException(ErrorReason.NonNQComponent, "Can't get component information from a non-NQ-component.");
				}
			}
			else
			{
				MainAssembly = assembly;
				AssemblyCopyrightAttribute acopy = (AssemblyCopyrightAttribute) Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));
				AssemblyDescriptionAttribute adesc = (AssemblyDescriptionAttribute) Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute));
				// Retrieve data from assembly
				Name = cda.Name;
				DisplayName = cda.DisplayName;
//				UpdaterURL = cda.UpdaterURL;
				Category = cda.Category;
				NoAutoLoad = cda.NoAutoLoad;
				Copyright = acopy.Copyright;
				Version = new AssemblyName(assembly.FullName).Version;
				if (adesc != null)
				{
					if (!String.IsNullOrEmpty(adesc.Description))
					{
						DisplayVersion = adesc.Description.Substring(1);
					}
					else
					{
						DisplayName = Version.ToString();
					}
				}
				else
				{
					DisplayVersion = Version.ToString();
				}

				// Check for resource definition
//				ResourceOwnerAttribute roa = (ResourceOwnerAttribute) Attribute.GetCustomAttribute(assembly, typeof(ResourceOwnerAttribute));
//				if (roa != null)
//				{
//					NonTranslatedResKey = roa.NonTranslatedKey;
//					NonTranslatedRes = roa.NonTranslatedName;
//					NonTranslatedPath = roa.NonTranslatedPath;
//					TranslatedResKey = roa.TranslatedKey;
//					TranslatedRes = roa.TranslatedName;
//				}

				// Check for component requirements
				ComponentRequirementAttribute[] rca = (ComponentRequirementAttribute[]) Attribute.GetCustomAttributes(assembly, typeof(ComponentRequirementAttribute));
				if (rca != null)
				{
					Requires = rca.Select(a => a.ComponentRequirement);
				}

				// Check for component compatibility
				ComponentCompatibilityAttribute[] cca = (ComponentCompatibilityAttribute[]) Attribute.GetCustomAttributes(assembly, typeof(ComponentCompatibilityAttribute));
				if (cca != null)
				{
					Compatibility = cca.Select(a => a.ComponentRequirement);
				}
			}
		}

		public static bool IsComponent(Assembly assembly)
		{
			ComponentDefinitionAttribute cda = (ComponentDefinitionAttribute) Attribute.GetCustomAttribute(assembly, typeof(ComponentDefinitionAttribute));
			if (cda == null)
			{
				// If there's no component definition, maybe it's a part of another component?
				ComponentPartAttribute cdp = (ComponentPartAttribute) Attribute.GetCustomAttribute(assembly, typeof(ComponentPartAttribute));
				return (cdp != null);
			}
			else
			{
				return true;
			}
		}
	}
}

