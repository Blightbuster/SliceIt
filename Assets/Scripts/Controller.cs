using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static DisplayController DisplayController;
    public static DragController DragController;
    public static TagController TagController;
    public static UIScoreController PlayerUIScoreController;
    public static UIScoreController OpponentUIScoreController;

    void Start()
    {
        DisplayController = GetComponent<DisplayController>();
        DragController = GetComponent<DragController>();
        TagController = GetComponent<TagController>();
        PlayerUIScoreController = GetComponents<UIScoreController>()[0];
        OpponentUIScoreController = GetComponents<UIScoreController>()[1];
    }
}
