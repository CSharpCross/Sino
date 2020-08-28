using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Aliyun.Auth
{
    public class AliyunCredential : IAliyunCredentials
    {
        public AliyunCredential()
        {
            RefreshDate = DateTime.UtcNow;
        }

        public AliyunCredential(string keyId, string secret)
        {
            AccessKeyId = keyId;
            AccessSecret = secret;
            RefreshDate = DateTime.UtcNow;
        }

        public AliyunCredential(string keyId, string secret, string securityToken)
        {
            AccessKeyId = keyId;
            AccessSecret = secret;
            SecurityToken = securityToken;
            RefreshDate = new DateTime();
        }

        public AliyunCredential(string keyId, string secret, int expiredHours)
        {
            AccessKeyId = keyId;
            AccessSecret = secret;
            RefreshDate = new DateTime();

            SetExpiredDate(expiredHours);
        }

        public AliyunCredential(string keyId, string secret, string securityToken, int expiredHours)
        {
            AccessKeyId = keyId;
            AccessSecret = secret;
            SecurityToken = securityToken;
            RefreshDate = new DateTime();

            SetExpiredDate(expiredHours);
        }

        public DateTime RefreshDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessSecret { get; set; }
        public string SecurityToken { get; set; }

        public string GetAccessKeyId()
        {
            return AccessKeyId;
        }

        public string GetAccessKeySecret()
        {
            return AccessSecret;
        }

        private void SetExpiredDate(int expiredHours)
        {
            if (0 < expiredHours)
            {
                ExpiredDate = DateTime.UtcNow.AddHours(expiredHours);
            }
        }

        public bool IsExpired()
        {
            if (null == ExpiredDate)
            {
                return false;
            }

            return !(ExpiredDate < DateTime.UtcNow);
        }
    }
}
