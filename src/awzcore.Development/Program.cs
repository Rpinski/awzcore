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
using System.Linq;
using System.Collections.Generic;
using awzcore.Development.Tools;
using System.IO;
using System.Reflection;

namespace awzcore.Development
{
	class Program
	{
		static Dictionary<string, Func<ITool>> tools = new Dictionary<string, Func<ITool>>();

		static Program()
		{
			// Initialize tools
			tools["versionize"] = () => new Versionize();
			tools["createnuget"] = () => new CreateNuGet();
		}

		public static void Main(string[] args)
		{
			string configFile = null;
			if ((args != null) && (args.Length > 1) && (args[0] == "-fromfile"))
			{
				configFile = args[1];
			}

			CommandLineHelper commandLineHelper = new CommandLineHelper(args, configFile);
			if (commandLineHelper.IsCmdLineParamSet("tool"))
			{
				string toolName = commandLineHelper["tool"].FirstOrDefault();
				if (tools.ContainsKey(toolName))
				{
					ITool tool = tools[toolName]();
					if (tool != null)
					{
						if (commandLineHelper.IsCmdLineParamSet("help"))
						{
							// Show help for given module
							tool.Help();
						}
						else
						{
							// Execute tool
							tool.Execute(commandLineHelper);
						}
					}
				}
			}
		}
	}
}
