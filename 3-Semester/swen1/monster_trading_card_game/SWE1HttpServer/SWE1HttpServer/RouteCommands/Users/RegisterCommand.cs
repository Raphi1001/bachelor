using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Core.Routing;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1HttpServer.Managers.Users;

namespace SWE1HttpServer.RouteCommands.Users
{
    class RegisterCommand : IRouteCommand
    {
        private IUserManager UserManager { get; }
        public UserCredentialsLog UserCredentialsLog { get; private set; }

        public RegisterCommand(IUserManager userManager, UserCredentialsLog userCredentialsLog)
        {
            UserCredentialsLog = userCredentialsLog;
            UserManager = userManager;
        }

        public Response Execute()
        {
            UserManager.RegisterUser(UserCredentialsLog);

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Created;

            return response;
        }
    }
}
