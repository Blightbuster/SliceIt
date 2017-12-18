using System;
using Game;
using Other;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Main : MonoBehaviour
    {
        public List<GameObject> SlicingObjects = new List<GameObject>();

        private void Start()
        {
            try
            {
                if (SecurePlayerPrefs.HasKey("IsRegistered") && SecurePlayerPrefs.GetInt("IsRegistered") == 1)
                {
                    if (!MultiplayerManager.Instance.LoggedIn) MultiplayerManager.Instance.Login();
                }
                else
                {
                    SecurePlayerPrefs.SetString("SlicingObject", "Sausage");
                    SecurePlayerPrefs.SetInt("IsRegistered", 0);
                }
            }
            catch (Exception) { }
            UpdateCustomizationButton();
        }

        public void UpdateCustomizationButton()
        {
            string selectedSlicingObject = SecurePlayerPrefs.GetString("SlicingObject");
            foreach (GameObject so in SlicingObjects)
            {
                so.SetActive(so.name == selectedSlicingObject);
            }
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
