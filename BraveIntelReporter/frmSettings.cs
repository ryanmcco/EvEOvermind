using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BraveIntelReporter
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnBrowseForChatlogs_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.SelectedPath = Configuration.LogDirectory;
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtChatlogPath.Text = fb.SelectedPath;
            }
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            txtChatlogPath.Text = Configuration.LogDirectory;
            txtAuthToken.Text = Configuration.AuthToken;
            chkRunOnStartup.Checked = Configuration.RunOnStartup;
            chkDisableReporting.Checked = !Configuration.EnableReporting;
            checkClipboard.Checked = Configuration.CheckClipboard;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Configuration.LogDirectory = txtChatlogPath.Text;
            Configuration.AuthToken = txtAuthToken.Text;
            Configuration.EnableReporting = !chkDisableReporting.Checked;
            Configuration.CheckClipboard = checkClipboard.Checked;
            Configuration.Save(chkRunOnStartup.Checked);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Configuration.settingsURL != string.Empty)
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.settingsURL);
                Process.Start(sInfo);
            }
        }

        private void chkRunOnStartup_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}
