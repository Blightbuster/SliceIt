using Other;
using UnityEngine;

namespace Menu
{
    public class Shop : MonoBehaviour
    {
        public void Back()
        {
            MenuManager.Instance.Load("Customisation");
        }
    }
}
