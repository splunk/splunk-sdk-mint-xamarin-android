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

	internal static class DateTimeExtensions
	{
		#region [ DateTime ]

		public static double DateTimeToUnixTimestamp(this DateTime dateTime)
		{
			return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
		}

		#endregion

		#region [ Double ]

		public static DateTime UnixTimeStampToDateTime(this double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		#endregion

		#region [ EPOCH ]

		private static readonly DateTime UnixEpoch =
			new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long GetCurrentUnixTimestampMillis()
		{
			return (long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
		}

		public static DateTime DateTimeFromUnixTimestampMillis(this long millis)
		{
			return UnixEpoch.AddMilliseconds(millis);
		}

		public static long GetCurrentUnixTimestampSeconds()
		{
			return (long)(DateTime.UtcNow - UnixEpoch).TotalSeconds;
		}

		public static DateTime DateTimeFromUnixTimestampSeconds(this long seconds)
		{
			return UnixEpoch.AddSeconds(seconds);
		}

		#endregion
	}
}

