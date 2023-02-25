using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Models;
using SWE1HttpServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Managers.Users;

namespace SWE1HttpServer.RouteCommands.Users
{
    class ShowUserStatsCommand : ProtectedRouteCommand
    {
        private IUserManager UserManager { get; }

        public ShowUserStatsCommand(IUserManager userManager)
        {
            UserManager = userManager;
        }

        public override Response Execute()
        {
            StatsLog stats = UserManager.ShowStats(User);

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;
            response.Payload = JsonConvert.SerializeObject(stats);

            return response;
        }
    }
}
