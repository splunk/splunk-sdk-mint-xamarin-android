#Splunk MINT SDK for Xamarin Android#

Splunk MINT allows you to gain mobile intelligence about your mobile apps by using the Splunk MINT SDKs with your existing mobile app projects. Then, you can use Splunk MINT Management Console to monitor and gain insights into all of your mobile apps. 

*Splunk®, Splunk>®, Splunk MINT are trademarks of Splunk Inc., in the United States and other countries.  Xamarin is a trademark of Xamarin Inc.*

##Introduction##

In addition to sending crash reports, you can send additional data to Splunk MINT to monitor specific actions and items in your mobile apps.

* **Monitor transactions**. Track any process in your app from start to finish and identify slow transactions that negatively affect the user experience.
* **Add and report events**.  Add events to your code and report them to track virtually any user activity on your app.
* **Report handled exceptions**. Log handled exceptions that occur, along with any custom information you want to add. 
* **Add custom data and breadcrumbs to crash reports**. Add custom data to your crash reports as key-value pairs. You can also add breadcrumbs to your code to tag events or actions, which are also included in crash reports.
* **Report user-specific data**. Track the experience of any given user by adding user identifiers to your code, then you can search for errors that affected a particular user and examine the corresponding crash data.
* **Send log output**. Collect and view system debug messages depending on the platform. For example, send LogCat output from Android devices or NSLog messages from iOS devices.
* **Report debug messages**. Display debug messages during testing before you deploy to production.

Mobile apps that use the Splunk MINT SDKs send data to the MINT Data Collector, which then sends the data to Splunk MINT Management Console.

**How to monitor your mobile apps with Splunk MINT**

1. Download a Splunk MINT SDK or plugin for a platform that your app runs on, then import the SDK or plugin into your mobile app project.

