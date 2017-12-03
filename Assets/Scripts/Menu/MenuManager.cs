using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;
        public List<GameObject> Menu = new List<GameObject>();

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void Load(string newMenu)
        {
            foreach (GameObject menu in Menu)
            {
                menu.SetActive(menu.name == newMenu);
            }
        }
    }
}
