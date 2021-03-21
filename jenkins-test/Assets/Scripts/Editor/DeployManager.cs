using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

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
        Debug.Log("Start build.");
        
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;
        
        #region Report Log
        
        var log = new StringBuilder();
        log.Append("Build Report: ");
        
        if (summary.result == BuildResult.Succeeded)
        {
            log.Append($"Success: (Size: {summary.totalSize} bytes)");
        }
        else if (summary.result == BuildResult.Failed)
        {
            log.Append("Fail!");
        }
        
        log.AppendLine($"(Time: {summary.totalTime.TotalSeconds:hh\\:mm\\:ss\\:fff}");
        log.AppendLine($"(Errors: {summary.totalErrors})");
        log.AppendLine($"(Warnings: {summary.totalWarnings})");
        
        Debug.Log(log.ToString());
        
        #endregion
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