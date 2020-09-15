using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.Extensions.AutoIndex
{
    /// <summary>
    /// Tiny客户端接口
    /// </summary>
    public interface ITinyIdClient
    {
        /// <summary>
        /// 请求标识符
        /// </summary>
        /// <param name="bizType">类别</param>
        Task<long> NextId(string bizType);

        /// <summary>
        /// 批量请求标识符
        /// </summary>
        /// <param name="bizType">类别</param>
        /// <param name="batchSize">请求数量</param>
        Task<IList<long>> NextId(string bizType, int batchSize);
    }
}
