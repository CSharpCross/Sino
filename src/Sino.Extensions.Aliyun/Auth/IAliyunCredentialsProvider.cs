namespace Sino.Extensions.Aliyun.Auth
{
    /// <summary>
    /// 认证信息提供器
    /// </summary>
    public interface IAliyunCredentialsProvider
    {
        IAliyunCredentials GetCredentials();
    }
}
