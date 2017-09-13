using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGO : MonoBehaviour
{
    private Vector3 lowerRight;

    // Use this for initialization
    void Start()
    {
        // Screens coordinate corner location
        Vector3 upperLeftScreen = new Vector3(0, Screen.height, 0);
        Vector3 upperRightScreen = new Vector3(Screen.width, Screen.height, 0);
        Vector3 lowerLeftScreen = new Vector3(0, 0, 0);
        Vector3 lowerRightScreen = new Vector3(Screen.width, 0, 0);

        //Corner locations in world coordinates
        Vector3 upperLeft = Camera.main.ScreenToWorldPoint(upperLeftScreen);
        Vector3 upperRight = Camera.main.ScreenToWorldPoint(upperRightScreen);
        Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(lowerLeftScreen);
        lowerRight = Camera.main.ScreenToWorldPoint(lowerRightScreen);

        AlignDisplay();
    }

    private void AlignDisplay()
    {
        Vector3 parentPosition = transform.parent.position;
        parentPosition -= GetComponentInParent<SpriteRenderer>().bounds.size;
        parentPosition.y += GetComponentInParent<SpriteRenderer>().bounds.size.y * 2;
        Vector3 parentCenter = (parentPosition + lowerRight) / 2;
        transform.position = parentCenter;
    }
}
