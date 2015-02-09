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
	/// Exception thrown when a service instance can't be created.
	/// </summary>
	public class ServiceLoadingException : Exception
	{
		private ErrorReason _reason = ErrorReason.UnknownReason;

		/// <summary>
		/// Constructor initializing the object with exception message text.
		/// </summary>
		/// <param name="reason">Reason code for the exception.</param>
		/// <param name="text">The message text.</param>
		public ServiceLoadingException(ErrorReason reason, string text)
			: base(text)
		{
			_reason = reason;
		}

		/// <summary>
		/// Returns the reason code for the exception.
		/// </summary>
		public ErrorReason Reason
		{
			get
			{
				return _reason;
			}
		}
	}
}
