using Battlehub.RTCommon;
using Commands.Impls;
using Utils;

namespace Commands
{
    public static class CommandBinding
    {
        public static void Bind()
        {
            IOC.Register(new UpdateAndOpenProjectCommand());
            
            IOC.RegisterFallback(()=> (IUpdateAndOpenProjectCommand)MakeCommand<UpdateAndOpenProjectCommand>());
            IOC.RegisterFallback(()=> (ISaveToCloudCommand)MakeCommand<SaveToCloudCommand>());
        }
        
        private static T MakeCommand<T>() where T : new()
        {
            var command = new T();
            IOCHelper.Inject(command);
            return command;
        }
    }
}