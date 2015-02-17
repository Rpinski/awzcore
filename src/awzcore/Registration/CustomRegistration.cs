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
using System.Reflection;
using System.Collections.Generic;
using awzcore.Metainfo;
using awzcore.Initialization;
using awzcore.Lifecycle;
using System.ComponentModel;

namespace awzcore.Registration
{
	/// <summary>
	/// Allows custom and flexible registration of components and services by code.
	/// </summary>
	public class CustomRegistration : IRegistration
	{
		public class CustomComponentRegistration
		{
			protected readonly ILogger Logger;
			protected readonly CustomRegistration Registration;

			public ComponentInfo ComponentInfo
			{
				get;
				private set;
			}

			protected CustomComponentRegistration(CustomComponentRegistration ccRegistration)
			{
				Logger = ccRegistration.Logger;
				Registration = ccRegistration.Registration;
				ComponentInfo = ccRegistration.ComponentInfo;
			}

			public CustomComponentRegistration(ILogger logger, CustomRegistration registration, ComponentInfo componentInfo)
			{
				Logger = logger;
				Registration = registration;
				ComponentInfo = componentInfo;
			}

			/// <summary>
			/// Registers a new service from given pre-created implementation instance.
			/// Uses implementation type as interface type for registration.
			/// </summary>
			/// <typeparam name="T">
			/// Type of service implementation,
			/// which is also used as interface type for later lookup of this service.
			/// <typeparam>
			/// <param name="instance">Instance initializer implementation to use.</param>
			/// <returns>Service registration helper object.</returns>
			public CustomServiceRegistration AddAsService<T>(T instance)
				where T : class
			{
				return AddAsService<T, T>(instance);
			}

			/// <summary>
			/// Registers a new service from given pre-created implementation instance.
			/// Explicitly sets the interface type to register the service with.
			/// </summary>
			/// <typeparam name="TI">Interface type used for later lookup of this service.<typeparam>
			/// <typeparam name="TC">Type of real service implementation.<typeparam>
			/// <param name="instance">Instance initializer implementation to use.</param>
			/// <returns>Service registration helper object.</returns>
			public CustomServiceRegistration AddAsService<TI, TC>(TC instance)
				where TC : class, TI
			{
				if (instance == null)
					throw new ArgumentNullException("instance");
				EnsureServiceInterfaceUniqueness(typeof(TI));

				var serviceInfo = new ServiceInfo {
					ServiceInterface = typeof(TI),
					ServiceInterfaceName = typeof(TC).FullName,
					ServiceType = typeof(TC),
					ServiceTypeName = typeof(TC).FullName,
					Initializer = null, // Allowed to be null in case of PreCreatedSingletonLifecycle
					Lifecycle = new PreCreatedSingletonLifecycle(instance)
				};
						
				Registration.ServiceInfos.Add(serviceInfo.ServiceInterface, serviceInfo);
				return new CustomServiceRegistration(this, serviceInfo);
			}

			/// <summary>
			/// Registers a new service with given service type.
			/// </summary>
			/// <typeparam name="T">
			/// Type of service implementation,
			/// which is also used as interface type for later lookup of this service.
			/// <typeparam>
			/// <returns>Service registration helper object.</returns>
			public CustomServiceRegistration AddService<T>()
				where T : new()
			{
				return AddService<T, T>();
			}

			/// <summary>
			/// Registers a new service with given interface and service implementation type.
			/// </summary>
			/// <typeparam name="TI">Interface type used for later lookup of this service.<typeparam>
			/// <typeparam name="TC">Type of real service implementation.<typeparam>
			/// <returns>Service registration helper object.</returns>
			public CustomServiceRegistration AddService<TI, TC>()
				where TC : TI, new()
			{
				return AddService<TI, TC>(new SimpleInitializer<TC>());
			}

