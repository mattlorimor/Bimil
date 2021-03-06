namespace Bimil {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnu = new System.Windows.Forms.ToolStrip();
            this.mnuNew = new System.Windows.Forms.ToolStripButton();
            this.mnuOpenAlone = new System.Windows.Forms.ToolStripButton();
            this.mnuOpenMenu = new System.Windows.Forms.ToolStripSplitButton();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSaveAlone = new System.Windows.Forms.ToolStripButton();
            this.mnuSaveMenu = new System.Windows.Forms.ToolStripSplitButton();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPropertiesMenu = new System.Windows.Forms.ToolStripSplitButton();
            this.mnuChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReadOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu0 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAdd = new System.Windows.Forms.ToolStripButton();
            this.mnuView = new System.Windows.Forms.ToolStripButton();
            this.mnuEdit = new System.Windows.Forms.ToolStripButton();
            this.mnuRemove = new System.Windows.Forms.ToolStripButton();
            this.mnuApp = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuAppOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuApp0 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSendFeedback = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAppUpgrade = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuApp1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAppAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAppDebug = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAppDebugRandomizeAllPasswords = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSearchRoot = new System.Windows.Forms.ToolStripSplitButton();
            this.mnuSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSearchWeak = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGeneratePassword = new System.Windows.Forms.ToolStripButton();
            this.pnlDocument = new System.Windows.Forms.Panel();
            this.lsvEntries = new System.Windows.Forms.ListView();
            this.lsvEntries_colTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnxEntry = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnxEntryView = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxEntryEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxEntry0 = new System.Windows.Forms.ToolStripSeparator();
            this.mnxEntryAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxEntryAddSimilar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxEntryAutotypeSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mnxEntryAutotype = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxEntrySeparatorBeforeCut = new System.Windows.Forms.ToolStripSeparator();
            this.mnxEntryCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxEntryCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnxEntryPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.bwUpgradeCheck = new System.ComponentModel.BackgroundWorker();
            this.tmrClose = new System.Windows.Forms.Timer(this.components);
            this.tip = new System.Windows.Forms.ToolTip(this.components);
            this.mnu.SuspendLayout();
            this.pnlDocument.SuspendLayout();
            this.mnxEntry.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnu
            // 
            this.mnu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mnu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpenAlone,
            this.mnuOpenMenu,
            this.mnuSaveAlone,
            this.mnuSaveMenu,
            this.mnuPropertiesMenu,
            this.mnu0,
            this.mnuAdd,
            this.mnuView,
            this.mnuEdit,
            this.mnuRemove,
            this.mnuApp,
            this.mnu1,
            this.mnuSearchRoot,
            this.mnuGeneratePassword});
            this.mnu.Location = new System.Drawing.Point(0, 0);
            this.mnu.Name = "mnu";
            this.mnu.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.mnu.Size = new System.Drawing.Size(582, 27);
            this.mnu.TabIndex = 1;
            // 
            // mnuNew
            // 
            this.mnuNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuNew.Image = global::Bimil.Properties.Resources.mnuNew_16;
            this.mnuNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(24, 24);
            this.mnuNew.Text = "New";
            this.mnuNew.ToolTipText = "New (Ctrl+N)";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpenAlone
            // 
            this.mnuOpenAlone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuOpenAlone.Image = global::Bimil.Properties.Resources.mnuOpen_16;
            this.mnuOpenAlone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuOpenAlone.Name = "mnuOpenAlone";
            this.mnuOpenAlone.Size = new System.Drawing.Size(24, 24);
            this.mnuOpenAlone.Tag = "mnuOpen";
            this.mnuOpenAlone.Text = "Open";
            this.mnuOpenAlone.ToolTipText = "Open (Ctrl+O)";
            this.mnuOpenAlone.Visible = false;
            this.mnuOpenAlone.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuOpenMenu
            // 
            this.mnuOpenMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuOpenMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.toolStripMenuItem2});
            this.mnuOpenMenu.Image = global::Bimil.Properties.Resources.mnuOpen_16;
            this.mnuOpenMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuOpenMenu.Name = "mnuOpenMenu";
            this.mnuOpenMenu.Size = new System.Drawing.Size(39, 24);
            this.mnuOpenMenu.Tag = "mnuOpen";
            this.mnuOpenMenu.Text = "Open";
            this.mnuOpenMenu.ToolTipText = "Open (Ctrl+O)";
            this.mnuOpenMenu.ButtonClick += new System.EventHandler(this.mnuOpen_Click);
            this.mnuOpenMenu.DropDownOpening += new System.EventHandler(this.mnuOpenMenu_DropDownOpening);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Image = global::Bimil.Properties.Resources.mnuOpen_16;
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.ShortcutKeyDisplayString = "Ctrl+O";
            this.mnuOpen.Size = new System.Drawing.Size(173, 26);
            this.mnuOpen.Text = "Open";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(170, 6);
            // 
            // mnuSaveAlone
            // 
            this.mnuSaveAlone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuSaveAlone.Enabled = false;
            this.mnuSaveAlone.Image = global::Bimil.Properties.Resources.mnuSave_16;
            this.mnuSaveAlone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSaveAlone.Name = "mnuSaveAlone";
            this.mnuSaveAlone.Size = new System.Drawing.Size(24, 24);
            this.mnuSaveAlone.Tag = "mnuSave";
            this.mnuSaveAlone.Text = "Save";
            this.mnuSaveAlone.ToolTipText = "Save (Ctrl+S)";
            this.mnuSaveAlone.Visible = false;
            // 
            // mnuSaveMenu
            // 
            this.mnuSaveMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuSaveMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSave,
            this.mnuSaveAs});
            this.mnuSaveMenu.Image = global::Bimil.Properties.Resources.mnuSave_16;
            this.mnuSaveMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSaveMenu.Name = "mnuSaveMenu";
            this.mnuSaveMenu.Size = new System.Drawing.Size(39, 24);
            this.mnuSaveMenu.Tag = "mnuSave";
            this.mnuSaveMenu.Text = "Save";
            this.mnuSaveMenu.ToolTipText = "Save (Ctrl+S)";
            this.mnuSaveMenu.ButtonClick += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Image = global::Bimil.Properties.Resources.mnuSave_16;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeyDisplayString = "Ctrl+S";
            this.mnuSave.Size = new System.Drawing.Size(165, 26);
            this.mnuSave.Text = "Save";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(165, 26);
            this.mnuSaveAs.Tag = "";
            this.mnuSaveAs.Text = "Save as";
            this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
            // 
            // mnuPropertiesMenu
            // 
            this.mnuPropertiesMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuPropertiesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuChangePassword,
            this.mnuReadOnly,
            this.toolStripMenuItem1,
            this.mnuProperties});
            this.mnuPropertiesMenu.Image = global::Bimil.Properties.Resources.mnuProperties_16;
            this.mnuPropertiesMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuPropertiesMenu.Name = "mnuPropertiesMenu";
            this.mnuPropertiesMenu.Size = new System.Drawing.Size(39, 24);
            this.mnuPropertiesMenu.Tag = "mnuProperties";
            this.mnuPropertiesMenu.Text = "Properties";
            this.mnuPropertiesMenu.ToolTipText = "File properties";
            this.mnuPropertiesMenu.ButtonClick += new System.EventHandler(this.mnuProperties_Click);
            this.mnuPropertiesMenu.DropDownOpening += new System.EventHandler(this.mnuPropertiesMenu_DropDownOpening);
            // 
            // mnuChangePassword
            // 
            this.mnuChangePassword.Image = global::Bimil.Properties.Resources.mnuChangePassword_16;
            this.mnuChangePassword.Name = "mnuChangePassword";
            this.mnuChangePassword.Size = new System.Drawing.Size(201, 26);
            this.mnuChangePassword.Text = "Change password";
            this.mnuChangePassword.Click += new System.EventHandler(this.mnuChangePassword_Click);
            // 
            // mnuReadOnly
            // 
            this.mnuReadOnly.CheckOnClick = true;
            this.mnuReadOnly.Image = global::Bimil.Properties.Resources.mnuReadOnly_16;
            this.mnuReadOnly.Name = "mnuReadOnly";
            this.mnuReadOnly.Size = new System.Drawing.Size(201, 26);
            this.mnuReadOnly.Text = "Read-only";
            this.mnuReadOnly.Click += new System.EventHandler(this.mnuReadOnly_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(198, 6);
            // 
            // mnuProperties
            // 
            this.mnuProperties.Image = global::Bimil.Properties.Resources.mnuProperties_16;
            this.mnuProperties.Name = "mnuProperties";
            this.mnuProperties.Size = new System.Drawing.Size(201, 26);
            this.mnuProperties.Tag = "";
            this.mnuProperties.Text = "Properties";
            this.mnuProperties.Click += new System.EventHandler(this.mnuProperties_Click);
            // 
            // mnu0
            // 
            this.mnu0.Name = "mnu0";
            this.mnu0.Size = new System.Drawing.Size(6, 27);
            // 
            // mnuAdd
            // 
            this.mnuAdd.Image = global::Bimil.Properties.Resources.mnuAdd_16;
            this.mnuAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuAdd.Name = "mnuAdd";
            this.mnuAdd.Size = new System.Drawing.Size(61, 24);
            this.mnuAdd.Text = "Add";
            this.mnuAdd.ToolTipText = "Add (Ins)";
            this.mnuAdd.Click += new System.EventHandler(this.mnuAdd_Click);
            // 
            // mnuView
            // 
            this.mnuView.Image = global::Bimil.Properties.Resources.mnuView_16;
            this.mnuView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuView.Name = "mnuView";
            this.mnuView.Size = new System.Drawing.Size(65, 24);
            this.mnuView.Text = "View";
            this.mnuView.ToolTipText = "View (F4)";
            this.mnuView.Visible = false;
            this.mnuView.Click += new System.EventHandler(this.mnuView_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Image = global::Bimil.Properties.Resources.mnuEdit_16;
            this.mnuEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(59, 24);
            this.mnuEdit.Text = "Edit";
            this.mnuEdit.ToolTipText = "Edit (F4)";
            this.mnuEdit.Click += new System.EventHandler(this.mnuEdit_Click);
            // 
            // mnuRemove
            // 
            this.mnuRemove.Image = global::Bimil.Properties.Resources.mnuRemove_16;
            this.mnuRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuRemove.Name = "mnuRemove";
            this.mnuRemove.Size = new System.Drawing.Size(87, 24);
            this.mnuRemove.Text = "Remove";
            this.mnuRemove.ToolTipText = "Remove (Del)";
            this.mnuRemove.Click += new System.EventHandler(this.mnuRemove_Click);
            // 
            // mnuApp
            // 
            this.mnuApp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuApp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuApp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAppOptions,
            this.mnuApp0,
            this.mnuSendFeedback,
            this.mnuAppUpgrade,
            this.mnuApp1,
            this.mnuAppAbout,
            this.mnuAppDebug,
            this.mnuAppDebugRandomizeAllPasswords});
            this.mnuApp.Image = global::Bimil.Properties.Resources.mnuApp_16;
            this.mnuApp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuApp.Name = "mnuApp";
            this.mnuApp.Size = new System.Drawing.Size(34, 24);
            this.mnuApp.Text = "Application";
            // 
            // mnuAppOptions
            // 
            this.mnuAppOptions.Name = "mnuAppOptions";
            this.mnuAppOptions.Size = new System.Drawing.Size(227, 26);
            this.mnuAppOptions.Text = "&Options";
            this.mnuAppOptions.Click += new System.EventHandler(this.mnuAppOptions_Click);
            // 
            // mnuApp0
            // 
            this.mnuApp0.Name = "mnuApp0";
            this.mnuApp0.Size = new System.Drawing.Size(224, 6);
            // 
            // mnuSendFeedback
            // 
            this.mnuSendFeedback.Name = "mnuSendFeedback";
            this.mnuSendFeedback.Size = new System.Drawing.Size(227, 26);
            this.mnuSendFeedback.Text = "Send &feedback";
            this.mnuSendFeedback.Click += new System.EventHandler(this.mnuAppFeedback_Click);
            // 
            // mnuAppUpgrade
            // 
            this.mnuAppUpgrade.Name = "mnuAppUpgrade";
            this.mnuAppUpgrade.Size = new System.Drawing.Size(227, 26);
            this.mnuAppUpgrade.Text = "Check for &upgrade";
            this.mnuAppUpgrade.Click += new System.EventHandler(this.mnuAppUpgrade_Click);
            // 
            // mnuApp1
            // 
            this.mnuApp1.Name = "mnuApp1";
            this.mnuApp1.Size = new System.Drawing.Size(224, 6);
            // 
            // mnuAppAbout
            // 
            this.mnuAppAbout.Name = "mnuAppAbout";
            this.mnuAppAbout.Size = new System.Drawing.Size(227, 26);
            this.mnuAppAbout.Text = "&About";
            this.mnuAppAbout.Click += new System.EventHandler(this.mnuAppAbout_Click);
            // 
            // mnuAppDebug
            // 
            this.mnuAppDebug.Name = "mnuAppDebug";
            this.mnuAppDebug.Size = new System.Drawing.Size(224, 6);
            this.mnuAppDebug.Visible = false;
            // 
            // mnuAppDebugRandomizeAllPasswords
            // 
            this.mnuAppDebugRandomizeAllPasswords.Name = "mnuAppDebugRandomizeAllPasswords";
            this.mnuAppDebugRandomizeAllPasswords.Size = new System.Drawing.Size(227, 26);
            this.mnuAppDebugRandomizeAllPasswords.Text = "Randomize all entries";
            this.mnuAppDebugRandomizeAllPasswords.Visible = false;
            this.mnuAppDebugRandomizeAllPasswords.Click += new System.EventHandler(this.mnuAppDebugRandomizeAllPasswords_Click);
            // 
            // mnu1
            // 
            this.mnu1.Name = "mnu1";
            this.mnu1.Size = new System.Drawing.Size(6, 27);
            // 
            // mnuSearchRoot
            // 
            this.mnuSearchRoot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuSearchRoot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSearch,
            this.mnuSearchWeak});
            this.mnuSearchRoot.Image = global::Bimil.Properties.Resources.mnuSearch_16;
            this.mnuSearchRoot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuSearchRoot.Name = "mnuSearchRoot";
            this.mnuSearchRoot.Size = new System.Drawing.Size(39, 24);
            this.mnuSearchRoot.Tag = "mnuSearch";
            this.mnuSearchRoot.Text = "Search";
            this.mnuSearchRoot.ToolTipText = "Search (Ctrl+F)";
            this.mnuSearchRoot.ButtonClick += new System.EventHandler(this.mnuSearch_Click);
            // 
            // mnuSearch
            // 
            this.mnuSearch.Image = global::Bimil.Properties.Resources.mnuSearch_16;
            this.mnuSearch.Name = "mnuSearch";
            this.mnuSearch.Size = new System.Drawing.Size(239, 26);
            this.mnuSearch.Text = "&Search";
            this.mnuSearch.Click += new System.EventHandler(this.mnuSearch_Click);
            // 
            // mnuSearchWeak
            // 
            this.mnuSearchWeak.Name = "mnuSearchWeak";
            this.mnuSearchWeak.Size = new System.Drawing.Size(239, 26);
            this.mnuSearchWeak.Text = "Search weak passwords";
            this.mnuSearchWeak.Click += new System.EventHandler(this.mnuSearchWeak_Click);
            // 
            // mnuGeneratePassword
            // 
            this.mnuGeneratePassword.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnuGeneratePassword.Image = global::Bimil.Properties.Resources.mnuGeneratePassword_16;
            this.mnuGeneratePassword.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnuGeneratePassword.Name = "mnuGeneratePassword";
            this.mnuGeneratePassword.Size = new System.Drawing.Size(24, 24);
            this.mnuGeneratePassword.Text = "Generate password";
            this.mnuGeneratePassword.Click += new System.EventHandler(this.mnuGeneratePassword_Click);
            // 
            // pnlDocument
            // 
            this.pnlDocument.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDocument.Controls.Add(this.lsvEntries);
            this.pnlDocument.Controls.Add(this.cmbSearch);
            this.pnlDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDocument.Location = new System.Drawing.Point(0, 27);
            this.pnlDocument.Name = "pnlDocument";
            this.pnlDocument.Size = new System.Drawing.Size(582, 366);
            this.pnlDocument.TabIndex = 0;
            // 
            // lsvEntries
            // 
            this.lsvEntries.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvEntries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lsvEntries_colTitle});
            this.lsvEntries.ContextMenuStrip = this.mnxEntry;
            this.lsvEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvEntries.FullRowSelect = true;
            this.lsvEntries.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvEntries.HideSelection = false;
            this.lsvEntries.LabelEdit = true;
            this.lsvEntries.LabelWrap = false;
            this.lsvEntries.Location = new System.Drawing.Point(0, 24);
            this.lsvEntries.Name = "lsvEntries";
            this.lsvEntries.Size = new System.Drawing.Size(582, 342);
            this.lsvEntries.TabIndex = 1;
            this.lsvEntries.UseCompatibleStateImageBehavior = false;
            this.lsvEntries.View = System.Windows.Forms.View.Details;
            this.lsvEntries.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lsvEntries_AfterLabelEdit);
            this.lsvEntries.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lsvEntries_BeforeLabelEdit);
            this.lsvEntries.ItemActivate += new System.EventHandler(this.lsvEntries_ItemActivate);
            this.lsvEntries.SelectedIndexChanged += new System.EventHandler(this.lsvEntries_SelectedIndexChanged);
            this.lsvEntries.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lsvEntries_KeyDown);
            // 
            // mnxEntry
            // 
            this.mnxEntry.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnxEntry.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnxEntryView,
            this.mnxEntryEdit,
            this.mnxEntry0,
            this.mnxEntryAdd,
            this.mnxEntryAddSimilar,
            this.mnxEntryAutotypeSeparator,
            this.mnxEntryAutotype,
            this.mnxEntrySeparatorBeforeCut,
            this.mnxEntryCut,
            this.mnxEntryCopy,
            this.mnxEntryPaste});
            this.mnxEntry.Name = "mnxEntry";
            this.mnxEntry.Size = new System.Drawing.Size(217, 258);
            this.mnxEntry.Opening += new System.ComponentModel.CancelEventHandler(this.mnxEntry_Opening);
            // 
            // mnxEntryView
            // 
            this.mnxEntryView.Image = global::Bimil.Properties.Resources.mnuView_16;
            this.mnxEntryView.Name = "mnxEntryView";
            this.mnxEntryView.Size = new System.Drawing.Size(216, 26);
            this.mnxEntryView.Tag = "mnuView";
            this.mnxEntryView.Text = "&View";
            this.mnxEntryView.Click += new System.EventHandler(this.mnxEntryView_Click);
            // 
            // mnxEntryEdit
            // 
            this.mnxEntryEdit.Image = global::Bimil.Properties.Resources.mnuEdit_16;
            this.mnxEntryEdit.Name = "mnxEntryEdit";
            this.mnxEntryEdit.Size = new System.Drawing.Size(216, 26);
            this.mnxEntryEdit.Tag = "mnuEdit";
            this.mnxEntryEdit.Text = "&Edit";
            this.mnxEntryEdit.Click += new System.EventHandler(this.mnxEntryEdit_Click);
            // 
            // mnxEntry0
            // 
            this.mnxEntry0.Name = "mnxEntry0";
            this.mnxEntry0.Size = new System.Drawing.Size(213, 6);
            // 
            // mnxEntryAdd
            // 
            this.mnxEntryAdd.Image = global::Bimil.Properties.Resources.mnuAdd_16;
            this.mnxEntryAdd.Name = "mnxEntryAdd";
            this.mnxEntryAdd.ShortcutKeyDisplayString = "Ins";
            this.mnxEntryAdd.Size = new System.Drawing.Size(216, 26);
            this.mnxEntryAdd.Tag = "mnuAdd";
            this.mnxEntryAdd.Text = "&Add";
            this.mnxEntryAdd.Click += new System.EventHandler(this.mnxEntryAdd_Click);
            // 
            // mnxEntryAddSimilar
            // 
            this.mnxEntryAddSimilar.Name = "mnxEntryAddSimilar";
            this.mnxEntryAddSimilar.ShortcutKeyDisplayString = "Alt+Ins";
            this.mnxEntryAddSimilar.Size = new System.Drawing.Size(216, 26);
            this.mnxEntryAddSimilar.Text = "Add &Similar";
            this.mnxEntryAddSimilar.Click += new System.EventHandler(this.mnxEntryAddSimilar_Click);
            // 
            // mnxEntryAutotypeSeparator
            // 
            this.mnxEntryAutotypeSeparator.Name = "mnxEntryAutotypeSeparator";
            this.mnxEntryAutotypeSeparator.Size = new System.Drawing.Size(213, 6);
            // 
            // mnxEntryAutotype
            // 
            this.mnxEntryAutotype.Name = "mnxEntryAutotype";
            this.mnxEntryAutotype.Size = new System.Drawing.Size(216, 26);
            this.mnxEntryAutotype.Text = "Auto-t&ype";
            this.mnxEntryAutotype.Click += new System.EventHandler(this.mnxEntryAutotype_Click);
            // 
            // mnxEntrySeparatorBeforeCut
            // 
            this.mnxEntrySeparatorBeforeCut.Name = "mnxEntrySeparatorBeforeCut";
            this.mnxEntrySeparatorBeforeCut.Size = new System.Drawing.Size(213, 6);
            // 
            // mnxEntryCut
            // 
            this.mnxEntryCut.Image = global::Bimil.Properties.Resources.mnuCut_16;
            this.mnxEntryCut.Name = "mnxEntryCut";
            this.mnxEntryCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.mnxEntryCut.Size = new System.Drawing.Size(216, 26);
            this.mnxEntryCut.Tag = "mnuCut";
            this.mnxEntryCut.Text = "Cu&t";
            this.mnxEntryCut.Click += new System.EventHandler(this.mnxEntryCut_Click);
            // 
            // mnxEntryCopy
            // 
            this.mnxEntryCopy.Image = global::Bimil.Properties.Resources.mnuCopy_16;
            this.mnxEntryCopy.Name = "mnxEntryCopy";
            this.mnxEntryCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.mnxEntryCopy.Size = new System.Drawing.Size(216, 26);
            this.mnxEntryCopy.Tag = "mnuCopy";
            this.mnxEntryCopy.Text = "&Copy";
            this.mnxEntryCopy.Click += new System.EventHandler(this.mnxEntryCopy_Click);
            // 
            // mnxEntryPaste
            // 
            this.mnxEntryPaste.Image = global::Bimil.Properties.Resources.mnuPaste_16;
            this.mnxEntryPaste.Name = "mnxEntryPaste";
            this.mnxEntryPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.mnxEntryPaste.Size = new System.Drawing.Size(216, 26);
            this.mnxEntryPaste.Tag = "mnuPaste";
            this.mnxEntryPaste.Text = "&Paste";
            this.mnxEntryPaste.Click += new System.EventHandler(this.mnxEntryPaste_Click);
            // 
            // cmbSearch
            // 
            this.cmbSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Location = new System.Drawing.Point(0, 0);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(582, 24);
            this.cmbSearch.Sorted = true;
            this.cmbSearch.TabIndex = 0;
            this.cmbSearch.SelectedIndexChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
            this.cmbSearch.TextChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
            this.cmbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbSearch_KeyDown);
            // 
            // bwUpgradeCheck
            // 
            this.bwUpgradeCheck.WorkerSupportsCancellation = true;
            this.bwUpgradeCheck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUpgradeCheck_DoWork);
            this.bwUpgradeCheck.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwUpgradeCheck_RunWorkerCompleted);
            // 
            // tmrClose
            // 
            this.tmrClose.Tick += new System.EventHandler(this.tmrClose_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 393);
            this.Controls.Add(this.pnlDocument);
            this.Controls.Add(this.mnu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(399, 298);
            this.Name = "MainForm";
            this.Text = "Bimil";
            this.Activated += new System.EventHandler(this.Form_Activated);
            this.Deactivate += new System.EventHandler(this.Form_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.mnu.ResumeLayout(false);
            this.mnu.PerformLayout();
            this.pnlDocument.ResumeLayout(false);
            this.mnxEntry.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mnu;
        private System.Windows.Forms.ToolStripButton mnuNew;
        private System.Windows.Forms.ToolStripSplitButton mnuOpenMenu;
        private System.Windows.Forms.ToolStripSplitButton mnuSaveMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.ToolStripSeparator mnu0;
        private System.Windows.Forms.ToolStripButton mnuEdit;
        private System.Windows.Forms.ToolStripButton mnuRemove;
        private System.Windows.Forms.ToolStripButton mnuAdd;
        private System.Windows.Forms.Panel pnlDocument;
        private System.Windows.Forms.ListView lsvEntries;
        private System.Windows.Forms.ColumnHeader lsvEntries_colTitle;
        private System.Windows.Forms.ComboBox cmbSearch;
        private System.Windows.Forms.ToolStripDropDownButton mnuApp;
        private System.Windows.Forms.ToolStripMenuItem mnuAppOptions;
        private System.Windows.Forms.ToolStripSeparator mnuApp0;
        private System.Windows.Forms.ToolStripMenuItem mnuSendFeedback;
        private System.Windows.Forms.ToolStripSeparator mnuApp1;
        private System.Windows.Forms.ToolStripMenuItem mnuAppAbout;
        private System.ComponentModel.BackgroundWorker bwUpgradeCheck;
        private System.Windows.Forms.ToolStripMenuItem mnuAppUpgrade;
        private System.Windows.Forms.Timer tmrClose;
        private System.Windows.Forms.ToolStripSeparator mnu1;
        private System.Windows.Forms.ToolStripButton mnuGeneratePassword;
        private System.Windows.Forms.ContextMenuStrip mnxEntry;
        private System.Windows.Forms.ToolStripMenuItem mnxEntryView;
        private System.Windows.Forms.ToolStripMenuItem mnxEntryEdit;
        private System.Windows.Forms.ToolStripSeparator mnxEntrySeparatorBeforeCut;
        private System.Windows.Forms.ToolStripMenuItem mnxEntryAutotype;
        private System.Windows.Forms.ToolStripSeparator mnxEntryAutotypeSeparator;
        private System.Windows.Forms.ToolStripMenuItem mnxEntryCut;
        private System.Windows.Forms.ToolStripMenuItem mnxEntryCopy;
        private System.Windows.Forms.ToolStripMenuItem mnxEntryPaste;
        private System.Windows.Forms.ToolStripSeparator mnxEntry0;
        private System.Windows.Forms.ToolStripMenuItem mnxEntryAdd;
        private System.Windows.Forms.ToolStripMenuItem mnxEntryAddSimilar;
        private System.Windows.Forms.ToolStripButton mnuOpenAlone;
        private System.Windows.Forms.ToolStripButton mnuSaveAlone;
        private System.Windows.Forms.ToolStripButton mnuView;
        private System.Windows.Forms.ToolStripSplitButton mnuPropertiesMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuChangePassword;
        private System.Windows.Forms.ToolStripMenuItem mnuReadOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuProperties;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSplitButton mnuSearchRoot;
        private System.Windows.Forms.ToolStripMenuItem mnuSearch;
        private System.Windows.Forms.ToolStripMenuItem mnuSearchWeak;
        private System.Windows.Forms.ToolStripSeparator mnuAppDebug;
        private System.Windows.Forms.ToolStripMenuItem mnuAppDebugRandomizeAllPasswords;
        private System.Windows.Forms.ToolTip tip;
    }
}

