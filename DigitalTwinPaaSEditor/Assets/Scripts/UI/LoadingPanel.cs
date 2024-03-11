using TMPro;
using UI;
using UnityEngine;

public interface ILoadingPanel : IPanel
{
}

public class LoadingPanel : PanelBase, ILoadingPanel
{
    [SerializeField] private TMP_Text _txtMessage;

    public void Awake()
    {
        SetMessage(string.Empty);
    }

    public override void OnShow(object arg)
    {
        var message = arg as string;
        SetMessage(message);
    }

    public void SetMessage(string message)
    {
        if (_txtMessage != null)
        {
            _txtMessage.text = message;
        }
    }
}