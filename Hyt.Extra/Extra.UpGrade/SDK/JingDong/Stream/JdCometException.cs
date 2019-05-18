using System;

namespace Extra.UpGrade.SDK.JingDong.Stream
{
    public class JdCometException : JdException
    {
        public JdCometException()
            : base()
        {
        }

        public JdCometException(string message, Exception cause)
            : base(message, cause)
        {
        }

        public JdCometException(string message)
            : base(message)
        {
        }
    }
}
