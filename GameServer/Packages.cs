using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Packages
    {
        public static Package PlayerJoinedPackage(int _playerid)
        {
            Package p = new Package();
            p.WriteInt(1); //Player joined type
            p.WriteInt(_playerid); //New players id
            return p;
        }
        public static Package PlayerWelcomePackage(string text)
        {
            Package p = new Package();
            p.WriteInt(0); //Player welcome type
            p.WriteString(text);
            return p;
        }
        public static Package PlayerDisconnectPackage(int _playerid)
        {
            Package p = new Package();
            p.WriteInt(2); //Player disconnect type
            p.WriteInt(_playerid); //Disconnected players id
            return p;
        }
    }
}
