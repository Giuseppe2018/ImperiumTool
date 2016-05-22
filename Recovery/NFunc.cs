using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;
using System.Windows.Forms;

namespace Recovery
{
    class NFunc
    {
        public static PS3API PS3 = new PS3API();
        public static int pid()
        {
            return RPC.Call(Natives.PLAYER_ID);
        }
        public static string psn()
        {
            int name = RPC.Call(Natives.GET_PLAYER_NAME, pid());
            return PS3.Extension.ReadString((uint)name);
        }
    }
}
