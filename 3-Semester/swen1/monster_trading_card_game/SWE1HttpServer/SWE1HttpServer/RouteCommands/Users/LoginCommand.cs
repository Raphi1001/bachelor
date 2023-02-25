using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Core.Routing;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1HttpServer.Managers.Users;
using Newtonsoft.Json;

namespace SWE1HttpServer.RouteCommands.Users
{
    class LoginCommand : IRouteCommand
    {
        private IUserManager UserManager { get; }
        public UserCredentialsLog UserCredentialsLog { get; private set; }

        public LoginCommand(IUserManager userManager, UserCredentialsLog userCredentialsLog)
        {
            UserCredentialsLog = userCredentialsLog;
            UserManager = userManager;
        }

        public Response Execute()
        {
            User user = UserManager.LoginUser(UserCredentialsLog);

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;
            response.Payload = JsonConvert.SerializeObject(user.Token);

            return response;
        }

    }
}
