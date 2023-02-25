using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1HttpServer.Managers.Packages;

namespace SWE1HttpServer.RouteCommands.Package
{
    class AddPackageCommand : ProtectedRouteCommand
    {
        private IPackageManager PackageManager { get; }

        public IList<CardLog> PackageCredentials { get; }

        public AddPackageCommand(IPackageManager packageManager, IList<CardLog> packageCredentials)
        {
            PackageManager = packageManager;
            PackageCredentials = packageCredentials;
        }

        public override Response Execute()
        {         
            PackageManager.AddPackage(User, PackageCredentials);

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Created;


            return response;
        }
    }
}
