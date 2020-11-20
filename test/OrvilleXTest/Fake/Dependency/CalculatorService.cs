using System;

namespace OrvilleXTest.Fake
{
    public class CalculatorService : ICalcService
	{
		public bool Disposed
		{
			get { throw new NotImplementedException(); }
		}

		public bool Initialized
		{
			get { throw new NotImplementedException(); }
		}

		public virtual int Sum(int x, int y)
		{
			return x + y;
		}
	}
}
