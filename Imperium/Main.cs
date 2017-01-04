using Newtonsoft.Json;
using PS3Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Web.Script.Serialization;

namespace Imperium
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        #region Classes, Structs, Dictionaries...
        public static PS3API PS3 = new PS3API();

        class RPC
        {
            // 1.27
            public static uint SFA1 = 0x1BE4C80;
            public static uint EFA1 = 0x1BE4D08;
            public static uint BFA1 = 0x18614;
            public static uint BAB1 = 0x18620;

            public static uint CBAB(uint F, uint T)
            {
                if (F > T)
                    return 0x4C000000 - (F - T);
                else if (F < T)
                    return T - F + 0x48000000;
                else
                    return 0x48000000;
            }

            public static void Enable()
            {
                byte[] mem = new byte[] {
            0xF8, 0x21, 0xFF, 0x91,
            0x7C, 0x08, 0x02, 0xA6,
            0xF8, 0x01, 0x00, 0x80,
            0x3C, 0x60, 0x10, 0x04,
            0x81, 0x83, 0x00, 0x4C,
            0x2C, 0x0C, 0x00, 0x00,
            0x41, 0x82, 0x00, 0x64,
            0x80, 0x83, 0x00, 0x04,
            0x80, 0xA3, 0x00, 0x08,
            0x80, 0xC3, 0x00, 0x0C,
            0x80, 0xE3, 0x00, 0x10,
            0x81, 0x03, 0x00, 0x14,
            0x81, 0x23, 0x00, 0x18,
            0x81, 0x43, 0x00, 0x1C,
            0x81, 0x63, 0x00, 0x20,
            0xC0, 0x23, 0x00, 0x24,
            0xc0, 0x43, 0x00, 0x28,
            0xC0, 0x63, 0x00, 0x2C,
            0xC0, 0x83, 0x00, 0x30,
            0xC0, 0xA3, 0x00, 0x34,
            0xc0, 0xC3, 0x00, 0x38,
            0xC0, 0xE3, 0x00, 0x3C,
            0xC1, 0x03, 0x00, 0x40,
            0xC1, 0x23, 0x00, 0x48,
            0x80, 0x63, 0x00, 0x00,
            0x7D, 0x89, 0x03, 0xA6,
            0x4E, 0x80, 0x04, 0x21,
            0x3C, 0x80, 0x10, 0x04,
            0x38, 0xA0, 0x00, 0x00,
            0x90, 0xA4, 0x00, 0x4C,
            0x90, 0x64, 0x00, 0x50,
            0xE8, 0x01, 0x00, 0x80,
            0x7C, 0x08, 0x03, 0xA6,
            0x38, 0x21, 0x00, 0x70 };

                PS3.SetMemory(SFA1, mem);
                PS3.Extension.WriteUInt32(EFA1, CBAB(EFA1, BAB1));
                PS3.Extension.WriteUInt32(BFA1, CBAB(BFA1, SFA1));
            }

            public static int Call(uint func_address, params object[] parameters)
            {
                uint address = func_address;
                int length = parameters.Length;
                int index = 0;
                uint num3 = 0;
                uint num4 = 0;
                uint num5 = 0;
                uint num6 = 0;
                while (index < length)
                {
                    if (parameters[index] is int)
                    {
                        PS3.Extension.WriteInt32(0x10040000 + (num3 * 4), (int)parameters[index]);
                        num3++;
                    }
                    else if (parameters[index] is uint)
                    {
                        PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), (uint)parameters[index]);
                        num3++;
                    }
                    else
                    {
                        uint num7;
                        if (parameters[index] is string)
                        {
                            num7 = 0x10042000 + (num4 * 0x400);
                            PS3.Extension.WriteString(num7, Convert.ToString(parameters[index]));
                            PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), num7);
                            num3++;
                            num4++;
                        }
                        else if (parameters[index] is float)
                        {
                            WriteSingle(0x10040024 + (num5 * 4), (float)parameters[index]);
                            num5++;
                        }
                        else if (parameters[index] is float[])
                        {
                            float[] input = (float[])parameters[index];
                            num7 = 0x10041000 + (num6 * 4);
                            WriteSingle(num7, input);
                            PS3.Extension.WriteUInt32(0x10040000 + (num3 * 4), num7);
                            num3++;
                            num6 += (uint)input.Length;
                        }
                    }
                    index++;
                }
                PS3.Extension.WriteUInt32(0x1004004C, address);
                while (PS3.Extension.ReadUInt32(0x1004004C) != 0) ;

                return PS3.Extension.ReadInt32(0x10040050);
            }

            private static void WriteSingle(uint address, float input)
            {
                byte[] Bytes = new byte[4];
                BitConverter.GetBytes(input).CopyTo((Array)Bytes, 0);
                Array.Reverse((Array)Bytes, 0, 4);
                PS3.SetMemory(address, Bytes);
            }

            private static void WriteSingle(uint address, float[] input)
            {
                int length = input.Length;
                byte[] Bytes = new byte[length * 4];
                for (int index = 0; index < length; ++index)
                    ReverseBytes(BitConverter.GetBytes(input[index])).CopyTo((Array)Bytes, index * 4);
                PS3.SetMemory(address, Bytes);
            }

            private static byte[] ReverseBytes(byte[] toReverse)
            {
                Array.Reverse((Array)toReverse);
                return toReverse;
            }
        }

        class Tunables
        {
            public static uint PTR_TUNABLES = 0x1E70374; // 1.26 // BLES
            public enum Indices : uint
            {
                // 1.26
                AMOUNT_TO_FORGIVE_BADSPORT_BY = 82,
                BADSPORT_RESET_MINUTES = 81,
                BAD_SPORT_QUITTING_EVENT_PLAYLIST = 4718,
                BAD_SPORT_QUITTING_PLAYLIST = 4717,
                BADSPORT_NUMDAYS_10TH_OFFENCE = 128,
                BADSPORT_NUMDAYS_1ST_OFFENCE = 119,
                BADSPORT_NUMDAYS_2ND_OFFENCE = 120,
                BADSPORT_NUMDAYS_3RD_OFFENCE = 121,
                BADSPORT_NUMDAYS_4TH_OFFENCE = 122,
                BADSPORT_NUMDAYS_5TH_OFFENCE = 123,
                BADSPORT_NUMDAYS_6TH_OFFENCE = 124,
                BADSPORT_NUMDAYS_7TH_OFFENCE = 125,
                BADSPORT_NUMDAYS_8TH_OFFENCE = 126,
                BADSPORT_NUMDAYS_9TH_OFFENCE = 127,
                BADSPORT_THRESHOLD = 79,
                BADSPORT_THRESHOLD_NOTCHEATER = 80,
                BADSPORTCHEAT_AUTOMUTE_PERCENT = 58,
                Car_impound_fee = 5978,
                DISABLE_CHRISTMAS_CLOTHING = 6884,
                DISABLE_CHRISTMAS_MASKS = 6885,
                DISABLE_CHRISTMAS_TREE_APARTMENT = 6883,
                DISABLE_CHRISTMAS_TREE_PERISHING_SQUARE = 6882,
                DISABLE_CHRISTMAS_TREE_PERISHING_SQUARE_SNOW = 6881,
                DISABLE_PV_DUPLICATE_FIX = 3649,
                DISABLE_SNOWBALLS = 6880,
                IDLEKICK_WARNING1 = 73,
                IDLEKICK_WARNING2 = 74,
                IDLEKICK_WARNING3 = 75,
                IDLEKICK_KICK = 76,
                INDEPENDENCE_DAY_DEACTIVATE_FIREWORKS_LAUNCHER = 6011,
                MAX_BET_LIMIT = 84,
                MAX_NUMBER_OF_SNOWBALLS = 6887,
                PICK_UP_NUMBER_OF_SNOWBALLS = 6888,
                PS_ELITAS_CHUTE_BAG = 6279,
                PS_EVENT_ITEM_HIGH_FLYER_CHUTE_BAG = 6281,
                PS_FLIGHT_SCHOOL_CHUTE_BAG = 6280,
                TOGGLE_ACTIVATE_INDEPENDENCE_PACK = 6003,
                Toggle_activate_Monster_truck = 6017,
                Toggle_activate_Western_sovereign = 6016,
                TOGGLE_CHRISTMAS_EVE_GIFT = 6935,
                TOGGLE_NEW_YEARS_DAY_GIFT = 6937,
                TOGGLE_NEW_YEARS_EVE_GIFT = 6936,
                TOGGLE_XMAS_CONTENT = 4724,
                TURN_ON_VALENTINES_EVENT = 4827,
                TURN_SNOW_ON_OFF = 4715,
                WEAPONS_INDEPENDENCEDAY_FIREWORKLAUNCHER_AMMO = 6031,
                INDEPENDENCE_DAY_DEACTIVATE_PLACED_FIREWORKS = 6012,
                SHOPPING_START = 59,
                SHOPPING_END = 69,
                KICK_VOTES_NEEDED_RATIO = 6,
                XP_MULTIPLIER = 1,
            };
            public static uint getTunableAddress(Indices index)
            {
                uint i = (uint)index;
                uint TunablesAddress = PS3.Extension.ReadUInt32(PTR_TUNABLES) + 4;
                if (TunablesAddress != 0)
                {
                    uint temp = TunablesAddress;
                    temp += (i * 4);
                    return temp;
                }
                return 0;
            }
            public static bool setTunable(Indices index, object value)
            {
                uint address = getTunableAddress(index);
                if (address != 0)
                {
                    if (value is int || value is uint || value is bool)
                    {
                        PS3.Extension.WriteInt32(address, Convert.ToInt32(value));
                    }
                    else if (value is float || value is long)
                    {
                        PS3.Extension.WriteFloat(address, (float)value);
                    }
                    return true;
                }
                return false;
            }
            public static int getTunableI(Indices index)
            {
                uint address = getTunableAddress(index);
                if (address != 0)
                {
                    return PS3.Extension.ReadInt32(address);
                }
                return 0;
            }
            public static float getTunableF(Indices index)
            {
                uint address = getTunableAddress(index);
                if (address != 0)
                {
                    return PS3.Extension.ReadFloat(address);
                }
                return 0;
            }
            public static bool getTunableB(Indices index)
            {
                uint address = getTunableAddress(index);
                if (address != 0)
                {
                    return PS3.Extension.ReadInt32(address) == 1;
                }
                return false;
            }
            public static void toggleTunable(Indices index, bool toggle)
            {
                setTunable(index, toggle);
            }
            public static void escapeBadsport()
            {
                setTunable(Indices.AMOUNT_TO_FORGIVE_BADSPORT_BY, Int32.MaxValue);
                setTunable(Indices.BADSPORT_RESET_MINUTES, 0);
                setTunable(Indices.BAD_SPORT_QUITTING_EVENT_PLAYLIST, 0);
                setTunable(Indices.BAD_SPORT_QUITTING_PLAYLIST, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_10TH_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_1ST_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_2ND_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_3RD_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_4TH_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_5TH_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_6TH_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_7TH_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_8TH_OFFENCE, 0);
                setTunable(Indices.BADSPORT_NUMDAYS_9TH_OFFENCE, 0);
                setTunable(Indices.BADSPORT_THRESHOLD, 0);
                setTunable(Indices.BADSPORT_THRESHOLD_NOTCHEATER, 0);
                setTunable(Indices.BADSPORTCHEAT_AUTOMUTE_PERCENT, 0);
            }
            public static bool disableIdleKick()
            {
                bool toggle = !(getTunableI(Indices.IDLEKICK_KICK) == 0x3B9ACA00);
                setTunable(Indices.IDLEKICK_WARNING1, toggle ? 0x3B9ACA00 : 120000);
                setTunable(Indices.IDLEKICK_WARNING2, toggle ? 0x3B9ACA00 : 300000);
                setTunable(Indices.IDLEKICK_WARNING3, toggle ? 0x3B9ACA00 : 600000);
                setTunable(Indices.IDLEKICK_KICK, toggle ? 0x3B9ACA00 : 900000);
                return toggle;
            }
            public static bool christmasWeather()
            {
                bool toggle = !getTunableB(Indices.TURN_SNOW_ON_OFF);
                toggleTunable(Indices.TURN_SNOW_ON_OFF, toggle);
                toggleTunable(Indices.DISABLE_CHRISTMAS_TREE_PERISHING_SQUARE_SNOW, !toggle);
                toggleTunable(Indices.DISABLE_CHRISTMAS_TREE_PERISHING_SQUARE, !toggle);
                toggleTunable(Indices.DISABLE_CHRISTMAS_TREE_APARTMENT, !toggle);
                toggleTunable(Indices.DISABLE_SNOWBALLS, !toggle);
                setTunable(Indices.MAX_NUMBER_OF_SNOWBALLS, toggle ? 255 : 0);
                setTunable(Indices.PICK_UP_NUMBER_OF_SNOWBALLS, toggle ? 255 : 0);
                return toggle;
            }
            public static bool christmasDLC()
            {
                bool toggle = !getTunableB(Indices.TOGGLE_XMAS_CONTENT);
                toggleTunable(Indices.TOGGLE_XMAS_CONTENT, toggle);
                toggleTunable(Indices.DISABLE_CHRISTMAS_CLOTHING, !toggle);
                toggleTunable(Indices.DISABLE_CHRISTMAS_MASKS, !toggle);
                toggleTunable(Indices.TOGGLE_CHRISTMAS_EVE_GIFT, toggle);
                return toggle;
            }
            public static bool valentinesDLC()
            {
                bool toggle = !getTunableB(Indices.TURN_ON_VALENTINES_EVENT);
                toggleTunable(Indices.TURN_ON_VALENTINES_EVENT, !getTunableB(Indices.TURN_ON_VALENTINES_EVENT));
                return toggle;
            }
            public static bool independenceDLC()
            {
                bool toggle = !getTunableB(Indices.TOGGLE_ACTIVATE_INDEPENDENCE_PACK);
                toggleTunable(Indices.TOGGLE_ACTIVATE_INDEPENDENCE_PACK, toggle);
                toggleTunable(Indices.INDEPENDENCE_DAY_DEACTIVATE_FIREWORKS_LAUNCHER, !toggle);
                toggleTunable(Indices.Toggle_activate_Western_sovereign, toggle);
                toggleTunable(Indices.Toggle_activate_Monster_truck, toggle);
                toggleTunable(Indices.INDEPENDENCE_DAY_DEACTIVATE_PLACED_FIREWORKS, !toggle);
                return toggle;
            }
            public static bool idleKick()
            {
                bool toggle = getTunableI(Indices.IDLEKICK_KICK) != Int32.MaxValue;
                setTunable(Indices.IDLEKICK_KICK, toggle ? Int32.MaxValue : 900000);
                setTunable(Indices.IDLEKICK_WARNING3, toggle ? Int32.MaxValue : 600000);
                setTunable(Indices.IDLEKICK_WARNING2, toggle ? Int32.MaxValue : 300000);
                setTunable(Indices.IDLEKICK_WARNING1, toggle ? Int32.MaxValue : 120000);
                return toggle;
            }
            public static void freeShopping(bool toggle)
            {
                for (Indices i = Indices.SHOPPING_START; i < Indices.SHOPPING_END; i++)
                {
                    setTunable(i, toggle ? 0 : 0x3F800000);
                }
            }

        }

        public struct OutfitStruct
        {
            public int mask, maskT;
            public int torso, torsoT;
            public int legs, legsT;
            public int hands, handsT;
            public int shoes, shoesT;
            public int extra, extraT;
            public int tops1, tops1T;
            public int armor, armorT;
            public int emblem, emblemT;
            public int tops2, tops2T;
            public int hat, hatT;
            public int eyes, eyesT;
            public int ears, earsT;
        }
        class Outfit
        {
            public static uint pointer = 0x02223918;
            public static uint len_struct = 0x34;
            public static uint len_struct_a = 0x28;
            public static uint len_name = 32;
            public static uint ptr_struct = 0x710;
            public static uint ptr_textures = 0x500;
            public static uint ptr_struct_a = 0x2F8;
            public static uint ptr_textures_a = 0x164;
            public static uint ptr_name = 0x108;
            public static string Name(int index = 0, string name = null)
            {
                if (index >= 0 && index <= 9)
                {
                    uint address = PS3.Extension.ReadUInt32(pointer);
                    uint offset = (uint)((int)address + (int)ptr_name + index * len_name);
                    if (name != null)
                        PS3.Extension.WriteString(offset, name);
                    return PS3.Extension.ReadString(offset);
                }
                return "";
            }
            public static OutfitStruct Fetch(int index = 0)
            {
                uint address = PS3.Extension.ReadUInt32(pointer);
                uint outfit_struct = (address - ptr_struct) + ((uint)index * len_struct) + 4;
                uint outfit_textures = (address - ptr_textures) + ((uint)index * len_struct);
                uint accessory_struct = (address - ptr_struct_a) + ((uint)index * len_struct_a);
                uint accessory_textures = (address - ptr_textures_a) + ((uint)index * len_struct_a);
                OutfitStruct outfit;

                outfit.mask = PS3.Extension.ReadInt32(outfit_struct);
                outfit.torso = PS3.Extension.ReadInt32(outfit_struct + 0x08);
                outfit.legs = PS3.Extension.ReadInt32(outfit_struct + 0x0C);
                outfit.hands = PS3.Extension.ReadInt32(outfit_struct + 0x10);
                outfit.shoes = PS3.Extension.ReadInt32(outfit_struct + 0x14);
                outfit.extra = PS3.Extension.ReadInt32(outfit_struct + 0x18);
                outfit.tops1 = PS3.Extension.ReadInt32(outfit_struct + 0x1C);
                outfit.armor = PS3.Extension.ReadInt32(outfit_struct + 0x20);
                outfit.emblem = PS3.Extension.ReadInt32(outfit_struct + 0x24);
                outfit.tops2 = PS3.Extension.ReadInt32(outfit_struct + 0x28);

                outfit.maskT = PS3.Extension.ReadInt32(outfit_textures);
                outfit.torsoT = PS3.Extension.ReadInt32(outfit_textures + 0x08);
                outfit.legsT = PS3.Extension.ReadInt32(outfit_textures + 0x0C);
                outfit.handsT = PS3.Extension.ReadInt32(outfit_textures + 0x10);
                outfit.shoesT = PS3.Extension.ReadInt32(outfit_textures + 0x14);
                outfit.extraT = PS3.Extension.ReadInt32(outfit_textures + 0x18);
                outfit.tops1T = PS3.Extension.ReadInt32(outfit_textures + 0x1C);
                outfit.armorT = PS3.Extension.ReadInt32(outfit_textures + 0x20);
                outfit.emblemT = PS3.Extension.ReadInt32(outfit_textures + 0x24);
                outfit.tops2T = PS3.Extension.ReadInt32(outfit_textures + 0x28);

                outfit.hat = PS3.Extension.ReadInt32(accessory_struct);
                outfit.eyes = PS3.Extension.ReadInt32(accessory_struct + 0x04);
                outfit.ears = PS3.Extension.ReadInt32(accessory_struct + 0x08);

                outfit.hatT = PS3.Extension.ReadInt32(accessory_textures);
                outfit.eyesT = PS3.Extension.ReadInt32(accessory_textures + 0x04);
                outfit.earsT = PS3.Extension.ReadInt32(accessory_textures + 0x08);

                return outfit;
            }
            public static void Apply(int index, OutfitStruct outfit)
            {
                uint address = PS3.Extension.ReadUInt32(pointer);
                uint outfit_struct = (address - ptr_struct) + ((uint)index * len_struct) + 4;
                uint outfit_textures = (address - ptr_textures) + ((uint)index * len_struct);
                uint accessory_struct = (address - ptr_struct_a) + ((uint)index * len_struct_a);
                uint accessory_textures = (address - ptr_textures_a) + ((uint)index * len_struct_a);

                PS3.Extension.WriteInt32(outfit_struct, outfit.mask);
                PS3.Extension.WriteInt32(outfit_struct + 0x08, outfit.torso);
                PS3.Extension.WriteInt32(outfit_struct + 0x0C, outfit.legs);
                PS3.Extension.WriteInt32(outfit_struct + 0x10, outfit.hands);
                PS3.Extension.WriteInt32(outfit_struct + 0x14, outfit.shoes);
                PS3.Extension.WriteInt32(outfit_struct + 0x18, outfit.extra);
                PS3.Extension.WriteInt32(outfit_struct + 0x1C, outfit.tops1);
                PS3.Extension.WriteInt32(outfit_struct + 0x20, outfit.armor);
                PS3.Extension.WriteInt32(outfit_struct + 0x24, outfit.emblem);
                PS3.Extension.WriteInt32(outfit_struct + 0x28, outfit.tops2);

                PS3.Extension.WriteInt32(outfit_textures, outfit.maskT);
                PS3.Extension.WriteInt32(outfit_textures + 0x08, outfit.torsoT);
                PS3.Extension.WriteInt32(outfit_textures + 0x0C, outfit.legsT);
                PS3.Extension.WriteInt32(outfit_textures + 0x10, outfit.handsT);
                PS3.Extension.WriteInt32(outfit_textures + 0x14, outfit.shoesT);
                PS3.Extension.WriteInt32(outfit_textures + 0x18, outfit.extraT);
                PS3.Extension.WriteInt32(outfit_textures + 0x1C, outfit.tops1T);
                PS3.Extension.WriteInt32(outfit_textures + 0x20, outfit.armorT);
                PS3.Extension.WriteInt32(outfit_textures + 0x24, outfit.emblemT);
                PS3.Extension.WriteInt32(outfit_textures + 0x28, outfit.tops2T);

                PS3.Extension.WriteInt32(accessory_struct, outfit.hat);
                PS3.Extension.WriteInt32(accessory_struct + 0x04, outfit.eyes);
                PS3.Extension.WriteInt32(accessory_struct + 0x08, outfit.ears);

                PS3.Extension.WriteInt32(accessory_textures, outfit.hatT);
                PS3.Extension.WriteInt32(accessory_textures + 0x04, outfit.eyesT);
                PS3.Extension.WriteInt32(accessory_textures + 0x08, outfit.earsT);
            }
        }

        class NativeFunctions
        {
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
        #region Stats
        public class StatData
        {
            public object value { get; set; }
            public string keyword { get; set; }
            public StatData(object _value, string _keyword)
            {
                value = _value;
                keyword = _keyword;
            }
        }
        public static Dictionary<string, StatData> Stats = new Dictionary<string, StatData>()
        {
            { "ADMIN_CLOTHES_GV_BS_1", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_10", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_11", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_12", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_2", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_3", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_4", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_5", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_6", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_7", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_8", new StatData(-1, "clothingDLC") },
            { "ADMIN_CLOTHES_GV_BS_9", new StatData(-1, "clothingDLC") },
            { "ADMIN_WEAPON_GV_BS_1", new StatData(-1, "weapons") },
            { "AIR_LAUNCHES_OVER_40M", new StatData(25, "awards") },
            { "AWD_5STAR_WANTED_AVOIDANCE", new StatData(50, "awards") },
            { "AWD_ACTIVATE_2_PERSON_KEY", new StatData(true, "awards") },
            { "AWD_ALL_ROLES_HEIST", new StatData(true, "awards") },
            { "AWD_BUY_EVERY_GUN", new StatData(true, "awards") },
            { "AWD_CAR_BOMBS_ENEMY_KILLS", new StatData(25, "awards") },
            { "AWD_CARS_EXPORTED", new StatData(50, "awards") },
            { "AWD_CONTROL_CROWDS", new StatData(25, "awards") },
            { "AWD_DAILYOBJCOMPLETED", new StatData(100, "awards") },
            { "AWD_DAILYOBJMONTHBONUS", new StatData(true, "awards") },
            { "AWD_DAILYOBJWEEKBONUS", new StatData(true, "awards") },
            { "AWD_DO_HEIST_AS_MEMBER", new StatData(25, "clothingExclusive") },
            { "AWD_DO_HEIST_AS_THE_LEADER", new StatData(25, "clothingExclusive") },
            { "AWD_DRIVELESTERCAR5MINS", new StatData(true, "awards") },
            { "AWD_DROPOFF_CAP_PACKAGES", new StatData(100, "chromeRims") },
            { "AWD_FINISH_HEIST_NO_DAMAGE", new StatData(true, "clothingExclusive") },
            { "AWD_FINISH_HEIST_SETUP_JOB", new StatData(50, "chromeRims") },
            { "AWD_FINISH_HEISTS", new StatData(50, "chromeRims") },
            { "AWD_FM25DIFFERENTDM", new StatData(true, "awards") },
            { "AWD_FM25DIFFERENTRACES", new StatData(true, "awards") },
            { "AWD_FM25DIFITEMSCLOTHES", new StatData(true, "awards") },
            { "AWD_FM6DARTCHKOUT", new StatData(true, "awards") },
            { "AWD_FM_DM_3KILLSAMEGUY", new StatData(50, "awards") },
            { "AWD_FM_DM_KILLSTREAK", new StatData(100, "awards") },
            { "AWD_FM_DM_STOLENKILL", new StatData(50, "awards") },
            { "AWD_FM_DM_TOTALKILLS", new StatData(500, "tattoos") },
            { "AWD_FM_DM_WINS", new StatData(50, "awards") },
            { "AWD_FM_GOLF_BIRDIES", new StatData(25, "awards") },
            { "AWD_FM_GOLF_HOLE_IN_1", new StatData(true, "awards") },
            { "AWD_FM_GOLF_WON", new StatData(25, "awards") },
            { "AWD_FM_GTA_RACES_WON", new StatData(50, "awards") },
            { "AWD_FM_RACE_LAST_FIRST", new StatData(25, "awards") },
            { "AWD_FM_RACES_FASTEST_LAP", new StatData(50, "vehicleMods") },
            { "AWD_FM_SHOOTRANG_CT_WON", new StatData(25, "awards") },
            { "AWD_FM_SHOOTRANG_GRAN_WON", new StatData(true, "awards") },
            { "AWD_FM_SHOOTRANG_RT_WON", new StatData(25, "awards") },
            { "AWD_FM_SHOOTRANG_TG_WON", new StatData(25, "awards") },
            { "AWD_FM_TDM_MVP", new StatData(50, "awards") },
            { "AWD_FM_TDM_WINS", new StatData(50, "awards") },
            { "AWD_FM_TENNIS_5_SET_WINS", new StatData(true, "awards") },
            { "AWD_FM_TENNIS_ACE", new StatData(25, "awards") },
            { "AWD_FM_TENNIS_STASETWIN", new StatData(true, "awards") },
            { "AWD_FM_TENNIS_WON", new StatData(25, "awards") },
            { "AWD_FMATTGANGHQ", new StatData(true, "awards") },
            { "AWD_FMBASEJMP", new StatData(25, "awards") },
            { "AWD_FMBBETWIN", new StatData(50000, "awards") },
            { "AWD_FMCRATEDROPS", new StatData(25, "awards") },
            { "AWD_FMDRIVEWITHOUTCRASH", new StatData(30, "awards") },
            { "AWD_FMFULLYMODDEDCAR", new StatData(true, "awards") },
            { "AWD_FMHORDWAVESSURVIVE", new StatData(10, "clothingExclusive") },
            { "AWD_FMKILL3ANDWINGTARACE", new StatData(true, "awards") },
            { "AWD_FMKILLBOUNTY", new StatData(25, "awards") },
            { "AWD_FMKILLSTREAKSDM", new StatData(true, "awards") },
            { "AWD_FMMOSTKILLSGANGHIDE", new StatData(true, "awards") },
            { "AWD_FMMOSTKILLSSURVIVE", new StatData(true, "awards") },
            { "AWD_FMPICKUPDLCCRATE1ST", new StatData(true, "awards") },
            { "AWD_FMRACEWORLDRECHOLDER", new StatData(true, "awards") },
            { "AWD_FMRALLYWONDRIVE", new StatData(25, "awards") },
            { "AWD_FMRALLYWONNAV", new StatData(25, "awards") },
            { "AWD_FMREVENGEKILLSDM", new StatData(50, "tattoos") },
            { "AWD_FMSHOOTDOWNCOPHELI", new StatData(25, "awards") },
            { "AWD_FMTATTOOALLBODYPARTS", new StatData(true, "awards") },
            { "AWD_FMWINAIRRACE", new StatData(25, "awards") },
            { "AWD_FMWINALLRACEMODES", new StatData(true, "awards") },
            { "AWD_FMWINCUSTOMRACE", new StatData(true, "awards") },
            { "AWD_FMWINEVERYGAMEMODE", new StatData(true, "awards") },
            { "AWD_FMWINRACETOPOINTS", new StatData(25, "awards") },
            { "AWD_FMWINSEARACE", new StatData(25, "awards") },
            { "AWD_HOLD_UP_SHOPS", new StatData(20, "awards") },
            { "AWD_KILL_CARRIER_CAPTURE", new StatData(100, "chromeRims") },
            { "AWD_KILL_PSYCHOPATHS", new StatData(100, "clothingExclusive") },
            { "AWD_KILL_TEAM_YOURSELF_LTS", new StatData(25, "clothingExclusive") },
            { "AWD_LAPDANCES", new StatData(25, "awards") },
            { "AWD_LESTERDELIVERVEHICLES", new StatData(25, "awards") },
            { "AWD_MENTALSTATE_TO_NORMAL", new StatData(50, "awards") },
            { "AWD_NIGHTVISION_KILLS", new StatData(100, "chromeRims") },
            { "AWD_NO_HAIRCUTS", new StatData(25, "awards") },
            { "AWD_ODISTRACTCOPSNOEATH", new StatData(25, "awards") },
            { "AWD_ONLY_PLAYER_ALIVE_LTS", new StatData(50, "chromeRims") },
            { "AWD_PARACHUTE_JUMPS_20M", new StatData(25, "awards") },
            { "AWD_PARACHUTE_JUMPS_50M", new StatData(25, "awards") },
            { "AWD_PASSENGERTIME", new StatData(4, "awards") },
            { "AWD_PICKUP_CAP_PACKAGES", new StatData(100, "clothingExclusive") },
            { "AWD_RACES_WON", new StatData(50, "awards") },
            { "AWD_SECURITY_CARS_ROBBED", new StatData(25, "awards") },
            { "AWD_SPLIT_HEIST_TAKE_EVENLY", new StatData(true, "awards") },
            { "AWD_STORE_20_CAR_IN_GARAGES", new StatData(true, "clothingExclusive") },
            { "AWD_TAKEDOWNSMUGPLANE", new StatData(50, "awards") },
            { "AWD_TIME_IN_HELICOPTER", new StatData(4, "awards") },
            { "AWD_TRADE_IN_YOUR_PROPERTY", new StatData(25, "awards") },
            { "AWD_VEHICLES_JACKEDR", new StatData(500, "awards") },
            { "AWD_WIN_AT_DARTS", new StatData(25, "awards") },
            { "AWD_WIN_CAPTURE_DONT_DYING", new StatData(25, "clothingExclusive") },
            { "AWD_WIN_CAPTURES", new StatData(50, "chromeRims") },
            { "AWD_WIN_GOLD_MEDAL_HEISTS", new StatData(25, "clothingExclusive") },
            { "AWD_WIN_LAST_TEAM_STANDINGS", new StatData(50, "chromeRims") },
            { "BOTTLE_IN_POSSESSION", new StatData(-1, "weapons") },
            { "CARS_EXPLODED", new StatData(500, "awards") },
            { "CHAR_FM_CARMOD_1_UNLCK", new StatData(-1, "vehicleMods") },
            { "CHAR_FM_CARMOD_2_UNLCK", new StatData(-1, "vehicleMods") },
            { "CHAR_FM_CARMOD_3_UNLCK", new StatData(-1, "vehicleMods") },
            { "CHAR_FM_CARMOD_4_UNLCK", new StatData(-1, "vehicleMods") },
            { "CHAR_FM_CARMOD_5_UNLCK", new StatData(-1, "vehicleMods") },
            { "CHAR_FM_CARMOD_6_UNLCK", new StatData(-1, "vehicleMods") },
            { "CHAR_FM_CARMOD_7_UNLCK", new StatData(-1, "vehicleMods") },
            { "CHAR_FM_VEHICLE_1_UNLCK", new StatData(-1, "heistVehicles") },
            { "CHAR_FM_VEHICLE_2_UNLCK", new StatData(-1, "heistVehicles") },
            { "CHAR_FM_WEAP_ADDON_1_UNLCK", new StatData(-1, "weapons") },
            { "CHAR_FM_WEAP_ADDON_2_UNLCK", new StatData(-1, "weapons") },
            { "CHAR_FM_WEAP_ADDON_3_UNLCK", new StatData(-1, "weapons") },
            { "CHAR_FM_WEAP_ADDON_4_UNLCK", new StatData(-1, "weapons") },
            { "CHAR_FM_WEAP_ADDON_5_UNLCK", new StatData(-1, "weapons") },
            { "CHAR_FM_WEAP_UNLOCKED", new StatData(-1, "weapons") },
            { "CHAR_FM_WEAP_UNLOCKED2", new StatData(-1, "weapons") },
            { "CHAR_KIT_10_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_11_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_12_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_1_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_2_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_3_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_4_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_5_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_6_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_7_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_8_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_9_FM_UNLCK", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE10", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE11", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE12", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE2", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE3", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE4", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE5", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE6", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE7", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE8", new StatData(-1, "kits") },
            { "CHAR_KIT_FM_PURCHASE9", new StatData(-1, "kits") },
            { "CHAR_WANTED_LEVEL_TIME5STAR", new StatData(-1, "awards") },
            { "CHAR_WEAP_FM_PURCHASE", new StatData(-1, "weapons") },
            { "CHAR_WEAP_FM_PURCHASE2", new StatData(-1, "weapons") },
            { "CLTHS_ACQUIRED_BERD", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_BERD_1", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_BERD_2", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_BERD_3", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_BERD_4", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_BERD_5", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_BERD_6", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_BERD_7", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_DECL", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_FEET", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_FEET_1", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_FEET_2", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_FEET_3", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_FEET_4", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_FEET_5", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_FEET_6", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_FEET_7", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_JBIB", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_JBIB_1", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_JBIB_2", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_JBIB_3", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_JBIB_4", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_JBIB_5", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_JBIB_6", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_JBIB_7", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_LEGS", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_LEGS_1", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_LEGS_2", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_LEGS_3", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_LEGS_4", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_LEGS_5", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_LEGS_6", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_LEGS_7", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_OUTFIT", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_1", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_10", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_2", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_3", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_4", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_5", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_6", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_7", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_8", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_PROPS_9", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL2", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL2_1", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL_1", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL_2", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL_3", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL_4", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL_5", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL_6", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_SPECIAL_7", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_TEETH", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_TEETH_1", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_TEETH_2", new StatData(-1, "clothing") },
            { "CLTHS_ACQUIRED_TORSO", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_BERD", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_BERD_1", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_BERD_2", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_BERD_3", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_BERD_4", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_BERD_5", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_BERD_6", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_BERD_7", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_DECL", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_FEET", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_FEET_1", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_FEET_2", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_FEET_3", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_FEET_4", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_FEET_5", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_FEET_6", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_FEET_7", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_HAIR", new StatData(-1, "clothingHair") },
            { "CLTHS_AVAILABLE_HAIR_1", new StatData(-1, "clothingHair") },
            { "CLTHS_AVAILABLE_HAIR_2", new StatData(-1, "clothingHair") },
            { "CLTHS_AVAILABLE_HAIR_3", new StatData(-1, "clothingHair") },
            { "CLTHS_AVAILABLE_HAIR_4", new StatData(-1, "clothingHair") },
            { "CLTHS_AVAILABLE_HAIR_5", new StatData(-1, "clothingHair") },
            { "CLTHS_AVAILABLE_HAIR_6", new StatData(-1, "clothingHair") },
            { "CLTHS_AVAILABLE_HAIR_7", new StatData(-1, "clothingHair") },
            { "CLTHS_AVAILABLE_JBIB", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_JBIB_1", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_JBIB_2", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_JBIB_3", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_JBIB_4", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_JBIB_5", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_JBIB_6", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_JBIB_7", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_LEGS", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_LEGS_1", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_LEGS_2", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_LEGS_3", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_LEGS_4", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_LEGS_5", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_LEGS_6", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_LEGS_7", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_OUTFIT", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_1", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_10", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_2", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_3", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_4", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_5", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_6", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_7", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_8", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_PROPS_9", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL2", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL2_1", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL_1", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL_2", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL_3", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL_4", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL_5", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL_6", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_SPECIAL_7", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_TEETH", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_TEETH_1", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_TEETH_2", new StatData(-1, "clothing") },
            { "CLTHS_AVAILABLE_TORSO", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_0", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_1", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_10", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_11", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_12", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_13", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_14", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_15", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_16", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_17", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_18", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_19", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_2", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_20", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_21", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_22", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_23", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_24", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_25", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_26", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_27", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_28", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_29", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_3", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_30", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_31", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_32", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_33", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_34", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_35", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_36", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_37", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_38", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_39", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_4", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_40", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_5", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_6", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_7", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_8", new StatData(-1, "clothing") },
            { "DLC_APPAREL_ACQUIRED_9", new StatData(-1, "clothing") },
            { "FM_CHANGECHAR_ASKED", new StatData(false, "redesign") },
            { "GRENADE_ENEMY_KILLS", new StatData(50, "awards") },
            { "KILLS_PLAYERS", new StatData(1000, "awards") },
            { "LONGEST_WHEELIE_DIST", new StatData(1000f, "awards") },
            { "MOST_ARM_WRESTLING_WINS", new StatData(25, "awards") },
            { "MOST_FLIPS_IN_ONE_JUMP", new StatData(5, "awards") },
            { "MOST_SPINS_IN_ONE_JUMP", new StatData(5, "awards") },
            { "MPPLY_AWD_COMPLET_HEIST_MEM", new StatData(true, "awards") },
            { "MPPLY_AWD_FLEECA_FIN", new StatData(true, "awards") },
            { "MPPLY_AWD_FM_CR_DM_MADE", new StatData(25, "awards") },
            { "MPPLY_AWD_FM_CR_PLAYED_BY_PEEP", new StatData(100, "awards") },
            { "MPPLY_AWD_FM_CR_RACES_MADE", new StatData(25, "awards") },
            { "MPPLY_AWD_HST_ORDER", new StatData(true, "awards") },
            { "MPPLY_AWD_HST_SAME_TEAM", new StatData(true, "awards") },
            { "MPPLY_AWD_HST_ULT_CHAL", new StatData(true, "awards") },
            { "MPPLY_AWD_HUMANE_FIN", new StatData(true, "awards") },
            { "MPPLY_AWD_PACIFIC_FIN", new StatData(true, "awards") },
            { "MPPLY_AWD_PRISON_FIN", new StatData(true, "awards") },
            { "MPPLY_AWD_SERIESA_FIN", new StatData(true, "awards") },
            { "MPPLY_NO_MORE_TUTORIALS", new StatData(true, "tutorial") },
            { "NUMBER_SLIPSTREAMS_IN_RACE", new StatData(100, "vehicleMods") },
            { "NUMBER_TURBO_STARTS_IN_RACE", new StatData(50, "vehicleMods") },
            { "PASS_DB_PLAYER_KILLS", new StatData(100, "awards") },
            { "PISTOL_ENEMY_KILLS", new StatData(500, "tattoos") },
            { "PLAYER_HEADSHOTS", new StatData(500, "tattoos") },
            { "RACES_WON", new StatData(50, "vehicleMods") },
            { "SAWNOFF_ENEMY_KILLS", new StatData(500, "awards") },
            { "SCRIPT_INCREASE_DRIV", new StatData(100, "skills") },
            { "SCRIPT_INCREASE_FLY", new StatData(100, "skills") },
            { "SCRIPT_INCREASE_LUNG", new StatData(100, "skills") },
            { "SCRIPT_INCREASE_MECH", new StatData(100, "skills") },
            { "SCRIPT_INCREASE_SHO", new StatData(100, "skills") },
            { "SCRIPT_INCREASE_STAM", new StatData(100, "skills") },
            { "SCRIPT_INCREASE_STL", new StatData(100, "skills") },
            { "SCRIPT_INCREASE_STRN", new StatData(100, "skills") },
            { "STKYBMB_ENEMY_KILLS", new StatData(50, "awards") },
            { "UNARMED_ENEMY_KILLS", new StatData(50, "awards") },
            { "USJS_COMPLETED", new StatData(50, "vehicleMods") },
            { "WEAP_FM_ADDON_PURCH", new StatData(-1, "weapons") },
            { "WEAP_FM_ADDON_PURCH2", new StatData(-1, "weapons") },
            { "WEAP_FM_ADDON_PURCH3", new StatData(-1, "weapons") },
            { "WEAP_FM_ADDON_PURCH4", new StatData(-1, "weapons") },
            { "WEAP_FM_ADDON_PURCH5", new StatData(-1, "weapons") },
            { "FM_ACT_PHN", new StatData(-1, "contacts") },
            { "FM_ACT_PH2", new StatData(-1, "contacts") },
            { "FM_ACT_PH3", new StatData(-1, "contacts") },
            { "FM_ACT_PH4", new StatData(-1, "contacts") },
            { "FM_ACT_PH5", new StatData(-1, "contacts") },
            { "FM_ACT_TX1", new StatData(-1, "contacts") },
            { "FLAREGUN_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "GRNLAUNCH_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "RPG_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "MINIGUNS_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "GRENADE_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "SMKGRENADE_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "STKYBMB_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "MOLOTOV_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "PETROLCAN_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "PRXMINE_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "HOMLNCH_FM_AMMO_CURRENT", new StatData(-1, "infiniteAmmo") },
            { "MPPLY_HEIST_ACH_TRACKER", new StatData(-1, "heistTrophies") },

        };
        #endregion

        #region Teleport
        public class TpData
        {
            public string label { get; set; }
            public string grouping { get; set; }
            public TpData(string _label, string _grouping)
            {
                label = _label;
                grouping = _grouping;
            }
        }
        public static Dictionary<double[], TpData> Locations = new Dictionary<double[], TpData>()
		{
			{ new double[] { 1705.173, 3747.373, 33.922 }, new TpData("Sandy Shores", "Ammunation") },
			{ new double[] { 234.312, -42.553, 69.676 }, new TpData("Hawick", "Ammunation") },
			{ new double[] { 843.569, -1018.228, 27.561 }, new TpData("La Mesa", "Ammunation") },
			{ new double[] { -320.263, 6071.031, 31.337 }, new TpData("Paleto Bay", "Ammunation") },
			{ new double[] { -663.388, -950.879, 21.399 }, new TpData("Little Seoul", "Ammunation") },
			{ new double[] { -1324.082, -388.411, 36.545 }, new TpData("Morningwood", "Ammunation") },
			{ new double[] { -1108.773, 2685.568, 18.875 }, new TpData("Great Chaparral", "Ammunation") },
			{ new double[] { -3158.208, 1078.877, 20.691 }, new TpData("Chumash", "Ammunation") },
			{ new double[] { 2568.549, 313.032, 108.461 }, new TpData("Tataviam", "Ammunation") },
			{ new double[] { 15.288, -1122.648, 28.816 }, new TpData("Pillbox Hill", "Ammunation") },
			{ new double[] { 812.893, -2139.652, 29.292 }, new TpData("Cypress Flats", "Ammunation") },
			
			{ new double[] { -827.346, -190.451, 37.604 }, new TpData("Rockford Hills", "Barber Shop") },
			{ new double[] { 130.348, -1715.111, 29.234 }, new TpData("Davis", "Barber Shop") },
			{ new double[] { -1295.014, -1116.923, 6.655 }, new TpData("Vespucci", "Barber Shop") },
			{ new double[] { 1936.458, 3720.811, 32.672 }, new TpData("Sandy Shores", "Barber Shop") },
			{ new double[] { 1202.648, -470.297, 66.246 }, new TpData("Mirror Park", "Barber Shop") },
			{ new double[] { -30.615, -142.411, 57.051 }, new TpData("Hawick", "Barber Shop") },
			{ new double[] { -284.387, 6236.210, 31.460 }, new TpData("Paleto Bay", "Barber Shop") },
			
			{ new double[] { 411.758, -808.082, 29.144 }, new TpData("Textile City", "Binco") },
			{ new double[] { -814.987, -1083.856, 11.012 }, new TpData("Vespucci Canals", "Binco") },
			
			{ new double[] { 89.663, -1390.938, 29.249 }, new TpData("Strawberry", "Discount") },
			{ new double[] { 1678.675, 4821.034, 42.007 }, new TpData("Grapeseed", "Discount") },
			{ new double[] { -1091.677, 2700.770, 19.625 }, new TpData("Great Chaparral", "Discount") },
			{ new double[] { 1200.602, 2696.959, 37.927 }, new TpData("Senora Desert", "Discount") },
			{ new double[] { -5.078, 6521.567, 31.270 }, new TpData("Paleto Bay", "Discount") },
			
			{ new double[] { -365.425, -131.809, 37.873 }, new TpData("Burton", "LS Customs") },
			{ new double[] { -1134.224, -1984.387, 13.166 }, new TpData("LS Airport", "LS Customs") },
			{ new double[] { 709.797, -1082.649, 22.398 }, new TpData("La Mesa", "LS Customs") },
			{ new double[] { 1178.653, 2666.179, 37.881 }, new TpData("Senora Desert", "LS Customs") },
			
			{ new double[] { -718.441, -162.860, 37.013 }, new TpData("Rockford Hills", "Ponsonbys") },
			{ new double[] { -150.952, -304.549, 38.925 }, new TpData("Burton", "Ponsonbys") },
			{ new double[] { -1461.290, -226.524, 49.249 }, new TpData("Morningwood", "Ponsonbys") },
			
			{ new double[] { -1209.446, -783.510, 17.169 }, new TpData("Del Perro", "Suburban") },
			{ new double[] { 617.782, 2736.849, 41.999 }, new TpData("Harmony", "Suburban") },
			{ new double[] { 130.452, -202.726, 54.505 }, new TpData("Harwick", "Suburban") },
			{ new double[] { -3165.330, 1062.592, 20.840 }, new TpData("Chumash", "Suburban") },
			
			{ new double[] { 318.079, 170.586, 103.767 }, new TpData("Downtown", "Tattoo") },
			{ new double[] { 1853.978, 3745.352, 33.082 }, new TpData("Sandy Shores", "Tattoo") },
			{ new double[] { -286.602, 6202.248, 31.322 }, new TpData("Paleto Bay", "Tattoo") },
			{ new double[] { -1159.103, -1417.739, 4.706 }, new TpData("Vespucci Canals", "Tattoo") },
			{ new double[] { 1319.359, -1643.693, 52.145 }, new TpData("El Burro Heights", "Tattoo") },
			{ new double[] { -3162.709, 1071.733, 20.681 }, new TpData("Chumash", "Tattoo") },
			
			{ new double[] { 126.219, 6608.142, 31.866 }, new TpData("Beekers Garage", "Miscellaneous") },
			{ new double[] { -1339.926, -1279.063, 4.870 }, new TpData("Vespucci Movie Masks", "Miscellaneous") },
		};
        void teleport(double[] loc)
        {
            float[] nLoc = { (float)loc[0], (float)loc[1], (float)loc[2] };
            uint location = 0x10030000;
            byte[] XLocation = BitConverter.GetBytes(nLoc[0]);
            byte[] YLocation = BitConverter.GetBytes(nLoc[1]);
            byte[] ZLocation = BitConverter.GetBytes(nLoc[2]);
            Array.Reverse(XLocation);
            Array.Reverse(YLocation);
            Array.Reverse(ZLocation);
            byte[] buffer = new byte[] { XLocation[0], XLocation[1], XLocation[2], XLocation[3], YLocation[0], YLocation[1], YLocation[2], YLocation[3], ZLocation[0], ZLocation[1], ZLocation[2] };
            PS3.SetMemory(location, buffer);
            Thread.Sleep(10);
            RPC.Call(Natives.DO_SCREEN_FADE_OUT, 400);
            Thread.Sleep(300);
            RPC.Call(Natives.SET_ENTITY_COORDS, NativeFunctions.isInVehicle() ? NativeFunctions.vehid() : NativeFunctions.pedid(), location, 1, 0, 0, 1);
            Thread.Sleep(1000);
            RPC.Call(Natives.DO_SCREEN_FADE_IN, 400);
        }
        #endregion

        class Garage
        {
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
        public class Setting
        {
            public string name { get; set; }
            public object value { get; set; }
            public Setting(string Name, object Value)
            {
                name = Name;
                value = Value;
            }
        }
        public class VehicleModel
        {
            public string label { get; set; }
            public string model { get; set; }
            public VehicleModel(string Label, string Model)
            {
                label = Label;
                model = Model;
            }
        }
        private List<Setting> settingsList = new List<Setting>();
        Dictionary<string, string> VehicleModels = new Dictionary<string, string>();
        string[] filePaths;
        private XmlDocument xd_mp = new XmlDocument();

        Dictionary<string, object> ImperiumData = new Dictionary<string, object>();
        #endregion

        public Main()
        {
            InitializeComponent();

            #region Server Data
            if (new Ping().Send("lexicongta.com").Status == IPStatus.Success) // Server is online
            {
                System.Net.HttpWebRequest Request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://lexicongta.com/imperium/client_data.json");
                Request.UserAgent = "dick";
                var Response = Request.GetResponse().GetResponseStream();
                using (StreamReader sr = new StreamReader(Response))
                {
                    ImperiumData = JsonConvert.DeserializeObject<Dictionary<string, object>>( sr.ReadToEnd() );

                    #region Main
                    if (ImperiumData.ContainsKey("name") && ImperiumData.ContainsKey("tag"))
                    {
                        this.Text = ImperiumData["name"] + " " + Variables.versionLabel + " " + ImperiumData["tag"];
                    }

                    if (ImperiumData.ContainsKey("latest_version") && ImperiumData.ContainsKey("latest_version_label"))
                    {
                        if (Convert.ToInt32(ImperiumData["latest_version"]) > Variables.version && !Properties.Settings.Default.UpdateNotified)
                        {
                            System.Diagnostics.Process.Start(
                                "http://www.nextgenupdate.com/forums/gta-5-mod-tools/921980-ps3-1-26-bles-cc-tm-imperium-account-editor-0-7-alpha-release-source.html"
                                );
                            DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Woah! There's an Imperium update available!\n\n" +
                            "Your version is " + Variables.versionLabel + ", the newest version is " + ImperiumData["latest_version_label"].ToString() + "!"
                            );
                            Properties.Settings.Default.UpdateNotified = true;
                            Properties.Settings.Default.Save();
                        }
                    }

                    if (ImperiumData.ContainsKey("news_show") && ImperiumData.ContainsKey("news"))
                    {
                        if (Convert.ToBoolean(ImperiumData["news_show"]) && ImperiumData["news"].ToString().Length > 0)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("--- NEWS! ------------------------------------------------------\n" +
                                ImperiumData["news"].ToString());
                        }
                    }
                    #endregion

                    #region RPC & Addresses
                    // RPC
                    if (ImperiumData.ContainsKey("SFA1"))
                        RPC.SFA1 = Convert.ToUInt32(ImperiumData["SFA1"]);
                    if (ImperiumData.ContainsKey("SFA1"))
                        RPC.SFA1 = Convert.ToUInt32(ImperiumData["SFA1"]);
                    if (ImperiumData.ContainsKey("EFA1"))
                        RPC.SFA1 = Convert.ToUInt32(ImperiumData["EFA1"]);
                    if (ImperiumData.ContainsKey("BFA1"))
                        RPC.SFA1 = Convert.ToUInt32(ImperiumData["BFA1"]);
                    if (ImperiumData.ContainsKey("BAB1"))
                        RPC.SFA1 = Convert.ToUInt32(ImperiumData["BAB1"]);
                    
                    // Addresses
                    if (ImperiumData.ContainsKey("UNK_70559AC7"))
                        Natives.UNK_70559AC7 = Convert.ToUInt32(ImperiumData["UNK_70559AC7"]);
                    if (ImperiumData.ContainsKey("MEM_MONEY"))
                        Natives.MEM_MONEY = Convert.ToUInt32(ImperiumData["MEM_MONEY"]);
                    if (ImperiumData.ContainsKey("CLEAR_ALL_PED_PROPS"))
                        Natives.CLEAR_ALL_PED_PROPS = Convert.ToUInt32(ImperiumData["CLEAR_ALL_PED_PROPS"]);
                    if (ImperiumData.ContainsKey("CLEAR_PED_DECORATIONS"))
                        Natives.CLEAR_PED_DECORATIONS = Convert.ToUInt32(ImperiumData["CLEAR_PED_DECORATIONS"]);
                    if (ImperiumData.ContainsKey("DO_SCREEN_FADE_IN"))
                        Natives.DO_SCREEN_FADE_IN = Convert.ToUInt32(ImperiumData["DO_SCREEN_FADE_IN"]);
                    if (ImperiumData.ContainsKey("DO_SCREEN_FADE_OUT"))
                        Natives.DO_SCREEN_FADE_OUT = Convert.ToUInt32(ImperiumData["DO_SCREEN_FADE_OUT"]);
                    if (ImperiumData.ContainsKey("GET_PLAYER_NAME"))
                        Natives.GET_PLAYER_NAME = Convert.ToUInt32(ImperiumData["GET_PLAYER_NAME"]);
                    if (ImperiumData.ContainsKey("GET_VEHICLE_PED_IS_USING"))
                        Natives.GET_VEHICLE_PED_IS_USING = Convert.ToUInt32(ImperiumData["GET_VEHICLE_PED_IS_USING"]);
                    if (ImperiumData.ContainsKey("IS_PED_IN_ANY_VEHICLE"))
                        Natives.IS_PED_IN_ANY_VEHICLE = Convert.ToUInt32(ImperiumData["IS_PED_IN_ANY_VEHICLE"]);
                    if (ImperiumData.ContainsKey("NETWORK_EARN_FROM_ROCKSTAR"))
                        Natives.NETWORK_EARN_FROM_ROCKSTAR = Convert.ToUInt32(ImperiumData["NETWORK_EARN_FROM_ROCKSTAR"]);
                    if (ImperiumData.ContainsKey("NETWORK_SPENT_CASH_DROP"))
                        Natives.NETWORK_SPENT_CASH_DROP = Convert.ToUInt32(ImperiumData["NETWORK_SPENT_CASH_DROP"]);
                    if (ImperiumData.ContainsKey("PLAYER_ID"))
                        Natives.PLAYER_ID = Convert.ToUInt32(ImperiumData["PLAYER_ID"]);
                    if (ImperiumData.ContainsKey("PLAYER_PED_ID"))
                        Natives.PLAYER_PED_ID = Convert.ToUInt32(ImperiumData["PLAYER_PED_ID"]);
                    if (ImperiumData.ContainsKey("SET_ENTITY_COORDS"))
                        Natives.SET_ENTITY_COORDS = Convert.ToUInt32(ImperiumData["SET_ENTITY_COORDS"]);
                    if (ImperiumData.ContainsKey("SET_PED_COMPONENT_VARIATION"))
                        Natives.SET_PED_COMPONENT_VARIATION = Convert.ToUInt32(ImperiumData["SET_PED_COMPONENT_VARIATION"]);
                    if (ImperiumData.ContainsKey("SET_PED_PROP_INDEX"))
                        Natives.SET_PED_PROP_INDEX = Convert.ToUInt32(ImperiumData["SET_PED_PROP_INDEX"]);
                    if (ImperiumData.ContainsKey("STAT_GET_BOOL"))
                        Natives.STAT_GET_BOOL = Convert.ToUInt32(ImperiumData["STAT_GET_BOOL"]);
                    if (ImperiumData.ContainsKey("STAT_GET_DATE"))
                        Natives.STAT_GET_DATE = Convert.ToUInt32(ImperiumData["STAT_GET_DATE"]);
                    if (ImperiumData.ContainsKey("STAT_GET_FLOAT"))
                        Natives.STAT_GET_FLOAT = Convert.ToUInt32(ImperiumData["STAT_GET_FLOAT"]);
                    if (ImperiumData.ContainsKey("STAT_GET_INT"))
                        Natives.STAT_GET_INT = Convert.ToUInt32(ImperiumData["STAT_GET_INT"]);
                    if (ImperiumData.ContainsKey("STAT_GET_POS"))
                        Natives.STAT_GET_POS = Convert.ToUInt32(ImperiumData["STAT_GET_POS"]);
                    if (ImperiumData.ContainsKey("STAT_GET_STRING"))
                        Natives.STAT_GET_STRING = Convert.ToUInt32(ImperiumData["STAT_GET_STRING"]);
                    if (ImperiumData.ContainsKey("STAT_GET_USER_ID"))
                        Natives.STAT_GET_USER_ID = Convert.ToUInt32(ImperiumData["STAT_GET_USER_ID"]);
                    if (ImperiumData.ContainsKey("STAT_SAVE"))
                        Natives.STAT_SAVE = Convert.ToUInt32(ImperiumData["STAT_SAVE"]);
                    if (ImperiumData.ContainsKey("STAT_SET_BOOL"))
                        Natives.STAT_SET_BOOL = Convert.ToUInt32(ImperiumData["STAT_SET_BOOL"]);
                    if (ImperiumData.ContainsKey("STAT_SET_DATE"))
                        Natives.STAT_SET_DATE = Convert.ToUInt32(ImperiumData["STAT_SET_DATE"]);
                    if (ImperiumData.ContainsKey("STAT_SET_FLOAT"))
                        Natives.STAT_SET_FLOAT = Convert.ToUInt32(ImperiumData["STAT_SET_FLOAT"]);
                    if (ImperiumData.ContainsKey("STAT_SET_INT"))
                        Natives.STAT_SET_INT = Convert.ToUInt32(ImperiumData["STAT_SET_INT"]);
                    if (ImperiumData.ContainsKey("STAT_SET_POS"))
                        Natives.STAT_SET_POS = Convert.ToUInt32(ImperiumData["STAT_SET_POS"]);
                    if (ImperiumData.ContainsKey("STAT_SET_STRING"))
                        Natives.STAT_SET_STRING = Convert.ToUInt32(ImperiumData["STAT_SET_STRING"]);
                    if (ImperiumData.ContainsKey("STAT_SET_USER_ID"))
                        Natives.STAT_SET_USER_ID = Convert.ToUInt32(ImperiumData["STAT_SET_USER_ID"]);
                    #endregion
                }
            }
            else
            {
                this.Text = "Imperium Account Editor " + Variables.versionLabel + " [PS3 BLES 1.27]";
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Unable to connect to server! (lexicongta.com)\n" +
                "The server is used to grab information about new updates & ensure compatibility with the latest GTA update.\n" +
                "Try again in a bit... For now, we'll use default application data."
                );
            }
            #endregion

            #region JSON File Importing
            // Initialize Settings
            string settings_filepath = "Data/Settings.json";
            if (File.Exists(settings_filepath))
            {
                string[] Settings_Settings_content = File.ReadAllLines(settings_filepath);
                foreach (string line in Settings_Settings_content)
                {
                    Setting setting = JsonConvert.DeserializeObject<Setting>(line);
                    settingsList.Add(setting);
                    if (setting.name == "Character_1")
                    {
                        char1.Checked = Variables.character1 = (bool)setting.value;
                    }
                    if (setting.name == "Character_2")
                    {
                        char2.Checked = Variables.character2 = (bool)setting.value;
                    }
                }
            }
            else MessageBox.Show(settings_filepath + " is missing! :(");
            // Initialize Vehicle Models
            string vehicles_filepath = "Data/Vehicles.json";
            if (File.Exists(vehicles_filepath))
            {
                string[] vehicles_content = File.ReadAllLines(vehicles_filepath);
                garModel.Properties.Items.Clear();
                // Grab from json
                foreach (string line in vehicles_content)
                {
                    VehicleModel vehicle = JsonConvert.DeserializeObject<VehicleModel>(line);
                    if (vehicle.model != "")
                    {
                        VehicleModels.Add(vehicle.label == "" ? vehicle.model.ToUpper() : vehicle.label, vehicle.model);
                    }
                }
                // Alphabetize
                /*var query = from item in VehicleModels
                            orderby item.Key ascending
                            select item;*/
                // Add to combo box
                foreach (KeyValuePair<string, string> entry in VehicleModels)
                {
                    garModel.Properties.Items.Add(entry.Key);
                }
            }
            else MessageBox.Show(vehicles_filepath + " is missing! :(");
            #endregion

            #region XML File Importing
            filePaths = Directory.GetFiles("Data/StatFiles/");
            foreach (string path in filePaths)
                INS_File.Properties.Items.Add(path.Substring(15, path.Length - 15));
            INS_File.SelectedIndex = 0;

            xd_mp.Load(filePaths[0]);
            foreach (XmlNode xmlNode in xd_mp.SelectNodes("StatsSetup/stats/stat/@Name"))
                INS_Stat.Properties.Items.Add((object)xmlNode.Value);
            #endregion

            // Refresh Outfit
            refreshOutfitListing();

        }

        #region Connect, Disconnect
        void Connect(SelectAPI api)
        {
            PS3.DisconnectTarget();
            Variables.api = api;
            bool tmapi = api == SelectAPI.TargetManager;
            PS3.ChangeAPI(api);
            try
            {
                PS3.ConnectTarget();
                PS3.AttachProcess();

                RPC.Enable();

                if (NativeFunctions.psn() == "")
                {
                    connectionStatus.Caption = "Error!";
                    connectionPSN.Caption = "RPC Enable Failed!";
                    connectionPSN.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    connectionStatus.Caption = "Connected [" + (tmapi ? "TM" : "CC") + "]";
                    connectionPSN.Caption = "Welcome, " + NativeFunctions.psn() + " [Console: " + PS3.GetConsoleName() + "]";
                    connectionPSN.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                    otftDoRefresh(true);

                }
                barSub_Connect.Glyph = Imperium.Properties.Resources.link_connected;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed\n\nError: " + ex.Message);
            }
        }

        // Connect
        private void barButton_TM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Connect(SelectAPI.TargetManager);
        }

        private void barButton_CC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Connect(SelectAPI.ControlConsole);
        }

        // Disconnect
        private void barButton_Disconnect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PS3.DisconnectTarget();
            connectionStatus.Caption = "Idle...";
            connectionPSN.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            barSub_Connect.Glyph = Imperium.Properties.Resources.link_idle;
        }
        #endregion

        #region Functions
        public static uint Hash(string input)
        {
            byte[] stingbytes = Encoding.UTF8.GetBytes(input.ToLower());
            uint num1 = 0U;
            for (int i = 0; i < stingbytes.Length; i++)
            {
                uint num2 = num1 + (uint)stingbytes[i];
                uint num3 = num2 + (num2 << 10);
                num1 = num3 ^ num3 >> 6;
            }
            uint num4 = num1 + (num1 << 3);
            uint num5 = num4 ^ num4 >> 11;
            return num5 + (num5 << 15);
        }

        private void char1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Variables.character1 = char1.Checked;
        }

        private void char2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Variables.character2 = char2.Checked;
        }

        void setStat(string stat, object value)
        {
            statStatus.Caption = "Last Used: " + stat;
            statStatus.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (value is int || value is bool)
            {
                uint address = value is int ? Natives.STAT_SET_INT : Natives.STAT_SET_BOOL;
                if (stat.Contains("MPPLY_"))
                {
                    RPC.Call(address, Main.Hash(stat), Convert.ToInt32(value), 1);
                }
                else
                {
                    if (Variables.character1)
                    {
                        RPC.Call(address, Main.Hash("MP0_" + stat), Convert.ToInt32(value), 1);
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

        void setStatQuery(params string[] snippets)
        {
            foreach (string snippet in snippets)
            {
                var query = from item in Stats
                            where item.Key.ToLower().Contains(snippet.ToLower())
                            orderby item.Key ascending
                            select item;
                foreach (KeyValuePair<string, StatData> item in query)
                {
                    setStat(item.Key, item.Value.value);
                }
            }
        }

        void setStatKeywordQuery(params string[] snippets)
        {
            foreach (string snippet in snippets)
            {
                var query = from item in Stats
                            where item.Value.keyword.ToLower().Contains(snippet.ToLower())
                            orderby item.Key ascending
                            select item;
                foreach (KeyValuePair<string, StatData> item in query)
                {
                    setStat(item.Key, item.Value.value);
                }
            }
        }

        string[] getOutfitTitles()
        {
            string filepath = "Data/Outfits.xml";
            if (File.Exists(filepath))
            {
                List<string> list = new List<string>();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                using (XmlReader reader = XmlReader.Create(new StringReader(File.ReadAllText(filepath)), settings))
                {
                    while (reader.ReadToFollowing("outfit"))
                    {
                        reader.MoveToFirstAttribute();
                        list.Add(reader.Value);
                    }
                }
                return list.ToArray();
            }
            return null;
        }
        
        string[] aoElements = new string[] { "mask", "hat", "eyes", "ears", "hair", "torso", "tops1", "tops2", "legs", "shoes", "face", "extra", "hands", "armor", "emblem" };
        
        Dictionary<string, object[]> getOutfitData(string title)
        {
            string filepath = "Data/Outfits.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            XmlNode foundNode = null;
            foreach (XmlNode node in doc.DocumentElement.SelectNodes("/root/outfit"))
            {
                if (node.Attributes["title"].InnerText == title)
                {
                    foundNode = node;
                }
            }

            Dictionary<string, object[]> data = new Dictionary<string, object[]>();
            if (foundNode != null)
            {
                data.Add("gender", new object[] { foundNode.Attributes["gender"].InnerText });
                data.Add("creator", new object[] { foundNode.Attributes["creator"].InnerText });
                for (int i = 0; i < aoElements.Count(); i++)
                {
                    data.Add(aoElements[i], new object[] { Convert.ToInt32(foundNode.SelectSingleNode(aoElements[i]).Attributes["model"].InnerText), Convert.ToInt32(foundNode.SelectSingleNode(aoElements[i]).Attributes["texture"].InnerText) });
                }
                data.Add("description", new object[] { foundNode.SelectSingleNode("description").InnerXml });
            }
            return data;
        }
        void refreshOutfitListing()
        {
            int index = aoListing.SelectedIndex;
            aoListing.Items.Clear();
            foreach (string label in getOutfitTitles())
            {
                aoListing.Items.Add(label);
            }
            aoListing.SelectedIndex = index;
        }
        void aoeRefreshControls()
        {
            if (aoListing.SelectedIndex != -1)
            {
                DevExpress.XtraEditors.SpinEdit[] aoeControls_m = new DevExpress.XtraEditors.SpinEdit[] { aoeMask_m, aoeHat_m, aoeEyes_m, aoeEars_m, aoeHair_m, aoeTorso_m, aoeTops1_m, aoeTops2_m, aoeLegs_m, aoeShoes_m, aoeFace_m, aoeExtra_m, aoeHands_m, aoeArmor_m, aoeEmblem_m };
                DevExpress.XtraEditors.SpinEdit[] aoeControls_t = new DevExpress.XtraEditors.SpinEdit[] { aoeMask_t, aoeHat_t, aoeEyes_t, aoeEars_t, aoeHair_t, aoeTorso_t, aoeTops1_t, aoeTops2_t, aoeLegs_t, aoeShoes_t, aoeFace_t, aoeExtra_t, aoeHands_t, aoeArmor_t, aoeEmblem_t };
                Dictionary<string, object[]> data = getOutfitData(aoListing.Text);
                aoeTitle.Text = getOutfitTitles()[aoListing.SelectedIndex];

                for (int i = 0; i < aoeControls_m.Count(); i++)
                {
                    aoeControls_m[i].Text = data[aoElements[i]][0].ToString();
                    aoeControls_t[i].Text = data[aoElements[i]][1].ToString();
                }

                aoeDescription.Text = data["description"][0].ToString();
                aoeGender.SelectedIndex = data["gender"][0].ToString() == "male" ? 0 : 1;
                aoeCreator.Text = "Creator: " + data["creator"][0].ToString();
            }
        }
        
        void Reset()
        {
            RPC.Call(Natives.CLEAR_ALL_PED_PROPS, NativeFunctions.pedid());
            RPC.Call(Natives.CLEAR_PED_DECORATIONS, NativeFunctions.pedid());
            RPC.Call(Natives.SET_PED_COMPONENT_VARIATION, NativeFunctions.pedid(), 1, 0, 0);
            RPC.Call(Natives.SET_PED_COMPONENT_VARIATION, NativeFunctions.pedid(), 5, 0, 0);
            RPC.Call(Natives.SET_PED_COMPONENT_VARIATION, NativeFunctions.pedid(), 9, 0, 0);
        }

        void setClothing(string family, string model, string texture)
        {
            int fam = 0;
            if (family == "HAT" || family == "EYES" || family == "EARS")
            {
                switch (family)
                {
                    case "HAT": fam = 0; break;
                    case "EYES": fam = 1; break;
                    case "EARS": fam = 2; break;
                }
                if (model != "-1" && texture != "-1")
                    RPC.Call(Natives.SET_PED_PROP_INDEX, NativeFunctions.pedid(), fam, Convert.ToInt32(model) - 1, Convert.ToInt32(texture));
            }
            else
            {
                switch (family)
                {
                    case "FACE": fam = 0; break;
                    case "MASK": fam = 1; break;
                    case "HAIR": fam = 2; break;
                    case "TORSO": fam = 3; break;
                    case "LEGS": fam = 4; break;
                    case "HANDS": fam = 5; break;
                    case "SHOES": fam = 6; break;
                    case "EXTRA": fam = 7; break;
                    case "TOPS1": fam = 8; break;
                    case "ARMOR": fam = 9; break;
                    case "EMBLEM": fam = 10; break;
                    case "TOPS2": fam = 11; break;
                }
                if (model != "-1" && texture != "-1")
                    RPC.Call(Natives.SET_PED_COMPONENT_VARIATION, NativeFunctions.pedid(), fam, Convert.ToInt32(model), Convert.ToInt32(texture));
            }
        }

        void otftDoRefresh(bool hard = false)
        {
            if (hard)
            {
                int scroll = otftListing.SelectedIndex;
                otftListing.Items.Clear();
                for (int i = 0; i < 10; i++)
                {
                    otftListing.Items.Add("[" + i + "] " + (Outfit.Name(i) == "" ? "Empty" : Outfit.Name(i)));
                }
                if (scroll >= 0)
                    otftListing.SelectedIndex = scroll;
            }
            if (Outfit.Name(otftListing.SelectedIndex) != "")
            {
                otftPanel.Visible = true;
                otftTitle.Text = Outfit.Name(otftListing.SelectedIndex);
                OutfitStruct o = Outfit.Fetch(otftListing.SelectedIndex);
                otft_eMask_m.Value = o.mask;
                otft_eMask_t.Value = o.maskT;
                otft_eTorso_m.Value = o.torso;
                otft_eTorso_t.Value = o.torsoT;
                otft_eLegs_m.Value = o.legs;
                otft_eLegs_t.Value = o.legsT;
                otft_eHands_m.Value = o.hands;
                otft_eHands_t.Value = o.handsT;
                otft_eShoes_m.Value = o.shoes;
                otft_eShoes_t.Value = o.shoesT;
                otft_eExtra_m.Value = o.extra;
                otft_eExtra_t.Value = o.extraT;
                otft_eTops1_m.Value = o.tops1;
                otft_eTops1_t.Value = o.tops1T;
                otft_eArmor_m.Value = o.armor;
                otft_eArmor_t.Value = o.armorT;
                otft_eEmblem_m.Value = o.emblem;
                otft_eEmblem_t.Value = o.emblemT;
                otft_eTops2_m.Value = o.tops2;
                otft_eTops2_t.Value = o.tops2T;
                otft_eHat_m.Value = o.hat;
                otft_eHat_t.Value = o.hatT;
                otft_eEyes_m.Value = o.eyes;
                otft_eEyes_t.Value = o.eyesT;
                otft_eEars_m.Value = o.ears;
                otft_eEars_t.Value = o.earsT;
            }
            else
            {
                otftPanel.Visible = false;
            }
        }

        bool garUpdating = false;

        void refreshGarage()
        {
            int index = garListing.SelectedIndex;
            garListing.Items.Clear();
            for (int i = 0; i < 39; i++)
            {
                uint model = Garage.getUint(i, Garage.Model);
                int ni = i + 1;
                string prefix = "[" + Math.Ceiling((decimal)ni / 13) + "/" + (ni > 13 ? (ni > 26 ? ni - 26 : ni - 13) : ni).ToString("D2") + "] ";
                var query = (from item in VehicleModels
                             where Hash(item.Value) == model
                             select new { item.Key }).SingleOrDefault();
                if (model == 0)
                    garListing.Items.Add(prefix + "---");
                else if (query == null)
                    garListing.Items.Add(prefix + model.ToString("X2"));
                else
                    garListing.Items.Add(prefix + query.Key);
            }
            garListing.SelectedIndex = index == -1 ? 0 : index;
        }

        void refreshGarageControls()
        {
            int i = garListing.SelectedIndex;
            if (i >= 0 && i <= 39)
            {
                garUpdating = true;
                garPlateText.Text = Garage.getString(i, Garage.Plate_Text);
                garRGB.Color = Color.FromArgb(
                    Garage.getInt(i, Garage.RGB_Cache_R), 
                    Garage.getInt(i, Garage.RGB_Cache_G), 
                    Garage.getInt(i, Garage.RGB_Cache_B)
                    );
                /*garRGB_Sec.Color = Color.FromArgb(
                    Convert.ToInt32(Garage.getUint(i, Garage.RGB_Cache_R | Garage.RGB_Secondary)),
                    Convert.ToInt32(Garage.getUint(i, Garage.RGB_Cache_G | Garage.RGB_Secondary)),
                    Convert.ToInt32(Garage.getUint(i, Garage.RGB_Cache_B | Garage.RGB_Secondary))
                    );*/
                garUpdating = false;
            }
        }
        public uint DateStruct_2_Memory(int _year, int _month, int _day, int _hour, int _minute, int _second, int _millisecond)
        {
            uint location = 0x10030000;
            PS3.Extension.WriteInt32(0x10030000, _year);
            PS3.Extension.WriteInt32(0x10030000 + 4, _month);
            PS3.Extension.WriteInt32(0x10030000 + 8, _day);
            PS3.Extension.WriteInt32(0x10030000 + 12, _hour);
            PS3.Extension.WriteInt32(0x10030000 + 16, _minute);
            PS3.Extension.WriteInt32(0x10030000 + 20, _second);
            PS3.Extension.WriteInt32(0x10030000 + 24, _millisecond);
            return location;
        }
        private string Cap1(string text)
        {
            return text.Substring(0, 1).ToUpper() + text.Substring(1, text.Length - 1).ToLower();
        }
        string formatStat(string stat)
        {
            return stat.Contains("MPPLY_") ? stat : ("MP0_" + stat);
        }

        #endregion

        #region Options
        private void gContacts_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("contacts");
        }

        private void gClothing_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("clothing", "clothingHair");
        }

        private void gWeapons_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("weapons");
        }

        private void gSkills_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("skills");
        }

        private void gKits_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("kits");
        }

        private void gChromeRims_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("chromeRims");
        }

        private void gTattoos_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("tattoos");
        }

        private void gAwards_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("awards");
        }

        private void gHeistVehicles_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("heistVehicles");
        }

        private void gVehicleMods_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("vehicleMods");
        }

        private void gInfiniteAmmo_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("infiniteAmmo");
        }

        private void gAddBank_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int value = Convert.ToInt32(gAddBank.Text);
            // Attempting to add negative values will result in random outcomes, so we check for if it's more than $0
            if (value > 0)
            {
                // A normal addition of money. Adds what you tell it to add.
                if (value <= 10000000)
                {
                    RPC.Call(Natives.NETWORK_EARN_FROM_ROCKSTAR, value);
                }
                // This native will not permit you to add more than $10,000,000 per call, so we break it up.
                else
                {
                    int valueStorage = value;
                    while (valueStorage > 0)
                    {
                        if (valueStorage >= 10000000)
                        {
                            RPC.Call(Natives.NETWORK_EARN_FROM_ROCKSTAR, 10000000);
                            valueStorage -= 10000000;
                        }
                        // This would be the final call. It's setting the remainder that is less than $10,000,000
                        else
                        {
                            RPC.Call(Natives.NETWORK_EARN_FROM_ROCKSTAR, valueStorage);
                            valueStorage = 0;
                        }
                    }
                }
            }
        }

        private void DLC_Christmas_Click(object sender, EventArgs e)
        {
            Lib.boolNotify(Tunables.christmasDLC());
        }

        private void DLC_Independence_Click(object sender, EventArgs e)
        {
            Lib.boolNotify(Tunables.independenceDLC());
        }

        private void DLC_Valentines_Click(object sender, EventArgs e)
        {
            Lib.boolNotify(Tunables.valentinesDLC());
        }
        private void uIdleKick_Click(object sender, EventArgs e)
        {
            Lib.boolNotify(Tunables.idleKick());
        }

        private void gTeleportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load Teleport Locations into combo box
            var query = from item in Locations
                        where item.Value.grouping.ToLower().Contains(gTeleportType.Text.ToLower())
                        orderby item.Value.label ascending
                        select item;
            gTeleportLoc.Properties.Items.Clear();
            foreach (KeyValuePair<double[], TpData> item in query)
            {
                gTeleportLoc.Properties.Items.Add(item.Value.label);
            }
            gTeleportLoc.SelectedIndex = 0;
        }

        private void gTeleport_Click(object sender, EventArgs e)
        {
            var query = (from item in Locations
                         where item.Value.grouping.ToLower() == gTeleportType.Text.ToLower() && item.Value.label.ToLower() == gTeleportLoc.Text.ToLower()
                        select item.Key).ToArray();
            teleport(query[0]);
        }

        private void gTutorial_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("tutorial");
        }

        private void gRedesign_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("redesign");
        }

        private void gHeistTrophies_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("heistTrophies");
        }

        private void gRank_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            setStat("CHAR_XP_FM", Lib.Ranks[Convert.ToInt32(gRank.Text) - 1]);
        }

        private void gRP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            setStat("CHAR_XP_FM", Convert.ToInt32(gRP.Text));
        }

        private void gAddCash_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.MEM_MONEY, Convert.ToInt32(gAddCash.Text), 0);
        }

        private void aoRefresh_Click(object sender, EventArgs e)
        {
            refreshOutfitListing();
            aoListing.SelectedIndex = outfitTabs.SelectedTabPageIndex = 0;
        }

        private void aoListing_SelectedIndexChanged(object sender, EventArgs e)
        {
            aoeRefreshControls();
        }

        private void aoeSave_Click(object sender, EventArgs e)
        {
            // Load XML
            string filepath = "Data/Outfits.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            // Find outfit node
            XmlNode foundNode = null;
            foreach (XmlNode node in doc.DocumentElement.SelectNodes("/root/outfit"))
            {
                if (node.Attributes["title"].InnerText == aoListing.Text)
                {
                    foundNode = node;
                }
            }

            // Set title
            foundNode.Attributes["title"].InnerText = aoeTitle.Text;

            // Set values
            DevExpress.XtraEditors.SpinEdit[] aoeControls_m = new DevExpress.XtraEditors.SpinEdit[] { aoeMask_m, aoeHat_m, aoeEyes_m, aoeEars_m, aoeHair_m, aoeTorso_m, aoeTops1_m, aoeTops2_m, aoeLegs_m, aoeShoes_m, aoeFace_m, aoeExtra_m, aoeHands_m, aoeArmor_m, aoeEmblem_m };
            DevExpress.XtraEditors.SpinEdit[] aoeControls_t = new DevExpress.XtraEditors.SpinEdit[] { aoeMask_t, aoeHat_t, aoeEyes_t, aoeEars_t, aoeHair_t, aoeTorso_t, aoeTops1_t, aoeTops2_t, aoeLegs_t, aoeShoes_t, aoeFace_t, aoeExtra_t, aoeHands_t, aoeArmor_t, aoeEmblem_t };
            for (int i = 0; i < aoElements.Count(); i++)
            {
                foundNode.SelectSingleNode(aoElements[i]).Attributes["model"].InnerText = aoeControls_m[i].Text;
                foundNode.SelectSingleNode(aoElements[i]).Attributes["texture"].InnerText = aoeControls_t[i].Text;
            }

            // Set gender
            foundNode.Attributes["gender"].InnerText = aoeGender.SelectedIndex == 0 ? "male" : "female";
            // Set description
            foundNode.SelectSingleNode("description").InnerXml = aoeDescription.Text;

            // Save XML
            doc.Save(filepath);

            refreshOutfitListing();
        }

        private void aoaAdd_Click(object sender, EventArgs e)
        {
            // Load XML
            string filepath = "Data/Outfits.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            // Create main node
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "outfit", null);

            // Add title attribute to main node
            XmlAttribute nodeTitle = doc.CreateAttribute("title");
            nodeTitle.Value = aoaTitle.Text;
            node.Attributes.SetNamedItem(nodeTitle);
            XmlAttribute nodeGender = doc.CreateAttribute("gender");
            nodeGender.Value = aoaGender.Text.ToLower();
            node.Attributes.SetNamedItem(nodeGender);
            XmlAttribute nodeCreator = doc.CreateAttribute("creator");
            nodeCreator.Value = aoaCreator.Text;
            node.Attributes.SetNamedItem(nodeCreator);

            DevExpress.XtraEditors.SpinEdit[] aoaControls_m = new DevExpress.XtraEditors.SpinEdit[] { aoaMask_m, aoaHat_m, aoaEyes_m, aoaEars_m, aoaHair_m, aoaTorso_m, aoaTops1_m, aoaTops2_m, aoaLegs_m, aoaShoes_m, aoaFace_m, aoaExtra_m, aoaHands_m, aoaArmor_m, aoaEmblem_m };
            DevExpress.XtraEditors.SpinEdit[] aoaControls_t = new DevExpress.XtraEditors.SpinEdit[] { aoaMask_t, aoaHat_t, aoaEyes_t, aoaEars_t, aoaHair_t, aoaTorso_t, aoaTops1_t, aoaTops2_t, aoaLegs_t, aoaShoes_t, aoaFace_t, aoaExtra_t, aoaHands_t, aoaArmor_t, aoaEmblem_t };
            for (int i = 0; i < aoElements.Count(); i++)
            {
                // Create new node
                XmlNode newNode = doc.CreateElement(aoElements[i]);

                // Add model attribute to new node
                XmlAttribute newNode_m = doc.CreateAttribute("model");
                newNode_m.Value = aoaControls_m[i].Text;
                newNode.Attributes.SetNamedItem(newNode_m);

                XmlAttribute newNode_t = doc.CreateAttribute("texture");
                newNode_t.Value = aoaControls_t[i].Text;
                newNode.Attributes.SetNamedItem(newNode_t);

                // Add new node to main node
                node.AppendChild(newNode);
            }

            // Add description
            XmlNode descriptionNode = doc.CreateElement("description");
            descriptionNode.InnerXml = aoaDescription.Text == "" ? "None" : aoaDescription.Text;
            node.AppendChild(descriptionNode);

            // Add main node to XML
            doc.DocumentElement.AppendChild(node);

            // Save XML
            doc.Save(filepath);

            refreshOutfitListing();
        }

        private void aoEquip_Click(object sender, EventArgs e)
        {
            Reset();
            setClothing("MASK", aoeMask_m.Text, aoeMask_t.Text);
            setClothing("HAT", aoeHat_m.Text, aoeHat_t.Text);
            setClothing("EYES", aoeEyes_m.Text, aoeEyes_t.Text);
            setClothing("EARS", aoeEars_m.Text, aoeEars_t.Text);
            setClothing("HAIR", aoeHair_m.Text, aoeHair_t.Text);
            setClothing("TORSO", aoeTorso_m.Text, aoeTorso_t.Text);
            setClothing("TOPS1", aoeTops1_m.Text, aoeTops1_t.Text);
            setClothing("TOPS2", aoeTops2_m.Text, aoeTops2_t.Text);
            setClothing("LEGS", aoeLegs_m.Text, aoeLegs_t.Text);
            setClothing("SHOES", aoeShoes_m.Text, aoeShoes_t.Text);
            setClothing("FACE", aoeFace_m.Text, aoeFace_t.Text);
            setClothing("EXTRA", aoeExtra_m.Text, aoeExtra_t.Text);
            setClothing("HANDS", aoeHands_m.Text, aoeHands_t.Text);
            setClothing("ARMOR", aoeArmor_m.Text, aoeArmor_t.Text);
            setClothing("EMBLEM", aoeEmblem_m.Text, aoeEmblem_t.Text);
        }

        private void aoDelete_Click(object sender, EventArgs e)
        {
            if (aoListing.SelectedIndex != -1)
            {
                string filepath = "Data/Outfits.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                XmlNode foundNode = null;
                foreach (XmlNode node in doc.DocumentElement.SelectNodes("/root/outfit"))
                {
                    if (node.Attributes["title"].InnerText == aoListing.Text)
                    {
                        foundNode = node;
                    }
                }
                foundNode.ParentNode.RemoveChild(foundNode);
                doc.Save(filepath);
                aoListing.SelectedIndex = 0;
                refreshOutfitListing();
            }
        }

        private void exSkillEnhanced_Click(object sender, EventArgs e)
        {
            setStat("SCRIPT_INCREASE_DRIV", 120);
            setStat("SCRIPT_INCREASE_FLY", 120);
            setStat("SCRIPT_INCREASE_LUNG", 120);
            setStat("SCRIPT_INCREASE_MECH", 120);
            setStat("SCRIPT_INCREASE_SHO", 120);
            setStat("SCRIPT_INCREASE_STAM", 120);
            setStat("SCRIPT_INCREASE_STL", 120);
            setStat("SCRIPT_INCREASE_STRN", 120);
        }

        private void exSkillSuperhuman_Click(object sender, EventArgs e)
        {
            setStat("SCRIPT_INCREASE_DRIV", 1000);
            setStat("SCRIPT_INCREASE_FLY", 1000);
            setStat("SCRIPT_INCREASE_LUNG", 1000);
            setStat("SCRIPT_INCREASE_MECH", 1000);
            setStat("SCRIPT_INCREASE_SHO", 1000);
            setStat("SCRIPT_INCREASE_STAM", 1000);
            setStat("SCRIPT_INCREASE_STL", 1000);
            setStat("SCRIPT_INCREASE_STRN", 1000);
        }

        private void sdSync_Click(object sender, EventArgs e)
        {
            NativeFunctions.save();
        }

        private void sdE_set_Click(object sender, EventArgs e)
        {
            switch (sdE_t.Text)
            {
                case "int":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_SET_INT, Hash(sdE_s.Text), Convert.ToInt32(sdE_v.Text), 1)))
                        sdE_r.Text = "Stat Query Successful!";
                    else sdE_r.Text = "Stat Query Failed...";
                    break;
                case "s64":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_SET_INT, Hash(sdE_s.Text), Convert.ToInt64(sdE_v.Text), 1)))
                        sdE_r.Text = "Stat Query Successful!";
                    else sdE_r.Text = "Stat Query Failed...";
                    break;
                case "u8":
                case "u16":
                case "u32":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_SET_INT, Hash(sdE_s.Text), Convert.ToUInt32(sdE_v.Text), 1)))
                        sdE_r.Text = "Stat Query Successful!";
                    else sdE_r.Text = "Stat Query Failed...";
                    break;
                case "u64":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_SET_INT, Hash(sdE_s.Text), Convert.ToUInt64(sdE_v.Text), 1)))
                        sdE_r.Text = "Stat Query Successful!";
                    else sdE_r.Text = "Stat Query Failed...";
                    break;
                case "bool":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_SET_BOOL, Hash(sdE_s.Text), Convert.ToInt32(sdE_v.Text), 1)))
                        sdE_r.Text = "Stat Query Successful!";
                    else sdE_r.Text = "Stat Query Failed...";
                    break;
                case "float":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_SET_FLOAT, Hash(sdE_s.Text), float.Parse((sdE_v.Text)), 1)))
                        sdE_r.Text = "Stat Query Successful!";
                    else sdE_r.Text = "Stat Query Failed...";
                    break;
                case "string":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_SET_STRING, Hash(sdE_s.Text), sdE_v.Text, 1)))
                        sdE_r.Text = "Stat Query Successful!";
                    else sdE_r.Text = "Stat Query Failed...";
                    break;
                case "userid":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_SET_USER_ID, Hash(sdE_s.Text), sdE_v.Text, 1)))
                        sdE_r.Text = "Stat Query Successful!";
                    else sdE_r.Text = "Stat Query Failed...";
                    break;
            }
        }
        
        private void sdV_get_Click(object sender, EventArgs e)
        {
            switch (sdV_t.Text)
            {
                case "int":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash(sdV_s.Text), 0x10030040)))
                    {
                        sdV_v.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
                        sdV_r.Text = "Stat Query Successful!";
                    }
                    else
                    {
                        sdV_v.Text = "";
                        sdV_r.Text = "Stat Query Failed...";
                    }
                    break;
                case "s64":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash(sdV_s.Text), 0x10030040)))
                    {
                        sdV_v.Text = PS3.Extension.ReadInt64(0x10030040).ToString();
                        sdV_r.Text = "Stat Query Successful!";
                    }
                    else
                    {
                        sdV_v.Text = "";
                        sdV_r.Text = "Stat Query Failed...";
                    }
                    break;
                case "u8":
                case "u16":
                case "u32":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash(sdV_s.Text), 0x10030040)))
                    {
                        sdV_v.Text = PS3.Extension.ReadUInt32(0x10030040).ToString();
                        sdV_r.Text = "Stat Query Successful!";
                    }
                    else
                    {
                        sdV_v.Text = "";
                        sdV_r.Text = "Stat Query Failed...";
                    }
                    break;
                case "u64":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash(sdV_s.Text), 0x10030040)))
                    {
                        sdV_v.Text = PS3.Extension.ReadUInt64(0x10030040).ToString();
                        sdV_r.Text = "Stat Query Successful!";
                    }
                    else
                    {
                        sdV_v.Text = "";
                        sdV_r.Text = "Stat Query Failed...";
                    }
                    break;
                case "bool":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_BOOL, Hash(sdV_s.Text), 0x10030040)))
                    {
                        sdV_v.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
                        sdV_r.Text = "Stat Query Successful!";
                    }
                    else
                    {
                        sdV_v.Text = "";
                        sdV_r.Text = "Stat Query Failed...";
                    }
                    break;
                case "float":
                    if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_FLOAT, Hash(sdV_s.Text), 0x10030040)))
                    {
                        sdV_v.Text = PS3.Extension.ReadFloat(0x10030040).ToString();
                        sdV_r.Text = "Stat Query Successful!";
                    }
                    else
                    {
                        sdV_v.Text = "";
                        sdV_r.Text = "Stat Query Failed...";
                    }
                    break;
                case "string":
                    sdV_v.Text = PS3.Extension.ReadString(Convert.ToUInt32(RPC.Call(Natives.STAT_GET_STRING, Hash(sdV_s.Text), -1)));
                    sdV_r.Text = "";
                    break;
                case "userid":
                    sdV_v.Text = PS3.Extension.ReadString(Convert.ToUInt32(RPC.Call(Natives.STAT_GET_USER_ID, Hash(sdV_s.Text))));
                    sdV_r.Text = "";
                    break;
            }
        }

        private void gModdedRoll_Click(object sender, EventArgs e)
        {
            setStat("SCRIPT_INCREASE_STL", 600);
        }

        private void gFastRun_Click(object sender, EventArgs e)
        {
            setStat("CHAR_FM_ABILITY_1_UNLCK", -1);
            setStat("CHAR_FM_ABILITY_2_UNLCK", -1);
            setStat("CHAR_FM_ABILITY_3_UNLCK", -1);
        }

        private void gResetTimer_Click(object sender, EventArgs e)
        {
            setStat("MPPLY_VEHICLE_SELL_TIME", 0);
        }
        
        private void otftListing_SelectedIndexChanged(object sender, EventArgs e)
        {
            otftDoRefresh();
        }

        private void otR_Click(object sender, EventArgs e)
        {
            otftDoRefresh(true);
        }

        private void ot1_s_Click(object sender, EventArgs e)
        {
            Outfit.Name(otftListing.SelectedIndex, otftTitle.Text);
            OutfitStruct outfit = Outfit.Fetch(otftListing.SelectedIndex);
            outfit.mask = Convert.ToInt32(otft_eMask_m.Value);
            outfit.maskT = Convert.ToInt32(otft_eMask_t.Value);
            outfit.torso = Convert.ToInt32(otft_eTorso_m.Value);
            outfit.torsoT = Convert.ToInt32(otft_eTorso_t.Value);
            outfit.legs = Convert.ToInt32(otft_eLegs_m.Value);
            outfit.legsT = Convert.ToInt32(otft_eLegs_t.Value);
            outfit.hands = Convert.ToInt32(otft_eHands_m.Value);
            outfit.handsT = Convert.ToInt32(otft_eHands_t.Value);
            outfit.shoes = Convert.ToInt32(otft_eShoes_m.Value);
            outfit.shoesT = Convert.ToInt32(otft_eShoes_t.Value);
            outfit.extra = Convert.ToInt32(otft_eExtra_m.Value);
            outfit.extraT = Convert.ToInt32(otft_eExtra_t.Value);
            outfit.tops1 = Convert.ToInt32(otft_eTops1_m.Value);
            outfit.tops1T = Convert.ToInt32(otft_eTops1_t.Value);
            outfit.armor = Convert.ToInt32(otft_eArmor_m.Value);
            outfit.armorT = Convert.ToInt32(otft_eArmor_t.Value);
            outfit.emblem = Convert.ToInt32(otft_eEmblem_m.Value);
            outfit.emblemT = Convert.ToInt32(otft_eEmblem_t.Value);
            outfit.tops2 = Convert.ToInt32(otft_eTops2_m.Value);
            outfit.tops2T = Convert.ToInt32(otft_eTops2_t.Value);
            outfit.hat = Convert.ToInt32(otft_eHat_m.Value);
            outfit.hatT = Convert.ToInt32(otft_eHat_t.Value);
            outfit.eyes = Convert.ToInt32(otft_eEyes_m.Value);
            outfit.eyesT = Convert.ToInt32(otft_eEyes_t.Value);
            outfit.ears = Convert.ToInt32(otft_eEars_m.Value);
            outfit.earsT = Convert.ToInt32(otft_eEars_t.Value);
            Outfit.Apply(otftListing.SelectedIndex, outfit);
            otftDoRefresh(true);
        }

        private void otftMod_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (otftMod.Text)
            {
                case "Font - Pricedown":
                    {
                        otftTitle.Text = "~s~<font face=\"$gtaCash\">Text";
                        break;
                    }
                case "Font - Sign-Painter":
                    {
                        otftTitle.Text = "~s~<font face=\"$Font5\">Text";
                        break;
                    }
                case "Font - Chalet":
                    {
                        otftTitle.Text = "~s~<font face=\"$Font2_cond\">Text";
                        break;
                    }
                case "Icon - R* Verified":
                    {
                        if (otftTitle.Text.Length <= 30)
                            otftTitle.Text += "¦";
                        break;
                    }
                case "Icon - R* Logo #1":
                    {
                        if (otftTitle.Text.Length <= 30)
                            otftTitle.Text += "÷";
                        break;
                    }
                case "Icon - R* Logo #2":
                    {
                        if (otftTitle.Text.Length <= 30)
                            otftTitle.Text += "∑";
                        break;
                    }
                case "Icon - Wanted Star":
                    {
                        if (otftTitle.Text.Length <= 27)
                            otftTitle.Text += "~ws~";
                        break;
                    }
                case "Icon - Lock":
                    {
                        if (otftTitle.Text.Length <= 30)
                            otftTitle.Text += "Ω";
                        break;
                    }
                case "Size - Small":
                    {
                        otftTitle.Text = "~s~<font size=\"10\">Text";
                        break;
                    }
                case "Size - Large":
                    {
                        otftTitle.Text = "~s~<font size=\"50\">Text";
                        break;
                    }
                case "Size - Huge":
                    {
                        otftTitle.Text = "~s~<font size=\"200\">Text";
                        break;
                    }
                case "Color - RGB":
                    {
                        otftTitle.Text = "~s~<font color=\"#FF0000\">Text";
                        break;
                    }
                case "Color - Blue":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~b~";
                        break;
                    }
                case "Color - Gold":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~y~";
                        break;
                    }
                case "Color - Green":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~g~";
                        break;
                    }
                case "Color - Light Blue":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~f~";
                        break;
                    }
                case "Color - Orange":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~o~";
                        break;
                    }
                case "Color - Purple":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~p~";
                        break;
                    }
                case "Color - Red":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~r~";
                        break;
                    }
                case "Color - Teal":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~d~";
                        break;
                    }
                case "Style - Bold":
                    {
                        if (otftTitle.Text.Length <= 28)
                            otftTitle.Text += "~h~";
                        break;
                    }
                case "Style - Italic":
                    {
                        if (otftTitle.Text.Length <= 23)
                            otftTitle.Text += "~italic~";
                        break;
                    }
            }
            otftMod.SelectedIndex = -1;
        }

        private void gDeductTotal_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.NETWORK_SPENT_CASH_DROP, Convert.ToInt32(gDeductTotal.Text));
        }

        private void gSnacks_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string[] statsSnacks = new string[] { "NO_BOUGHT_YUM_SNACKS", "NO_BOUGHT_HEALTH_SNACKS", "NO_BOUGHT_EPIC_SNACKS", "NUMBER_OF_ORANGE_BOUGHT", "NUMBER_OF_BOURGE_BOUGHT" };
            foreach (string snack in statsSnacks)
            {
                setStat(snack, Convert.ToInt32(gSnacks.Text));
            }
        }

        private void gArmor_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string[] statsArmor = new string[] { "MP_CHAR_ARMOUR_1_COUNT", "MP_CHAR_ARMOUR_2_COUNT", "MP_CHAR_ARMOUR_3_COUNT", "MP_CHAR_ARMOUR_4_COUNT", "MP_CHAR_ARMOUR_5_COUNT" };
            foreach (string armor in statsArmor)
            {
                setStat(armor, Convert.ToInt32(gArmor.Text));
            }
        }

        private void gFireworks_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string[] statsFireworks = new string[] { "FIREWORK_TYPE_1_WHITE", "FIREWORK_TYPE_1_RED", "FIREWORK_TYPE_1_BLUE", "FIREWORK_TYPE_2_WHITE", "FIREWORK_TYPE_2_RED", "FIREWORK_TYPE_2_BLUE", "FIREWORK_TYPE_3_WHITE", "FIREWORK_TYPE_3_RED", "FIREWORK_TYPE_3_BLUE", "FIREWORK_TYPE_4_WHITE", "FIREWORK_TYPE_4_RED", "FIREWORK_TYPE_4_BLUE" };
            foreach (string firework in statsFireworks)
            {
                setStat(firework, Convert.ToInt32(gFireworks.Text));
            }
        }
        private void advert_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://lexicongta.com/order");
        }
        private void garModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var query = (from item in VehicleModels
                         where item.Key == garModel.Text
                         select new { item.Value }).SingleOrDefault();
            uint model = Hash(query.Value);
            Garage.setUint(garListing.SelectedIndex, Garage.Model, model);
            Garage.resetSlot(garListing.SelectedIndex);
            refreshGarage();
        }

        private void garListing_Refresh_Click(object sender, EventArgs e)
        {
            refreshGarage();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < 13; i++)
            {
                Garage.setByte(i, Garage.Paint_Primary, Convert.ToByte(120));
                Garage.resetSlot(i);
            }
        }

        private void garListing_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshGarageControls();
        }

        private void garRefresh_Click(object sender, EventArgs e)
        {
            refreshGarageControls();
        }

        private void garPlateText_EditValueChanged(object sender, EventArgs e)
        {
            if (!garUpdating && garPlateText.Text.Length <= 8)
            {
                Garage.setString(garListing.SelectedIndex, Garage.Plate_Text, garPlateText.Text);
                Garage.resetSlot(garListing.SelectedIndex);
            }
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            Garage.setByte(garListing.SelectedIndex, Garage.Paint_Primary, Convert.ToByte(120));
            Garage.resetSlot(garListing.SelectedIndex);
        }

        private void garRGB_Prim_EditValueChanged(object sender, EventArgs e)
        {
            if (!garUpdating)
            {
                int i = garListing.SelectedIndex;
                Garage.setUint(i, Garage.RGB_Cache_R, garRGB.Color.R);
                Garage.setUint(i, Garage.RGB_Cache_G, garRGB.Color.G);
                Garage.setUint(i, Garage.RGB_Cache_B, garRGB.Color.B);
            }
        }

        private void garRGB_Prim_Set_Click(object sender, EventArgs e)
        {
            int i = garListing.SelectedIndex;
            Garage.setUint(i, Garage.RGB, Garage.getUint(i, Garage.RGB) | Garage.RGB_Primary);
            Garage.resetSlot(i);
        }

        private void garRGB_Prim_Clear_Click(object sender, EventArgs e)
        {
            int i = garListing.SelectedIndex;
            Garage.setUint(i, Garage.RGB, Garage.getUint(i, Garage.RGB) & (0xFFFFFFFF ^ Garage.RGB_Primary));
            Garage.resetSlot(i);
        }
        private void garRGB_Sec_Set_Click(object sender, EventArgs e)
        {
            int i = garListing.SelectedIndex;
            Garage.setUint(i, Garage.RGB, Garage.getUint(i, Garage.RGB) | Garage.RGB_Secondary);
            Garage.resetSlot(i);
        }

        private void garRGB_Sec_Clear_Click(object sender, EventArgs e)
        {
            int i = garListing.SelectedIndex;
            Garage.setUint(i, Garage.RGB, Garage.getUint(i, Garage.RGB) & (0xFFFFFFFF ^ Garage.RGB_Secondary));
            Garage.resetSlot(i);
        }

        private void simpleButton2_Click_2(object sender, EventArgs e)
        {
            // Old Date
            uint old_date = DateStruct_2_Memory(2015, 12, 24, 0, 0, 0, 0);
            bool _1 = RPC.Call(Natives.STAT_SET_DATE, Main.Hash("MP0_CHAR_DATE_CREATED"), old_date, 7, 1) == 1;
            bool _2 = RPC.Call(Natives.STAT_SET_DATE, Main.Hash("MPPLY_STARTED_MP"), old_date, 1) == 1;

            // Recent Date
            uint new_date = DateStruct_2_Memory(2016, 8, 11, 6, 34, 54, 23);
            bool _3 = RPC.Call(Natives.STAT_SET_DATE, Main.Hash("MP0_CHAR_DATE_RANKUP"), new_date, 7, 1) == 1;

            // Posix/Unix Timestamp
            int timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(2015, 12, 24))).TotalSeconds;
            bool _4 = RPC.Call(Natives.STAT_SET_INT, Main.Hash("MP0_CLOUD_TIME_CHAR_CREATED"), timestamp, 1) == 1;
            bool _5 = RPC.Call(Natives.STAT_SET_INT, Main.Hash("MP0_PS_TIME_CHAR_CREATED"), timestamp, 1) == 1;

            // Duration
            int duration = Convert.ToInt32((8/*D*/ * 86400000) + (12/*H*/ * 3600000) + (54/*M*/ * 60000) + (6/*S*/ * 1000));
            bool _6 = RPC.Call(Natives.STAT_SET_INT, Main.Hash("MP0_TOTAL_PLAYING_TIME"), duration, 1) == 1;
            bool _7 = RPC.Call(Natives.STAT_SET_INT, Main.Hash("MPPLY_TOTAL_TIME_SPENT_FREEMODE"), duration, 1) == 1;
            bool _8 = RPC.Call(Natives.STAT_SET_INT, Main.Hash("LEADERBOARD_PLAYING_TIME"), duration, 1) == 1;

            // Output
            MessageBox.Show(
                "#1 " + (_1 ? "Worked\n" : "Failed\n") +
                "#2 " + (_2 ? "Worked\n" : "Failed\n") +
                "#3 " + (_3 ? "Worked\n" : "Failed\n") +
                "#4 " + (_4 ? "Worked\n" : "Failed\n") +
                "#5 " + (_5 ? "Worked\n" : "Failed\n") +
                "#6 " + (_6 ? "Worked\n" : "Failed\n") +
                "#7 " + (_7 ? "Worked\n" : "Failed\n") +
                "#8 " + (_8 ? "Worked\n" : "Failed\n") +
                "");
        }

        private void dbgrDate_V_Do_Click(object sender, EventArgs e)
        {
            dbgrDate_V_Response.Text = RPC.Call(Natives.STAT_GET_DATE, Main.Hash(dbgrDate_V_Stat.Text), 0x10030000, 7, 3) == 1 ? "Stat Query Successful!" : "Stat Query Failed...";

            dbgrDate_V_Year.Text = PS3.Extension.ReadInt32(0x10030000).ToString();
            dbgrDate_V_Month.Text = PS3.Extension.ReadInt32(0x10030000 + 4).ToString();
            dbgrDate_V_Day.Text = PS3.Extension.ReadInt32(0x10030000 + 8).ToString();
            dbgrDate_V_Hour.Text = PS3.Extension.ReadInt32(0x10030000 + 12).ToString();
            dbgrDate_V_Minute.Text = PS3.Extension.ReadInt32(0x10030000 + 16).ToString();
            dbgrDate_V_Second.Text = PS3.Extension.ReadInt32(0x10030000 + 20).ToString();
        }

        private void dbgrDate_E_Do_Click(object sender, EventArgs e)
        {
            uint date = DateStruct_2_Memory(
                Convert.ToInt32(dbgrDate_E_Year.Text),
                Convert.ToInt32(dbgrDate_E_Month.Text),
                Convert.ToInt32(dbgrDate_E_Day.Text),
                Convert.ToInt32(dbgrDate_E_Hour.Text),
                Convert.ToInt32(dbgrDate_E_Minute.Text),
                Convert.ToInt32(dbgrDate_E_Second.Text),
                0
                );
            dbgrDate_E_Response.Text = RPC.Call(Natives.STAT_SET_DATE, Main.Hash(dbgrDate_E_Stat.Text), date, 7, 1) == 1 ? "Stat Query Successful!" : "Stat Query Failed...";
        }

        private void dbgrPos_V_Do_Click(object sender, EventArgs e)
        {
            dbgrPos_V_Response.Text = RPC.Call(Natives.STAT_GET_POS, Main.Hash(dbgrPos_V_Stat.Text), 0x10030000, 0x10030000 + 4, 0x10030000 + 8, 0) == 1 ? "Stat Query Successful!" : "Stat Query Failed...";
            dbgrPos_V_X.Text = PS3.Extension.ReadFloat(0x10030000).ToString();
            dbgrPos_V_Y.Text = PS3.Extension.ReadFloat(0x10030000 + 4).ToString();
            dbgrPos_V_Z.Text = PS3.Extension.ReadFloat(0x10030000 + 8).ToString();
        }

        private void dbgrPos_E_Do_Click(object sender, EventArgs e)
        {
            dbgrPos_E_Response.Text = RPC.Call(Natives.STAT_SET_POS, Main.Hash(dbgrPos_E_Stat.Text), float.Parse(dbgrPos_E_X.Text), float.Parse(dbgrPos_E_Y.Text), float.Parse(dbgrPos_E_Z.Text), 1) == 1 ? "Stat Query Successful!" : "Stat Query Failed...";
        }

        private void INS_File_SelectedIndexChanged(object sender, EventArgs e)
        {
            xd_mp.Load("Data/StatFiles/" + INS_File.Text);
            INS_Stat.Properties.Items.Clear();
            foreach (XmlNode xmlNode in xd_mp.SelectNodes("StatsSetup/stats/stat/@Name"))
                INS_Stat.Properties.Items.Add((object)xmlNode.Value);
            INS_Stat.SelectedIndex = 0;
        }
        private void INS_Stat_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 0;
            foreach (XmlNode xmlNode in xd_mp.SelectNodes("StatsSetup/stats/stat/@Name"))
            {
                if (xmlNode.Value == INS_Stat.Text)
                {
                    string deflabel = "Unavailable";

                    try { INS_Type.Text = Cap1(xd_mp.SelectNodes("StatsSetup/stats/stat/@Type")[index].Value); }
                    catch { INS_Type.Text = deflabel; }
                    try { if (xd_mp.SelectNodes("StatsSetup/stats/stat/@SaveCategory")[index].Value == "1") INS_Save.Text = "True"; else INS_Save.Text = "False"; }
                    catch { INS_Save.Text = deflabel; }
                    try { INS_Online.Text = Cap1(xd_mp.SelectNodes("StatsSetup/stats/stat/@online")[index].Value); }
                    catch { INS_Online.Text = deflabel; }
                    try { INS_Profile.Text = Cap1(xd_mp.SelectNodes("StatsSetup/stats/stat/@profile")[index].Value); }
                    catch { INS_Profile.Text = deflabel; }
                    try { INS_Owner.Text = Cap1(xd_mp.SelectNodes("StatsSetup/stats/stat/@Owner")[index].Value); }
                    catch { INS_Owner.Text = deflabel; }
                    try { INS_Character.Text = Cap1(xd_mp.SelectNodes("StatsSetup/stats/stat/@characterStat")[index].Value); }
                    catch { INS_Character.Text = deflabel; }

                    try { INS_ServerAuthoritative.Text = Cap1(xd_mp.SelectNodes("StatsSetup/stats/stat/@ServerAuthoritative")[index].Value); }
                    catch { INS_ServerAuthoritative.Text = deflabel; }
                    try { INS_FlushPriority.Text = xd_mp.SelectNodes("StatsSetup/stats/stat/@FlushPriority")[index].Value; }
                    catch { INS_FlushPriority.Text = deflabel; }
                    try { INS_UserData.Text = xd_mp.SelectNodes("StatsSetup/stats/stat/@UserData")[index].Value; }
                    catch { INS_UserData.Text = deflabel; }
                    try { INS_Default.Text = xd_mp.SelectNodes("StatsSetup/stats/stat/@Default")[index].Value; }
                    catch { INS_Default.Text = deflabel; }
                    try { INS_Min.Text = xd_mp.SelectNodes("StatsSetup/stats/stat/@Min")[index].Value; }
                    catch { INS_Min.Text = deflabel; }
                    try { INS_Max.Text = xd_mp.SelectNodes("StatsSetup/stats/stat/@Max")[index].Value; }
                    catch { INS_Max.Text = deflabel; }

                    try { INS_Comment.Text = xd_mp.SelectNodes("StatsSetup/stats/stat/@Comment")[index].Value; }
                    catch { INS_Comment.Text = deflabel; }
                }
                ++index;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            setStat("CHAR_WEAP_VIEWED", -1);
            setStat("CHAR_WEAP_VIEWED2", -1);
            setStat("CHAR_WEAP_VIEWED3", -1);
            setStat("CHAR_WEAP_ADDON_1_VIEWED", -1);
            setStat("CHAR_WEAP_ADDON_2_VIEWED", -1);
            setStat("CHAR_WEAP_ADDON_3_VIEWED", -1);
            setStat("CHAR_WEAP_ADDON_4_VIEWED", -1);
            setStat("CHAR_WEAP_ADDON_5_VIEWED", -1);
            setStat("CHAR_WEAP_ADDON_6_VIEWED", -1);
            setStat("CHAR_KIT_1_FM_VIEWED", -1);
            setStat("CHAR_KIT_2_FM_VIEWED", -1);
            setStat("CHAR_KIT_3_FM_VIEWED", -1);
            setStat("CHAR_KIT_4_FM_VIEWED", -1);
            setStat("CHAR_KIT_5_FM_VIEWED", -1);
            setStat("CHAR_KIT_6_FM_VIEWED", -1);
            setStat("CHAR_KIT_7_FM_VIEWED", -1);
            setStat("CHAR_KIT_8_FM_VIEWED", -1);
            setStat("CHAR_KIT_9_FM_VIEWED", -1);
            setStat("CHAR_KIT_10_FM_VIEWED", -1);
            setStat("CHAR_KIT_11_FM_VIEWED", -1);
            setStat("CHAR_KIT_12_FM_VIEWED", -1);
            setStat("CHAR_KIT_13_FM_VIEWED", -1);
            setStat("CHAR_KIT_14_FM_VIEWED", -1);
            setStat("CHAR_KIT_15_FM_VIEWED", -1);
            setStat("CHAR_KIT_16_FM_VIEWED", -1);
            setStat("TATTOO_FM_VIEWED_0", -1);
            setStat("TATTOO_FM_VIEWED_1", -1);
            setStat("TATTOO_FM_VIEWED_2", -1);
            setStat("TATTOO_FM_VIEWED_3", -1);
            setStat("TATTOO_FM_VIEWED_4", -1);
            setStat("TATTOO_FM_VIEWED_5", -1);
            setStat("TATTOO_FM_VIEWED_6", -1);
            setStat("TATTOO_FM_VIEWED_7", -1);
            setStat("TATTOO_FM_VIEWED_8", -1);
            setStat("TATTOO_FM_VIEWED_9", -1);
            setStat("TATTOO_FM_VIEWED_10", -1);
            setStat("TATTOO_FM_VIEWED_11", -1);
            setStat("TATTOO_FM_VIEWED_12", -1);
            setStat("CHAR_CARMODWHEELS_VIEWED_0", -1);
            setStat("CHAR_CARMODWHEELS_VIEWED_1", -1);
            setStat("CHAR_CARMODWHEELS_VIEWED_2", -1);
            setStat("CHAR_CARMODWHEELS_VIEWED_3", -1);
            setStat("CHAR_CARMODWHEELS_VIEWED_4", -1);
            setStat("CHAR_CARMODWHEELS_VIEWED_5", -1);
            setStat("CHAR_CARMODWHEELS_VIEWED_6", -1);
            setStat("CHAR_CARMODWHEELS_VIEWED_7", -1);
            setStat("CHAR_CARMODWHCOL_VIEWED_0", -1);
            setStat("CHAR_CARMODWHCOL_VIEWED_1", -1);
            setStat("CHAR_CARPAINTPRIME_VIEW_0", -1);
            setStat("CHAR_CARPAINTPRIME_VIEW_1", -1);
            setStat("CHAR_CARPAINTPRIME_VIEW_2", -1);
            setStat("CHAR_CARPAINTPRIME_VIEW_3", -1);
            setStat("CHAR_CARPAINTPRIME_VIEW_4", -1);
            setStat("CHAR_CARPAINTPRIME_VIEW_5", -1);
            setStat("CHAR_CARPAINTPRIME_VIEW_6", -1);
            setStat("CHAR_CARPAINTPRIME_VIEW_7", -1);
            setStat("CHAR_CARPAINTSEC_VIEW_0", -1);
            setStat("CHAR_CARPAINTSEC_VIEW_1", -1);
            setStat("CHAR_CARPAINTSEC_VIEW_2", -1);
            setStat("CHAR_CARPAINTSEC_VIEW_3", -1);
            setStat("CHAR_CARPAINTSEC_VIEW_4", -1);
            setStat("CHAR_CARPAINTSEC_VIEW_5", -1);
            setStat("CHAR_CARPAINTSEC_VIEW_6", -1);
            setStat("CHAR_CARPAINTSEC_VIEW_7", -1);
            setStat("CREW_EMBLEMS_PURCHASED", -1);
            setStat("CHAR_HAIR_VIEWED1", -1);
            setStat("CHAR_HAIR_VIEWED2", -1);
            setStat("CHAR_HAIR_VIEWED3", -1);
            setStat("CHAR_HAIR_VIEWED4", -1);
            setStat("CHAR_HAIR_VIEWED5", -1);
            setStat("CHAR_HAIR_VIEWED6", -1);
            setStat("CHAR_HAIR_VIEWED7", -1);
            setStat("CHAR_HAIR_VIEWED8", -1);
            setStat("CHAR_HAIR_VIEWED9", -1);
            setStat("CHAR_HAIR_VIEWED10", -1);
            setStat("CHAR_HAIR_VIEWED11", -1);
            setStat("CHAR_HAIR_VIEWED12", -1);
        }
        private void timeDur_Save_Click(object sender, EventArgs e)
        {
            try
            {
                RPC.Call(Natives.STAT_SET_INT, Main.Hash(formatStat(timeDur_Stat.Text)),
                    (Convert.ToUInt32(timeDur_Day.Text) * 86400000) +
                    (Convert.ToUInt32(timeDur_Hour.Text) * 3600000) +
                    (Convert.ToUInt32(timeDur_Minute.Text) * 60000) +
                    (Convert.ToUInt32(timeDur_Second.Text) * 1000), 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void timeDate_Save_Click(object sender, EventArgs e)
        {
            uint new_date = DateStruct_2_Memory(
                (int)timeDate_Year.Value, (int)timeDate_Month.Value, (int)timeDate_Day.Value, 
                (int)timeDate_Hour.Value, (int)timeDate_Minute.Value, (int)timeDate_Second.Value, 0);
            RPC.Call(Natives.STAT_SET_DATE, Main.Hash(formatStat(timeDate_Stat.Text)), new_date, 7, 1);
        }

        private void TMB_Do_Click(object sender, EventArgs e)
        {
            // I'm too lazy to properly cast it so I'm just throwing garbage in that works as int32.
            Dictionary<string, int> Values = new Dictionary<string, int>()
            {
                { "5 Billion", 2 },
                { "10 Billion", 3 },
                { "15 Billion", 4 },
                { "20 Billion", 6 },
                { "25 Billion", 7 },
                { "30 Billion", 8 },
                { "35 Billion", 9 },
                { "40 Billion", 10 },
                { "45 Billion", 11 },
                { "50 Billion", 13 },
                { "55 Billion", 14 },
                { "60 Billion", 15 },
                { "65 Billion", 16 },
                { "70 Billion", 17 },
                { "75 Billion", 18 },
                { "80 Billion", 20 },
                { "85 Billion", 21 },
                { "90 Billion", 22 },
                { "95 Billion", 23 },
                { "100 Billion", 24 },
                { "110 Billion", 27 },
                { "115 Billion", 28 },
                { "120 Billion", 29 },
                { "125 Billion", 30 },
                { "130 Billion", 31 },
                { "135 Billion", 32 },
                { "140 Billion", 34 },
                { "145 Billion", 35 },
                { "150 Billion", 36 },
                { "155 Billion", 37 },
                { "160 Billion", 38 },
                { "165 Billion", 39 },
                { "170 Billion", 41 },
                { "175 Billion", 42 },
                { "180 Billion", 43 },
                { "185 Billion", 44 },
                { "190 Billion", 45 },
                { "195 Billion", 46 },
                { "200 Billion", 48 },
                { "205 Billion", 49 },
                { "210 Billion", 50 },
                { "215 Billion", 51 },
                { "220 Billion", 52 },
                { "225 Billion", 53 },
                { "230 Billion", 55 },
                { "235 Billion", 56 },
                { "240 Billion", 57 },
                { "245 Billion", 58 },
                { "250 Billion", 59 },
                { "255 Billion", 61 },
                { "260 Billion", 62 },
                { "265 Billion", 63 },
                { "270 Billion", 64 },
                { "275 Billion", 65 },
                { "280 Billion", 66 },
                { "285 Billion", 67 },
                { "290 Billion", 69 },
                { "295 Billion", 70 },
                { "300 Billion", 71 },
                { "305 Billion", 72 },
                { "310 Billion", 73 },
                { "315 Billion", 74 },
                { "320 Billion", 76 },
                { "325 Billion", 77 },
                { "330 Billion", 78 },
                { "335 Billion", 79 },
                { "340 Billion", 80 },
                { "345 Billion", 81 },
                { "350 Billion", 83 },
                { "355 Billion", 84 },
                { "360 Billion", 85 },
                { "365 Billion", 86 },
                { "370 Billion", 87 },
                { "375 Billion", 88 },
                { "380 Billion", 90 },
                { "385 Billion", 91 },
                { "390 Billion", 92 },
                { "395 Billion", 93 },
                { "400 Billion", 94 },
                { "405 Billion", 95 },
                { "410 Billion", 96 },
                { "415 Billion", 98 },
                { "420 Billion", 99 },
                { "425 Billion", 100 },
                { "430 Billion", 101 },
                { "435 Billion", 102 },
                { "440 Billion", 103 },
                { "445 Billion", 105 },
                { "450 Billion", 106 },
                { "455 Billion", 107 },
                { "460 Billion", 108 },
                { "465 Billion", 119 },
                { "470 Billion", 110 },
                { "475 Billion", 112 },
                { "480 Billion", 113 },
                { "485 Billion", 114 },
                { "490 Billion", 115 },
                { "495 Billion", 116 },
                { "500 Billion", 117 },
            };
            RPC.Call(Natives.STAT_SET_INT, Hash("CASH_GIFT_NEW"), Values[TMB_Choice.Text], 1);
            DevExpress.XtraEditors.XtraMessageBox.Show("The value might be off a little bit...\n\n" +
            "--- FOR USE ON PS3 ---\n1. Leave GTA Online.\n2. Join a new session.\n\n" +
            "--- FOR TRANSFERRING ---\n1. Leave GTA Online.\n2. Transfer.\n(If you join a new session on PS3 after setting this, it won't transfer.)");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("SCADMIN_BADSPORT_START"), 0, 1);
            RPC.Call(Natives.STAT_SET_INT, Hash("SCADMIN_BADSPORT_END"), 0, 1);
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_BAD_SPORT_BITSET"), 0, 1);
            RPC.Call(Natives.STAT_SET_INT, Hash("MP1_BAD_SPORT_BITSET"), 0, 1);
            RPC.Call(Natives.STAT_SET_BOOL, Hash("MPPLY_WAS_I_BAD_SPORT"), 0, 1);
            RPC.Call(Natives.STAT_SET_FLOAT, Hash("MPPLY_OVERALL_BADSPORT"), 0, 1);
            RPC.Call(Natives.STAT_SET_BOOL, Hash("MPPLY_CHAR_IS_BADSPORT"), 0, 1);
            RPC.Call(Natives.STAT_SET_INT, Hash("MPPLY_BECAME_BADSPORT_NUM"), 0, 1);
            RPC.Call(Natives.STAT_SET_INT, Hash("MPPLY_BADSPORT_MESSAGE"), 0, 1);
            Tunables.escapeBadsport();
        }

        private void WS_Weapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_" + WS_Weapon.Text + "_ENEMY_KILLS"), 0x10030040)))
            {
                WS_KillsPlayer.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else WS_KillsPlayer.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_" + WS_Weapon.Text + "_KILLS"), 0x10030040)))
            {
                WS_KillsNPC.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else WS_KillsNPC.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_" + WS_Weapon.Text + "_DEATHS"), 0x10030040)))
            {
                WS_Deaths.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else WS_Deaths.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_" + WS_Weapon.Text + "_HITS"), 0x10030040)))
            {
                WS_Hits.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else WS_Hits.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_" + WS_Weapon.Text + "_SHOTS"), 0x10030040)))
            {
                WS_Shots.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else WS_Shots.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_" + WS_Weapon.Text + "_HEADSHOTS"), 0x10030040)))
            {
                WS_Headshots.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else WS_Headshots.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_" + WS_Weapon.Text + "_FM_AMMO_CURRENT"), 0x10030040)))
            {
                WS_Ammo.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else WS_Ammo.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_" + WS_Weapon.Text + "_FM_AMMO_BOUGHT"), 0x10030040)))
            {
                WS_AmmoBought.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else WS_AmmoBought.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_BOOL, Hash("MP0_" + WS_Weapon.Text + "_AQUIRED_AS_GIFT"), 0x10030040)))
            {
                WS_Gift.Checked = PS3.Extension.ReadBool(0x10030040);
            }
            else WS_Gift.Checked = false;
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_BOOL, Hash("MP0_" + WS_Weapon.Text + "_IN_POSSESSION"), 0x10030040)))
            {
                WS_Possession.Checked = PS3.Extension.ReadBool(0x10030040);
            }
            else WS_Possession.Checked = false;
        }

        private void WS_Save_Click(object sender, EventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_" + WS_Weapon.Text + "_ENEMY_KILLS"), Convert.ToInt32(WS_KillsPlayer.Text));
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_" + WS_Weapon.Text + "_KILLS"), Convert.ToInt32(WS_KillsNPC.Text));
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_" + WS_Weapon.Text + "_DEATHS"), Convert.ToInt32(WS_Deaths.Text));
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_" + WS_Weapon.Text + "_HITS"), Convert.ToInt32(WS_Hits.Text));
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_" + WS_Weapon.Text + "_SHOTS"), Convert.ToInt32(WS_Shots.Text));
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_" + WS_Weapon.Text + "_HEADSHOTS"), Convert.ToInt32(WS_Headshots.Text));
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_" + WS_Weapon.Text + "_FM_AMMO_CURRENT"), Convert.ToInt32(WS_Ammo.Text));
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_" + WS_Weapon.Text + "_FM_AMMO_BOUGHT"), Convert.ToInt32(WS_AmmoBought.Text));

            RPC.Call(Natives.STAT_SET_BOOL, Hash("MP0_" + WS_Weapon.Text + "_AQUIRED_AS_GIFT"), Convert.ToInt32(WS_Gift.Checked));
            RPC.Call(Natives.STAT_SET_BOOL, Hash("MP0_" + WS_Weapon.Text + "_IN_POSSESSION"), Convert.ToInt32(WS_Possession.Checked));
        }

        private void CMBT_Kills_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_KILLS"), Convert.ToInt32(CMBT_Kills.Text));
        }

        private void CMBT_KillsPlayers_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_KILLS_PLAYERS"), Convert.ToInt32(CMBT_KillsPlayers.Text));
        }

        private void CMBT_Deaths_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_DEATHS"), Convert.ToInt32(CMBT_Deaths.Text));
        }

        private void CMBT_DeathsPlayer_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_DEATHS_PLAYER"), Convert.ToInt32(CMBT_DeathsPlayer.Text));
        }

        private void CMBT_Shots_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_SHOTS"), Convert.ToInt32(CMBT_Shots.Text));
        }

        private void CMBT_Accuracy_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_FLOAT, Hash("MP0_WEAPON_ACCURACY"), float.Parse(CMBT_Accuracy.Text));
        }

        private void CMBT_Hits_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_HITS"), Convert.ToInt32(CMBT_Hits.Text));
        }

        private void CMBT_Headshots_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_HEADSHOTS"), Convert.ToInt32(CMBT_Headshots.Text));
        }

        private void CMBT_HeadshotsPlayers_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_PLAYER_HEADSHOTS"), Convert.ToInt32(CMBT_HeadshotsPlayers.Text));
        }

        private void CMBT_BountPlaced_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_BOUNTPLACED"), Convert.ToInt32(CMBT_BountPlaced.Text));
        }

        private void CMBT_BountOn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            RPC.Call(Natives.STAT_SET_INT, Hash("MP0_BOUNTSONU"), Convert.ToInt32(CMBT_BountOn.Text));
        }

        private void CMBT_Load_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_KILLS"), 0x10030040)))
            {
                CMBT_Kills.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_Kills.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_KILLS_PLAYERS"), 0x10030040)))
            {
                CMBT_KillsPlayers.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_KillsPlayers.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_DEATHS"), 0x10030040)))
            {
                CMBT_Deaths.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_Deaths.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_DEATHS_PLAYER"), 0x10030040)))
            {
                CMBT_DeathsPlayer.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_DeathsPlayer.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_SHOTS"), 0x10030040)))
            {
                CMBT_Shots.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_Shots.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_FLOAT, Hash("MP0_WEAPON_ACCURACY"), 0x10030040)))
            {
                CMBT_Accuracy.Text = PS3.Extension.ReadFloat(0x10030040).ToString();
            }
            else CMBT_Accuracy.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_HITS"), 0x10030040)))
            {
                CMBT_Hits.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_Hits.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_HEADSHOTS"), 0x10030040)))
            {
                CMBT_Headshots.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_Headshots.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_PLAYER_HEADSHOTS"), 0x10030040)))
            {
                CMBT_HeadshotsPlayers.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_HeadshotsPlayers.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_BOUNTPLACED"), 0x10030040)))
            {
                CMBT_BountPlaced.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_BountPlaced.Text = "0";
            if (Convert.ToBoolean(RPC.Call(Natives.STAT_GET_INT, Hash("MP0_BOUNTSONU"), 0x10030040)))
            {
                CMBT_BountOn.Text = PS3.Extension.ReadInt32(0x10030040).ToString();
            }
            else CMBT_BountOn.Text = "0";
        }
        #endregion

    }
}
