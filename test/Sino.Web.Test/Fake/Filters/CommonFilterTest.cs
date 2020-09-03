using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Sino.Web.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Web.Test
{
    public class CommonFilterTest
    {
        public static ActionExecutingContext CreateActionExecutingContext(string controllerName, IDictionary<string, object> actionArguments, string clientIp)
        {
            var filter = Mock.Of<IFilterMetadata>();
            
            var actionDescriptor = new ActionDescriptor();
            actionDescriptor.RouteValues.Add("controller", controllerName);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add(ActionLogFilterAttribute.ALIYUN_SLB_CLIENT_IP_HEADER_NAME, clientIp);

            return new ActionExecutingContext(
                new ActionContext(httpContext, new RouteData(), actionDescriptor),
                new IFilterMetadata[] { filter },
                actionArguments,
                controller: new object()
            );
        }

        public static ActionExecutedContext CreateActionExecutedContext(string controllerName, string clientIp, string result)
        {
            var filter = Mock.Of<IFilterMetadata>();

            var actionDescriptor = new ActionDescriptor();
            actionDescriptor.RouteValues.Add("controller", controllerName);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add(ActionLogFilterAttribute.ALIYUN_SLB_CLIENT_IP_HEADER_NAME, clientIp);

            var contentResult = new ContentResult();
            contentResult.Content = result;
            contentResult.StatusCode = 200;

            return new ActionExecutedContext(
                new ActionContext(httpContext, new RouteData(), actionDescriptor),
                new IFilterMetadata[] { filter },
                new object())
            {
                Result = contentResult
            };
        }

        public static ResultExecutingContext CreateResultExecutingContext(object result)
        {
            var filter = Mock.Of<IFilterMetadata>();

            var actionDescription = new ControllerActionDescriptor();
            var objectResult = new ObjectResult(result);

            return new ResultExecutingContext(new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                actionDescription
                ), new IFilterMetadata[] { filter }, objectResult, new object());
        }
    }
}
