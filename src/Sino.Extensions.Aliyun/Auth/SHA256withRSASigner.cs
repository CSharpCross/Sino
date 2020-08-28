using System;
using System.Security.Cryptography;
using System.Text;

namespace Sino.Extensions.Aliyun.Auth
{
    public class SHA256withRSASigner : Signer
    {
        private const string ALGORITHM_NAME = "SHA256withRSA";

        public override string SignString(string stringToSign, string accessKeySecret)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));
                    var RSAFormatter = new RSAPKCS1SignatureFormatter(rsa);
                    RSAFormatter.SetHashAlgorithm("SHA256");
                    var signedHash = RSAFormatter.CreateSignature(hashValue);
                    return Convert.ToBase64String(signedHash);
                }
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
            return "PRIVATEKEY";
        }
    }
}
