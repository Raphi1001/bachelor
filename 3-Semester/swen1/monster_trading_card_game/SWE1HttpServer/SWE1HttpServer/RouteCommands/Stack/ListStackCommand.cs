using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SWE1HttpServer.Managers.Stacks;

namespace SWE1HttpServer.RouteCommands.Stack
{
    class ListStackCommand : ProtectedRouteCommand
    {
        private IStackManager StackManager { get; }

        public ListStackCommand(IStackManager stackManager)
        {
            StackManager = stackManager;
        }

        public override Response Execute()
        {

            IEnumerable<CardLog> cardLogList = StackManager.ListStackLog(User);
            
            //this is only reached if no exception is thrown
            var response = new Response();
            response.Payload = JsonConvert.SerializeObject(cardLogList);
            response.StatusCode = StatusCode.Ok;

            return response;
        }
    }
}
