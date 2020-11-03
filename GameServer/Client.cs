using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace GameServer
{
    class Client
    {
        private TcpClient client = null;
        private Socket socket = null;
        private int id = -1;

        private int nullPackages = 0;

        private byte[] cache = new byte[1024];
        private int bufferSize = 1024;

        public bool isEmptry = false;

        public Client(int _id, TcpClient _client)
        {
            client = _client;
            id = _id;
            Init();
        }

        public void Init()
        {
            if(client == null)
            {
                isEmptry = true;
                return;
            }
            socket = client.Client;

            socket.BeginReceive(cache, 0, bufferSize, SocketFlags.None, ReceiveCallback, null);
        }
        public void Disconnect()
        {
            try
            {
                Console.WriteLine("Disconnecting client " + id);
                client.Close();
            }
            catch (Exception) { }
        }

        public void SendBytes(byte[] _data)
        {
            socket.BeginSend(_data, 0, _data.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendCallback(IAsyncResult ar)
        {
            socket.EndSend(ar);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            int size = socket.EndReceive(ar);

            if(size < 1)
            {
                nullPackages++;

                if(nullPackages >= GameServer.instance.maxNullPackages)
                {
                    Console.WriteLine("Client[{0}] has send to much Nullpackages -> disconnecting", id);
                    GameServer.instance.DisconnectClient(id);
                    return;
                }

                socket.BeginReceive(cache, 0, bufferSize, SocketFlags.None, ReceiveCallback, null);
                return;
            }

            byte[] data = new byte[size];
            Array.Copy(cache, 0, data, 0, size);

            Console.WriteLine("Client[{0}] Incomming package... Size: {1}", id, size);

            Console.WriteLine("Translated to Text: " + new ASCIIEncoding().GetString(data));

            HandlePackage.Handle(id, new Package(data));

            try
            {
                socket.BeginReceive(cache, 0, bufferSize, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception) { }
        }
    }
}
