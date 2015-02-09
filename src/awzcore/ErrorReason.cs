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

namespace awzcore
{
	/// <summary>
	/// Determines the reason of an exception thrown by the Service Manager.
	/// </summary>
	public enum ErrorReason
	{
		/// <summary>
		/// Unspecified reason.
		/// </summary>
		UnknownReason,
		/// <summary>
		/// No correct service name has been specified for a service.
		/// </summary>
		ServiceNameUnspecified,
		/// <summary>
		/// The interfaces implemented by a substitutor service don't match
		/// the interfaces implemented by the substituted service.
		/// </summary>
		SubstitutorInterfaceMismatch,
		/// <summary>
		/// Attempt to define more than one service with the same name.
		/// </summary>
		ServiceMultiplyDefined,
		/// <summary>
		/// No correct name has been specified for a component.
		/// </summary>
		ComponentNameUnspecified,
		/// <summary>
		/// Attempt to define more than one component with the same name.
		/// </summary>
		ComponentMultiplyDefined,
		/// <summary>
		/// No real NQ component.
		/// </summary>
		NonNQComponent,
		/// <summary>
		/// Can't access a single-instance service while it is in initialization state.
		/// </summary>
		ServiceInitLock,
		/// <summary>
		/// Service name is unknown.
		/// </summary>
		UnknownService,
		/// <summary>
		/// Attempt to load more than one instance of a single-instance service.
		/// </summary>
		ServiceMultiplyLoaded,
		/// <summary>
		/// The service definition needs a specific constructor definition in the
		/// implementing class, that could not be found.
		/// </summary>
		NoConstructor,
		/// <summary>
		/// The given type is not implemented by requested service.
		/// </summary>
		UnsupportedInterfaceType,
		/// <summary>
		/// A service 1 substitutes service 2, that substitutes service 1 etc.
		/// </summary>
		CircularSubstitution,
		/// <summary>
		/// A service 2 is injected into service 1, that is again injected into service 2 etc.
		/// </summary>
		CircularAutoInjection,
		/// <summary>
		/// A service name in an auto-injection definition is unknown.
		/// </summary>
		UnresolvedAutoInjection
	}
}
