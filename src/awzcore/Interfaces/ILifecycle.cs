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

namespace awzcore.Interfaces
{
	/// <summary>
	/// Interface for controllers of a service's lifecycle.
	/// </summary>
	public interface ILifecycle
	{
		/// <summary>
		/// Initializes the lifecycle.
		/// </summary>
		/// <param name="data">Initialization parameters for the service.</param>
		/// <param name="logger">Logger instance.</param>
		void Prepare(InitializationData data, ILogger logger);

		/// <summary>
		/// Called directly after service manager has loaded all meta-information.
		/// </summary>
		void InitialActivation();

		/// <summary>
		/// Retrieves a new instance of the given service.
		/// </summary>
		/// <param name="data">Initialization parameters.</param>
		object Create(InitializationData data);
		
		/// <summary>
		/// Destroys the object managed by this lifecycle.
		/// </summary>
		void Destroy();
	}
}

