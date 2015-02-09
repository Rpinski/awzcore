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

namespace awzcore.Logging
{
	/// <summary>
	/// Logger which fires events when messages are created.
	/// </summary>
	public class EventLogger : ILogger
	{
		public event EventHandler<LoggerEventArgs> MessageLogged;

		void OnMessageLogged(object fromService, LogType logType, Exception logException, string logMessage, params object[] args)
		{
			if (MessageLogged != null)
			{
				string message;
				if ((args != null) && (args.Length > 0))
				{
					message = String.Format(logMessage, args);
				}
				else
				{
					message = logMessage;
				}
				MessageLogged(this, new LoggerEventArgs(DateTime.Now, fromService, logType, logMessage));
			}
		}

		#region ILogger implementation

		public void Write(object sourceService, LogType logType, Exception logException, string logMessage, params object[] args)
		{
			OnMessageLogged(sourceService, logType, logException, logMessage, args);
		}

		#endregion
	}
}

