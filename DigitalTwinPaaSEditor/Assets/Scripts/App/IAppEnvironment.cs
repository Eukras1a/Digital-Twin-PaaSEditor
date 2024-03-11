using Projects;

namespace App
{
    public interface IAppEnvironment
    {
        public IProjectInfo CurProjectInfo { get; }
        public string AccessToken { get; }
    }

    public class AppEnvironment : IAppEnvironment
    {
        public IProjectInfo CurProjectInfo { get; set; }
        public string AccessToken { get; set; }
    }
}