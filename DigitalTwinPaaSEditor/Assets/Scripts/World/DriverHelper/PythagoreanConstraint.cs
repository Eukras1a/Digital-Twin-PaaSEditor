using UnityEngine;

namespace World.DriverHelper
{
    public class PythagoreanConstraint : MonoBehaviour
    {
        [SerializeField]
        private Transform _source;
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private bool x;
        [SerializeField]
        private bool y;
        [SerializeField]
        private bool z;

        private float _distance;
        [SerializeField]
        private Vector3 _offsetAngle;


        // Start is called before the first frame update
        void Start()
        {
            var targetPosition = _target.localPosition;
            _distance = Vector3.Distance(targetPosition, _source.localPosition);
            _targetZ = targetPosition.z;
        }

        private float _targetZ;
        // Update is called once per frame
        void Update()
        {
            var localPosition = _source.localPosition;
            var offsetZ = _targetZ - localPosition.z;
            var f = Mathf.Acos(offsetZ / _distance);
            _target.localPosition = new Vector3(_distance * Mathf.Sin(f) + localPosition.x, 0f, _targetZ);
            _source.localEulerAngles = new Vector3(0f, f * Mathf.Rad2Deg, 0f) + _offsetAngle;
        }
    }
}
