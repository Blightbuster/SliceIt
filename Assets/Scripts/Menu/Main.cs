using Game;
using Other;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            if (!MultiplayerManager.Instance.LoggedIn) MultiplayerManager.Instance.Login();
            Scenes.SetString("SlicingObject", "Banana");
        }

        public void PlayButton()
        {
            MenuManager.Instance.Load("Gamemode");
        }

        public void StatisticsButton()
        {
            MenuManager.Instance.Load("Statistics");
        }

        public void CustomisationButton()
        {
            MenuManager.Instance.Load("Customisation");
        }

        public void SettingsButton()
        {
            MenuManager.Instance.Load("Settings");
        }
    }
}
