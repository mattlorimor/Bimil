﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Medo.Security.Cryptography.Bimil;
using Medo.Configuration;

namespace Bimil {
    internal partial class MainForm : Form {

        private BimilDocument Document = null;
        private string DocumentFileName = null;
        private bool DocumentChanged = false;
        private RecentFiles RecentFiles = new RecentFiles();

        public MainForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            mnu.Font = SystemFonts.MessageBoxFont;
            lsvPasswords.Font = SystemFonts.MessageBoxFont;

            mnu.Renderer = Helpers.ToolStripBorderlessSystemRendererInstance;

            Medo.Windows.Forms.State.SetupOnLoadAndClose(this);
        }

        private void Form_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.Alt | Keys.Menu: {
                        mnu.Select();
                        mnuNew.Select();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    } break;

                case Keys.Escape: {
                        if (Settings.CloseOnEscape) {
                            this.Close();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                    } break;

                case Keys.Control | Keys.N: {
                        mnuNew_Click(null, null);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    } break;

                case Keys.Control | Keys.O: {
                        mnuOpen_ButtonClick(null, null);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    } break;

                case Keys.Control | Keys.S: {
                        mnuSave_ButtonClick(null, null);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    } break;

                case Keys.Insert: {
                        mnuAdd_Click(null, null);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    } break;

                case Keys.F4: {
                        mnuEdit_Click(null, null);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    } break;

                case Keys.Delete: {
                        mnuRemove_Click(null, null);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    } break;

            }
        }

        private void Form_Load(object sender, EventArgs e) {
            RefreshFiles();
            RefreshItems();
            UpdateMenu();
        }

        private void RefreshFiles() {
            mnuOpen.DropDownItems.Clear();
            for (int i = 0; i < this.RecentFiles.Count; i++) {
                var item = new ToolStripMenuItem(this.RecentFiles[i].Title) { Tag = this.RecentFiles[i].FileName };
                item.Click += new EventHandler(delegate(object sender2, EventArgs e2) {
                    if (SaveIfNeeded() != DialogResult.OK) { return; }
                    LoadFile(item.Tag.ToString());
                });
                mnuOpen.DropDownItems.Add(item);
            }
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e) {
            if (SaveIfNeeded() != DialogResult.OK) {
                e.Cancel = true;
                return;
            }

            this.Document = null;
            this.DocumentFileName = null;
            this.DocumentChanged = false;
            Application.Exit();
        }

        private void Form_Resize(object sender, EventArgs e) {
            lsvPasswords.Columns[0].Width = lsvPasswords.Width - SystemInformation.VerticalScrollBarWidth;
        }

        private void lsvPasswords_ItemActivate(object sender, EventArgs e) {
            mnuEdit_Click(null, null);
        }

        private void lsvPasswords_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateMenu();
        }


        private DialogResult SaveIfNeeded() {
            if (this.DocumentChanged) {
                string question;
                if (this.DocumentFileName != null) {
                    var file = new FileInfo(this.DocumentFileName);
                    var title = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
                    question = title + " is not saved. Do you wish to save it now?"; 
                } else {
                    question = "Document is not saved. Do you wish to save it now?";
                }
                switch (Medo.MessageBox.ShowQuestion(this, question, MessageBoxButtons.YesNoCancel)) {
                    case DialogResult.Yes:
                        mnuSave_ButtonClick(null, null);
                        return (this.DocumentChanged == false) ? DialogResult.OK : DialogResult.Cancel;
                    case DialogResult.No: return DialogResult.OK;
                    case DialogResult.Cancel: return DialogResult.Cancel;
                    default: return DialogResult.Cancel;
                }
            } else {
                return DialogResult.OK;
            }
        }


        #region Menu

        private void mnuNew_Click(object sender, EventArgs e) {
            if (SaveIfNeeded() != DialogResult.OK) { return; }
            BimilDocument doc = null;
            try {
                using (var frm = new NewPasswordForm()) {
                    if (frm.ShowDialog(this) == DialogResult.OK) {
                        doc = new BimilDocument(frm.Password);
                    }
                }
            } finally {
                GC.Collect(); //in attempt to kill password string
            }
            if (doc != null) {
                this.Document = doc;
                this.DocumentFileName = null;
                this.DocumentChanged = false;
            }
            RefreshItems();
            UpdateMenu();
        }

        private void mnuOpen_ButtonClick(object sender, EventArgs e) {
            if (SaveIfNeeded() != DialogResult.OK) { return; }
            using (var frm = new OpenFileDialog() { AddExtension = true, AutoUpgradeEnabled = true, Filter = "Bimil files|*.bimil|All files|*.*", RestoreDirectory = true }) {
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    LoadFile(frm.FileName);
                }
            }
        }

        private void LoadFile(string fileName) {
            try {
                BimilDocument doc = null;
                try {
                    using (var frm = new PasswordForm()) {
                        if (frm.ShowDialog(this) == DialogResult.OK) {
                            doc = BimilDocument.Open(fileName, frm.Password);
                        }
                    }
                } finally {
                    GC.Collect(); //in attempt to kill password string
                }
                this.Document = doc;
                this.DocumentFileName = fileName;
                this.DocumentChanged = false;
                this.RecentFiles.Push(this.DocumentFileName);
                RefreshFiles();
            } catch (FormatException) {
                Medo.MessageBox.ShowError(this, "Either password is wrong or file is damaged.");
            }
            RefreshItems();
            UpdateMenu();
        }

        private void mnuSave_ButtonClick(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            if (this.DocumentFileName != null) {
                this.Document.Save(this.DocumentFileName);
                this.DocumentChanged = false;
                UpdateMenu();
            } else {
                mnuSaveAs_Click(null, null);
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            using (var frm = new SaveFileDialog() { AddExtension = true, AutoUpgradeEnabled = true, Filter = "Bimil files|*.bimil|All files|*.*", RestoreDirectory = true }) {
                if (this.DocumentFileName != null) { frm.FileName = this.DocumentFileName; }
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    this.Document.Save(frm.FileName);
                    this.DocumentFileName = frm.FileName;
                    this.DocumentChanged = false;
                    this.RecentFiles.Push(this.DocumentFileName);
                    RefreshFiles();
                    UpdateMenu();
                }
            }
        }

        private void mnuChangePassword_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            BimilDocument currDoc = null;
            try {
                using (var frm = new PasswordForm()) {
                    if (frm.ShowDialog(this) == DialogResult.OK) {
                        using (var stream = new MemoryStream()) {
                            this.Document.Save(stream);
                            stream.Position = 0;
                            currDoc = BimilDocument.Open(stream, frm.Password);
                        }
                    }
                }
            } catch (FormatException) {
                Medo.MessageBox.ShowError(this, "Old password does not match.");
            } finally {
                GC.Collect(); //in attempt to kill password string
            }

            if (currDoc == null) { return; }

            BimilDocument newDoc = null;
            try {
                using (var frm = new NewPasswordForm()) {
                    if (frm.ShowDialog(this) == DialogResult.OK) {
                        using (var stream = new MemoryStream()) {
                            currDoc.Save(stream, frm.Password);
                            stream.Position = 0;
                            newDoc = BimilDocument.Open(stream, frm.Password);
                        }
                    }
                }
            } finally {
                GC.Collect(); //in attempt to kill password string
            }

            this.Document = newDoc;
            this.DocumentChanged = true;
            RefreshItems();
            UpdateMenu();
        }

        private void mnuAdd_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            using (var frm = new SelectTemplateForm()) {
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    var item = this.Document.AddItem("New item", 0);
                    foreach (var record in frm.Template.Records) {
                        item.AddRecord(record.Key, "", record.Value);
                    }

                    using (var frm2 = new EditItemForm(this.Document, item, true)) {
                        if (frm2.ShowDialog(this) == DialogResult.OK) {
                            var listItem = new ListViewItem(item.Title) { Tag = item };
                            lsvPasswords.Items.Add(listItem);
                            this.DocumentChanged = true;
                        } else {
                            this.Document.Items.Remove(item);
                        }
                    }

                    UpdateMenu();
                }
            }
        }

        private void mnuEdit_Click(object sender, EventArgs e) {
            if ((this.Document == null) || (lsvPasswords.SelectedItems.Count != 1)) { return; }

            var item = (BimilItem)(lsvPasswords.SelectedItems[0].Tag);
            using (var frm2 = new EditItemForm(this.Document, item, false)) {
                if (frm2.ShowDialog(this) == DialogResult.OK) {
                    lsvPasswords.SelectedItems[0].Text = item.Title;
                    this.DocumentChanged = true;
                    UpdateMenu();
                }
            }
        }

        private void mnuRemove_Click(object sender, EventArgs e) {
            if ((this.Document == null) || (lsvPasswords.SelectedItems.Count == 0)) { return; }

            for (int i = lsvPasswords.SelectedItems.Count - 1; i >= 0; i--) {
                var item = (BimilItem)(lsvPasswords.SelectedItems[i].Tag);
                this.Document.Items.Remove(item);
                lsvPasswords.Items.Remove(lsvPasswords.SelectedItems[i]);
            }
            this.DocumentChanged = true;
            UpdateMenu();
        }

        private void mnuOptions_Click(object sender, EventArgs e) {
            using (var frm = new SettingsForm()) {
                frm.ShowDialog(this);
            }
        }

        private void mnuReportABug_Click(object sender, EventArgs e) {
            Medo.Diagnostics.ErrorReport.ShowDialog(this, null, new Uri("http://jmedved.com/errorreport/"));
        }

        private void mnuAbout_Click(object sender, EventArgs e) {
            Medo.Windows.Forms.AboutBox.ShowDialog(this, new Uri("http://www.jmedved.com/bimil/"));
        }

        #endregion


        private void RefreshItems() {
            lsvPasswords.BeginUpdate();
            lsvPasswords.Items.Clear();
            if (this.Document != null) {
                foreach (var item in this.Document.Items) {
                    lsvPasswords.Items.Add(new ListViewItem(item.Title) { Tag = item });
                }
            }
            lsvPasswords.EndUpdate();
        }

        private void UpdateMenu() {
            mnuSave.Enabled = (this.Document != null);
            mnuChangePassword.Enabled = (this.Document != null);
            mnuAdd.Enabled = (this.Document != null);
            mnuEdit.Enabled = (this.Document != null) && (lsvPasswords.SelectedItems.Count == 1);
            mnuRemove.Enabled = (this.Document != null) && (lsvPasswords.SelectedItems.Count > 0);

            lsvPasswords.Enabled = (this.Document != null);

            if (this.DocumentFileName == null) {
                this.Text = this.DocumentChanged ? "Bimil*" : "Bimil";
            } else {
                var file = new FileInfo(this.DocumentFileName);
                var title = file.Name.Substring(0, file.Name.Length - file.Extension.Length);

                this.Text = title + (this.DocumentChanged ? "*" : "") + " - Bimil";
            }
        }

    }
}
