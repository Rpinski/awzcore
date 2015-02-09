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
	/// Attribute defining a service substitution.
	/// </summary>
	/// <remarks>
	/// This attribute specifies, which other services this service
	/// substitutes. Then every access to one of the substituted
	/// services is redirected to this service.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class)]
	public class ServiceSubstituteAttribute : Attribute
	{
		/// <summary>
		/// Constructor initializing the attribute with a service list.
		/// </summary>
		/// <param name="serviceList">At least one service to substitute.</param>
		public ServiceSubstituteAttribute(params Type[] serviceList)
		{
			this.ServiceList = serviceList;
		}

		/// <summary>
		/// Returns the list of services substituted by this service.
		/// </summary>
		public Type[] ServiceList
		{
			get;
			private set;
		}
	}
}
