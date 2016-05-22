using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PS3Lib;

namespace Recovery
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        public static PS3API PS3 = new PS3API();
        public Main()
        {
            InitializeComponent();
        }
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

        private void char1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Variables.character1 = char1.Checked;
        }

        private void char2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Variables.character2 = char2.Checked;
        }
    }
}
