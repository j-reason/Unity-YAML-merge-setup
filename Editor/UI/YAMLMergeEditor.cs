using UnityEditor;
using UnityEngine;
using YAMLMergeInEditor.Commands;

namespace YAMLMergeInEditor.
public class YAMLMergeEditor : EditorWindow
{

    [MenuItem("Tools/Git/My Editor Window")]
    public static void ShowExample()
    {
        YAMLMergeEditor wnd = GetWindow<YAMLMergeEditor>(utility: true, title: "YAML Merge Tool");
        wnd.Show();
    }


    private void OnGUI()
    {
        
        if (!GitCommands.TryGetMergeCommit(Settings.DefaultRepoPath, out string commitHash))
        {
            EditorGUILayout.LabelField("No Ongoing Merge Detect. Start merge using Git Tool of choice");
        }



    }

}
