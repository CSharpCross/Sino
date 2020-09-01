using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Sino.IntegrationTest
{
    public class ValidationTest : IClassFixture<WebAppFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _client;

        public ValidationTest(ITestOutputHelper output, WebAppFixture webApp)
        {
            _output = output;
            _client = webApp.CreateClient();
        }


    }
}
