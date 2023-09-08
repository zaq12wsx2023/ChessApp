using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CHESS
{
    class Program
    {
        public static string StartPt { get; set; }
        public static string EndPt { get; set; }

        private static HttpClient _client = new HttpClient();

        public static ChessAPI.API_Config _config = new ChessAPI.API_Config();

        public static ChessAPI.API_2_Response _Response2 = new ChessAPI.API_2_Response();

        public static void Main(string[] args)
        {
            GetStartEndPt();

            SendAPIRequest();

            ShowResult();
            Console.ReadLine();
        }

        public static void ShowResult()
        {
            if (_Response2 != null)
            {
                Console.WriteLine("Number of Moves: " + (_Response2.numberOfMoves ?? ""));
                Console.WriteLine("Shortest Path: " + (_Response2.shortestPath ?? ""));
            }
        }
        public static void SendAPIRequest()
        {
            try
            {
                APIRequestAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void APIRequestAsync()
        {
            try
            {
                CallAPI1();
                if ((_config.OperationId ?? "").Length > 0)
                {
                    Console.WriteLine("OperationId: " + _config.OperationId);
                    CallAPI2();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void CallAPI2()
        {
            try
            {
                _config.URI_2 += _config.OperationId;
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, _config.URI_2);

                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.DefaultConnectionLimit = 9999;
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                req.Headers.Add("Authorization", "No Auth");
                req.Headers.Add(HttpRequestHeader.ContentType.ToString(), "application/json");
                req.Headers.Add("Connection", "keep-alive");

                HttpResponseMessage response = _client.SendAsync(req).Result;

                if (response.IsSuccessStatusCode)
                {
                    ChessAPI.API_2_Response API2Response = response.Content.ReadAsAsync<ChessAPI.API_2_Response>().Result;
                    //return Task.FromResult(packageList);
                    _Response2 = API2Response;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void CallAPI1()
        {
            try
            {
                string authInfo = "";
                authInfo = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(authInfo));
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("", "") });
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage response = _client.PostAsync(_config.URI_1, content).Result;
                var statusCode = (int)response?.StatusCode;
                var ResponseContent = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    var ResponseResult = response.Content.ReadAsStringAsync().Result;
                    //Operation Id 29c016e5-c69d-42c4-a7a2-477ca81ff3eb was created. Please query it to find your results
                    var OperationId = (ResponseContent ?? "").Replace("Operation Id", "").Replace("was created. Please query it to find your results", "");
                    _config.OperationId = OperationId.Trim();
                }
                else
                {
                    throw new Exception($"StatusCodeId:{statusCode} |StatusCode: {response.StatusCode }|{ResponseContent?.ToString() }|{ response.RequestMessage}");
                }
            }
            catch (AggregateException err)
            {
                string ErrorCode = "";
                foreach (var errInner in err.InnerExceptions)
                {
                    ErrorCode += errInner;
                }
                throw new Exception(ErrorCode);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public static void GetStartEndPt()
        {
            Console.Write("Please enter a Start point? ");
            StartPt = Console.ReadLine();

            Console.Write("Please enter an End point? ");
            EndPt = Console.ReadLine();

            //StartPt = "C1"; EndPt = "F7";
            InitChessAPIConfig();
            _config.URI_1 = _config.URI_1.Replace("[StartPt]", StartPt.ToUpper()).Replace("[EndPt]", EndPt.ToUpper());

        }


        private static void InitChessAPIConfig()
        {
            _config.URI_1 = "https://knightpath.azurewebsites.net/api/knightpath?source=[StartPt]&target=[EndPt]";
            _config.URI_2 = "https://knightpath.azurewebsites.net/api/knightpath?operationId=";
        }


    }
}
