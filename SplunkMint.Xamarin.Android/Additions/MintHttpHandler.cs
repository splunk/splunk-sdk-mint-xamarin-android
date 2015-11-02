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
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Splunk.Mint.Network;
using System.Collections.Generic;
using System.Linq;
using Object = Java.Lang.Object;

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
			string url = request.RequestUri.ToString ();
			IList<string> urls = Mint.URLBlackList;
			bool urlIsBlacklisted = urls.FirstOrDefault (p => url.Contains (p)) != null;
			HttpResponseMessage responseMessage = null;

			if (!urlIsBlacklisted) {
				byte[] contentBytes = await request.Content.ReadAsByteArrayAsync();
				long startTime = DateTimeExtensions.GetCurrentUnixTimestampMillis ();
				string exceptionCaughtMessage = null;
				byte[] receivedBytes = null;

				try
				{
					responseMessage = await base.SendAsync(request, cancellationToken);
				    receivedBytes = await responseMessage.Content.ReadAsByteArrayAsync();
				}
				catch (Exception ex) {
					exceptionCaughtMessage = ex.Message;
				}

				long endTime = DateTimeExtensions.GetCurrentUnixTimestampMillis ();

			    await SaveNetworkActionAsync (url, request.RequestUri.Scheme, startTime, endTime, (int)(responseMessage?.StatusCode ?? 0),
					contentBytes.Length, receivedBytes?.Length ?? 0, exceptionCaughtMessage, Mint.ExtraData.ToJavaDictionary());
			} else {
				responseMessage = await base.SendAsync (request, cancellationToken);
			}

			return responseMessage;
		}

		private async Task SaveNetworkActionAsync(string url, string protocol, long startTime, long endTime, 
			int statusCode, long requestLength, long responseLength, string exception, IDictionary<string, Java.Lang.Object> extras)
		{
			await Task.Run(() =>
				{
					NetLogManager.Instance.LogNetworkRequest (url, protocol, startTime, endTime, 
						statusCode, requestLength, responseLength, exception, extras);
				});
		}
	}

	#endregion
}

