using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildAssetsLister : MonoBehaviour
{
    [MenuItem("Build/Build and List Assets")]
    public static void BuildAndListAssets()
    {
        // Cấu hình cho build
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        // Update the scenes based on the scenes you want to include in the build.
        buildPlayerOptions.scenes = new[] {
            "Assets/___Scenes/Loading.unity",
            "Assets/___Scenes/game_1.unity",
            "Assets/___Scenes/plappy_bird.unity",
            "Assets/___Scenes/racing.unity",
            "Assets/___Scenes/shotter.unity",
            "Assets/___Scenes/kart.unity",
            "Assets/___Scenes/squid_game.unity",
            "Assets/___Scenes/glass_bridge.unity",
            "Assets/___Scenes/tug.unity"
        };

        // Specify the location where you want to save the build.
        buildPlayerOptions.locationPathName = "D:/Download/test.apk";

        // Change the target to Android.
        buildPlayerOptions.target = BuildTarget.Android;

        // Set the build options, if you have any specific ones.
        buildPlayerOptions.options = BuildOptions.None;

        // Thực hiện build
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        // Kiểm tra nếu build thành công
        if (summary.result == BuildResult.Succeeded)
        {
            var usedAssets = new List<KeyValuePair<string, long>>();
            foreach (var packedAsset in report.packedAssets)
            {
                foreach (var asset in packedAsset.contents)
                {
                    if (asset.sourceAssetPath.StartsWith("Assets/"))
                    {
                        FileInfo fileInfo = new FileInfo(asset.sourceAssetPath);
                        long size = fileInfo.Exists ? fileInfo.Length : 0;
                        usedAssets.Add(new KeyValuePair<string, long>(asset.sourceAssetPath, size));
                    }
                }
            }

            // Sắp xếp theo kích thước tệp
            var sortedAssets = usedAssets.OrderByDescending(pair => pair.Value);

            Debug.Log("Assets used in build, sorted by size:");
            foreach (var pair in sortedAssets)
            {
                Debug.Log($"{pair.Key}: {pair.Value} bytes");
            }
        }
        else
        {
            Debug.LogError("Build failed");
        }
    }
}
