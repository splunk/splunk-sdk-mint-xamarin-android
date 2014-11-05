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
			MintXamarin.InitAndStartXamarinSession(Application.Context, "3520f5c9");
			Mint.EnableDebug ();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button nullReferenceButton = FindViewById<Button> (Resource.Id.myButton);
			Button startSessionButton = FindViewById<Button> (Resource.Id.startSessionButton);
			Button closeSessionButton = FindViewById<Button> (Resource.Id.closeSessionButton);
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
			startSessionButton.Click += StartSessionClick;
			closeSessionButton.Click += CloseSessionClick;
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
				Mint.LogException (new Java.Lang.Exception(Java.Lang.Exception.FromException(ex)));
			}
		}

		void NullReferenceClick(object sender, EventArgs args)
		{
			string a = null;
			a.ToString();
		}

		void StartSessionClick(object sender, EventArgs args)
		{
			Mint.StartSession (this);
		}

		void CloseSessionClick(object sender, EventArgs args)
		{
			Mint.CloseSession (this);
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
			//throw new NotImplementedException ();
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
			// Use SplunkInterceptionHttpHandler to intercept your networking REST calls
			MintHttpHandler interceptionHandler = new MintHttpHandler(new NativeMessageHandler());
			HttpClient httpClient = new HttpClient (interceptionHandler);
			HttpResponseMessage responseMessage = await httpClient.PostAsync(URLRequestBin, new StringContent("Just A Test"));

			Log.Debug (Tag, responseMessage.ToString ());
		}
	}
}