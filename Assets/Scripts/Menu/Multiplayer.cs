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
            MenuManager.Instance.Load("PrivateMatch");
        }

        public void Back()
        {
            MenuManager.Instance.Load("Gamemode");
        }
    }
}
