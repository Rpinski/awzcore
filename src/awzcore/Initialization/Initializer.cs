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
// 	Copyright (C) 2014 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------

using System;
using awzcore.Interfaces;

namespace awzcore.Initialization
{
	/// <summary>
	/// Static helper for creation of Initializer instances.
	/// </summary>
	public static class Initializer
	{
		/// <summary>
		/// Creates a new instance of Initializer class.
		/// </summary>
		/// <param name="initializer">Initializer.</param>
		/// <typeparam name="T">Type of returned service instance returned by this initialier.</typeparam>
		public static Initializer<T> Using<T>(Func<InitializationData, T> initializer)
		{
			return new Initializer<T>(initializer);
		}
	}

	/// <summary>
	/// Initializer using a delegate to create instances of a given service.
	/// </summary>
	public class Initializer<T> : IInitializer
	{
		Func<InitializationData, T> _initializer;

		/// <summary>
		/// Creater initializer using given delegate to create a new service instance.
		/// </summary>
		/// <param name="initializer">Initializer delegate.</param>
		public Initializer(Func<InitializationData, T> initializer)
		{
			_initializer = initializer;
		}

		#region IInitializer implementation

		public object Create(InitializationData data)
		{
			return _initializer(data);
		}

		public bool SupportsDynamicParameters
		{
			get
			{
				return false;
			}
		}

		#endregion
	}

	/// <summary>
	/// Initializer creating instances of a given service by calling their parameterless constructor.
	/// </summary>
	public class SimpleInitializer<T> : Initializer<T>
		where T : new()
	{
		/// <summary>
		/// Creates initializer calling the default constructor of given class.
		/// </summary>
		public SimpleInitializer()
			: base(d => new T())
		{
		}
	}
}

