using System.Collections.Generic;
using MeadowGames.UINodeConnect4;
using TMPro;
using UnityEngine;

namespace NodeEditor.NodeImpls
{
    public class DataFilterEditorNode : BaseNode, IEditorNode
    {
        [SerializeField] private Port _outPort;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_InputField _inputFilter;
        public const string FilterKey = "Filter";
        public const string FieldKey = "Field";
        
        public void OnIn(Port port, object arg)
        {
            if (arg is not Dictionary<string, object> args)
                return;
            var field = _inputField.text;
            if (!args.TryGetValue(field, out var value))
            {
                return;
            }
            
            if (value.ToString() != _inputFilter.text)
            {
                return;
            }
            
            if (!TryGetTargetPortByOutPort(_outPort, out var targets))
                return;
            
            foreach (var target in targets)
            {
                target.EditorNode.OnIn(target.Port, arg);
            }
        }

        public void OnOut()
        {
        }

        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>()
            {
                {FilterKey, _inputFilter.text},
                {FieldKey, _inputField.text}
            };
        }
        
        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(FilterKey, out var filter))
            {
                _inputFilter.text = filter.ToString();
            }
            if (data.TryGetValue(FieldKey, out var field))
            {
                _inputField.text = field.ToString();
            }
        }
    }
}