using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private bool isMoving = false;
    private GridGenerator gridGenerator;
    private PlayerController playerController;

    void Start()
    {
        gridGenerator = FindObjectOfType<GridGenerator>();
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (!isMoving)
        {
            // Get the player's tile
            TileInfo playerTile = gridGenerator.GetTileFromWorldPos(playerController.transform.position);
            if (playerTile != null)
            {
                // Get the adjacent tiles to the player's tile
                List<TileInfo> adjacentTiles = gridGenerator.GetAdjacentTiles(playerTile);
                // Find the closest non-obstacle tile among the adjacent tiles
                TileInfo targetTile = GetClosestNonObstacleTile(adjacentTiles);
                if (targetTile != null)
                {
                    // Start moving towards the target tile
                    StartCoroutine(MoveToTile(targetTile));
                }
            }
        }
    }

    TileInfo GetClosestNonObstacleTile(List<TileInfo> tiles)
    {
        TileInfo closestTile = null;
        float closestDistance = Mathf.Infinity;

        foreach (TileInfo tile in tiles)
        {
            // Check if the tile is not an obstacle
            if (!tile.hasObstacle)
            {
                float distance = Vector3.Distance(transform.position, tile.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTile = tile;
                }
            }
        }

        return closestTile;
    }

    IEnumerator MoveToTile(TileInfo tile)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, tile.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, tile.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }
}

