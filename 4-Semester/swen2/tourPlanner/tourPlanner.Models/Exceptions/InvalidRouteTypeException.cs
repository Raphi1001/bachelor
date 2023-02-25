using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tourPlanner.Models.Exceptions
{
    public class InvalidRouteTypeException : Exception
    {
        public InvalidRouteTypeException() { }
        public InvalidRouteTypeException(string message) : base(message) { }
        public InvalidRouteTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
