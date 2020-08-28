using Microsoft.AspNetCore.Mvc.Filters;
using Sino.Web.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FilterCollectionExtensions
    {
        /// <summary>
        /// 添加标准输出过滤器
        /// </summary>
        public static IFilterMetadata AddStandardResultFilter(this FilterCollection filters, IServiceCollection services)
        {
            services.AddSingleton(new StandardResultFilterAttribute());

            return filters.AddService(typeof(StandardResultFilterAttribute));
        }

        /// <summary>
        /// 添加接口日志记录过滤器
        /// </summary>
        /// <param name="useInput">是否记录输入请求</param>
        /// <param name="useOutput">是否记录输出请求</param>
        public static IFilterMetadata AddActionLogFilter(this FilterCollection filters, IServiceCollection services, bool useInput = true, bool useOutput = true)
        {
            services.AddSingleton(new ActionLogFilterAttribute()
            {
                Input = useInput,
                Output = useOutput
            });

            return filters.AddService(typeof(ActionLogFilterAttribute));
        }

        /// <summary>
        /// 添加接口二次校验过滤器
        /// </summary>
        /// <param name="token">密钥令牌</param>
        /// <param name="signatureQueryName">signature查询字符串名称</param>
        /// <param name="timeStampQueryName">时间戳查询字符串名称</param>
        /// <param name="nonceQueryName">随机数查询字符串名称</param>
        /// <param name="errorMessage">接口校验失败提示</param>
        /// <param name="errorCode">接口校验失败代码</param>
        /// <param name="timeOut">令牌时限，单位s</param>
        public static IFilterMetadata AddCheckSignatureFilter(this FilterCollection filters, IServiceCollection services, string token, string signatureQueryName = "signature", string timeStampQueryName = "timestamp",
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

            return filters.AddService(typeof(CheckSignatureFilterAttribute));
        }
    }
}
