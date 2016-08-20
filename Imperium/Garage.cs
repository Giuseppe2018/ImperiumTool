using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;
using System.Threading;

namespace Imperium
{
    class Garage
    {
        public static PS3API PS3 = new PS3API();
        public static uint pointer = 0x1E70390;
        public static uint Armor = 115,
        Body = 211,
        Brakes = 99,
        Bulletproof = 195,
        Bulletproof2 = 210,
        Bumper_Front = 55,
        Bumper_Rear = 59,
        Chassis = 71,
        CustomTire_Front = 155,
        CustomTire_Rear = 159,
        Engine = 95,
        Exhaust = 67,
        Grille = 75,
        Hood = 79,
        Horn = 107,
        Insurance = 287,
        Model = 176,
        Padding = 400,
        Paint_Pearl = 39,
        Paint_Primary = 31,
        Paint_Secondary = 35,
        Plate_Type = 11,
        Plate_Text = 12,
        Repair = 285,
        Rims_Color = 43,
        Rims_Front = 143,
        Rims_Rear = 147,
        Rims_Type = 191,
        Roof = 91,
        Skirts = 63,
        Smoke_B = 171,
        Smoke_Enabled = 131,
        Smoke_G = 167,
        Smoke_R = 163,
        Spolier = 51,
        Suspension = 111,
        Transmission = 103,
        Turbo = 123,
        Window = 175,
        Xenon = 139,
        RGB_Cache_R = 49 * 4,
        RGB_Cache_G = 50 * 4,
        RGB_Cache_B = 51 * 4,
        RGB = 52 * 4,
        RGB_Primary = 0x2000,
        RGB_Secondary = 0x1000;
        public static uint offset()
        {
            PS3.ConnectTarget();
            return PS3.Extension.ReadUInt32(pointer);
        }
        public static uint getUint(int slot, uint mod)
        {
            return PS3.Extension.ReadUInt32(Convert.ToUInt32(offset() + (slot * Padding) + mod));
        }
        public static void setUint(int slot, uint mod, uint value)
        {
            PS3.Extension.WriteUInt32(Convert.ToUInt32(offset() + (slot * Padding) + mod), value);
        }
        public static int getInt(int slot, uint mod)
        {
            return PS3.Extension.ReadInt32(Convert.ToUInt32(offset() + (slot * Padding) + mod));
        }
        public static void setByte(int slot, uint mod, byte value)
        {
            PS3.Extension.WriteByte(Convert.ToUInt32(offset() + (slot * Padding) + mod), value);
        }
        public static byte getByte(int slot, uint mod)
        {
            return PS3.Extension.ReadByte(Convert.ToUInt32(offset() + (slot * Padding) + mod));
        }
        public static void setInt(int slot, uint mod, int value)
        {
            PS3.Extension.WriteInt32(Convert.ToUInt32(offset() + (slot * Padding) + mod), value);
        }
        public static void setString(int slot, uint mod, string value)
        {
            PS3.Extension.WriteString(Convert.ToUInt32(offset() + (slot * Padding) + mod), value);
        }
        public static string getString(int slot, uint mod)
        {
            return PS3.Extension.ReadString(Convert.ToUInt32(offset() + (slot * Padding) + mod));
        }
        public static void resetSlot(int slot)
        {
            uint model = Garage.getUint(slot, Garage.Model);
            Garage.setUint(slot, Garage.Model, 0);
            Thread.Sleep(250);
            Garage.setUint(slot, Garage.Model, model);
        }
    }
}
