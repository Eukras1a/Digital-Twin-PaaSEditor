using Battlehub.RTCommon;
using Battlehub.RTSL;
using Projects;
using UnityEngine;

namespace App
{
    public class AppConfig : MonoBehaviour
    {
        public string AssetbundlePath;
        public string EditorAssetbundlePath;
        public string TestAccessToken;
        public string TestProjectId;

        public void Awake()
        {
            IOC.Register<IAppEnvironment>(new AppEnvironment
            {
                AccessToken = TestAccessToken,
                CurProjectInfo = new ProjectInfo
                {
                    Id = TestProjectId
                }
            });
        }

        public void Start()
        {
            var assetBundleLoader = IOC.Resolve<IAssetBundleLoader>();
#if UNITY_EDITOR
            assetBundleLoader.AssetBundlesPath = EditorAssetbundlePath;
#else
            assetBundleLoader.AssetBundlesPath = AssetbundlePath;
#endif
        }
    }
}