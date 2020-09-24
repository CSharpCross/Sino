using System;

namespace Sino.Extensions.Redis.Internal
{
    public interface ILock : IDisposable
    {
        /// <summary>
        /// 资源名
        /// </summary>
        string Resource { get; }

        /// <summary>
        /// 锁编号
        /// </summary>
        string LockId { get; }

        /// <summary>
        /// 是否已获取到锁
        /// </summary>
        bool IsAcquired { get; }

        /// <summary>
        /// 锁状态
        /// </summary>
        LockStatus Status { get; }

        /// <summary>
        /// 记录关于锁各类状态计数
        /// </summary>
        LockInstanceSummary InstanceSummary { get; }

        /// <summary>
        /// 锁被延长有效时间的次数
        /// </summary>
        int ExtendCount { get; }
    }
}
