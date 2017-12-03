using Other;
using UnityEngine;

namespace Menu
{
    public class Statistics : MonoBehaviour
    {
        public void Back()
        {
            MenuManager.Instance.Load("Main");
        }
    }
}
