using UnityEngine;
using System.IO;
using UnityEditor;

public static class Utility
{

    [MenuItem("Tools/Git/is Merge")]
    public static bool tryGetMergeCommit(string repositoryPath, out string commitHash)
    {
        string mergeHeadPath = Path.Combine(repositoryPath, ".git", "MERGE_HEAD");
        Debug.Log($"Testing: {mergeHeadPath}");
        if (!File.Exists(mergeHeadPath))
        {
            Debug.Log("No Merge Happening");
            commitHash = null;
            return false;
        }


        commitHash = File.ReadAllText(mergeHeadPath);
        Debug.Log($"Merge Commit: {commitHash}");
        return true;
    }
    
}
