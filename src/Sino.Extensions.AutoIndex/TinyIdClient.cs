using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.AutoIndex
{
    public class TinyIdClient : ITinyIdClient
    {
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
