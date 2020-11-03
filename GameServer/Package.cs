using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Package
    {
        public List<byte> myBytes = new List<byte>();

        public Package()
        { }

        public Package(byte[] _bytes)
        {
            foreach(byte b in _bytes)
            {
                myBytes.Add(b);
            }
        }
        public Package(byte[] _bytes, bool isSystem)
        {
            foreach (byte b in _bytes)
            {
                myBytes.Add(b);
            }
            isSystemPackage = isSystem;
        }

        public byte[] GetBytes()
        {
            return myBytes.ToArray();
        }

        public bool isSystemPackage = false;
    }
}
