using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Battlehub.RTCommon;
using Battlehub.RTEditor.UI;

namespace Utils
{
    public static class IOCHelper
    {
        private static void InjectDependencies(object instance)
        {
            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
            {
                InjectAttribute injectAttribute = propertyInfo.GetCustomAttribute<InjectAttribute>();
                if (injectAttribute != null)
                {
                    object injectPropertyValue = propertyInfo.GetValue(instance);
                    if (injectPropertyValue == null)
                    {
                        injectPropertyValue = IOC.Resolve(propertyInfo.PropertyType);
                        propertyInfo.SetValue(instance, injectPropertyValue);
                    }
                }
            }
        }
        
        public static void Inject<T>(T command)
        {
            InjectDependencies(command);
        }
    }
}