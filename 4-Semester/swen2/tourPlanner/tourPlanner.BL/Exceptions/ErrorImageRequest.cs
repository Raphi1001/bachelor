using System.Runtime.Serialization;

namespace tourPlanner.BL.Exceptions
{
    [Serializable]
    internal class ErrorImageRequest : Exception
    {
        public ErrorImageRequest()
        {
        }

        public ErrorImageRequest(string? message) : base(message)
        {
        }

        public ErrorImageRequest(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ErrorImageRequest(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}