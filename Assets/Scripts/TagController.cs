using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagController : MonoBehaviour
{
    public List<GameObject> TagWeigh;

    public GameObject Scale;

    private Vector2 scalLowerLeftCorner;

    // Use this for initialization
    void Start()
    {
        Bounds bounds = Scale.GetComponent<SpriteRenderer>().bounds; // Get the boundingbox of the parents sprite
        scalLowerLeftCorner = bounds.min; // bounds.min is the lower-left corner of the sprite
    }

    // Update is called once per frame
    void Update()
    {
        TagWeigh.Clear();

        foreach (SpriteSlicer2DSliceInfo sliceInfo in GetComponent<ObjectSlicer>().SlicedSpriteInfo)
        {
            foreach (GameObject go in sliceInfo.ChildObjects)
            {
                if (go.activeSelf && go.GetComponent<MeshRenderer>().bounds.center.x > scalLowerLeftCorner.x && go.GetComponent<GroundChecker>().IsGrounded) TagWeigh.Add(go);
            }
        }
    }
}
