using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Server.Data;

namespace TTT.Server.Games
{
    public class ServerConnection
    {
        public int ConnectionId { get; set; }

        public User User { get; set; }

        public NetPeer Peer { get; set; }

        public Guid? GameId { get; set; }
    }
}
