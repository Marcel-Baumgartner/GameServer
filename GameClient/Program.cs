using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GameClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();
            client.Connect("localhost", 22522);
            Package p = new Package();
            p.WriteInt(0);
            p.WriteInt(1);
            client.GetStream().Write(p.GetAllBytes(), 0, p.GetLenght());
            client.Close();
        }
    }
}
