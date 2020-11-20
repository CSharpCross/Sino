using System;

namespace OrvilleX
{
    /// <summary>
    /// 已存在相同数据异常
    /// </summary>
    public class EntityAlreadyExistException : BaseException
    {
        public EntityAlreadyExistException() { }

        public EntityAlreadyExistException(int code)
            : base(code) { }

        public EntityAlreadyExistException(string message)
            : base(message) { }

        public EntityAlreadyExistException(string message, int code)
            : base(message, code) { }

        public EntityAlreadyExistException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
