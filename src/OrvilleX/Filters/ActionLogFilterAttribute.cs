using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using OrvilleX.Logging;
using System.Linq;

namespace OrvilleX.Filters
{
    /// <summary>
    /// 动作输入输出日志记录过滤器
    /// </summary>
    public class ActionLogFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public const string ALIYUN_SLB_CLIENT_IP_HEADER_NAME = "X-Forwarded-For";

        public ActionLogFilterAttribute()
            : base() 
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public ActionLogFilterAttribute(ILogger logger)
            : base() 
        {
            _logger = logger;
        }

        /// <summary>
        /// 是否记录输入日志
        /// </summary>
        public bool Input { get; set; } = true;

        /// <summary>
        /// 是否记录输出日志
        /// </summary>
        public bool Output { get; set; } = true;

        /// <summary>
        /// 客户端IP所在头部名称
        /// </summary>
        public string ClientIPHeaderName { get; set; } = ALIYUN_SLB_CLIENT_IP_HEADER_NAME;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var useInput = GetAttribute(context)?.Input ?? Input;
            if (useInput)
            {
                _logger.Info(new LogInfo()
                {
                    Method = context.ActionDescriptor.RouteValues["controller"],
                    Argument = new
                    {
                        RequestId = context.HttpContext.TraceIdentifier,
                        Arguments = context.ActionArguments,
                        Host = GetClientUserIp(context)
                    },
                    Description = "入参记录"
                });
            }
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var useOutput = GetAttribute(context)?.Output ?? Output;
            if (useOutput)
            {
                _logger.Info(new LogInfo()
                {
                    Method = context.ActionDescriptor.RouteValues["controller"],
                    Argument = new
                    {
                        RequestId = context.HttpContext.TraceIdentifier,
                        context.Result,
                        Host = GetClientUserIp(context)
                    },
                    Description = "出参记录"
                });
            }
            base.OnActionExecuted(context);
        }

        private ActionLogFilterAttribute GetAttribute(ActionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var attribute = descriptor.MethodInfo.GetCustomAttributes(typeof(ActionLogFilterAttribute), true).FirstOrDefault();
                return attribute as ActionLogFilterAttribute;
            }
            return null;
        }

        private string GetClientUserIp(ActionContext context)
        {
            var ip = context.HttpContext.Request.Headers[ClientIPHeaderName].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
                ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            return ip;
        }
    }
}
