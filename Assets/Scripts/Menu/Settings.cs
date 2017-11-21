using Other;
using UnityEngine;

namespace Menu
{
    public class Settings : MonoBehaviour
    {
        public void Back()
        {
            Scenes.Load("Main");
        }
    }
}
