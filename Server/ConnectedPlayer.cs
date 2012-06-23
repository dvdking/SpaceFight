using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class ConnectedPlayer
    {
        public int UID;
        public ShipData Data;

        public ConnectedPlayer()
        {
            UID = UIDGen.Next();
        }
    }
}
