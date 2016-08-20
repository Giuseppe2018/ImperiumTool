using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;
using System.Windows.Forms;

namespace Imperium
{
    class NFunc
    {
        public static PS3API PS3 = new PS3API();
        public static int pid()
        {
            PS3.ConnectTarget();
            PS3.AttachProcess();
            return RPC.Call(Natives.PLAYER_ID);
        }
        public static int pedid()
        {
            PS3.ConnectTarget();
            PS3.AttachProcess();
            return RPC.Call(Natives.PLAYER_PED_ID);
        }
        public static int vehid()
        {
            PS3.ConnectTarget();
            PS3.AttachProcess();
            return RPC.Call(Natives.GET_VEHICLE_PED_IS_USING, pedid());
        }
        public static bool isInVehicle()
        {
            PS3.ConnectTarget();
            PS3.AttachProcess();
            return Convert.ToBoolean(RPC.Call(Natives.IS_PED_IN_ANY_VEHICLE, pedid()));
        }
        public static string psn()
        {
            PS3.ConnectTarget();
            PS3.AttachProcess();
            int name = RPC.Call(Natives.GET_PLAYER_NAME, pid());
            return PS3.Extension.ReadString((uint)name);
        }
        public static void save()
        {
            PS3.ConnectTarget();
            PS3.AttachProcess();
            RPC.Call(Natives.STAT_SAVE, 0, false, 3);
        }
    }
}
