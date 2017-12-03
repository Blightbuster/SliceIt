using Game;
using Other;
using UnityEngine;

namespace Menu
{
    public class PrivateMatch : MonoBehaviour
    {
        public void JoinButton() { }

        public void CreateButton() { }

        public void Back()
        {
            MenuManager.Instance.Load("Multiplayer");
        }
    }
}
