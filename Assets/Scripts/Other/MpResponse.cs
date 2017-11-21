using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

namespace Other
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

        public class AvaibleGames : BaseResponse
        {
            public List<string> GameNames;
        }

        public class Move : BaseResponse
        {
            public float Mass;
        }

        public class Player : BaseResponse
        {
            public string PlayerName;
        }
    }
}
