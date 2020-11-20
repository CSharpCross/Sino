using System;
using System.Collections.Generic;
using System.Text;

namespace OrvilleX.AutoIndex.Generator
{
    /// <summary>
    /// Id生成器工厂接口
    /// </summary>
    public interface IIdGeneratorFactory
    {
        /// <summary>
        /// 根据分类获取ID生成器
        /// </summary>
        IIdGenerator GetIdGenerator(string bizType);
    }
}
