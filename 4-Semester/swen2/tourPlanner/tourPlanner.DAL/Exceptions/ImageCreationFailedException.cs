using System.Runtime.Serialization;

namespace tourPlanner.DAL.Exceptions
{
    [Serializable]
    internal class ImageCreationFailedException : Exception
    {
        public ImageCreationFailedException()
        {
        }

        public ImageCreationFailedException(string? message) : base(message)
        {
        }

        public ImageCreationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ImageCreationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
