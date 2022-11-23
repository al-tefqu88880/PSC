using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileInfoDisplay : MonoBehaviour
{


    public Vector2 GetHex(Vector3 point)
    {
        int column;
        int row;
        float radius = 1.0;

        // Find out which major row and column we are on:
        row = (int)(point.z / 0.87f);
        column = (int)(point.x / (radius + radius / 2));

        // Compute the offset into these row and column:
        float dz = point.z - (float)row * 0.87f;
        float dx = point.x - (float)column * (radius + radius / 2);

        // Are we on the left of the hexagon edge, or on the right?
        if (((row ^ column) & 1) == 0)
        {
            dz = 0.87f - dz;
        }

        int right = dz * (radius - radius / 2) < 0.87f * (dx - radius / 2) ? 1 : 0;

        // Now we have all the information we need, just fine-tune row and column.
        row += (column ^ row ^ right) & 1;
        column += right;

        return new Vector2(Mathf.RoundToInt(column), Mathf.RoundToInt(row / 2));
    }


    TMP_Text text;
    public GridLayout grid;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
            text.SetText(Input.mousePosition.ToString());       //setup backend tilemap read data
            //Debug.Log(hitInfo.point.ToString());
        }
        else
        {
            //text.SetText("");
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3Int cell = grid.WorldToCell(mousePos);
        text.SetText(cell.ToString());

    }
}
