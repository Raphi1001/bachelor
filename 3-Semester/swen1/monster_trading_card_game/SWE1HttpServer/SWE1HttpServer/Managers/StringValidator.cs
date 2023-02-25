using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers
{
    public static class StringValidator
    {
        public static void Validate(string str)
        {
            if (str is null || !Regex.IsMatch(str, @"^[a-zA-Z0-9_:\x20.!?\-)]+$"))
                throw new InvalidException();
        }

        public static void ValidateEmpty(string str)
        {
            if (str is null || !Regex.IsMatch(str, @"^$|^[a-zA-Z0-9_:\x20.!?\-)]+$"))
                throw new InvalidException();
        }
    }
}
