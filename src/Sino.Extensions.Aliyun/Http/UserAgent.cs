using Sino.Extensions.Aliyun.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Sino.Extensions.Aliyun.Http
{
    public class UserAgent
    {
        private IList<string> _excludedList = new List<string>();
        private IDictionary<string, string> _userAgent = new Dictionary<string, string>();

        public static string DefaultMessage { get; private set; }

        public string ClientVersion { get; private set; }

        public string CoreVersion { get; private set; }

        public string OSVersion { get; private set; }

        public UserAgent()
        {
            Init();
        }

        private void Init()
        {
            OSVersion = RuntimeInformation.OSDescription;
            ClientVersion = GetRuntimeRegexValue();
            CoreVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _excludedList.Add("core");
            _excludedList.Add("microsoft.netcore.app");
            DefaultMessage = $"Alibaba Cloud ({OSVersion}) {ClientVersion} Core/{CoreVersion}";
        }

        private string GetRuntimeRegexValue()
        {
            string value = RuntimeEnvironment.GetRuntimeDirectory();
            var rx = new Regex(@"(\.NET).*(\\|\/).*(\d)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = rx.Match(value);
            char[] separator = { '\\', '/' };

            if (matches.Success)
            {
                var clientValueArray = matches.Value.Split(separator);
                var finalValue = "";
                for(var i = 0; i < clientValueArray.Length - 1; ++i)
                {
                    finalValue += clientValueArray[i].Replace(".", "").ToLower();
                }
                finalValue += "/" + clientValueArray[clientValueArray.Length - 1];
                return finalValue;
            }

            return "RuntimeNotFound";
        }

        public void AppendUserAgent(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return;
            }

            if (_excludedList.Contains(key.ToLowerInvariant()))
            {
                return;
            }

            DictionaryUtil.Add(_userAgent, key, value);
        }

        public ReadOnlyDictionary<string, string> GetSysUserAgentDict()
        {
            return new ReadOnlyDictionary<string, string>(_userAgent);
        }

        public static string Resolve(UserAgent requestConfig, UserAgent clientConfig)
        {
            var finalDict = new Dictionary<string, string>();
            if (clientConfig != null && clientConfig.GetSysUserAgentDict().Count > 0)
            {
                finalDict = new Dictionary<string, string>(clientConfig.GetSysUserAgentDict());
            }

            if (requestConfig != null && requestConfig.GetSysUserAgentDict().Count > 0)
            {
                finalDict = new Dictionary<string, string>(requestConfig.GetSysUserAgentDict());
            }

            var agent = new StringBuilder(DefaultMessage);

            foreach (var item in finalDict)
            {
                agent.Append(" " + item.Key + "/" + item.Value);
            }

            return agent.ToString();
        }
    }
}
