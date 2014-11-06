using System;
using Android.Util;

namespace Splunk.Mint
{
	public static class Extensions
	{
		public static Java.Lang.Exception ToJavaException (this System.Exception exception)
		{
			Java.Lang.Exception javaException = new Java.Lang.Exception (Java.Lang.Exception.FromException (exception));

			return javaException;
		}
	}
}

