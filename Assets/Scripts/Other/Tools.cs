using Game;
using UnityEngine;

namespace Other
{
    public class Tools
    {

        public static void CreatePopup(string text, int displayTime)
        {
            Canvas canvasComponent = GameObject.Find("Canvas").GetComponent<Canvas>();
            GameObject popup = GameObject.Instantiate(Resources.Load<GameObject>("Popup"));
            Popup popupScript = popup.GetComponent<Popup>();
            popupScript.Text = text;
            popupScript.DisplayTime = displayTime;
            popup.transform.SetParent(canvasComponent.gameObject.transform, false);
        }
    }
}