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
using System.Reflection;

namespace awzcore.Interfaces
{
	/// <summary>
	/// Interface for bootstrap services collecting meta-information about available services and components.
	/// </summary>
	public interface IRegistration
	{
		/// <summary>
		/// Loads all components and services and collects all metadata.
		/// </summary>
		/// <param name="assemblies">List of assemblies to find/use services from.</param>
		void LoadComponentData(IEnumerable<Assembly> assemblies);

		/// <summary>
		/// Gets the collected component infos by component name.
		/// </summary>
		/// <value>The component infos.</value>
		IDictionary<string, IComponentInfo> ComponentInfos
		{
			get;
		}

		/// <summary>
		/// Gets the collected service infos by service type.
		/// </summary>
		/// <value>The service infos.</value>
		IDictionary<Type, IServiceInfo> ServiceInfos
		{
			get;
		}

		/// <summary>
		/// Gets the substitutions as type-to-type mapping
		/// </summary>
		/// <value>The substitutions.</value>
		IDictionary<Type, Type> Substitutions
		{
			get;
		}

		/// <summary>
		/// Gets the service lists as name-to-type-list mapping.
		/// </summary>
		/// <value>The service lists.</value>
		IDictionary<object, IList<Type>> ServiceLists
		{
			get;
		}
	}
}

