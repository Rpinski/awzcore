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
	/// Specifies the "host mode" of the NQ application and the service category.
	/// A service is loaded if its category matches the mode.
	/// </summary>
	public enum HostMode
	{
		/// <summary>
		/// General mode. Only allowed to specify the service category, that means
		/// that the service is always loaded, not depending on the host mode of
		/// the application.
		/// </summary>
		General = 0,
		/// <summary>
		/// The NQ host is a GUI application.
		/// </summary>
		GUI,
		/// <summary>
		/// The NQ host is a console application.
		/// </summary>
		Console,
		/// <summary>
		/// The NQ host application is a Windows service.
		/// </summary>
		WindowsService,
		/// <summary>
		/// The NQ host is running as part of a web server.
		/// </summary>
		WebServer,
		/// <summary>
		/// The NQ host is running in a web browser application.
		/// </summary>
		Browser
	}
}
