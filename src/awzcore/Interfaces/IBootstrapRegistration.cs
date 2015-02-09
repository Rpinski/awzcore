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

namespace awzcore.Interfaces
{
	/// <summary>
	/// Allows a bootstrap service implementation to customize its registration process.
	/// </summary>
	public interface IBootstrapRegistration
	{
		/// <summary>
		/// Called when service should register itself in given bootstrap service container.
		/// </summary>
		/// <param name="serviceContainer">Service container.</param>
		void Register(IBootstrapServiceContainer serviceContainer);
	}
}

