using System;

namespace TeeGonSdk
{
    /// <summary>
    /// TOP客户端异常。
    /// </summary>
    public class TopException : Exception
    {
        private string error;

        public TopException()
            : base()
        {
        }

        public TopException(string message)
            : base(message)
        {
            this.error = message;
        }

        public string Error
        {
            get { return this.error; }
        }
    }
}
