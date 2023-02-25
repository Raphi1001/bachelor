using System.Runtime.Serialization;

namespace tourPlanner.Models.Exceptions
{

    [Serializable]
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException()
        {
        }

        public InvalidParameterException(string? message) : base(message)
        {
        }

        public InvalidParameterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
