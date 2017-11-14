using System;
using System.Net.Sockets;
using Other;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class MultiplayerManager : MonoBehaviour
    {
        public bool LoggedIn = false;
        public GameObject Popup;

        [SerializeField]
        private string _server = "localhost";
        [SerializeField]
        private int _port = 1531;  // Gründungsjahr des Katharineums zu Lübeck

        private TcpClient _client;
        private NetworkStream _stream;

        private static bool _connectedToServer = false;
        private static bool _initialized = false;

        public void Start()
        {
            if (_connectedToServer || _initialized) return;

            if (!_initialized) _initialized = true;
            DontDestroyOnLoad(this);
            ConnectToServer();
        }

        public bool ConnectToServer()
        {
            try
            {
                _client = new TcpClient(_server, _port);
                _stream = _client.GetStream();
                _connectedToServer = true;
                Debug.Log("Connected to Server");
                return true;
            }
            catch (Exception)
            {
                Debug.Log("Couldn't connect to Server");
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
            // Set new Login Credentials
            SecurePlayerPrefs.SetString("ClientName", clientName);
            SecurePlayerPrefs.SetString("ClientPassword", clientPassword);

            // Send request
            string rawResponse = SendRequest(new MpRequest.Register(), null, false);    // We dont need a token for this request -> ignore if token is expired

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                if(statusResponse.ErrorLevel == "UsernameAlreadyTaken") Debug.Log("Username already Taken");
                return false;
            }

            // Handle response
            MpResponse.Token response = JsonUtility.FromJson<MpResponse.Token>(rawResponse);
            SecurePlayerPrefs.SetString("ClientToken", response.ClientToken);           // Set new token
            SecurePlayerPrefs.SetInt("ClientTokenExpire", CurrentTimestamp() + 3600);   // We add 3600 seconds because the token will be valid for one hour
            LoggedIn = true;    // Set status

            Debug.Log("Successfully registered as: " + clientName);
            return true;
        }

        public bool Login()
        {
            return Login(SecurePlayerPrefs.GetString("ClientName"), SecurePlayerPrefs.GetString("ClientPassword"));
        }

        public bool Login(string clientName, string clientPassword)
        {
            if (!(clientName == null || clientPassword == null))
            {
                // Set new Login Credentials
                SecurePlayerPrefs.SetString("ClientName", clientName);
                SecurePlayerPrefs.SetString("ClientPassword", clientPassword);
            }

            // Send request
            string rawResponse = SendRequest(new MpRequest.Login(), null, false);   // We dont need a token for this request -> ignore if token is expired

            // Check if request failed
            if (RequestFailed(rawResponse))
            {
                MpResponse.Status statusResponse = JsonUtility.FromJson<MpResponse.Status>(rawResponse);
                if(statusResponse.ErrorLevel == "InvalidLogin") Debug.Log("Invalid Login");
                return false;
            }

            // Handle response
            MpResponse.Token response = JsonUtility.FromJson<MpResponse.Token>(rawResponse);
            SecurePlayerPrefs.SetString("ClientToken", response.ClientToken);           // Set new token
            SecurePlayerPrefs.SetInt("ClientTokenExpire", CurrentTimestamp() + 3600);   // We add 3600 seconds because the token will be valid for one hour
            LoggedIn = true;    // Set status

            Other.Tools.CreatePopup("Successfully logged in!", 2);
            Debug.Log("Successfully logged in");
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
                if (InvalidToken(statusResponse, true))
                {
                    Debug.Log("Invalid Token. Requesting new one and retrying logout.");
                    Logout();
                }
                return false;
            }

            // Clear client data
            SecurePlayerPrefs.SetString("ClientName", "");
            SecurePlayerPrefs.SetString("ClientPassword", "");
            SecurePlayerPrefs.SetInt("ClientTokenExpire", 0);
            LoggedIn = false;
            return true;
        }

        // --- Only helper functions from here on ---

        private string SendRequest(object request, Action callback = null, bool checkTokenExpire = true)
        {
            if (checkTokenExpire && CurrentTimestamp() > SecurePlayerPrefs.GetInt("ClientTokenExpire")) Login();

            if (!_connectedToServer) if (!ConnectToServer()) return null;                   // Are we already connected to the server?
            var data = System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(request));    // Convert object -> json -> bytes
            _stream.Write(data, 0, data.Length);                                            // Write data to stream
            _stream.Flush();                                                                // Flush and wait for respons
            if (callback == null)
            {
                data = new byte[2048];
                var bytes = _stream.Read(data, 0, data.Length);                             // Buffer to store response bytes
                return System.Text.Encoding.ASCII.GetString(data, 0, bytes);                // Convert bytes -> json
            }
            else
            {
                // TODO ADD CALLBACK TO GIVEN FUNCTION
                return null;
            }
        }

        private object BytesToObject(IAsyncResult response, Type responseType)
        {
            var buffer = (byte[])response.AsyncState;
            var bytesAvailable = _stream.EndRead(response);
            return JsonUtility.FromJson(System.Text.Encoding.ASCII.GetString(buffer, 0, bytesAvailable), responseType);
        }

        private static bool RequestFailed(string response)
        {
            return response.Contains("\"Success\":\"false\"");
        }

        private bool InvalidToken(MpResponse.Status response, bool requestNewToken)
        {
            if (response.ErrorLevel != "InvalidToken") return false;
            Login(SecurePlayerPrefs.GetString("ClientName"), SecurePlayerPrefs.GetString("ClientPassword"));
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
    }
}
