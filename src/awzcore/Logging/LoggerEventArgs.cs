/*  
	------------------------------------------------------------------------
	 NQ Core Library
	
	 Homepage: http://www.awzhome.de/
	------------------------------------------------------------------------
	
	This Source Code Form is subject to the terms of the Mozilla Public
	License, v. 2.0. If a copy of the MPL was not distributed with this
	file, You can obtain one at http://mozilla.org/MPL/2.0/.
   
	The Original Code is code of NQ Core Library.

	The Initial Developer of the Original Code is Andreas Weizel.
	Portions created by the Initial Developer are
	Copyright (C) 2012 Andreas Weizel. All Rights Reserved.
	
	Contributor(s): (none)
	
	------------------------------------------------------------------------
*/

using System;

namespace awzcore.Logging
{

	/// <summary>
	/// Provides data for the logging event of the Service Manager.
	/// </summary>
	public class LoggerEventArgs : EventArgs
	{
		/// <summary>
		/// Constructor initializing the object with event data.
		/// </summary>
		/// <param name="timestamp">Log message timestamp.</param>
		/// <param name="sourceService">Instance of the service issuing the log event.</param>
		/// <param name="logType">Log message type like <c>Error</c>, <c>Info</c> or <c>Debug</c>.</param>
		/// <param name="logMessage">The log message text.</param>
		public LoggerEventArgs(DateTime timestamp, object sourceService, LogType logType, string logMessage)
		{
			this.Timestamp = timestamp;
			this.SourceService = sourceService;
			this.LogType = logType;
			this.LogMessage = logMessage;
		}

		/// <summary>
		/// Returns the original timestamp of the log event.
		/// </summary>
		public DateTime Timestamp
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns the service instance issuing the log event.
		/// </summary>
		public object SourceService
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns the log message type like <c>Error</c>, <c>Info</c> or <c>Debug</c>.
		/// </summary>
		public LogType LogType
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns the log message text.
		/// </summary>
		public string LogMessage
		{
			get;
			private set;
		}
	}

}