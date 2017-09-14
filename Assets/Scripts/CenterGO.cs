using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGO : MonoBehaviour
{
    public Vector2 ViewportCorner;

    // Use this for initialization
    void Start()
    {
        //Corner locations in world coordinates
        Vector2 worldCorner = Camera.main.ViewportToWorldPoint(ViewportCorner);

        AlignDisplay(worldCorner);
    }

    private void AlignDisplay(Vector2 worldCorner)
    {
        Bounds bounds = transform.parent.GetComponent<SpriteRenderer>().bounds; // Get the boundingbox of the parents sprite
        Vector2 parentTopLeft = bounds.min; // bounds.min is the lower-left corner of the sprite
        parentTopLeft.y += bounds.size.y;   // Add the height of the sprite to the lower-left corner to get the upper-right corner
        Vector2 parentCenter = (parentTopLeft + worldCorner) / 2;    // Calculate the middle betwen both vectors
        transform.position = parentCenter;
    }
}
