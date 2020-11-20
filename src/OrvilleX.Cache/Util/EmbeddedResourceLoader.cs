using System.IO;
using System.Reflection;

namespace OrvilleX.Cache.Util
{
    internal static class EmbeddedResourceLoader
    {
        /// <summary>
        /// 读取内置资源
        /// </summary>
        internal static string GetEmbeddedResource(string name)
        {
            var assembly = typeof(EmbeddedResourceLoader).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream(name))
            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
