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
using awzcore.Interfaces;

namespace awzcore.Metainfo
{
	public class ComponentInfo : IComponentInfo
	{
		#region IComponentInfo Member

		public string Name
		{
			get;
			set;
		}

		public string DisplayName
		{
			get;
			set;
		}

		public string Copyright
		{
			get;
			set;
		}

		public bool IsComponentPart
		{
			get;
			set;
		}

		public bool NoAutoLoad
		{
			get;
			set;
		}

		public IEnumerable<IComponentRequirement> Requires
		{
			get;
			set;
		}

		public IEnumerable<IComponentRequirement> Compatibility
		{
			get;
			set;
		}

		public System.Reflection.Assembly MainAssembly
		{
			get;
			set;
		}

		public IEnumerable<System.Reflection.Assembly> PartAssemblies
		{
			get;
			set;
		}

		public string MainAssemblyFullName
		{
			get;
			set;
		}

		public string[] PartAssembliesFullNames
		{
			get;
			set;
		}

		public Version Version
		{
			get;
			set;
		}

		public string DisplayVersion
		{
			get;
			set;
		}

		public HostMode Category
		{
			get;
			set;
		}

		#endregion
	}
}