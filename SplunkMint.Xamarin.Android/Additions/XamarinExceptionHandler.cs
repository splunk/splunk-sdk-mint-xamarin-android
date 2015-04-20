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
using Android.Runtime;
using Java.Lang;
using System.Threading;

namespace Splunk.Mint
{
	public partial class Mint
	{
		static string Tag = "XamarinExceptionHandler";

		#region [ Properties ]

		private static Action lastBreath = delegate {};
		public static Action LastBreath { get{ return lastBreath; } set{ lastBreath = value; } }
		public static Func<System.Exception, bool> HandleUnobservedException { get; set; }
		public static event EventHandler<SplunkUnhandledEventArgs> UnhandledExceptionHandled = delegate { };

		#endregion

		public static void InitAndStartXamarinSession(Android.Content.Context context, string apiKey)
		{
			Mint.AddExtraData ("XamarinSDKVersion", "4.0.2");
			XamarinExceptionHandler.InitXamarinExceptionHandler ();
			Mint.InitAndStartSession (context, apiKey);
		}

		private static class XamarinExceptionHandler
		{
			public static void InitXamarinExceptionHandler ()
			{
				AndroidEnvironment.UnhandledExceptionRaiser += HandleUnhandledExceptionRaiser;
				UncaughtExceptionHandler uncaughtHandler = new UncaughtExceptionHandler ();
				uncaughtHandler.UncaughtExceptionHandled += OnUncaughtExceptionHandled;
				Java.Lang.Thread.DefaultUncaughtExceptionHandler = uncaughtHandler;
				AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledHandler;
				System.Threading.Tasks.TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionsHandler;
				AsyncSynchronizationContext.ExceptionCaught += SyncContextExceptionHandler;
				AsyncSynchronizationContext.Register();
			}

			/// <summary>
			/// It will register the async handler for unawaited void and Task unhandled exceptions thrown in the application.
			/// </summary>
			/// <remarks>
			/// This may be needed in special cases where the synchronization context could be null in early initialization.
			/// The async handlers are registered in the initialization process of the component.
			/// </remarks> 
			public static void RegisterAsyncHandlerContext()
			{
				AsyncSynchronizationContext.ExceptionCaught += SyncContextExceptionHandler;
				AsyncSynchronizationContext.Register();
			}

			/// <summary>
			/// Register monitoring of unobserved Task unhandled exceptions.
			/// </summary>
			public static void RegisterUnobservedTaskExceptions()
			{
				System.Threading.Tasks.TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionsHandler;
			}

			/// <summary>
			/// Unregister monitoring of unobserved Task unhandled exceptions.
			/// </summary>
			public static void UnregisterUnobservedTaskExceptions()
			{
				System.Threading.Tasks.TaskScheduler.UnobservedTaskException -= UnobservedTaskExceptionsHandler;
			}

			#region [ Private Methods ]

			private static void SyncContextExceptionHandler(object sender, System.Exception exception)
			{
				System.Diagnostics.Debug.WriteLine ("SyncContextExceptionHandler invoked");
				LogUnobservedUnawaitedException (exception);
			}

			private static void UnobservedTaskExceptionsHandler(object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
			{
				System.Diagnostics.Debug.WriteLine ("UnobservedTaskExceptionsHandler invoked");
				LogUnobservedUnawaitedException (e.Exception);
			}

			private static void LogUnobservedUnawaitedException(System.Exception exception)
			{
				if ((HandleUnobservedException != null &&
					HandleUnobservedException (exception)) ||					
					HandleUnobservedException == null) 
				{
					System.Diagnostics.Debug.WriteLine ("LogUnobservedUnawaitedExceptionAsync invoked");
					Mint.XamarinException (exception.ToJavaException (), false, null);
					OnUnhandledExceptionHandled (exception, "System.Exception");
				}
			}
				
			static void HandleUnhandledExceptionRaiser (object sender, RaiseThrowableEventArgs e)
			{
				Mint.XamarinException (e.Exception.ToJavaException (), false, null);
				LastBreath ();
				OnUnhandledExceptionHandled (e.Exception, "System.Exception");
			}

			static void CurrentDomainUnhandledHandler (object sender, UnhandledExceptionEventArgs e)
			{
				Mint.XamarinException ((e.ExceptionObject as System.Exception).ToJavaException(), false, null);
				LastBreath ();
				OnUnhandledExceptionHandled (e.ExceptionObject, "System.Exception");
			}

			static void OnUncaughtExceptionHandled(object sender, Throwable ex)
			{
				OnUnhandledExceptionHandled(ex, "Throwable");
			}

			static void OnUnhandledExceptionHandled(object exception, string type)
			{
				SplunkUnhandledEventArgs args = new SplunkUnhandledEventArgs ();
				args.ExceptionObject = exception;
				args.TypeOfException = type;
				args.HandledSuccessfully = true;
				UnhandledExceptionHandled (null, args);
			}

			#endregion
		}

