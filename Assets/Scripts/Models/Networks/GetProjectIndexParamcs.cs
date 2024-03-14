using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.Networks
{
    public class GetProjectIndexRequest
    {
        [JsonProperty("project_id")]
        public string ProjectId;
        [JsonProperty("access_token")]
        public string AccessToken;
    }

    public class GetProjectIndexResponse
    {
        [JsonProperty("project_id")]
        public string ProjectId;
        [JsonProperty("project_version")]
        public int ProjectVersion;
        [JsonProperty("project_name")]
        public string ProjectName;
        [JsonProperty("download_token")]
        public MinIOToken DownloadToken;
        [JsonProperty("files")]
        public Dictionary<string, string> Files;
    }
}