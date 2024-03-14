using System.Collections.Generic;
using Battlehub.RTCommon;
using MeadowGames.UINodeConnect4;
using MeadowGames.UINodeConnect4.UICContextMenu;
using UnityEngine.UI;

namespace NodeEditor.Menu
{
    public class AddNodeItem  : ContextItem
    {
        Button _button;
        public Node node;

        public void AddNode()
        {
            var nodeEditorContext = IOC.Resolve<NodeEditorContext>();
            var newNode = nodeEditorContext.AddNode(node, node.transform.localPosition);
            ContextMenu.GraphManager.UnselectAllElements();
            newNode.Select();
        }

        protected override void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
        }

        void OnEnable()
        {
            _button.onClick.AddListener(AddNode);
        }

        void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}