﻿using cakeslice;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Display;

    public GameType GameMode = GameType.Computer;

    public enum GameType
    {
        Computer,
        Bluetooth,
        Wifi
    }

    // Update is called once per frame
    void Update()
    {
        // Get weight of all slices -> Sum them up -> Display the value on the screen
        float totalWeigh = 0;
        foreach (GameObject go in GetComponent<TagController>().TagWeigh)
        {
            totalWeigh += go.GetComponent<Rigidbody2D>().mass;
            go.GetComponent<Outline>().color = 1; // Change outline-color of all slices on the scale
        }
        Display.GetComponent<DisplayController>().TargetNumber = totalWeigh;

        foreach (SpriteSlicer2DSliceInfo sliceInfo in GetComponent<ObjectSlicer>().SlicedSpriteInfo)
        {
            foreach (GameObject go in sliceInfo.ChildObjects)
            {
                if (go.GetComponent<Outline>().color == 1 && !GetComponent<TagController>().TagWeigh.Contains(go))
                {
                    go.GetComponent<Outline>().color = 0;
                }
            }
        }
    }

    public void FinishMove()
    {
        if (GameMode == GameType.Computer)
        {
            foreach (GameObject slice in GetComponent<TagController>().TagWeigh)
            {
                slice.AddComponent<FadeAndDestroy>();
            }
        }
    }
}
