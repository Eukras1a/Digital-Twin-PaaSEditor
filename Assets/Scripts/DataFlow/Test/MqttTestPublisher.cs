using System;
using System.Collections.Generic;
using DataFlow.Provider;
using UnityEngine;

namespace DataFlow.Test
{
    public class MqttTestPublisher : MonoBehaviour
    {
        [SerializeField]
        private MqttClient _client;
    
        private int _index;
        private float _startTime;
        [SerializeField]
        public bool _runOnAwake = true;
        [SerializeField]
        private float _awakeDelay = 3f;
        [SerializeField]
        public bool _isLoop = true;
        [SerializeField]
        private float _loopInterval = 10f;
    
        public List<PublishMessage> Messages = new();
        public void Start()
        {
            if (_runOnAwake)
            {
                _index = 0;
                _startTime = Time.time + _awakeDelay;
                _isRunning = true;
            }
        }

        private bool _isRunning = false;

        public void Update()
        {
            if (!_isRunning)
            {
                return;
            }
        
            while (Messages[_index].Time < Time.time - _startTime)
            {
                _client.PublishAsync(Messages[_index].Message);
                _index++;
                if (_index >= Messages.Count)
                {
                    break;
                }
            }
        
            if (_index >= Messages.Count)
            {
                _startTime = Time.time + _loopInterval;
                _index = 0;
                if (!_isLoop)
                {
                    _isRunning = false;
                }
            }
        }
    }

    [Serializable]
    public class PublishMessage
    {
        public float Time;
        public string Message;
    }
}