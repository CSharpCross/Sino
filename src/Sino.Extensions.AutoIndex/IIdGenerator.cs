using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.AutoIndex
{
    /// <summary>
    /// 生成器必须实现的接口
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// 获取下一个标识
        /// </summary>
        long NextId();

        /// <summary>
        /// 批量获取标识
        /// </summary>
        /// <param name="batchSize">获取个数</param>
        IList<long> NextId(int batchSize);
    }
}
