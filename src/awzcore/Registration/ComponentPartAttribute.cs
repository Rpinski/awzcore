// 	------------------------------------------------------------------------
// 	 awzcore Library
//
// 	 Homepage: http://www.awzhome.de/
// 	------------------------------------------------------------------------
//
// 	This Source Code Form is subject to the terms of the Mozilla Public
// 	License, v. 2.0. If a copy of the MPL was not distributed with this
// 	file, You can obtain one at http://mozilla.org/MPL/2// 	The Original Code is code of awzcore Library.e Library.
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
	/// Marks an assembly as a component part.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class ComponentPartAttribute : Attribute
	{
		protected string _ppName;

		/// <summary>
		/// Initializes the attribute instance.
		/// </summary>
		/// <param name="name">The name of the component, that the marked assembly is a part of.</param>
		public ComponentPartAttribute(string name)
		{
			_ppName = name;
		}

		/// <summary>
		/// Gets/sets the name of the component, that the marked assembly is a part of.
		/// </summary>
		public string Name
		{
			get
			{
				return _ppName;
			}
			set
			{
				_ppName = value;
			}
		}
	}
}
