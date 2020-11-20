using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OrvilleX.AutoIndex.Entity
{
    public class SegmentId
    {
        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool _isInit;

        /// <summary>
        /// 当前编号
        /// </summary>
        private long _currentId;

        /// <summary>
        /// 允许的最大编号
        /// </summary>
        public long MaxId { get; set; }

        /// <summary>
        /// 需要去加载NextId阈值
        /// </summary>
        public long LoadingId { get; set; }

        public long CurrentId { get => _currentId; set => _currentId = value; }

        /// <summary>
        /// 步进
        /// </summary>
        public int Delta { get; set; }


        public int Remainder { get; set; }

        /// <summary>
        ///  这个方法主要为了1,4,7,10...这种序列准备的
        ///  设置好初始值之后，会以delta的方式递增，保证无论开始id是多少都能生成正确的序列
        ///  如当前是号段是(1000,2000]，delta=3, remainder=0，则经过这个方法后，currentId会先递增到1002,之后每次增加delta
        ///  因为currentId会先递增，所以会浪费一个id，所以做了一次减delta的操作，实际currentId会从999开始增，第一个id还是1002
        /// </summary>
        public void Init()
        {
            if (_isInit)
                return;

            lock (this)
            {
                if (_isInit)
                    return;

                long id = CurrentId;

                if (id % Delta == Remainder)
                {
                    _isInit = true;
                    return;
                }

                for (int i = 0; i <= Delta; i++)
                {
                    id = Interlocked.Increment(ref _currentId);
                    if (id % Delta == Remainder)
                    {
                        Interlocked.Add(ref _currentId, 0 - Delta);
                        _isInit = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 获取下一个编号
        /// </summary>
        public Result NextId()
        {
            Init();
            long id = Interlocked.Add(ref _currentId, Delta);
            if (id > MaxId)
            {
                return new Result(ResultCode.Over, id);
            }
            if (id > LoadingId)
            {
                return new Result(ResultCode.Loading, id);
            }
            return new Result(ResultCode.Normal, id);
        }

        /// <summary>
        /// 当前是否还有可用编号
        /// </summary>
        public bool Useful()
        {
            return CurrentId <= MaxId;
        }
    }
}
