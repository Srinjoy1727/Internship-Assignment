using UnityEngine;

[CreateAssetMenu(fileName = "New Obstacle Data", menuName = "Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    public Vector2Int gridSize; // Define grid size here
    public bool[] obstacles; // Define array to store obstacle data

    // Method to check if there is an obstacle at a given position
    public bool HasObstacleAtPosition(Vector2Int position)
    {
        int index = position.y * gridSize.x + position.x;
        if (index < 0 || index >= obstacles.Length)
        {
            Debug.LogWarning("Invalid position for obstacle check.");
            return false;
        }

        return obstacles[index];
    }
}
