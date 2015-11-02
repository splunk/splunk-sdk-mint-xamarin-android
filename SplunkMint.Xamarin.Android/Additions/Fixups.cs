 /*
    Copyright 2015 Splunk, Inc.
    *
    Licensed under the Apache License, Version 2.0 (the "License"): you may
    not use this file except in compliance with the License. You may obtain
    a copy of the License at
    *
    *     http://www.apache.org/licenses/LICENSE-2.0
    *
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
    WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
    License for the specific language governing permissions and limitations
    under the License.
*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Splunk.Mint
{
    public partial class Mint
    {
        static Mint()
        {
            URLBlackList = new List<string>();    
        }

        public static IList<string> URLBlackList { get; private set; }

        public static void AddURLToBlackList(string url)
        {
            if (!URLBlackList.Contains(url)) {
                URLBlackList.Add(url);
            }
            Mint.AddURLToBlackListInternal(url);
        }

        public static void LogExceptionMap (IDictionary<string, string> map, Exception exception)
        {
            LogExceptionMapInternal(map.ToJavaDictionary(), exception.ToJavaException());
        }

        public static void AddExtraDataMap(IDictionary<string, string> extraData)
        {
            AddExtraDataMapInternal(extraData.ToJavaDictionary());
        }

        public static IDictionary<string, string> ExtraData
        {
            get { return Mint.ExtraDataInternal().FromJavaDictionary(); }
        }
    }
}

namespace Splunk.Mint.Network 
{
	public partial class Counter
	{
		protected override global::Java.Lang.Object RawValue 
		{
			get{
				return RawValue2;
			}
		}

		public global::Java.Lang.Long Value 
		{
			get{
				return RawValue2;
			}
		}

	}

	public partial class Timer
	{
		protected override global::Java.Lang.Object RawValue 
		{
			get{
				return RawValue2;
			}
		}

		public global::Java.Lang.Long Value 
		{
			get{
				return RawValue2;
			}
		}
	}
}