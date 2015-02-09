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

namespace awzcore.Interfaces
{
	/// <summary>
	/// Interface for bootstrap services generating <see cref="IAutoInjection"/> instances.
	/// </summary>
	public interface IAutoInjectionFactory
	{
		IAutoInjection Create();
	}

	/// <summary>
	/// Interface for objects that represent an automatic service injection definition.
	/// </summary>
	public interface IAutoInjection
	{
		/// <summary>
		/// Returns the name of the AttachList to be injected.
		/// </summary>
		string BoundAttachList
		{
			get;
		}

		/// <summary>
		/// Returns the registered interface of the service to be injected.
		/// </summary>
		System.Type BoundService
		{
			get;
		}

		/// <summary>
		/// Returns the type of the used interface of the service to be injected.
		/// </summary>
		/// <remarks>
		/// The type given here should not be the registered interface for the service,
		/// but any other (more abstract) interface, that the service implements.
		/// </remarks>
		Type Interface
		{
			get;
		}

		/// <summary>
		/// Specifies whether the injected service/services instances should be created as 
		/// dependent services, i.e. the current instance should be passed to them.
		/// </summary>
		bool InjectAsDependent
		{
			get;
		}

		/// <summary>
		/// Returns whether <see cref="awzcore.NQAutoInjectionAttribute.BoundName">BoundName</see>
		/// specifies an AttachList.
		/// </summary>
		bool InjectFromAttachList
		{
			get;
		}

		/// <summary>
		/// Specifies whether this injection should be overridden by the instance passed through
		/// <see cref="awzcore.INQServiceManager.CreateDependentService#T">CreateDependentService</see>
		/// method and its variations.
		/// </summary>
		bool Overridable
		{
			get;
		}
	}
}
