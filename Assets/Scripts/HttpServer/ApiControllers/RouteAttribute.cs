using System;

namespace HttpServer.ApiControllers
{
    public class RouteAttribute : Attribute
    {
        public string Path { get; }
        public string Method { get; }
        public RouteAttribute(string method, string path)
        {
            Method = method;
            Path = path;
        }
        
        public RouteAttribute(string path)
        {
            Path = path;
        }
    }
}