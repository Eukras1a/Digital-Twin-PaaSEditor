namespace HttpServer
{
    public interface IHttpServer
    {
        void Start();
        void Stop();
        void Restart();
    }
}