using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

using Debug = UnityEngine.Debug;

/// <summary>
/// All Git Commands needed for Windows
/// </summary>
public static class GitCommandsWinows
{


    private static bool s_debug = false;
    private const string MERGETOOL = "unityyamlmerge";
    private const string OUTPUT_QUOTE = @"""""""";
    private const string INPUT_QUOTE = @"""";

    public static bool isProjectGitRepo(string path)
    {
        string command = "git rev-parse --is-inside-work-tree";
        string output = CommandOutput(command, path, s_debug);

        if (bool.TryParse(output, out var retVal))
            return retVal;

        return false;
    }

    public static bool isMergeYAMLSetup(string path)
    {
        string command = "git config --get merge.tool";
        string output = CommandOutput(command, path, s_debug);

        return String.Equals(MERGETOOL,output);
    }

    public static bool isYAMLMergePathSame(string repoPath, string YAMLMergePath)
    {
        string command = "git config --get mergetool.unityyamlmerge.cmd";
        string output = CommandOutput(command, repoPath, s_debug);
        output = output.TrimEnd();

        string expectedOutput = @$"'{YAMLMergePath}' merge -p ""$BASE"" ""$REMOTE"" ""$LOCAL"" ""$MERGED""";
        Debug.Log($"Comparing: \n\rResult: {output}\n\rExpected: {expectedOutput}");
        return string.Equals(output, expectedOutput);
    }

    public static void SetupYAML(string repoPath, string YAMLMergePath)
    {
        string addYAML = "git config merge.tool unityyamlmerge";
        string trustExitCode = "git config mergetool.unityyamlmerge.trustExitCode false";

        string setYAMLPath = BuildYAMLMergePath(YAMLMergePath,true);
        

        CommandOutput(addYAML, repoPath, s_debug);
        CommandOutput(trustExitCode, repoPath, s_debug);
        CommandOutput(setYAMLPath, repoPath, s_debug);
        
    }

    private static string BuildYAMLMergePath(string YAMLMergePath, bool forOutput)
    {
        string quote = forOutput ? OUTPUT_QUOTE : INPUT_QUOTE;
       return $"git config mergetool.unityyamlmerge.cmd \"'{YAMLMergePath}' merge -p {quote}$BASE{quote} {quote}$REMOTE{quote} {quote}$LOCAL{quote} {quote}$MERGED{quote}\"";
    }


    private static string CommandOutput(string command,
                                    string workingDirectory = null, bool debugCommand = false)
    {
        try
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardError = procStartInfo.RedirectStandardInput = procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            if (null != workingDirectory)
            {
                procStartInfo.WorkingDirectory = workingDirectory;
            }

            Process proc = new Process();
            proc.StartInfo = procStartInfo;

            StringBuilder sb = new StringBuilder();
            proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                sb.AppendLine(e.Data);
            };
            proc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                sb.AppendLine(e.Data);
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();

            if (debugCommand)
                Debug.Log($"Running {command} in {workingDirectory}.\r\n Output: {sb.ToString()}");

            return sb.ToString();
        }
        catch (Exception objException)
        {
            string errorMessage = $"Error in command: {command}, {objException.Message}";
            if (debugCommand)
                Debug.LogError(errorMessage);
            return errorMessage;
        }
    }
}
