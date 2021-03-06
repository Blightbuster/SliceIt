﻿using UnityEngine;

namespace Game
{
    public class CenterGO : MonoBehaviour
    {
        public Vector2 ViewportCorner;

        // Use this for initialization
        private void Start()
        {
            //Corner locations in world coordinates
            Vector2 worldCorner = Camera.main.ViewportToWorldPoint(ViewportCorner);

            AlignDisplay(worldCorner);
        }

        private void AlignDisplay(Vector2 worldCorner)
        {
            var bounds = transform.parent.GetComponent<SpriteRenderer>().bounds; // Get the boundingbox of the parents sprite
            Vector2 parentTopLeft = bounds.min; // bounds.min is the lower-left corner of the sprite
            parentTopLeft.y += bounds.size.y; // Add the height of the sprite to the lower-left corner to get the upper-right corner
            var parentCenter = (parentTopLeft + worldCorner) / 2; // Calculate the middle betwen both vectors
            transform.position = parentCenter;
        }
    }
}