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
using awzcore.Interfaces;
using System.Threading.Tasks;

namespace awzcore
{
	/// <summary>
	/// Helper class to register any class as bootstrap service that doesn't implement
	/// the <see cref="IBootstrapRegistration"/> interface itself.
	/// </summary>
	public static class BootstrapRegistration
	{
		/// <summary>
		/// Creates a new registration wrapper around a bootstrap service instance.
		/// </summary>
		/// <param name="bootstrapService">Bootstrap service to wrap for registration.</param>
		/// <returns>><see cref="IBootstrapRegistration"/> implementation.</returns>
		public static IBootstrapRegistration For<T>(T bootstrapService) where T : class
		{
			return new BootstrapRegistrationImpl<T>(bootstrapService);
		}

		/// <summary>
		/// Helper class to register any class as bootstrap service that doesn't implement
		/// the <see cref="IBootstrapRegistration"/> interface itself.
		/// </summary>
		/// <typeparam name="T">Type to register as bootstrap service.</typeparam>
		private class BootstrapRegistrationImpl<T> : IBootstrapRegistration
			where T : class
		{
			readonly T _bootstrapService;

			internal BootstrapRegistrationImpl(T bootstrapService)
			{
				_bootstrapService = bootstrapService;
			}

			#region IBootstrapRegistration implementation

			public void Register(IBootstrapServiceContainer serviceContainer)
			{
				// Register bootstrap service with given type
				serviceContainer.Register(_bootstrapService);
			}

			#endregion
		}
	}
}

