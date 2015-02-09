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


namespace awzcore.Interfaces
{
	/// <summary>
	/// Interface for bootstrap services generating <see cref="IComponentInfo"/> instances.
	/// </summary>
	public interface IComponentInfoFactory
	{
		IComponentInfo Create();
	}

	/// <summary>
	/// Interface for a class that saves information about a component.
	/// </summary>
	public interface IComponentInfo
	{
		/// <summary>
		/// Returns the internal name of the component.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Returns the display name of the component.
		/// </summary>
		string DisplayName
		{
			get;
		}

		/// <summary>
		/// Returns the copyright information.
		/// </summary>
		string Copyright
		{
			get;
		}

		/// <summary>
		/// Indicates if an assembly is only a part of a component. This is used internally
		/// and will usually always return <c>true</c>.
		/// </summary>
		bool IsComponentPart
		{
			get;
		}

		/// <summary>
		/// If <c>True</c> the component is not loaded automatically when it's present in the bin directory.
		/// It can be loaded explicitly by setting the <c>-ca</c> command-line parameter.
		/// </summary>
		bool NoAutoLoad
		{
			get;
		}

		/// <summary>
		/// Returns all loading requirements for the component represented by an array
		/// of <see cref="awzcore.IComponentRequirement"/> objects.
		/// </summary>
		IEnumerable<IComponentRequirement> Requires
		{
			get;
		}

		/// <summary>
		/// Returns the version constraints that indicate which version the component is
		/// also compatible to. If the required version of a component is not found this
		/// is used to identify a compatible version.
		/// </summary>
		IEnumerable<IComponentRequirement> Compatibility
		{
			get;
		}

		/// <summary>
		/// Returns the <see cref="System.Reflection.Assembly">Assembly</see> object for the
		/// main assembly of the component.
		/// </summary>
		System.Reflection.Assembly MainAssembly
		{
			get;
		}

		/// <summary>
		/// Returns the <see cref="System.Reflection.Assembly">Assembly</see> objects of
		/// all component part assemblies for the current component.
		/// </summary>
		IEnumerable<System.Reflection.Assembly> PartAssemblies
		{
			get;
		}

		/// <summary>
		/// Returns the component's version.
		/// </summary>
		System.Version Version
		{
			get;
		}

		/// <summary>
		/// Returns the displayable version of the component as a string.
		/// </summary>
		string DisplayVersion
		{
			get;
		}

		/// <summary>
		/// Retrieves the category assigned to this service.
		/// The category sets in which environment this component should be loaded.
		/// </summary>
		HostMode Category
		{
			get;
		}

	}
}
