using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PS3Lib;
using Newtonsoft.Json;
using System.IO;

namespace Recovery
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        public static PS3API PS3 = new PS3API();
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
        private List<Setting> settingsList = new List<Setting>();
        public Main()
        {
            InitializeComponent();
            string[] Settings_Settings_content = File.ReadAllLines("Settings/Settings.json");
            foreach (string line in Settings_Settings_content)
            {
                //load contents into tunable object
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
        #region Stats
        public static Dictionary<string, object> Stats = new Dictionary<string, object>()
        {
            // { STAT, VALUE }, 
            { "ADMIN_CLOTHES_GV_BS_1", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_10", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_11", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_12", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_2", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_3", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_4", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_5", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_6", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_7", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_8", -1 }, // DLC Clothing
            { "ADMIN_CLOTHES_GV_BS_9", -1 }, // DLC Clothing
            { "ADMIN_WEAPON_GV_BS_1", -1 }, // Unlock Weapons
            { "ADVRIFLE_ENEMY_KILLS", 600 }, // Unlock Tints
            { "AIR_LAUNCHES_OVER_40M", 25 }, // Unlock All Achievements
            { "APPISTOL_ENEMY_KILLS", 600 }, // Unlock Tints
            { "ASLTMG_ENEMY_KILLS", 600 }, // Unlock Tints
            { "ASLTRIFLE_ENEMY_KILLS", 600 }, // Unlock Tints
            { "ASLTSHTGN_ENEMY_KILLS", 600 }, // Unlock Tints
            { "ASLTSMG_ENEMY_KILLS", 600 }, // Unlock Tints
            { "AWD_5STAR_WANTED_AVOIDANCE", 50 }, // Unlock All Achievements
            { "AWD_ACTIVATE_2_PERSON_KEY", true }, // Unlock All Achievements
            { "AWD_ALL_ROLES_HEIST", true }, // Unlock All Achievements
            { "AWD_BUY_EVERY_GUN", true }, // Unlock All Achievements
            { "AWD_CAR_BOMBS_ENEMY_KILLS", 25 }, // Unlock All Achievements
            { "AWD_CARS_EXPORTED", 50 }, // Unlock All Achievements
            { "AWD_CONTROL_CROWDS", 25 }, // Unlock All Achievements
            { "AWD_DAILYOBJCOMPLETED", 100 }, // Unlock All Achievements
            { "AWD_DAILYOBJMONTHBONUS", true }, // Unlock All Achievements
            { "AWD_DAILYOBJWEEKBONUS", true }, // Unlock All Achievements
            { "AWD_DO_HEIST_AS_MEMBER", 25 }, // Exclusive Shirt - For Hire
            { "AWD_DO_HEIST_AS_THE_LEADER", 25 }, // Exclusive Shirt - Shot Caller
            { "AWD_DRIVELESTERCAR5MINS", true }, // Unlock All Achievements
            { "AWD_DROPOFF_CAP_PACKAGES", 100 }, // Chrome Rims - Lowrider
            { "AWD_FINISH_HEIST_NO_DAMAGE", true }, // Exclusive Shirt - Can't Touch This
            { "AWD_FINISH_HEIST_SETUP_JOB", 50 }, // Chrome Rims - Tuner
            { "AWD_FINISH_HEISTS", 50 }, // Chrome Rims - High End
            { "AWD_FM25DIFFERENTDM", true }, // Unlock All Achievements
            { "AWD_FM25DIFFERENTRACES", true }, // Unlock All Achievements
            { "AWD_FM25DIFITEMSCLOTHES", true }, // Unlock All Achievements
            { "AWD_FM6DARTCHKOUT", true }, // Unlock All Achievements
            { "AWD_FM_DM_3KILLSAMEGUY", 50 }, // Unlock All Achievements
            { "AWD_FM_DM_KILLSTREAK", 100 }, // Unlock All Achievements
            { "AWD_FM_DM_STOLENKILL", 50 }, // Unlock All Achievements
            { "AWD_FM_DM_TOTALKILLS", 500 }, // Unlock Tattoos
            { "AWD_FM_DM_WINS", 50 }, // Unlock All Achievements
            { "AWD_FM_GOLF_BIRDIES", 25 }, // Unlock All Achievements
            { "AWD_FM_GOLF_HOLE_IN_1", true }, // Unlock All Achievements
            { "AWD_FM_GOLF_WON", 25 }, // Unlock All Achievements
            { "AWD_FM_GTA_RACES_WON", 50 }, // Unlock All Achievements
            { "AWD_FM_RACE_LAST_FIRST", 25 }, // Unlock All Achievements
            { "AWD_FM_RACES_FASTEST_LAP", 50 }, // Vehicle Unlocks
            { "AWD_FM_SHOOTRANG_CT_WON", 25 }, // Unlock All Achievements
            { "AWD_FM_SHOOTRANG_GRAN_WON", true }, // Unlock All Achievements
            { "AWD_FM_SHOOTRANG_RT_WON", 25 }, // Unlock All Achievements
            { "AWD_FM_SHOOTRANG_TG_WON", 25 }, // Unlock All Achievements
            { "AWD_FM_TDM_MVP", 50 }, // Unlock All Achievements
            { "AWD_FM_TDM_WINS", 50 }, // Unlock All Achievements
            { "AWD_FM_TENNIS_5_SET_WINS", true }, // Unlock All Achievements
            { "AWD_FM_TENNIS_ACE", 25 }, // Unlock All Achievements
            { "AWD_FM_TENNIS_STASETWIN", true }, // Unlock All Achievements
            { "AWD_FM_TENNIS_WON", 25 }, // Unlock All Achievements
            { "AWD_FMATTGANGHQ", true }, // Unlock All Achievements
            { "AWD_FMBASEJMP", 25 }, // Unlock All Achievements
            { "AWD_FMBBETWIN", 50000 }, // Unlock All Achievements
            { "AWD_FMCRATEDROPS", 25 }, // Unlock All Achievements
            { "AWD_FMDRIVEWITHOUTCRASH", 30 }, // Unlock All Achievements
            { "AWD_FMFULLYMODDEDCAR", true }, // Unlock All Achievements
            { "AWD_FMHORDWAVESSURVIVE", 10 }, // Exclusive Shirt - Red Skull
            { "AWD_FMKILL3ANDWINGTARACE", true }, // Unlock All Achievements
            { "AWD_FMKILLBOUNTY", 25 }, // Unlock All Achievements
            { "AWD_FMKILLSTREAKSDM", true }, // Unlock All Achievements
            { "AWD_FMMOSTKILLSGANGHIDE", true }, // Unlock All Achievements
            { "AWD_FMMOSTKILLSSURVIVE", true }, // Unlock All Achievements
            { "AWD_FMPICKUPDLCCRATE1ST", true }, // Unlock All Achievements
            { "AWD_FMRACEWORLDRECHOLDER", true }, // Unlock All Achievements
            { "AWD_FMRALLYWONDRIVE", 25 }, // Unlock All Achievements
            { "AWD_FMRALLYWONNAV", 25 }, // Unlock All Achievements
            { "AWD_FMREVENGEKILLSDM", 50 }, // Unlock Tattoos
            { "AWD_FMSHOOTDOWNCOPHELI", 25 }, // Unlock All Achievements
            { "AWD_FMTATTOOALLBODYPARTS", true }, // Unlock All Achievements
            { "AWD_FMWINAIRRACE", 25 }, // Unlock All Achievements
            { "AWD_FMWINALLRACEMODES", true }, // Unlock All Achievements
            { "AWD_FMWINCUSTOMRACE", true }, // Unlock All Achievements
            { "AWD_FMWINEVERYGAMEMODE", true }, // Unlock All Achievements
            { "AWD_FMWINRACETOPOINTS", 25 }, // Unlock All Achievements
            { "AWD_FMWINSEARACE", 25 }, // Unlock All Achievements
            { "AWD_HOLD_UP_SHOPS", 20 }, // Unlock All Achievements
            { "AWD_KILL_CARRIER_CAPTURE", 100 }, // Chrome Rims - Offroad
            { "AWD_KILL_PSYCHOPATHS", 100 }, // Exclusive Shirt - Psycho
            { "AWD_KILL_TEAM_YOURSELF_LTS", 25 }, // Exclusive Shirt - One Man Army
            { "AWD_LAPDANCES", 25 }, // Unlock All Achievements
            { "AWD_LESTERDELIVERVEHICLES", 25 }, // Unlock All Achievements
            { "AWD_MENTALSTATE_TO_NORMAL", 50 }, // Unlock All Achievements
            { "AWD_NIGHTVISION_KILLS", 100 }, // Chrome Rims - Bike
            { "AWD_NO_HAIRCUTS", 25 }, // Unlock All Achievements
            { "AWD_ODISTRACTCOPSNOEATH", 25 }, // Unlock All Achievements
            { "AWD_ONLY_PLAYER_ALIVE_LTS", 50 }, // Chrome Rims - Muscle
            { "AWD_PARACHUTE_JUMPS_20M", 25 }, // Unlock All Achievements
            { "AWD_PARACHUTE_JUMPS_50M", 25 }, // Unlock All Achievements
            { "AWD_PASSENGERTIME", 4 }, // Unlock All Achievements
            { "AWD_PICKUP_CAP_PACKAGES", 100 }, // Exclusive Shirt - Gimme That
            { "AWD_RACES_WON", 50 }, // Unlock All Achievements
            { "AWD_SECURITY_CARS_ROBBED", 25 }, // Unlock All Achievements
            { "AWD_SPLIT_HEIST_TAKE_EVENLY", true }, // Unlock All Achievements
            { "AWD_STORE_20_CAR_IN_GARAGES", true }, // Exclusive Shirt - Showroom
            { "AWD_TAKEDOWNSMUGPLANE", 50 }, // Unlock All Achievements
            { "AWD_TIME_IN_HELICOPTER", 4 }, // Unlock All Achievements
            { "AWD_TRADE_IN_YOUR_PROPERTY", 25 }, // Unlock All Achievements
            { "AWD_VEHICLES_JACKEDR", 500 }, // Unlock All Achievements
            { "AWD_WIN_AT_DARTS", 25 }, // Unlock All Achievements
            { "AWD_WIN_CAPTURE_DONT_DYING", 25 }, // Exclusive Shirt - Death Defying
            { "AWD_WIN_CAPTURES", 50 }, // Chrome Rims - Sport
            { "AWD_WIN_GOLD_MEDAL_HEISTS", 25 }, // Exclusive Shirt - Decorated
            { "AWD_WIN_LAST_TEAM_STANDINGS", 50 }, // Chrome Rims - SUV
            { "BOTTLE_IN_POSSESSION", -1 }, // Unlock Weapons
            { "CARS_EXPLODED", 500 }, // Unlock All Achievements
            { "CHAR_FM_CARMOD_1_UNLCK", -1 }, // Vehicle Unlocks
            { "CHAR_FM_CARMOD_2_UNLCK", -1 }, // Vehicle Unlocks
            { "CHAR_FM_CARMOD_3_UNLCK", -1 }, // Vehicle Unlocks
            { "CHAR_FM_CARMOD_4_UNLCK", -1 }, // Vehicle Unlocks
            { "CHAR_FM_CARMOD_5_UNLCK", -1 }, // Vehicle Unlocks
            { "CHAR_FM_CARMOD_6_UNLCK", -1 }, // Vehicle Unlocks
            { "CHAR_FM_CARMOD_7_UNLCK", -1 }, // Vehicle Unlocks
            { "CHAR_FM_VEHICLE_1_UNLCK", -1 }, // Unlock Heist Vehicles
            { "CHAR_FM_VEHICLE_2_UNLCK", -1 }, // Unlock Heist Vehicles
            { "CHAR_FM_WEAP_ADDON_1_UNLCK", -1 }, // Unlock Weapons
            { "CHAR_FM_WEAP_ADDON_2_UNLCK", -1 }, // Unlock Weapons
            { "CHAR_FM_WEAP_ADDON_3_UNLCK", -1 }, // Unlock Weapons
            { "CHAR_FM_WEAP_ADDON_4_UNLCK", -1 }, // Unlock Weapons
            { "CHAR_FM_WEAP_ADDON_5_UNLCK", -1 }, // Unlock Weapons
            { "CHAR_FM_WEAP_UNLOCKED", -1 }, // Unlock Weapons
            { "CHAR_FM_WEAP_UNLOCKED2", -1 }, // Unlock Weapons
            { "CHAR_KIT_10_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_11_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_12_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_1_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_2_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_3_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_4_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_5_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_6_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_7_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_8_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_9_FM_UNLCK", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE10", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE11", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE12", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE2", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE3", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE4", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE5", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE6", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE7", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE8", -1 }, // Unlock Kits
            { "CHAR_KIT_FM_PURCHASE9", -1 }, // Unlock Kits
            { "CHAR_WANTED_LEVEL_TIME5STAR", -1 }, // Unlock All Achievements
            { "CHAR_WEAP_FM_PURCHASE", -1 }, // Unlock Weapons
            { "CHAR_WEAP_FM_PURCHASE2", -1 }, // Unlock Weapons
            { "CLTHS_ACQUIRED_BERD", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_BERD_1", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_BERD_2", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_BERD_3", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_BERD_4", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_BERD_5", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_BERD_6", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_BERD_7", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_DECL", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_FEET", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_FEET_1", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_FEET_2", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_FEET_3", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_FEET_4", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_FEET_5", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_FEET_6", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_FEET_7", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_JBIB", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_JBIB_1", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_JBIB_2", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_JBIB_3", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_JBIB_4", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_JBIB_5", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_JBIB_6", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_JBIB_7", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_LEGS", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_LEGS_1", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_LEGS_2", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_LEGS_3", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_LEGS_4", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_LEGS_5", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_LEGS_6", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_LEGS_7", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_OUTFIT", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_1", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_10", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_2", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_3", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_4", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_5", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_6", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_7", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_8", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_PROPS_9", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL2", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL2_1", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL_1", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL_2", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL_3", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL_4", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL_5", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL_6", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_SPECIAL_7", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_TEETH", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_TEETH_1", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_TEETH_2", -1 }, // Unlock Clothing
            { "CLTHS_ACQUIRED_TORSO", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_BERD", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_BERD_1", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_BERD_2", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_BERD_3", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_BERD_4", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_BERD_5", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_BERD_6", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_BERD_7", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_DECL", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_FEET", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_FEET_1", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_FEET_2", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_FEET_3", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_FEET_4", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_FEET_5", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_FEET_6", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_FEET_7", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_HAIR", -1 }, // Unlock Hair
            { "CLTHS_AVAILABLE_HAIR_1", -1 }, // Unlock Hair
            { "CLTHS_AVAILABLE_HAIR_2", -1 }, // Unlock Hair
            { "CLTHS_AVAILABLE_HAIR_3", -1 }, // Unlock Hair
            { "CLTHS_AVAILABLE_HAIR_4", -1 }, // Unlock Hair
            { "CLTHS_AVAILABLE_HAIR_5", -1 }, // Unlock Hair
            { "CLTHS_AVAILABLE_HAIR_6", -1 }, // Unlock Hair
            { "CLTHS_AVAILABLE_HAIR_7", -1 }, // Unlock Hair
            { "CLTHS_AVAILABLE_JBIB", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_JBIB_1", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_JBIB_2", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_JBIB_3", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_JBIB_4", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_JBIB_5", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_JBIB_6", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_JBIB_7", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_LEGS", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_LEGS_1", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_LEGS_2", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_LEGS_3", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_LEGS_4", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_LEGS_5", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_LEGS_6", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_LEGS_7", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_OUTFIT", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_1", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_10", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_2", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_3", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_4", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_5", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_6", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_7", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_8", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_PROPS_9", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL2", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL2_1", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL_1", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL_2", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL_3", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL_4", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL_5", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL_6", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_SPECIAL_7", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_TEETH", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_TEETH_1", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_TEETH_2", -1 }, // Unlock Clothing
            { "CLTHS_AVAILABLE_TORSO", -1 }, // Unlock Clothing
            { "CMBTMG_ENEMY_KILLS", 600 }, // Unlock Tints
            { "CMBTPISTOL_ENEMY_KILLS", 600 }, // Unlock Tints
            { "CRBNRIFLE_ENEMY_KILLS", 600 }, // Unlock Tints
            { "DLC_APPAREL_ACQUIRED_0", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_1", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_10", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_11", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_12", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_13", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_14", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_15", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_16", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_17", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_18", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_19", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_2", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_20", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_21", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_22", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_23", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_24", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_25", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_26", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_27", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_28", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_29", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_3", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_30", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_31", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_32", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_33", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_34", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_35", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_36", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_37", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_38", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_39", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_4", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_40", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_5", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_6", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_7", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_8", -1 }, // Unlock Clothing
            { "DLC_APPAREL_ACQUIRED_9", -1 }, // Unlock Clothing
            { "FM_CHANGECHAR_ASKED", false }, // Character Redesign Prompt
            { "GRENADE_ENEMY_KILLS", 50 }, // Unlock All Achievements
            { "GRNLAUNCH_ENEMY_KILLS", 600 }, // Unlock Tints
            { "HVYSNIPER_ENEMY_KILLS", 600 }, // Unlock Tints
            { "KILLS_PLAYERS", 1000 }, // Unlock All Achievements
            { "LONGEST_WHEELIE_DIST", 1000f }, // Unlock All Achievements
            { "MG_ENEMY_KILLS", 600 }, // Unlock Tints
            { "MICROSMG_ENEMY_KILLS", 600 }, // Unlock Tints
            { "MINIGUNS_ENEMY_KILLS", 600 }, // Unlock Tints
            { "MOST_ARM_WRESTLING_WINS", 25 }, // Unlock All Achievements
            { "MOST_FLIPS_IN_ONE_JUMP", 5 }, // Unlock All Achievements
            { "MOST_SPINS_IN_ONE_JUMP", 5 }, // Unlock All Achievements
            { "MPPLY_AWD_COMPLET_HEIST_MEM", true }, // Unlock All Achievements
            { "MPPLY_AWD_FLEECA_FIN", true }, // Unlock All Achievements
            { "MPPLY_AWD_FM_CR_DM_MADE", 25 }, // Unlock All Achievements
            { "MPPLY_AWD_FM_CR_PLAYED_BY_PEEP", 100 }, // Unlock All Achievements
            { "MPPLY_AWD_FM_CR_RACES_MADE", 25 }, // Unlock All Achievements
            { "MPPLY_AWD_HST_ORDER", true }, // Unlock All Achievements
            { "MPPLY_AWD_HST_SAME_TEAM", true }, // Unlock All Achievements
            { "MPPLY_AWD_HST_ULT_CHAL", true }, // Unlock All Achievements
            { "MPPLY_AWD_HUMANE_FIN", true }, // Unlock All Achievements
            { "MPPLY_AWD_PACIFIC_FIN", true }, // Unlock All Achievements
            { "MPPLY_AWD_PRISON_FIN", true }, // Unlock All Achievements
            { "MPPLY_AWD_SERIESA_FIN", true }, // Unlock All Achievements
            { "MPPLY_BAD_CREW_EMBLEM", 0 }, // Clear Reports
            { "MPPLY_BAD_CREW_MOTTO", 0 }, // Clear Reports
            { "MPPLY_BAD_CREW_NAME", 0 }, // Clear Reports
            { "MPPLY_BAD_CREW_STATUS", 0 }, // Clear Reports
            { "MPPLY_COMMEND_STRENGTH", 100 }, // Clear Reports
            { "MPPLY_EXPLOITS", 0 }, // Clear Reports
            { "MPPLY_FRIENDLY", 100 }, // Clear Reports
            { "MPPLY_GAME_EXPLOITS", 0 }, // Clear Reports
            { "MPPLY_GRIEFING", 0 }, // Clear Reports
            { "MPPLY_HELPFUL", 100 }, // Clear Reports
            { "MPPLY_ISPUNISHED", 0 }, // Clear Reports
            { "MPPLY_NO_MORE_TUTORIALS", true }, // Tutorial Bypass
            { "MPPLY_OFFENSIVE_LANGUAGE", 0 }, // Clear Reports
            { "MPPLY_OFFENSIVE_TAGPLATE", 0 }, // Clear Reports
            { "MPPLY_OFFENSIVE_UGC", 0 }, // Clear Reports
            { "MPPLY_REPORT_STRENGTH", 0 }, // Clear Reports
            { "MPPLY_VC_ANNOYINGME", 0 }, // Clear Reports
            { "MPPLY_VC_HATE", 0 }, // Clear Reports
            { "NUMBER_SLIPSTREAMS_IN_RACE", 100 }, // Vehicle Unlocks
            { "NUMBER_TURBO_STARTS_IN_RACE", 50 }, // Vehicle Unlocks
            { "PASS_DB_PLAYER_KILLS", 100 }, // Unlock All Achievements
            { "PISTOL_ENEMY_KILLS", 500 }, // Unlock Tattoos
            { "PISTOL_KILLS", 600 }, // Unlock Tints
            { "PLAYER_HEADSHOTS", 500 }, // Unlock Tattoos
            { "PUMP_ENEMY_KILLS", 600 }, // Unlock Tints
            { "RACES_WON", 50 }, // Vehicle Unlocks
            { "RPG_ENEMY_KILLS", 600 }, // Unlock Tints
            { "SAWNOFF_ENEMY_KILLS", 500 }, // Unlock All Achievements
            { "SCRIPT_INCREASE_DRIV", 100 }, // Maximum Skills
            { "SCRIPT_INCREASE_FLY", 100 }, // Maximum Skills
            { "SCRIPT_INCREASE_LUNG", 100 }, // Maximum Skills
            { "SCRIPT_INCREASE_MECH", 100 }, // Maximum Skills
            { "SCRIPT_INCREASE_SHO", 100 }, // Maximum Skills
            { "SCRIPT_INCREASE_STAM", 100 }, // Maximum Skills
            { "SCRIPT_INCREASE_STL", 100 }, // Maximum Skills
            { "SCRIPT_INCREASE_STRN", 100 }, // Maximum Skills
            { "SMG_ENEMY_KILLS", 600 }, // Unlock Tints
            { "SNIPERRFL_ENEMY_KILLS", 600 }, // Unlock Tints
            { "STKYBMB_ENEMY_KILLS", 50 }, // Unlock All Achievements
            { "UNARMED_ENEMY_KILLS", 50 }, // Unlock All Achievements
            { "USJS_COMPLETED", 50 }, // Vehicle Unlocks
            { "WEAP_FM_ADDON_PURCH", -1 }, // Unlock Weapons
            { "WEAP_FM_ADDON_PURCH2", -1 }, // Unlock Weapons
            { "WEAP_FM_ADDON_PURCH3", -1 }, // Unlock Weapons
            { "WEAP_FM_ADDON_PURCH4", -1 }, // Unlock Weapons
            { "WEAP_FM_ADDON_PURCH5", -1 }, // Unlock Weapons
        };
        #endregion
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
        #region Link
        string cmethod;
        void connect(SelectAPI api)
        {
            bool tmapi = api == SelectAPI.TargetManager;
            PS3.ChangeAPI(api);
            cmethod = tmapi ? "TMAPI" : "CCAPI";
            try
            {
                PS3.ConnectTarget();
                PS3.AttachProcess();
                RPC.Enable();

                if (NFunc.psn() == "")
                {
                    connectionStatus.Caption = "Error!";
                    connectionPSN.Caption = "RPC Enable Failed!";
                    connectionPSN.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    connectionStatus.Caption = "Connected [" + (tmapi ? "TM" : "CC") + "]";
                    connectionPSN.Caption = "Welcome, " + NFunc.psn() + " [Console: " + PS3.GetConsoleName() + "]";
                    connectionPSN.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                barSub_Connect.Glyph = Recovery.Properties.Resources.link_connected;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed\n\nError: " + ex.Message);
            }
        }
        private void barButton_TM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            connect(SelectAPI.TargetManager);
        }

        private void barButton_CC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            connect(SelectAPI.ControlConsole);
        }

        private void barButton_Disconnect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PS3.DisconnectTarget();
            connectionStatus.Caption = "Idle...";
            connectionPSN.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            barSub_Connect.Glyph = Recovery.Properties.Resources.link_idle;
        }
        #endregion
        #region Functions
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
            statStatus.Caption = "Used: " + stat;
            statStatus.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
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
        #endregion
        private void statqSearch_EditValueChanged(object sender, EventArgs e)
        {
            // Search stat dictionary for any stat containing your search
            var query = from item in Stats
                        where item.Key.ToLower().Contains(statqSearch.Text.ToLower())
                        orderby item.Key ascending
                        select item;
            // Clear list
            statqList.Items.Clear();
            // For each of the query results
            foreach (KeyValuePair<string, object> item in query)
            {
                // Add dictionary key & value to list
                statqList.Items.Add(item.Key + " = " + item.Value.ToString());
            }
        }
        #region Tunables
        private void DLC_Christmas_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Tunables.christmasDLC() ? "Enabled" : "Disabled");
        }

        private void DLC_Independence_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Tunables.independenceDLC() ? "Enabled" : "Disabled");
        }

        private void DLC_Valentines_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Tunables.valentinesDLC() ? "Enabled" : "Disabled");
        }
        #endregion
    }
}
