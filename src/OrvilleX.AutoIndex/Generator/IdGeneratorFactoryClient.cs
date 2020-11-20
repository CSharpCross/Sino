using Microsoft.Extensions.Logging;
using OrvilleX.AutoIndex.Service;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OrvilleX.AutoIndex.Generator
{
    public class IdGeneratorFactoryClient : AbstractIdGeneratorFactory
    {
        private readonly TinyIdClientConfiguration _tinyIdClientConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoggerFactory _loggerFactory;

        private IList<string> _servers;

        public IdGeneratorFactoryClient(TinyIdClientConfiguration tinyIdClientConfiguration, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            _tinyIdClientConfiguration = tinyIdClientConfiguration;
            _httpClientFactory = httpClientFactory;
            _loggerFactory = loggerFactory;

            Init();
        }

        private void Init()
        {
            if (string.IsNullOrEmpty(_tinyIdClientConfiguration.Token))
                throw new ArgumentNullException(nameof(_tinyIdClientConfiguration.Token));
            if (_tinyIdClientConfiguration.Servers == null || _tinyIdClientConfiguration.Servers.Count < 1)
                throw new ArgumentNullException(nameof(_tinyIdClientConfiguration.Servers));

            _servers = new List<string>();
            foreach(var server in _tinyIdClientConfiguration.Servers)
            {
                _servers.Add($"http://{server}/tinyid/id/nextSegmentIdSimple?token={_tinyIdClientConfiguration.Token}&bizType=");
            }
        }

        protected override IIdGenerator CreateIdGenerator(string bizType)
        {
            return new CachedIdGenerator(bizType, new HttpSegmentIdService(_servers, _httpClientFactory, _loggerFactory.CreateLogger<HttpSegmentIdService>()));
        }
    }
}
