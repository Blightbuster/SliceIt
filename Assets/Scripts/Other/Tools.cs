using Game;
using UnityEngine;

namespace Other
{
    public class Tools
    {
        public enum DisplayTime
        {
            Short = 1,
            Medium = 2,
            Long = 3
        }

        public class Messages
        {
            public static readonly string Error = "An error occured";
            public static readonly string ConnectionError = "Can't connect to server";
            public static readonly string ConnectionSuccess = "Connected to server";
            public static readonly string DisconnectServer = "Disconnected from Server";
            public static readonly string RegisterSuccess = "Successfully registered";
            public static readonly string LoggedIn = "Successfully logged in";
            public static readonly string LoggedOut = "Successfully logged out";
            public static readonly string InvalidLogin = "Invalid login data";
            public static readonly string AlreadyInQueue = "You are already in queue";
            public static readonly string SearchingGames = "Searching for avaible games...";
            public static readonly string AlreadyCreatedGame = "You have already created a game";
            public static readonly string CreatePrivateGame = "Created private game";
            public static readonly string CouldntFindGame = "Couldnt find the game";
            public static readonly string InvalidGamePassword = "Wrong password";
            public static readonly string NotInGame = "Wrong password";
            public static readonly string UsernameNotAllowed = "Username not allowed";
            public static readonly string UsernameInfo = "Use letters, numbers and underscores";
            public static readonly string LoginRequired = "Login first before playing online";
        }


        public static void CreatePopup(string text, DisplayTime displayTime = Tools.DisplayTime.Medium)
        {
            Canvas canvasComponent = GameObject.Find("Canvas").GetComponent<Canvas>();
            GameObject popup = Object.Instantiate(Resources.Load<GameObject>("Popup"));
            Popup popupScript = popup.GetComponent<Popup>();
            popupScript.Text = text;
            popupScript.DisplayTime = (int) displayTime;
            popup.transform.SetParent(canvasComponent.gameObject.transform, false);
        }
    }
}