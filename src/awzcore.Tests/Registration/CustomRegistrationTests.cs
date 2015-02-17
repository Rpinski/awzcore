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
using awzcore.Registration;
using NUnit.Framework;
using awzcore.Interfaces;
using awzcore.Lifecycle;
using awzcore.Initialization;
using System.Linq;

namespace awzcore.Tests.Registration
{
	public class CustomRegistrationTests
	{
		CustomRegistration _registration;

		[SetUp]
		public void SetUp()
		{
			_registration = new CustomRegistration();
		}

		[Test]
		[Description("AddComponent must create a wrapper object and add it to registration.")]
		public void AddComponent()
		{
			const string ComponentName = "MyTestComponent";
			const string ComponentCopyright = "Copyright bla";

			var componentInfoWrapper = _registration.AddComponent(ComponentName, ComponentCopyright);

			Assert.That(_registration.ComponentInfos.ContainsKey(ComponentName));
			var componentInfo = _registration.ComponentInfos[ComponentName];
			Assert.That(componentInfo.Name == ComponentName);

			Assert.That(object.ReferenceEquals(componentInfo, componentInfoWrapper.ComponentInfo));
		}

		[Test]
		[Description("AddComponent must block registration of same component name multiple times.")]
		public void AddComponentMultipleTimes()
		{
			Func<CustomRegistration.CustomComponentRegistration> registrationFunc =
				() => _registration.AddComponent("MyTestComponent");
			var componentInfoWrapper = registrationFunc();
			Assert.That(componentInfoWrapper, Is.Not.Null);
			Assert.That(componentInfoWrapper.ComponentInfo.Name, Is.EqualTo("MyTestComponent"));
			Assert.That(() => registrationFunc(),
				Throws.Exception.TypeOf<ComponentDefinitionException>().With.Property("Reason").EqualTo(ErrorReason.ComponentMultiplyDefined));
		}

