using SWE1HttpServer.Core.Client;
using SWE1HttpServer.Core.Listener;
using SWE1HttpServer.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Core.Server
{
    public class HttpServer : IServer
    {
        protected readonly IListener listener;
        protected readonly IRouter router;
        protected bool isListening;

        public HttpServer(IPAddress address, int port, IRouter router)
        {
            listener = new Listener.HttpListener(address, port);
            this.router = router;
        }

        public virtual void Start()
        {
            listener.Start();
            isListening = true;

            while (isListening)
            {
                var client = listener.AcceptClient();
                HandleClient(client);
            }
        }

        public virtual void Stop()
        {
            isListening = false;
            listener.Stop();
        }

        protected virtual void HandleClient(IClient client)
        {
            var request = client.ReceiveRequest();

            Response.Response response;
            try
            {
                var command = router.Resolve(request);
                if (command != null)
                {
                    response = command.Execute();
                }
                else
                {
                    response = new Response.Response()
                    {
                        StatusCode = Response.StatusCode.BadRequest
                    };
                }
            }
            catch (RouteNotAuthorizedException)
            {
                response = new Response.Response()
                {
                    StatusCode = Response.StatusCode.Unauthorized
                };
            }
            client.SendResponse(response);
        }
    }
}
