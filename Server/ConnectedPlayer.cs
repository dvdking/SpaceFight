using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace Server
{
    class ConnectedPlayer
    {
        public int UID;
        public ShipData Data;

        public NetConnection NetConnection { get; private set; }

        public ConnectedPlayer(NetConnection netConnection)
        {
            UID = UIDGen.Next();
            NetConnection = netConnection;
        }
    }
}
