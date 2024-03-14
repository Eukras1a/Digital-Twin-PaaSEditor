using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Networks;
using Newtonsoft.Json;
using UnityEngine;

namespace Utils.Service
{
    public class ServiceResult<T>
    {
        public bool IsSuc;
        public T Data;
        public string Message;
    }
    
    public class ServiceProxy : IServiceProxy
    {
        // public string ServerAddress = "http://127.0.0.1";
        public string ServiceUrl { get; }

        public ServiceProxy(string serviceUrl)
        {
            ServiceUrl = serviceUrl;
        }
        
        public async Task<ServiceResult<TResponse>> TryRequestAsync<TRequest, TResponse>(string api, TRequest requestArg)
        {
            var requestUri = $"{ServiceUrl}{api}";
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(requestUri);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = 0;
                
                if (requestArg != null)
                {
                    var requestStr = JsonConvert.SerializeObject(requestArg);
                    Debug.Log($"Request:{requestUri},{requestStr}");
                    var requestBytes = Encoding.UTF8.GetBytes(requestStr);
                    req.ContentLength = requestBytes.Length;
                    await using var requestStream = req.GetRequestStream();
                    await requestStream.WriteAsync(requestBytes, 0, requestBytes.Length);
                }
                else
                {
                    Debug.Log($"Request:{requestUri}");
                }
                
                var response = (HttpWebResponse)await req.GetResponseAsync();
                await using var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    var responseStr = await reader.ReadToEndAsync();
                    
                    if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                    {
                        var message = $"http status code:[{response.StatusCode}]{requestUri}{responseStr}";
                        Debug.LogError($"Request error:{requestUri},{message}");
                        return new ServiceResult<TResponse>
                        {
                            IsSuc = false,
                            Message = message 
                        };
                    }
                    
                    try
                    {
                        Debug.Log($"Request:[{response.StatusCode}] {requestUri},{responseStr}");
                        var responseArg = JsonConvert.DeserializeObject<NetResponse<TResponse>>(responseStr);
                        return new ServiceResult<TResponse>
                        {
                            IsSuc = responseArg.Code == 1,
                            Data = responseArg.Data,
                            Message = responseArg.Message
                        };
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Request error:{e.Message} \n {requestUri},{responseStr}");
                        return new ServiceResult<TResponse>
                        {
                            IsSuc = false,
                            Message = e.Message
                        };
                    }
                    
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Request error:{requestUri},{e.Message}");
            }
            return new ServiceResult<TResponse>
            {
                IsSuc = false,
                Message = ""
            };
        }

        public TResponse Request<TRequest, TResponse>(string api, TRequest request)
        {
            throw new System.NotImplementedException();
        }
    }

    public class NetResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}