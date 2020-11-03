using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class ServerSend
    {
        public static void SendPackageToClient(int from, int _id, Package _package)
        {
            Package _toSend = new Package();
            _toSend.WriteInt(from);
            _toSend.WriteInt(_id);
            _toSend.Write(_package.GetAllBytes());
            GameServer.instance.SendToClient(_id, _toSend);
        }
        public static void SendBroadcast(int _from, Package _package)
        {
            Console.WriteLine("Bytes to broadcast: " + _package.GetAllBytes().Length);
            for(int i = 0; i <= GameServer.instance.maxPlayers; i++)
            {
                if(i != _from)
                {
                    try
                    {
                        SendPackageToClient(_from, i, _package);
                    }
                    catch (Exception) { }
                }
            }
        }
        public static void SendWelcomePackage(int id)
        {
            ServerSend.SendPackageToClient(-1, id, Packages.PlayerWelcomePackage("Welcome to this server"));
        }
    }
}
