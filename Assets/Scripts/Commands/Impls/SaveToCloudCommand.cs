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
    public class SaveToCloudCommand : Command, ISaveToCloudCommand
    {
        [Inject] public IUIContext UIContext { get; set; }
        [Inject] public ILocalization Localization { get; set; }
        [Inject] public IProjectAsync Project { get; set; }
        [Inject] public IAppEnvironment AppEnvironment { get; set; }
        [Inject] public IStorageAsync<long> Storage { get; set; }
        [Inject] public IRTE RuntimeEditor { get; set; }
        [Inject] public IServiceProxy ServiceProxy { get; set; }
        
        private int NumOfMaxTry = 10;
        
        protected override async void Handle()
        {
            Debug.Log("SaveToCloudCommand");

            if (!Project.State.IsOpened)
            {
                return;
            }
            
            RuntimeEditor.IsBusy = true;
            UIContext.ShowPanel<ILoadingPanel>(Localization.GetString("Project_Saving", "正在努力保存项目中！！！"));
            
            try
            {
                using (await Project.LockAsync())
                {
                    await UploadProject();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message} \n {e.StackTrace}");
            }
            
            UIContext.HidePanel<ILoadingPanel>();
            RuntimeEditor.IsBusy = false;
        }

        private async Task UploadProject()
        {
            var projectName = Project.State.ProjectInfo.Name;
            var storageRootPath = await Storage.GetRootPathAsync();

            var projectRootPath = Path.Combine(storageRootPath, projectName);
            Debug.Log($"Project path:{projectRootPath}");
            var dirInfo = new DirectoryInfo(projectRootPath);

            var fileMd5Pairs = new Dictionary<string, string>();
            CollectFileMd5(dirInfo, projectRootPath, fileMd5Pairs);

            for (var i = 0; i <NumOfMaxTry; ++i)
            {
                var result = await ServiceProxy.TryRequestAsync<SaveProjectIndexRequest, SaveProjectIndexResponse>(
                    NetworkApis.SaveProjectIndex,
                    new SaveProjectIndexRequest
                    {
                        ProjectId = AppEnvironment.CurProjectInfo.Id,
                        AccessToken = AppEnvironment.AccessToken,
                        Files = fileMd5Pairs
                    });
                if (!result.IsSuc)
                {
                    throw new Exception("An error occurred while calling the save file service!");
                }

                var resultData = result.Data;
                if (resultData == null)
                    return;
                if (resultData.NeedUpdateFiles == null || resultData.NeedUpdateFiles.Count == 0)
                    return;
                
                var minioClient = await MinioUtil.InitWithToken(result.Data.UploadToken);
                await UploadProjectFiles(resultData.NeedUpdateFiles, projectRootPath, minioClient);
                // await SaveProjectIndex(minioClient, fileMd5Pairs, projectName);
            }
        }
        
        private static async Task UploadProjectFiles(Dictionary<string, string> fileMd5Pairs, string rootPath,  IMinioClient minioClient)
        {
            foreach (var file in fileMd5Pairs)
            {
                var fileMd5 = file.Value;
                var filePath = Path.Combine(rootPath, file.Key);
                Debug.Log($"[{DateTime.Now:HH:mm:ss.fff}] Upload :{file.Key},{fileMd5}");
                
                var bucketName = MinIOUtil.GetFileBucketName(fileMd5);
                if (!await MinioUtil.IsBucketExists(minioClient, bucketName))
                {
                    await MinioUtil.MakeBucketAsync(minioClient, bucketName);
                }
                
                await MinioUtil.PutFileObject(minioClient, bucketName, fileMd5, filePath);
            }
        }

        private static async Task SaveProjectIndex(IMinioClient minioClient, Dictionary<string, string> fileMd5Pairs, string projectName)
        {
            var projectIndexBucketName = "project-index";
            if (!await MinioUtil.IsBucketExists(minioClient, projectIndexBucketName))
            {
                await MinioUtil.MakeBucketAsync(minioClient, projectIndexBucketName);
            }

            var indexPairs = new Dictionary<string, string>();
            foreach (var pair in fileMd5Pairs)
            {
                indexPairs.Add(pair.Key, pair.Value);
            }
            var indexStr = JsonConvert.SerializeObject(indexPairs);
            var indexBytes = Encoding.UTF8.GetBytes(indexStr);
            await MinioUtil.PutObject(minioClient, projectIndexBucketName, projectName,
                new MemoryStream(indexBytes));
        }

        private void CollectFileMd5(DirectoryInfo directoryInfo, string projectRoot, Dictionary<string, string> files)
        {
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                var relativePath = Path.GetRelativePath(projectRoot, fileInfo.FullName);
                relativePath = relativePath.Replace("\\", "/");
                files.Add(relativePath, FileMd5Util.GetMd5(fileInfo.FullName));
            }
            
            foreach (var directory in directoryInfo.GetDirectories())
            {
                CollectFileMd5(directory, projectRoot, files);
            }
        }
    }
    
}