namespace Sino.Extensions.Redis.Internal
{
    public struct LockInstanceSummary
    {
		public LockInstanceSummary(int acquired, int conflicted, int error)
		{
			Acquired = acquired;
			Conflicted = conflicted;
			Error = error;
        }

		public readonly int Acquired;
		public readonly int Conflicted;
		public readonly int Error;

		public override string ToString()
		{
			return $"Acquired: {Acquired}, Conflicted: {Conflicted}, Error: {Error}";
		}
	}
}
