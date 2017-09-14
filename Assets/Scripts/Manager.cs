using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static GameObject GameManager;

    void Start()
    {
        GameManager = GameObject.Find("GameManager");
    }
}
