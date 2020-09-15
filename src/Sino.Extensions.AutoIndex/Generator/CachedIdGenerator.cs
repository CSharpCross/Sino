using Sino.Extensions.AutoIndex.Entity;
using Sino.Extensions.AutoIndex.Service;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sino.Extensions.AutoIndex.Generator
{
    /// <summary>
    /// 带有缓存的编号生成
    /// </summary>
    public class CachedIdGenerator : IIdGenerator
    {
        private volatile SegmentId _current;
        private volatile SegmentId _next;
        private volatile bool _isLoadingNext;
        private object _lock = new object();

        protected string BizType { get; set; }
        protected ISegmentIdService SegmentIdService { get; set; }

        public CachedIdGenerator(string bizType, ISegmentIdService segmentIdService)
        {
            BizType = bizType;
            SegmentIdService = segmentIdService;
        }

        public async Task LoadCurrent()
        {
            if (_current == null || !_current.Useful())
            {
                if (_next == null)
                {
                    SegmentId segmentId = await QuerySegmentId();
                    _current = segmentId;
                }
                else
                {
                    _current = _next;
                    _next = null;
                }
            }
        }

        private async Task<SegmentId> QuerySegmentId()
        {
            return await SegmentIdService.GetNextSegmentId(BizType);
        }

        public void LoadNext()
        {
            if (_next == null && !_isLoadingNext)
            {
                lock(_lock)
                {
                    if(_next == null && !_isLoadingNext)
                    {
                        _isLoadingNext = true;
                        ThreadPool.QueueUserWorkItem(async (x) =>
                        {
                            try
                            {
                                _next = await QuerySegmentId();
                            }
                            finally
                            {
                                _isLoadingNext = false;
                            }
                        });
                    }
                }
            }
        }

        public async Task<long> NextId()
        {
            while(true)
            {
                if (_current == null)
                {
                    await LoadCurrent();
                    continue;
                }
                Result result = _current.NextId();
                if (result.Code == ResultCode.Over)
                {
                    await LoadCurrent();
                }
                else
                {
                    if (result.Code == ResultCode.Loading)
                    {
                        LoadNext();
                    }
                    return result.Id;
                }
            }
        }

        public async Task<IList<long>> NextId(int batchSize)
        {
            var ids = new List<long>();
            for (int i = 0; i < batchSize; i++)
            {
                long id = await NextId();
                ids.Add(id);
            }
            return ids;
        }
    }
}
