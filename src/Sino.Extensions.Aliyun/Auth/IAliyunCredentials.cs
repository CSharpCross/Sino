using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Aliyun.Auth
{
    /// <summary>
    /// 认证信息
    /// </summary>
    public interface IAliyunCredentials
    {
        string GetAccessKeyId();
        string GetAccessKeySecret();
    }
}
