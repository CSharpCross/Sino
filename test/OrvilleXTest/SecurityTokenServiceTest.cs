using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth.Sts;
using Aliyun.Acs.Core.Profile;
using Aliyun.OSS;
using System;
using Xunit;

namespace OrvilleXTest
{
    /// <summary>
    /// 阿里云STS使用方式演示代码
    /// </summary>
    public class SecurityTokenServiceTest
    {
        [Fact]
        public void GetAccessInfoTest()
        {
            string regionid = "cn-hangzhou";
            string endpoint = "sts.cn-hangzhou.aliyuncs.com"; // 如果是VPC内部，地址为sts-vpc.cn-hangzhou.aliyuncs.com
            string accessid = "";
            string accesskey = "";
            string ossendpoint = "http://oss-cn-hangzhou.aliyuncs.com";

            IClientProfile profile = DefaultProfile.GetProfile(regionid, accessid, accesskey);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            AssumeRoleRequest request = new AssumeRoleRequest();
            request.AcceptFormat = Aliyun.Acs.Core.Http.FormatType.JSON;
            request.RoleArn = "";
            request.RoleSessionName = "tms";
            request.QueryParameters.Add("DurationSeconds", "1800"); // 存在Bug需要使用手动方式注入

            AssumeRoleResponse response = client.GetAcsResponse(request);

            string time = DateTime.Parse(response.Credentials.Expiration).ToLocalTime().ToString();

            Assert.NotNull(response);
            Assert.NotNull(response.Credentials.AccessKeyId);
            Assert.NotNull(response.Credentials.AccessKeySecret);
            Assert.NotNull(response.Credentials.SecurityToken);

            var ossClient = new OssClient(ossendpoint, response.Credentials.AccessKeyId, response.Credentials.AccessKeySecret, response.Credentials.SecurityToken);
            var obj = ossClient.GetObject("", "20200614222040_13fdaea5-5d4a-4d38-9ccd-112fdd32b7a8.jpg");
            using(var fs = obj.Content)
            {

            }
        }
    }
}
