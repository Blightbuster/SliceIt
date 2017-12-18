using Game;
using Other;
using UnityEngine;

namespace Menu
{
    public class Customisation : MonoBehaviour
    {
        public void BackButton()
        {
            MenuManager.Instance.Load("Main");
        }

        public void SetSlicingObject(string type)
        {
            SecurePlayerPrefs.SetString("SlicingObject", type);
            MenuManager.Instance.Load("Main");
            MenuManager.Instance.GetComponent<Main>().UpdateCustomizationButton();
        }
    }
}
