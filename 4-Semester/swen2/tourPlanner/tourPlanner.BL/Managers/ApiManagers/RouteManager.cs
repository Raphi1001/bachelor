using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tourPlanner.BL.Exceptions;
using tourPlanner.BL.Mapquest;
using tourPlanner.DAL;
using tourPlanner.Models.Enums;
using tourPlanner.Models.Route;

namespace tourPlanner.BL.Managers.ApiManagers
{
    public class RouteManager : IRouteManager
    {
        private readonly IRouteGenerator _routeGenerator;

        public RouteManager(IRouteGenerator routeGenerator)
        {
            _routeGenerator = routeGenerator;
        }

        public async Task<RouteInternal?> CreateRouteAsync(string to, string from, RouteTypeEnum routeType)
        {

            RouteInternal? routeInternal = null;
            try
            {
                RouteTransfere? route = await _routeGenerator.GenerateRouteAsync(to, from, routeType);
                routeInternal = route?.ToInternal();
            }
            catch(Exception)
            {
                //exeption is caught and null returned
            }

            return routeInternal;
        }
    }
}
