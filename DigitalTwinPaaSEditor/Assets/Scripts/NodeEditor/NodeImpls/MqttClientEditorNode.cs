using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battlehub;
using Battlehub.RTCommon;
using MeadowGames.UINodeConnect4;
using MQTTnet;
using MQTTnet.Client;
using TMPro;
using UnityEngine;

namespace NodeEditor.NodeImpls
{
    public class MqttClientEditorNode : BaseNode, IEditorNode
    {
        [SerializeField] private TMP_InputField _mqttHost;
        [SerializeField] private TMP_InputField _mqttPort;
        [SerializeField] private TMP_InputField _mqttUser;
        [SerializeField] private TMP_InputField _mqttPassword;
        [SerializeField] private TMP_InputField _mqttTopic;
        [SerializeField] private Port _portReceivced;
        [SerializeField] private Port _portConnected;
        [SerializeField] private Port _portDisconnected;
        [SerializeField] public UnityEngine.UI.Button _btnConnect;
    
        private IMqttClient _mqttClient;
        private bool _isConnected;
    
        // Start is called before the first frame update
        void Start()
        {
            _btnConnect.onClick.AddListener(OnConnectedClick);
        }

        private void OnDisable()
        {
            if (_mqttClient != null)
            {
                _mqttClient.DisconnectAsync();
                _mqttClient.Dispose();
            }
        }

        private void OnEnable()
        {
            SetButtonText("Connect");
        }

        private void SetButtonText(string key)
        {
            var localization = IOC.Resolve<ILocalization>();
            var txtButton = _btnConnect.GetComponent<TMP_Text>();
            if (localization != null && txtButton != null)
            {
                txtButton.text = localization.GetString(key, null);
            }
        }

        private void OnConnectedClick()
        {
            InitMqttClient();
        }

        private void InitMqttClient()
        {
            var clientID = "PASS_CLIENT_" + Guid.NewGuid();
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                .WithTcpServer(_mqttHost.text, int.Parse(_mqttPort.text))
                .WithCredentials(_mqttUser.text, _mqttPassword.text)
                .WithClientId(clientID)
                .WithCleanSession()
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                .WithTlsOptions(new MqttClientTlsOptions
                {
                    UseTls = false // 是否使用 tls加密
                });
            MqttClientOptions clientOptions = builder.Build();
            _mqttClient = new MqttFactory().CreateMqttClient();
            _mqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;
            _mqttClient.ConnectedAsync += OnConnectedAsync;
            _mqttClient.DisconnectedAsync += OnDisconnectedAsync;
            _mqttClient.ConnectAsync(clientOptions);
        }

        private void DisposeMqttClient()
        {
            if (_mqttClient == null)
                return;
            _mqttClient.Dispose();
        }

        private Task OnDisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            _isConnected = false;
        
            Debug.Log("MQTT连接断开");
            lock (EventQueue)
            {
                EventQueue.Enqueue(new MQTTAsyncEvent
                {
                    eventType = MQTTEventType.Disconnected
                } );
            }
            return Task.CompletedTask;
        }

        private Task OnConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            _isConnected = true;
        
            var topicFilterBuilder = new MqttTopicFilterBuilder();
            topicFilterBuilder.WithTopic(_mqttTopic.text);
            var mqttTopicFilter = topicFilterBuilder.Build();
            _mqttClient.SubscribeAsync(mqttTopicFilter);
            Debug.Log("MQTT连接成功");
            lock (EventQueue)
            {
                EventQueue.Enqueue(new MQTTAsyncEvent
                {
                    eventType = MQTTEventType.Connected
                } );
            }
            return Task.CompletedTask;
        }

        public Queue<MQTTAsyncEvent> EventQueue = new();

        private Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            if (arg.ApplicationMessage.PayloadSegment.Array == null)
                return Task.CompletedTask;

            lock (EventQueue)
            {
                EventQueue.Enqueue(new MQTTAsyncEvent
                {
                    eventType = MQTTEventType.Message,
                    Args = arg
                });
            }

            return Task.CompletedTask;
        }

        // Update is called once per frame
        void Update()
        {
            if (!EventQueue.Any())
                return;

            MQTTAsyncEvent @event = null;
            lock (EventQueue)
            {
                if (EventQueue.Any())
                {
                    @event = EventQueue.Dequeue();
                }
            }
        
            if (@event == null) 
                return;

            switch (@event.eventType)
            {
                case MQTTEventType.Message:
                    HandleOnReceiveMessage(@event);
                    break;
                case MQTTEventType.Connected:
                    HandleOnConnected();
                    break;
                case MQTTEventType.Disconnected:
                    HandleOnDisconnected();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
        }

        private void HandleOnConnected()
        {
            if (!TryGetTargetPortByOutPort(_portConnected, out var targets))
                return;

            foreach (var target in targets)
            {
                target.EditorNode.OnIn(target.Port, null);
            }
        }
    
        private void HandleOnDisconnected()
        {
            if (!TryGetTargetPortByOutPort(_portDisconnected, out var targets))
                return;
        
            foreach (var target in targets)
            {
                target.EditorNode.OnIn(target.Port, null);
            }
        }

        private void HandleOnReceiveMessage(MQTTAsyncEvent @event)
        {
            if (!TryGetTargetPortByOutPort(_portReceivced, out var targets))
                return;
            var arg = new Dictionary<string, object>()
            {
                { "Payload", @event.Args.ApplicationMessage.PayloadSegment.Array },
                { "Topic", @event.Args.ApplicationMessage.Topic }
            };
            foreach (var target in targets)
            {
                target.EditorNode.OnIn(target.Port, arg);
            }
        }

        public void OnIn(Port port, object arg)
        {
        }

        public void OnOut()
        {
        }

        private const string HostKey = "Host";
        private const string PortKey = "Port";
        private const string UserNameKey = "UserName";
        private const string PasswordKey = "Password";
        private const string TopicKey = "Topic";
    

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>()
            {
                { HostKey, _mqttHost.text },
                { PortKey, _mqttPort.text },
                { UserNameKey, _mqttUser.text },
                { PasswordKey, _mqttPassword.text },
                { TopicKey, _mqttTopic.text },
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(HostKey, out var host))
            {
                _mqttHost.text = host.ToString();
            }
            if (data.TryGetValue(PortKey, out var port))
            {
                _mqttPort.text = port.ToString();
            }
            if (data.TryGetValue(UserNameKey, out var userName))
            {
                _mqttUser.text = userName.ToString();
            }
            if (data.TryGetValue(PasswordKey, out var password))
            {
                _mqttPassword.text = password.ToString();
            }
            if (data.TryGetValue(TopicKey, out var topic))
            {
                _mqttTopic.text = topic.ToString();
            }
        }
    }

    public class MQTTAsyncEvent
    {
        public MQTTEventType eventType;
        public MqttApplicationMessageReceivedEventArgs Args;
    }

    public enum MQTTEventType
    {
        Message,
        Connected,
        Disconnected
    }
}