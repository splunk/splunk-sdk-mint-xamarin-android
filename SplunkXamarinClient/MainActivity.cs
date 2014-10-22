using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Splunk.Mint;

namespace SplunkXamarinClient
{
	[Activity (Label = "SplunkXamarinClient", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Mint.InitAndStartSession(Application.Context, "3520f5c9");
			Mint.EnableDebug ();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				string a = null;
				a.ToString();
			};
		}
	}
}