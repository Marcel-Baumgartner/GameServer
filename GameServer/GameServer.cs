using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    public class GameServer
    {
        private int port = 22522;
        private TcpListener socket;
        private Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public static GameServer instance = null;

        #region ServerRules
        public int maxNullPackages = 3;
        public int maxPlayers = 10;
        #endregion

        public void Start(int _port)
        {
            #region Singleton
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new Exception("Server is already running");
            }
            #endregion
            Console.WriteLine("Starting Server");
            port = _port;
            socket = new TcpListener(IPAddress.Any, port);

            for (int i = 0; i <= maxPlayers - 1; i++)
            {
                Client c = new Client(i, null);
                clients.Add(i, c);
            }

            SetupTcp(); //Initailize Tcp Data

            Console.WriteLine("Server started");
        }
        public void SetupTcp()
        {
            Console.WriteLine("Setting up TCP");

            socket.Start();

            socket.BeginAcceptTcpClient(OnClientConnected, null);
        }
        public void DisconnectClient(int _id)
        {
            Client c = clients[_id];
            c.Disconnect();
            clients.Remove(_id);
        }
        public void SendToClient(int _id, Package _package)
        {
            try
            {
                clients[_id].SendBytes(_package.GetBytes());
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error while sending package to client {0} -> {1}", _id, ex.Message));
            }
        }
        private void OnClientConnected(IAsyncResult ar)
        {
            TcpClient client = socket.EndAcceptTcpClient(ar);
            socket.BeginAcceptTcpClient(OnClientConnected, null);

            Console.WriteLine("Incomming Connection Request from " + client.Client.RemoteEndPoint.ToString());

            int i = 0;
            foreach (Client _c in clients.Values)
            {
                if (_c.isEmptry)
                {
                    Client c = new Client(i, client);
                    clients.Remove(i);
                    clients.Add(i, c);
                    Console.WriteLine("Client sucessfully connected");
                    ServerSend.SendWelcomePackage(i);
                    return;
                }
                i++;
            }
            Console.WriteLine("Max Player limit reached. Disconnecting Client");
            client.Close();
        }
    }
}
