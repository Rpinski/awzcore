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
using NUnit.Framework;
using awzcore.Interfaces;
using System.Diagnostics;
using awzcore.Initialization;

namespace awzcore.Tests.Initialization
{
	public class InitializerTests
	{
		public class InitializerTestInstance
		{
			public static bool InstanceCreated = false;

			public InitializerTestInstance()
			{
				InstanceCreated = true;
			}
		}

		[Test]
		[DescriptionAttribute("Verifies Initializer functionality.")]
		public void TestInitializer()
		{
			var preparedInstance = new InitializerTestInstance();
			IInitializer initializer = Initializer.Using(d => preparedInstance);
			InitializerTestInstance instance = initializer.Create(null) as InitializerTestInstance;
			Assert.That(instance, Is.SameAs(preparedInstance));
		}

		[Test]
		[DescriptionAttribute("Verifies SimpleInitializer functionality.")]
		public void TestSimpleInitializer()
		{
			IInitializer initializer = new SimpleInitializer<InitializerTestInstance>();
			InitializerTestInstance instance = initializer.Create(null) as InitializerTestInstance;
			Assert.That(instance, Is.Not.Null);
		}
	}
}

