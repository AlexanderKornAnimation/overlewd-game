using System;

namespace Nutaku.Unity
{
    /// <summary>
    /// Thrown when Nutaku SDK initialization fails.
    /// </summary>
    public class InitializationException : Exception
    {
        public InitializationException()
        {
        }

        public InitializationException(string message)
            : base(message)
        {
        }

        public InitializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
