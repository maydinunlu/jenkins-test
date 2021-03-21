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
        var args = GetArgs();
        Console.WriteLine(args.ToString());

        var buildName = args.BuildName;
        if (args.BuildType == "Apk")
        {
            buildName += ".apk";
        }
        else if (args.BuildType == "Bundle")
        {
            buildName += ".aab";
        }
        var locationPathName = args.TargetDir + buildName;
        
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetScenes(),
            locationPathName = locationPathName,
            target = BuildTarget.Android
        };
        
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

    private static Args GetArgs()
    {
        var returnValue = new Args();
 
        // find: -executeMethod
        //   +1: JenkinsBuild.BuildMacOS (Build Function)
        //   +2: FindTheGnome
        //   +3: D:\Jenkins\Builds\Find the Gnome\47\output (Target)
        var args = System.Environment.GetCommandLineArgs();
        var execMethodArgPos = -1;
        var allArgsFound = false;
        
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == "-executeMethod")
            {
                execMethodArgPos = i;
            }
            
            var realPos = execMethodArgPos == -1 ? -1 : i - execMethodArgPos - 2;
            if (realPos < 0)
            {
                continue;
            }

            if (realPos == 0)
            {
                returnValue.BuildName = args[i];
            }
            else if (realPos == 1)
            {
                returnValue.Version = args[i];
            }
            else if (realPos == 2)
            {
                returnValue.BuildType = args[i];
            }
            else if (realPos == 3)
            {
                returnValue.Development = Convert.ToBoolean(args[i]);
            }
            else if (realPos == 4)
            {
                returnValue.SRDebugEnable = Convert.ToBoolean(args[i]);
            }
            else if (realPos == 5)
            {
                returnValue.AnalyticsEnable = Convert.ToBoolean(args[i]);
            }
            else if (realPos == 6)
            {
                returnValue.PlayFabTitleId = args[i];
            }
            else if (realPos == 7)
            {
                returnValue.GameGrowthEnvironment = args[i];
            }
                
            if (realPos == 8)
            {
                returnValue.TargetDir = args[i];
                if (!returnValue.TargetDir.EndsWith(System.IO.Path.DirectorySeparatorChar + ""))
                {
                    returnValue.TargetDir += System.IO.Path.DirectorySeparatorChar;
                } 
 
                allArgsFound = true;
            }
        }

        if (!allArgsFound)
        {
            Console.WriteLine("[JenkinsBuild] Incorrect Parameters for -executeMethod Format: -executeMethod JenkinsBuild.BuildWindows64 <app name> <output dir>");
        }
            
        return returnValue;
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
    
    private class Args
    {
        public string BuildName { get; set; }
        public string Version { get; set; }
        public string BuildType { get; set; } // Apk, Bundle
        public bool Development { get; set; }
        public bool SRDebugEnable { get; set; }
        public bool AnalyticsEnable { get; set; }
        public string PlayFabTitleId { get; set; }
        public string GameGrowthEnvironment { get; set; }
        public string TargetDir = "~/Desktop";

        public override string ToString()
        {
            var log = $"Build Args: ";
            log += $"(BuildName: {BuildName}";
            log += $"(BuildType: {BuildType}";
            log += $"(Development: {Development}";
            log += $"(SRDebugEnable: {SRDebugEnable}";
            log += $"(AnalyticsEnable: {AnalyticsEnable}";
            log += $"(PlayFabTitleId: {PlayFabTitleId}";
            log += $"(GameGrowthEnvironment: {GameGrowthEnvironment}";
            log += $"(TargetDir: {TargetDir}";

            return log;
        }
    }
}