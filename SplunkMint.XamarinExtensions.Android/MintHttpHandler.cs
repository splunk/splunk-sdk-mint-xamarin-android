using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Splunk.Mint;
using System.Diagnostics;
using Splunk.Mint.Network;

namespace SplunkMint.XamarinExtensions.Android
{
	#region Splunk Network Interception Http Handler

	/// <summary>
	/// An interception handler to use with your HttpClient REST client implementation.
	/// </summary>
	public class MintHttpHandler : DelegatingHandler
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="messageHandler">The HttpClientHandler.</param>
		public MintHttpHandler(HttpMessageHandler messageHandler)
			: base(messageHandler)
		{
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			byte[] contentBytes = await request.Content.ReadAsByteArrayAsync();

			//			foreach (KeyValuePair<string, IEnumerable<string>> requestHeader in request.Headers)
			//			{
			//				networkDataFixture.Headers.Add(requestHeader.Key, requestHeader.Value.FirstOrDefault());
			//			}

			Debug.WriteLine(string.Format("Intercepting call!"));
			Debug.WriteLine(string.Format("URL: {0}", request.RequestUri));
			Debug.WriteLine(string.Format("HTTP Method: {0}", request.Method));
			byte[] sendBytes = await request.Content.ReadAsByteArrayAsync();
			Debug.WriteLine(string.Format("Bytes to send: {0}", sendBytes.Length));
			Stopwatch stopwatch = new Stopwatch();
			long startTime = Extensions.GetCurrentUnixTimestampMillis ();
			stopwatch.Start();
			string exceptionCaughtMessage = null;
			HttpResponseMessage responseMessage = null;
			try
			{
				responseMessage = await base.SendAsync(request, cancellationToken);
			}
			catch (Exception ex) {
				exceptionCaughtMessage = ex.Message;
			}

			stopwatch.Stop();
			long endTime = Extensions.GetCurrentUnixTimestampMillis (); 

			//			networkDataFixture.StatusCode = (int)responseMessage.StatusCode;
			//			networkDataFixture.Failed = !responseMessage.IsSuccessStatusCode;
			//			networkDataFixture.Duration = stopwatch.ElapsedMilliseconds;

			Debug.WriteLine(string.Format("Response Code: {0}", responseMessage.StatusCode));
			byte[] receivedBytes = await responseMessage.Content.ReadAsByteArrayAsync();
			Debug.WriteLine(string.Format("Latency time in millis: {0}", stopwatch.ElapsedMilliseconds));
			Debug.WriteLine(string.Format("Bytes received: {0}", receivedBytes.Length));

			await SaveNetworkActionAsync (request.RequestUri.AbsolutePath, request.RequestUri.Scheme, startTime, endTime, (int)responseMessage.StatusCode,
				contentBytes.Length, receivedBytes.Length, exceptionCaughtMessage);

			return responseMessage;
		}

		private async Task SaveNetworkActionAsync(string url, string protocol, long startTime, long endTime, 
			int statusCode, long requestLength, long responseLength, string exception)
		{
			await Task.Run(() =>
				{
					NetLogManager.Instance.LogNetworkRequest (url, protocol, startTime, endTime, 
						statusCode, requestLength, responseLength, exception);
				});
		}
	}

	#endregion
}

