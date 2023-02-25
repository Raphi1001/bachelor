using System.Runtime.Serialization;

namespace tourPlanner.BL.Exceptions
{
    [Serializable]
    internal class InvalidRouteException : Exception
    {
        public InvalidRouteException()
        {
        }

        public InvalidRouteException(string? message) : base(message)
        {
        }

        public InvalidRouteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidRouteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}