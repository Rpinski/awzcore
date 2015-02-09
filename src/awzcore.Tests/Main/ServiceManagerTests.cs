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
// 	Copyright (C) 2015 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------

using System;
using awzcore.Interfaces;
using awzcore.Registration;
using NUnit.Framework;
using awzcore.Lifecycle;

namespace awzcore.Tests.Main
{
	public class ServiceManagerTests
	{
		ServiceManager _serviceManager;
		CustomRegistration _registration;

		[SetUp]
		public void SetUp()
		{
			_serviceManager = new ServiceManager();
			_registration = new CustomRegistration();
			_serviceManager.RegisterBootstrap(BootstrapRegistration.For<IRegistration>(_registration));
		}

		[Test]
		[Description("Registers a simple service and tries to retrieve it.")]
		public void GetSimpleService()
		{
			_registration.AddComponent("MyTestComponent")
				.AddService<TestServiceDefaultCtor>();

			_serviceManager.Load();

			var instance1 = _serviceManager.Get<TestServiceDefaultCtor>();
			Assert.That(instance1, Is.TypeOf<TestServiceDefaultCtor>());
			var instance2 = _serviceManager.Get<TestServiceDefaultCtor>();
			Assert.That(instance2, Is.TypeOf<TestServiceDefaultCtor>());
			Assert.That(instance1, Is.Not.EqualTo(instance2));
		}

		[Test]
		[Description("Registers a singleton service and tries to retrieve it.")]
		public void GetSingletonService()
		{
			_registration.AddComponent("MyTestComponent")
				.AddService<TestServiceDefaultCtor>().Lifecycle(new SingletonLifecycle());

			_serviceManager.Load();

			var instance1 = _serviceManager.Get<TestServiceDefaultCtor>();
			Assert.That(instance1, Is.TypeOf<TestServiceDefaultCtor>());
			var instance2 = _serviceManager.Get<TestServiceDefaultCtor>();
			Assert.That(instance2, Is.TypeOf<TestServiceDefaultCtor>());
			Assert.That(instance1, Is.EqualTo(instance2));
		}

		[Test]
		[Description("Registers a service and another, that substitutes its registration. Service manager must return the 2nd service.")]
		public void GetSubstitutedService()
		{
			var instance1 = new TestServiceDefaultCtor();
			var instance2 = new TestServiceCustomCtor(5);

			_registration.AddComponent("MyTestComponent")
				.AddAsService(instance1)
				.AddAsService(instance2).Substitutes<TestServiceDefaultCtor>();

			_serviceManager.Load();

			var substitutorInstance = _serviceManager.Get<TestServiceCustomCtor>();
			Assert.That(substitutorInstance, Is.TypeOf<TestServiceCustomCtor>());
			Assert.That(substitutorInstance, Is.EqualTo(instance2));

			var substituteInstance = _serviceManager.Get<TestServiceDefaultCtor>();
			Assert.That(substituteInstance, Is.TypeOf<TestServiceCustomCtor>());
			Assert.That(substituteInstance, Is.EqualTo(instance2));
		}
	}
}

