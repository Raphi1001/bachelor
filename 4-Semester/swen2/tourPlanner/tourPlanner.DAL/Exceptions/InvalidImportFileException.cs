using System.Runtime.Serialization;

namespace tourPlanner.DAL.Exceptions
{
    [Serializable]
    public class InvalidImportFileException : Exception
    {
        public InvalidImportFileException()
        {
        }

        public InvalidImportFileException(string? message) : base(message)
        {
        }

        public InvalidImportFileException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidImportFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}