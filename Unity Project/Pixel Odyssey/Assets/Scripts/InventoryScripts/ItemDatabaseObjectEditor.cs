//ItemDatabaseObjectEditor

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(ItemDatabaseObject))]
public class ItemDatabaseObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemDatabaseObject database = (ItemDatabaseObject)target;

        if (GUILayout.Button("Populate Items"))
        {
            string[] guids = AssetDatabase.FindAssets("t:ItemObject");
            database.Items = guids.Select(guid => AssetDatabase.LoadAssetAtPath<ItemObject>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif