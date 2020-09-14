namespace Sino.Extensions.AutoIndex.Entity
{
    public class Result
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public ResultCode Code { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }

        public Result(ResultCode code, long id)
        {
            Code = code;
            Id = id;
        }
    }
}
