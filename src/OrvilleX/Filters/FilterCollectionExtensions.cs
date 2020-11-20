using Microsoft.AspNetCore.Mvc.Filters;
using OrvilleX.Filters;
using Sino.Web.Filters;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FilterCollectionExtensions
    {
        /// <summary>
        /// 注入标准输出过滤器
        /// </summary>
        public static IServiceCollection AddStandardResultFilter(this IServiceCollection services)
        {
            services.AddSingleton(new StandardResultFilterAttribute());
            return services;
        }

        /// <summary>
        /// 使用标准输出过滤器
        /// </summary>
        public static IFilterMetadata UseStandardResultFilter(this FilterCollection filters)
        {
            return filters.AddService(typeof(StandardResultFilterAttribute));
        }

        /// <summary>
        /// 注入接口日志记录过滤器
        /// </summary>
        /// <param name="useInput">是否记录输入请求</param>
        /// <param name="useOutput">是否记录输出请求</param>
        public static IServiceCollection AddActionLogFilter(this IServiceCollection services, bool useInput = true, bool useOutput = true)
        {
            services.AddSingleton(new ActionLogFilterAttribute()
            {
                Input = useInput,
                Output = useOutput
            });
            return services;
        }

        /// <summary>
        /// 添加接口日志记录过滤器
        /// </summary>
        /// <param name="useInput">是否记录输入请求</param>
        /// <param name="useOutput">是否记录输出请求</param>
        public static IFilterMetadata UseActionLogFilter(this FilterCollection filters)
        {
            return filters.AddService(typeof(ActionLogFilterAttribute));
        }

        /// <summary>
        /// 注入接口二次校验过滤器
        /// </summary>
        /// <param name="token">密钥令牌</param>
        /// <param name="signatureQueryName">signature查询字符串名称</param>
        /// <param name="timeStampQueryName">时间戳查询字符串名称</param>
        /// <param name="nonceQueryName">随机数查询字符串名称</param>
        /// <param name="errorMessage">接口校验失败提示</param>
        /// <param name="errorCode">接口校验失败代码</param>
        /// <param name="timeOut">令牌时限，单位s</param>
        public static IServiceCollection AddCheckSignatureFilter(this IServiceCollection services, string token, string signatureQueryName = "signature", string timeStampQueryName = "timestamp",
            string nonceQueryName = "nonce", string errorMessage = "接口校验失败", int errorCode = -2, int timeOut = 120)
        {
            services.AddSingleton(new CheckSignatureFilterAttribute()
            {
                Token = token,
                SignatureQueryName = signatureQueryName,
                TimeStampQueryName = timeStampQueryName,
                NonceQueryName = nonceQueryName,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                TimeOut = timeOut
            });
            return services;
        }

        /// <summary>
        /// 添加接口二次校验过滤器
        /// </summary>
        public static IFilterMetadata UseCheckSignatureFilter(this FilterCollection filters)
        {
            return filters.AddService(typeof(CheckSignatureFilterAttribute));
        }
    }
}
