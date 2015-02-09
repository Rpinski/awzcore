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
using System.Linq;
using System.Text;
using System.IO;

namespace awzcore.Development
{
	/// <summary>
	/// Helper class for managing command-line parameters of an application.
	/// </summary>
	public class CommandLineHelper
	{
		private Dictionary<string, string[]> _cmdArgs = new Dictionary<string, string[]>();

		/// <summary>
		/// Creates a new instance of the command-line helper class.
		/// </summary>
		/// <param name="cmdargs">Array of command-line parameters as passed into Main method of the application.</param>
		public CommandLineHelper(string[] cmdargs) : this(cmdargs, null)
		{
		}

		/// <summary>
		/// Creates a new instance of the command-line helper class.
		/// </summary>
		/// <param name="cmdargs">Array of command-line parameters as passed into Main method of the application.</param>
		/// <param name="cfgFileName">
		/// Name of the file, where the helper tries to read additional parameters from.
		/// The file is assumed to be in the execution path.
		/// </param>
		public CommandLineHelper(string[] cmdargs, string cfgFileName)
		{
			string curparam = String.Empty;
			List<String> argscopy = new List<string>();

			if (!String.IsNullOrEmpty(cfgFileName))
			{
				// Read parameters from config file
				StreamReader cfgfile = null;
				try
				{
					cfgfile = new StreamReader(cfgFileName);
					while (!cfgfile.EndOfStream)
					{
						string cfgline = cfgfile.ReadLine();
						if ((cfgline != String.Empty) && (cfgline[0] != '#'))
						{
							argscopy.Add(cfgline);
						}
					}
				}
				catch (IOException)
				{
					// It doesn't matter if we can't read the file, it's not essential
				}
				finally
				{
					if (cfgfile != null)
						cfgfile.Close();
				}
			}

			// Add the command line parameters
			argscopy.AddRange(cmdargs);

			// Save parameters in a more readable structure
			Dictionary<string, List<string>> list = new Dictionary<string, List<string>>();
			foreach (string arg in argscopy)
			{
				if (arg[0] == '-')
				{
					curparam = arg.Substring(1);
					// Exclusive operator !
					if (curparam.EndsWith("!"))
					{
						// Override former setting
						curparam = curparam.Substring(0, curparam.Length - 1);
						if (list.ContainsKey(curparam))
							list.Remove(curparam);
					}

					if (!list.ContainsKey(curparam))
					{
						// Create a new list
						list.Add(curparam, new List<string>());
					}
				}
				else if (!String.IsNullOrEmpty(curparam))
				{
					// Add parameter to the list of the command line switch
					list[curparam].Add(arg);
				}
			}
			// Converting the command line switch lists to string arrays
			foreach (string paramlistKey in list.Keys)
			{
				string[] cmdarray = new string[list[paramlistKey].Count];
				if (cmdarray.Length > 0)
				{
					list[paramlistKey].CopyTo(cmdarray);
				}
				_cmdArgs.Add(paramlistKey, cmdarray);
			}
		}

		/// <summary>
		/// Retrieves the value of a command line parameter.
		/// </summary>
		/// <param name="paramname">Parameter name, without prefixes like <c>/</c> or <c>-</c>.</param>
		/// <returns>A string array with parameter data (a file name, for example).</returns>
		/// <remarks>
		/// The standard implementation <see cref="awzcore.ServiceManager">NQServiceManager</see> assumes the command
		/// line parameters to be of following format:
		/// <code>app.exe -p1 data1 data2 -p2 data3 -p3 -p4</code>
		/// where <c>-p1</c> to <c>-p4</c> are the parameter names and <c>data1</c> to <c>data4</c> the parameter data.
		/// </remarks>
		public string[] this[string paramname]
		{
			get
			{
				if (_cmdArgs.ContainsKey(paramname))
				{
					return _cmdArgs[paramname];
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Checks if a command-line parameter is set.
		/// </summary>
		/// <param name="paramname">Parameter name, without prefixes like <c>/</c> or <c>-</c>.</param>
		/// <returns><c>True</c>, if command-line parameter is set, <c>false</c> otherwise.</returns>
		public bool IsCmdLineParamSet(string paramname)
		{
			return _cmdArgs.ContainsKey(paramname);
		}
	}
}
