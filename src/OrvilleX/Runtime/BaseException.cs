using System;

namespace OrvilleX
{
    public class BaseException : Exception
    {
        public int Code { get; private set; }

        public BaseException() { }

        public BaseException(int code)
        {
            this.Code = code;
        }

        public BaseException(string message)
            : base(message) { }

        public BaseException(string message,int code)
            : base(message)
        {
            this.Code = code;
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
