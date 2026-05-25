using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KillZone))]
public class KillZoneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KillZone killZone = (KillZone)target;

        GUILayout.Space(8);
        GUILayout.Label("Visibility Controls", EditorStyles.boldLabel);

        if (killZone != null)
        {
            if (killZone.isHidden)
            {
                if (GUILayout.Button("Show KillZone"))
                {
                    killZone.ShowRenderers();
                    EditorUtility.SetDirty(killZone);
                }
            }
            else
            {
                if (GUILayout.Button("Hide KillZone"))
                {
                    killZone.HideRenderers();
                    EditorUtility.SetDirty(killZone);
                }
            }
        }
    }
}