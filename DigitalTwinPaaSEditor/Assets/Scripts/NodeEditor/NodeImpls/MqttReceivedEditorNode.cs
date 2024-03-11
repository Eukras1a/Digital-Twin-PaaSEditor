using System;
using System.Collections.Generic;
using MeadowGames.UINodeConnect4;
using Newtonsoft.Json;
using UnityEngine;

namespace NodeEditor.NodeImpls
{
    public class MqttReceivedEditorNode : BaseNode, IEditorNode
    {
        [SerializeField] private Port _payloadPort;
        
        public void OnIn(Port port, object arg)
        {
            if (arg is not Dictionary<string, object> args)
                return;
            
            if (!args.ContainsKey("Payload"))
                throw new Exception("Payload is not found in args");
            
            var payload = args["Payload"] as byte[];
            if (payload == null)
                throw new Exception("Payload is not byte[]");

            var payloadStr = System.Text.Encoding.UTF8.GetString(payload);
            Debug.Log(payloadStr);

            if (!TryGetTargetPortByOutPort(_payloadPort, out var targets))
                return;
            try
            {
                var payloadData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(payloadStr);

                foreach (var target in targets)
                {
                    foreach (var line in payloadData)
                    {
                        target.EditorNode.OnIn(target.Port, line);
                    }
                }
            }
            catch (Exception e)
            {
                var payloadData = JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadStr);
                foreach (var target in targets)
                {
                    foreach (var line in payloadData)
                    {
                        target.EditorNode.OnIn(target.Port, payloadData);
                    }
                }
            }
            
        }
        
        public void OnOut()
        {
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>();
        }

        public void Load(Dictionary<string, object> data)
        {
        }
    }
}