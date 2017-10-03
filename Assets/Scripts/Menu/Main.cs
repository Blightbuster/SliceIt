using UnityEngine;

namespace Menu
{
    public class Main : MonoBehaviour
    {
        public void PlayButton()
        {
            Scenes.Load("Gamemode");
        }

        public void StatisticsButton()
        {
            Scenes.Load("Statistics");
        }

        public void CustomisationButton()
        {
            Scenes.Load("Customisation");
        }

        public void SettingsButton()
        {
            Scenes.Load("Settings");
        }

    }
}
