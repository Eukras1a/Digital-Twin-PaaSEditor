using HttpServer.ApiControllers;
using Newtonsoft.Json;
using UnityEngine;
using World;

namespace HttpServer.ControllerImpls
{
    [Route("/api/camera")]
    public class NetCameraController : IApiController
    {
        [Post("move")]
        public void Move(string body)
        {
            var moveRequestTemplate = JsonConvert.DeserializeObject<MoveRequestTemplate>(body);
            if (WorldContext.Instance == null)
            {
                Debug.LogError("WorldContext.Instance == null");
                return;
            }
            
            if (WorldContext.Instance.MainCamera == null)
            {
                Debug.LogError("WorldContext.Instance.MainCamera == null");
                return;
            }

            var position = ParseVector3(moveRequestTemplate.Position);
            var angle = ParseVector3(moveRequestTemplate.Angle);
            
            WorldContext.Instance.DoMainThread(() =>
            {
                WorldContext.Instance.MainCamera.Move(position, angle, moveRequestTemplate.Time);
            });
        }
    
        public Vector3 ParseVector3(string position)
        {
            var split = position.Split(',');
            return new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
        }
        
        
        public class MoveRequestTemplate
        {
            [JsonProperty("p")]
            public string Position;
            [JsonProperty("a")]
            public string Angle;
            [JsonProperty("t")]
            public float Time;
        }
    }
}