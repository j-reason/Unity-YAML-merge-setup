using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[InitializeOnLoad]
public static class YAMLSmartMergeSetup
{

    // Paths
    public static readonly string PROJECTPATH = Path.Combine(Application.dataPath, "../");
    public static readonly string YAMLMERGETOOLPATH = Path.Join(UnityEditor.EditorApplication.applicationContentsPath,"/Tools", "/UnityYAMLMerge.exe");

    //Key for dialogue box
    const string k_dontAskMeAgainKey = "YAMLMergeInitialiser.SetupDialogue";

    static YAMLSmartMergeSetup()
    {
        RunOnStartUp();
    }

    
    public static void RunOnStartUp()
    {
        //if ticked don't do this, don't do it
        if (EditorPrefs.GetBool(k_dontAskMeAgainKey, false))
            return;

        string path = PROJECTPATH;
        if (string.IsNullOrEmpty(path))
            return;

        int setupCode = RunChecks(path);

        //silently fail on returns -1 and -2


        if (setupCode == -3)
        {
            SetupYAML(path, "This git project is configured to use YAML Smart Merge, but hasn't been configured yet on this machine.\r\nWould you like to set that up now?");
            return;
        }

        if (setupCode == -4)
        {
            SetupYAML(path, "YAML Smart Merge has been setup for a different version of Unity.\r\nWould you like to fix that now?");
            return;
        }

    }

    [MenuItem("Tools/Git/Setup YAML Smart Merge")]
    public static void RunFromMenu()
    {
        string path = EditorUtility.OpenFolderPanel("Select Git Repo", "", "");
        if (string.IsNullOrEmpty(path))
            return;

        int setupCode = RunChecks(path);

        if (setupCode == -1)
        {
            EditorUtility.DisplayDialog("YAML Smart Merge Setup", "This project does not contain GIT.\r\n(Or Git couldn't be located on your machine)","Ok");
            return;
        }

        if (setupCode == -2)
        {
            EditorUtility.DisplayDialog("YAML Smart Merge Setup", $"Error: Unable to Locate UnityYAMLMerge.exe\r\nExpected at:{YAMLMERGETOOLPATH}", "Ok");
            return;
        }

        if (setupCode == -3)
        {
            SetupYAML(path, "This git project is configured to use YAML Smart Merge, but hasn't been configured yet on this machine.\r\nWould you like to set that up now?");
            return;
        }

        if (setupCode == -4)
        {
            SetupYAML(path, "YAML Smart Merge has been setup for a different version of Unity.\r\nWould you like to fix that now?");
            return;
        }

        EditorUtility.DisplayDialog("YAML Smart Merge Setup","YAML Smart Merge has already been setup for this project", "Ok");
    }


    [MenuItem("Tools/Git/Run Unity YAML Merge", priority = 0)]
    public static void RunMerge()
    {
        GitCommands.RunMerge(PROJECTPATH);
    }



    private static int RunChecks(string repoPath)
    {
        if (!GitCommands.isProjectGitRepo(repoPath)) //Git not intialise on system or not in project
            return -1;
        if (!System.IO.File.Exists(YAMLMERGETOOLPATH)) //Check for YAML Merge Tool
            return -2;

        if (!GitCommands.isMergeYAMLSetup(repoPath)) //YAML not configured in project
            return -3;

        if (!GitCommands.isYAMLMergePathSame(repoPath, YAMLMERGETOOLPATH)) //YAML not setup but not this version of unity
            return -4;

        //all setup properly
        return 1;
    }

    private static void SetupYAML(string repoPath, string message)
    {
        //Show dialogue asking if they want to do this
        int isAgreed = EditorUtility.DisplayDialogComplex("YAML Smart Merge Setup",
            message,
            "Okay", "No", "No. Don't ask me again.");

        if (isAgreed == 1)
            return;

        if (isAgreed == 2)
        {
            EditorPrefs.SetBool(k_dontAskMeAgainKey, true);
            return;
        }

        EditorPrefs.SetBool(k_dontAskMeAgainKey, false);
        GitCommands.SetupYAML(repoPath, YAMLMERGETOOLPATH);
    }


}
