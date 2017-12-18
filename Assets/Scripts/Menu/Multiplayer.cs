using Game;
using Other;
using UnityEngine;

namespace Menu
{
    public class Multiplayer : MonoBehaviour
    {
        public GameObject Menu;

        public void QuickMatchButton()
        {
            MultiplayerManager.Instance.QuickMatch();
            Menu.transform.Find("WaitingForOpponent").gameObject.SetActive(true);
            Menu.transform.Find("QuickMatch").gameObject.SetActive(false);
        }

        public void CancelQuickMatchButton()
        {
            MultiplayerManager.Instance.CancelQuickMatch();
            Menu.transform.Find("WaitingForOpponent").gameObject.SetActive(false);
            Menu.transform.Find("QuickMatch").gameObject.SetActive(true);
        }

        public void PrivateMatchButton()
        {
            Other.Tools.CreatePopup(Other.Tools.Messages.ComingSoon);
            return;
            CancelQuickMatchButton();
            MenuManager.Instance.Load("PrivateMatch");
        }

        public void Back()
        {
            CancelQuickMatchButton();
            MenuManager.Instance.Load("Gamemode");
        }
    }
}
