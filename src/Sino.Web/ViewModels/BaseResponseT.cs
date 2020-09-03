using Newtonsoft.Json;

namespace Sino.ViewModels
{
    /// <summary>
    /// 视图输出根类
    /// </summary>
    public class BaseResponse<T>
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 是否请求成功
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; } = true;

        /// <summary>
        /// 数据对象
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
