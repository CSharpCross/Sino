using OrvilleX.Cache.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrvilleX.Cache
{
    /// <summary>
    /// 提供分布式缓存能力接口
    /// </summary>
    public interface IDistributedLockProvider
    {
        /// <summary>
        /// 创建锁
        /// </summary>
        /// <param name="resource">资源标识</param>
        /// <param name="expiryTime">锁的时间，如果在达到时间后对应的处理程序未结束，将会自动增加锁的时间</param>
        ILock CreateLock(string resource, TimeSpan expiryTime);

        Task<ILock> CreateLockAsync(string resource, TimeSpan expiryTime);

        /// <summary>
        /// 创建锁
        /// </summary>
        /// <param name="resource">资源标识</param>
        /// <param name="expiryTime">锁的时间</param>
        /// <param name="waitTime">等待时间，如果当前锁尚未释放则会等待对应时间</param>
        /// <param name="retryTime">重试等待时间，每次重试之间等待的时间</param>
        ILock CreateLock(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null);

        Task<ILock> CreateLockAsync(string resource, TimeSpan expiryTime, TimeSpan waitTime, TimeSpan retryTime, CancellationToken? cancellationToken = null);
    }
}
