using System;
using Android.Runtime;
using Java.Lang;

namespace Splunk.Mint
{
	public partial class Mint
	{
		static string Tag = "XamarinExceptionHandler";

		private static Action lastBreath = delegate {};
		public static Action LastBreath { get{ return lastBreath; } set{ lastBreath = value; } }

		public static void InitAndStartXamarinSession(Android.Content.Context context, string apiKey)
		{
			XamarinExceptionHandler.InitXamarinExceptionHandler ();
			Mint.InitAndStartSession (context, apiKey);
		}

		private static class XamarinExceptionHandler
		{
			public static void InitXamarinExceptionHandler ()
			{
				AndroidEnvironment.UnhandledExceptionRaiser += HandleUnhandledExceptionRaiser;
				Thread.DefaultUncaughtExceptionHandler = new UncaughtExceptionHandler ();
				AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledHandler;
			}

			static void HandleUnhandledExceptionRaiser (object sender, RaiseThrowableEventArgs e)
			{
				Mint.XamarinException (e.Exception.ToJavaException (), false, null);
				LastBreath ();
			}

			static void CurrentDomainUnhandledHandler (object sender, UnhandledExceptionEventArgs e)
			{
				Mint.XamarinException ((e.ExceptionObject as System.Exception).ToJavaException(), false, null);
				LastBreath ();
			}
		}

		#region [ Exception Handler Class ]

		private class UncaughtExceptionHandler : Java.Lang.Object, Thread.IUncaughtExceptionHandler
		{
			public void UncaughtException (Thread thread, Throwable ex)
			{
				Mint.XamarinException (ex.ToJavaException (), false, null);
				LastBreath ();
			}
		}

		#endregion
	}
}

