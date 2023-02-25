using Newtonsoft.Json;
using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Managers.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.RouteCommands.Messages
{
    class ShowScoreboardCommand : ProtectedRouteCommand
    {
        private IUserManager UserManager { get; }

        public ShowScoreboardCommand(IUserManager userManager)
        {
            UserManager = userManager;
        }

        public override Response Execute()
        {
            IEnumerable<int> scoreboard = UserManager.ShowScoreboard();

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;
            response.Payload = JsonConvert.SerializeObject(scoreboard);

            return response;
        }
    }
}
