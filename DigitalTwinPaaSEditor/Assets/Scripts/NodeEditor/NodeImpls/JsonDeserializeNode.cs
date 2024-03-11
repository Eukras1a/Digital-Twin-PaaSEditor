using System;
using System.Collections.Generic;
using MeadowGames.UINodeConnect4;
using Newtonsoft.Json;
using UnityEngine;

namespace NodeEditor.NodeImpls
{
    [RequireComponent(typeof(Node))]
    public class JsonDeserializeNode : BaseNode, IEditorNode
    {
        private Node _node;

        [SerializeField] private Port _portIn;
        [SerializeField] private Port _portOut;

        public void Awake()
        {
            _node = GetComponent<Node>();
        }

        public void OnIn(Port port, object inArg)
        {
            if (inArg is not string content)
                return;
            
            try
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                if (!TryGetTargetPortByOutPort(_portOut, out var targets))
                    return;

                foreach (var target in targets)
                {
                    target.EditorNode.OnIn(target.Port, data);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void OnOut()
        {
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>()
            {
            };
        }

        public void Load(Dictionary<string, object> data)
        {
        }
    }
}