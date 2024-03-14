using System.Collections.Generic;
using MeadowGames.UINodeConnect4;

public interface IEditorNode
{
    string PrefabName { get; set; }

    void OnIn(Port port, object arg);

    void OnOut();
    
    Dictionary<string, object> Save();
    void Load(Dictionary<string, object> data);
}