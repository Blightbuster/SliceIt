using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGO : MonoBehaviour
{
    private Vector3 lowerRight;

    // Use this for initialization
    void Start()
	{
		Vector3 lowerRightScreen = new Vector3(Screen.width, 0, 0); //Screen coordinate corner location
        lowerRight = Camera.main.ScreenToWorldPoint(lowerRightScreen);  //Corner locatios in world coordinates

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
