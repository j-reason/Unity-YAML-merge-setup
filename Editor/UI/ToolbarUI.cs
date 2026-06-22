using Mono.Cecil;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using YAMLMergeInEditor.Commands;

namespace YAMLMergeInEditor
{
    public static class ToolbarUI
    {


        [MenuItem("Tools/Git/CLiWrapTest")]
        public static void TestGit()
        {

            Setup.RunSetup();



        }

        [MenuItem("Tools/Git/Test Merge")]
        public static void TestMerge()
        {
            if(GitCommands.TryGetMergeCommit(Settings.DefaultRepoPath, out var hash))
            {
                GitCommands.GetMergeConflictFiles(Settings.DefaultRepoPath);
            }
        }



    } 
}
