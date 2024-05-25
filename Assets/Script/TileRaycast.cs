using UnityEngine;
using TMPro;  // Ensure this namespace is included for TextMeshProUGUI

public class TileRaycast : MonoBehaviour
{
    public TextMeshProUGUI tileInfoText;

    void Update()
    {
        if (tileInfoText == null)
        {
            Debug.LogError("tileInfoText is not assigned in the Inspector");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                tileInfoText.text = $"Tile Position: ({tileInfo.x}, {tileInfo.y})";
            }
            else
            {
                tileInfoText.text = "";
            }
        }
        else
        {
            tileInfoText.text = "";
        }
    }
}
