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
	/// Interface for bootstrap services generating <see cref="IServiceInfo"/> instances.
	/// </summary>
	public interface IServiceInfoFactory
	{
		IServiceInfo Create();
	}

	/// <summary>
	/// Interface for a class that saves information about a registered service.
	/// </summary>
	public interface IServiceInfo
	{
		/// <summary>
		/// Returns the interface registered for this service.
		/// </summary>
		System.Type ServiceInterface
		{
			get;
		}

		/// <summary>
		/// Returns the lifecycle implementation defined for this service.
		/// </summary>
		/// <value>The lifecycle.</value>
		ILifecycle Lifecycle
		{
			get;
		}

		/// <summary>
		/// Returns the instance initializer implementation defined for this service.
		/// </summary>
		/// <value>The initializer.</value>
		IInitializer Initializer
		{
			get;
		}

		/// <summary>
		/// Returns the internal name of the component that has registered the service.
		/// </summary>
		string ParentComponent
		{
			get;
		}

		/// <summary>
		/// Returns a list containing the registered interfaces of all services
		/// that have been substituted by this service.
		/// </summary>
		IEnumerable<Type> Substitutes
		{
			get;
		}

		/// <summary>
		/// Returns the <see cref="System.Type">Type</see> object for the component's internal implementation
		/// class for this service.
		/// </summary>
		System.Type ServiceType
		{
			get;
		}

		/// <summary>
		/// Retrieves an array of all service lists this service has been added to on registration.
		/// </summary>
		IEnumerable<string> MemberOfLists
		{
			get;
		}

		/// <summary>
		/// Returns all loading requirements for the service represented by an array
		/// of <see cref="IComponentRequirement"/>
		/// objects.
		/// </summary>
		IEnumerable<IComponentRequirement> Requires
		{
			get;
		}

		/// <summary>
		/// Returns an array of <see cref="System.Type">Type</see> objects for all
		/// service interfaces that the service implements.
		/// </summary>
		IEnumerable<Type> InterfaceTypes
		{
			get;
		}

		/// <summary>
		/// Returns information about services or AttachLists that have to be auto-injected
		/// into instances of this service.
		/// </summary>
		IEnumerable<IAutoInjection> AutoInjections
		{
			get;
		}
	}
}
