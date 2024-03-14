using UnityEngine;
using World.Drivers;

namespace World.DriverHelper
{
    public class PhysicalFollowTargetWithLimit : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        Rigidbody _rigidbody;

        [SerializeField]
        private Axis _direction;
        [SerializeField]
        private float _maxLimit = 1;
        [SerializeField]
        private float _minLimit = 0;
        
        private Transform _transform;
        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            if (_target == null)
            {
                return;
            }
            
            // _transform.Translate(_target.position - transform.position, Space.World);
            _transform.position = _target.position;
            if (_direction == Axis.X)
            {
            }
            else if (_direction == Axis.Y)
            {
            }
            else if (_direction == Axis.Z)
            {
                var localPosition = _transform.localPosition;
                if (_transform.localPosition.z > _maxLimit)
                {
                    localPosition = new Vector3(localPosition.x, localPosition.y, _maxLimit);
                    _transform.localPosition = localPosition;
                }
                else if (_transform.localPosition.z < _minLimit)
                {
                    localPosition = new Vector3(localPosition.x, localPosition.y, _minLimit);
                    _transform.localPosition = localPosition;
                }
            }
            
        }
    }
}