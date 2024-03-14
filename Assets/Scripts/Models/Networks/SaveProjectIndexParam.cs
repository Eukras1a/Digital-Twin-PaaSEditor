using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.Networks
{
    public class SaveProjectIndexRequest
    {
        [JsonProperty("project_id")] public string ProjectId;
        [JsonProperty("access_token")] public string AccessToken;
        [JsonProperty("files")] public Dictionary<string,string> Files;
    }
    
    public class SaveProjectIndexResponse
    {
        [JsonProperty("project_id")] public string ProjectId;
        [JsonProperty("project_version")] public int ProjectVersion;
        [JsonProperty("project_name")] public string ProjectName;
        [JsonProperty("upload_token")] public MinIOToken UploadToken;
        [JsonProperty("upload_files")] public Dictionary<string, string> NeedUpdateFiles;
    }
}