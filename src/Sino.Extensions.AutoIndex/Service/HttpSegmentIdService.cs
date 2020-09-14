using Microsoft.Extensions.Logging;
using Sino.Extensions.AutoIndex.Entity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sino.Extensions.AutoIndex.Service
{
    /// <summary>
    /// 基于HTTP实现的服务
    /// </summary>
    public class HttpSegmentIdService : ISegmentIdService
    {
        private readonly ILogger<HttpSegmentIdService> _logger;

        protected IList<string> Servers { get; set; }

        protected IHttpClientFactory HttpClientFactory { get; set; }

        public HttpSegmentIdService(IList<string> servers, IHttpClientFactory httpClientFactory, ILogger<HttpSegmentIdService> logger)
        {
            _logger = logger;
            Servers = servers;
            HttpClientFactory = httpClientFactory;
        }

        public async Task<SegmentId> GetNextSegmentId(string bizType)
        {
            var httpClient = HttpClientFactory.CreateClient();
            string url = ChooseService(bizType);

            using(var response = await httpClient.PostAsync(url, new StringContent("")))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                SegmentId segmentId = new SegmentId();
                string[] arr = content.Split(',');
                segmentId.CurrentId = long.Parse(arr[0]);
                segmentId.LoadingId = long.Parse(arr[1]);
                segmentId.MaxId = long.Parse(arr[2]);
                segmentId.Delta = int.Parse(arr[3]);
                segmentId.Remainder = int.Parse(arr[4]);
                return segmentId;
            }
        }

        private string ChooseService(string bizType)
        {
            string url = "";
            if (Servers != null && Servers.Count == 1)
            {
                url = Servers[0];
            }
            else if(Servers != null && Servers.Count > 1)
            {
                Random r = new Random();
                url = Servers[r.Next(Servers.Count)];
            }
            url += bizType;
            return url;
        }
    }
}
