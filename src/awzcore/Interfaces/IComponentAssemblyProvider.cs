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

using System.Collections.Generic;
using System.Reflection;
using System;

namespace awzcore.Interfaces
{
	/// <summary>
	/// Interface for bootstrap services defining the set of assemblies to use services from.
	/// </summary>
	public interface IComponentAssemblyProvider
	{
		/// <summary>
		/// Retrieves or loads assemblies containing services and components.
		/// </summary>
		/// <returns>Available assemblies to use.</returns>
		IEnumerable<Assembly> GetAssemblies();

		/// <summary>
		/// Returns the path where component assemblies can be found.
		/// </summary>
		/// <returns>The path where component assemblies can be found.</returns>
		string GetComponentPath();
	}

	/// <summary>
	/// Dummy implementation for <see cref="IComponentAssemblyProvider"/> interface for architectures where
	/// dynamic loading of types or assemblies is not necessary.
	/// </summary>
	public class DummyComponentAssemblyProvider : IComponentAssemblyProvider
	{
		static readonly Lazy<ILogger> _lazyInstance = new Lazy<ILogger>(() => new DummyLogger());

		public static ILogger Instance
		{
			get
			{
				return _lazyInstance.Value;
			}
		}

		#region IComponentAssemblyProvider implementation

		public IEnumerable<Assembly> GetAssemblies()
		{
			return new Assembly[0];
		}

		public string GetComponentPath()
		{
			return String.Empty;
		}

		#endregion
	}
}
