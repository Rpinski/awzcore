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
	/// Sets a loading requirement for a component or service to make its presence
	/// dependent on the presence of another component.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, Inherited = true, AllowMultiple = true)]
	public class ComponentRequirementAttribute : Attribute
	{
		ComponentRequirement compReq = new ComponentRequirement();

		/// <summary>
		/// Initializes the attribute instance.
		/// </summary>
		/// <param name="compName">Name of the required component.</param>
		public ComponentRequirementAttribute(string compName)
		{
			compReq.ComponentName = compName;
		}

		/// <summary>
		/// Gets/sets the version of the component, that the marked component/service requires.
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
		/// Gets/sets the relational operator for the version constraint.
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
		/// Retrieves the requirement information as an instance of
		/// <see cref="AWZhome.Modularity.Registration.IComponentRequirement">INQComponentRequirement</see>.
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
