﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bimil {
    internal partial class SettingsForm : Form {
        public SettingsForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
        }

        private readonly Medo.Configuration.RunOnStartup RunOnStartup = new Medo.Configuration.RunOnStartup(Medo.Configuration.RunOnStartup.Current.Title, Medo.Configuration.RunOnStartup.Current.ExecutablePath, "/tray");

        private void SettingsForm_Load(object sender, EventArgs e) {
            chbUseNotificationArea.Checked = Settings.UseNotificationArea;
            chbRunOnStartup.Checked = this.RunOnStartup.RunForCurrentUser;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            Settings.UseNotificationArea = chbUseNotificationArea.Checked;
            this.RunOnStartup.RunForCurrentUser = chbRunOnStartup.Checked;
        }

    }
}
