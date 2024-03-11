using System;
using Battlehub.RTCommon;
using Battlehub.RTEditor;
using NodeEditor;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace RTEditor.ComponentEditors
{
    public class DeviceComponentEditor : MonoBehaviour
    {
        [SerializeField] private Button _btnBinding;

        private void Awake()
        {
            _btnBinding.onClick.AddListener(OnBindingClick);
        }

        private void OnBindingClick()
        {
            var gameObjectEditor = GetComponentInParent<GameObjectEditor>();
            if (gameObjectEditor == null)
                return;

            var selectedGameObject = gameObjectEditor.SelectedGameObject;
            if (selectedGameObject == null)
                return;
            
            var deviceCtrl = selectedGameObject.GetComponent<DeviceCtrl>();
            if (deviceCtrl == null)
                return;
            
            var nodeEditorContext = IOC.Resolve<NodeEditorContext>();
            nodeEditorContext.Enable = true;
            nodeEditorContext.CurDevice = deviceCtrl;
        }
    }
}