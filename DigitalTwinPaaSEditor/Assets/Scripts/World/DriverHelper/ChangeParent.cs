using UnityEngine;
using UnityEngine.Animations;

namespace World.DriverHelper
{
    public class ChangeParent : MonoBehaviour
    {
        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private Axis _axis;
        
        [SerializeField]
        private float _targetValue = 0.5f;
        [SerializeField]
        private CompareSymbol _symbol = CompareSymbol.LessThan;
        private void LateUpdate()
        {
            bool isMatch = false;
            if (_axis == Axis.X)
            {
                if (_symbol == CompareSymbol.LessThan)
                {
                    isMatch = transform.localPosition.x < _targetValue;
                }
                else if (_symbol == CompareSymbol.GreaterThan)
                {
                    isMatch = transform.localPosition.x > _targetValue;
                }
            }
            else if (_axis == Axis.Y)
            {
                if (_symbol == CompareSymbol.LessThan)
                {
                    isMatch = transform.localPosition.y < _targetValue;
                }
                else if (_symbol == CompareSymbol.GreaterThan)
                {
                    isMatch = transform.localPosition.y > _targetValue;
                }
            }
            else if (_axis == Axis.Z)
            {
                if (_symbol == CompareSymbol.LessThan)
                {
                    isMatch = transform.localPosition.z < _targetValue;
                }
                else if (_symbol == CompareSymbol.GreaterThan)
                {
                    isMatch = transform.localPosition.z > _targetValue;
                }
            }
            
            if(isMatch)
            {
                transform.SetParent(_parent);
                enabled = false;
            }
        }
    }
}