using System;
using UnityEditor;
using UnityEngine;

public static class GitCommands
{
    public static bool isProjectGitRepo(string path)
    {
#if UNITY_EDITOR_WIN
        return GitCommandsWinows.isProjectGitRepo(path);
#else
        return false;
#endif
    }

    public static bool isMergeYAMLSetup(string path)
    {
#if UNITY_EDITOR_WIN
        return GitCommandsWinows.isMergeYAMLSetup(path);
#else
        return false;
# endif
    }

    public static bool isYAMLMergePathSame(string repoPath, string YAMLMergePath)
    {
#if UNITY_EDITOR_WIN
        return GitCommandsWinows.isYAMLMergePathSame(repoPath,YAMLMergePath);
#else
        return false;
# endif
    }

    public static void SetupYAML(string repoPath, string YAMLMergePath)
    {
#if UNITY_EDITOR_WIN
        GitCommandsWinows.SetupYAML(repoPath, YAMLMergePath);
#else

#endif

    }
}
