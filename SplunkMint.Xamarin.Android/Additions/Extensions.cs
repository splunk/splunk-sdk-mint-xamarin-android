using System;
using Android.Util;

namespace Splunk.Mint
{
	public static class Extensions
	{
		public static Java.Lang.Exception ToJavaException (this System.Exception exception)
		{
//			string[] messages = exception.Message.Split (new [] { '\n' }, 1, StringSplitOptions.RemoveEmptyEntries);
//			string message = null;
//			if (messages != null &&
//				messages [0] != null) 
//			{
//				message = messages [0];
//			}

			Java.Lang.Exception javaException = new Java.Lang.Exception (Java.Lang.Exception.FromException (exception));

//			Log.Debug ("XamarinJavaExtension", javaException.Message);

			return javaException;
		}

		private class XamarinException : System.Exception
		{
			private string SplunkStackTrace { get; set; }
			private string SplunkMessage { get; set; }

			/// <summary>
			/// Initializes a custom Splunk>MINT exception
			/// </summary>
			/// <param name="message">The exception message.</param>
			/// <param name="stacktrace">The exception stacktrace.</param>
			/// <exception cref="ArgumentNullException">If message or stacktrace properties are null or empty.</exception>
			public XamarinException(string message, string stacktrace)
			{
				if (string.IsNullOrWhiteSpace(message))
					throw new ArgumentNullException("message", "Parameter cannot be null.");

				if (string.IsNullOrWhiteSpace(stacktrace))
					throw new ArgumentNullException("stacktrace", "Parameter cannot be null.");

				SplunkStackTrace = stacktrace;
				SplunkMessage = message;
			}

			public override string Message
			{
				get { return SplunkMessage; }
			}

			public override string StackTrace
			{
				get { return SplunkStackTrace; }
			}
		}
	}
}

