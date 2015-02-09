// 	------------------------------------------------------------------------
/// 	 awzcore Library//
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

namespace awzcore.Lifecycle
{
	/// <summary>
	/// Lifecycle saving an already created service instance and always retrieving it.
	/// </summary>
	public class PreCreatedSingletonLifecycle : ILifecycle
	{
		object _instance;

		public PreCreatedSingletonLifecycle(object instance)
		{
			_instance = instance;
		}

		#region ILifecycle implementation

		public void Prepare(InitializationData data, ILogger logger)
		{
			// No-op
		}

		public void InitialActivation()
		{
			// No-op
		}

		public object Create(InitializationData data)
		{
			return _instance;
		}

		#endregion
	}
}

