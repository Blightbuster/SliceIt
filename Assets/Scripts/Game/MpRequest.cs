using UnityEngine;

namespace Game
{
    public class MpRequest : MonoBehaviour
    {
        public enum Action
        {
            Register,
            Login,
            QuickMatch,
            CancelQuickMatch,
            CreatePrivateGame,
            JoinPrivateGame,
            GetAvaibleGames,
            FinishMove
        }

        public abstract class BaseRequest
        {
            public string ClientName = ZPlayerPrefs.GetString("ClientName", "");
            public string ClientPassword = ZPlayerPrefs.GetString("ClientPassword", "");
            public string ActionType;
        }

        public class Register : BaseRequest
        {
            public Register()
            {
                ActionType = Action.Register.ToString();
            }
        }

        public class Login : BaseRequest
        {
            public Login()
            {
                ActionType = Action.Login.ToString();
            }
        }

        public class QuickMatch : BaseRequest
        {
            public QuickMatch()
            {
                ActionType = Action.QuickMatch.ToString();
            }
        }

        public class CancelQuickMatch : BaseRequest
        {
            public CancelQuickMatch()
            {
                ActionType = Action.CancelQuickMatch.ToString();
            }
        }

        public class CreatePrivateGame : BaseRequest
        {
            public string GamePassword = "";
            public CreatePrivateGame()
            {
                ActionType = Action.CreatePrivateGame.ToString();
            }
        }

        public class JoinPrivateGame : BaseRequest
        {
            public string GamePassword = "";
            public string GameName = "";
            public JoinPrivateGame()
            {
                ActionType = Action.JoinPrivateGame.ToString();
            }
        }

        public class GetAvaibleGames : BaseRequest
        {
            public GetAvaibleGames()
            {
                ActionType = Action.GetAvaibleGames.ToString();
            }
        }

        public class FinishMove : BaseRequest
        {
            public float Mass = 0;
            public FinishMove()
            {
                ActionType = Action.FinishMove.ToString();
            }
        }
    }
}
