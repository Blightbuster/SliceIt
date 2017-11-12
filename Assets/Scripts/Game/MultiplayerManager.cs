using System;
using System.Net.Sockets;
using UnityEngine;

namespace Game
{
    public class MultiplayerManager : MonoBehaviour
    {
        public bool LoggedIn = false;

        [SerializeField]
        private string _server = "localhost";
        [SerializeField]
        private int _port = 1531;  // Gründungsjahr des Katharineums zu Lübeck

        private TcpClient _client;
        private NetworkStream _stream;

        private static bool _connectedToServer = false;

        public void Start()
        {
            if (_connectedToServer) return;
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
                return true;
            }
            catch (Exception)
            {
                Debug.LogError("Couldn't connect to Server");
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
                Debug.LogError("Something went wrong while disconnecting from the Server");
            }
            _connectedToServer = false;
        }

        public bool Register(string clientName, string clientPassword)
        {
            // Set new Login credentials
            ZPlayerPrefs.SetString("ClientName", clientName);
            ZPlayerPrefs.SetString("ClientPassword", clientPassword);

            MpResponse.Status response = (MpResponse.Status)SendRequest(new MpRequest.Register(), typeof(MpResponse.Status));

            if (response.Succes) return true;

            // At this point we know, that the registration failed
            if (response.ErrorLevel == "UsernameAlreadyTaken")
            {
                Debug.Log("Username already taken");
            }
            return false;
        }

        public bool Login(string clientName, string clientPassword)
        {
            // Set new Login credentials
            ZPlayerPrefs.SetString("ClientName", clientName);
            ZPlayerPrefs.SetString("ClientPassword", clientPassword);

            // Send request
            MpResponse.Status response = (MpResponse.Status)SendRequest(new MpRequest.Login(), typeof(MpResponse.Status));

            LoggedIn = response.Succes; // Set status
            Debug.Log(LoggedIn ? "Logged in successfully" : "Login failed");
            return LoggedIn;            // Finally return result of login
        }

        // ----- Only helper functions from here on -----

        private object SendRequest(object request, Type responseType, Action callback = null)
        {
            if (!_connectedToServer) if (!ConnectToServer()) return null;                   // Are we already connected to the server?
            var data = System.Text.Encoding.ASCII.GetBytes(JsonUtility.ToJson(request));    // Convert object -> json -> bytes
            _stream.Write(data, 0, data.Length);                                            // Write data to stream
            _stream.Flush();                                                                // Flush and wait for respons
            if (callback == null)
            {
                data = new byte[2048];
                var bytes = _stream.Read(data, 0, data.Length);                             // Buffer to store response bytes
                return JsonUtility.FromJson(System.Text.Encoding.ASCII.GetString(data, 0, bytes), responseType);    // Convert bytes -> json -> object
            }
            else
            {
                return null;
            }
        }

        private object BytesToObject(IAsyncResult response, Type responseType)
        {
            var buffer = (byte[])response.AsyncState;
            var bytesAvailable = _stream.EndRead(response);
            return JsonUtility.FromJson(System.Text.Encoding.ASCII.GetString(buffer, 0, bytesAvailable), responseType);
        }
    }
}
