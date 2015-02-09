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
// 	Copyright (C) 2012-2014 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------

using System;
using System.Collections;

namespace awzcore.Registration
{
	/// <summary>
	/// Indicates an NQ service.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ExportedServiceAttribute : Attribute
	{
		/// <summary>
		/// Constructor initializing the attribute with the service name.
		/// </summary>
		public ExportedServiceAttribute()
		{
		}

		/// <summary>
		/// Gets/sets whether the service is a single instance service.
		/// </summary>
		/// <remarks>
		/// Single instance services are created only once at application start.
		/// Then Service Manager's
		/// <see cref="AWZhome.Modularity.Registration.IServiceManager.GetService">GetService</see>
		/// method always returns that one instance. Single instance services can't be
		/// created, they are managed by the Service Manager.
		/// </remarks>
		public bool SingleInstance
		{
			get;
			set;
		}

		// TODO Remove this in future!
		/// <summary>
		/// Gets/sets the service category.
		/// </summary>
		/// <remarks>
		/// The category determines, in which NQ host the service is available.
		/// For example, a service with category
		/// <see cref="AWZhome.Modularity.Registration.NQHostMode.GUI">NQHostMode.GUI</see>
		/// is only available in a GUI application.
		/// </remarks>
//		public HostMode Category
//		{
//			get;
//			set;
//		}
	}
}
