using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NLog;
using Sino.Web.Filters;
using Sino.Web.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sino.Web.Test
{
    public class ActionLogFilterAttributeTest
    {
        [Fact]
        public void ActionExecutingNormalTest()
        {
            var mock = new Mock<ILogger>();
            LogInfo info = null;

            mock.Setup(x => x.Info<LogInfo>(It.IsAny<LogInfo>())).
                Callback<LogInfo>(s => info = s);
            var actionContext = CommonFilterTest.CreateActionExecutingContext("test", new Dictionary<string, object>() {
                { "do", "sad"}
            }, "192.168.1.213");

            var actionLogFilter = new ActionLogFilterAttribute(mock.Object);
            actionLogFilter.OnActionExecuting(actionContext);
            
            Assert.NotNull(info);
            Assert.Equal("test", info.Method);
            Assert.Equal("入参记录", info.Description);
        }

        [Fact]
        public void ActionExecutedNormalTest()
        {
            var mock = new Mock<ILogger>();
            LogInfo info = null;

            mock.Setup(x => x.Info(It.IsAny<LogInfo>())).
                Callback<LogInfo>(s => info = s);
            var actionContext = CommonFilterTest.CreateActionExecutedContext("result", "192.168.1.52", "testcontent");

            var actionLogFilter = new ActionLogFilterAttribute(mock.Object);
            actionLogFilter.OnActionExecuted(actionContext);

            Assert.NotNull(info);
            Assert.Equal("result", info.Method);
            Assert.Equal("出参记录", info.Description);
        }
    }
}
