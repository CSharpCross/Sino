using Microsoft.AspNetCore.Mvc;
using OrvilleX.ViewModels;
using OrvilleXTest.Fake;
using Sino.Web.Filters;
using Xunit;

namespace OrvilleXTest
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
