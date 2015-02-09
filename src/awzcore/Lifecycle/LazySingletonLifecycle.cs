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
using System.Collections.Generic;

namespace awzcore.Lifecycle
{
	/// <summary>
	/// Lifecycle lazily (i.e. on first access) creating a singleton instance of a service
	/// and from then on always returning that one instance.
	/// </summary>
	public class LazySingletonLifecycle : ILifecycle
	{
		InitializationData _initializationData;
		object _instance;

		#region ILifecycle implementation

		public void Prepare(InitializationData data, ILogger logger)
		{
			if (data == null)
				throw new ArgumentNullException("data");
			if (data.ServiceInfo == null)
				throw new ArgumentException("No valid ServiceInfo in InitializationData");
			if (data.ServiceInfo.Initializer == null)
				throw new ArgumentException("No valid initializer in InitializationData");

			_initializationData = data;
		}

		public void InitialActivation()
		{
		}

		public object Create(InitializationData data)
		{
			// Always retrieve the same singleton instance, once we have created it
			if (_instance == null)
			{
				// Lazy initialization of singleton service instance
				_instance = _initializationData.ServiceInfo.Initializer.Create(_initializationData);
			}
			return _instance;
		}

		#endregion
	}
}

