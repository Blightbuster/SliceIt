using Game;
using Other;
using UnityEngine;

namespace Menu
{
    public class Gamemode : MonoBehaviour
    {
        public void SinglePlayerButton()
        {
            Scenes.SetString("GameMode", "Bot");
            Scenes.SetString("OpponentName", "Bot");
            Scenes.Load("Game");
        }

        public void MultiplayerButton()
        {
            if (!MultiplayerManager.Instance.LoggedIn)
            {
                Other.Tools.CreatePopup(Other.Tools.Messages.LoginRequired);
                return;
            }
            MenuManager.Instance.Load("Multiplayer");
        }

        public void Back()
        {
            MenuManager.Instance.Load("Main");
        }
    }
}
