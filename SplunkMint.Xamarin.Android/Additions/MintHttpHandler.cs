using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Splunk.Mint.Network;

namespace Splunk.Mint
{
	#region Splunk Network Xamarin Android Interception HTTP Delegating Handler

	/// <summary>
	/// An interception handler to use with your HttpClient REST client implementation.
	/// </summary>
	public class MintHttpHandler : DelegatingHandler
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="messageHandler">The HttpClientDelegatingHandler.</param>
		public MintHttpHandler(HttpMessageHandler messageHandler)
			: base(messageHandler)
		{
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			byte[] contentBytes = await request.Content.ReadAsByteArrayAsync();
			long startTime = DateTimeExtensions.GetCurrentUnixTimestampMillis ();
			string exceptionCaughtMessage = null;
			HttpResponseMessage responseMessage = null;

			try
			{
				responseMessage = await base.SendAsync(request, cancellationToken);
			}
			catch (Exception ex) {
				exceptionCaughtMessage = ex.Message;
			}

			long endTime = DateTimeExtensions.GetCurrentUnixTimestampMillis (); 
			byte[] receivedBytes = await responseMessage.Content.ReadAsByteArrayAsync();

			await SaveNetworkActionAsync (request.RequestUri.ToString(), request.RequestUri.Scheme, startTime, endTime, (int)responseMessage.StatusCode,
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

