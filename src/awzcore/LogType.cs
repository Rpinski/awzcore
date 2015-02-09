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
// 	The Original Code is code of NQ Core Library.
//
// 	The Initial Developer of the Original Code is Andreas Weizel.
// 	Portions created by the Initial Developer are
// 	Copyright (C) 2012-2014 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------


using System;

namespace awzcore
{

	/// <summary>
	/// Specifies the type of a log message.
	/// </summary>
	[Flags]
	public enum LogType
	{
		/// <summary>
		/// No logging.
		/// </summary>
		NoLogging = 0,
		/// <summary>
		/// Error message.
		/// </summary>
		Error = 1,
		/// <summary>
		/// Informational message.
		/// </summary>
		Info = 2,
		/// <summary>
		/// Debug message.
		/// </summary>
		Debug = 4,
		/// <summary>
		/// Warning message.
		/// </summary>
		Warning = 8,
		/// <summary>
		/// All messages.
		/// </summary>
		All = 15
	}
}