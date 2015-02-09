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

namespace awzcore.Interfaces
{
	/// <summary>
	/// Adds some utility methods for <see cref="ILogger"/> interface.
	/// </summary>
	public static class LoggerExtensions
	{
		/// <summary>
		/// Logs a message to the application protocol.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logType">Type of the logged message.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Write(this ILogger logger, LogType logType, string logMessage, params object[] args)
		{
			logger.Write(null, logType, null, logMessage, args);
		}

		/// <summary>
		/// Logs an exception to the application protocol.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logType">Type of the logged message.</param>
		/// <param name="logException">The exception object to be logged.</param>
		/// <remarks>
		/// This method logs the name and the message of the exception and also puts out the stack trace,
		/// if the command-line parameter <c>-ls</c> is set.
		/// </remarks>
		public static void Write(this ILogger logger, LogType logType, System.Exception logException)
		{
			logger.Write(null, logType, logException, null, null);
		}

		/// <summary>
		/// Logs an exception from a service to the application protocol.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="fromService">Instance of the service that has initiated the log event.</param>
		/// <param name="logType">Type of the logged message.</param>
		/// <param name="logException">The exception object to be logged.</param>
		/// <remarks>
		/// This method logs the name and the message of the exception and also puts out the stack trace,
		/// if the command-line parameter <c>-ls</c> is set.
		/// </remarks>
		public static void Write(this ILogger logger, object fromService, LogType logType, System.Exception logException)
		{
			logger.Write(fromService, logType, logException, null, null);
		}

		/// <summary>
		/// Logs a message from a service to the application protocol.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="fromService">Instance of the service that has initiated the log event.</param>
		/// <param name="logType">Type of the logged message.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Write(this ILogger logger, object fromService, LogType logType, string logMessage, params object[] args)
		{
			logger.Write(fromService, logType, null, logMessage, args);
		}

		/// <summary>
		/// Creates an informational log message.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		/// <returns>The wrapper instance.</returns>
		public static void Info(this ILogger logger, string logMessage, params object[] args)
		{
			logger.Write(LogType.Info, logMessage, args);
		}

		/// <summary>
		/// Creates an informational log message from an exception.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logException">Exception to log information from.</param>
		public static void Info(this ILogger logger, Exception logException)
		{
			logger.Write(LogType.Info, logException);
		}

		/// <summary>
		/// Creates an informational log message from an exception with additional textual message.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logException">Exception to log information from.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Info(this ILogger logger, Exception logException, string logMessage, params object[] args)
		{
			logger.Write(null, LogType.Info, logException, logMessage, args);
		}

		/// <summary>
		/// Creates a debug log message.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Debug(this ILogger logger, string logMessage, params object[] args)
		{
			logger.Write(LogType.Debug, logMessage, args);
		}

		/// <summary>
		/// Creates a debug log message from an exception.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logException">Exception to log information from.</param>
		public static void Debug(this ILogger logger, Exception logException)
		{
			logger.Write(LogType.Debug, logException);
		}

		/// <summary>
		/// Creates a debug log message from an exception with additional textual message.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logException">Exception to log information from.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Debug(this ILogger logger, Exception logException, string logMessage, params object[] args)
		{
			logger.Write(null, LogType.Debug, logException, logMessage, args);
		}

		/// <summary>
		/// Creates an error log message.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Error(this ILogger logger, string logMessage, params object[] args)
		{
			logger.Write(LogType.Error, logMessage, args);
		}

		/// <summary>
		/// Creates an error log message from an exception.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logException">Exception to log information from.</param>
		public static void Error(this ILogger logger, Exception logException)
		{
			logger.Write(LogType.Error, logException);
		}

		/// <summary>
		/// Creates an error log message from an exception with additional textual message.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logException">Exception to log information from.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Error(this ILogger logger, Exception logException, string logMessage, params object[] args)
		{
			logger.Write(null, LogType.Error, logException, logMessage, args);
		}

		/// <summary>
		/// Creates a warning log message.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Warning(this ILogger logger, string logMessage, params object[] args)
		{
			logger.Write(LogType.Warning, logMessage, args);
		}

		/// <summary>
		/// Creates a warning log message from an exception.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logException">Exception to log information from.</param>
		public static void Warning(this ILogger logger, Exception logException)
		{
			logger.Write(LogType.Warning, logException);
		}

		/// <summary>
		/// Creates a warning log message from an exception with additional textual message.
		/// </summary>
		/// <param name="logger">Instance of logger bootstrap service.</param>
		/// <param name="logException">Exception to log information from.</param>
		/// <param name="logMessage">Message text. May include format definitions like in <see cref="System.String.Format"/></param>
		/// <param name="args">
		/// Additional arguments to be output in log,
		/// according to format definition in <paramref name="logMessage"/>.
		/// </param>
		public static void Warning(this ILogger logger, Exception logException, string logMessage, params object[] args)
		{
			logger.Write(null, LogType.Warning, logException, logMessage, args);
		}
	}
}
