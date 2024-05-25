using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleData))]
public class ObstacleEditorTool : Editor
{
    private bool[] obstacles;

    public override void OnInspectorGUI()
    {
        ObstacleData obstacleData = (ObstacleData)target;

        GUILayout.Space(20);
        GUILayout.Label("Obstacle Grid:");

        if (obstacles == null || obstacles.Length != 100)
        {
            obstacles = obstacleData.obstacles;
        }

        GUILayout.BeginVertical("box");

        for (int y = 0; y < 10; y++)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                obstacles[index] = GUILayout.Toggle(obstacles[index], "");
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        if (GUI.changed)
        {
            obstacleData.obstacles = obstacles;
            EditorUtility.SetDirty(obstacleData); // Ensure changes are saved
        }
    }
}

