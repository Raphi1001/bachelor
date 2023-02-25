using System.Runtime.Serialization;

namespace tourPlanner.BL.Exceptions
{
    [Serializable]
    internal class ImportedTourAlreadyExistsException : Exception
    {
        public ImportedTourAlreadyExistsException()
        {
        }

        public ImportedTourAlreadyExistsException(string? message) : base(message)
        {
        }

        public ImportedTourAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ImportedTourAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}