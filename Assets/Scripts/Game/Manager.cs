using UnityEngine;
using UnityEngine.Networking;

namespace Game
{
    public class Manager : MonoBehaviour
    {
        public static GameManager GameManager;
        public static MultiplayerManager MultiplayerManager;

        private void Start()
        {
            GameManager = GetComponentInChildren<GameManager>();
            MultiplayerManager = GetComponentInChildren<MultiplayerManager>();
        }
    }
}
