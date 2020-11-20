using System;

namespace OrvilleX
{
    /// <summary>
    /// 未找到该数据
    /// </summary>
    public class EntityNotFoundException : BaseException
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(int code)
            : base(code) { }

        public EntityNotFoundException(string message)
            : base(message) { }

        public EntityNotFoundException(string message, int code)
            : base(message, code) { }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
