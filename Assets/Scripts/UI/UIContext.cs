using Battlehub.RTCommon;
using UnityEngine;

namespace UI
{
    public interface IUIContext
    {
        void ShowPanel<T>(object arg = null) where T : IPanel;
        void HidePanel<T>() where T : IPanel;
    }
    
    public class UIContext : MonoBehaviour,IUIContext
    {
        [SerializeField]
        private LoadingPanel LoadingPanel;
        
        public void Awake()
        {
            IOC.Register<IUIContext>(this);
            IOC.Register<ILoadingPanel>(LoadingPanel);
        }

        public void ShowPanel<T>(object arg = null) where T : IPanel
        {
            var panel = IOC.Resolve<T>();
            if (panel != null)
            {
                panel.Show(arg);
            }
        }
        
        public void HidePanel<T>() where T : IPanel
        {
            var panel = IOC.Resolve<T>();
            if (panel != null)
            {
                panel.Hide();
            }
        }
    }
}