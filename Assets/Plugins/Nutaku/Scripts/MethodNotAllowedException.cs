using System;

namespace Nutaku.Unity
{
    /// <summary>
	/// It is thrown when using the REST API which can not be used.
	/// If GuestPlay becomes available for APK games, this exception would also be thrown for them performing certain actions.
    /// </summary>
    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException()
        {
        }

        public MethodNotAllowedException(string message)
            : base(message)
        {
        }

        public MethodNotAllowedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
