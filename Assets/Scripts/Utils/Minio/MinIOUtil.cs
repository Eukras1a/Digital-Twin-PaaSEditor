using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Minio;
using Minio.Credentials;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Encryption;
using Models.Networks;
using UnityEngine;

namespace Utils.Minio
{
    public static class MinioUtil
    {
        public static Task<IMinioClient> InitWithToken(MinIOToken token)
        {
            Debug.Log("MinioClientInit");
            var minio = new MinioClient()
                .WithEndpoint("43.143.122.109:9000")
                .WithCredentials(token.AccessKey, token.SecretKey)
                .WithSessionToken(token.SessionToken)
                .Build();
            return Task.FromResult(minio);
        }
        
        public static Task<IMinioClient> Init()
        {
            Debug.Log("MinioClientInit");
            var minio = new MinioClient()
                .WithEndpoint("43.143.122.109:9000")
                .WithCredentials("minioadmin",
                    "k2fDzRyjBXWp")
                .Build();
            return Task.FromResult(minio);
        }
        
        public static Task<bool> IsBucketExists(IMinioClient minio, string bucketName)
        {
            var bktExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
            return minio.BucketExistsAsync(bktExistsArgs);
        }

        public static async  Task GetFileAsync(IMinioClient minio,
            string bucketName = "my-bucket-name",
            string objectName = "my-object-name",
            string fileName = "my-file-name")
        {
            var args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithFile(fileName);
            await minio.GetObjectAsync(args).ConfigureAwait(false);
        }
        
        public static async  Task GetStreamAsync(IMinioClient minio,
            string bucketName,
            string objectName,
            Action<Stream> callbackStream)
        {
            var args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(callbackStream);
            var stat = await minio.GetObjectAsync(args).ConfigureAwait(false);
        }

        public static async Task MakeBucketAsync(IMinioClient minio,
            string bucketName = "my-bucket-name", string loc = "us-east-1")
        {
            await minio.MakeBucketAsync(
                new MakeBucketArgs()
                    .WithBucket(bucketName)
                    .WithLocation(loc)
            ).ConfigureAwait(false);
        }

        public static async Task PutFileObject(IMinioClient minio,
            string bucketName = "my-bucket-name",
            string objectName = "my-object-name",
            string fileName = "location-of-file",
            IProgress<ProgressReport> progress = null,
            IServerSideEncryption sse = null)
        {
            await using var filestream = File.OpenRead(fileName);
            // var metaData = new Dictionary<string, string>
            //     (StringComparer.Ordinal) { { "Test-Metadata", "Test  Test" } };
            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(filestream)
                .WithObjectSize(filestream.Length)
                .WithContentType("application/octet-stream")
                // .WithHeaders(metaData)
                .WithProgress(progress)
                .WithServerSideEncryption(sse);
            _ = await minio.PutObjectAsync(args).ConfigureAwait(false);
        }

        public static async Task PutObject(IMinioClient minio,
            string bucketName ,
            string objectName ,
            Stream srcStream,
            IServerSideEncryption sse = null)
        {
            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(srcStream)
                .WithObjectSize(srcStream.Length)
                .WithServerSideEncryption(sse);
            _ = await minio.PutObjectAsync(args).ConfigureAwait(false);
        }
    }
}