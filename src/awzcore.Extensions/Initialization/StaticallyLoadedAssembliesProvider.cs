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

namespace awzcore.Initialization
{
	/// <summary>
	/// Simple assembly provider just retrieving list of assemblies that are already loaded into process.
	/// </summary>
	public class StaticallyLoadedAssembliesProvider : IComponentAssemblyProvider
	{
		#region IComponentAssemblyProvider implementation

		public System.Collections.Generic.IEnumerable<System.Reflection.Assembly> GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		public string GetComponentPath()
		{
			return Environment.CurrentDirectory;
		}

		#endregion
	}
}

