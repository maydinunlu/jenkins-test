using System;
using System.Collections.Generic;
using UnityEditor;

public class DeployManager
{
    private static readonly string[] _scenes = GetScenes();

    [MenuItem ("Build/Android")]
    public static void BuildAndroid()
    {
        #region Location

        var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var locationPathName = path + @"/test.apk";

        #endregion
        
        #region BuildPlayerOptions

        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = locationPathName,
            target = BuildTarget.Android
        };

        #endregion
        
        StartBuild(buildPlayerOptions);
    }
    
    private static void StartBuild(BuildPlayerOptions buildPlayerOptions)
    {
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;
    }

    private static string[] GetScenes() 
    {
        var sceneList = new List<string>();
        foreach(var scene in EditorBuildSettings.scenes) 
        {
            if (!scene.enabled)
            {
                continue;
            } 
            sceneList.Add(scene.path);
        }
        return sceneList.ToArray();
    }
}