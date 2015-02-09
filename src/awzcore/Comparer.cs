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
	/// Helper class to compare two values of types implementing 
	/// <see cref="System.IComparable">IComparable</see> 
	/// using a condition expressed by a <see cref="awzcore.Condition"/>
	/// value.
	/// </summary>
	/// <typeparam name="T">
	/// Type of the compared values. The type must
	/// implement <see cref="System.IComparable">IComparable</see>.
	/// </typeparam>
	public static class Comparer<T> where T : IComparable<T>
	{
		/// <summary>
		/// Compares two values using a <see cref="awzcore.Condition"/> expression.
		/// </summary>
		/// <param name="operand1">First value.</param>
		/// <param name="cond">Comparison operand.</param>
		/// <param name="operand2">Second value.</param>
		/// <returns>The logical result of the condition evaluation.</returns>
		public static bool CheckCondition(T operand1, Condition cond, T operand2)
		{
			bool condition = false;
			int comparison = operand1.CompareTo(operand2);

			// Check if we have matched any condition
			if ((cond & Condition.Equal) == Condition.Equal)
			{
				condition = condition || (comparison == 0);
			}
			if ((cond & Condition.Greater) == Condition.Greater)
			{
				condition = condition || (comparison > 0);
			}
			if ((cond & Condition.Lower) == Condition.Lower)
			{
				condition = condition || (comparison < 0);
			}

			return condition;
		}
	}
}
