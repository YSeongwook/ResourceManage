using System.IO;
using UnityEditor;

public class AssetBundleBuilder
{
    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        // 에셋 번들 빌드 경로 설정
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        // 에셋 번들 빌드
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
            BuildAssetBundleOptions.None, 
            BuildTarget.StandaloneWindows); // 빌드 타겟을 모바일로 변경 시 BuildTarget.iOS 또는 BuildTarget.Android로 변경 가능
    }
}