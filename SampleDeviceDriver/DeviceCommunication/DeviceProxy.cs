using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using VideoOS.Platform.DriverFramework.Utilities;

namespace SampleDeviceDriver.DeviceCommunication
{
    public class DeviceProxy
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        public void Send(string url, string switchId, string state)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                Toolbox.Log.LogDebug(nameof(Send), $"{url} - {switchId} - {state}");
                var content = new StringContent("{'state':'" + state + "'}");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.PutAsync(url + "api/Switches/" + switchId, content).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
                Toolbox.Log.LogError(nameof(Send), $"{url} - {switchId} - {state} => {e}");
            }
        }

        public string Get(string url, string switchId)
        {
            Toolbox.Log.LogDebug(nameof(Get), $"{url} - {switchId}");
            string actualUrl = url + "api/Switches/" + switchId;
            try
            {
                string content = client.GetStringAsync(actualUrl).Result;
                Toolbox.Log.LogDebug(nameof(Get), $"{url} - {switchId} => {content}");

                // expect content = "{'state':'" + state + "'}"
                // simple JSON parsing ;-)  w/o newlines... 
                var kvs = content.Split(new char[] { '{', '\'', '"', ' ', ':', '}' }, StringSplitOptions.RemoveEmptyEntries);
                string state = kvs[1];
                return state;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"can't get current state from {actualUrl}", ex);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.GetType() == typeof(HttpRequestException))
                    throw new ApplicationException($"can't get current state from {actualUrl}", ex.InnerException);
                throw;
            }
        }
    }
}