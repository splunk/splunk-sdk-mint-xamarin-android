using System;
using Android.Runtime;
using Android.Util;
using Java.Lang;

namespace Splunk.Mint
{
	public static class XamarinExceptionHandler
	{
		static string Tag = "XamarinExceptionHandler";

		public static void InitXamarinExceptionHandler ()
		{
			AndroidEnvironment.UnhandledExceptionRaiser += HandleUnhandledExceptionRaiser;
			Thread.DefaultUncaughtExceptionHandler = new UncaughtExceptionHandler ();
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledHandler;
		}

		static void HandleUnhandledExceptionRaiser (object sender, RaiseThrowableEventArgs e)
		{
			Log.Debug (Tag, "AndroidEnvironment.UnhandledExceptionRaiser handler is invoked!");
		}

		static void CurrentDomainUnhandledHandler (object sender, UnhandledExceptionEventArgs e)
		{
			Log.Debug (Tag, "CurrentDomainUnhandledHandler handler is invoked!");
		}
	}

	#region [ Exception Handler Class ]

	public class UncaughtExceptionHandler : Java.Lang.Object, Thread.IUncaughtExceptionHandler
	{
		static string Tag = "XamarinExceptionHandler";

		public void UncaughtException (Thread thread, Throwable ex)
		{
			Log.Debug (Tag, "UncaughtException handler is invoked!");
		}
	}

	#endregion
}

