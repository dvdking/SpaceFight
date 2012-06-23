using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;
using Space_Fight;

namespace Server
{
    internal class Server
    {
        private NetServer server;
        private NetPeerConfiguration _configuration;

        private static void Main(string[] args)
        {
        }



        public Server()
        {
            _configuration = new NetPeerConfiguration("space_fight");
            server = new NetServer(_configuration);

        }

        public void Run()
        {
            server.Start();
            NetIncomingMessage message;
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                while ((message = server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            SendDiscoveryResponse(message);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();

                            if(status == NetConnectionStatus.Connected)
                            {
                                //add new player
                            }
                            else if(status == NetConnectionStatus.Disconnected)
                            {
                                //delete old player
                            }
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine(message.ReadString());
                            break;
                        case NetIncomingMessageType.Data:
                            switch ((GameData)message.ReadByte())
                            {
                                case GameData.ShipData:
                                    ShipData sd = new ShipData();
                                    message.ReadAllFields(sd);
                                    break;
                            }
                            break;

                    }
                }
            }
        
        }

        private void AddNewPlayer(NetConnection connection)
        {


        }

        private void SendDiscoveryResponse(NetIncomingMessage message)
        {
            NetOutgoingMessage outgoingMessage = server.CreateMessage();
            outgoingMessage.Write("Server name");
            server.SendDiscoveryResponse(outgoingMessage, message.SenderEndpoint);
        }
    }
}