		[Test]
		[Description("AddAsService must create a new ServiceInfo using a PreCreatedSingletonLifecycle")]
		public void AddAsService()
		{
			string testService = "bla";

			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddAsService(testService);

			Assert.That(_registration.ServiceInfos.ContainsKey(typeof(string)));
			var serviceInfo = _registration.ServiceInfos[typeof(string)];
			Assert.That(serviceInfo.ServiceType == typeof(string));
			Assert.That(serviceInfo.Initializer == null);
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<PreCreatedSingletonLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService must create a new ServiceInfo with default lifecycle and initializer.")]
		public void AddServiceDefaultCtorImplementationEqualsInterfaceType()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<TestServiceDefaultCtor>();

			Type implementationType = typeof(TestServiceDefaultCtor);

			Assert.That(_registration.ServiceInfos.ContainsKey(implementationType));
			var serviceInfo = _registration.ServiceInfos[implementationType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<SimpleInitializer<TestServiceDefaultCtor>>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<MultiLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService must block registration of same interface multiple times.")]
		public void AddServiceForSameInterfaceTypeMultipleTimes()
		{
			var componentInfoWrapper = _registration.AddComponent("MyTestComponent");
			Func<CustomRegistration.CustomServiceRegistration> registrationFunc =
				() => componentInfoWrapper.AddService<TestServiceDefaultCtor>();
			var serviceInfoWrapper = registrationFunc();
			Assert.That(serviceInfoWrapper, Is.Not.Null);
			Assert.That(serviceInfoWrapper.ServiceInfo.ServiceInterface, Is.EqualTo(typeof(TestServiceDefaultCtor)));
			Assert.That(() => registrationFunc(),
				Throws.Exception.TypeOf<ServiceDefinitionException>().With.Property("Reason").EqualTo(ErrorReason.ServiceMultiplyDefined));
		}

		[Test]
		[Description("AddService must create a new ServiceInfo with default lifecycle and initializer using a separate interface type.")]
		public void AddServiceDefaultCtorSeparateInterfaceType()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<ITestService, TestServiceDefaultCtor>();

			Type implementationType = typeof(TestServiceDefaultCtor);
			Type interfaceType = typeof(ITestService);

			Assert.That(_registration.ServiceInfos.ContainsKey(interfaceType));
			var serviceInfo = _registration.ServiceInfos[interfaceType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<SimpleInitializer<TestServiceDefaultCtor>>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<MultiLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService must create a new ServiceInfo with default lifecycle and custom initializer func.")]
		public void AddServiceWithInitializerFunc()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<ITestService, TestServiceCustomCtor>(d => new TestServiceCustomCtor(10));

			Type implementationType = typeof(TestServiceCustomCtor);
			Type interfaceType = typeof(ITestService);

			Assert.That(_registration.ServiceInfos.ContainsKey(interfaceType));
			var serviceInfo = _registration.ServiceInfos[interfaceType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<Initializer<TestServiceCustomCtor>>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<MultiLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService must create a new ServiceInfo with custom lifecycle and initializer func.")]
		public void AddServiceWithInitializerFuncAndCustomLifecycle()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<ITestService, TestServiceCustomCtor>(d => new TestServiceCustomCtor(10), new SingletonLifecycle());

			Type implementationType = typeof(TestServiceCustomCtor);
			Type interfaceType = typeof(ITestService);

			Assert.That(_registration.ServiceInfos.ContainsKey(interfaceType));
			var serviceInfo = _registration.ServiceInfos[interfaceType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<Initializer<TestServiceCustomCtor>>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<SingletonLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService must create a new ServiceInfo with default lifecycle and custom initializer.")]
		public void AddServiceWithCustomInitializer()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<ITestService, TestServiceCustomCtor>(Initializer.Using(d => new TestServiceCustomCtor(10)));

			Type implementationType = typeof(TestServiceCustomCtor);
			Type interfaceType = typeof(ITestService);

			Assert.That(_registration.ServiceInfos.ContainsKey(interfaceType));
			var serviceInfo = _registration.ServiceInfos[interfaceType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<Initializer<TestServiceCustomCtor>>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<MultiLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService must create a new ServiceInfo with custom lifecycle and initializer.")]
		public void AddServiceWithCustomInitializerAndLifecycle()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<ITestService, TestServiceCustomCtor>(
					Initializer.Using(d => new TestServiceCustomCtor(10)), new SingletonLifecycle());

			Type implementationType = typeof(TestServiceCustomCtor);
			Type interfaceType = typeof(ITestService);

			Assert.That(_registration.ServiceInfos.ContainsKey(interfaceType));
			var serviceInfo = _registration.ServiceInfos[interfaceType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<Initializer<TestServiceCustomCtor>>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<SingletonLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService with dynamic type must create a new ServiceInfo with default lifecycle and initializer.")]
		public void AddServiceWithTypeDefaultCtorImplementationEqualsInterfaceType()
		{
			Type implementationType = typeof(TestServiceDefaultCtor);

			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService(implementationType);

			Assert.That(_registration.ServiceInfos.ContainsKey(implementationType));
			var serviceInfo = _registration.ServiceInfos[implementationType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<DynamicConstructorInitializer>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<MultiLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService with dynamic type must create a new ServiceInfo with default lifecycle and initializer using a separate interface type.")]
		public void AddServiceWithTypeDefaultCtorSeparateInterfaceType()
		{
			Type implementationType = typeof(TestServiceDefaultCtor);
			Type interfaceType = typeof(ITestService);

			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService(interfaceType, implementationType);

			Assert.That(_registration.ServiceInfos.ContainsKey(interfaceType));
			var serviceInfo = _registration.ServiceInfos[interfaceType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<DynamicConstructorInitializer>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<MultiLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService with dynamic type must create a new ServiceInfo with default lifecycle and initializer using a separate interface type.")]
		public void AddServiceWithTypeSeparateInterfaceTypeAndCustomLifecycle()
		{
			Type implementationType = typeof(TestServiceDefaultCtor);
			Type interfaceType = typeof(ITestService);

			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService(interfaceType, implementationType, new SingletonLifecycle());

			Assert.That(_registration.ServiceInfos.ContainsKey(interfaceType));
			var serviceInfo = _registration.ServiceInfos[interfaceType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<DynamicConstructorInitializer>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<SingletonLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService must create a new ServiceInfo with default lifecycle and initializer and later allow to change defaults.")]
		public void AddServiceSettingLifecycleAndInitializerLater()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<TestServiceDefaultCtor>()
				.Initializer(Initializer.Using((d) => new TestServiceDefaultCtor()))
				.Lifecycle(new SingletonLifecycle());

			Type implementationType = typeof(TestServiceDefaultCtor);

			Assert.That(_registration.ServiceInfos.ContainsKey(implementationType));
			var serviceInfo = _registration.ServiceInfos[implementationType];
			Assert.That(serviceInfo.ServiceType == implementationType);
			Assert.That(serviceInfo.Initializer, Is.TypeOf<Initializer<TestServiceDefaultCtor>>());
			Assert.That(serviceInfo.Lifecycle, Is.TypeOf<SingletonLifecycle>());

			Assert.That(serviceInfo, Is.SameAs(serviceInfoWrapper.ServiceInfo));
		}

		[Test]
		[Description("AddService must allow to add new registered service to a service list.")]
		public void AddServiceWithServiceListInclusion()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<TestServiceDefaultCtor>()
				.InList("ServiceList1")
				.InList("ServiceList2");

			Type implementationType = typeof(TestServiceDefaultCtor);

			Assert.That(_registration.ServiceInfos.ContainsKey(implementationType));
			var serviceInfo = _registration.ServiceInfos[implementationType];
			Assert.That(serviceInfo.MemberOfLists.Contains("ServiceList1"));
			Assert.That(serviceInfo.MemberOfLists.Contains("ServiceList2"));

			Assert.That(_registration.ServiceLists.ContainsKey("ServiceList1"));
			var serviceList = _registration.ServiceLists["ServiceList1"];
			Assert.That(serviceList.Contains(implementationType));
			Assert.That(_registration.ServiceLists.ContainsKey("ServiceList2"));
			serviceList = _registration.ServiceLists["ServiceList2"];
			Assert.That(serviceList.Contains(implementationType));
		}

		[Test]
		[Description("AddService must allow to set new registered service as a substitute for another.")]
		public void AddServiceWithSubstitution()
		{
			var serviceInfoWrapper = _registration.AddComponent("MyTestComponent")
				.AddService<TestServiceDefaultCtor>()
				.Substitutes<TestServiceCustomCtor>();

			Type implementationType = typeof(TestServiceDefaultCtor);
			Type substitutedType = typeof(TestServiceCustomCtor);

			Assert.That(_registration.ServiceInfos.ContainsKey(implementationType));
			var serviceInfo = _registration.ServiceInfos[implementationType];
			Assert.That(serviceInfo.Substitutes.Contains(substitutedType));

			Assert.That(_registration.Substitutions.ContainsKey(substitutedType));
			var targetServiceType = _registration.Substitutions[substitutedType];
			Assert.That(targetServiceType == implementationType);
		}
	}
}
