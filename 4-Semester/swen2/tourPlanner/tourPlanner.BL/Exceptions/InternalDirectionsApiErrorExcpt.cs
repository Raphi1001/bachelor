using System.Runtime.Serialization;

namespace tourPlanner.BL
{
    [Serializable]
    public class InternalDirectionsApiErrorExcpt : Exception
    {
        public InternalDirectionsApiErrorExcpt()
        {
        }

        public InternalDirectionsApiErrorExcpt(string? message) : base(message)
        {
        }

        public InternalDirectionsApiErrorExcpt(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InternalDirectionsApiErrorExcpt(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}