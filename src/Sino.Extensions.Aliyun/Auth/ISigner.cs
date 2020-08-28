namespace Sino.Extensions.Aliyun.Auth
{
    /// <summary>
    /// 令牌
    /// </summary>
    public interface ISigner
    {
        string SignerName { get; }
        string SignerVersion { get; }
        string SignString(string source, string accessSecret);
    }
}
