using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueMoveModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
        {
            Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(oldPath) as Dialogue;
            
            Debug.Log($"old  {oldPath}");
            Debug.Log($"new  {newPath}");
            
            if (dialogue == null)
            {
                return AssetMoveResult.DidNotMove;
            }
            

            if (Path.GetDirectoryName(oldPath) != Path.GetDirectoryName(newPath))
            {
                return AssetMoveResult.DidNotMove;
            }

            dialogue.name = Path.GetFileNameWithoutExtension(newPath);
            dialogue.OnValidateUpdate();
            dialogue.OnBeforeSerialize();
            
            return AssetMoveResult.DidNotMove;
        }
    }
}

