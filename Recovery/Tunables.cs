using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;

namespace Recovery
{

    class Tunables
    {
        public static PS3API PS3 = new PS3API();
        public static PS3API API = new PS3API();
        public static uint PTR_TUNABLES = 0x1E70374; // 1.26 // BLES
        public enum Indices : uint
        {
            // 1.26
            AMOUNT_TO_FORGIVE_BADSPORT_BY = 82,
            BADSPORT_RESET_MINUTES = 81,
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
        public static void freeShopping(bool toggle) 
        {
            for (Indices i = Indices.SHOPPING_START; i < Indices.SHOPPING_END; i++)
            {
                setTunable(i, toggle ? 0 : 0x3F800000); 
            }
        }
        
    }
}
