using OrvilleX.AutoIndex.Generator;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrvilleX.AutoIndex.Mock
{
    public class MockIdGeneratorFactory : AbstractIdGeneratorFactory
    {
        protected override IIdGenerator CreateIdGenerator(string bizType)
        {
            return new MockIdGenerator();
        }
    }
}
