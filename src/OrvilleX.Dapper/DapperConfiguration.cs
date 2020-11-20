using System;
using System.Collections.Generic;
using System.Text;

namespace OrvilleX.Dapper
{
    public class DapperConfiguration : IDapperConfiguration
    {
        public string ReadConnectionString { get; set; }

        public string WriteConnectionString { get; set; }
    }
}
