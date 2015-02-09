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
using System.IO;
using System.Reflection;
using System.Linq;
using System.Globalization;
using System.Collections;

namespace awzcore.Initialization
{
	/// <summary>
	/// Interface for assembly providers loading all assemblies available in a specified directory.
	/// </summary>
	public class AssembliesFromDirectoryProvider : IComponentAssemblyProvider
	{
		ICallParameterHandler _callParameterHandler;
		string _componentPath;

		List<string> _loadedAssemblyFiles = new List<string>();

		public AssembliesFromDirectoryProvider(ICallParameterHandler callParameterHandler)
		{
			_callParameterHandler = callParameterHandler;
		}

		#region IComponentAssemblyProvider implementation

		public System.Collections.Generic.IEnumerable<System.Reflection.Assembly> GetAssemblies()
		{
			List<string> forbidden = new List<string>();

			// Add forbidden assembly names to a searchable list
			if (_callParameterHandler["cf"] != null)
			{
				foreach (string filename in _callParameterHandler["cf"])
				{
					forbidden.Add(filename.ToLower());
				}
			}

			_componentPath = Environment.CurrentDirectory;

			// Iterate through all component assemblies in the specified directory
			string[] assemblies = _callParameterHandler["ca"];
			if (assemblies == null)
			{
				// Identify all DLLs as component assemblies, whose names end with .Component.dll or .Part.dll
				List<string> componentAssemblies = new List<string>();
				componentAssemblies.AddRange(Directory.GetFiles(_componentPath, "*.Component.dll", SearchOption.TopDirectoryOnly));
				componentAssemblies.AddRange(Directory.GetFiles(_componentPath, "*.Part.dll", SearchOption.TopDirectoryOnly));
				assemblies = componentAssemblies.ToArray();
			}

			return LoadComponentAssemblies(assemblies, forbidden.ToArray());
		}

		public string GetComponentPath()
		{
			// TODO Return real path here
			return Environment.CurrentDirectory;
		}

		#endregion

		/// <summary>
		/// Load assemblies.
		/// </summary>
		/// <param name = "assemblyFiles">List of assembly file paths.</param>
		/// <param name = "forbiddenAssemblyFiles">List of file paths of assemblies not allowed to be loaded.</param>
		/// <returns>List of loaded assembly instances.</returns>
		IEnumerable<Assembly> LoadComponentAssemblies(IEnumerable<string> assemblyFiles, IEnumerable<string> forbiddenAssemblyFiles)
		{
			var loadedAssemblies = new List<Assembly>();
			var nameComparer = new AssemblyFileNameComparer();
			foreach (string dllFile in assemblyFiles.Except(forbiddenAssemblyFiles, nameComparer))
			{
				string assemblyFile = Path.Combine(_componentPath, Path.GetFileName(dllFile));
				// Try to load the DLL as a .NET assembly
				try
				{
					AssemblyName asmName = AssemblyName.GetAssemblyName(assemblyFile);
					if (!_loadedAssemblyFiles.Contains(asmName.Name))
					{
						loadedAssemblies.Add(AppDomain.CurrentDomain.Load(asmName));
						_loadedAssemblyFiles.Add(assemblyFile);
					}
				}
				catch (BadImageFormatException)
				{
					// Do nothing as it might be possible that the DLL is not a .NET assembly
				}
			}

			return loadedAssemblies;
		}

		class AssemblyFileNameComparer : IEqualityComparer<string>
		{
			public bool Equals(string x, string y)
			{
				return Path.GetFileName(x).Equals(Path.GetFileName(y), StringComparison.InvariantCultureIgnoreCase);
			}

			public int GetHashCode(string obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}

