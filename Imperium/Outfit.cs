using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;

namespace Imperium
{
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
        public static PS3API PS3 = new PS3API();
        public static PS3API API = new PS3API();
        public static uint pointer = 0x02223518;
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
                PS3.ConnectTarget();
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
            PS3.ConnectTarget();
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
            PS3.ConnectTarget();
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
}
