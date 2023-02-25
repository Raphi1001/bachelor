using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Exceptions;
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
    class ShowUserDataCommand : ProtectedRouteCommand
    {
        private IUserManager UserManager { get; }

        public string Username { get; private set; }

        public ShowUserDataCommand(IUserManager userManager, string username)
        {
            Username = username;
            UserManager = userManager;
        }

        public override Response Execute()
        {
            UserDataLog userDataCredentials = UserManager.ShowUserData(Username, User);
           
            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;
            response.Payload = JsonConvert.SerializeObject(userDataCredentials);

            return response;
        }
    }
}
