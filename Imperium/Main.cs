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

            string[] Settings_Settings_content = File.ReadAllLines("Data/Settings.json");
            foreach (string line in Settings_Settings_content)
            {
                /*
                 { "name": "Character_1", "value": true }
                 { "name": "Character_2", "value": false }
                 */
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

            refreshOutfitListing();

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
            RPC.Call(Natives.SET_ENTITY_COORDS, NFunc.isInVehicle() ? NFunc.vehid() : NFunc.pedid(), location, 1, 0, 0, 1);
            Thread.Sleep(1000);
            RPC.Call(Natives.DO_SCREEN_FADE_IN, 400);
        }
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
                        orderby item.Value.keyword ascending
                        select item;
            statqList.Items.Clear();
            foreach (KeyValuePair<string, StatData> item in query)
            {
                statqList.Items.Add("[" + item.Value.keyword + "] " + item.Key + " = " + item.Value.value.ToString());
            }
        }
        private void statkwqSearch_EditValueChanged(object sender, EventArgs e)
        {
            var query = from item in Stats
                        where item.Value.keyword.ToLower().Contains(statkwqSearch.Text.ToLower())
                        orderby item.Value.keyword ascending
                        select item;
            statkwqList.Items.Clear();
            foreach (KeyValuePair<string, StatData> item in query)
            {
                statkwqList.Items.Add("[" + item.Value.keyword + "] " + item.Key + " = " + item.Value.value.ToString());
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
            RPC.Call(Natives.MONEY, Convert.ToInt32(gAddCash.Text), 0);
        }

        #region Outfit
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
        private void aoRefresh_Click(object sender, EventArgs e)
        {
            refreshOutfitListing();
            aoListing.SelectedIndex = outfitTabs.SelectedTabPageIndex = 0;
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

        void Reset()
        {
            RPC.Call(Natives.CLEAR_ALL_PED_PROPS, NFunc.pedid());
            RPC.Call(Natives.CLEAR_PED_DECORATIONS, NFunc.pedid());
            RPC.Call(Natives.SET_PED_COMPONENT_VARIATION, NFunc.pedid(), 1, 0, 0);
            RPC.Call(Natives.SET_PED_COMPONENT_VARIATION, NFunc.pedid(), 5, 0, 0);
            RPC.Call(Natives.SET_PED_COMPONENT_VARIATION, NFunc.pedid(), 9, 0, 0);
        }
        int fam;
        void setClothing(string family, string model, string texture)
        {
            if (family == "HAT" || family == "EYES" || family == "EARS")
            {
                switch (family)
                {
                    case "HAT": fam = 0; break;
                    case "EYES": fam = 1; break;
                    case "EARS": fam = 2; break;
                }
                if (model != "-1" && texture != "-1")
                    RPC.Call(Natives.SET_PED_PROP_INDEX, NFunc.pedid(), fam, Convert.ToInt32(model) - 1, Convert.ToInt32(texture));
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
                    RPC.Call(Natives.SET_PED_COMPONENT_VARIATION, NFunc.pedid(), fam, Convert.ToInt32(model), Convert.ToInt32(texture));
            }
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
        #endregion
    }
}
