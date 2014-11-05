using System;
using Android.Runtime;
using Android.Util;
using Java.Lang;

namespace Splunk.Mint
{
	public class MintXamarin
	{
		static string Tag = "XamarinExceptionHandler";

		public static void InitAndStartXamarinSession(Android.Content.Context context, string apiKey)
		{
			XamarinExceptionHandler.InitXamarinExceptionHandler ();
			Mint.InitAndStartSession (context, apiKey);

//			logNetworkRequest(final String url, final String protocol, final long startT, final long endT,
//				final int statusCode, final long requestLength, final long responseLength, final String exception)
//			Network.NetLogManager.Instance.LogNetworkRequest ();
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
			}

			static void CurrentDomainUnhandledHandler (object sender, UnhandledExceptionEventArgs e)
			{
				Mint.XamarinException ((e.ExceptionObject as System.Exception).ToJavaException(), false, null);
			}
		}

		#region [ Exception Handler Class ]

		private class UncaughtExceptionHandler : Java.Lang.Object, Thread.IUncaughtExceptionHandler
		{
			public void UncaughtException (Thread thread, Throwable ex)
			{
				Mint.XamarinException (ex.ToJavaException (), false, null);
			}
		}

		#endregion
	}
}