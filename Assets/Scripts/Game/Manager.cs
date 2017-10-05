using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class Manager : MonoBehaviour
    {
        public static GameManager GameManager;
        public static NetworkManager NetworkManager;

        void Start()
        {
            GameManager = GetComponentInChildren<GameManager>();
            NetworkManager = GetComponentInChildren<NetworkManager>();
        }
    }
}
