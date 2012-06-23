using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren;
using Lidgren.Network;
namespace Space_Fight
{
    class NetworkManager
    {
        NetPeerConfiguration _netPeerConfiguration;
        NetServer netServer;
        private NetClient netClient;

        public NetworkManager()
        {
            _netPeerConfiguration = new NetPeerConfiguration("hm...");
            netServer = new NetServer(_netPeerConfiguration);
        }
    }
}
