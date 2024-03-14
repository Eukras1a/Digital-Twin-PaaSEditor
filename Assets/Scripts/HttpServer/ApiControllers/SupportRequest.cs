using System.Reflection;

namespace HttpServer.ApiControllers
{
    internal class SupportRequest
    {
        public string Method { get; set; }
        public MethodInfo MethodCallback { get; set; }
        public IApiController ApiController { get; set; }
    }
}