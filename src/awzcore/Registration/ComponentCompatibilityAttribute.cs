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
using awzcore.Interfaces;
using awzcore.Metainfo;

namespace awzcore.Registration
{
	/// <summary>
	/// Attribute to set the versions an NQ component is compatible with.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class ComponentCompatibilityAttribute : Attribute
	{
		ComponentRequirement compReq = new ComponentRequirement();

		/// <summary>
		/// The attribute's constructor.
		/// </summary>
		/// <param name="version">The compatibility version.</param>
		/// <param name="condition">The compatibility condition.</param>
		public ComponentCompatibilityAttribute(string version, Condition condition)
		{
			compReq.Version = new Version(version);
			compReq.Condition = condition;
		}

		/// <summary>
		/// Get or sets the compatibility version.
		/// </summary>
		public string Version
		{
			get
			{
				return compReq.Version.ToString();
			}
			set
			{
				compReq.Version = new Version(value);
			}
		}

		/// <summary>
		/// Gets or sets the compatibility condition.
		/// </summary>
		public Condition Condition
		{
			get
			{
				return compReq.Condition;
			}
			set
			{
				compReq.Condition = value;
			}
		}

		/// <summary>
		/// Gets the <see cref="IComponentRequirement"/>
		/// instance that has been created for the compatibility constraint. This property is mainly used by
		/// the Service Manager.
		/// </summary>
		public IComponentRequirement ComponentRequirement
		{
			get
			{
				return compReq;
			}
		}
	}
}
