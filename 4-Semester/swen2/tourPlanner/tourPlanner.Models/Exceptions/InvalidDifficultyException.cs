using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tourPlanner.Models.Exceptions
{
    public class InvalidDifficultyException : Exception
    {
        public InvalidDifficultyException() { }
        public InvalidDifficultyException(string message) : base(message) { }
        public InvalidDifficultyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
