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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace awzcore.Registration
{
	/// <summary>
	/// Defines a service or AttachList, that will be injected into a service instance automatically,
	/// when it's created by the Service Manager.
	/// </summary>
	/// <remarks>
	/// This attribute can be set multiple times. The order of the attributes defines the order of the
	/// injected services through the class constructor.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class AutoInjectionAttribute : Attribute
	{
		/// <summary>
		/// Creates a new instance of the attribute indicating an auto-injection
		/// of a single service.
		/// </summary>
		/// <param name="boundService">Type of the registered interface of the service to be injected.</param>
		public AutoInjectionAttribute(Type boundService)
		{
			// Some default values
			this.BoundService = boundService;
			this.InjectAsDependent = false;
			this.InjectFromAttachList = false;
			this.Overridable = false;
		}

		/// <summary>
		/// Creates a new instance of the attribute indicating an auto-injection
		/// of an AttachList.
		/// </summary>
		/// <param name="boundAttachList">Name of the service or AttachList to be injected.</param>
		/// <param name="serviceInterface">Type of the service interface to be injected.</param>
		public AutoInjectionAttribute(string boundAttachList, Type serviceInterface)
		{
			// Some default values
			this.BoundAttachList = boundAttachList;
			this.ServiceInterface = serviceInterface;
			this.InjectAsDependent = false;
			this.InjectFromAttachList = true;
			this.Overridable = false;
		}

		/// <summary>
		/// Gets/sets the registered interface of the bound service.
		/// </summary>
		public System.Type BoundService
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the name of the AttachList to be injected.
		/// </summary>
		public string BoundAttachList
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the type of the service interface to be injected.
		/// </summary>
		public Type ServiceInterface
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets whether the injected service/services instances should be created as 
		/// dependent services, i.e. the current instance should be passed to them.
		/// </summary>
		public bool InjectAsDependent
		{
			get;
			set;
		}

		/// <summary>
		/// Retrieves whether an AttachList (<c>true</c>) or a single service (<c>false</c>)
		/// has to be injected.
		/// </summary>
		/// <remarks>
		/// The returned value is determined by the constructor, that has been used.
		/// </remarks>
		public bool InjectFromAttachList
		{
			get;
			set;
		}

		/// <summary>
		/// Specifies whether this injection should be overridden by the instance passed through
		/// <see cref="AWZhome.Modularity.Registration.INQServiceManager.CreateDependentService#T">CreateDependentService</see>
		/// method and its variations.
		/// </summary>
		public bool Overridable
		{
			get;
			set;
		}
	}
}
