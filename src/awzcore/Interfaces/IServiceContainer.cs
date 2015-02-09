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
	/// Represents a class containing services.
	/// </summary>
	public interface IServiceContainer
	{
		/// <summary>
		/// Retrieves a service instance.
		/// </summary>
		/// <param name="passedInstances">Instances to be passed to created instance.</param>
		/// <typeparam name="T">Type of the retrieved service instance.</typeparam>
		/// <returns>Instance of the service object, typed as set by the <typeparamref name="T">T</typeparamref> parameter.</returns>
		/// <remarks>
		/// <c>Get</c> resolves any service substitutes. So if the requested
		/// service has been substituted by another service, this method will return an instance of that substituting service.
		/// </remarks>
		T Get<T>(params object[] passedInstances) where T : class;
	}
}

