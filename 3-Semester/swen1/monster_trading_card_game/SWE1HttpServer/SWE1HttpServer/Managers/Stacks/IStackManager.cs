using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers.Stacks
{
    public interface IStackManager
    {
        IEnumerable<CardLog> ListStackLog(User user);
    }
}
