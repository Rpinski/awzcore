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
namespace awzcore.Interfaces
{
	/// <summary>
	/// Interface for bootstrap services generating <see cref="IComponentRequirement"/> instances.
	/// </summary>
	public interface IComponentRequirementFactory
	{
		IComponentRequirement Create();
	}

	/// <summary>
	/// Interface for objects that represent loading rules for a component.
	/// </summary>
	public interface IComponentRequirement
	{
		/// <summary>
		/// Retrieves the internal name of the component whose version has to be compared.
		/// </summary>
		string ComponentName
		{
			get;
		}

		/// <summary>
		/// Returns the component version for the loading rule.
		/// </summary>
		System.Version Version
		{
			get;
		}

		/// <summary>
		/// Returns the condition operator for version comparison.
		/// </summary>
		Condition Condition
		{
			get;
		}
	}
}
