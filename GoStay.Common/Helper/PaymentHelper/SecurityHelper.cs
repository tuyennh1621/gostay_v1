using GoStay.Common.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Common.Helper.PaymentHelper
{
    public class SecurityHelper
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        public SecurityHelper()
        {
            //encrypt and decrypt password using secure
        }
        public string getHash(string partnerCode, string merchantRefId,
            string amount, string paymentCode, string storeId, string storeName, string publicKeyXML)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"amount\":" +
                amount + ",\"paymentCode\":\"" +
                paymentCode + "\",\"storeId\":\"" +
                storeId + "\",\"storeName\":\"" +
                storeName + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(4096)) //KeySize
            {
                try
                {
                    // MoMo's public key has format PEM.
                    // You must convert it to XML format. Recommend tool: https://superdry.apphb.com/tools/online-rsa-key-converter
                    rsa.FromXmlString(publicKeyXML);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }
        public string buildQueryHash(string partnerCode, string merchantRefId,
            string requestid, string publicKey)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"requestId\":\"" +
                requestid + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }

        public string buildRefundHash(string partnerCode, string merchantRefId,
            string momoTranId, long amount, string description, string publicKey)
        {
            string json = "{\"partnerCode\":\"" +
                partnerCode + "\",\"partnerRefId\":\"" +
                merchantRefId + "\",\"momoTransId\":\"" +
                momoTranId + "\",\"amount\":" +
                amount + ",\"description\":\"" +
                description + "\"}";

            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server
                    rsa.FromXmlString(publicKey);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }

            }

            return result;

        }
        public string signSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;

            }
        }
        public string signSHA256Appota(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            var x = Convert.ToBase64String(messageBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
            return x;
        }

        public string GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppConfigs.AppotaKey);
            var time = DateTime.Now;
            var numberOfSeconds = time.Subtract(time).TotalSeconds;
            // Get the offset from current time in UTC time
            DateTimeOffset dto = new DateTimeOffset(DateTime.Now.AddMinutes(20));
            // Get the unix timestamp in seconds
            string unixTime = dto.ToUnixTimeSeconds().ToString();
            var timespan = Int64.Parse(unixTime);
            var headers = new JwtHeader()
            {
                {"typ", "JWT"},
                {"alg", "HS256"},
                { "cty", "appotapay-api;v=1" }
            };
            var payload = new JwtPayload()
            {
                {"iss", AppConfigs.AppotaIssuer },
                {"jti", AppConfigs.AppotaApi + "-" + unixTime },
                {"api_key", AppConfigs.AppotaApi },
                { "exp", timespan}
            };

            var JsonWeb = new JwtSecurityToken(headers, payload);
            var token = tokenHandler.WriteToken(JsonWeb);
            token = token.Remove(token.Length - 1, 1);
            var jwt = generateHash(token, AppConfigs.AppotaKey);
            return token + "." + jwt;
        }
        public static string generateHash(string str, string cypherkey)
        {
            var encoding = new System.Text.ASCIIEncoding();

            var messageBytes = encoding.GetBytes(str);
            var keyBytes = encoding.GetBytes(cypherkey);
            using var hmacsha256 = new HMACSHA256(keyBytes);
            var hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashmessage).Replace('/', '_').Replace('+', '-').Replace("=", "");
        }
    }
}
