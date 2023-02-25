using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace tourPlanner.BL.Exceptions
{
    internal class ServerErrorException : Exception
    {
        internal ServerErrorException() : base()
        {
        }
        internal ServerErrorException(string? message) : base(message)
        {
        }
        internal ServerErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
