using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Game
{
    public static class SecurePlayerPrefs
    {
        // Set false if you don't want to use encrypt/decrypt value for debugging
        public static bool UseSecure = true;

        private const int Iterations = 555;

        // You should change following password and salt value using Initialize
        static string _strPassword = "im91dSa3.!1^2";

        static string _strSalt = "DK!2-sa+";

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            Save();
        }

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
            Save();
        }

        public static float GetFloat(string key)
        {
            return GetFloat(key, 0.0f);
        }

        public static float GetFloat(string key, float defaultValue, bool isDecrypt = true)
        {
            float retValue = defaultValue;

            string strValue = GetString(key);

            if (float.TryParse(strValue, out retValue))
            {
                return retValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int GetInt(string key)
        {
            return GetInt(key, 0);
        }

        public static int GetInt(string key, int defaultValue, bool isDecrypt = true)
        {
            int retValue = defaultValue;

            string strValue = GetString(key);

            if (int.TryParse(strValue, out retValue))
            {
                return retValue;
            }
            else
            {
                return defaultValue;
            }
        }


        public static string GetString(string key)
        {
            string strEncryptValue = GetRowString(key);

            return Decrypt(strEncryptValue, _strPassword);
        }

        public static string GetRowString(string key)
        {
            string strEncryptKey = Encrypt(key, _strPassword);
            string strEncryptValue = PlayerPrefs.GetString(strEncryptKey);

            return strEncryptValue;
        }

        public static string GetString(string key, string defaultValue)
        {
            string strEncryptValue = GetRowString(key, defaultValue);
            return Decrypt(strEncryptValue, _strPassword);
        }

        public static string GetRowString(string key, string defaultValue)
        {
            string strEncryptKey = Encrypt(key, _strPassword);
            string strEncryptDefaultValue = Encrypt(defaultValue, _strPassword);

            string strEncryptValue = PlayerPrefs.GetString(strEncryptKey, strEncryptDefaultValue);

            return strEncryptValue;
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(Encrypt(key, _strPassword));
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }

        public static void SetFloat(string key, float value)
        {
            string strValue = System.Convert.ToString(value);
            SetString(key, strValue);
        }

        public static void SetInt(string key, int value)
        {
            string strValue = System.Convert.ToString(value);
            SetString(key, strValue);
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(Encrypt(key, _strPassword), Encrypt(value, _strPassword));
            Save();
        }

        // --- Helper functions ---
        public static void Initialize(string newPassword, string newSalt)
        {
            _strPassword = newPassword;
            _strSalt = newSalt;
        }

        private static byte[] GetSalt()
        {
            byte[] salt = Encoding.UTF8.GetBytes(_strSalt);
            return salt;
        }

        // This encryption algorithm is based on MSDN docs
        private static string Encrypt(string strPlain, string password)
        {
            if (!UseSecure)
                return strPlain;

            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, GetSalt(), Iterations);

                byte[] key = rfc2898DeriveBytes.GetBytes(8);

                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, GetSalt()), CryptoStreamMode.Write))
                {
                    memoryStream.Write(GetSalt(), 0, GetSalt().Length);

                    byte[] plainTextBytes = Encoding.UTF8.GetBytes(strPlain);

                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Encrypt Exception: " + e);
                return strPlain;
            }
        }

        // This decryption algorithm is based on MSDN docs
        private static string Decrypt(string strEncript, string password)
        {
            if (!UseSecure)
                return strEncript;

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(strEncript);

                using (var memoryStream = new MemoryStream(cipherBytes))
                {
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                    byte[] salt = GetSalt();
                    memoryStream.Read(salt, 0, salt.Length);

                    // Use derive bytes to generate key from password and salt
                    var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations);

                    byte[] key = rfc2898DeriveBytes.GetBytes(8);

                    using (var cryptoStream =
                        new CryptoStream(memoryStream, des.CreateDecryptor(key, salt), CryptoStreamMode.Read))
                    using (var streamReader = new StreamReader(cryptoStream))
                    {
                        string strPlain = streamReader.ReadToEnd();
                        return strPlain;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Decrypt Exception: " + e);
                return strEncript;
            }

        }

    }
}