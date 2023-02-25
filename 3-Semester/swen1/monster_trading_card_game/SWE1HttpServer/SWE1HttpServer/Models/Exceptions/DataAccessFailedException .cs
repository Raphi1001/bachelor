using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models.Exceptions
{
    [Serializable]
    internal class DataAccessFailedException : Exception
    {
        public DataAccessFailedException()
        {
        }

        public DataAccessFailedException(string message) : base(message)
        {
        }

        public DataAccessFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
