using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using App;
using Battlehub;
using Battlehub.RTCommon;
using Battlehub.RTEditor.UI;
using Battlehub.RTSL.Interface;
using Cysharp.Threading.Tasks;
using Minio;
using Models.Networks;
using Networks;
using Newtonsoft.Json;
using UI;
using UnityEngine;
using Utils.IO;
using Utils.Minio;

namespace Commands.Impls
{
    public class UpdateAndOpenProjectCommand : Command, IUpdateAndOpenProjectCommand
    {
        [Inject] public IUIContext UIContext { get; set; }
        [Inject] public IRTE RuntimeEditor { get; set; }
        [Inject] public IServiceProxy Service { get; set; }
        [Inject] public IAppEnvironment AppEnvironment { get; set; }
        [Inject] public ILocalization Localization { get; set; }
        [Inject] public IProjectAsync Project { get; set; }
        [Inject] public IStorageAsync<long> Storage { get; set; }

        protected override async void Handle()
        {
            RuntimeEditor.IsBusy = true;
            Debug.Log("Show Loading!");
            UIContext.ShowPanel<ILoadingPanel>(Localization.GetString("Project_Loading", "正在努力加载项目中！！！"));
            var projectId = AppEnvironment.CurProjectInfo.Id;
            var result = await Service.TryRequestAsync<GetProjectIndexRequest, GetProjectIndexResponse>(
                NetworkApis.GetProjectIndex, new GetProjectIndexRequest
                {
                    ProjectId = projectId,
                    AccessToken = AppEnvironment.AccessToken
                });

            if (!result.IsSuc)
            {
                RuntimeEditor.IsBusy = false;
                //todo 提示失败，并退出
                return;
            }

            if (Project.State.IsOpened)
            {
                await Project.CloseProjectAsync();
            }

            //todo 暂时默认先删掉
            // await Project.DeleteProjectAsync(projectId);
            
            // if (result.Data.Files.Count == 0)
            // {
            //     var projectInfo = await Project.CreateProjectAsync(projectId);
            //     Debug.Log($"new project:{projectInfo.Name}");
            // }
            // else
            // {
            //     
            // }
            
            var projectName = projectId;
            var storageRootPath = await Storage.GetRootPathAsync();
            var projectRootPath = Path.Combine(storageRootPath, projectName);
            try
            {
                var projectIndex = result.Data.Files;
                if (projectIndex == null || projectIndex.Count == 0)
                {
                    var projectInfo = await Project.CreateProjectAsync(projectId);
                    Debug.Log($"new project:{projectInfo.Name}");
                }
                else
                {
                    var minioClient = await MinioUtil.InitWithToken(result.Data.DownloadToken);
                    await DownloadProjectFilesAsyncAsync(minioClient, projectRootPath, projectIndex);    
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message} \n {e.StackTrace}");
            }
            await Project.OpenProjectAsync(projectId);
            Debug.Log($"Update result:{result.IsSuc}");
                        
            RuntimeEditor.IsBusy = false;
            UIContext.HidePanel<ILoadingPanel>();
        }

        private async Task DownloadProjectFilesAsyncAsync(IMinioClient minioClient, string projectRootPath,
            Dictionary<string,string> projectIndex)
        {
            if (projectIndex == null)
                return;
            
            foreach (var pair in projectIndex)
            {
                var fileMd5 = pair.Value;
                var fileRelativePath = pair.Key;
                var bucketName = MinIOUtil.GetFileBucketName(fileMd5);
                var fileFullName = Path.Combine(projectRootPath, fileRelativePath);

                if (File.Exists(fileFullName) &&
                    fileMd5 == FileMd5Util.GetMd5(fileFullName))
                    continue;
                
                var dirName = Path.GetDirectoryName(fileFullName);
                if (dirName != null && !Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
                await MinioUtil.GetFileAsync(minioClient, bucketName, fileMd5, fileFullName);
            }
        }

        private static async Task<Dictionary<string, string>> LoadProjectIndexAsync(IMinioClient minioClient, string projectName)
        {
            var projectIndexBucketName = "project-index";
            Dictionary<string, string> projectIndex = null;
            await MinioUtil.GetStreamAsync(minioClient, projectIndexBucketName, projectName,
                stream =>
                {
                    var bytes = new byte[stream.Length];
                    _ = stream.Read(bytes, 0, bytes.Length);
                    var indexStr = Encoding.UTF8.GetString(bytes);
                    projectIndex = JsonConvert.DeserializeObject<Dictionary<string, string>>(indexStr);
                });
            return projectIndex;
        }
    }
}