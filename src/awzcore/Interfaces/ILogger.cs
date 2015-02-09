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

namespace awzcore.Interfaces
{
	/// <summary>
	/// Bootstrap service interface for logging functionality.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Logs a message from a service to the application protocol.
		/// </summary>
		/// <param name="sourceService">Instance of the service that has initiated the log event.</param>
		/// <param name="logType">Type of the logged message.</param>
		/// <param name="logException">The exception object to be logged.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		void Write(object sourceService, LogType logType, System.Exception logException, string logMessage, params object[] args);
	}

	/// <summary>
	/// Dummy logger implementing <see cref="ILogger"/> interface, but not putting anything out.
	/// </summary>
	public class DummyLogger : ILogger
	{
		static readonly Lazy<ILogger> _lazyInstance = new Lazy<ILogger>(() => new DummyLogger());

		public static ILogger Instance
		{
			get
			{
				return _lazyInstance.Value;
			}
		}

		#region ILogger implementation

		public void Write(object sourceService, LogType logType, Exception logException, string logMessage, params object[] args)
		{
		}

		#endregion
	}
}

