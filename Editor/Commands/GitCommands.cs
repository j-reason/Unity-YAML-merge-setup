using CliWrap;
using CliWrap.Buffered;
using System.IO;
using UnityEngine;
using Debug = YAMLMergeInEditor.Debug;

namespace YAMLMergeInEditor.Commands
{
    public class GitCommands
    {

        public static async Awaitable<bool> isProjectGitRepo(string repoPath)

        {

            var command = CommandUtility.Git(repoPath, "rev-parse", "--is-inside-work-tree");
            bool isGitRepo = await CommandUtility.RunAndCheck(command, output => bool.Parse(output));

            if (!isGitRepo)
                Debug.Log(Debug.LogLevel.Warn, $"{repoPath} is not a Git Repository. Unable to use YAML Smart Merge");

            return isGitRepo;
        }

        public static async Awaitable<bool> isGitInstalled(string repoPath)
        {
            //run git --version and if it failes git ins't installed
            var command = CommandUtility.Git(repoPath, "--version"); //git --version
            bool response;

            //Can't use CommandUtility.RunAndCheck() because the catch is specifically what's being looked for
            try
            {
                Debug.Log(Debug.LogLevel.Trace, $"Running: {command}");
                var output = await command.ExecuteAsync();
                response = output.IsSuccess;
            }
            catch
            {
                response = false;
            }

            Debug.Log(Debug.LogLevel.Debug, $"Git Installed: {response}");

            if (!response)
                Debug.Log(Debug.LogLevel.Warn, $"No Git Instance found. Unable to use YAML Smart Merge");

            return response;
        }

        public static bool TryGetMergeCommit(string repositoryPath, out string commitHash)
        {
            string mergeHeadPath = Path.Combine(repositoryPath, ".git", "MERGE_HEAD");
            Debug.Log(Debug.LogLevel.Trace, $"Looking for MERGE_HEAD at {mergeHeadPath}");
            if (!File.Exists(mergeHeadPath))
            {
                Debug.Log(Debug.LogLevel.Trace, $"No MERGE_HEAD found");
                commitHash = null;
                return false;
            }


            commitHash = File.ReadAllText(mergeHeadPath);
            Debug.Log(Debug.LogLevel.Trace, $"MERGE_HEAD found. Commit Hash: {commitHash}");
            return true;
        }

        public async static void GetMergeConflictFiles(string repositoryPath)
        {
            var command = CommandUtility.Git(repositoryPath, "diff", "--name-only", "--diff-filter=U", "--relative"); //git diff --name-only --diff-filter=U --relative
            string output = await CommandUtility.RunGetOutput(command);

            Debug.Log(Debug.LogLevel.Trace, $"Merge Conflict Files: \r\n{output}");
        }

    }

}