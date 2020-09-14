using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.AutoIndex
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
