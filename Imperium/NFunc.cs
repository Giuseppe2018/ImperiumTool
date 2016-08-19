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
            return RPC.Call(Natives.PLAYER_ID);
        }
        public static int pedid()
        {
            return RPC.Call(Natives.PLAYER_PED_ID);
        }
        public static int vehid()
        {
            return RPC.Call(Natives.GET_VEHICLE_PED_IS_USING, pedid());
        }
        public static bool isInVehicle() 
        {
            return Convert.ToBoolean(RPC.Call(Natives.IS_PED_IN_ANY_VEHICLE, pedid()));
        }
        public static string psn()
        {
            int name = RPC.Call(Natives.GET_PLAYER_NAME, pid());
            return PS3.Extension.ReadString((uint)name);
        }
        public static void save()
        {
            RPC.Call(Natives.STAT_SAVE, 0, false, 3);
        }
    }
}
