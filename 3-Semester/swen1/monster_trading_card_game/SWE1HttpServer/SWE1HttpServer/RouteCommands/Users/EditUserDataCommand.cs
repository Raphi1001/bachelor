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
    class EditUserDataCommand : ProtectedRouteCommand
    {
        private IUserManager UserManager { get; }
        public string Username { get; private set; }
        public UserDataLog UserDataLog;

        public EditUserDataCommand(IUserManager userManager, string username, UserDataLog userDataLog)
        {
            UserManager = userManager;
            Username = username;
            UserDataLog = userDataLog;
        }

        public override Response Execute()
        {
            UserManager.EditUserData(Username, User, UserDataLog);
           
            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;
            return response;
        }
    }
}
