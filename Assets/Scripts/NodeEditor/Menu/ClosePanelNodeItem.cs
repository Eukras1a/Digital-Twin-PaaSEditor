using Battlehub.RTCommon;
using MeadowGames.UINodeConnect4;
using MeadowGames.UINodeConnect4.UICContextMenu;
using UnityEngine;
using UnityEngine.UI;
using World;
using World.Drivers;

namespace NodeEditor.Menu
{
    public class ClosePanelNodeItem  : ContextItem
    {
        Button _button;

        public void OnCloseClick()
        {
            var nodeEditorContext = IOC.Resolve<NodeEditorContext>();
            nodeEditorContext.ClearGraph();
            nodeEditorContext.Enable = false;
        }
        
        protected override void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
        }
        
        void OnEnable()
        {
            _button.onClick.AddListener(OnCloseClick);
        }

        void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}