using Sino.Extensions.AutoIndex.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sino.Extensions.AutoIndex.Service
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface ISegmentIdService
    {
        /// <summary>
        /// 根据bizType获取下一个SegmentId对象
        /// </summary>
        Task<SegmentId> GetNextSegmentId(string bizType);
    }
}
