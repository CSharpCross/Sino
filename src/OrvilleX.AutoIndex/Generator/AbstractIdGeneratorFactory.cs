using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace OrvilleX.AutoIndex.Generator
{
    /// <summary>
    /// Id生成器抽象工厂
    /// </summary>
    public abstract class AbstractIdGeneratorFactory : IIdGeneratorFactory
    {
        private static ConcurrentDictionary<string, IIdGenerator> _generators = new ConcurrentDictionary<string, IIdGenerator>();

        public IIdGenerator GetIdGenerator(string bizType)
        {
            if (_generators.TryGetValue(bizType, out var generator))
            {
                return generator;
            }

            var idgenerator = CreateIdGenerator(bizType);
            _generators.TryAdd(bizType, idgenerator);

            return idgenerator;
        }

        protected abstract IIdGenerator CreateIdGenerator(string bizType);
    }
}
