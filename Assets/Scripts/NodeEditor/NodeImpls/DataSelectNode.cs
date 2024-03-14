using System.Collections.Generic;
using System.Linq;
using MeadowGames.UINodeConnect4;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

namespace NodeEditor.NodeImpls
{
    [RequireComponent(typeof(Node))]
    public class DataSelectNode : BaseNode, IEditorNode
    {
        private Node _node;
        [SerializeField]
        private Transform _outPortParent;
        [SerializeField]
        private Port _outPortTemp;
        [SerializeField]
        private Port _portIn;

        public List<string> _outKeys = new();
        public const string OutKeys = "OutKeys";
        public void Awake()
        {
            _node = GetComponent<Node>();
        }

        public void OnIn(Port port, object inArg)
        {
            if (inArg is not Dictionary<string, object> args)
                return;
            foreach (var arg in args)
            {
                TryAddOutPort(arg.Key);
            }

            foreach (var arg in args)
            {
                if (!TryGetOutPort(arg.Key, out Port outPort))
                    continue;
                if (!TryGetTargetPortByOutPort(outPort, out var targets))
                    continue;
                foreach (var target in targets)
                {
                    target.EditorNode.OnIn(target.Port, arg.Value);
                }
            }
        }

        public void OnOut()
        {
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>()
            {
                {OutKeys, _outKeys}
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            _outKeys.Clear();
            if (data.TryGetValue(OutKeys, out var keys))
            {
                var outKeys = (JArray)keys;
                foreach (var outKey in outKeys)
                {
                    var key = outKey.Value<string>();
                    _outKeys.Add(key);
                    TryAddOutPort(key);
                }
            }
        }

        private bool TryGetOutPort(string portId, out Port port)
        {
            port = _node.ports.Find(p => p.ID == portId);
            return port != null;
        }

        public void TryAddOutPort(string portId)
        {
            if (_node.ports.Any(nodePort => nodePort.ID == portId))
                return;
            _outKeys.Add(portId);
            var portObj = Instantiate(_outPortTemp.gameObject, _outPortParent, false);
            portObj.SetActive(true);
            var port = portObj.GetComponent<Port>();
            port.ID = portId;
            var text = port.GetComponentInChildren<TMP_Text>();
            if (text != null)
            {
                text.text = portId;
            }
        }
    }
}