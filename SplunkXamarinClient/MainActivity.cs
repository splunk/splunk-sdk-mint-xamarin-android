using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Splunk.Mint;
using Android.Util;
using System.Net.Http;
using SplunkMint.XamarinExtensions.Android;
using System.Net.Http.Headers;
using ModernHttpClient;
using System.Net;
using System.Collections.Generic;

namespace SplunkXamarinClient
{
	[Activity (Label = "SplunkXamarinClient", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, IMintCallback
	{
		static string TransactionName = "TransactionXamarinAndroid";
		static string Tag = "MainActivity";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Mint.SetMintCallback(this);
			Mint.EnableDebug ();
			Mint.SetUserIdentifier ("gtaskos@splunk.com");
//			Mint.DisableNetworkMonitoring ();
			Mint.AddURLToBlackList("www.facebook.com");
			Mint.EnableLogging(true);

			// Log the last 100 messages
			Mint.SetLogging(100);

			// Log all messages with priority level "warning" and higher, on all tags
			Mint.SetLogging("*:W");

			// Log the latest 100 messages with priority level "warning" and higher,
			// on all tags
			Mint.SetLogging(100, "*:W");

			// Log all messages from the ActivityManager at priority "Info" or above,
			// all log messages with tag "MyApp", with priority "Debug" or above:
			Mint.SetLogging(400, "ActivityManager:I MyApp:D *:S");

			MintXamarin.InitAndStartXamarinSession(Application.Context, "3520f5c9");

			Mint.AddExtraData ("ExtraKey1", "ExtraValue1");

			Mint.ClearExtraData ();

			IDictionary<string, string> dictionaryMap = new Dictionary<string, string> ();
			dictionaryMap.Add ("ExtraDictionaryKey1", "ExtraDictionaryValue1");
			dictionaryMap.Add ("ExtraDictionaryKey2", "ExtraDictionaryValue2");

			Mint.AddExtraDataMap (dictionaryMap);

			IDictionary<string, string> globalExtras = Mint.ExtraData;

			Mint.RemoveExtraData ("ExtraKey1");

			Mint.LeaveBreadcrumb ("MainActivity:Oncreate");

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button nullReferenceButton = FindViewById<Button> (Resource.Id.myButton);
			Button flushButton = FindViewById<Button> (Resource.Id.flushButton);
			Button logEventLogLevelButton = FindViewById<Button> (Resource.Id.logEventLogLevelButton);
			Button logEventButton = FindViewById<Button> (Resource.Id.logEventButton);
			Button handleExceptionNullButton = FindViewById<Button> (Resource.Id.handleExceptionNullButton);
			Button handleExceptionArgumentButton = FindViewById<Button> (Resource.Id.handleExceptionArgumentButton);
			Button startTransactionButton = FindViewById<Button> (Resource.Id.startTransactionButton);
			Button stopTransactionButton = FindViewById<Button> (Resource.Id.stopTransactionButton);
			Button cancelTransactionButton = FindViewById<Button> (Resource.Id.cancelTransactionButton);
			Button httpClientButton = FindViewById<Button> (Resource.Id.httpClientButton);
			Button modernHttpClientButton = FindViewById<Button> (Resource.Id.modernHttpClientButton);
			
			nullReferenceButton.Click += NullReferenceClick;
			flushButton.Click += FlushClick;
			logEventLogLevelButton.Click += LogEventLogLevelClick;
			logEventButton.Click += LogEventClick;
			handleExceptionNullButton.Click += HandleNullReferenceClick;
			handleExceptionArgumentButton.Click += HandleArgumentClick;
			startTransactionButton.Click += TransactionStartClick;
			stopTransactionButton.Click += TransactionStopClick;
			cancelTransactionButton.Click += TransactionCancelClick;
			httpClientButton.Click += HttpClientRestPost;
			modernHttpClientButton.Click += ModernHttpClientRestPost;
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			Mint.StartSession (this);
		}

		protected override void OnStop ()
		{
			base.OnStop ();
			Mint.CloseSession (this);
		}

		void HandleNullReferenceClick(object sender, EventArgs args)
		{
			try
			{
				string a = null;
				a.ToString();
			}
			catch (Exception ex) {

				Java.Lang.Exception javaException = ex.ToJavaException();
				Log.Debug(Tag, "Java Exception: " + javaException.ToString());
				Mint.LogException (javaException);
			}
		}

		void HandleArgumentClick(object sender, EventArgs args)
		{
			try
			{
				throw new ArgumentException("A Xamarin Android ArgumentException for testing purposes!");
			}
			catch (Exception ex) {
				Mint.LogExceptionMessage ("HandledKey1", "HandledValue1", ex.ToJavaException());
			}
		}

		void HandleApplicationExceptionClick(object sender, EventArgs args)
		{
			try
			{
				throw new ApplicationException("A Xamarin Android ArgumentException for testing purposes!");
			}
			catch (Exception ex) {
				IDictionary<string, string> dictionaryMap = new Dictionary<string, string> ();
				dictionaryMap.Add ("DictionaryKey1", "DictionaryValue1");
				dictionaryMap.Add ("DictionaryKey2", "DictionaryValue2");
				Mint.LogExceptionMap (dictionaryMap, ex.ToJavaException());
			}
		}

		void NullReferenceClick(object sender, EventArgs args)
		{
			string a = null;
			a.ToString();
		}

		void FlushClick(object sender, EventArgs args)
		{
			Mint.Flush ();
		}

		void TransactionStartClick(object sender, EventArgs args)
		{
			Mint.TransactionStart (TransactionName);
		}

		void TransactionStopClick(object sender, EventArgs args)
		{
			Mint.TransactionStop (TransactionName);
		}

		void TransactionCancelClick(object sender, EventArgs args)
		{
			Mint.TransactionCancel (TransactionName, "Because I can!");
		}

		void LogEventClick(object sender, EventArgs args)
		{
			Mint.LogEvent ("Log a Xamarin Android Event!");
		}

		void LogEventLogLevelClick(object sender, EventArgs args)
		{
			Mint.LogEvent ("Log a Xamarin Android Event with Log Level!", MintLogLevel.Info);
		}

		public void DataSaverResponse (DataSaverResponse p0)
		{
			Log.Debug (Tag, string.Format("Data Saver Response: {0}", p0.ToString ()));
		}

		public void LastBreath (Java.Lang.Exception p0)
		{

		}

		public void NetSenderResponse (NetSenderResponse p0)
		{
			Log.Debug (Tag, string.Format("Net Sender Response: {0}", p0.ToString ()));
		}

		private const string URLRequestBin = "http://requestb.in/1jb4mq01";

		async void HttpClientRestPost (object sender, EventArgs args)
		{
			try
			{
				using (HttpClientHandler handler = new HttpClientHandler())
				{
					MintHttpHandler interceptionHandler = new MintHttpHandler(handler);
					HttpClient httpClient = new HttpClient(interceptionHandler);
					StringContent dataStringContent = new StringContent("Sample Text Data for HttpClient!");
					dataStringContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
					HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URLRequestBin)
					{
						Content = dataStringContent
					};
					HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);
					string responseString = await response.Content.ReadAsStringAsync();
					if (response.StatusCode == HttpStatusCode.OK &&
						response.IsSuccessStatusCode)
					{
						Log.Debug(Tag, "HttpClient Succeed!");
					}
					else
					{
						Log.Debug(Tag, "HttpClient Failed!");
					}
				}
			}
			catch(Exception ex) {
				Log.Debug(Tag, string.Format("Exception from HttpClient request: {0}", ex));
			}
		}

		async void ModernHttpClientRestPost (object sender, EventArgs e)
		{
			// Use MintHttpHandler to intercept your networking REST calls
			MintHttpHandler interceptionHandler = new MintHttpHandler(new NativeMessageHandler());
			HttpClient httpClient = new HttpClient (interceptionHandler);
			HttpResponseMessage responseMessage = await httpClient.PostAsync(URLRequestBin, new StringContent("Just A Test"));

			Log.Debug (Tag, responseMessage.ToString ());
		}
	}
}