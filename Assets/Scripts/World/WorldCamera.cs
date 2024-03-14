using System;
using DG.Tweening;
using UnityEngine;

namespace World
{
    public class WorldCamera : MonoBehaviour
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public void Start()
        {
            name = gameObject.GetInstanceID().ToString();
            WorldContext.Instance.SetMainCamera(this);
        }

        public void Move(Vector3 position, Vector3 angle, float time)
        {
            _transform.DOMove(position, time).SetEase(Ease.Linear);
            _transform.DORotate(angle, time).SetEase(Ease.Linear);
        }
    }
}