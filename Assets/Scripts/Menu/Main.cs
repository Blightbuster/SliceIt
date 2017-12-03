using Game;
using Other;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Main : MonoBehaviour
    {
        public Sprite LoginSprite;
        public Sprite LogoutSprite;

        private void Start()
        {
            GameObject.Find("Login").GetComponent<Image>().sprite = MultiplayerManager.Instance.LoggedIn ? LogoutSprite : LoginSprite;
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

        public void LoginButton()
        {
            if (MultiplayerManager.Instance.LoggedIn)
            {
                if (MultiplayerManager.Instance.Logout())
                {
                    GameObject.Find("Login").GetComponent<Image>().sprite = LoginSprite;
                }
            }
            else
            {
                if (MultiplayerManager.Instance.Login())
                {
                    GameObject.Find("Login").GetComponent<Image>().sprite = LogoutSprite;
                }
            }
        }
    }
}
