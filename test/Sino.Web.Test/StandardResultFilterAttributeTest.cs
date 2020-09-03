using Microsoft.AspNetCore.Mvc;
using Sino.ViewModels;
using Sino.Web.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sino.Web.Test
{
    public class StandardResultFilterAttributeTest
    {
        [Fact]
        public void ResultExecuting()
        {
            var resultContext = CommonFilterTest.CreateResultExecutingContext(new Info
            {
                Node = "test"
            });

            var filter = new StandardResultFilterAttribute();
            filter.OnResultExecuting(resultContext);

            var objectResult = resultContext.Result as ObjectResult;
            var response = objectResult.Value as BaseResponse;
            var info = response.Data as Info;

            Assert.NotNull(info);
            Assert.True(response.Success);
            Assert.Equal("0", response.ErrorCode);
            Assert.Equal("test", info.Node);
        }
    }

    public class Info
    {
        public string Node { get; set; }
    }
}
