using RPG.Dialogue;
using UnityEditor;
using UnityEngine;

public class DialogueModificationProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (string importedAsset in importedAssets)
        {
            Dialogue dialogue = AssetDatabase.LoadAssetAtPath(importedAsset,typeof(Dialogue)) as Dialogue;

            if (dialogue != null)
            {
                dialogue.CreateRootNode();
            }
        }
    }
}
