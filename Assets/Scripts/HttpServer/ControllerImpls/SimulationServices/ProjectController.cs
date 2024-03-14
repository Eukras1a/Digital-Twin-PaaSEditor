using System.Collections.Generic;
using HttpServer.ApiControllers;
using Models.Networks;
using Newtonsoft.Json;

namespace HttpServer.ControllerImpls.SimulationServices
{
    [Route("/minio")]
    public class ProjectController : IApiController
    {
        [Post("/getObjectByToken")]
        public GetProjectIndexResponse GetProjectIndex(string body)
        {
            var requestArg = JsonConvert.DeserializeObject<GetProjectIndexRequest>(body);
            return new GetProjectIndexResponse
            {
                ProjectId = requestArg.ProjectId,
                Files = new Dictionary<string, string>(),
                DownloadToken = new MinIOToken(),
                ProjectName = $"Project_${requestArg.ProjectId}",
                ProjectVersion = 100
            };
        }

        public static HashSet<string> FileStore = new HashSet<string>();
        public static Dictionary<string, int> ProjectVersion = new Dictionary<string, int>();

        [Post("/compareObject")]
        public SaveProjectIndexResponse SaveProjectIndex(string body)
        {
            var requestArg = JsonConvert.DeserializeObject<SaveProjectIndexRequest>(body);

            var needFiles = new Dictionary<string, string>();
            foreach (var file in requestArg.Files)
            {
                if (FileStore.Contains(file.Value))
                    continue;
                FileStore.Add(file.Value);
                needFiles.Add(file.Key, file.Value);
            }

            ProjectVersion.TryGetValue(requestArg.ProjectId, out var version);
            if (needFiles.Count != 0)
            {
                version += 1;
                ProjectVersion[requestArg.ProjectId] = version;
            }
            
            return new SaveProjectIndexResponse
            {
                ProjectId = requestArg.ProjectId,
                NeedUpdateFiles = needFiles,
                UploadToken = new MinIOToken(),
                ProjectName = $"Project_${requestArg.ProjectId}",
                ProjectVersion = version
            };
        }
    }
}