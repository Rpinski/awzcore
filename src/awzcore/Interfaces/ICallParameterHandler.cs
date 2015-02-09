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

namespace awzcore.Interfaces
{
	/// <summary>
	/// Interface for call parameter handlers.
	/// </summary>
	public interface ICallParameterHandler
	{
		/// <summary>
		/// Retrieves the value of a call parameter.
		/// </summary>
		/// <param name="paramname">Parameter name, without prefixes like <c>/</c> or <c>-</c>.</param>
		/// <returns>A string array with parameter data (a file name, for example).</returns>
		/// <remarks>
		/// The standard implementation <see cref="awzcore.ServiceManager"> assumes the command
		/// line parameters to be of following format:
		/// <code>app.exe -p1 data1 data2 -p2 data3 -p3 -p4</code>
		/// where <c>-p1</c> to <c>-p4</c> are the parameter names and <c>data1</c> to <c>data4</c> the parameter data.
		/// </remarks>
		string[] this[string paramname]
		{
			get;
		}

		/// <summary>
		/// Checks if a call parameter is set.
		/// </summary>
		/// <param name="paramname">Parameter name, without prefixes like <c>/</c> or <c>-</c>.</param>
		/// <returns><c>True</c>, if command-line parameter is set, <c>false</c> otherwise.</returns>
		bool IsParamSet(string paramname);
	}

	/// <summary>
	/// Dummy logger implementing <see cref="ICallParameterHandler"/> interface, but not reading any parameters.
	/// </summary>
	public class DummyCallParameterHandler : ICallParameterHandler
	{
		static readonly Lazy<ICallParameterHandler> _lazyInstance =
			new Lazy<ICallParameterHandler>(() => new DummyCallParameterHandler());

		public static ICallParameterHandler Instance
		{
			get
			{
				return _lazyInstance.Value;
			}
		}

		#region ICallParameterHandler implementation

		public bool IsParamSet(string paramname)
		{
			return false;
		}

		public string[] this[string paramname]
		{
			get
			{
				return null;
			}
		}

		#endregion
	}
}

