using Other;
using UnityEngine;

namespace Menu
{
    public class TopPlayers : MonoBehaviour
    {
        public void Back()
        {
            MenuManager.Instance.Load("Statistics");
        }
    }
}
