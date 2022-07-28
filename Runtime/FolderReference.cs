using System.IO;
using UnityEditor;
using UnityEngine;
 
namespace RishUI.EditorUtils
{
    [System.Serializable]
    public class FolderReference
    {
        public string GUID;
 
        public string Path => AssetDatabase.GUIDToAssetPath(GUID);
    //    public DefaultAsset Asset => AssetDatabase.LoadAssetAtPath<DefaultAsset>(Path);
    }
}