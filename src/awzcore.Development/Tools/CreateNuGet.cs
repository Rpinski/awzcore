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
// 	Copyright (C) 2015 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------

using System;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace awzcore.Development.Tools
{
	public class CreateNuGet : ITool
	{
		#region ITool implementation

		public void Execute(CommandLineHelper commandLineHelper)
		{
			string packagesDir = Environment.CurrentDirectory;
			if (commandLineHelper.IsCmdLineParamSet("packagesdir"))
			{
				packagesDir = commandLineHelper["packagesdir"].FirstOrDefault() ?? packagesDir;
			}

			string outputDir = Environment.CurrentDirectory;
			if (commandLineHelper.IsCmdLineParamSet("outputdir"))
			{
				outputDir = commandLineHelper["outputdir"].FirstOrDefault() ?? outputDir;
			}

			var nuspecFiles = commandLineHelper.IsCmdLineParamSet("nuspec") ? commandLineHelper["nuspec"] : new string[0];
			if (!Directory.Exists(packagesDir))
			{
				Console.WriteLine("ERROR: Couldn't find NuGet packages path!");
				return;
			}
			string nuGetCmdLinePackagePath = Directory.EnumerateDirectories(packagesDir, "NuGet.CommandLine.*").FirstOrDefault();
			if (nuGetCmdLinePackagePath == null)
			{
				Console.WriteLine("ERROR: Couldn't find NuGet command line tool!");
				return;
			}

			string nuGetBinPath = Path.Combine(nuGetCmdLinePackagePath, "tools");

			foreach (string nuspecFile in nuspecFiles)
			{
				var processStartInfo = new ProcessStartInfo() {
					FileName = Path.Combine(nuGetBinPath, "NuGet.exe"),
					Arguments = string.Format(
						"pack \"{0}\" -BasePath \"{1}\" -OutputDirectory \"{1}\"",
						nuspecFile,
						outputDir
					),
					CreateNoWindow = true
				};
				Process.Start(processStartInfo);
			}
			Console.WriteLine("NR6Pack NuGet package created.");
		}

		public void Help()
		{
			Console.WriteLine("devtool -tool createnuget -packagesdir <packages-directory> -nuspec <name-of-nuspec-file> <name-of-nuspec-file> ... -outputdir <output-directory>");
		}

		#endregion
	}
}

