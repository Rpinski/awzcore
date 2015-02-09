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
	/// Interface for containers managing service instances and holding metainfo.
	/// </summary>
	public interface IMetaInfoContainer
	{
		IDictionary<string, IComponentInfo> ComponentInfos
		{
			get;
			set;
		}

		IDictionary<Type, IServiceInfo> ServiceInfos
		{
			get;
			set;
		}

		IDictionary<Type, Type> Substitutions
		{
			get;
			set;
		}

		IDictionary<object, IList<Type>> ServiceLists
		{
			get;
			set;
		}
	}
}

