using Battlehub.RTCommon;
using Battlehub.UIControls.MenuControl;
using Commands;
using UnityEngine;

namespace Projects
{
    [MenuDefinition(order:-90)]
    public class MenuProject : MonoBehaviour
    {
        [MenuCommand("MenuFile/SaveToCloud", priority: 50)]
        public void SaveToCloud()
        {
            IOC.Resolve<ISaveToCloudCommand>().Do();
        }
    }
}