using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Assets.PlayId.Scripts.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.PlayId.Scripts.Data
{
    /// <summary>
    /// JWT debugger: https://jwt.io/
    /// </summary>
    public class JWT
    {
        public readonly string Encoded;

        public string Header => Base64UrlEncoder.Decode(Encoded.Split('.')[0]);
        public string Payload => Base64UrlEncoder.Decode(Encoded.Split('.')[1]);
        public string SignedData => Encoded.Split('.')[0] + "." + Encoded.Split('.')[1];
        public string Signature => Encoded.Split('.')[2];

        public const string JwksUri = "https://playid.org/.well-known/jwks";

        private static Dictionary<string, Dictionary<string, string>> KnownPublicKeys
        {
            get => PlayerPrefs.HasKey(JwksUri) ? JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(PlayerPrefs.GetString(JwksUri)) : new Dictionary<string, Dictionary<string, string>>();
            set => PlayerPrefs.SetString(JwksUri, JsonConvert.SerializeObject(value));
        }

        public JWT(string encoded)
        {
            Encoded = encoded;
        }

        /// <summary>
        /// More info: https://github.com/hippogamesunity/PlayID/wiki/ID-Token-validation
        /// Signature validation makes sense on a backend only in most cases.
        /// </summary>
        public void ValidateSignature(string clientId)
        {
            var header = JObject.Parse(Header);

            if ((string)header["typ"] != "JWT")
            {
                throw new Exception("Unexpected header (typ).");
            }

            if ((string)header["alg"] != "RS256")
            {
                throw new Exception("Unexpected header (alg).");
            }

            var payload = JObject.Parse(Payload);

            if ((string)payload["iss"] != "https://playid.org")
            {
                throw new Exception("Unexpected payload (iss).");
            }

            if ((string)payload["aud"] != clientId)
            {
                throw new Exception("Unexpected payload (aud).");
            }

            var exp = (long)payload["exp"];

            if (exp < ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds())
            {
                throw new Exception("JWT expired.");
            }

            var kid = (string)header["kid"] ?? throw new Exception("Key missed (kid).");

            if (KnownPublicKeys.ContainsKey(kid))
            {
                var verified = VerifySignature(KnownPublicKeys[kid]["n"], KnownPublicKeys[kid]["e"]);

                if (!verified)
                {
                    throw new Exception("Invalid JWT signature.");
                }

                return;
            }

            using var httpClient = new HttpClient();

            var response = httpClient.GetAsync(JwksUri).Result;
            var responseText = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                var certs = JObject.Parse(responseText);
                var keys = (certs["keys"] ?? throw new Exception("Key missed (keys).")).ToDictionary(i => i["kid"].Value<string>(), i => i.ToObject<Dictionary<string, string>>());

                KnownPublicKeys = keys;

                if (!keys.TryGetValue(kid, out var key) || key == null)
                {
                    throw new Exception($"Public key not found (kid={kid}).");
                }

                if (!key.TryGetValue("n", out var modulus))
                {
                    throw new Exception($"Invalid modulus (kid={kid}).");
                }

                if (!key.TryGetValue("e", out var exponent))
                {
                    throw new Exception($"Invalid exponent (kid={kid}).");
                }

                var verified = VerifySignature(modulus, exponent);

                if (!verified)
                {
                    throw new Exception("Invalid JWT signature.");
                }
            }
            else
            {
                throw new Exception(responseText);
            }
        }

        private bool VerifySignature(string modulus, string exponent)
        {
            var parameters = new RSAParameters
            {
                Modulus = Base64UrlEncoder.DecodeBytes(modulus),
                Exponent = Base64UrlEncoder.DecodeBytes(exponent)
            };
            var provider = new RSACryptoServiceProvider();

            provider.ImportParameters(parameters);

            var signature = Base64UrlEncoder.DecodeBytes(Signature);
            var data = Encoding.UTF8.GetBytes(SignedData);
            var verified = provider.VerifyData(data, SHA256.Create(), signature);

            return verified;
        }
    }
}