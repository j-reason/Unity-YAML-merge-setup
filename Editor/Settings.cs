using System.IO;
using UnityEngine;
using Debug = YAMLMergeInEditor.Debug;

public static class Settings
{
    


    public static string ToolName { get; private set; } = "unityyamlmerge";
    public static string DefaultRepoPath { get; private set; } = Path.Combine(Application.dataPath, "../");
    public static string DefaultMergeToolPath { get; private set; } = Path.Join(UnityEditor.EditorApplication.applicationContentsPath, "/Tools", "/UnityYAMLMerge.exe");

    public static bool CreateBackupOnMerge { get; private set; } = false;

    public static Debug.LogLevel LogLevel { get; private set; } = Debug.LogLevel.Trace;
}
