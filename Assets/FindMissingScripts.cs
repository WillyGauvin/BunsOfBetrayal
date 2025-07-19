using UnityEngine;
using UnityEditor;

public class FindMissingScripts
{
    [MenuItem("Tools/Find Missing Scripts")]
    static void Find()
    {
        int count = 0;
        foreach (GameObject go in Object.FindObjectsOfType<GameObject>())
        {
            Component[] components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.LogWarning($"Missing script on: {GetFullPath(go)}", go);
                    count++;
                }
            }
        }
        Debug.Log($"Found {count} GameObject(s) with missing scripts.");
    }

    static string GetFullPath(GameObject obj)
    {
        return obj.transform.parent == null
            ? obj.name
            : GetFullPath(obj.transform.parent.gameObject) + "/" + obj.name;
    }
}