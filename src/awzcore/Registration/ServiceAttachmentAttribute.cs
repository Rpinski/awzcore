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
	/// Sets the AttachLists that the marked service is registered to.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ServiceAttachmentAttribute : Attribute
	{
		private string[] _serviceList = null;

		/// <summary>
		/// Initializes the attribute.
		/// </summary>
		/// <param name="serviceList">One or more AttachList names that the service belongs to.</param>
		public ServiceAttachmentAttribute(params string[] serviceList)
		{
			_serviceList = serviceList;
		}

		/// <summary>
		/// Gets the list of AttachLists the marked service is registered to.
		/// </summary>
		public string[] ServiceList
		{
			get
			{
				return _serviceList;
			}
		}
	}
}
