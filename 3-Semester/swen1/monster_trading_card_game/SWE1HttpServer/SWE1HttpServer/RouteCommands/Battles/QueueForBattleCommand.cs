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
using System.Threading;
using SWE1HttpServer.Managers.Battles;

namespace SWE1HttpServer.RouteCommands.Battles
{
    class QueueForBattleCommand : ProtectedRouteCommand
    {
        private IBattleManager BattleManager { get; }

        public QueueForBattleCommand(IBattleManager battleManager)
        {
            BattleManager = battleManager;
        }

        public override Response Execute()
        {
            BattleLog battleLog = BattleManager.QueueForBattle(User);
            
            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;
            response.Payload = JsonConvert.SerializeObject(battleLog);

            return response;
        }
    }
}
