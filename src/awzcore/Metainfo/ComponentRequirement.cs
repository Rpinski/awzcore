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
using System.Linq;
using System.Text;
using awzcore.Interfaces;

namespace awzcore.Metainfo
{
	public class ComponentRequirement : IComponentRequirement
	{
		#region IComponentRequirement Member

		public string ComponentName
		{
			get;
			set;
		}

		public Version Version
		{
			get;
			set;
		}

		public Condition Condition
		{
			get;
			set;
		}

		#endregion
	}
}