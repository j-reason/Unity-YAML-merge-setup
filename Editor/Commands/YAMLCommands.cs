using CliWrap;
using CliWrap.Buffered;
using System.Diagnostics;
using UnityEngine;
using Debug = YAMLMergeInEditor.Debug;

namespace YAMLMergeInEditor.Commands
{
    public static class YAMLCommands
    {





        public static async Awaitable<bool> isYAMLInstalled(string repositoryPath)
        {

            var command = CommandUtility.Git(repositoryPath, "config", "--get", "merge.tool"); //git config --get merge.tool
            bool isInstalled = await CommandUtility.RunAndCheck(command, output => output.Contains(Settings.ToolName));

            Debug.Log(Debug.LogLevel.Info, $"YAML Installed: {isInstalled}");

            return isInstalled;
        }

        public static async Awaitable InstallYAML(string repoPath, string YAMLMergePath)
        {

            var addYAMLToolCommand = Utility.Git(repoPath, "config", "merge.tool", "unityyamlmerge"); //git config merge.tool unityyamlmerge
            var trustExitCodeCommand = Utility.Git(repoPath, "config", "mergetool.unityyamlmerge.trustExitCode", "false"); //git config mergetool.unityyamlmerge.trustExitCode false
            var disableOrigCommand = Utility.Git(repoPath, "config", "--global", "mergetool.keepBackup", Settings.CreateBackupOnMerge.ToString()); //"git config --global mergetool.keepBackup false";
            var AddYAMLPath = Utility.Git(repoPath, "config", "mergetool.unityyamlmerge.cmd", $"\'{YAMLMergePath}\'", "merge", "-p", "\"$BASE\"", "\"$REMOTE\"", "\"$LOCAL\"", "\"$MERGED\""); //"git config mergetool.unityyamlmerge.cmd [PATH_TO_YAML_TOOL_EXE] merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"";

            try
            {
                await CommandUtility.Run(addYAMLToolCommand);
                await CommandUtility.Run(trustExitCodeCommand);
                await CommandUtility.Run(disableOrigCommand);
                await CommandUtility.Run(AddYAMLPath);
            }
            catch 
            {
                Debug.Log(Debug.LogLevel.Error, "Unable to Setup YAML Merge Tool");
            }


        }





    }
}
