using System;

namespace OrvilleX
{
    /// <summary>
    /// 操作异常
    /// </summary>
    public class RequestOperationException : BaseException
    {
        public RequestOperationException() { }

        public RequestOperationException(int code)
            : base(code) { }

        public RequestOperationException(string message)
            : base(message) { }

        public RequestOperationException(string message, int code)
            : base(message, code) { }

        public RequestOperationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
