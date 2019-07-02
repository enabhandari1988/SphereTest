
using Newtonsoft.Json;
using OpenQA.Selenium.Remote;
using SphereTest.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Utf8Json;


namespace SphereTest.Helpers
{
    public static class ApiHelper
    {
        public static string BaseUrl { get; set; }
        public static string Resource { get; set; }
        public static string Parameter { get; set; }

        private static string _apiUrl;
        private static readonly HttpClient ApiClient = new HttpClient();
        private static HttpResponseMessage _responseMessage;
		#region Basics


        /// <summary>
        /// get latest response
        /// </summary>
        /// <returns></returns>
        public static HttpResponseMessage GetLastResponse()
        {
            return _responseMessage;
        }

        //  addHeader("Authorization", "Basic " + base64Text);
        //  addHeader("x-token", "xxf43535353534");
        /// <summary>
        /// Description: Add header for the request if needed
        /// </summary>
        /// <param name="head">head </param>
        /// <param name="val">head val</param>
        public static void AddHeader(string head, string val)
        {
            if (head.ToLower().Equals("content-type"))
            {
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(val));

            }
            else
            {
                ApiClient.DefaultRequestHeaders.Add(head, val);
            }
        }



        #region POST

        /// <summary>
        /// send the request for API with Post method and specific body content type. the API Content Type include XML,JSON, xwwwformurlencoded
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <param name="contenttype"></param>
        /// <returns>response </returns>
        public static string InvokeApiPost(string uri, string body, APIContentType contenttype)
        {
            HttpContent content = new StringContent(body, Encoding.UTF8);
            switch (contenttype)
            {
                case APIContentType.XML:
                    content = new StringContent(body, Encoding.UTF8, "application/xml");
                    break;
                case APIContentType.xwwwformurlencoded:
                    content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
                    break;
                default:
                    content = new StringContent(body, Encoding.UTF8, "application/json");
                    break;
            }
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			var res = ApiClient.PostAsync(uri, content).Result;
			_responseMessage = res;

			var response = JsonConvert.DeserializeObject<object>(_responseMessage.Content.ReadAsStringAsync().Result);
			ApiClient.DefaultRequestHeaders.Clear();
			return response.ToString();
		}

		#endregion

		/// <summary>
		/// send the request for API without body content type in Json type.
		/// </summary>
		/// <param name="uri"></param>
		/// <param header="header">application/octet-stream</param>
		/// <returns></returns>
		public static string InvokeApiPostWithoutBody(string uri)
        {

            HttpContent content = new StringContent("");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var res = ApiClient.PostAsync(uri, content).Result;

            var response = res.Content.ReadAsStringAsync().Result;
            ApiClient.DefaultRequestHeaders.Clear();
            return response;
        }

        public static string InvokeAPI_POST(string uri, string body)
        {
            HttpContent content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			var res = ApiClient.PostAsync(uri, content).Result;
            ApiClient.DefaultRequestHeaders.Clear();

            var response = res.Content.ReadAsStringAsync().Result;
            return response;
        }

		/// <summary>
		/// send the request for API with Post method and body content type in Json type.
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="body"></param>
		public static string InvokeApiPostJson(string uri, string body)
		{

			HttpContent content = new StringContent(body, Encoding.UTF8, "application/json");
			var res = ApiClient.PostAsync(uri, content).Result;

			var response = res.Content.ReadAsStringAsync().Result;
			ApiClient.DefaultRequestHeaders.Clear();
			return response;
		}


		#region Other Helpers

		/// <summary>
		/// To verify user message in response
		/// </summary>
		/// <param name="response"></param>
		/// <param name="UserMessage">If is null or empty, no need to verify</param>
		/// <returns>true: verified passed;</returns>
		public static bool HasErrorMessageInResponse(string response, string ExpectedUserMessage)
        {
            if (string.IsNullOrEmpty(ExpectedUserMessage))
            {
                return true;
            }
            else
            {
                try
                {
                    var actualUM = Utility.GetFieldValueFromJsonString(response, "UserMessage");
                    return actualUM.ToLower().Equals(ExpectedUserMessage.ToLower());
                }
                catch
                {
                    return false;
                }

            }
        }

		#endregion

		/// <summary>
		/// Generate Json body by key value pairs and the string key value separate by ";"
		/// note; this method only used for the body with out sub nodes 
		/// </summary>
		/// <param name="keyValueString"></param>
		/// <returns></returns>
		public static string GenerateJsonBody(string keyValueString)
        {
            var keyNum = keyValueString.Split(';');
            var JsonString = "{ ";

            for (var i = 0; i < keyNum.Length; i++)
            {
                var tempPair = keyNum[i];
                var keyValuePair = tempPair.Split('=');

                var key = "\"" + keyValuePair[0] + "\"";
                var value = "\"" + keyValuePair[1] + "\"";
                JsonString = JsonString + key + ":";
                JsonString = JsonString + value + " ,";
            }

            JsonString = JsonString.Substring(0, JsonString.Length - 1);
            return JsonString + "}";
        }

		/// <summary>
		/// Generate Json body by one key value pair
		/// note; this method only used for the body with single value pair
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string CreateAPIBody(string name, string value)
		{
			string body = "{" + "\"" + name + "\"" + ":" + "\"" + value + "\"" + "}";
			return body;
		}

		#endregion


	}
}

