using OrvilleX.AutoIndex.Entity;
using System.Threading.Tasks;

namespace OrvilleX.AutoIndex.Service
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
