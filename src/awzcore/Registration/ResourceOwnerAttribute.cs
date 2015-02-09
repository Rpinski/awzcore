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
	[AttributeUsage(AttributeTargets.Assembly)]
	public class ResourceOwnerAttribute : Attribute
	{
		private string _nonTranslatedKey = null;
		private string _nonTranslatedName = null;
		private string _translatedKey = null;
		private string _translatedName = null;
		private string _nonTranslatedPath = String.Empty;

		public ResourceOwnerAttribute(string transKey, string transName)
		{
			_translatedKey = transKey;
			_translatedName = transName;
		}

		public ResourceOwnerAttribute(string nonTransKey, string nonTransName, string transKey, string transName)
			: this(transKey, transName)
		{
			_nonTranslatedKey = nonTransKey;
			_nonTranslatedName = nonTransName;
		}

		public string NonTranslatedKey
		{
			get
			{
				return _nonTranslatedKey;
			}
			set
			{
				_nonTranslatedKey = value;
			}
		}

		public string NonTranslatedName
		{
			get
			{
				return _nonTranslatedName;
			}
			set
			{
				_nonTranslatedName = value;
			}
		}

		public string TranslatedKey
		{
			get
			{
				return _translatedKey;
			}
			set
			{
				_translatedKey = value;
			}
		}

		public string TranslatedName
		{
			get
			{
				return _translatedName;
			}
			set
			{
				_translatedName = value;
			}
		}
		public string NonTranslatedPath
		{
			get
			{
				return _nonTranslatedPath;
			}
			set
			{
				_nonTranslatedPath = value;
			}
		}
	}
}
