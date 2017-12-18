using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Other;
using UnityEngine;

namespace Game
{
    public class MultiplayerManager : MonoBehaviour
    {
        public bool LoggedIn = false;
        public static MultiplayerManager Instance = null;

        private const string Server = "18.217.114.209";
        private const int Port = 1531; // Gründungsjahr des Katharineums zu Lübeck

        private TcpClient _client;
        private NetworkStream _stream;
        private StreamWriter _writer;

        private static bool _connectedToServer = false;

        private static CallbackType _expectedCallback;
        private static bool _receivedCallback = false;
        private static string _callbackResponse;

        private enum CallbackType
        {
            None,
            QuickMatch,
            FinishMove
        }

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            if (_connectedToServer == false)
            {
                ConnectToServer();
            }
            if (!SecurePlayerPrefs.HasKey("IsRegistered"))
            {
                SecurePlayerPrefs.SetInt("IsRegistered", 0);
            }
        }

        private void Update()
        {
            if (!_receivedCallback) return;

            switch (_expectedCallback)
            {
                case CallbackType.FinishMove:
                    FinishMoveCallback();
                    break;
                case CallbackType.QuickMatch:
                    QuickMatchCallback();
                    break;
            }
            _expectedCallback = CallbackType.None;
            _receivedCallback = false;
        }

        public bool ConnectToServer()
        {
            try
            {
                _client = new TcpClient(Server, Port);
                _stream = _client.GetStream();
                _writer = new StreamWriter(_client.GetStream());
                _connectedToServer = true;
                Other.Tools.CreatePopup(Other.Tools.Messages.ConnectionSuccess);
                return true;
            }
            catch (Exception)
            {
                Other.Tools.CreatePopup(Other.Tools.Messages.ConnectionError);
                _connectedToServer = false;
                return false;
            }

        }

        public void DisconnetFromServer()
        {
            try
            {
                _stream.Close();
                _client.Close();
                Other.Tools.CreatePopup(Other.Tools.Messages.DisconnectServer);
            }
            catch (Exception)
            {
                Debug.Log("Something went wrong while disconnecting from the Server");
            }
            _connectedToServer = false;
        }

        /* --- Requests---
         * 
         * Schedule of requests is commonly as follown:
         * 
         * (1) Configure request
         * (2) Send request
         * (3) Check if request failed
         * (4) Handly response
         */

        public bool Register(string clientName, string clientPassword)
        {
            if (!IsUsernameAllowed(clientName)) return false;

            // Set new Login Credentials
            SecurePlayerPrefs.SetString("ClientName", clientName);
            SecurePlayerPrefs.SetString("ClientPassword", clientPassword);

            // Send request
            string rawResponse = SendRequest(new MpRequest.Register(), CallbackType.None, false);    // We dont need a token for this request -> ignore if token is expired

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                if (statusResponse.ErrorLevel == "UsernameAlreadyTaken") Debug.Log("Username already Taken");
                return false;
            }

            // Handle response
            MpResponse.Token response = JsonUtility.FromJson<MpResponse.Token>(rawResponse);
            SecurePlayerPrefs.SetString("ClientToken", response.ClientToken);           // Set new token
            SecurePlayerPrefs.SetInt("ClientTokenExpire", CurrentTimestamp() + 3600);   // We add 3600 seconds because the token will be valid for one hour
            LoggedIn = true;    // Set status
            SecurePlayerPrefs.SetInt("IsRegistered", 1);

