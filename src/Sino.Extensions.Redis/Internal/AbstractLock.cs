namespace Sino.Extensions.Redis.Internal
{
    public abstract class AbstractLock : ILock
    {
        public string Resource { get; protected set; }

        public string LockId { get; protected set; }

        public abstract bool IsAcquired { get; }

        public LockStatus Status { get; protected set; }

        public LockInstanceSummary InstanceSummary { get; protected set; }

        public int ExtendCount { get; protected set; }

        public abstract void Dispose();
    }
}
