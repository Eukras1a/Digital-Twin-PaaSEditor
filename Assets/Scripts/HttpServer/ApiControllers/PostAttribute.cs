namespace HttpServer.ApiControllers
{
    public class PostAttribute : RouteAttribute
    {
        public PostAttribute(string route) : base("POST", route)
        {
            Route = route;
        }
        
        public string Route { get; }
    }
}