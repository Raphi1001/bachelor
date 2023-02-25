using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tourPlanner.UIL.Exceptions
{

        internal class InvalidInputException : Exception
        {
             internal InvalidInputException() { }
             internal InvalidInputException(string message) : base(message) { }
             internal InvalidInputException(string message, Exception innerException) : base(message, innerException) { }
        }
 
}
