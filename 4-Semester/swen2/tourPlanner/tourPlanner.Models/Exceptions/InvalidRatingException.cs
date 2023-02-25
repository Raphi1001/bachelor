using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tourPlanner.Models.Exceptions
{
    public class InvalidRatingException : Exception
    {
        public InvalidRatingException() { }
        public InvalidRatingException(string message) : base(message) { }
        public InvalidRatingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
