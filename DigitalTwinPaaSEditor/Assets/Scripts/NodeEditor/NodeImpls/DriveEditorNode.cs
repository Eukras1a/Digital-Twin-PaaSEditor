using System.Collections.Generic;
using System.Linq;
using Battlehub.RTCommon;
using MeadowGames.UINodeConnect4;
using TMPro;
using UnityEngine;
using World.Drivers;

namespace NodeEditor.NodeImpls
{
    public class DriveEditorNode : BaseNode, IEditorNode
    {
        private IDriver _driver;
        [SerializeField]
        private TMP_Text _textHeader;

        private string _driverNodeId;

        public void SetNodeCtrl(IDriver driver)
        {
            _driver = driver;
            SetDriverNodeId(driver.NodeId);
        }
        
        public void OnIn(Port port, object arg)
        {
            if (arg is not Dictionary<string, object> args)
                return;
            
            if (_driver == null)
                return;
            
            var doArgs = args.ToDictionary(p => p.Key, p => p.Value.ToString());
            _driver.Do(doArgs);
        }

        public void OnOut()
        {
        }

        private const string DriveNodeIdKey = "DriveNodeId";
        public Dictionary<string, object> Save()
        {
            return new Dictionary<string, object>()
            {
                {DriveNodeIdKey, _driverNodeId},
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            if (!data.TryGetValue(DriveNodeIdKey, out var driverNodeId))
                return;
            
            SetDriverNodeId(driverNodeId?.ToString());
            var nodeEditorContext = IOC.Resolve<NodeEditorContext>();
            var curDevice = nodeEditorContext.CurDevice;
            if (curDevice == null) {
                _driver = null;
            }
            
            _driver = curDevice.GetDriver(_driverNodeId);
        }

        private void SetDriverNodeId(string driverNodeId)
        {
            _driverNodeId = driverNodeId;
            _textHeader.text = _driverNodeId;
        }
    }
}