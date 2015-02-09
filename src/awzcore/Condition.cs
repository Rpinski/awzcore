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
	/// Constants for expressing of comparison conditions.
	/// </summary>
	/// <remarks>
	/// <para>The constants may be combined. For example, the value</para>
	/// <code>NQCondition.Equal | NQCondition.Greater</code>
	/// <para>expresses a "greater or equal" condition.</para>
	/// </remarks>
	[Flags]
	public enum Condition
	{
		/// <summary>
		/// The compared values should be equal.
		/// </summary>
		Equal = 1,
		/// <summary>
		/// The first compared value must be greater than the second.
		/// </summary>
		Greater = 2,
		/// <summary>
		/// The first compared value must be lower than the sedond.
		/// </summary>
		Lower = 4
	}
}
