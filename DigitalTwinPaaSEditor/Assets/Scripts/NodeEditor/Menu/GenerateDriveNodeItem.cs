using Battlehub.RTCommon;
using MeadowGames.UINodeConnect4;
using MeadowGames.UINodeConnect4.UICContextMenu;
using NodeEditor.NodeImpls;
using UnityEngine;
using UnityEngine.UI;
using World.Drivers;

namespace NodeEditor.Menu
{
    public class GenerateDriveNodeItem  : ContextItem
    {
        Button _button;
        [SerializeField]
        private Node _driveNode;

        public void GenerateDriveNodes()
        {
            var nodeEditorContext = IOC.Resolve<NodeEditorContext>();
            var device = nodeEditorContext.CurDevice;
            if (device == null)
                return;
            
            var drivers = device.GetComponentsInChildren<IDriver>();
            foreach (var nodeCtrl in drivers)
            {
                var newNode = nodeEditorContext.AddNode(_driveNode, _driveNode.transform.position);
                newNode.ID = $"{device.Id}_{nodeCtrl.NodeId}";
                var driveEditorNode = newNode.GetComponent<DriveEditorNode>();
                driveEditorNode.SetNodeCtrl(nodeCtrl);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
        }

        void OnEnable()
        {
            _button.onClick.AddListener(GenerateDriveNodes);
        }

        void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}