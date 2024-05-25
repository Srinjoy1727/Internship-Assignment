using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public ObstacleData obstacleData;
    private TileInfo[,] grid;
    private int gridSizeX = 10;  // Assuming a grid size of 10x10
    private int gridSizeY = 10;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new TileInfo[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                bool hasObstacle = obstacleData.obstacles[x + y * gridSizeX];
                Vector3 position = new Vector3(x * 1.1f, 0, y * 1.1f);
                GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity);
                TileInfo tileInfo = tileObject.GetComponent<TileInfo>();
                tileInfo.SetPosition(x, y, hasObstacle);
                grid[x, y] = tileInfo;
            }
        }
    }

    public TileInfo GetTileFromWorldPos(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / 1.1f);
        int y = Mathf.RoundToInt(worldPos.z / 1.1f);

        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            return grid[x, y];
        }

        return null;
    }

    public List<TileInfo> GetNeighbours(TileInfo tile)
    {
        List<TileInfo> neighbours = new List<TileInfo>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = tile.x + x;
                int checkY = tile.y + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public List<TileInfo> GetAdjacentTiles(TileInfo tile)
    {
        List<TileInfo> adjacentTiles = new List<TileInfo>();

        int[,] directions = new int[,]
        {
            { 0, 1 },  // Up
            { 1, 0 },  // Right
            { 0, -1 }, // Down
            { -1, 0 }  // Left
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int checkX = tile.x + directions[i, 0];
            int checkY = tile.y + directions[i, 1];

            if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
            {
                adjacentTiles.Add(grid[checkX, checkY]);
            }
        }

        return adjacentTiles;
    }
}
