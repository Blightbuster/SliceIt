using UnityEngine;
using UnityEngine.UI;

// Only used for debugging
namespace Game
{
    public class GameStateDisplay : MonoBehaviour {
        // Update is called once per frame
        void Update ()
        {
            GetComponent<Text>().text = Manager.GameManager.State.ToString();
        }
    }
}
