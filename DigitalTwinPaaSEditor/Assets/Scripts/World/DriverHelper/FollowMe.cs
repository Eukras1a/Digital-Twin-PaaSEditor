using UnityEngine;
using UnityEngine.Serialization;

namespace World.DriverHelper
{
    public class FollowMe : MonoBehaviour
    {
        [FormerlySerializedAs("_target")] [SerializeField]
        public Transform Target;
        [FormerlySerializedAs("rotation")] [SerializeField]
        public bool EnableRotation = false;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Target != null)
            {
                Target.position = transform.position;
            
                if (EnableRotation)
                {
                    Target.rotation = transform.rotation;
                }
            }
        }
    }
}