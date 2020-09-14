using Sino.Extensions.AutoIndex.Generator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.AutoIndex
{
    public class TinyIdClient : ITinyIdClient
    {
        private IIdGeneratorFactory _idGeneratorFactory;

        public TinyIdClient(IIdGeneratorFactory factory)
        {
            _idGeneratorFactory = factory;
        }

        public long NextId(string bizType)
        {
            throw new NotImplementedException();
        }

        public IList<long> NextId(string bizType, int batchSize)
        {
            throw new NotImplementedException();
        }
    }
}
