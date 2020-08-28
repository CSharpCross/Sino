using System;
using System.Security.Cryptography;
using System.Text;

namespace Sino.Extensions.Aliyun.Auth
{
    public class HmacSHA1Signer : Signer
    {
        private const string ALGORITHM_NAME = "HMAC-SHA1";

        public override string SignString(string stringToSign, string accessKeySecret)
        {
            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(accessKeySecret)))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
                return Convert.ToBase64String(hashValue);
            }
        }

        public override string SignString(string stringToSign, IAliyunCredentials credentials)
        {
            return SignString(stringToSign, credentials.GetAccessKeySecret());
        }

        public override string GetSignerName()
        {
            return ALGORITHM_NAME;
        }

        public override string GetSignerVersion()
        {
            return "1.0";
        }

        public override string GetSignerType()
        {
            return null;
        }
    }
}
