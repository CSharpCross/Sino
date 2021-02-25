using OrvilleX.Dependency;

namespace OrvilleXTest.Fake
{
    public interface ICalcService : ISingletonDependency
	{
		bool Disposed { get; }
		bool Initialized { get; }

		int Sum(int x, int y);
	}
}
