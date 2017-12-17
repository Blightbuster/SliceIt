using Game;
using Other;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Settings : MonoBehaviour
    {
        public InputField Username;
        public InputField Password;

        private void Start()
        {
            Username.text = SecurePlayerPrefs.GetString("ClientName");
            Password.text = SecurePlayerPrefs.GetString("ClientPassword");
        }

        public void Back()
        {
            MenuManager.Instance.Load("Main");
        }

        public void LoginButton()
        {
            MultiplayerManager.Instance.Login(Username.text, Password.text);
        }

        public void RegisterButton()
        {
            MultiplayerManager.Instance.Register(Username.text, Password.text);
        }
    }
}
