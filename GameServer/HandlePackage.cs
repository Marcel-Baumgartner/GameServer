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
            try
            {
                if (_package.GetAllBytes().Length < 5)
                {
                    Console.WriteLine("An unknow package has been received. -> diconnecting client " + _from);
                    GameServer.instance.DisconnectClient(_from);
                    return;
                }

                int id = BitConverter.ToInt32(_package.GetAllBytes(), 0);

                switch (id)
                {
                    case 0: //Broadcast
                        _package.myBytes.RemoveRange(0, 8);
                        ServerSend.SendBroadcast(_from, _package);
                        break;
                    case 1:
                        _package.ReadInt();
                        int target = _package.ReadInt();
                        _package.myBytes.RemoveRange(0, 8);
                        ServerSend.SendPackageToClient(_from, target, _package);
                        break;
                    default:
                        Console.WriteLine("An unknow package has been received. -> diconnecting client " + _from);
                        GameServer.instance.DisconnectClient(_from);
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error at handling package from " + _from + " -> disconnecting");
                Console.WriteLine("Error: " + ex.ToString());
                GameServer.instance.DisconnectClient(_from);
            }
        }
    }
}
