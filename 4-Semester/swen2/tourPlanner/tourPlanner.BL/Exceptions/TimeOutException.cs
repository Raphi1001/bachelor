using System.Runtime.Serialization;

namespace tourPlanner.BL.Exceptions
{
    [Serializable]
    internal class TimeOutException : Exception
    {
        public TimeOutException() : base()
        {
        } 
        public TimeOutException(string? message) : base(message)
        {
        }
        public TimeOutException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TimeOutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}