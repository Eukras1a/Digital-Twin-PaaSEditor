using Newtonsoft.Json;

namespace Models.Networks
{
    public class MinIOToken
    {
        [JsonProperty("secretKey")]
        public string SecretKey;
        [JsonProperty("accessKey")]
        public string AccessKey;
        [JsonProperty("sessionToken")]
        public string SessionToken;
    }
}