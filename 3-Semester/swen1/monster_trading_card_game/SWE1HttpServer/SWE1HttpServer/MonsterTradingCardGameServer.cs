using SWE1HttpServer.Core.Client;
using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Core.Routing;
using SWE1HttpServer.Core.Server;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer
{
    public class MonsterTradingCardGameServer : HttpServer
    {
        public MonsterTradingCardGameServer(IPAddress address, int port, IRouter router) : base(address, port, router)
        {
        }
        public override void Start()
        {
            Console.WriteLine("Waiting for Client...");
            listener.Start();
            isListening = true;

            while (isListening)
            {
                var client = listener.AcceptClient();

                Task.Run(() => HandleClient(client));
            }
        }

        protected override void HandleClient(IClient client)
        {
            Console.WriteLine("Recieced Request");

            var request = client.ReceiveRequest();

            Response response = new Response();
            try
            {
                var command = router.Resolve(request);
                if (command is null)
                {
                    throw new BadRequestException();
                }

                response = command.Execute();

            }
            catch (RouteNotAuthorizedException)
            {
                response.StatusCode = StatusCode.Unauthorized;
            }
            catch (BadRequestException)
            {
                response.StatusCode = StatusCode.BadRequest;
            }
            catch (InvalidException)
            {
                response.StatusCode = StatusCode.BadRequest;
            }
            catch (ForbiddenException)
            {
                response.StatusCode = StatusCode.Forbidden;
            }
            catch (ConflictException)
            {
                response.StatusCode = StatusCode.Conflict;
            }
            catch (UnauthorizedAccessException)
            {
                response.StatusCode = StatusCode.Unauthorized;
            }
            catch (NoContentException)
            {
                response.StatusCode = StatusCode.NoContent;
            }
            catch (InternalServerErrorException)
            {
                response.StatusCode = StatusCode.InternalServerError;
            }
            catch (NotImplementedException)
            {
                response.StatusCode = StatusCode.NotImplemented;
            }
                

            Console.WriteLine("Response Status Code: " + response.StatusCode);
            client.SendResponse(response);
        }

    }
}

