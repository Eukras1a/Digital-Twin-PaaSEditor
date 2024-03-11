namespace Projects
{
    public interface IProjectInfo
    {
        string Id { get; set; }
        string Name { get; set; }
    }

    public class ProjectInfo : IProjectInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}