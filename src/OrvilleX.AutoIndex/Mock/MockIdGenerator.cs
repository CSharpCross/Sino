using OrvilleX.AutoIndex.Generator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrvilleX.AutoIndex.Mock
{
    public class MockIdGenerator : IIdGenerator
    {
        public long id;

        public Task<long> NextId()
        {
            return Task.FromResult(id++);
        }

        public Task<IList<long>> NextId(int batchSize)
        {
            List<long> ids = new List<long>();
            for(var i = 0;i < batchSize; i++)
            {
                ids.Add(id++);
            }
            return Task.FromResult((IList<long>)ids);
        }
    }
}
