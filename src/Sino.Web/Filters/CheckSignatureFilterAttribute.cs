using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Sino.ViewModels;

namespace Sino.Web.Filters
{
    /// <summary>
    /// 接口安全二次校验过滤器
    /// </summary>
    public class CheckSignatureFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 是否使用接口校验
        /// </summary>
        public bool Use { get; set; } = true;

        /// <summary>
        /// 密钥令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// signature查询字符串名称
        /// </summary>
        public string SignatureQueryName { get; set; } = "signature";

        /// <summary>
        /// 时间戳查询字符串名称
        /// </summary>
        public string TimeStampQueryName { get; set; } = "timestamp";

        /// <summary>
        /// 随机数查询字符串名称
        /// </summary>
        public string NonceQueryName { get; set; } = "nonce";

        /// <summary>
        /// 接口校验失败提示
        /// </summary>
        public string ErrorMessage { get; set; } = "接口校验失败";

        /// <summary>
        /// 接口校验失败代码
        /// </summary>
        public int ErrorCode { get; set; } = -2;

        /// <summary>
        /// 基于校验中的时间戳进行判断，单位s
        /// </summary>
        public int TimeOut { get; set; } = 120;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var attribute = GetAttribute(context);

            if (attribute != null && attribute.Use)
            {
                bool isNotPass = true;
                var queryName = new string[] { SignatureQueryName, TimeStampQueryName, NonceQueryName };
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (var queryValue in context.HttpContext.Request.Headers)
                {
                    if (queryName.Any(x => x.ToLower() == queryValue.Key.ToLower()))
                    {
                        if (queryValue.Value.Count >= 1 && !string.IsNullOrEmpty(queryValue.Value.FirstOrDefault()))
                            values.Add(queryValue.Key.ToLower(), queryValue.Value.FirstOrDefault());
                    }
                }

                if (values.Count == 3)
                {
                    string timestamp = values[TimeStampQueryName.ToLower()];
                    var hash = MD5Hash(Token + timestamp + values[NonceQueryName.ToLower()]);
                    if (!IsTimeStampOutTime(timestamp) && string.Compare(hash, values[SignatureQueryName.ToLower()], true) == 0)
                    {
                        isNotPass = false;
                    }
                }

                if (isNotPass)
                {
                    context.Result = new ObjectResult(new BaseResponse
                    {
                        success = false,
                        errorCode = ErrorCode.ToString(),
                        errorMessage = ErrorMessage
                    });
                    return;
                }
            }
            base.OnActionExecuting(context);
        }

        private CheckSignatureFilterAttribute GetAttribute(ActionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var attribute = descriptor.MethodInfo.GetCustomAttributes(typeof(CheckSignatureFilterAttribute), true).FirstOrDefault();
                return attribute as CheckSignatureFilterAttribute;
            }
            return null;
        }

        private string MD5Hash(string value)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
            var result = new StringBuilder();
            foreach (byte t in bytes)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        private bool IsTimeStampOutTime(string timestamp)
        {
            try
            {
                long origin = Convert.ToInt64(timestamp);
                long dest = GetTimeStamp();
                return origin + TimeOut < dest ? true : false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}
