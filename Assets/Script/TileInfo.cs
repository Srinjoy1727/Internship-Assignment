
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int x;
    public int y;
    public bool hasObstacle;
    public int gCost;
    public int hCost;
    public TileInfo parent;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public void SetPosition(int x, int y, bool hasObstacle)
    {
        this.x = x;
        this.y = y;
        this.hasObstacle = hasObstacle;
        UpdateTileColor();
    }

    void UpdateTileColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (hasObstacle)
        {
            renderer.material.color = Color.red;
        }
        else
        {
            renderer.material.color = Color.white;
        }
    }
}
