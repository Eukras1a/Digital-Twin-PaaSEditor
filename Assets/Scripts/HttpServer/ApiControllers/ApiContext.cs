using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HttpServer.ApiControllers
{
    public class ApiContext
    {
        IDictionary<string, SupportRequest> _support = new Dictionary<string, SupportRequest>();

        public void Initialize()
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var controllerTypes = assembly.GetTypes();
                foreach (var controllerType in controllerTypes)
                {
                    if (!typeof(IApiController).IsAssignableFrom(controllerType))
                        continue;
                    if (controllerType.IsAbstract || controllerType.IsEnum || controllerType.IsInterface)
                        continue;
                    
                    var routeBase = "/";
                    var routeAttribute = controllerType.GetCustomAttribute<RouteAttribute>();
                    if (routeAttribute != null)
                    {
                        routeBase = routeAttribute.Path.Trim('/') + "/";
                    }
                    var controllerInstance = System.Activator.CreateInstance(controllerType);
                    var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var method in methods)
                    {
                        var postAttribute = method.GetCustomAttribute<PostAttribute>();
                        if (postAttribute == null)
                            continue;
                        
                        var routeFullPath = routeBase + postAttribute.Path.Trim('/');
                        routeFullPath = "/" + routeFullPath.Trim('/');
                        Debug.Log($"Api:{routeFullPath},{method}");
                        _support.Add(routeFullPath, new SupportRequest
                        {
                            Method = postAttribute.Method,
                            MethodCallback = method,
                            ApiController = controllerInstance as IApiController
                        });
                    }
                }
                
            }
        }
        
        public class HandleResult
        {
            public int Code;
            public object Body;
        }

        private HandleResult NotFound = new HandleResult
        {
            Code = 404
        };
        
        private HandleResult NotSupportMethod = new HandleResult
        {
            Code = 400,
            Body = "Not support method"
        };
        
        private HandleResult InternalError = new HandleResult
        {
            Code = 500,
            Body = "Service internal error"
        };
        
        public HandleResult HandleRequest(string url, string method, string body)
        {
            if (!_support.TryGetValue(url, out var supportRequest))
                return NotFound;
            
            if (supportRequest.Method != method)
                return NotSupportMethod;

            try
            {
                var obj = supportRequest.MethodCallback.Invoke(supportRequest.ApiController, new object[]{body});
                return new HandleResult
                {
                    Code = 200,
                    Body = obj
                };
            }
            catch (Exception e)
            {
                var message = "";
                var curException = e;
                do
                {
                    message += e.Message + "\n";
                    curException = curException.InnerException;
                } while (curException != null);
                
                return new HandleResult
                {
                    Code = 500,
                    Body = message
                };
            }
        }
    }
}