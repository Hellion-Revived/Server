// MSConnection.cs
//
// Copyright (C) 2023, OpenHellion contributors
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using OpenHellion.Networking.Message.MainServer;
using ZeroGravity;

namespace OpenHellion.Networking;

/// <summary>
/// 	Handles connections to the main server.<br/>
/// 	Since the main server uses an REST api, the requests cause a callback.
/// </summary>
public static class MSConnection
{
		public static string IpAddress = "127.0.0.1";
		public static ushort Port = 6001;

		public static string Address {
			get {
				return "http://" + IpAddress + ":" + Port;
			}
		}

		public delegate void SendCallback<T>(T data);

		/// <summary>
		/// 	Send a request to get data from the main server.
		/// </summary>
		public static void Get<T>(MSMessage message, SendCallback<T> callback)
		{
			Task task = Task.Run(() =>
			{
				try {
					// Create new client and request and load it with data.
					HttpClient httpClient = new HttpClient();
					HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Address + "/api/" + message.GetDestination());
					request.Content = new StringContent(message.ToString(), Encoding.UTF8, "application/json");

					Dbg.Log("Sending data to:", Address + "/api/" + message.GetDestination());

					// Send message and get result.
					HttpResponseMessage result = httpClient.SendAsync(request).Result;

					// Read data as string.
					string str = result.Content.ReadAsStringAsync().Result;

					Dbg.Log("Data:", str);

					// Make object out of data.
					callback(Json.Deserialize<T>(str));

					// Clean up.
					httpClient.Dispose();
					request.Dispose();
					result.Dispose();
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine("\nException Caught!");
					Console.WriteLine("Message :{0} ", e.Message);
				}
			});
		}

		/// <summary>
		/// 	Send a request to get data from the main server without a callback.
		/// </summary>
		public static void Send(MSMessage message)
		{
			Task task = Task.Run(async () =>
			{
				try {
					HttpClient httpClient = new HttpClient();
					HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Address + "/api/" + message.GetDestination());
					request.Content = new StringContent(message.ToString(), Encoding.UTF8, "application/json");

					// Send message.
					HttpResponseMessage result = httpClient.SendAsync(request).Result;

					// Clean up.
					httpClient.Dispose();
					request.Dispose();
					result.Dispose();
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine("\nException Caught!");
					Console.WriteLine("Message :{0} ", e.Message);
				}
			});
		}
}
