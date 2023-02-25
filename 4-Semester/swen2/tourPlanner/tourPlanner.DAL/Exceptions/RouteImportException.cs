using System.Runtime.Serialization;

namespace tourPlanner.DAL.Exceptions
{
    [Serializable]
    internal class RouteImportException : Exception
    {
        public RouteImportException()
        {
        }

        public RouteImportException(string? message) : base(message)
        {
        }

        public RouteImportException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RouteImportException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}