using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Sino.ViewModels;
using System.Linq;

namespace Sino.Web.Filters
{
    /// <summary>
    /// 标准输出格式化过滤器
    /// </summary>
    public class StandardResultFilterAttribute : ResultFilterAttribute
    {
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUse { get; set; } = true;

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var attribute = descriptor.MethodInfo.GetCustomAttributes(typeof(StandardResultFilterAttribute), true).FirstOrDefault();
                if (attribute != null && attribute is StandardResultFilterAttribute standardResult)
                {
                    if (standardResult.IsUse)
                    {
                        var result = context.Result;
                        if (result is EmptyResult || result is ObjectResult)
                        {
                            context.Result = result is EmptyResult ? new ObjectResult(null) : result;
                            var obj = context.Result as ObjectResult;
                            if (!(obj.Value is BaseResponse))
                            {
                                obj.Value = new BaseResponse
                                {
                                    success = true,
                                    data = obj.Value
                                };
                            }
                        }
                    }
                }
            }
            base.OnResultExecuting(context);
        }
    }
}