			/// <summary>
			/// Registers a new service with given interface and service implementation type.
			/// </summary>
			/// <typeparam name="TI">Interface type used for later lookup of this service.<typeparam>
			/// <typeparam name="TC">Type of real service implementation.<typeparam>
			/// <param name="initializerFunc">Delegate for instance creation.</param>
			/// <returns>Service registration helper object.</returns>
			public CustomServiceRegistration AddService<TI, TC>(Func<InitializationData, TC> initializerFunc)
				where TC : TI
			{
				if (initializerFunc == null)
					throw new ArgumentNullException("initializerFunc");

				return AddService<TI, TC>(new Initializer<TC>(initializerFunc), new MultiLifecycle());
			}

			/// <summary>
			/// Registers a new service with given interface and service implementation type.
			/// </summary>
			/// <typeparam name="TI">Interface type used for later lookup of this service.<typeparam>
			/// <typeparam name="TC">Type of real service implementation.<typeparam>
			/// <param name="initializerFunc">Delegate for instance creation.</param>
			/// <param name="lifecycle">Lifecycle manager implementation to use.</param>
			/// <returns>Service registration helper object.</returns>
			public CustomServiceRegistration AddService<TI, TC>(Func<InitializationData, TC> initializerFunc, ILifecycle lifecycle)
				where TC : TI
			{
				if (initializerFunc == null)
					throw new ArgumentNullException("initializerFunc");

				return AddService<TI, TC>(new Initializer<TC>(initializerFunc), lifecycle);
			}

			/// <summary>
			/// Registers a new service with given interface and service implementation type.
			/// </summary>
			/// <typeparam name="TI">Interface type used for later lookup of this service.<typeparam>
			/// <typeparam name="TC">Type of real service implementation.<typeparam>
			/// <param name="initializer">Instance initializer implementation to use.</param>
			/// <returns>Service registration helper object.</returns>
			public CustomServiceRegistration AddService<TI, TC>(IInitializer initializer)
				where TC : TI
			{
				return AddService<TI, TC>(initializer, new MultiLifecycle());
			}

			/// <summary>
			/// Registers a new service with given interface and service implementation type.
			/// </summary>
			/// <typeparam name="TI">Interface type used for later lookup of this service.<typeparam>
			/// <typeparam name="TC">Type of real service implementation.<typeparam>
			/// <param name="initializer">Instance initializer implementation to use.</param>
			/// <param name="lifecycle">Lifecycle manager implementation to use.</param>
			/// <returns>Service registration helper object.</returns>
			public CustomServiceRegistration AddService<TI, TC>(IInitializer initializer, ILifecycle lifecycle)
				where TC : TI
			{
				if (initializer == null)
					throw new ArgumentNullException("initializer");
				if (lifecycle == null)
					throw new ArgumentNullException("lifecycle");
				EnsureServiceInterfaceUniqueness(typeof(TI));

				var serviceInfo = new ServiceInfo {
					ServiceInterface = typeof(TI),
					ServiceInterfaceName = typeof(TC).FullName,
					ServiceType = typeof(TC),
					ServiceTypeName = typeof(TC).FullName,
					Initializer = initializer,
					Lifecycle = lifecycle,
				};

				Registration.ServiceInfos.Add(serviceInfo.ServiceInterface, serviceInfo);
				return new CustomServiceRegistration(this, serviceInfo);
			}

			/// <summary>
			/// Registers a new service with given interface type.
			/// </summary>
			/// <param name="interfaceType">
			/// Type of service implementation,
			/// which is also used as interface type for later lookup of this service.
			/// </param>
			/// <returns>Service registration helper object.</returns>
			/// <remarks>>Uses <see cref="awzcore.Initialization.DynamicConstructorInitializer"/> by default.</remarks>
			public CustomServiceRegistration AddService(Type interfaceType)
			{
				return AddService(interfaceType, interfaceType);
			}

			/// <summary>
			/// Registers a new service with given interface type.
			/// </summary>
			/// <param name="interfaceType">
			/// Type of service implementation,
			/// which is also used as interface type for later lookup of this service.
			/// </param>
			/// <param name="lifecycle">Lifecycle manager implementation to use.</param>
			/// <returns>Service registration helper object.</returns>
			/// <remarks>>Uses <see cref="awzcore.Initialization.DynamicConstructorInitializer"/> by default.</remarks>
			public CustomServiceRegistration AddService(Type interfaceType, ILifecycle lifecycle)
			{
				return AddService(interfaceType, interfaceType);
			}

