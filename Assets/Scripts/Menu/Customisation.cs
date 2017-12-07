﻿using Other;
using UnityEngine;

namespace Menu
{
    public class Customisation : MonoBehaviour
    {
        public void BackButton()
        {
            MenuManager.Instance.Load("Main");
        }

        public void SetSlicingObject(string type)
        {
            Scenes.SetString("SlicingObject", type);
            MenuManager.Instance.Load("Main");
        }
    }
}
