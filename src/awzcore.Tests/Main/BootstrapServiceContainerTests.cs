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
using NUnit.Framework;
using awzcore.Initialization;
using awzcore.Metainfo;
using awzcore.Registration;

namespace awzcore.Tests.Main
{
	public class BootstrapServiceContainerTests
	{
		IBootstrapServiceContainer _bootstrapServiceContainer;

		public BootstrapServiceContainerTests()
		{
		}

		[SetUp]
		public void SetUp()
		{
			_bootstrapServiceContainer = new BootstrapServiceContainer();
		}

		[Test]
		[Description("Without any additional registrations some default service instances must retrieved.")]
		public void DefaultServices()
		{
			Assert.That(_bootstrapServiceContainer.GetLogger(), Is.SameAs(DummyLogger.Instance));
			Assert.That(_bootstrapServiceContainer.GetCallParameterHandler(), Is.SameAs(DummyCallParameterHandler.Instance));
			Assert.That(_bootstrapServiceContainer.GetComponentAssemblyProvider(), Is.TypeOf<DummyComponentAssemblyProvider>());
			Assert.That(_bootstrapServiceContainer.GetMetaInfoContainer(), Is.TypeOf<DefaultMetaInfoContainer>());
			Assert.That(_bootstrapServiceContainer.GetRegistration(), Is.TypeOf<AttributeBasedRegistration>());
		}

		[Test]
		[Description("After registering a new service type, same registered service instance must be retrieved.")]
		public void ServiceRegistration()
		{
			_bootstrapServiceContainer.Register<ITestService>(new TestServiceDefaultCtor());
			var testService1 = _bootstrapServiceContainer.Get<ITestService>();
			Assert.That(testService1, Is.Not.Null);
			var testService2 = _bootstrapServiceContainer.Get<ITestService>();
			Assert.That(testService2, Is.Not.Null);
			Assert.That(testService1, Is.SameAs(testService2));
		}

		[Test]
		[Description("After registering an already registered service type, last registered service instance must be retrieved.")]
		public void ServiceReRegistration()
		{
			_bootstrapServiceContainer.Register<ITestService>(new TestServiceDefaultCtor());
			var testService1 = _bootstrapServiceContainer.Get<ITestService>();
			Assert.That(testService1, Is.Not.Null);
			Assert.That(testService1, Is.TypeOf<TestServiceDefaultCtor>());
			_bootstrapServiceContainer.Register<ITestService>(new TestServiceCustomCtor(3));
			var testService2 = _bootstrapServiceContainer.Get<ITestService>();
			Assert.That(testService2, Is.Not.Null);
			Assert.That(testService2, Is.TypeOf<TestServiceCustomCtor>());
		}
	}
}

