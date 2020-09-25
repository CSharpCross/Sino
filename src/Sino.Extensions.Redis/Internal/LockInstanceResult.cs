using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Extensions.Redis.Internal
{
    public enum LockInstanceResult
    {
        Success,
        Conflicted,
        Error
    }
}
