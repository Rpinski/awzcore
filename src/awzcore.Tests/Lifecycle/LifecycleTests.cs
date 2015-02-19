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
// 	Copyright (C) 2014-2015 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------

using System;
using NUnit.Framework;
using awzcore.Interfaces;
using awzcore.Lifecycle;
using awzcore.Metainfo;
using awzcore.Initialization;
using awzcore.Tests.Fakes;

namespace awzcore.Tests.Lifecycle
{
	public class LifecycleTests
	{
		public class LifecycleTestInstance : IDisposable
		{
			public static bool InstanceCreated = false;
			public static bool InstanceDestroyed = false;

			public LifecycleTestInstance()
			{
				InstanceCreated = true;
			}

			public void Dispose()
			{
				InstanceDestroyed = true;
			}
		}

		InitializationData _initializationData;

		[SetUp]
		public void SetUp()
		{
			LifecycleTestInstance.InstanceCreated = false;
			LifecycleTestInstance.InstanceDestroyed = false;

			ServiceInfo serviceInfo = new ServiceInfo {
				Initializer = Initializer.Using((d) => new LifecycleTestInstance())
			};
			_initializationData = new InitializationData(new FakeServiceContainer(), serviceInfo);
		}

		[Test]
		[Description("Verifies that LazySingletonLifecycle creates the singleton only on demand.")]
		public void TestLazySingletonLifecycle()
		{
			ILifecycle lifecycle = new LazySingletonLifecycle();

			lifecycle.Prepare(_initializationData, DummyLogger.Instance);
			Assert.That(LifecycleTestInstance.InstanceCreated, Is.Not.True);

			lifecycle.InitialActivation();
			Assert.That(LifecycleTestInstance.InstanceCreated, Is.Not.True);

			var instance1 = lifecycle.Create(_initializationData);
			var instance2 = lifecycle.Create(_initializationData);

			Assert.That(instance1, Is.Not.Null);
			Assert.That(instance1, Is.SameAs(instance2));
			
			// Test destruction of singleton instance
			lifecycle.Destroy();
			Assert.That(LifecycleTestInstance.InstanceDestroyed, Is.True);
		}
		
		[Test]
		[Description("Verifies that SingletonLifecycle creates the singleton on initialization.")]
		public void TestSingletonLifecycle()
		{
			ILifecycle lifecycle = new SingletonLifecycle();

			lifecycle.Prepare(_initializationData, DummyLogger.Instance);
			Assert.That(LifecycleTestInstance.InstanceCreated, Is.Not.True);

			lifecycle.InitialActivation();
			Assert.That(LifecycleTestInstance.InstanceCreated, Is.True);

			var instance1 = lifecycle.Create(_initializationData);
			var instance2 = lifecycle.Create(_initializationData);

			Assert.That(instance1, Is.Not.Null);
			Assert.That(instance1, Is.SameAs(instance2));
			
			// Test destruction of singleton instance
			lifecycle.Destroy();
			Assert.That(LifecycleTestInstance.InstanceDestroyed, Is.True);
		}

		[Test]
		[Description("Verifies that MultiLifecycle creates new instances on every request.")]
		public void TestMultiLifecycle()
		{
			ILifecycle lifecycle = new MultiLifecycle();

			lifecycle.Prepare(_initializationData, DummyLogger.Instance);
			Assert.That(LifecycleTestInstance.InstanceCreated, Is.Not.True);

			lifecycle.InitialActivation();
			Assert.That(LifecycleTestInstance.InstanceCreated, Is.Not.True);

			var instance1 = lifecycle.Create(_initializationData);
			var instance2 = lifecycle.Create(_initializationData);

			Assert.That(instance1, Is.Not.Null);
			Assert.That(instance1, Is.Not.SameAs(instance2));
		}

		[Test]
		[Description("Verifies that PreCreatedSingletonLifecycle returns the same pre-defined service instance on every request.")]
		public void TestPreCreatedSingletonLifecycle()
		{
			var preparedInstance = new LifecycleTestInstance();
			ILifecycle lifecycle = new PreCreatedSingletonLifecycle(preparedInstance);

			lifecycle.Prepare(_initializationData, DummyLogger.Instance);
			lifecycle.InitialActivation();

			var instance1 = lifecycle.Create(_initializationData);
			var instance2 = lifecycle.Create(_initializationData);

			Assert.That(instance1, Is.Not.Null);
			Assert.That(instance1, Is.SameAs(preparedInstance));
			Assert.That(instance2, Is.Not.Null);
			Assert.That(instance2, Is.SameAs(preparedInstance));
		}
	}
}

