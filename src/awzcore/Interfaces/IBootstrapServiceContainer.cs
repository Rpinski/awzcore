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

namespace awzcore.Interfaces
{
	/// <summary>
	/// Interface for the service container managing all bootstrap services.
	/// </summary>
	public interface IBootstrapServiceContainer
	{
		/// <summary>
		/// Registers specific object instance as bootstrap service.
		/// </summary>
		/// <param name="instance">Service instance.</param>
		/// <typeparam name="T">Service type</typeparam>
		void Register<T>(T instance) where T : class;

		/// <summary>
		/// Returns instance of given service type.
		/// </summary>
		/// <typeparam name="T">Service type</typeparam>
		T Get<T>() where T : class;
	}

	/// <summary>
	/// Some extension helper methods to quickly access some important bootstrap services.
	/// </summary>
	public static class BootstrapServiceContainerHelperExtensions
	{
		/// <summary>
		/// Retrieves instance of logger service.
		/// </summary>
		/// <returns><see cref="ILogger"/> implementation.</returns>
		public static ILogger GetLogger(this IBootstrapServiceContainer container)
		{
			return container.Get<ILogger>();
		}

		/// <summary>
		/// Retrieves instance of call parameter handler service.
		/// </summary>
		/// <returns><see cref="ICallParameterHandler"/> implementation.</returns>
		public static ICallParameterHandler GetCallParameterHandler(this IBootstrapServiceContainer container)
		{
			return container.Get<ICallParameterHandler>();
		}

		/// <summary>
		/// Retrieves instance of component assembly provider service.
		/// </summary>
		/// <returns><see cref="IComponentAssemblyProvider"/> implementation.</returns>
		public static IComponentAssemblyProvider GetComponentAssemblyProvider(this IBootstrapServiceContainer container)
		{
			return container.Get<IComponentAssemblyProvider>();
		}

		/// <summary>
		/// Retrieves instance of metainfo container service.
		/// </summary>
		/// <returns><see cref="IMetaInfoContainer"/> implementation.</returns>
		public static IMetaInfoContainer GetMetaInfoContainer(this IBootstrapServiceContainer container)
		{
			return container.Get<IMetaInfoContainer>();
		}

		/// <summary>
		/// Retrieves instance of registration service.
		/// </summary>
		/// <returns><see cref="IRegistration"/> implementation.</returns>
		public static IRegistration GetRegistration(this IBootstrapServiceContainer container)
		{
			return container.Get<IRegistration>();
		}
	}
}

