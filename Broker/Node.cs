using Ledger;
using Monix;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Broker
{
    public class Node : Server
    {
        private readonly Blockchain<Transaction> chain;

       
        private bool isSynced { get; set; }

        public Node(Blockchain<Transaction> chain, IPAddress address, int port) : base(address, port)
        {
            this.chain = chain;
        }

        protected override IResponse HandleRequest(IRequest request)
        {
            if(request.Headers.ContainsKey("type"))
            {
                var type = request.Headers["type"];

            }
            else
            {
                return Response.Create(ContentType.Plain, ResponseType.ClientError);
            }

            throw new NotImplementedException();
        }
    }
}
