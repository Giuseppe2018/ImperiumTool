namespace Recovery
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSub_Connect = new DevExpress.XtraBars.BarSubItem();
            this.barButton_CC = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_TM = new DevExpress.XtraBars.BarButtonItem();
            this.barButton_Disconnect = new DevExpress.XtraBars.BarButtonItem();
            this.barSub_Settings = new DevExpress.XtraBars.BarSubItem();
            this.skinBarSubItem2 = new DevExpress.XtraBars.SkinBarSubItem();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.statStatus = new DevExpress.XtraBars.BarStaticItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.connectionStatus = new DevExpress.XtraBars.BarStaticItem();
            this.connectionPSN = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.skinBarSubItem1 = new DevExpress.XtraBars.SkinBarSubItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(12, 92);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(570, 393);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(564, 365);
            this.xtraTabPage1.Text = "General";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Recovery.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(570, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.connectionStatus,
            this.barButtonItem1,
            this.barSub_Connect,
            this.skinBarSubItem1,
            this.barSub_Settings,
            this.barButton_TM,
            this.barButton_CC,
            this.barButton_Disconnect,
            this.skinBarSubItem2,
            this.barStaticItem2,
            this.barStaticItem4,
            this.statStatus,
            this.connectionPSN,
            this.barEditItem1});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 22;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.barManager1.ShowScreenTipsInMenus = true;
            this.barManager1.ShowScreenTipsInToolbars = false;
            this.barManager1.ShowShortcutInScreenTips = false;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSub_Connect, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSub_Settings, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem4, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.statStatus)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.AutoPopupMode = DevExpress.XtraBars.BarAutoPopupMode.None;
            this.bar2.OptionsBar.DisableCustomization = true;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barSub_Connect
            // 
            this.barSub_Connect.Caption = "Connect";
            this.barSub_Connect.Glyph = global::Recovery.Properties.Resources.link_idle;
            this.barSub_Connect.Id = 2;
            this.barSub_Connect.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButton_CC, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButton_TM, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButton_Disconnect, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barSub_Connect.Name = "barSub_Connect";
            // 
            // barButton_CC
            // 
            this.barButton_CC.Caption = "Control Console (CCAPI)";
            this.barButton_CC.Glyph = global::Recovery.Properties.Resources.ccapi;
            this.barButton_CC.Id = 6;
            this.barButton_CC.Name = "barButton_CC";
            this.barButton_CC.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_CC_ItemClick);
            // 
            // barButton_TM
            // 
            this.barButton_TM.Caption = "Target Manager (TMAPI)";
            this.barButton_TM.Glyph = global::Recovery.Properties.Resources.tmapi;
            this.barButton_TM.Id = 5;
            this.barButton_TM.Name = "barButton_TM";
            this.barButton_TM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_TM_ItemClick);
            // 
            // barButton_Disconnect
            // 
            this.barButton_Disconnect.Caption = "Disconnect";
            this.barButton_Disconnect.Glyph = global::Recovery.Properties.Resources.disconnect;
            this.barButton_Disconnect.Id = 7;
            this.barButton_Disconnect.Name = "barButton_Disconnect";
            this.barButton_Disconnect.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_Disconnect_ItemClick);
            // 
            // barSub_Settings
            // 
            this.barSub_Settings.Caption = "Settings";
            this.barSub_Settings.Glyph = ((System.Drawing.Image)(resources.GetObject("barSub_Settings.Glyph")));
            this.barSub_Settings.Id = 4;
            this.barSub_Settings.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.skinBarSubItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barSub_Settings.Name = "barSub_Settings";
            // 
            // skinBarSubItem2
            // 
            this.skinBarSubItem2.Caption = "Theme";
            this.skinBarSubItem2.Glyph = global::Recovery.Properties.Resources.theme;
            this.skinBarSubItem2.Id = 8;
            this.skinBarSubItem2.Name = "skinBarSubItem2";
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Caption = "Developed by Kryptus";
            this.barStaticItem4.Id = 13;
            this.barStaticItem4.LeftIndent = 10;
            this.barStaticItem4.Name = "barStaticItem4";
            this.barStaticItem4.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // statStatus
            // 
            this.statStatus.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.statStatus.Caption = "STAT STATUS";
            this.statStatus.Id = 17;
            this.statStatus.Name = "statStatus";
            this.statStatus.RightIndent = 10;
            this.statStatus.TextAlignment = System.Drawing.StringAlignment.Near;
            this.statStatus.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.connectionStatus),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.connectionPSN, "", false, false, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Caption)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // connectionStatus
            // 
            this.connectionStatus.Caption = "Idle...";
            this.connectionStatus.Id = 0;
            this.connectionStatus.Name = "connectionStatus";
            this.connectionStatus.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // connectionPSN
            // 
            this.connectionPSN.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.connectionPSN.Caption = "PSN";
            this.connectionPSN.Id = 19;
            this.connectionPSN.Name = "connectionPSN";
            this.connectionPSN.RightIndent = 10;
            this.connectionPSN.TextAlignment = System.Drawing.StringAlignment.Near;
            this.connectionPSN.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(594, 22);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 491);
            this.barDockControlBottom.Size = new System.Drawing.Size(594, 25);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 22);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 469);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(594, 22);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 469);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // skinBarSubItem1
            // 
            this.skinBarSubItem1.Caption = "Theme";
            this.skinBarSubItem1.Id = 3;
            this.skinBarSubItem1.Name = "skinBarSubItem1";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "Kryptus";
            this.barStaticItem2.Id = 11;
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "TM";
            this.barEditItem1.Edit = this.repositoryItemTextEdit1;
            this.barEditItem1.EditValue = "192.168.1.22";
            this.barEditItem1.Id = 21;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 516);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GTA V Recovery Tool 2.0 [1.26 BLES]";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarStaticItem connectionStatus;
        private DevExpress.XtraBars.BarSubItem barSub_Connect;
        private DevExpress.XtraBars.BarButtonItem barButton_CC;
        private DevExpress.XtraBars.BarButtonItem barButton_TM;
        private DevExpress.XtraBars.BarButtonItem barButton_Disconnect;
        private DevExpress.XtraBars.BarSubItem barSub_Settings;
        private DevExpress.XtraBars.SkinBarSubItem skinBarSubItem1;
        private DevExpress.XtraBars.SkinBarSubItem skinBarSubItem2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem barStaticItem4;
        private DevExpress.XtraBars.BarStaticItem statStatus;
        private DevExpress.XtraBars.BarStaticItem connectionPSN;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;

    }
}

