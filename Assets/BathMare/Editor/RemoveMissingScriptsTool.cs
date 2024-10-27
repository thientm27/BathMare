using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class RemoveMissingScriptsTool : EditorWindow
{
    [MenuItem("Tools/Remove Missing Scripts")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(RemoveMissingScriptsTool));
    }

    private void OnGUI()
    {
        GUILayout.Label("Remove Missing Scripts Tool", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Remove Missing Scripts in Current Scene"))
        {
            RemoveMissingScriptsInCurrentScene();
        }
        
        if (GUILayout.Button("Remove Missing Scripts in Prefabs"))
        {
            RemoveMissingScriptsInAllPrefabs();
        }
    }

    private static void RemoveMissingScriptsInCurrentScene()
    {
        var rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();
        int removedCount = 0;
        
        foreach (var rootObject in rootObjects)
        {
            removedCount += RemoveMissingScripts(rootObject);
        }
        
        Debug.Log($"Removed {removedCount} missing scripts from current scene.");
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    private static void RemoveMissingScriptsInAllPrefabs()
    {
        string[] allPrefabs = AssetDatabase.FindAssets("t:Prefab");
        int totalRemoved = 0;

        foreach (string prefabGUID in allPrefabs)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null)
            {
                int removedCount = RemoveMissingScripts(prefab);
                if (removedCount > 0)
                {
                    totalRemoved += removedCount;
                    PrefabUtility.SavePrefabAsset(prefab);
                    Debug.Log($"Removed {removedCount} missing scripts from {prefabPath}");
                }
            }
        }

        Debug.Log($"Total missing scripts removed from prefabs: {totalRemoved}");
    }

    private static int RemoveMissingScripts(GameObject gameObject)
    {
        int removedCount = 0;
        var components = new List<Component>(gameObject.GetComponents<Component>());

        for (int i = components.Count - 1; i >= 0; i--)
        {
            if (components[i] == null)
            {
                Undo.RegisterCompleteObjectUndo(gameObject, "Remove Missing Scripts");
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                removedCount++;
            }
        }

        foreach (Transform child in gameObject.transform)
        {
            removedCount += RemoveMissingScripts(child.gameObject);
        }

        return removedCount;
    }
}
