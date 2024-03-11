using UnityEngine;

public class DoRotation : MonoBehaviour
{
    [SerializeField] private float _speed = 90;
    [SerializeField] Vector3 _axis = Vector3.back;
    private Transform _transform;
    private float _radianSpeed = 1.5f;

    private void Awake()
    {
        _transform = transform;
        _radianSpeed = _speed / 180 * Mathf.PI;
    }

    private void OnEnable()
    {
        _transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        _transform.Rotate(_axis, _radianSpeed);
    }
}
