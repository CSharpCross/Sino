namespace Sino.Extensions.AutoIndex.Entity
{
    /// <summary>
    /// 返回状态码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 正常可用
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 需要去加载NextId
        /// </summary>
        Loading = 2,
        /// <summary>
        /// 超过MaxId不可用
        /// </summary>
        Over = 3
    }
}
