using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using World.Drivers;

namespace World
{
    public class DeviceCtrl : MonoBehaviour
    {
        [SerializeField]
        public string DeviceId;
        
        private List<IDriver> Nodes = new();
        public string Id => DeviceId;

        public void Awake()
        {
            Nodes.AddRange(GetComponentsInChildren<IDriver>());
        }

        public void Do(string @params)
        {
            try
            {
                var paramsLines = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(@params);
                foreach (var line in paramsLines)
                {
                    foreach (var node in Nodes)
                    {
                        node.Do(line);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        [ContextMenu("Reset")]
        public void Reset()
        {
            foreach (var node in Nodes)
            {
                node.Reset();
            }
        }      
        
        [ContextMenu("TestFile")]
        public void TestFromFile()
        {
            StartCoroutine(DoTestFile());
        }

        private IEnumerator DoTestFile()
        {
            var textLines = File.ReadLines("C:\\Users\\ChenS\\Desktop\test.txt");
            foreach (var line in textLines)
            {
                Do(line);
                yield return new WaitForSeconds(0.5f);
            }
        }

        public IDriver GetDriver(string driverNodeId)
        {
            var drivers = GetComponentsInChildren<IDriver>();
            foreach (var driver in drivers)
            {
                if (driver.NodeId == driverNodeId)
                    return driver;
            }

            return null;
        }
    }
}