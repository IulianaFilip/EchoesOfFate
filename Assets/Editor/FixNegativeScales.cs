using UnityEditor;
using UnityEngine;

public class FixNegativeScales : EditorWindow
{
    [MenuItem("Tools/Fix Negative Scales")]
    public static void ShowWindow()
    {
        if (EditorUtility.DisplayDialog("Fix Negative Scales",
            "This will scan the scene and change all negative scale values to positive. Continue?", "Yes", "Cancel"))
        {
            int fixedCount = 0;

            foreach (Transform t in GameObject.FindObjectsOfType<Transform>())
            {
                Vector3 localScale = t.localScale;
                bool changed = false;

                if (localScale.x < 0) { localScale.x = Mathf.Abs(localScale.x); changed = true; }
                if (localScale.y < 0) { localScale.y = Mathf.Abs(localScale.y); changed = true; }
                if (localScale.z < 0) { localScale.z = Mathf.Abs(localScale.z); changed = true; }

                if (changed)
                {
                    Undo.RecordObject(t, "Fix Negative Scale");
                    t.localScale = localScale;
                    fixedCount++;
                }
            }

            EditorUtility.DisplayDialog("Done", $"Fixed negative scale on {fixedCount} objects.", "OK");
        }
    }
}

