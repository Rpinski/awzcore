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
using System.IO;
using System.Linq;

namespace awzcore.Development.Tools
{
	/// <summary>
	/// Computes and writes a version to given file by replacing placeholders.
	/// </summary>
	public class Versionize : ITool
	{
		#region ITool implementation

		public void Execute(CommandLineHelper commandLineHelper)
		{
			string directory = Environment.CurrentDirectory;
			string mainVersion = "0.0.0.0";
			string readableVersion = null;
			if (commandLineHelper.IsCmdLineParamSet("dir"))
			{
				directory = commandLineHelper["dir"].FirstOrDefault() ?? directory;
			}
			if (commandLineHelper.IsCmdLineParamSet("version"))
			{
				mainVersion = commandLineHelper["version"].FirstOrDefault() ?? mainVersion;
			}
			if (commandLineHelper.IsCmdLineParamSet("readableVersion"))
			{
				readableVersion = commandLineHelper["readableVersion"].FirstOrDefault();
			}
			readableVersion = readableVersion ?? mainVersion;

			string inputFile = Path.Combine(directory, "AssemblyInfoTemplate.cs");
			string outputFile = Path.Combine(directory, "AssemblyInfo.cs");
			Console.WriteLine("Setting file revision in {0}", outputFile);

			// Assembly revision is the number of days past since 2014-01-01
			string revision = (DateTime.Now - (new DateTime(2014, 1, 1))).Days.ToString();
			ReplaceStringInFile(inputFile, outputFile, mainVersion, readableVersion, revision);
		}

		void ReplaceStringInFile(string inputFile, string outputFile, string mainVersion, string readableVersion, string generatedRevision)
		{
			try
			{
				using (StreamReader reader = new StreamReader(inputFile))
				{
					using (StreamWriter writer = new StreamWriter(outputFile))
					{
						while (!reader.EndOfStream)
						{
							string line = reader.ReadLine();
							// Replace placeholder and write to output
							writer.WriteLine(
								line
								.Replace("%%readableVersion%%", readableVersion)
								.Replace("%%version%%", mainVersion)
								.Replace("%%genrev%%", generatedRevision));
						}
						writer.Flush();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: {0}", ex.Message);
			}
		}

		public void Help()
		{
			Console.WriteLine("devtool -tool versionize <path-of-AssemblyInfo.cs>");
		}

		#endregion
	}
}

