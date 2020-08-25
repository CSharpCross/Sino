using Sino.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Dapper
{
    public interface IDapperConfiguration : ISingletonDependency
    {
        /// <summary>
        /// 写连接字符串
        /// </summary>
		string WriteConnectionString { get; set; }

        /// <summary>
        /// 读连接字符串
        /// </summary>
        string ReadConnectionString { get; set; }
    }
}
