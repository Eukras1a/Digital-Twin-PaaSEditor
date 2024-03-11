using UnityEngine;
using UnityEngine.Serialization;

namespace World.DriverHelper
{
    public class LookAtTo : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;
        [FormerlySerializedAs("BeginAngle")] [SerializeField]
        private Vector3 OffsetAngle;
        // Start is called before the first frame update
        void Start()
        {
            OffsetAngle = transform.eulerAngles;
        }

        // Update is called once per frame
        void Update()
        {
            if (_target != null)
            {
                transform.LookAt(_target);
                transform.localEulerAngles += OffsetAngle;
            }
        }
    }
}