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
// 	Copyright (C) 2012-2014 Andreas Weizel. All Rights Reserved.
//
// 	Contributor(s): (none)
//
// 	------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace awzcore.Interfaces
{
	/// <summary>
	/// Class carrying initialization data for service instance creation in <see cref="IInitializer"/>.
	/// </summary>
	public class InitializationData
	{
		public InitializationData(IServiceContainer serviceContainer, IServiceInfo serviceInfo)
		{
			if (serviceContainer == null)
				throw new ArgumentNullException("serviceContainer");
			if (serviceInfo == null)
				throw new ArgumentNullException("serviceInfo");

			ServiceContainer = serviceContainer;
			ServiceInfo = serviceInfo;
		}

		/// <summary>
		/// Gets the service container.
		/// </summary>
		/// <value>The service container.</value>
		public IServiceContainer ServiceContainer
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns metadata object for service whose instance is initialized.
		/// </summary>
		/// <value>The service info.</value>
		public IServiceInfo ServiceInfo
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the dynamic parameters to be injected into created instance.
		/// </summary>
		/// <value>The dynamic parameters as tuples of type and value.</value>
		/// <remarks>>
		/// Not every <see cref="IInitializer"/> implementation supports dynamic parameters.
		/// </remarks>
		public IEnumerable<Tuple<Type,object>> DynamicParameters
		{
			get;
			set;
		}
	}

	/// <summary>
	/// Interface for custom, service-specific instance creators,
	/// which are responsible for instantiation of services,
	/// also handling dependecies and auto-injection.
	/// </summary>
	public interface IInitializer
	{
		/// <summary>
		/// Retrieves a new instance of the given service.
		/// </summary>
		/// <param name="data">Initialization parameters.</param>
		object Create(InitializationData data);

		/// <summary>
		/// Gets a value indicating whether this <see cref="IInitializer"/> supports dynamic parameters.
		/// </summary>
		/// <value><c>true</c> if supports dynamic parameters; otherwise, <c>false</c>.</value>
		bool SupportsDynamicParameters { get; }
	}
}
