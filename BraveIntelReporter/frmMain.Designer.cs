namespace BraveIntelReporter
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtIntel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReported = new System.Windows.Forms.Label();
            this.lblFailed = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.lblMonitoringFiles = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.visitEvEOvermindcomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intelMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.planningMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.veteranKillsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topKillersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.v1AnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shipFitAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDScanLocalScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOutputMinimal = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOutputStandard = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOutputVerbose = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetEveToBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtIntel
            // 
            this.txtIntel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIntel.Location = new System.Drawing.Point(10, 27);
            this.txtIntel.Multiline = true;
            this.txtIntel.Name = "txtIntel";
            this.txtIntel.ReadOnly = true;
            this.txtIntel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIntel.Size = new System.Drawing.Size(508, 192);
            this.txtIntel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Reported: ";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Failed: ";
            // 
            // lblReported
            // 
            this.lblReported.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblReported.AutoSize = true;
            this.lblReported.Location = new System.Drawing.Point(75, 237);
            this.lblReported.Name = "lblReported";
            this.lblReported.Size = new System.Drawing.Size(13, 13);
            this.lblReported.TabIndex = 3;
            this.lblReported.Text = "0";
            // 
            // lblFailed
            // 
            this.lblFailed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFailed.AutoSize = true;
            this.lblFailed.Location = new System.Drawing.Point(75, 262);
            this.lblFailed.Name = "lblFailed";
            this.lblFailed.Size = new System.Drawing.Size(13, 13);
            this.lblFailed.TabIndex = 4;
            this.lblFailed.Text = "0";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Reporter is minimized (still reporting)";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 237);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Monitoring Files: ";
            // 
            // lblMonitoringFiles
            // 
            this.lblMonitoringFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMonitoringFiles.AutoSize = true;
            this.lblMonitoringFiles.Location = new System.Drawing.Point(268, 237);
            this.lblMonitoringFiles.Name = "lblMonitoringFiles";
            this.lblMonitoringFiles.Size = new System.Drawing.Size(10, 13);
            this.lblMonitoringFiles.TabIndex = 6;
            this.lblMonitoringFiles.Text = "-";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visitEvEOvermindcomToolStripMenuItem,
            this.addDScanLocalScanToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(518, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // visitEvEOvermindcomToolStripMenuItem
            // 
            this.visitEvEOvermindcomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intelMapToolStripMenuItem,
            this.planningMapToolStripMenuItem,
            this.jumpMapToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.veteranKillsToolStripMenuItem,
            this.topKillersToolStripMenuItem,
            this.v1AnalysisToolStripMenuItem,
            this.shipFitAnalysisToolStripMenuItem});
            this.visitEvEOvermindcomToolStripMenuItem.Name = "visitEvEOvermindcomToolStripMenuItem";
            this.visitEvEOvermindcomToolStripMenuItem.Size = new System.Drawing.Size(121, 20);
            this.visitEvEOvermindcomToolStripMenuItem.Text = "EvEOvermind Tools";
            // 
            // intelMapToolStripMenuItem
            // 
            this.intelMapToolStripMenuItem.Name = "intelMapToolStripMenuItem";
            this.intelMapToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.intelMapToolStripMenuItem.Text = "Intel Map";
            this.intelMapToolStripMenuItem.Click += new System.EventHandler(this.mnuViewMap_Click);
            // 
            // planningMapToolStripMenuItem
            // 
            this.planningMapToolStripMenuItem.Name = "planningMapToolStripMenuItem";
            this.planningMapToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.planningMapToolStripMenuItem.Text = "Planning Map";
            this.planningMapToolStripMenuItem.Click += new System.EventHandler(this.mnuViewPlanningMap_Click);
            // 
            // jumpMapToolStripMenuItem
            // 
            this.jumpMapToolStripMenuItem.Name = "jumpMapToolStripMenuItem";
            this.jumpMapToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.jumpMapToolStripMenuItem.Text = "Jump Map";
            this.jumpMapToolStripMenuItem.Click += new System.EventHandler(this.mnuViewJumpMap_Click);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.searchToolStripMenuItem.Text = "Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.mnuViewSearch_Click);
            // 
            // veteranKillsToolStripMenuItem
            // 
            this.veteranKillsToolStripMenuItem.Name = "veteranKillsToolStripMenuItem";
            this.veteranKillsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.veteranKillsToolStripMenuItem.Text = "Veteran Kills";
            this.veteranKillsToolStripMenuItem.Click += new System.EventHandler(this.mnuViewVeterans_Click);
            // 
            // topKillersToolStripMenuItem
            // 
            this.topKillersToolStripMenuItem.Name = "topKillersToolStripMenuItem";
            this.topKillersToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.topKillersToolStripMenuItem.Text = "Top Killers";
            this.topKillersToolStripMenuItem.Click += new System.EventHandler(this.mnuViewLeaderBoard_Click);
            // 
            // v1AnalysisToolStripMenuItem
            // 
            this.v1AnalysisToolStripMenuItem.Name = "v1AnalysisToolStripMenuItem";
            this.v1AnalysisToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.v1AnalysisToolStripMenuItem.Text = "1v1 Analysis";
            this.v1AnalysisToolStripMenuItem.Click += new System.EventHandler(this.mnuViewSolo_Click);
            // 
            // shipFitAnalysisToolStripMenuItem
            // 
            this.shipFitAnalysisToolStripMenuItem.Name = "shipFitAnalysisToolStripMenuItem";
            this.shipFitAnalysisToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.shipFitAnalysisToolStripMenuItem.Text = "Ship Fit Analysis";
            this.shipFitAnalysisToolStripMenuItem.Click += new System.EventHandler(this.mnuViewFitting_Click);
            // 
            // addDScanLocalScanToolStripMenuItem
            // 
            this.addDScanLocalScanToolStripMenuItem.Name = "addDScanLocalScanToolStripMenuItem";
            this.addDScanLocalScanToolStripMenuItem.Size = new System.Drawing.Size(135, 20);
            this.addDScanLocalScanToolStripMenuItem.Text = "Add DScan/LocalScan";
            this.addDScanLocalScanToolStripMenuItem.Click += new System.EventHandler(this.mnuDscan_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outputToolStripMenuItem,
            this.mnuSetEveToBackground,
            this.optionsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOutputMinimal,
            this.mnuOutputStandard,
            this.mnuOutputVerbose});
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.outputToolStripMenuItem.Text = "Output";
            // 
            // mnuOutputMinimal
            // 
            this.mnuOutputMinimal.Name = "mnuOutputMinimal";
            this.mnuOutputMinimal.Size = new System.Drawing.Size(121, 22);
            this.mnuOutputMinimal.Text = "Minimal";
            this.mnuOutputMinimal.Click += new System.EventHandler(this.mnuOutputMinimal_Click);
            // 
            // mnuOutputStandard
            // 
            this.mnuOutputStandard.Name = "mnuOutputStandard";
            this.mnuOutputStandard.Size = new System.Drawing.Size(121, 22);
            this.mnuOutputStandard.Text = "Standard";
            // 
            // mnuOutputVerbose
            // 
            this.mnuOutputVerbose.Checked = true;
            this.mnuOutputVerbose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuOutputVerbose.Name = "mnuOutputVerbose";
            this.mnuOutputVerbose.Size = new System.Drawing.Size(121, 22);
            this.mnuOutputVerbose.Text = "Verbose";
            this.mnuOutputVerbose.Click += new System.EventHandler(this.mnuOutputVerbose_Click);
            // 
            // mnuSetEveToBackground
            // 
            this.mnuSetEveToBackground.Name = "mnuSetEveToBackground";
            this.mnuSetEveToBackground.Size = new System.Drawing.Size(202, 22);
            this.mnuSetEveToBackground.Text = "Keep EVE in Background";
            this.mnuSetEveToBackground.Click += new System.EventHandler(this.mnuSetEveToBackground_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 283);
            this.Controls.Add(this.lblMonitoringFiles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFailed);
            this.Controls.Add(this.lblReported);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIntel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "EVEOvermind Reporter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIntel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReported;
        private System.Windows.Forms.Label lblFailed;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMonitoringFiles;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuOutputMinimal;
        private System.Windows.Forms.ToolStripMenuItem mnuOutputStandard;
        private System.Windows.Forms.ToolStripMenuItem mnuOutputVerbose;
        private System.Windows.Forms.ToolStripMenuItem mnuSetEveToBackground;
        private System.Windows.Forms.ToolStripMenuItem visitEvEOvermindcomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intelMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem planningMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem veteranKillsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topKillersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem v1AnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shipFitAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDScanLocalScanToolStripMenuItem;
    }
}

