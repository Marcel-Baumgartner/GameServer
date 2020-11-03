using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class ServerSend
    {
        public static void SendPackageToClient(int _id, Package _package)
        {
            GameServer.instance.SendToClient(_id, _package);
        }
        public static void SendBroadcast(int _from, Package _package)
        {
            _package.myBytes.RemoveRange(0, 4);
            Console.WriteLine("Bytes to broadcast: " + _package.myBytes.Count);
            for(int i = 0; i <= GameServer.instance.maxPlayers; i++)
            {
                if(i != _from)
                {
                    try
                    {
                        SendPackageToClient(i, _package);
                    }
                    catch (Exception) { }
                }
            }
        }
        public static void SendWelcomePackage(int id)
        {
            Package p = new Package(new ASCIIEncoding().GetBytes("Welcome to this server"));
            ServerSend.SendPackageToClient(id, p);
        }
    }
}
