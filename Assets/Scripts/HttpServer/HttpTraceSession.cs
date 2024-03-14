using System;
using System.Net.Sockets;
using System.Text;
using HttpServer.ApiControllers;
using NetCoreServer;
using Newtonsoft.Json;
using UnityEngine;

namespace HttpServer
{
    class HttpTraceSession : HttpSession
    {
        private readonly ApiContext _apiContext;
        public HttpTraceSession(NetCoreServer.HttpServer server, ApiContext apiContext) : base(server)
        {
            _apiContext = apiContext;
        }

        protected override void OnReceivedRequest(HttpRequest request)
        {
            try
            {
                Debug.Log($"Server Request:{request.Url}:{request.Method},{request.Body}");
                var handleResult = _apiContext.HandleRequest(request.Url, request.Method, request.Body);
                if (handleResult.Code == 200)
                {
                    Debug.Log($"Server Response:[{handleResult.Code}],{JsonConvert.SerializeObject(handleResult.Body)}");
                    SendResponseAsync(Response.MakeGetResponse(JsonConvert.SerializeObject(new DataResponseBody
                    {
                        Code = 1,
                        Data = handleResult.Body
                    })));
                }
                else
                {
                    Debug.LogError($"Server Response:[{handleResult.Code}],{JsonConvert.SerializeObject(handleResult.Body)}");
                    SendResponseAsync(Response.MakeErrorResponse(handleResult.Code, JsonConvert.SerializeObject(new DataResponseBody
                    {
                        Code = handleResult.Code,
                        Data = handleResult.Body
                    })));
                }
            }
            catch (Exception e)
            {
                SendResponseBodyAsync(JsonConvert.SerializeObject(new ErrorResponseBody
                {
                    Code = 1000,
                    Msg = e.Message
                }));
            }
        }

        class DataResponseBody
        {
            public int Code { get; set; }
            public object Data { get; set; }
        }
        
        class ErrorResponseBody
        {
            public int Code { get; set; }
            public string Msg { get; set; }
        }
        
        class NotDataResponseBody
        {
            public int Code { get; set; }
        }
        
        private void SendSuccessResponseAsync()
        {
            SendResponseBodyAsync(JsonConvert.SerializeObject(new NotDataResponseBody
            {
                Code = 1
            }));
        }

        protected override void OnReceivedRequestError(HttpRequest request, string error)
        {
            Console.WriteLine($"Request error: {error}");
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Session caught an error with code {error}");
        }
    }
}