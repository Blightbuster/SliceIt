using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Other
{
    public static class Scenes
    {
        private static Dictionary<string, string> _parameters = new Dictionary<string, string>();

        public static void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static string GetString(string paramKey)
        {
            return _parameters[paramKey];
        }

        public static void SetString(string paramKey, string paramValue)
        {
            _parameters[paramKey] = paramValue;
        }

        public static int GetInt(string paramKey)
        {
            return int.Parse(_parameters[paramKey]);
        }

        public static void SetInt(string paramKey, int paramValue)
        {
            _parameters[paramKey] = paramValue.ToString();
        }

        public static bool GetBool(string paramKey)
        {
            return bool.Parse(_parameters[paramKey]);
        }

        public static void SetBool(string paramKey, bool paramValue)
        {
            _parameters[paramKey] = (paramValue ? 1 : 0).ToString();
        }
    }
}
