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
    }
}
