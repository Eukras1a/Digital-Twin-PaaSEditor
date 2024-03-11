using System.Collections.Generic;

namespace World.Drivers
{
    public interface IDriver
    {
        string NodeId { get; }
        void Do(Dictionary<string, string> args);
        void Reset();
        string GetNodeName();
    }
}