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
        public static void setStat(string stat, object value)
        {
            if (value is int || value is bool)
            {
                Natives address = value is int ? Natives.STAT_SET_INT : Natives.STAT_SET_BOOL;
                if (stat.Contains("MPPLY_"))
                {
                    RPC.Call(address, Main.Hash(stat), (int)value, 1);
                }
                else
                {
                    if (Variables.character1)
                    {
                        RPC.Call(address, Main.Hash("MP0_" + stat), (int)value, 1);
                    }
                    if (Variables.character2)
                    {
                        RPC.Call(address, Main.Hash("MP1_" + stat), (int)value, 1);
                    }
                }
            }
            else if (value is float || value is double)
            {
                if (stat.Contains("MPPLY_"))
                {
                    RPC.Call(Natives.STAT_SET_FLOAT, Main.Hash(stat), (float)value, 1);
                }
                else
                {
                    if (Variables.character1)
                    {
                        RPC.Call(Natives.STAT_SET_FLOAT, Main.Hash("MP0_" + stat), (float)value, 1);
                    }
                    if (Variables.character2)
                    {
                        RPC.Call(Natives.STAT_SET_FLOAT, Main.Hash("MP1_" + stat), (float)value, 1);
                    }
                }
            }
            else
            {
                MessageBox.Show("Unkown stat type");
            }
        }
    }
}
