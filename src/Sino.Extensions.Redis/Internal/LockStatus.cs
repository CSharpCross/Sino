namespace Sino.Extensions.Redis.Internal
{
    /// <summary>
    /// 锁的状态
    /// </summary>
    public enum LockStatus
    {
        /// <summary>
        /// 尚未获得锁或锁已被释放
        /// </summary>
        Unlocked,

        /// <summary>
        /// 成功获取到锁
        /// </summary>
        Acquired,

        /// <summary>
        /// 由于未能达到最少法定数量无法获取锁
        /// </summary>
        NoQuorum,

        /// <summary>
        /// 由于当前锁编号与当前锁编号不一致，无法获取到锁
        /// </summary>
        Conflicted,

        /// <summary>
        /// 锁已经超时
        /// </summary>
        Expired
    }
}
