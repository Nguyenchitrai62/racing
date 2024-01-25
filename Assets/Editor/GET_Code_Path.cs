using UnityEditor;
using UnityEngine;

public class GET_Code_Path
{
    [MenuItem("GameObject/GET_Code_Path", false, 1)]
    private static void CopyUnityFindPath()
    {
        if (Selection.activeTransform != null)
        {
            string path = GetGameObjectFindPath(Selection.activeTransform);
            EditorGUIUtility.systemCopyBuffer = path;
            Debug.Log("Copied to clipboard: " + path);
        }
    }

    private static string GetGameObjectFindPath(Transform transform)
    {
        if (transform == null) return "";

        // Tìm root object
        Transform rootTransform = transform;
        while (rootTransform.parent != null)
        {
            rootTransform = rootTransform.parent;
        }

        // Đường dẫn cho GameObject.Find
        string rootPath = rootTransform.name;
        string childPath = GetChildPath(rootTransform, transform, "");

        // Nếu không có con, chỉ trả về GameObject.Find
        if (string.IsNullOrEmpty(childPath))
        {
            return $"GameObject.Find(\"{rootPath}\")";
        }

        // Tạo chuỗi đường dẫn hoàn chỉnh
        return $"GameObject.Find(\"{rootPath}\").transform.Find(\"{childPath}\").gameObject";
    }

    private static string GetChildPath(Transform root, Transform child, string path)
    {
        if (child == root) return path;
        string newPath = child.name + (string.IsNullOrEmpty(path) ? "" : "/") + path;
        return GetChildPath(root, child.parent, newPath);
    }
}
