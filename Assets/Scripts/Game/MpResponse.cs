using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MpResponse : MonoBehaviour
    {
        public class Status
        {
            public bool Succes;
            public string ErrorLevel;
        }

        public class List
        {
            public List<string> Data;
        }

        public class Move
        {
            public float Mass;
        }
    }
}
