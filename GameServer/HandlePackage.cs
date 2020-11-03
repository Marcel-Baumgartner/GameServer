using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class HandlePackage
    {
        public static void Handle(int _from, Package _package)
        {
            if (_package.GetBytes().Length < 5)
            {
                Console.WriteLine("An unknow package has been received. -> diconnecting client " + _from);
                GameServer.instance.DisconnectClient(_from);
            }

            int id = BitConverter.ToInt32(_package.GetBytes(), 0);

            switch(id)
            {
                case 0: //Broadcast
                    ServerSend.SendBroadcast(_from, _package);
                    break;
                case 1:
                    _package.myBytes.RemoveRange(0, 4);
                    int targetId = BitConverter.ToInt32(_package.myBytes.ToArray(), 0);
                    _package.myBytes.RemoveRange(0, 4);
                    ServerSend.SendPackageToClient(targetId, _package);
                    break;
                default:
                    Console.WriteLine("An unknow package has been received. -> diconnecting client " + _from);
                    GameServer.instance.DisconnectClient(_from);
                    break;
            }
        }
    }
}
