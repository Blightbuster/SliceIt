using UnityEngine;

namespace Menu
{
    public class Multiplayer : MonoBehaviour
    {
        public void Join() { }

        public void Create() { }

        public void Back()
        {
            Scenes.Load("Gamemode");
        }
    }
}
