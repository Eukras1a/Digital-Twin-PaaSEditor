using System.Net;
using System.Net.Sockets;
using HttpServer.ApiControllers;
using NetCoreServer;
using UnityEngine;

namespace HttpServer
{
    class HttpTraceServer : NetCoreServer.HttpServer
    {
        public HttpTraceServer(IPAddress address, int port) : base(address, port) {}
        ApiContext _apiContext = new ApiContext();
        protected override TcpSession CreateSession() { return new HttpTraceSession(this, _apiContext); }

        protected override void OnError(SocketError error)
        {
            Debug.LogError($"Server caught an error with code {error}");
        }
        
        public void Initialize()
        {
            _apiContext.Initialize();
        }
    }
}