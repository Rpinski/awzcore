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
using System.Collections.Generic;

namespace awzcore.Metainfo
{
	/// <summary>
	/// Default not thread-safe implementation of a metainfo container.
	/// </summary>
	public class DefaultMetaInfoContainer : IMetaInfoContainer
	{
		readonly Dictionary<string, IComponentInfo> _componentInfos = new Dictionary<string, IComponentInfo>();
		readonly Dictionary<Type, IServiceInfo> _serviceInfos = new Dictionary<Type, IServiceInfo>();
		readonly Dictionary<Type, Type> _substitutions = new Dictionary<Type, Type>();
		readonly Dictionary<object, IList<Type>> _serviceLists = new Dictionary<object, IList<Type>>();

		#region IMetaInfoContainer implementation

		public System.Collections.Generic.IDictionary<string, IComponentInfo> ComponentInfos
		{
			get
			{
				return _componentInfos;
			}
			set
			{
				if (value != null)
				{
					foreach (var item in value)
					{
						_componentInfos[item.Key] = item.Value;
					}
				}
			}
		}

		public System.Collections.Generic.IDictionary<Type, IServiceInfo> ServiceInfos
		{
			get
			{
				return _serviceInfos;
			}
			set
			{
				if (value != null)
				{
					foreach (var item in value)
					{
						_serviceInfos[item.Key] = item.Value;
					}
				}
			}
		}

		public System.Collections.Generic.IDictionary<Type, Type> Substitutions
		{
			get
			{
				return _substitutions;
			}
			set
			{
				if (value != null)
				{
					foreach (var item in value)
					{
						_substitutions[item.Key] = item.Value;
					}
				}
			}
		}

		public System.Collections.Generic.IDictionary<object, System.Collections.Generic.IList<Type>> ServiceLists
		{
			get
			{
				return _serviceLists;
			}
			set
			{
				if (value != null)
				{
					foreach (var item in value)
					{
						_serviceLists[item.Key] = item.Value;
					}
				}
			}
		}

		#endregion
	}
}

