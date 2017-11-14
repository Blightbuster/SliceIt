using Game;
using UnityEngine;

namespace Other
{
    public class MpRequest : MonoBehaviour
    {
        // All Actions which are used in the requests
        public enum Action
        {
            Register,
            Login,
            Logout,
            QuickMatch,
            CancelQuickMatch,
            CreatePrivateGame,
            JoinPrivateGame,
            GetAvaibleGames,
            FinishMove
        }

        // --- Classes for inheritance ---

        public abstract class BaseRequest
        {
            public string ActionType;
        }

        public abstract class PasswordAuth : BaseRequest
        {
            public string ClientName = SecurePlayerPrefs.GetString("ClientName", "");
            public string ClientPassword = SecurePlayerPrefs.GetString("ClientPassword", "");
        }

        public abstract class TokenAuth : BaseRequest
        {
            public string ClientToken = SecurePlayerPrefs.GetString("ClientToken", "");
        }

        // --- All Request below are send to the Server ---

        public class Register : PasswordAuth
        {
            public Register()
            {
                ActionType = Action.Register.ToString();
            }
        }

        public class Login : PasswordAuth
        {
            public Login()
            {
                ActionType = Action.Login.ToString();
            }
        }

        public class Logout : TokenAuth
        {
            public Logout()
            {
                ActionType = Action.Logout.ToString();
            }
        }

        public class QuickMatch : TokenAuth
        {
            public QuickMatch()
            {
                ActionType = Action.QuickMatch.ToString();
            }
        }

        public class CancelQuickMatch : TokenAuth
        {
            public CancelQuickMatch()
            {
                ActionType = Action.CancelQuickMatch.ToString();
            }
        }

        public class CreatePrivateGame : TokenAuth
        {
            public string GamePassword = "";
            public CreatePrivateGame()
            {
                ActionType = Action.CreatePrivateGame.ToString();
            }
        }

        public class JoinPrivateGame : TokenAuth
        {
            public string GameName = "";
            public string GamePassword = "";
            public JoinPrivateGame()
            {
                ActionType = Action.JoinPrivateGame.ToString();
            }
        }

        public class GetAvaibleGames : TokenAuth
        {
            public GetAvaibleGames()
            {
                ActionType = Action.GetAvaibleGames.ToString();
            }
        }

        public class FinishMove : TokenAuth
        {
            public float Mass = 0;
            public FinishMove()
            {
                ActionType = Action.FinishMove.ToString();
            }
        }
    }
}
