using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class PanelBase : MonoBehaviour, IPanel
    {
        public virtual void OnShow(object arg)
        {
        }

        public virtual void OnHide()
        {
        }
        
        public void Show(object arg = null)
        {
            gameObject.SetActive(true);
            OnShow(arg);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}