2. [Log in to Splunk MINT Management Console](https://mint.splunk.com/dashboard) and create a project for your app. You'll get an API key for the project and a line of code to add for that particular platform&mdash;copy it to your clipboard.

3. Paste this line of code containing your project API key into your app to integrate MINT (for details, see the platform-specific sections in this guide).

    When you start using your app, it will begin to send data to the Splunk MINT Data Collector.

4. Go back to MINT Management Console and open your project. You'll start to see data appear in your dashboards in minutes.

    Repeat this procedure for each of the platforms your app runs on. After you've set up your projects in MINT Management Console, you can use the Splunk MINT App in Splunk&reg; Enterprise to see aggregated data for all of your mobile app projects over all time.

**Documentation**

* For more information about Splunk MINT, see the [Splunk MINT Overview](http://docs.splunk.com/Documentation/Mint/latest/ProductOverview/AboutSplunkMINT). 

**How to contribute**

If you would like to contribute to the SDK, go here for more information:

* [Splunk and open source](http://dev.splunk.com/view/opensource/SP-CAAAEDM)

* [Individual contributions](http://dev.splunk.com/goto/individualcontributions)

* [Company contributions](http://dev.splunk.com/view/companycontributions/SP-CAAAEDR)

## Requirements and installation for Xamarin Android ##

The requirements for the Splunk MINT SDK for Xamarin Android are:

* Visual Studio 2012 or later, or Xamarin Studio
* NuGet (the latest version)
* Xamarin Android
* Android SDK 1.6 (API Level 4) and later
* A Splunk MINT Management Console project for the Android platform type.

### Install the SDK plugin in Visual Studio ###

To install the Splunk MINT NuGet package by using the Package Manager Console, do the following:

1. Open the project you want to use with Splunk MINT.
2. On the **Tools** menu, point to **Library Package Manager**, and then click **Package Manager Console**.
3. In the **Package Manager Console** at the **PM>** prompt, type the following:
   
    Install-Package SplunkMint.Xamarin.Android

To install the Splunk MINT SDK by using the Package Manager window in Visual Studio:

1. Open the project you want to use with Splunk MINT.
2. On the **Tools** menu, point to **Library Package Manager**, and then click **Manage NuGet Packages for Solution**.
3. In the Manage NuGet Packages window, click **Online** from the list on the left, and then enter "SplunkMint.Xamarin.Android" into the **Search Online** field in the upper-right corner.

    Several Splunk MINT may packages appear in the list.

4. Click **Install** for the Splunk MINT Xamarin Android package to install.
5. In the Select Projects window, select the checkboxes next to the projects in which you want to install the package, and then click **OK**.

###Install the SDK plugin in Xamarin Studio ###

To install the Splunk MINT Nuget package in Xamarin Studio:

1. Select the project that you want to target.
2. From the Xamarin Studio menu, select **Project > Add packages**.
3. Search for "SplunkMint.Xamarin.Android".
4. Select the package,then click **Add Package** to add Splunk MINT to your project

## Add Splunk MINT to your Xamarin Android project ##

To use the SDK:

1. In the class that will use Splunk MINT, add the following `using` statement:

    `using SplunkMint;`

2. To add Splunk MINT to your project, you need just one line of code with your API key:

    ```
    protected override void OnCreate (Bundle bundle)
    {
        base.OnCreate (bundle);
       
        // Code...
        Mint.InitAndStartXamarinSession(Application.Context, "API_KEY");

        // Code...
    }
    ```
One line of code―that's it!

The **InitAndStartXamarinSession(Application.Context,** "API_KEY"**)** method installs the Splunk exception handlers for Xamarin uncaught exceptions and the performance monitor, sends all the saved data and performance metrics to Splunk MINT, and starts a new session for your activity.

A few more details are necessary though:

* If you haven't already given your app permission to access the Internet, you need to so that your app can send crash reports and performance metrics to Splunk MINT. Add the following line to your app **AndroidManifest.xml** file in the **Source** tab or use the checkboxes on the **Application** tab to give permission to access the internet and network state:

    `<uses-permission android:name="android.permission.INTERNET" />`
    
    `<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />`
       
* To have a better experience with the Splunk MINT dashboards, use numeric versioning schemes, preferably in MAJOR.MINOR.RELEASE format.

## Verify the SDK is integrated ##

To verify that the Splunk MINT SDK is correctly integrated into your Xamarin Android project, look at the **Insights** page of your Splunk MINT Management Console dashboard to view app activity. You can also crash your app intentionally to review the error information.

1. Use the following code to intentionally crash your app by causing a **nullPointer** exception:

    ```
    void NullReferenceClick(object sender, EventArgs args)
    {
        string a = null;
        a.ToString();
    }
    ```
    
    Allow some time (usually less than five minutes) for the error instance to appear on your dashboard. If your Splunk MINT Management Console dashboard indicates that your app has crashed, you know your app has successfully integrated Splunk MINT. Further, Splunk MINT generates an email message to inform you about exactly what went wrong.

2. Crash your app again to see the error counter on your dashboard increase.
3. To see Splunk MINT logs in your logcat console, enable debug mode by adding the following line before the **InitAndStartXamarinSession** method:

    `Mint.EnableDebug ();`
    
4. To investigate the cause of the error, go to the Errors page on your Splunk MINT Management Console dashboard. Click the error to examine the information Splunk MINT collects, and examine the error's stack trace to identify the exact lines of code that caused the problem.

## Customize session handling ##

By default, Splunk MINT uses the **InitAndStartXamarinSession** method and the time zone of our servers to calculate the time a user's session begins. Splunk MINT also flushes data automatically. However, you might want to customize session handling.

Use the following methods to start, close, and flush sessions:

* To explicitly start the session, use the **StartSession(***Context***)** method at the **onStart** method of your activity:

    `Mint.StartSession (this);`

    **Note**  If a session is opened before one minute has passed, this method does not open a new session.

* To close the active session, use the **CloseSession(***Context***)** method:

    `Mint.CloseSession(this);`
       
* To manually flush all saved data, use the **Flush()** method:

    `Mint.Flush();`

**Example code: Start session**

```
[Activity (Label = "SplunkXamarinClient", MainLauncher = true, Icon = "@drawable/icon")]
public class MainActivity : Activity
{
    protected override void OnCreate (Bundle bundle)
    {
        base.OnCreate (bundle);

        // TODO: Update with your API key
        MintXamarin.InitAndStartXamarinSession(Application.Context, "API_KEY");
        
        // Your code here
    }

    // Resume the session
    protected override void OnResume ()
    {
        base.OnResume ();
        Mint.StartSession (this);
    }

    // Stop the session
    protected override void OnStop ()
    {
        base.OnStop ();
        Mint.CloseSession (this);
    }
}
```

**Example code: Custom exit method**

Let's say you have implemented your own exit method using a button in your **AndroidManifest.xml** layout file or using a dialog box that intercepts the <tt>onback</tt> key event. The modified code below explicitly defines an exit method as the indicator that a session has ended. In this case, the user explicitly exited the application.

```
[Activity (Label = "SplunkXamarinClient", MainLauncher = true, Icon = "@drawable/icon")]
public class MainActivity : Activity
{
    protected override void OnCreate (Bundle bundle)
    {
        base.OnCreate (bundle);
        Button stopSessionButton = FindViewById<Button> (Resource.Id.yourButtonId);
        stopSessionButton.Click += StopSessionClick;

        // TODO: Update with your API key
        Mint.InitAndStartXamarinSession(Application.Context, "API_KEY");
        
        // Your code here
    }

    // Resume the session
    protected override void OnResume ()
    {
        base.OnResume ();
        Mint.StartSession (this);
    }

    // Stop the session
    void StopSessionClick(object sender, EventArgs args)
    {
        Mint.CloseSession (this);
    }
}
```

**Example code: Flush data**

Flush session data as follows:

```
[Activity (Label = "SplunkXamarinClient", MainLauncher = true, Icon = "@drawable/icon")]
public class MainActivity : Activity
{
    protected override void OnCreate (Bundle bundle)
    {
        base.OnCreate (bundle);

        // TODO: Update with your API key
        Mint.InitAndStartXamarinSession(Application.Context, "API_KEY");
        
        // Your code here
    }

    // Resume the session
    protected override void OnResume ()
    {
        base.OnResume ();
        Mint.StartSession (this);
    }

    // Stop the session
    protected override void OnStop ()
    {
        base.OnStop ();
        Mint.CloseSession (this);
        Mint.Flush ();
    }
}
```

## Monitor transactions ##

Transactions let you keep track of any process inside your application with a beginning and an end. For example, a transaction could be a process such as registration, login, or a purchase. 

A transaction is basically an event that starts and then finishes in one of three ways:

* The transaction is completed normally, resulting in a status of "SUCCESS".
* The transaction is cancelled by the user, possibly because the process took too much time to complete, resulting in a status of "CANCEL".
* The transaction failed because the app crashed, resulting in a status of "FAIL".

Use the following methods to work with transactions:

* To start a transaction, use the **transactionStart(***string***)** method as follows:

    `Mint.TransactionStart("Test1");`

* To stop a transaction, use the **transactionStop(***string***)** method as follows:

    `Mint.TransactionStop("Test1");`

* To cancel a transaction, use the **transactionCancel(***string, string***)** method as follows:

    `Mint.TransactionCancel("Test1", "This is the reason");`

To identify slow transactions that negatively affect the user experience, monitor how long transactions take to complete by going to the Transactions dashboard in Splunk MINT Management Console.

## Add and report events ##

In addition to reporting the sequence of events leading up to an app crash, Splunk MINT can report events that are not associated with a crash. For example, if your application asks users to make a selection, you can report the user's selection. You can also include the log level with the event: `Verbose, Debug, Info, Warning, or Error`.

* To report an event, use the **LogEvent(***string***)** method as follows:

    `Mint.LogEvent("Button1 pressed");`

* To report an event with the log level, use the **LogEvent(***string*, **MintLogLevel)** method as follows:

    `Mint.LogEvent("Button1 pressed", MintLogLevel.Info);`

Add as many events as you like to track virtually any user activity on your app. To view the event data, see the Events dashboard in Splunk MINT Management Console.

## Report handled exceptions ##

At times, you might expect your app to throws exceptions. When you handle those exceptions with a try-catch block, you can use the Splunk MINT exception handling feature to keep track of any exceptions your app throws and catches. Splunk MINT can also collect customized data associated with an exception.

* To log an exception, use the **LogException(Java.Lang.Exception)** method as follows:

    ```
    void HandleNullReferenceClick(object sender, EventArgs args)
    {
        try
        {
            string a = null;
            a.ToString();
        }
        catch (Exception ex) {
            // Since you are catching a System.Exception type you will need
            // to convert it to a Java.Lang.Exception type.
            // We provide you a convenient exception method ToJavaException().
            Mint.LogException (ex.ToJavaException());
        }
    }
    ```
    
* To add more information to the log, use the **LogExceptionMessage(***string*, *string*, **Java.Lang.Exception)** method as follows:

    `Mint.LogExceptionMessage ("HandledKey1", "HandledValue1", ex.ToJavaException());`
       
* To specify multiple key-value pairs as a HashMap, use the **LogExceptionMap(***customdata*, *exception***)** method as follows:

    ```
    IDictionary<string, string> dictionaryMap = new Dictionary<string, string> ();
    dictionaryMap.Add ("DictionaryKey1", "DictionaryValue1");
    dictionaryMap.Add ("DictionaryKey2", "DictionaryValue2");
    Mint.LogExceptionMap (dictionaryMap, ex.ToJavaException());
    ```
    
## Add custom data to crash reports ##

Although Splunk MINT collects plenty of data associated with each crash of your app, you can collect additional custom crash data. To add custom data to your crash reports, use the extra data map. The data values have a length limit of 128 characters.

* To add key-value pairs to the extra data map, use the **AddExtraData(***string*, *string***)** method as follows:

    `Mint.AddExtraData ("ExtraKey1", "ExtraValue1");`

* To add the custom data as a HashMap, use the **AddExtraDataMap(IDictionary<***string*, *string***>)** method as follows:

    ```
    IDictionarying, string> dictionaryMap = new Dictionary<string, string> ();
    dictionaryMap.Add ("ExtraDictionaryKey1", "ExtraDictionaryValue1");
    dictionaryMap.Add ("ExtraDictionaryKey2", "ExtraDictionaryValue2");
    Mint.AddExtraDataMap (dictionaryMap);
    ```
    
* Get the extra data map by using the **ExtraData** property.

    `IDictionary<string, string> globalExtras = Mint.ExtraData;`

* To remove a specific value from the extra data, use the **RemoveExtraData(***string***)** method as follows:

    `Mint.RemoveExtraData("Key");`
       
* To clear the extra data completely, use the **ClearExtraData()** method as follows:

    `Mint.ClearExtraData();`

To view the custom crash data in Splunk MINT Management Console:

1. Go to the **Errors** dashboard and select an error. 
2. In the error details section, click the **Error Instances** tab. 
3. In the **Show All** column, click the arrow.

## Add breadcrumbs to crash reports ##

To help investigate the cause of a crash, you can have Splunk MINT report the flow of events a user experienced leading up to the crash. When you know this sequence of things the user did with your app before it crashed, you are better equipped to reproduce the crash and diagnose the problem. To tag the events or actions in your app, add breadcrumbs to your code. Splunk MINT retains data associated with a maximum of 16 breadcrumbs prior to a crash.

* Use the **LeaveBreadcrumb(***string***)** method at the points of interest in your code as follows:

    `Mint.LeaveBreadcrumb("keyPressed");`
    `Mint.LeaveBreadcrumb("loginDone");`

## Use the crash callback ##

Use the **IMintCallback** interface to debug your Splunk MINT SDK projects. The **IMintCallback** interface provides two callback methods that give you more information about data that is being sent over the network or saved locally.

The SDK sends data over the network in the following cases:

* **When the app starts**. The SDK sends a "hello" packet to update all of the real-time information that is displayed in the Splunk MINT dashboards, along with all of the information that was collected and saved from previous sessions.
* **When you explicitly call the Flush() method**. The SDK sends all information that has been gathered and saved until that moment.
* **When an unexpected runtime exception occurs and crashes your app**. The SDK collects the crash information and quickly informs you about that crash.

If the SDK can't complete the process of sending data over the network connection, the data is saved locally so that it can be sent the next time an active connection is available and the SDK initializes or you use the **Flush()** method. 

The **IMintCallback** interface has three callbacks that you can use to find out what is happening: 

* The **DataSaverResponse** callback function tells you when data is saved locally.
* The **NetSenderResponse** callback function tells you when data is sent over the network.
* The **LastBreath** callback returns the exception that crashed your app.

Implement the **IMintCallback** interface as follows:

```
[Activity (Label = "SplunkXamarinClient", MainLauncher = true, Icon = "@drawable/icon")]
public class MainActivity : Activity, IMintCallback
{
    // Code ...
}
```

**DataSaverResponse**

Every time the SDK tries to save any information, the **DataSaverResponse** function is called with an object of the class **DataSaverResponse** as a parameter.

The **DataSaverResponse** contains information about the data that is saved, specifically the following fields:

```
// The data to save
public virtual string Data { get; }

// The path of the file
public virtual string Filepath { get; }

// The exception if any, or null
public virtual Java.Lang.Exception Exception { get; }

// A Boolean indicating whether the data was saved successfully
public virtual Java.Lang.Boolean SavedSuccessfully { get; }
```

The following example shows a simple way to use the **DataSaverResponse** callback function:

```
public void DataSaverResponse (DataSaverResponse p0)
{
    Log.Debug (Tag, string.Format("Data Saver Response: {0}", p0.ToString ()));
}
```

**NetSenderResponse**

Every time the SDK tries to send data over a network connection, the **NetSenderResponse** function is called with an object of the class **NetSenderResponse** as a parameter.

The **NetSenderResponse** contains information about the data that is sent, specifically the following fields:

```
// The data to send
public virtual string Data { get; }

// The URL of the server
ppublic virtual string Url { get; }

// The server's response
public virtual string ServerResponse { get; }

// The server's response code
public virtual int ResponseCode { get; }

// The exception if any, or null
public virtual Java.Lang.Exception Exception { get; }

// A Boolean indicating whether the data was sent successfully
public virtual Java.Lang.Boolean SentSuccessfully { get; }
```

The following example shows a simple way to use the **NetSenderResponse** callback function:

```
public void NetSenderResponse (NetSenderResponse p0)
{
    Log.Debug (Tag, string.Format("Net Sender Response: {0}", p0.ToString ()));
}
```

**LastBreath**

The **LastBreath** callback function is called after a runtime exception occurs and before the application terminates, returning the exception as a parameter so that you can know exactly what went wrong.

Use the **LastBreath** callback method as follows: 

```
public void LastBreath (Java.Lang.Exception p0)
{
    // Code ...
}
```

**Note**  Be careful when using the **LastBreath** function because it blocks the thread while running.

## Report user-specific data ##

With Splunk MINT, you can closely track the experience of any given user, for example to investigate a complaint. First, provide a user identifier such as an ID number from a database, an email address, a push ID, or a username. (However, please do not transmit any personally-identifiable or sensitive data into Splunk MINT.) Then in the Splunk MINT Management Console dashboard, you can search for errors that affect a particular user ID and examine crash data associated with her or her usage of your app. This feature is useful for apps with a high average revenue per user (ARPU), for apps that are deployed in a mobile device management (MDM) environment, and during quality-assurance testing.

* Use the **SetUserIdentifier(***string***)** method to set a user identifier as follows:

    `Mint.SetUserIdentifier ("someone@mint.splunk.com");`

Then, to search errors for a specific user name or ID, go to the Errors dashboard in Splunk MINT Management Console, then enter the user name or ID under **Search by username** in the list of filters.

## Disable network monitoring ##

Normally, Splunk MINT monitors all network calls. You can disable network monitoring entirely. Or, disable monitoring just for specific URLs by adding them to a monitoring blacklist to ignore any requests to these URLs. For example, you can add "www.facebook.com" to your blacklist to ignore requests to this site.

**Note** Currently, there are known compatibility issues with network monitoring and certain external libraries such as OkHttp, resulting in crashes. Splunk is currently working on this issue, but until it is resolved, disable network monitoring entirely if you experience this issue.

* To disable network monitoring entirely, use the **DisableNetworkMonitoring()** method before the **InitAndStartXamarinSession(***Context, string***)** method as follows:

    `Mint.DisableNetworkMonitoring ();`
       
* To add a URL to the network monitoring blacklist, use the **AddURLToBlackList(***string***)** method before the **InitAndStartXamarinSession(***Context, string***)** method as follows:

    `Mint.AddURLToBlackList("www.facebook.com");`

## Report LogCat output ##

LogCat is the Android logging system that lets you collect and view system debug output. To investigate how your app and user devices affect each other, you can instruct your app to send LogCat output with a crash report.

**Note** If you enable LogCat logging, only unhandled exceptions will contain the LogCat output. 

1. To send LogCat output with your exceptions, add the following line to your app's **AndroidManifest.xml** file on the **Source** tab or select the corresponding checkboxes on the **Application** tab:

    `<uses-permission android:name="android.permission.READ_LOGS" />`

    Splunk MINT uses this permission to read the LogCat output.

    **Note**  The READ_LOGS permission instructs the app to report the entire LogCat output, including passwords and other sensitive data that your app collects. Take care when storing and sharing this data.

2. To enable LogCat logging, use the **enableLogging** method as follows:

    `Mint.EnableLogging(true);`
       
3. Optionally, to restrict the number of lines or to set a filter string for log output, use the **SetLogging** method with additional parameters:

    * **SetLogging(***lines***)**
    * **SetLogging(***filter***)**
    * **SetLogging(***lines*, *filter***)**

The following code shows examples of using the **SetLogging** method with different filter expressions:

```
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
```

By default, Splunk MINT sends the last 5,000 lines with no filter. Splunk MINT filtering uses the same filtering mechanism as LogCat. For more information about filter expressions, see [Filtering Log Output](http://developer.android.com/tools/debugging/debugging-log.html#filteringOutput) on the Android Developers website.

To examine the LogCat output:

1. In your Splunk MINT Management Console dashboard, go to the **Errors** page and select an error.
2. On the error details page, click the **Error Instances** tab.
3. In the **View Logs** column, click the icon to view a log file for a particular error instance.

## Report debugging messages ##

You can display debug messages during testing to examine application errors and debug information before you deploy to production. However, you should disable this feature in production.

* To display debug messages, use the **EnableDebug()** method as follows:

    `Mint.EnableDebug();`

