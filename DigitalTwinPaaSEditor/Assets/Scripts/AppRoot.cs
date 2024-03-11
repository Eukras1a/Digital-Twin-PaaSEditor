using System;
using Battlehub;
using Battlehub.RTCommon;
using Commands;
using Networks;
using UnityEngine;
using Utils.Service;

public class AppRoot : MonoBehaviour
{
    [SerializeField]
    private string ServiceUrl = "http://10.7.7.103:8080";
    // public static void Initialize()
    // {
    //     KnownAssemblies.Add("Twin.Runtime");
    // }
    
    public void Awake()
    {
        KnownAssemblies.Add("Twin.Runtime");
        
        Debug.Log("AppRoot.Awake");
        try
        {
            var httpServer = new HttpServer.HttpServerImplementation(System.Net.IPAddress.Any, 8080);
            httpServer.Start();
            
            Debug.Log($"Path:{Application.persistentDataPath}");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
        IOC.Register<IServiceProxy>(new ServiceProxy(ServiceUrl));
    }

    public void Start()
    {
        CommandBinding.Bind();
        
        IOC.Resolve<IUpdateAndOpenProjectCommand>().Do();
    }
}
