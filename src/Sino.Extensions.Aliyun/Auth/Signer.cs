using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Aliyun.Auth
{
    public abstract class Signer
    {
        public abstract string SignString(string stringToSign, IAliyunCredentials credentials);
        public abstract string SignString(string stringToSign, string accessKeySecret);
        public abstract string GetSignerName();
        public abstract string GetSignerVersion();
        public abstract string GetSignerType();
    }
}