			/// <summary>
			/// Registers a new service with given interface and service implementation type.
			/// </summary>
			/// <param name="interfaceType">Interface type used for later lookup of this service.<param>
			/// <param name="implementationType">Type of real service implementation.<param>
			/// <returns>Service registration helper object.</returns>
			/// <remarks>>Uses <see cref="awzcore.Initialization.DynamicConstructorInitializer"/> by default.</remarks>
			public CustomServiceRegistration AddService(Type interfaceType, Type implementationType)
			{
				return AddService(interfaceType, implementationType, new MultiLifecycle());
			}

			/// <summary>
			/// Registers a new service with given interface and service implementation type.
			/// </summary>
			/// <returns>Service registration helper object.</returns>
			/// <remarks>>Uses <see cref="awzcore.Initialization.DynamicConstructorInitializer"/> by default.</remarks>
			public CustomServiceRegistration AddService(Type interfaceType, Type implementationType, ILifecycle lifecycle)
			{
				if (interfaceType == null)
					throw new ArgumentNullException("interfaceType");
				if (implementationType == null)
					throw new ArgumentNullException("implementationType");
				if (lifecycle == null)
					throw new ArgumentNullException("lifecycle");
				EnsureServiceInterfaceUniqueness(interfaceType);

				var serviceInfo = new ServiceInfo {
					ServiceInterface = interfaceType,
					ServiceInterfaceName = interfaceType.FullName,
					ServiceType = implementationType,
					ServiceTypeName = implementationType.FullName,
					Initializer = new DynamicConstructorInitializer(Logger),
					Lifecycle = lifecycle
				};
				Registration.ServiceInfos.Add(serviceInfo.ServiceInterface, serviceInfo);
				return new CustomServiceRegistration(this, serviceInfo);
			}

			/// <summary>
			/// Throws an appropriate exception when given service interface is already registered.
			/// </summary>
			/// <param name="serviceInterface">Service interface type to check.</param>
			private void EnsureServiceInterfaceUniqueness(Type serviceInterface)
			{
				if (Registration.ServiceInfos.ContainsKey(serviceInterface))
				{
					// This interface type is already registered
					throw new ServiceDefinitionException(ErrorReason.ServiceMultiplyDefined, "Service " + serviceInterface.Name + " is about to be defined for a second time.");
				}
			}
		}

		public class CustomServiceRegistration : CustomComponentRegistration
		{
			readonly ServiceInfo _serviceInfo;
			readonly List<IAutoInjection> _autoInjections = new List<IAutoInjection>();
			readonly List<Type> _substitutes = new List<Type>();
			readonly List<string> _substituteNames = new List<string>();
			readonly List<IComponentRequirement> _requires = new List<IComponentRequirement>();
			readonly List<string> _inLists = new List<string>();

			public ServiceInfo ServiceInfo
			{
				get
				{
					return _serviceInfo;
				}
			}

			public CustomServiceRegistration(CustomComponentRegistration ccRegistration, ServiceInfo serviceInfo)
				: base(ccRegistration)
			{
				_serviceInfo = serviceInfo;

				// Some additional initializations (collections)
				serviceInfo.AutoInjections = _autoInjections;
				serviceInfo.Substitutes = _substitutes;
				serviceInfo.SubstituteNames = _substituteNames;
				serviceInfo.Requires = _requires;
				serviceInfo.MemberOfLists = _inLists;
			}

			/// <summary>
			/// Sets an <see cref="awzcore.Interfaces.IInitializer"/> in registered service.
			/// </summary>
			/// <param name="initializer">Initializer instance.</param>
			public CustomServiceRegistration Initializer(IInitializer initializer)
			{
				_serviceInfo.Initializer = initializer;
				return this;
			}

			/// <summary>
			/// Sets a <see cref="awzcore.Interfaces.ILifecycle"/> in registered service.
			/// </summary>
			/// <param name="lifecycle">Lifecycle.</param>
			public CustomServiceRegistration Lifecycle(ILifecycle lifecycle)
			{
				_serviceInfo.Lifecycle = lifecycle;
				return this;
			}

