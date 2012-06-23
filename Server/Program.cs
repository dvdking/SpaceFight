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
        private List<ConnectedPlayer> ConnectedPlayers; 

        private NetServer server;
        private NetPeerConfiguration _configuration;

        private static void Main(string[] args)
        {
        }



        public Server()
        {
            ConnectedPlayers = new List<ConnectedPlayer>();

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
                                AddNewPlayer(message.SenderConnection);
                            }
                            else if(status == NetConnectionStatus.Disconnected)
                            {
                                DeletePlayer(message.SenderConnection);
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
            ConnectedPlayer player = new ConnectedPlayer(connection);
            ConnectedPlayers.Add(player);

            NetOutgoingMessage outgoingMessage = server.CreateMessage();
            outgoingMessage.WriteAllFields(player);
            server.SendMessage(outgoingMessage, player.NetConnection, NetDeliveryMethod.Unreliable);
        }

        private void DeletePlayer(NetConnection connection)
        {
            ConnectedPlayer player = FindPlayer(connection);

            NetOutgoingMessage message = server.CreateMessage();
            message.Write((byte)GameData.PlayerDisconected);
            message.Write(player.UID);
            server.SendToAll(message, NetDeliveryMethod.Unreliable);
        }

        private void SendDiscoveryResponse(NetIncomingMessage message)
        {
            NetOutgoingMessage outgoingMessage = server.CreateMessage();
            outgoingMessage.Write("Server name");
            server.SendDiscoveryResponse(outgoingMessage, message.SenderEndpoint);
        }

        private ConnectedPlayer FindPlayer(NetConnection connection)
        {
            return ConnectedPlayers.FirstOrDefault((player => player.NetConnection == connection));
        }
    }
}

