using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models.Exceptions
{
    [Serializable]
    public class NoContentException : Exception
    {
        public NoContentException()
        {
        }

        public NoContentException(string message) : base(message)
        {
        }

        public NoContentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
