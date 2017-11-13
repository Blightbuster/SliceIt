using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MpResponse : MonoBehaviour
    {
        public abstract class BaseResponse { }

        public class Status : BaseResponse
        {
            public bool Success;
            public string ErrorLevel;
        }

        public class Token : BaseResponse
        {
            public string ClientToken;
        }

        public class List : BaseResponse
        {
            public List<string> Data;
        }

        public class Move : BaseResponse
        {
            public float Mass;
        }
    }
}
