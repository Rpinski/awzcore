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
using System.Collections.Generic;
using System.Threading.Tasks;
using awzcore.Interfaces;

namespace awzcore
{
	/// <summary>
	/// Service manager - main programming interface to NQ.
	/// </summary>
	public class ServiceManager : IServiceContainer
	{
		readonly BootstrapServiceContainer _bootstrapServices = new BootstrapServiceContainer();
		bool _isLoaded = false;

		public ServiceManager()
		{
		}

		/// <summary>
		/// Registers a bootstrap service.
		/// </summary>
		/// <param name="bootstrapService">Bootstrap service to register.</param>
		/// <remarks>>
		/// Once <see cref="Load"/> method has been called to initialize manager, new services can't be registered.
		/// </remarks>
		public void RegisterBootstrap(IBootstrapRegistration bootstrapService)
		{
			if (_isLoaded)
			{
				// Don't change bootstrap service once we have loaded and collected everything to operate.
				throw new InvalidOperationException("Can't install bootstrap service after metainfo has been collected.");
			}

			bootstrapService.Register(BootstrapServices);
		}

		/// <summary>
		/// Initializes all components and service information.
		/// </summary>
		public void Load()
		{
			if (_isLoaded)
			{
				// Already loaded
				// TODO Throw an exception?
				return;
			}

			// Ensure we have implementations for all essential bootstrap services
			CheckBootstrapService<ILogger>();
			CheckBootstrapService<IComponentAssemblyProvider>();
			CheckBootstrapService<IRegistration>();
			CheckBootstrapService<IMetaInfoContainer>();

			var logger = BootstrapServices.GetLogger();

			// Get and/or load assemblies that we want to use
			var assemblies = BootstrapServices.GetComponentAssemblyProvider().GetAssemblies();

			// Collect metadata
			var registration = BootstrapServices.GetRegistration();
			registration.LoadComponentData(assemblies);
			var metaInfoContainer = BootstrapServices.GetMetaInfoContainer();
			metaInfoContainer.ComponentInfos = registration.ComponentInfos;
			metaInfoContainer.ServiceInfos = registration.ServiceInfos;
			metaInfoContainer.Substitutions = registration.Substitutions;
			metaInfoContainer.ServiceLists = registration.ServiceLists;

			// Initialize all lifecycles
			foreach (var serviceInfoMapping in metaInfoContainer.ServiceInfos)
			{
				serviceInfoMapping.Value.Lifecycle.Prepare(
					new InitializationData(this, serviceInfoMapping.Value),
					logger);
				serviceInfoMapping.Value.Lifecycle.InitialActivation();
			}

			_isLoaded = true;
		}

		/// <summary>
		/// Checks whether the given bootstrap service type is available in container
		/// and throws an exception otherwise.
		/// </summary>
		/// <typeparam name="T">Type of bootstrap service to check.</typeparam>
		void CheckBootstrapService<T>() where T : class
		{
			var bootstrapService = BootstrapServices.Get<T>();
			if (bootstrapService == null)
				throw new InvalidOperationException("No " + typeof(T).Name + " implementation available.");
		}

		protected IBootstrapServiceContainer BootstrapServices
		{
			get
			{
				return _bootstrapServices;
			}
		}

		/// <summary>
		/// Retrieves a service instance.
		/// </summary>
		/// <param name="passedInstances">Instances to be passed to created instance.</param>
		/// <typeparam name="T">Type of the retrieved service instance.</typeparam>
		/// <returns>Instance of the service object, typed as set by the <typeparamref name="T">T</typeparamref> parameter.</returns>
		/// <remarks>
		/// <c>Get</c> resolves any service substitutes. So if the requested
		/// service has been substituted by another service, this method will return an instance of that substituting service.
		/// </remarks>
		public T Get<T>(params object[] passedInstances) where T : class
		{
			CheckBootstrapService<ILogger>();
			CheckBootstrapService<IMetaInfoContainer>();
			var metaInfoContainer = BootstrapServices.GetMetaInfoContainer();

			if (!metaInfoContainer.ServiceInfos.ContainsKey(typeof(T)))
			{
				// Service is not registered
				throw new ServiceLoadingException(ErrorReason.UnknownService, "Service " + typeof(T).Name + " is not registered.");
			}

			var substitutorType = GetSubstitutingService(typeof(T));
			var serviceInfo = metaInfoContainer.ServiceInfos[substitutorType ?? typeof(T)];

			// Get lifecycle for requested service and let it retrieve the service instance
			object instance = serviceInfo.Lifecycle.Create(new InitializationData(this, serviceInfo));
			if (instance is T)
			{
				T createdInstance = (T) instance;
				return createdInstance;
			}
			else
			{
				// Throw a ServiceLoadingException
				string message = String.Format("Service {0} does not implement type {1}.", serviceInfo.ServiceInterface.Name, typeof(T).Name);
				BootstrapServices.GetLogger().Write(LogType.Error, "Unsupported interface type: " + message);
				throw new ServiceLoadingException(ErrorReason.UnsupportedInterfaceType, message);
			}
		}

		public Type GetSubstitutingService(Type serviceInterface)
		{
			CheckBootstrapService<IMetaInfoContainer>();
			var metaInfoContainer = BootstrapServices.GetMetaInfoContainer();

			// Search recursively for the actual service that is substituting the given one
			if (metaInfoContainer.Substitutions.ContainsKey(serviceInterface))
			{
				Type substitutor = metaInfoContainer.Substitutions[serviceInterface];
				Type substitutor2 = this.GetSubstitutingService(substitutor);
				if (substitutor2 != null)
				{
					return substitutor2;
				}
				else
				{
					return substitutor;
				}
			}
			else
			{
				return null;
			}
		}
	}
}

