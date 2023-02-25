using SWE1HttpServer.Core.Response;
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
    class AcquirePackageCommand : ProtectedRouteCommand
    {
        private IPackageManager PackageManager { get; }

        public AcquirePackageCommand(IPackageManager packageManager)
        {
            PackageManager = packageManager;
        }

        public override Response Execute()
        {

            PackageManager.AcquirePackage(User);

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;

            return response;
        }
    }
}