		#region [ Exception Handler Class ]

		private class UncaughtExceptionHandler : Java.Lang.Object, Java.Lang.Thread.IUncaughtExceptionHandler
		{
			public event EventHandler<Throwable> UncaughtExceptionHandled = delegate { };

			public void UncaughtException (Java.Lang.Thread thread, Throwable ex)
			{
				Mint.XamarinException (ex.ToJavaException (), false, null);
				LastBreath ();
				UncaughtExceptionHandled (null, ex);
			}
		}

		#endregion

		#region Models

		/// <summary>
		/// Event argument class for the unhandled exceptions occurred and handled by the plugin.
		/// </summary>
		public class SplunkUnhandledEventArgs : EventArgs
		{
			/// <summary>
			/// The JSON fixture for the request to the server.
			/// </summary>
			public string ClientJsonRequest { get; set; }

			/// <summary>
			/// The exception object captured from the system.
			/// </summary>
			public object ExceptionObject { get; set; }

			/// <summary>
			/// If BugSense handled the exception successfully.
			/// </summary>
			public bool HandledSuccessfully { get; set; }   

			public string TypeOfException { get; set; }
		}

		#endregion

		#region [ Internal Class SynchronizationContext For Unawaited Void Methods ]

		internal class AsyncSynchronizationContext : SynchronizationContext
		{
			public static event EventHandler<System.Exception> ExceptionCaught = delegate { };

			public static AsyncSynchronizationContext Register()
			{
				var syncContext = Current;

				AsyncSynchronizationContext customSynchronizationContext = null;
				if (syncContext != null)
				{
					customSynchronizationContext = syncContext as AsyncSynchronizationContext;

					if (customSynchronizationContext == null)
					{
						customSynchronizationContext = new AsyncSynchronizationContext(syncContext);
						try
						{
							SetSynchronizationContext(customSynchronizationContext);
						}
						catch (System.Exception ex)
						{
							Console.WriteLine("SetSynchronizationContext Exception: {0}", ex);
						}
					}
				}
				return customSynchronizationContext;
			}

			private readonly SynchronizationContext _syncContext;

			public AsyncSynchronizationContext(SynchronizationContext syncContext)
			{
				_syncContext = syncContext;
			}

			public override SynchronizationContext CreateCopy()
			{
				return new AsyncSynchronizationContext(_syncContext.CreateCopy());
			}

			public override void OperationCompleted()
			{
				_syncContext.OperationCompleted();
			}

			public override void OperationStarted()
			{
				_syncContext.OperationStarted();
			}

			public override void Post(SendOrPostCallback d, object state)
			{
				_syncContext.Post(WrapCallback(d), state);
			}

			public override void Send(SendOrPostCallback d, object state)
			{
				_syncContext.Send(d, state);
			}

			private static SendOrPostCallback WrapCallback(SendOrPostCallback sendOrPostCallback)
			{
				return state =>
				{
					System.Exception exception = null;

					try
					{
						sendOrPostCallback(state);
					}
					catch (System.Exception ex)
					{
						exception = ex;
					}

					if (exception != null)
					{
						ExceptionCaught(null, exception);
						// Invoke here the exception
					}
				};
			}
		}

		#endregion
	}
}

