using Game;
using Other;
using UnityEngine;

namespace Menu
{
    public class Multiplayer : MonoBehaviour
    {
        public void QuickMatchButton()
        {
            MultiplayerManager.Instance.QuickMatch();
        }

        public void PrivateMatchButton()
        {
            Scenes.Load("PrivateMatch");
        }

        public void Back()
        {
            Scenes.Load("Gamemode");
        }
    }
}
