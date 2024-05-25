using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private bool isMoving = false;
    private GridGenerator gridGenerator;

    void Start()
    {
        gridGenerator = FindObjectOfType<GridGenerator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
                if (tileInfo != null && !tileInfo.hasObstacle)
                {
                    List<TileInfo> path = FindPath(transform.position, tileInfo.transform.position);
                    if (path != null && path.Count > 0)
                    {
                        StartCoroutine(FollowPath(path));
                    }
                }
            }
        }
    }

    List<TileInfo> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        TileInfo startTile = gridGenerator.GetTileFromWorldPos(startPos);
        TileInfo targetTile = gridGenerator.GetTileFromWorldPos(targetPos);

        if (startTile == null || targetTile == null)
        {
            return null;
        }

        List<TileInfo> openSet = new List<TileInfo>();
        HashSet<TileInfo> closedSet = new HashSet<TileInfo>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            TileInfo currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentTile.fCost || (openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost))
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == targetTile)
            {
                return RetracePath(startTile, targetTile);
            }

            foreach (TileInfo neighbour in gridGenerator.GetNeighbours(currentTile))
            {
                if (neighbour.hasObstacle || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetTile);
                    neighbour.parent = currentTile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    List<TileInfo> RetracePath(TileInfo startTile, TileInfo endTile)
    {
        List<TileInfo> path = new List<TileInfo>();
        TileInfo currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        path.Reverse();
        return path;
    }

    IEnumerator FollowPath(List<TileInfo> path)
    {
        isMoving = true;
        foreach (TileInfo tile in path)
        {
            while (Vector3.Distance(transform.position, tile.transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, tile.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        isMoving = false;
    }

    int GetDistance(TileInfo tileA, TileInfo tileB)
    {
        int dstX = Mathf.Abs(tileA.x - tileB.x);
        int dstY = Mathf.Abs(tileA.y - tileB.y);
        return dstX + dstY;
    }
}
