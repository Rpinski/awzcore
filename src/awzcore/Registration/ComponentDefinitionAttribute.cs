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
using System.Collections;

namespace awzcore.Registration
{

	/// <summary>
	/// Attribute to mark an assembly as an NQ main component.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class ComponentDefinitionAttribute : ComponentPartAttribute
	{
		private string _mpDisplayName;
		private string _mpUpdaterURL;
		private HostMode _category;
		private bool _noAutoLoad = false;

		/// <summary>
		/// The attribute's constructor.
		/// </summary>
		/// <param name="name">Component name.</param>
		public ComponentDefinitionAttribute(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Sets or returns the user readable name of the component.
		/// </summary>
		public string DisplayName
		{
			get
			{
				return _mpDisplayName;
			}
			set
			{
				_mpDisplayName = value;
			}
		}

		/// <summary>
		/// Sets or returns the URL where updates for the component can be downloaded.
		/// </summary>
		public string UpdaterURL
		{
			get
			{
				return _mpUpdaterURL;
			}
			set
			{
				_mpUpdaterURL = value;
			}
		}

		/// <summary>
		/// Gets or sets the component's category.
		/// </summary>
		public HostMode Category
		{
			get
			{
				return _category;
			}
			set
			{
				_category = value;
			}
		}

		/// <summary>
		/// When the value is set to <c>true</c>, the component is not loaded automatically
		/// by the Service Manager while it loads all components from bin directory.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Please keep in mind, that the assembly is always loaded physically independently
		/// of the value of NoAutoLoad. But the component and it's services are not registered
		/// and so are not accessible for other services.
		/// </para>
		/// <para>
		/// A component marked by NoAutoLoad property can be loaded by specifying
		/// the <c>-ca</c> command-line argument.
		/// </para>
		/// </remarks>
		public bool NoAutoLoad
		{
			get
			{
				return _noAutoLoad;
			}
			set
			{
				_noAutoLoad = value;
			}
		}

	}

}
