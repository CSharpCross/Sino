using OrvilleX.AutoIndex.Generator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrvilleX.AutoIndex
{
    public class TinyIdClient : ITinyIdClient
    {
        private readonly IIdGeneratorFactory _idGeneratorFactory;

        public TinyIdClient(IIdGeneratorFactory factory)
        {
            _idGeneratorFactory = factory;
        }

        public async Task<long> NextId(string bizType)
        {
            if (string.IsNullOrEmpty(bizType))
                throw new ArgumentNullException(nameof(bizType));

            var idGenerator = _idGeneratorFactory.GetIdGenerator(bizType);
            return await idGenerator.NextId();
        }

        public async Task<IList<long>> NextId(string bizType, int batchSize)
        {
            if (string.IsNullOrEmpty(bizType))
                throw new ArgumentNullException(nameof(bizType));

            if (batchSize <= 0)
            {
                var ids = new List<long>();
                long id = await NextId(bizType);
                ids.Add(id);
                return ids;
            }
            var idGenerator = _idGeneratorFactory.GetIdGenerator(bizType);
            return await idGenerator.NextId(batchSize);
        }
    }
}