			/// <summary>
			/// Defines an interface which is substututed by this service.
			/// </summary>
			/// <typeparam name="substitutedService">Interface type to substitute.</typeparam>
			public CustomServiceRegistration Substitutes<T>()
			{
				return Substitutes(typeof(T));
			}

			/// <summary>
			/// Defines an interface which is substututed by this service.
			/// </summary>
			/// <param name="substitutedService">Interface type to substitute.</param>
			public CustomServiceRegistration Substitutes(Type substitutedService)
			{
				if (substitutedService == null)
					throw new ArgumentNullException("substitutedService");

				_substitutes.Add(substitutedService);
				_substituteNames.Add(substitutedService.FullName);
				Registration.Substitutions.Add(substitutedService, _serviceInfo.ServiceInterface);
				return this;
			}

			/// <summary>
			/// Adds this service to a service list.
			/// </summary>
			/// <param name="listName">List identifier.</param>
			public CustomServiceRegistration InList(string listName)
			{
				if (string.IsNullOrEmpty(listName))
					throw new ArgumentException("listName is not a valid service list identifier.");

				_inLists.Add(listName);

				// Create new service lists on demand
				var globalServiceLists = Registration.ServiceLists;
				if (!globalServiceLists.ContainsKey(listName))
				{
					globalServiceLists.Add(listName, new List<Type>());
				}
				Registration.ServiceLists[listName].Add(_serviceInfo.ServiceInterface);
				return this;
			}
		}

		readonly ILogger _logger;
		readonly Dictionary<string, IComponentInfo> _componentInfos = new Dictionary<string, IComponentInfo>();
		readonly Dictionary<Type, IServiceInfo> _serviceInfos = new Dictionary<Type, IServiceInfo>();
		readonly Dictionary<object, IList<Type>> _serviceLists = new Dictionary<object, IList<Type>>();

		readonly Dictionary<Type, Type> _substitutions = new Dictionary<Type, Type>();

		public CustomRegistration()
			: this(DummyLogger.Instance)
		{
		}

		public CustomRegistration(ILogger logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Registers a new component with given name.
		/// </summary>
		/// <returns>Component registration helper object.</returns>
		/// <param name="name">Internal component name.</param>
		/// <param name="copyright">Copyright string for component.</param>
		public CustomComponentRegistration AddComponent(string name, string copyright = null)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			var componentInfo = new ComponentInfo {
				Name = name,
				Copyright = copyright
			};
			return AddComponent(componentInfo);
		}

		/// <summary>
		/// Registers a new component using data from given metadata object.
		/// </summary>
		/// <returns>Component registration helper object.</returns>
		/// <param name="componentInfo">Component info.</param>
		public CustomComponentRegistration AddComponent(ComponentInfo componentInfo)
		{
			if (componentInfo == null)
				throw new ArgumentNullException("componentInfo");
			if (string.IsNullOrEmpty(componentInfo.Name))
				throw new ComponentDefinitionException(ErrorReason.ComponentNameUnspecified, "componentInfo doesn't contain a valid name.");
			if (_componentInfos.ContainsKey(componentInfo.Name))
				throw new ComponentDefinitionException(ErrorReason.ComponentMultiplyDefined, "Trying to define component " + componentInfo.Name + " for a second time.");
			
			_componentInfos.Add(componentInfo.Name, componentInfo);
			return new CustomComponentRegistration(_logger, this, componentInfo);
		}

		#region IRegistration implementation

		public void LoadComponentData(IEnumerable<Assembly> assemblies)
		{
			// No-op
		}

		public IDictionary<string, IComponentInfo> ComponentInfos
		{
			get
			{
				return _componentInfos;
			}
		}

		public IDictionary<Type, IServiceInfo> ServiceInfos
		{
			get
			{
				return _serviceInfos;
			}
		}

		public IDictionary<Type, Type> Substitutions
		{
			get
			{
				return _substitutions;
			}
		}

		public IDictionary<object, IList<Type>> ServiceLists
		{
			get
			{
				return _serviceLists;
			}
		}

		#endregion
	}
}

