using UnityEngine;

namespace Menu
{
    public class Gamemode : MonoBehaviour
    {
        public void SinglePlayer()
        {
            Scenes.SetInt("Gamemode", 0);   // 0 = Bot; 1 = Bluetooth; 2 = Wifi;
            Scenes.Load("Game");
        }

        public void Multiplayer()
        {
            Scenes.Load("Multiplayer");
        }

        public void Back()
        {
            Scenes.Load("Main");
        }
    }
}
