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
using awzcore.Interfaces;
using awzcore.Initialization;
using awzcore.Metainfo;
using awzcore.Registration;

namespace awzcore
{
	/// <summary>
	/// Container for bootstrap services providing functionality of main service container.
	/// </summary>
	public class BootstrapServiceContainer : IBootstrapServiceContainer
	{
		readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
		readonly Dictionary<Type, Func<object>> _defaultInitializers = new Dictionary<Type, Func<object>>();

		public BootstrapServiceContainer()
		{
			// Default initializers
			_defaultInitializers[typeof(ILogger)] = () => DummyLogger.Instance;
			_defaultInitializers[typeof(ICallParameterHandler)] = () => DummyCallParameterHandler.Instance;
			_defaultInitializers[typeof(IComponentAssemblyProvider)] = () => new DummyComponentAssemblyProvider();
			_defaultInitializers[typeof(IMetaInfoContainer)] = () => new DefaultMetaInfoContainer();
			_defaultInitializers[typeof(IRegistration)] = () => new AttributeBasedRegistration(this.GetCallParameterHandler(), this.GetLogger());
		}

		/// <summary>
		/// Registers specific object instance as bootstrap service.
		/// </summary>
		/// <param name="instance">Service instance.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Register<T>(T instance) where T : class
		{
			if (instance == null)
				return;

			_services[typeof(T)] = instance;
		}

		/// <summary>
		/// Returns instance of given service type.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Get<T>() where T : class
		{
			Type keyType = typeof(T);
			if (_services.ContainsKey(keyType))
			{
				return _services[keyType] as T;
			}

			// Do we have a default creator for lazy initialization?
			if (_defaultInitializers.ContainsKey(keyType))
			{
				T instance = _defaultInitializers[keyType]() as T;
				if (instance != null)
				{
					Register(instance);
					return instance;
				}
			}

			return null;
		}
	}
}

