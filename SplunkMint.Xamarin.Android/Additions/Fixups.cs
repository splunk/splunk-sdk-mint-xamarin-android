using System;

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