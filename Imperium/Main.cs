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
using System.Threading;

namespace Imperium
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
            { "CLTHS_AVAILABLE_HAIR", new StatData(-1, "hair") },
            { "CLTHS_AVAILABLE_HAIR_1", new StatData(-1, "hair") },
            { "CLTHS_AVAILABLE_HAIR_2", new StatData(-1, "hair") },
            { "CLTHS_AVAILABLE_HAIR_3", new StatData(-1, "hair") },
            { "CLTHS_AVAILABLE_HAIR_4", new StatData(-1, "hair") },
            { "CLTHS_AVAILABLE_HAIR_5", new StatData(-1, "hair") },
            { "CLTHS_AVAILABLE_HAIR_6", new StatData(-1, "hair") },
            { "CLTHS_AVAILABLE_HAIR_7", new StatData(-1, "hair") },
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
                barSub_Connect.Glyph = Imperium.Properties.Resources.link_connected;
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
            barSub_Connect.Glyph = Imperium.Properties.Resources.link_idle;
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
            statStatus.Caption = "Last Used: " + stat;
            statStatus.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            if (value is int || value is bool)
            {
                Natives address = value is int ? Natives.STAT_SET_INT : Natives.STAT_SET_BOOL;
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
            new Thread(() =>
            {
                RPC.PS3.ConnectTarget();
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
            }).Start();
        }
        void setStatKeywordQuery(params string[] snippets)
        {
            new Thread(() =>
            {
                RPC.PS3.ConnectTarget();
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
            }).Start();
        }
        #endregion
        private void statqSearch_EditValueChanged(object sender, EventArgs e)
        {
            var query = from item in Stats
                        where item.Key.ToLower().Contains(statqSearch.Text.ToLower())
                        orderby item.Key ascending
                        select item;
            statqList.Items.Clear();
            foreach (KeyValuePair<string, StatData> item in query)
            {
                statqList.Items.Add(item.Key + " = " + item.Value.value.ToString());
            }
        }
        private void statkwqSearch_EditValueChanged(object sender, EventArgs e)
        {
            var query = from item in Stats
                        where item.Value.keyword.ToLower().Contains(statkwqSearch.Text.ToLower())
                        orderby item.Key ascending
                        select item;
            statkwqList.Items.Clear();
            foreach (KeyValuePair<string, StatData> item in query)
            {
                statkwqList.Items.Add(item.Key + " = " + item.Value.value.ToString());
            }
        }
        #region Tunables
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
        #endregion

        private void gContacts_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("contacts");
        }

        private void gClothing_Click(object sender, EventArgs e)
        {
            setStatKeywordQuery("clothing", "hair");
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

        private void simpleButton1_Click(object sender, EventArgs e)
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
    }
}
