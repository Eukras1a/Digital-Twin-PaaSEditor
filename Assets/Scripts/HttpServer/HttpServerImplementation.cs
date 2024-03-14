using System.Net;

namespace HttpServer
{
    public class HttpServerImplementation : IHttpServer
    {
        private readonly HttpTraceServer _server;

        public HttpServerImplementation(IPAddress address, int port)
        {
            _server = new HttpTraceServer(address, port);
            _server.OptionReuseAddress = true;
            _server.Initialize();
        }
    
        public void Start()
        {
            _server.Start();
        }
    
        public void Stop()
        {
            _server.Stop();
        }
    
        public void Restart()
        {
            _server.Restart();
        }
    
    
    }
}