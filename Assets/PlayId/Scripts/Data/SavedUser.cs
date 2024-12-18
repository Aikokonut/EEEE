﻿using Assets.PlayId.Scripts.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace Assets.PlayId.Scripts.Data
{
    public class SavedUser : User
    {
        public readonly string ClientId;

        [Preserve]
        protected SavedUser()
        {
        }

        public SavedUser(TokenResponse tokenResponse, string clientId) : base(tokenResponse)
        {
            ClientId = clientId;
        }

        public static SavedUser Load(string clientId)
        {
            var key = GetKey(clientId);

            if (!PlayerPrefs.HasKey(key)) return null;

            try
            {
                var encrypted = PlayerPrefs.GetString(key);
                var json = AES.Decrypt(encrypted, SystemInfo.deviceUniqueIdentifier);

                return JsonConvert.DeserializeObject<SavedUser>(json);
            }
            catch
            {
                return null;
            }
        }

        public void Save()
        {
            var key = GetKey(ClientId);
            var json = JsonConvert.SerializeObject(this);
            var encrypted = AES.Encrypt(json, SystemInfo.deviceUniqueIdentifier);

            PlayerPrefs.SetString(key, encrypted);
            PlayerPrefs.Save();
        }

        public void Delete()
        {
            var key = GetKey(ClientId);

            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        private static string GetKey(string clientId)
        {
            return Md5.ComputeHash(nameof(SavedUser) + ':' + clientId);
        }
    }
}