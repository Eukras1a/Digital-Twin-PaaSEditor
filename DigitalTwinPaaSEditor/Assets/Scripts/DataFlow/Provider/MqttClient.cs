using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace DataFlow.Provider
{
    public class MqttClient : MonoBehaviour
    {
        // private static readonly string MQTTURI = "172.20.1.110";
        public string MQTTURI = "10.17.7.103";
        public string TestMsg= "{\"type\":\"get\"}";

        public int MQTTPort = 1883;
        public string MQTTUser = "";
        public string MQTTPassword = "";
        public string Topic = "test";

        public Queue<string> RecvQueue = new Queue<string>();
        [SerializeField]
        private UnityEvent<string> OnRecv;
        [SerializeField]
        public UnityEngine.UI.Button BtnConnect;

        // Start is called before the first frame update
        void Start()
        {
            BtnConnect.onClick.AddListener(InitMQTT);
        }
    
        /// <summary>
        /// 状态
        /// </summary>
        public enum MQTTStatus
        {
            Empty = 0,

            /// <summary>
            /// 连接中
            /// </summary>
            Connecting = 1,

            /// <summary>
            /// 连接成功
            /// </summary>
            Connected = 2,

            /// <summary>
            /// 连接失败
            /// </summary>
            Failed = 3,
        }
    
        /// <summary>
        /// 订阅消息
        /// </summary>
        public void SubscribeAsync(MqttClientSubscribeOptions options)
        {
            _Client.SubscribeAsync(options);
        }

        /// <summary>
        /// 取消订阅消息
        /// </summary>
        public void UnsubscribeAsync(MqttClientUnsubscribeOptions options)
        { 
            _Client.UnsubscribeAsync(options);
        }
    
        /// <summary>
        /// 重新连接
        /// </summary>
        private void Reconnect()
        {
            Debug.Log("重新连接");
            // Task.Run(delegate ()
            // {
            //     _Status = MQTTStatus.Connecting;
            //     _Client.ReconnectAsync();
            // });
        }

        /// <summary>
        /// 新消息事件
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            if (arg.ApplicationMessage.PayloadSegment.Array == null) 
                return Task.CompletedTask;
            
            var str = System.Text.Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment.Array);
            Debug.Log($"Receive:" + DateTime.Now.ToString("HH:mm:ss.fff")  + " - " + arg.ApplicationMessage.Topic + "," + str);
            lock (RecvQueue)
            {
                RecvQueue.Enqueue(str);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 重连间隔时间(s)
        /// </summary>
        private static readonly float ReconnectGapTime = 10;

        /// <summary>
        /// 客户端
        /// </summary>
        private IMqttClient _Client;

        /// <summary>
        /// 失败次数
        /// </summary>
        private int _FailCount;

        /// <summary>
        /// 当前状态
        /// </summary>
        private MQTTStatus _Status;

        /// <summary>
        /// 等待时间
        /// </summary>
        private float _WaitTime;
        private string _ClientID;
        /// <summary>
        /// 连接断开事件
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task Client_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            Debug.Log("MQTT连接断开:" + arg.Reason);
            // Loom.QueueOnMainThread(() =>
            // {
            // });
            _Status = MQTTStatus.Failed;
            _FailCount++;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 连接成功事件
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task Client_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            _Status = MQTTStatus.Connected;
            Debug.Log("MQTT连接成功");            
            _FailCount = 0;

            SubscribeAsync(new MqttClientSubscribeOptions
            {
                TopicFilters = new List<MqttTopicFilter>()
                {
                    new()
                    {
                        Topic = Topic
                    }
                }
            });
            return Task.CompletedTask;
        }
    
        /// <summary>
        /// 初始化MQTT信息
        /// </summary>
        private void InitMQTT()
        {
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                .WithTcpServer(MQTTURI, MQTTPort) // 要访问的mqtt服务端的 ip 和 端口号
                .WithCredentials(MQTTUser, MQTTPassword) // 要访问的mqtt服务端的用户名和密码
                .WithClientId(_ClientID) // 设置客户端id
                .WithCleanSession()
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500) //选择mqtt版本，这里选用的mqtt5
                .WithTlsOptions(new MqttClientTlsOptions()
                {
                        UseTls = false // 是否使用 tls加密
                });
            MqttClientOptions clientOptions = builder.Build();
            _Client = new MqttFactory().CreateMqttClient();

            _Client.ConnectedAsync += Client_ConnectedAsync; // 客户端连接成功事件
            _Client.DisconnectedAsync += Client_DisconnectedAsync; // 客户端连接关闭事件
            _Client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync; ; // 收到消息事件

            _Status = MQTTStatus.Connecting;
            _Client.ConnectAsync(clientOptions);
        }

        // Update is called once per frame
        void Update()
        {
            if (!RecvQueue.Any())
                return;

            var msg = "";
            lock (RecvQueue)
            {
                if (RecvQueue.Any())
                {
                    msg = RecvQueue.Dequeue();
                }
            }

            if (!string.IsNullOrEmpty(msg))
            {
                
                
                OnRecv?.Invoke(msg);
            }
        }

        [UnityEngine.ContextMenu("Test")]
        public void Test()
        {
            // UnityWebRequest webRequest = UnityWebRequest.Get("http://10.17.7.103:13141/content/get");
            // webRequest.SendWebRequest();
            Debug.Log($"{DateTime.Now:HH:mm:ss.fff},Send Message");
            _Client.PublishAsync(new MqttApplicationMessage
            {
                Topic = Topic,
                Payload = System.Text.Encoding.UTF8.GetBytes(TestMsg),
            });
        }
    
        public void PublishAsync(string msg)
        {
            // Debug.Log($"{DateTime.Now:HH:mm:ss.fff},Send Message");
            _Client.PublishAsync(new MqttApplicationMessage
            {
                Topic = Topic,
                PayloadSegment = System.Text.Encoding.UTF8.GetBytes(msg),
            });
        }
    }
}
