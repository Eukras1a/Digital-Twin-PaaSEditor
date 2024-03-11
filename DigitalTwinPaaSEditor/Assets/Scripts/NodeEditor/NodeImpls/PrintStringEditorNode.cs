using System.Collections.Generic;
using MeadowGames.UINodeConnect4;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace NodeEditor.NodeImpls
{
    public class PrintStringEditorNode : BaseNode, IEditorNode
    {
        private const string BufferKey = "Content";
        [SerializeField] private Port _contentPort;
     
        [SerializeField]
        TMP_Text _text;

        public void Start()
        {
            _text.text = "";
        }

        public void OnIn(Port port, object arg)
        {
            var str = JsonConvert.SerializeObject(arg);
            _text.text = str;
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