            Other.Tools.CreatePopup(Other.Tools.Messages.RegisterSuccess);
            return true;
        }

        public bool Login(bool showPopups = true)
        {
            return Login(SecurePlayerPrefs.GetString("ClientName"), SecurePlayerPrefs.GetString("ClientPassword"), showPopups);
        }

        public bool Login(string clientName, string clientPassword, bool showPopups = true)
        {
            if (!IsUsernameAllowed(clientName)) return false;

            if (clientName != SecurePlayerPrefs.GetString("ClientName") || clientPassword != SecurePlayerPrefs.GetString("ClientPassword"))
            {
                // Set new Login Credentials
                SecurePlayerPrefs.SetString("ClientName", clientName);
                SecurePlayerPrefs.SetString("ClientPassword", clientPassword);
            }

            // Send request
            string rawResponse = SendRequest(new MpRequest.Login(), CallbackType.None, false);       // We dont need a token for this request -> ignore if token is expired

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                if (statusResponse.ErrorLevel == "InvalidLogin" && showPopups) Other.Tools.CreatePopup(Other.Tools.Messages.InvalidLogin);
                return false;
            }

            // Handle response
            MpResponse.Token response = JsonUtility.FromJson<MpResponse.Token>(rawResponse);
            SecurePlayerPrefs.SetString("ClientToken", response.ClientToken);           // Set new token
            SecurePlayerPrefs.SetInt("ClientTokenExpire", CurrentTimestamp() + 3600);   // We add 3600 seconds because the token will be valid for one hour
            LoggedIn = true;    // Set status
            SecurePlayerPrefs.SetInt("IsRegistered", 1);

            if (showPopups) Other.Tools.CreatePopup(Other.Tools.Messages.LoggedIn);
            return true;
        }

        public bool Logout()
        {
            // Send request
            string rawResponse = SendRequest(new MpRequest.Logout());

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                InvalidToken(statusResponse);
                return false;
            }

            // Clear client data
            SecurePlayerPrefs.SetString("ClientName", "");
            SecurePlayerPrefs.SetString("ClientPassword", "");
            SecurePlayerPrefs.SetInt("ClientTokenExpire", 0);
            LoggedIn = false;
            Other.Tools.CreatePopup(Other.Tools.Messages.LoggedOut);
            return true;
        }

        public void QuickMatch()
        {
            // Send request
            SendRequest(new MpRequest.QuickMatch(), CallbackType.QuickMatch);
        }

        private void QuickMatchCallback()
        {
            string rawResponse = _callbackResponse;

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                InvalidToken(statusResponse);
                if (statusResponse.ErrorLevel == "AlreadyInQueue") Other.Tools.CreatePopup(Other.Tools.Messages.AlreadyInQueue);
            }
            else
            {
                // Handle request
                MpResponse.Player playerResponse = JsonUtility.FromJson<MpResponse.Player>(rawResponse);
                Scenes.SetString("GameMode", "Online");
                Scenes.SetString("OpponentName", playerResponse.PlayerName);
                Scenes.Load("Game");
                Other.Tools.CreatePopup(Other.Tools.Messages.SearchingGames);
            }
        }

        public bool CancelQuickMatch()
        {
            // Send request
            string rawResponse = SendRequest(new MpRequest.CancelQuickMatch());

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                InvalidToken(statusResponse);
                return false;
            }

            // Handle request
            return true;
        }

        public bool CreatePrivateGame(string gamePassword)
        {
            // Configure request
            MpRequest.CreatePrivateGame request = new MpRequest.CreatePrivateGame();
            request.GamePassword = gamePassword;

            // Send request
            string rawResponse = SendRequest(request);

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                InvalidToken(statusResponse);
                if (statusResponse.ErrorLevel == "AlreadyCreatedGame") Other.Tools.CreatePopup(Other.Tools.Messages.AlreadyCreatedGame);
                return false;
            }

            // Handle response
            Other.Tools.CreatePopup(Other.Tools.Messages.CreatePrivateGame);
            return true;
        }

        public bool JoinPrivateGame(string gameName, string gamePassword)
        {
            // Configure request
            MpRequest.JoinPrivateGame request = new MpRequest.JoinPrivateGame();
            request.GameName = gameName;
            request.GamePassword = gamePassword;

            // Send request
            string rawResponse = SendRequest(request);

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                InvalidToken(statusResponse);
                if (statusResponse.ErrorLevel == "InvalidGameName") Other.Tools.CreatePopup(Other.Tools.Messages.CouldntFindGame);
                if (statusResponse.ErrorLevel == "InvalidPassword") Other.Tools.CreatePopup(Other.Tools.Messages.InvalidGamePassword);
                return false;
            }

            // Handle response
            Scenes.SetString("GameMode", "Online");
            Scenes.SetString("OpponentName", gameName);
            Scenes.Load("Game");
            return true;
        }

        public List<string> GetAvaibleGames()
        {
            // Send request
            string rawResponse = SendRequest(new MpRequest.GetAvaibleGames());

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                InvalidToken(statusResponse);
            }

            // Handle response
            MpResponse.AvaibleGames avaibleGamesResponse = JsonUtility.FromJson<MpResponse.AvaibleGames>(rawResponse);
            return avaibleGamesResponse.GameNames;
        }

        public void FinishMove(float mass)
        {
            // Configure request
            MpRequest.FinishMove request = new MpRequest.FinishMove();
            request.Mass = mass;

            // Send request
            SendRequest(request, CallbackType.FinishMove);
        }

        private void FinishMoveCallback()
        {
            string rawResponse = _callbackResponse;

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                InvalidToken(statusResponse);
                if (statusResponse.ErrorLevel == "NotInGame") Other.Tools.CreatePopup(Other.Tools.Messages.NotInGame);
                if (statusResponse.ErrorLevel == "InvalidMass") Other.Tools.CreatePopup(Other.Tools.Messages.Error);
            }

            // Handle response
            MpResponse.Move moveResponse = JsonUtility.FromJson<MpResponse.Move>(rawResponse);
            if (GameManager.Instance.State == GameManager.GameState.WaitForOpponent) GameManager.Instance.OpponentMove(moveResponse.Mass);
        }

        // --- Only helper functions from here on ---

        private string SendRequest(object request, CallbackType callback = CallbackType.None, bool checkTokenExpire = true)
        {
            if (checkTokenExpire && CurrentTimestamp() > SecurePlayerPrefs.GetInt("ClientTokenExpire")) Login();

            if (!_connectedToServer) if (!ConnectToServer()) return null;           // Are we already connected to the server?

            // Send request
            var sendingData = JsonUtility.ToJson(request);                          // Convert object -> json -> bytes
            _writer.WriteLine(sendingData);                                         // Write data to stream
            _writer.Flush();                                                        // Flush and wait for respons

            // Receive response
            byte[] data = System.Text.Encoding.ASCII.GetBytes(sendingData);
            if (callback == CallbackType.None)
            {
                var bytes = _stream.Read(data, 0, data.Length);                     // Read response bytes
                return System.Text.Encoding.ASCII.GetString(data, 0, bytes);        // Convert bytes -> json
            }
            else
            {
                _expectedCallback = callback;
                _stream.BeginRead(data, 0, data.Length, ResponseCallback, data);    // Read response bytes
                return null;
            }
        }

        private void ResponseCallback(IAsyncResult response)
        {
            _callbackResponse = BytesToString(response);
            _receivedCallback = true;
        }

        private string BytesToString(IAsyncResult response)
        {
            var buffer = (byte[])response.AsyncState;
            var bytesAvailable = _stream.EndRead(response);
            return System.Text.Encoding.ASCII.GetString(buffer, 0, bytesAvailable);
        }

        private static bool RequestFailed(string response)
        {
            return response.Contains("\"Success\":false");
        }

        private bool InvalidToken(MpResponse.Status response)
        {
            if (response.ErrorLevel != "InvalidToken") return false;
            Debug.Log("Invalid Token. Requesting new one");
            Other.Tools.CreatePopup(Other.Tools.Messages.Error);
            Login(false);
            return true;
        }

        private static int CurrentTimestamp()
        {
            /*  Schedule:
             *  (1) Get TimeSpan between now and 01.01.1970
             *  (2) Convert TimeSpan in seconds
             *  (3) Round seconds down to nearest int
             *  (4) Return value
             */

            TimeSpan epochStart = DateTime.UtcNow - new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
            return (int)Math.Floor(epochStart.TotalSeconds);
        }

        private static bool IsUsernameAllowed(string username)
        {
            string message = null;
            if (!username.All(char.IsLetterOrDigit)) message = Other.Tools.Messages.UsernameInfoCharacter;
            if (username.Length < 4) message = Other.Tools.Messages.UsernameInfoToShort;
            if (username.Length > 16) message = Other.Tools.Messages.UsernameInfoToLong;

            if (message == null) return true;   // Username is allowed

            Other.Tools.CreatePopup(Other.Tools.Messages.UsernameNotAllowed + "\n" + message);
            return false;
        }
    }
}