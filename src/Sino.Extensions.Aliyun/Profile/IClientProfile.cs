using Sino.Extensions.Aliyun.Auth;
using Sino.Extensions.Aliyun.Http;
using Sino.Extensions.Aliyun.Regions;
using System.Collections.Generic;

namespace Sino.Extensions.Aliyun.Profile
{
    public interface IClientProfile
    {
        string DefaultClientName { get; set; }

        ISigner GetSigner();

        string GetRegionId();

        FormatType GetFormat();

        AliyunCredential GetCredential();

        List<Endpoint> GetEndpoints(string product, string regionId, string serviceCode, string endpointType);

        void SetLocationConfig(string regionId, string product, string endpoint);

        void SetCredentialsProvider(IAliyunCredentialsProvider credentialsProvider);

        void AddEndpoint(string endpointName, string regionId, string product, string domain,
            bool isNeverExpire = false);
    }
}
