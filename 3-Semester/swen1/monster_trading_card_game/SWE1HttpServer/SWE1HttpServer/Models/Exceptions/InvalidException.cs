using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models.Exceptions
{
    [Serializable]
    public class InvalidException : Exception
    {
        public InvalidException()
        {
        }

        public InvalidException(string message) : base(message)
        {
        }

        public InvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
