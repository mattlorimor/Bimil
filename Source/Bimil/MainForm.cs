using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using Medo.Configuration;
using Medo.Security.Cryptography;
using Medo.Security.Cryptography.PasswordSafe;
using LegacyFile = Medo.Security.Cryptography.Bimil;

namespace Bimil {
    internal partial class MainForm : Form {

        private Document Document = null;
        private string DocumentFileName = null;
        private bool DocumentReadOnlyChanged = false;
        private readonly List<string> Categories = new List<string>();

        public MainForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            mnu.Font = SystemFonts.MessageBoxFont;
            lsvEntries.Font = SystemFonts.MessageBoxFont;

            mnu.Renderer = Helpers.ToolStripBorderlessSystemRendererInstance;
            Helpers.ScaleToolstrip(mnu);

            Medo.Windows.Forms.State.SetupOnLoadAndClose(this);

#if DEBUG
            mnuAppDebug.Visible = true;
            mnuAppDebugRandomizeAllPasswords.Visible = true;
#endif
        }


        private bool SuppressMenuKey = false;

        protected override bool ProcessDialogKey(Keys keyData) {
            if (((keyData & Keys.Alt) == Keys.Alt) && (keyData != (Keys.Alt | Keys.Menu))) { this.SuppressMenuKey = true; }

            switch (keyData) {

                case Keys.F10:
                    ToggleMenu();
                    return true;

                case Keys.Escape: {
                        if (Settings.CloseOnEscape) {
                            this.Close();
                            return true;
                        }
                    }
                    break;

                case Keys.Control | Keys.N:
                case Keys.Alt | Keys.N:
                    mnuNew.PerformClick();
                    return true;

                case Keys.Control | Keys.O:
                    if (mnuOpenMenu.Visible) {
                        mnuOpenMenu.PerformButtonClick();
                    } else {
                        mnuOpenAlone.PerformClick();
                    }
                    return true;

                case Keys.Alt | Keys.O:
                    if (mnuOpenMenu.Visible) {
                        mnuOpenMenu.ShowDropDown();
                    } else {
                        mnuOpenAlone.PerformClick();
                    }
                    return true;

                case Keys.Control | Keys.S:
                    mnuSaveMenu.PerformButtonClick();
                    return true;

                case Keys.Alt | Keys.S:
                    mnuSaveMenu.ShowDropDown();
                    return true;

                case Keys.Alt | Keys.F:
                    mnuSearchRoot.ShowDropDown();
                    return true;

                case Keys.Control | Keys.F:
                    mnuSearchRoot.PerformButtonClick();
                    return true;

                case Keys.Control | Keys.T:
                    mnxEntryAutotype_Click(null, null);
                    return true;

                case Keys.Shift | Keys.Delete:
                    mnxEntryCut_Click(null, null);
                    return true;

                case Keys.Control | Keys.Insert:
                    mnxEntryCopy_Click(null, null);
                    return true;

                case Keys.Shift | Keys.Insert:
                    mnxEntryPaste_Click(null, null);
                    return true;

                case Keys.Control | Keys.V:
                    if (ClipboardHelper.HasDataOnClipboard) {
                        mnxEntryPaste_Click(null, null);
                        return true;
                    } else {
                        return base.ProcessDialogKey(keyData);
                    }

                case Keys.F5:
                    RefreshCategories();
                    RefreshItems();
                    UpdateMenu();
                    return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyData == Keys.Menu) {
                if (this.SuppressMenuKey) { this.SuppressMenuKey = false; return; }
                ToggleMenu();
                e.Handled = true;
                e.SuppressKeyPress = true;
            } else {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            if (e.KeyData == Keys.Menu) {
                if (this.SuppressMenuKey) { this.SuppressMenuKey = false; return; }
                ToggleMenu();
                e.Handled = true;
                e.SuppressKeyPress = true;
            } else {
                base.OnKeyUp(e);
            }
        }


        private void Form_Load(object sender, EventArgs e) {
            RefreshFiles();
            RefreshCategories();
            RefreshItems();
            UpdateMenu();
        }

        private void Form_Shown(object sender, EventArgs e) {
            var args = Environment.GetCommandLineArgs();
            var fileName = (args.Length) > 1 ? args[1] : null;
            if (fileName != null) {
                LoadFile(fileName);
            } else if (Settings.ShowStart) {
                using (var frm = new StartForm()) {
                    if (frm.ShowDialog(this) == DialogResult.OK) {
                        switch (frm.Action) {
                            case Helpers.StartAction.New:
                                mnuNew_Click(null, null);
                                break;

                            case Helpers.StartAction.Open:
                                if (frm.FileName != null) {
                                    LoadFile(frm.FileName, isReadOnly: false);
                                } else {
                                    mnuOpen.PerformClick();
                                }
                                break;

                            case Helpers.StartAction.OpenReadonly:
                                if (frm.FileName != null) {
                                    LoadFile(frm.FileName, isReadOnly: true);
                                } else {
                                    this.ShowNextOpenFileDialogWithReadOnly = true;
                                    mnuOpen.PerformClick();
                                }
                                break;
                        }
                    }
                }
            } else if (Settings.LoadLast) {
                var fileNames = new List<string>(App.Recent.FileNames);
                var lastFile = (fileNames.Count > 0) ? fileNames[0] : null;
                if (File.Exists(lastFile)) {
                    LoadFile(lastFile);
                }
            }

#if WINDOWS_STORE
            mnuAppUpgrade.Visible = false;
#else
            if (Helpers.IsRunningOnMono) {
                mnuAppUpgrade.Visible = false; //don't show update for Mono clients
            } else if (!Config.IsAssumedInstalled) {
                mnuAppUpgrade.Visible = false; //don't show update for portable executable
            } else {
                var version = Assembly.GetExecutingAssembly().GetName().Version; //don't auto-check for development builds
                if ((version.Major != 0) || (version.Minor != 0)) { bwUpgradeCheck.RunWorkerAsync(); }
            }
#endif
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e) {
            if (SaveIfNeeded() != DialogResult.OK) {
                e.Cancel = true;
                return;
            }

            bwUpgradeCheck.CancelAsync();
            ClipboardHelper.Cancel();

            this.Document = null;
            this.DocumentFileName = null;
        }

        private void Form_Resize(object sender, EventArgs e) {
            lsvEntries.Columns[0].Width = lsvEntries.ClientSize.Width;
        }

        private void Form_Deactivate(object sender, EventArgs e) {
            if (Settings.AutoCloseTimeout > 0) {
                tmrClose.Interval = Settings.AutoCloseTimeout * 1000;
                tmrClose.Enabled = true;
            }
        }

        private void Form_Activated(object sender, EventArgs e) {
            if (this.Focused) {
                tmrClose.Enabled = false;
            } else if (tmrClose.Enabled) { //because damn Activated gets triggered after ShowDialog exists - even if application is not in focus
                tmrClose.Enabled = false;
                tmrClose.Enabled = true;
            }
        }


        private void tmrClose_Tick(object sender, EventArgs e) {
            if (Application.OpenForms.Count <= 1) {
                tmrClose.Enabled = false;

                if (this.Document != null) {
                    if (Settings.AutoCloseSave && this.Document.HasChanged) {
                        if (this.DocumentFileName != null) {
                            mnuSave_Click(null, null);
                        } else { //if it cannot be saved, cancel close
                            tmrClose.Enabled = true;
                            return;
                        }
                    }

                    if (this.WindowState == FormWindowState.Minimized) { this.WindowState = FormWindowState.Normal; }

                    while (true) {
                        using (var frm = new PasswordForm()) {
                            if (frm.ShowDialog(this) == DialogResult.OK) {
                                if (this.Document.ValidatePassphrase(frm.Password)) {
                                    break;
                                } else {
                                    Medo.MessageBox.ShowError(this, "Password does not match.");
                                    continue;
                                }
                            } else {
                                this.Document = null;
                                this.DocumentFileName = null;
                                break;
                            }
                        }
                    }

                    RefreshFiles();
                    RefreshCategories();
                    RefreshItems();
                    UpdateMenu();
                }
            }
        }


        private void RefreshFiles() {
            for (var i = mnuOpenMenu.DropDownItems.Count - 1; i >= 0; i--) {
                if ((mnuOpenMenu.DropDownItems[i] is ToolStripMenuItem) && (mnuOpenMenu.DropDownItems[i].Tag is RecentlyUsedFile)) {
                    mnuOpenMenu.DropDownItems.RemoveAt(i);
                } else {
                    break;
                }
            }
            foreach (var file in App.Recent.Files) {
                var item = new ToolStripMenuItem(file.Title) { Tag = file, ToolTipText = file.FileName };
                item.Click += delegate {
                    if (SaveIfNeeded() != DialogResult.OK) { return; }
                    var fileName = ((RecentlyUsedFile)item.Tag).FileName;
                    LoadFile(fileName);
                };
                mnuOpenMenu.DropDownItems.Add(item);
            }

            mnuOpenMenu.Visible = (App.Recent.Count > 0);
            mnuOpenAlone.Visible = !mnuOpenMenu.Visible;
        }

        private void lsvEntries_ItemActivate(object sender, EventArgs e) {
            if (mnuEdit.Visible) {
                mnuEdit_Click(null, null);
            } else {
                mnuView_Click(null, null);
            }
        }


        private void cmbSearch_KeyDown(object sender, KeyEventArgs e) {
            if (!Helpers.HandleSearchKeyDown(e, lsvEntries, cmbSearch)) {
                switch (e.KeyData) {
                    case Keys.PageDown: {
                            Helpers.HandleSearchPageDown(cmbSearch);
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.PageUp: {
                            Helpers.HandleSearchPageUp(cmbSearch);
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        break;

                    case Keys.Control | Keys.X:
                        if (cmbSearch.SelectedText.Length > 0) {
                            ClipboardHelper.SetClipboardText(this, cmbSearch.SelectedText, sensitiveData: false);
                            cmbSearch.SelectedText = "";
                        }
                        e.Handled = true;
                        break;

                    default:
                        lsvEntries_KeyDown(null, e);
                        break;
                }
            }
        }

        private void lsvEntries_BeforeLabelEdit(object sender, LabelEditEventArgs e) {
            if (!Settings.EditableByDefault) { e.CancelEdit = true; }
        }

        private void lsvEntries_AfterLabelEdit(object sender, LabelEditEventArgs e) {
            if (lsvEntries.Items[e.Item].Tag is Entry entry) {
                if (!string.IsNullOrWhiteSpace(e.Label)) {
                    entry.Title = e.Label;
                } else {
                    e.CancelEdit = true;
                }
            }
        }

        private void lsvEntries_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.Insert: {
                        if (mnuAdd.Enabled) { mnuAdd_Click(null, null); }
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;

                case Keys.Alt | Keys.Insert: {
                        if (mnuAdd.Enabled && (lsvEntries.SelectedItems.Count == 1)) { mnxEntryAddSimilar_Click(null, null); }
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;

                case Keys.F2: {
                        if ((lsvEntries.SelectedItems.Count == 1) && !this.Document.IsReadOnly) {
                            lsvEntries.SelectedItems[0].BeginEdit();
                        }
                    }
                    break;

                case Keys.F4: {
                        if (mnuEdit.Visible) {
                            if (mnuEdit.Enabled) { mnuEdit_Click(null, null); }
                        } else {
                            if (mnuView.Enabled) { mnuView_Click(null, null); }
                        }
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;

                case Keys.Delete: {
                        if (mnuRemove.Enabled) { mnuRemove_Click(null, null); }
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;

                case Keys.PageDown: {
                        var index = (cmbSearch.SelectedIndex == -1) ? 0 : cmbSearch.SelectedIndex;
                        cmbSearch.SelectedIndex = Math.Min(index + 1, cmbSearch.Items.Count - 1);
                        cmbSearch.SelectAll();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;

                case Keys.PageUp: {
                        var index = (cmbSearch.SelectedIndex == -1) ? cmbSearch.Items.Count - 1 : cmbSearch.SelectedIndex;
                        cmbSearch.SelectedIndex = Math.Max(index - 1, 0);
                        cmbSearch.SelectAll();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;

                case Keys.Control | Keys.X:
                    mnxEntryCut_Click(null, null);
                    break;

                case Keys.Control | Keys.C:
                    mnxEntryCopy_Click(null, null);
                    break;

                case Keys.Control | Keys.V:
                    mnxEntryPaste_Click(null, null);
                    break;
            }
        }

        private void lsvEntries_SelectedIndexChanged(object sender, EventArgs e) {
            if (lsvEntries.SelectedItems.Count > 0) {
                var entry = lsvEntries.SelectedItems[0].Tag as Entry;
                if (entry == null) {
                    lsvEntries.SelectedItems.Clear();
                    return;
                }
            }
            UpdateMenu();
        }


        private DialogResult SaveIfNeeded() {
            if (this.Document?.HasChanged ?? false) {
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
                        mnuSave_Click(null, null);
                        return (this.Document.HasChanged == false) ? DialogResult.OK : DialogResult.Cancel;
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
            Document doc = null;
            try {
                using (var frm = new NewPasswordForm()) {
                    if (frm.ShowDialog(this) == DialogResult.OK) {
                        doc = new Document(frm.Password);
                    }
                }
            } finally {
                GC.Collect(); //in attempt to kill password string
            }
            if (doc != null) {
                this.Document = doc;
                this.DocumentFileName = null;
            }
            RefreshCategories();
            RefreshItems();
            UpdateMenu();
            cmbSearch.Select();
        }


        private void mnuOpenMenu_DropDownOpening(object sender, EventArgs e) {
            foreach (ToolStripItem menuItem in mnuOpenMenu.DropDownItems) {
                if (menuItem.Tag is RecentlyUsedFile recentFile) {
                    var fileName = recentFile.FileName;
                    if (fileName != null) {
                        var isReadOnly = Helpers.GetReadOnly(fileName);
                        if (isReadOnly == null) {
                            Helpers.ScaleToolstripItem(menuItem, "picNonexistent");
                        } else if (isReadOnly == true) {
                            Helpers.ScaleToolstripItem(menuItem, "mnuReadOnly");
                        } else {
                            menuItem.Image = null;
                        }
                    }
                }
            }
        }

        private void mnuOpen_Click(object sender, EventArgs e) {
            if (SaveIfNeeded() != DialogResult.OK) { return; }
            using (var frm = new OpenFileDialog() { AddExtension = true, AutoUpgradeEnabled = true, Filter = "Bimil and PasswordSafe files|*.bimil;*.psafe3|Bimil files|*.bimil|Password Safe files|*.psafe3|All files|*.*", RestoreDirectory = true, ShowReadOnly = true, ReadOnlyChecked = this.ShowNextOpenFileDialogWithReadOnly }) {
                if (this.DocumentFileName != null) { //use the same directory as the currently opened file
                    frm.InitialDirectory = Path.GetDirectoryName(this.DocumentFileName);
                } else if (App.Recent.Count > 0) { //default to the most recently used directory
                    frm.InitialDirectory = Path.GetDirectoryName(App.Recent[0].FileName);
                }
                this.ShowNextOpenFileDialogWithReadOnly = false;
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    var isReadOnly = frm.ReadOnlyChecked || (Helpers.GetReadOnly(frm.FileName) == true);
                    if (LoadFile(frm.FileName, isReadOnly: isReadOnly)) {
                        if ((this.Document.LastSaveApplication == null) || !this.Document.LastSaveApplication.StartsWith("Bimil ")) {
                            var application = string.IsNullOrEmpty(this.Document.LastSaveApplication) ? "unknown" : this.Document.LastSaveApplication;
                            Medo.MessageBox.ShowWarning(this, "Be careful when saving files not created by Bimil.\n\nWhile such files can be used, not necessarily all the features of the original software will be supported. Likewise, files edited by Bimil won't be necessarily fully readable by other software; e.g. two-factor keys are only supported in Bimil.\n\nLast edited by:\n" + application);
                        }
                    }
                }
            }
        }

        private bool ShowNextOpenFileDialogWithReadOnly;


        private bool LoadFile(string fileName, bool isReadOnly = false) { //return false if file cannot be open
            DocumentResult document = null;
            string password;

            var isFileReadOnly = Helpers.GetReadOnly(fileName);
            try {
                if (isFileReadOnly == null) {
                    foreach (var recentFile in App.Recent.Files) {
                        if (string.Equals(fileName, recentFile.FileName, StringComparison.OrdinalIgnoreCase)) {
                            if (Medo.MessageBox.ShowQuestion(this, "File " + Path.GetFileName(fileName) + " could not be open.\nDo you wish to remove it from the recent list?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                                App.Recent.Remove(recentFile.FileName);
                                this.RefreshFiles();
                                break;
                            }
                        }
                    }
                    return false;
                }

                var title = Helpers.GetFileTitle(fileName);

                while (true) {
                    using (var frm = new PasswordForm(title)) {
                        if (frm.ShowDialog(this) == DialogResult.OK) {
                            password = frm.Password;
                        } else {
                            return false;
                        }
                    }

                    if (fileName.EndsWith(".bimil", StringComparison.OrdinalIgnoreCase)) {
                        if ((document = LoadPasswordSafeFile(fileName, password)) != null) {
                            return true;
                        } else if ((document = LoadBimilFile(fileName, password)) != null) {
                            return true;
                        }
                    } else { //Password Safe
                        if ((document = LoadPasswordSafeFile(fileName, password)) != null) {
                            return true;
                        }
                    }

                    Medo.MessageBox.ShowError(this, "Either password is wrong or file is damaged.");
                }
            } catch (IOException ex) {
                Medo.MessageBox.ShowError(this, "Cannot open file.\n\n" + ex.Message);
                return false;
            } catch (SecurityException ex) {
                Medo.MessageBox.ShowError(this, "Cannot open file.\n\n" + ex.Message);
                return false;
            } finally {
                password = null;
                GC.Collect(); //in attempt to kill password string

                if (document != null) {
                    this.Document = document.Document;
                    this.DocumentFileName = document.FileName;
                    SetReadonly(isReadOnly || (isFileReadOnly == true));
                    this.DocumentReadOnlyChanged = false;
                }

                RefreshCategories();
                RefreshItems();
                UpdateMenu();
                cmbSearch.Select();
            }
        }

        private void SetReadonly(bool isReadOnly) {
            this.Document.IsReadOnly = isReadOnly; //file status is changed upon save
            this.DocumentReadOnlyChanged = true;

            cmbSearch.BackColor = isReadOnly ? SystemColors.Control : SystemColors.Window;
            lsvEntries.BackColor = isReadOnly ? SystemColors.Control : SystemColors.Window;
            lsvEntries.LabelEdit = !isReadOnly;

            cmbSearch.Text = "";
            mnuView.Visible = !isReadOnly;
            mnuEdit.Visible = !isReadOnly;
        }


        private DocumentResult LoadPasswordSafeFile(string fileName, string password = null) {
            try {
                Document document;
                using (var fileStream = File.OpenRead(fileName)) {
                    document = Document.Load(fileStream, password);
                    document.TrackAccess = false;

                    if (document.LastSaveApplication.StartsWith("Bimil ")) { //convert temporary password safe fields to permanent ones
                        foreach (var entry in document.Entries) {
                            if (entry.Records.Contains((RecordType)TemporaryRecordType.TwoFactorKey)) {
                                var buffer = new byte[1024];
                                try {
                                    OneTimePassword.FromBase32(entry[(RecordType)TemporaryRecordType.TwoFactorKey].Text, buffer, out var bytesLength);
                                    var bytes = new byte[bytesLength];
                                    try {
                                        Buffer.BlockCopy(buffer, 0, bytes, 0, bytes.Length);
                                        entry.TwoFactorKey = bytes;
                                    } finally {
                                        Array.Clear(bytes, 0, bytes.Length);
                                    }
                                } catch (FormatException) {
                                    Medo.MessageBox.ShowWarning(this, string.Format("Cannot convert 2-factor key {0} in entry {1}", entry[(RecordType)TemporaryRecordType.TwoFactorKey].Text, entry.Title));
                                    if (entry.Notes.Length > 0) { entry.Notes += "\n"; }
                                    entry.Notes += "Two factor key: " + entry[(RecordType)TemporaryRecordType.TwoFactorKey].Text;
                                } finally {
                                    Array.Clear(buffer, 0, buffer.Length);
                                }
                                entry[(RecordType)TemporaryRecordType.TwoFactorKey] = null;
                            }
                            if (entry.Records.Contains((RecordType)TemporaryRecordType.CreditCardNumber)) {
                                entry.CreditCardNumber = entry[(RecordType)TemporaryRecordType.CreditCardNumber].Text;
                                entry[(RecordType)TemporaryRecordType.CreditCardNumber] = null;
                            }
                            if (entry.Records.Contains((RecordType)TemporaryRecordType.CreditCardExpiration)) {
                                entry.CreditCardExpiration = entry[(RecordType)TemporaryRecordType.CreditCardExpiration].Text;
                                entry[(RecordType)TemporaryRecordType.CreditCardExpiration] = null;
                            }
                            if (entry.Records.Contains((RecordType)TemporaryRecordType.CreditCardSecurityCode)) {
                                entry.CreditCardVerificationValue = entry[(RecordType)TemporaryRecordType.CreditCardSecurityCode].Text;
                                entry[(RecordType)TemporaryRecordType.CreditCardSecurityCode] = null;
                            }
                            if (entry.Records.Contains((RecordType)TemporaryRecordType.CreditCardPin)) {
                                entry.CreditCardPin = entry[(RecordType)TemporaryRecordType.CreditCardPin].Text;
                                entry[(RecordType)TemporaryRecordType.CreditCardPin] = null;
                            }
                        }
                    }
                }

                App.Recent.Push(fileName);
                RefreshFiles();

                return new DocumentResult(document, fileName);
            } catch (FormatException) {
                return null;
            }
        }

        private DocumentResult LoadBimilFile(string fileName, string password = null) {
            try {
                var legacyDoc = LegacyFile.BimilDocument.Open(fileName, password);
                return new DocumentResult(DocumentConversion.ConvertFromBimil(legacyDoc, password), fileName);
            } catch (FormatException) {
                return null;
            }
        }

        private void mnuSave_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            if (this.DocumentFileName != null) {
                try {
                    if (Helpers.GetReadOnly(this.DocumentFileName) == true) { Helpers.SetReadOnly(this.DocumentFileName, false); } //remove read-only before saving
                    using (var fileStream = new FileStream(this.DocumentFileName, FileMode.Create, FileAccess.Write)) {
                        this.Document.Save(fileStream);
                    }
                    if (this.Document.IsReadOnly) { Helpers.SetReadOnly(this.DocumentFileName, true); }
                    this.DocumentReadOnlyChanged = false;
                } catch (SystemException ex) {
                    Medo.MessageBox.ShowError(this, "Cannot save file.\n" + ex.Message);
                }
                UpdateMenu();
            } else {
                mnuSaveAs_Click(null, null);
            }
            cmbSearch.Select();
        }

        private void mnuSaveAs_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            using (var frm = new SaveFileDialog() { AddExtension = true, AutoUpgradeEnabled = true, Filter = "Bimil files|*.bimil|Password Safe files|*.psafe3|All files|*.*", RestoreDirectory = true }) {
                if (this.DocumentFileName != null) {
                    frm.InitialDirectory = Path.GetDirectoryName(this.DocumentFileName);
                    frm.FileName = Path.GetFileName(this.DocumentFileName);
                } else if (App.Recent.Count > 0) { //default to the most recently used directory
                    frm.InitialDirectory = Path.GetDirectoryName(App.Recent[0].FileName);
                }
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    if (Helpers.GetReadOnly(frm.FileName) == true) { Helpers.SetReadOnly(frm.FileName, false); } //remove read-only before saving
                    using (var fileStream = new FileStream(frm.FileName, FileMode.Create, FileAccess.Write)) {
                        this.Document.Save(fileStream);
                    }
                    if (this.Document.IsReadOnly) { Helpers.SetReadOnly(frm.FileName, true); }
                    this.DocumentFileName = frm.FileName;
                    this.DocumentReadOnlyChanged = false;
                    App.Recent.Push(this.DocumentFileName);
                    RefreshFiles();
                    UpdateMenu();
                }
            }

            cmbSearch.Select();
        }


        private void mnuPropertiesMenu_DropDownOpening(object sender, EventArgs e) {
            mnuChangePassword.Enabled = !this.Document.IsReadOnly;
            mnuReadOnly.Checked = this.Document.IsReadOnly;
        }

        private void mnuChangePassword_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            try {
                using (var frm = new PasswordForm()) {
                    if (frm.ShowDialog(this) == DialogResult.OK) {
                        if (!this.Document.ValidatePassphrase(frm.Password)) {
                            Medo.MessageBox.ShowError(this, "Old password does not match.");
                            return;
                        }
                    } else {
                        return;
                    }
                }
            } catch (FormatException) {
            } finally {
                GC.Collect(); //in attempt to kill password string
            }

            try {
                using (var frm = new NewPasswordForm()) {
                    if (frm.ShowDialog(this) == DialogResult.OK) {
                        using (var stream = new MemoryStream()) {
                            this.Document.ChangePassphrase(frm.Password);
                        }
                    }
                }
            } finally {
                GC.Collect(); //in attempt to kill password string
            }

            RefreshCategories();
            RefreshItems((lsvEntries.SelectedItems.Count > 0) ? new Entry[] { (Entry)(lsvEntries.SelectedItems[0].Tag) } : null);
            UpdateMenu();
            cmbSearch.Select();
        }

        private void mnuReadOnly_Click(object sender, EventArgs e) {
            SetReadonly(mnuReadOnly.Checked);
            UpdateMenu();
        }

        private void mnuProperties_Click(object sender, EventArgs e) {
            using (var frm = new DocumentInfoForm(this.Document)) {
                frm.ShowDialog(this);
            }
        }

        private void mnuAdd_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            //determine current category
            var categoryText = cmbSearch.Text.Trim();
            foreach (var category in this.Categories) {
                if (category.Equals(categoryText, StringComparison.CurrentCultureIgnoreCase)) {
                    categoryText = category;
                    break;
                }
            }

            using (var frm = new SelectTemplateForm()) {
                if (frm.ShowDialog(this) == DialogResult.OK) {
                    AddItem(frm.Template.RecordTypes, categoryText);
                }
            }
        }


        private void mnuView_Click(object sender, EventArgs e) {
            ShowEntry(false);
        }

        private void mnuEdit_Click(object sender, EventArgs e) {
            ShowEntry(true);
        }

        private void mnuRemove_Click(object sender, EventArgs e) {
            var isAnyEntrySelected = (lsvEntries.SelectedItems.Count >= 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            if ((this.Document == null) || !isAnyEntrySelected) { return; }

            for (var i = lsvEntries.SelectedItems.Count - 1; i >= 0; i--) {
                if (lsvEntries.SelectedItems[i].Tag is Entry item) {
                    this.Document.Entries.Remove(item);
                    lsvEntries.Items.Remove(lsvEntries.SelectedItems[i]);
                }
            }
            UpdateMenu();
            cmbSearch.Select();
        }


        private void mnuSearch_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            using (var frm = new SearchForm(this.Document, this.Categories, cmbSearch.Text)) {
                frm.ShowDialog(this);
            }
            RefreshCategories();
        }


        private void mnuSearchWeak_Click(object sender, EventArgs e) {
            if (this.Document == null) { return; }

            using (var frm = new SearchWeakForm(this.Document, this.Categories)) {
                frm.ShowDialog(this);
            }
            RefreshCategories();
        }


        private void mnuGeneratePassword_Click(object sender, EventArgs e) {
            using (var frm = new PasswordGeneratorForm(noSave: true)) {
                frm.ShowDialog(this);
            }
        }


        private void mnuAppOptions_Click(object sender, EventArgs e) {
            using (var frm = new SettingsForm()) {
                frm.ShowDialog(this);
            }
            UpdateMenu();
            cmbSearch.Select();
        }

        private void mnuAppFeedback_Click(object sender, EventArgs e) {
            Medo.Diagnostics.ErrorReport.ShowDialog(this, null, new Uri("https://medo64.com/feedback/"));
            cmbSearch.Select();
        }

        private void mnuAppUpgrade_Click(object sender, EventArgs e) {
            Medo.Services.Upgrade.ShowDialog(this, new Uri("https://medo64.com/upgrade/"));
        }

        private void mnuAppAbout_Click(object sender, EventArgs e) {
            Medo.Windows.Forms.AboutBox.ShowDialog(this, new Uri("https://www.medo64.com/bimil/"));
            cmbSearch.Select();
        }


        private void mnuAppDebugRandomizeAllPasswords_Click(object sender, EventArgs e) {
            var rnd = RandomNumberGenerator.Create();
            if (this.Document != null) {
                foreach (var entry in this.Document.Entries) {
                    entry.PasswordHistory.Clear();
                    while (entry.Records.Contains(RecordType.TwoFactorKey)) {
                        entry.Records.Remove(entry.Records[RecordType.TwoFactorKey]);
                    }
                    foreach (var record in entry.Records) {
                        if (record.RecordType == RecordType.Password) {
                            var bytes = new byte[10];
                            rnd.GetBytes(bytes);
                            var password = Convert.ToBase64String(bytes);
                            var shortenLength = bytes[0] % 4;
                            record.Text = password.Substring(0, password.Length - shortenLength);
                        }
                    }
                }
                Medo.MessageBox.ShowInformation(this, "Passwords randomized.");
            } else {
                Medo.MessageBox.ShowWarning(this, "No document!");
            }
        }


        private void mnxEntry_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            if (Helpers.IsRunningOnMono) {
                mnxEntryAutotypeSeparator.Visible = false;
                mnxEntryAutotype.Visible = false;
            }

            if (Settings.EditableByDefault) {
                mnxEntryView.Font = new Font(mnxEntryView.Font, FontStyle.Regular);
                mnxEntryEdit.Font = new Font(mnxEntryView.Font, FontStyle.Bold);
                mnxEntryView.ShortcutKeyDisplayString = "";
                mnxEntryEdit.ShortcutKeyDisplayString = "F4";
            } else {
                mnxEntryView.Font = new Font(mnxEntryView.Font, FontStyle.Bold);
                mnxEntryEdit.Font = new Font(mnxEntryView.Font, FontStyle.Regular);
                mnxEntryView.ShortcutKeyDisplayString = "F4";
                mnxEntryEdit.ShortcutKeyDisplayString = "";
            }

            var isAnyEntrySelected = (lsvEntries.SelectedItems.Count >= 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            var isSingleEntrySelected = (lsvEntries.SelectedItems.Count == 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            mnxEntryView.Enabled = isSingleEntrySelected;
            mnxEntryEdit.Enabled = isSingleEntrySelected;
            mnxEntryAdd.Enabled = true;
            mnxEntryAddSimilar.Enabled = isSingleEntrySelected;
            mnxEntryCut.Enabled = isAnyEntrySelected;
            mnxEntryCopy.Enabled = isAnyEntrySelected;
            mnxEntryPaste.Enabled = ClipboardHelper.HasDataOnClipboard;
            mnxEntryAutotype.Enabled = isSingleEntrySelected;

            for (var i = mnxEntry.Items.IndexOf(mnxEntrySeparatorBeforeCut) - 1; i > mnxEntry.Items.IndexOf(mnxEntryAutotype); i--) {
                mnxEntry.Items.RemoveAt(i);
            }

            var nextMenuIndex = mnxEntry.Items.IndexOf(mnxEntryAutotype) + 1;
            var hasCopyEntries = false;
            var entry = (lsvEntries.SelectedItems.Count == 1) ? lsvEntries.SelectedItems[0].Tag as Entry : null;
            if (entry != null) {
                foreach (var record in entry.Records) {
                    ToolStripMenuItem mnxItem = null;
                    var isFilteredCopy = false;
                    switch (record.RecordType) {
                        case RecordType.UserName:
                        case RecordType.EmailAddress:
                            if (record.Text.Length > 0) {
                                mnxItem = new ToolStripMenuItem("Copy " + Helpers.ToTitleCase(Helpers.GetRecordCaption(record)) + " (" + record.Text + ")");
                                mnxItem.Click += delegate { ClipboardHelper.SetClipboardText(this, record.Text, record.RecordType); };
                            }
                            break;

                        case RecordType.Password:
                            if (record.Text.Length > 0) {
                                mnxItem = new ToolStripMenuItem("Copy " + Helpers.ToTitleCase(Helpers.GetRecordCaption(record)) + " (" + new string('*', record.Text.Length) + ")");
                                mnxItem.Click += delegate { ClipboardHelper.SetClipboardText(this, record.Text, record.RecordType); };
                            }
                            break;

                        case RecordType.TwoFactorKey:
                            var keyBytes = record.GetBytes();
                            if (keyBytes.Length > 0) {
                                try {
                                    var code = Helpers.GetTwoFactorCode(OneTimePassword.ToBase32(keyBytes, keyBytes.Length, SecretFormatFlags.None), space: true);
                                    mnxItem = new ToolStripMenuItem("Copy Two-factor Code (" + code + ")");
                                    isFilteredCopy = true;
                                } finally {
                                    Array.Clear(keyBytes, 0, keyBytes.Length);
                                }
                                mnxItem.Click += delegate {
                                    var keyBytes2 = record.GetBytes();
                                    try {
                                        //recalculate code - just in case menu was open too long and displayed code expired
                                        var code = Helpers.GetTwoFactorCode(OneTimePassword.ToBase32(keyBytes2, keyBytes2.Length, SecretFormatFlags.None));
                                        ClipboardHelper.SetClipboardText(this, code, record.RecordType);
                                    } finally {
                                        Array.Clear(keyBytes2, 0, keyBytes2.Length);
                                    }
                                };
                            }
                            break;
                    }

                    if (mnxItem != null) {
                        if (isFilteredCopy) {
                            mnxItem.Image = Properties.Resources.btnCopyFiltered_16;
                            mnxItem.Tag = "btnCopyFiltered";
                        } else {
                            mnxItem.Image = Properties.Resources.btnCopy_16;
                            mnxItem.Tag = "btnCopy";
                        }
                        if (!hasCopyEntries) { mnxEntry.Items.Insert(nextMenuIndex++, new ToolStripSeparator()); }
                        hasCopyEntries = true;
                        mnxEntry.Items.Insert(nextMenuIndex++, mnxItem);
                    }
                }

                { //add URL items
                    var hasSeparator = (mnxEntry.Items[mnxEntry.Items.Count - 1] is ToolStripSeparator);
                    foreach (var record in entry.Records) {
                        if ((record.RecordType == RecordType.Url) && (record.Text.Length > 0)) {
                            if (!hasSeparator) {
                                mnxEntry.Items.Insert(nextMenuIndex++, new ToolStripSeparator());
                                hasSeparator = true;
                            }
                            var mnxItem = new ToolStripMenuItem("Go to " + record.Text) {
                                Image = Bimil.Properties.Resources.btnExecuteUrl_16,
                                Tag = "btnExecuteUrl"
                            };
                            mnxItem.Click += delegate { Execute.StartUrl(record.Text); };
                            mnxEntry.Items.Insert(nextMenuIndex++, mnxItem);
                        }
                    }
                }

                { //add RunCommand items
                    var hasSeparator = (mnxEntry.Items[mnxEntry.Items.Count - 1] is ToolStripSeparator);
                    foreach (var record in entry.Records) {
                        if ((record.RecordType == RecordType.RunCommand) && (record.Text.Length > 0)) {
                            if (!hasSeparator) {
                                mnxEntry.Items.Insert(nextMenuIndex++, new ToolStripSeparator());
                                hasSeparator = true;
                            }
                            var mnxItem = new ToolStripMenuItem("Execute " + Execute.GetStartInfo(record.Text).FileName) {
                                Image = Bimil.Properties.Resources.btnExecute_16,
                                Tag = "btnExecute"
                            };
                            mnxItem.Click += delegate { Execute.StartCommand(record.Text, this); };
                            mnxEntry.Items.Insert(nextMenuIndex++, mnxItem);
                        }
                    }
                }
            }

            Helpers.ScaleToolstrip(mnxEntry);
        }

        private void mnxEntryView_Click(object sender, EventArgs e) {
            ShowEntry(false);
        }

        private void mnxEntryEdit_Click(object sender, EventArgs e) {
            ShowEntry(true);
        }

        private void mnxEntryAdd_Click(object sender, EventArgs e) {
            mnuAdd_Click(null, null);
        }

        private void mnxEntryAddSimilar_Click(object sender, EventArgs e) {
            var isSingleEntrySelected = (lsvEntries.SelectedItems.Count == 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            if ((this.Document == null) || !isSingleEntrySelected) { return; }

            var entry = (Entry)(lsvEntries.SelectedItems[0].Tag);
            var recordTypes = new List<RecordType>();
            foreach (var record in entry.Records) {
                recordTypes.Add(record.RecordType);
            }

            AddItem(recordTypes, entry.Group);
        }

        private void mnxEntryCut_Click(object sender, EventArgs e) {
            var isAnyEntrySelected = (lsvEntries.SelectedItems.Count >= 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            if ((this.Document == null) || !isAnyEntrySelected) { return; }

            var entries = new List<Entry>();
            foreach (ListViewItem selectedItem in lsvEntries.SelectedItems) {
                entries.Add((Entry)(selectedItem.Tag));
            }
            ClipboardHelper.SetClipboardData(this, entries.AsReadOnly(), sensitiveData: true);

            for (var i = lsvEntries.Items.Count - 1; i >= 0; i--) {
                if (lsvEntries.Items[i].Selected) {
                    var entry = (Entry)(lsvEntries.Items[i].Tag);
                    this.Document.Entries.Remove(entry);
                    lsvEntries.Items.RemoveAt(i);
                }
            }
        }

        private void mnxEntryCopy_Click(object sender, EventArgs e) {
            var isAnyEntrySelected = (lsvEntries.SelectedItems.Count >= 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            if ((this.Document == null) || !isAnyEntrySelected) { return; }

            var entries = new List<Entry>();
            foreach (ListViewItem selectedItem in lsvEntries.SelectedItems) {
                entries.Add((Entry)(selectedItem.Tag));
            }
            ClipboardHelper.SetClipboardData(this, entries.AsReadOnly(), sensitiveData: true);
        }

        private void mnxEntryPaste_Click(object sender, EventArgs e) {
            string category = null;

            var isAnyEntrySelected = (lsvEntries.SelectedItems.Count >= 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            if (isAnyEntrySelected) { //put pasted item in the same category as selected
                var selectedEntry = (Entry)(lsvEntries.SelectedItems[0].Tag);
                category = selectedEntry.Group;
            }

            if (category == null) {
                foreach (String item in cmbSearch.Items) { //you must run over all items to determine current category (case-insensitive)
                    if (string.Equals(cmbSearch.Text, item, StringComparison.CurrentCultureIgnoreCase)) {
                        category = item;
                        break;
                    }
                }
            }

            var entries = new List<Entry>(ClipboardHelper.GetClipboardData());
            foreach (var entry in entries) {
                if (!string.IsNullOrEmpty(category)) { entry.Group = category; } //switch category of pasted item to currently selected category
                this.Document.Entries.Add(entry);
            }
            RefreshCategories();
            RefreshItems(entries.AsReadOnly());
        }

        private void mnxEntryAutotype_Click(object sender, EventArgs e) {
            var isSingleEntrySelected = (lsvEntries.SelectedItems.Count == 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            if ((this.Document == null) || !isSingleEntrySelected) { return; }

            var entry = (Entry)(lsvEntries.SelectedItems[0].Tag);

            var frm = new AutotypeForm(entry);
            frm.Shown += delegate {
                this.Visible = false;
            };
            frm.FormClosed += delegate {
                this.Visible = true;
                this.Select();
            };
            frm.Show();
        }

        #endregion


        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e) {
            RefreshItems();
        }


        private void AddItem(IEnumerable<RecordType> recordTypes, string categoryText) {
            if (this.Document == null) { return; }

            var entry = new Entry("New item");
            entry.Records[RecordType.Password] = null;
            this.Document.Entries.Add(entry);
            foreach (var recordType in recordTypes) {
                entry.Records.Add(new Record(recordType));
            }

            if (Settings.SavePasswordHistoryByDefault) {
                entry.PasswordHistory.MaximumCount = Settings.SavePasswordHistoryDefaultCount;
                entry.PasswordHistory.Enabled = true;
            }

            using (var frm2 = new ItemForm(this.Document, entry, this.Categories, startsAsEditable: true, isNew: true, defaultCategory: categoryText)) {
                if (frm2.ShowDialog(this) == DialogResult.OK) {
                    RefreshItems(new Entry[] { entry });
                    RefreshCategories();
                } else {
                    this.Document.Entries.Remove(entry);
                }
            }

            UpdateMenu();
            cmbSearch.Select();
        }

        private void RefreshCategories() {
            this.Categories.Clear();
            if (this.Document != null) {
                this.Categories.Add("");
                foreach (var entry in this.Document.Entries) {
                    var cached = new EntryCache(entry);
                    if (!this.Categories.Contains(cached.Group)) {
                        this.Categories.Add(cached.Group);
                    }
                }
            }
            this.Categories.Sort();

            cmbSearch.BeginUpdate();
            cmbSearch.Items.Clear();
            foreach (var category in this.Categories) {
                cmbSearch.Items.Add(category);
            }
            cmbSearch.EndUpdate();
        }

        private void RefreshItems(IEnumerable<Entry> entriesToSelect = null) {
            Helpers.PerformEntrySearch(this.Document, lsvEntries, cmbSearch.Text, entriesToSelect);
            var shownCount = lsvEntries.Items.Count;
            var totalCount = (this.Document != null) ? this.Document.Entries.Count : 0;
            if (shownCount > 0) {
                tip.SetToolTip(cmbSearch, $"{shownCount} " + ((shownCount == 1) ? "entry" : "entries") + $" shown (out of {totalCount}).");
            } else {
                tip.SetToolTip(cmbSearch, null);
            }
            Form_Resize(null, null); //to support both ListView full row with and without scrollbar
        }


        private void UpdateMenu() {
            var isAnyEntrySelected = (lsvEntries.SelectedItems.Count >= 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            var isSingleEntrySelected = (lsvEntries.SelectedItems.Count == 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);

            mnuView.Visible = !Settings.EditableByDefault || ((this.Document != null) && this.Document.IsReadOnly);
            mnuEdit.Visible = !mnuView.Visible;

            mnuSaveMenu.Enabled = (this.Document != null);
            mnuSaveMenu.Visible = mnuSaveMenu.Enabled;
            mnuSaveAlone.Visible = !mnuSaveMenu.Visible;

            mnuPropertiesMenu.Enabled = (this.Document != null);
            mnuAdd.Enabled = (this.Document != null) && (!this.Document.IsReadOnly);
            mnuView.Enabled = (this.Document != null) && isSingleEntrySelected;
            mnuEdit.Enabled = (this.Document != null) && isSingleEntrySelected;
            mnuRemove.Enabled = (this.Document != null) && isAnyEntrySelected && (!this.Document.IsReadOnly);
            mnuSearchRoot.Enabled = (this.Document != null);

            pnlDocument.Visible = (this.Document != null);

            if (this.DocumentFileName == null) {
                this.Text = (this.Document?.HasChanged ?? false) ? "Bimil*" : "Bimil";
            } else {
                var title = Helpers.GetFileTitle(this.DocumentFileName);
                this.Text = title + ((this.Document?.HasChanged ?? false) || this.DocumentReadOnlyChanged ? "*" : "") + " - Bimil";
            }
        }

        private void ShowEntry(bool startsAsEditable) {
            var isSingleEntrySelected = (lsvEntries.SelectedItems.Count == 1) && ((lsvEntries.SelectedItems[0].Tag as Entry) != null);
            if ((this.Document == null) || !isSingleEntrySelected) { return; }

            var item = lsvEntries.SelectedItems[0].Tag as Entry;
            if (item == null) { return; }
            using (var frm2 = new ItemForm(this.Document, item, this.Categories, startsAsEditable: startsAsEditable)) {
                var oldGroup = item.Group;
                if (frm2.ShowDialog(this) == DialogResult.OK) {
                    lsvEntries.SelectedItems[0].Text = item.Title;
                    RefreshCategories();
                    RefreshItems(new Entry[] { item });
                } else if (!string.Equals(oldGroup, item.Group, StringComparison.Ordinal)) {
                    RefreshCategories();
                    RefreshItems(new Entry[] { item });
                }
                UpdateMenu();
            }
        }


        private void ToggleMenu() {
            if (mnu.ContainsFocus) {
                if (lsvEntries.Visible) {
                    lsvEntries.Select();
                } else {
                    this.Select();
                }
            } else {
                mnu.Select();
                mnu.Items[0].Select();
            }
        }


        #region Upgrade

        private void bwUpgradeCheck_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
            e.Cancel = true;

            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 3000) { //wait for three seconds
                Thread.Sleep(100);
                if (bwUpgradeCheck.CancellationPending) { return; }
            }

            var file = Medo.Services.Upgrade.GetUpgradeFile(new Uri("https://medo64.com/upgrade/"));
            if (file != null) {
                if (bwUpgradeCheck.CancellationPending) { return; }
                e.Cancel = false;
            }
        }

        private void bwUpgradeCheck_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
            if (!e.Cancelled) {
                Helpers.ScaleToolstripItem(mnuApp, "mnuAppUpgrade");
                mnuAppUpgrade.Text = "Upgrade is available";

                var locationButton = this.PointToScreen(new Point(mnu.Left + mnuApp.Bounds.Left, mnu.Top + mnuApp.Bounds.Top));
                var locationForm = this.Location;
                var tipX = locationButton.X - locationForm.X + mnuApp.Bounds.Width / 2;
                var tipY = locationButton.Y - locationForm.Y + mnuApp.Bounds.Height / 2;

                tip.ToolTipIcon = ToolTipIcon.Info;
                tip.ToolTipTitle = "Upgrade";
                tip.Show("Upgrade is available.", this, tipX, tipY, 1729);
            }
        }

        #endregion


        private class DocumentResult {

            public DocumentResult(Document document, string fileName) {
                this.Document = document;
                this.FileName = fileName;
            }


            public Document Document { get; }
            public String FileName { get; }

        }


        public enum TemporaryRecordType {
            /// <summary>
            /// Not to be used.
            /// Two-factor authentication key.
            /// Encoded in base 32.
            /// </summary>
            TwoFactorKey = 0xf0,
            /// <summary>
            /// Not to be used.
            /// Credit card number.
            /// Number should consist of digits and spaces.
            /// </summary>
            CreditCardNumber = 0xf1,
            /// <summary>
            /// Not to be used.
            /// Credit card expiration.
            /// Expiration should be MM/YY, where MM is 01-12, and YY 00-99.
            /// </summary>
            CreditCardExpiration = 0xf2,
            /// <summary>
            /// Not to be used.
            /// Credit card verification value.
            /// CVV (CVV2) is three or four digits.
            /// </summary>
            CreditCardSecurityCode = 0xf3,
            /// <summary>
            /// Not to be used.
            /// Credit card PIN.
            /// PIN is four to twelve digits long (ISO-9564).
            /// </summary>
            CreditCardPin = 0xf4,
        }
    }
}
