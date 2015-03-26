using System.Windows.Forms;
using Helper;

namespace MageServer.Forms
{
    partial class ServerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StartServerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AutoScrollMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ShowConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ChatLogTabPage = new System.Windows.Forms.TabPage();
			this.ChatLogPrefixBox = new System.Windows.Forms.ComboBox();
			this.ChatLogTextBox = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.ChatLog = new Helper.LogBox();
			this.label2 = new System.Windows.Forms.Label();
			this.MainLogTabPage = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.MainLog = new Helper.LogBox();
			this.label1 = new System.Windows.Forms.Label();
			this.TabControl = new System.Windows.Forms.TabControl();
			this.WhisperLogTabPage = new System.Windows.Forms.TabPage();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.WhisperLog = new Helper.LogBox();
			this.label7 = new System.Windows.Forms.Label();
			this.CheatLogTabPage = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.CheatLog = new Helper.LogBox();
			this.label3 = new System.Windows.Forms.Label();
			this.AdminLogTabPage = new System.Windows.Forms.TabPage();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.AdminLog = new Helper.LogBox();
			this.label4 = new System.Windows.Forms.Label();
			this.ReportLogTabPage = new System.Windows.Forms.TabPage();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.ReportLog = new Helper.LogBox();
			this.label6 = new System.Windows.Forms.Label();
			this.MiscLogTabPage = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.MiscLog = new Helper.LogBox();
			this.label5 = new System.Windows.Forms.Label();
			this.MainMenu.SuspendLayout();
			this.ChatLogTabPage.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.MainLogTabPage.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.TabControl.SuspendLayout();
			this.WhisperLogTabPage.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.CheatLogTabPage.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.AdminLogTabPage.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.ReportLogTabPage.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.MiscLogTabPage.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartServerMenuItem,
            this.ExitMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// StartServerMenuItem
			// 
			this.StartServerMenuItem.Name = "StartServerMenuItem";
			this.StartServerMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
			this.StartServerMenuItem.Size = new System.Drawing.Size(171, 22);
			this.StartServerMenuItem.Text = "&Start Server";
			this.StartServerMenuItem.Click += new System.EventHandler(this.StartServerMenuItem_Click);
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Name = "ExitMenuItem";
			this.ExitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
			this.ExitMenuItem.Size = new System.Drawing.Size(171, 22);
			this.ExitMenuItem.Text = "E&xit";
			this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
			// 
			// MainMenu
			// 
			this.MainMenu.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(678, 24);
			this.MainMenu.TabIndex = 4;
			this.MainMenu.Text = "menuStrip1";
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AutoScrollMenuItem,
            this.ShowConfigurationToolStripMenuItem});
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.settingsToolStripMenuItem.Text = "&Settings";
			// 
			// AutoScrollMenuItem
			// 
			this.AutoScrollMenuItem.Checked = true;
			this.AutoScrollMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.AutoScrollMenuItem.Name = "AutoScrollMenuItem";
			this.AutoScrollMenuItem.Size = new System.Drawing.Size(215, 22);
			this.AutoScrollMenuItem.Text = "&Auto-Scroll Logs";
			this.AutoScrollMenuItem.Click += new System.EventHandler(this.AutoScrollToolStripMenuItemClick);
			// 
			// ShowConfigurationToolStripMenuItem
			// 
			this.ShowConfigurationToolStripMenuItem.Name = "ShowConfigurationToolStripMenuItem";
			this.ShowConfigurationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
			this.ShowConfigurationToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.ShowConfigurationToolStripMenuItem.Text = "Open &Configuration";
			this.ShowConfigurationToolStripMenuItem.Click += new System.EventHandler(this.ShowConfigurationToolStripMenuItem_Click);
			// 
			// ChatLogTabPage
			// 
			this.ChatLogTabPage.Controls.Add(this.ChatLogPrefixBox);
			this.ChatLogTabPage.Controls.Add(this.ChatLogTextBox);
			this.ChatLogTabPage.Controls.Add(this.groupBox2);
			this.ChatLogTabPage.Location = new System.Drawing.Point(4, 24);
			this.ChatLogTabPage.Name = "ChatLogTabPage";
			this.ChatLogTabPage.Size = new System.Drawing.Size(673, 317);
			this.ChatLogTabPage.TabIndex = 3;
			this.ChatLogTabPage.Text = "Chat Log";
			this.ChatLogTabPage.UseVisualStyleBackColor = true;
			// 
			// ChatLogPrefixBox
			// 
			this.ChatLogPrefixBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ChatLogPrefixBox.FormattingEnabled = true;
			this.ChatLogPrefixBox.Items.AddRange(new object[] {
            "[Server] Sorien:",
            "[Blank]",
            "[Server]"});
			this.ChatLogPrefixBox.Location = new System.Drawing.Point(6, 287);
			this.ChatLogPrefixBox.Name = "ChatLogPrefixBox";
			this.ChatLogPrefixBox.Size = new System.Drawing.Size(106, 23);
			this.ChatLogPrefixBox.TabIndex = 3;
			// 
			// ChatLogTextBox
			// 
			this.ChatLogTextBox.Location = new System.Drawing.Point(118, 288);
			this.ChatLogTextBox.MaxLength = 90;
			this.ChatLogTextBox.Name = "ChatLogTextBox";
			this.ChatLogTextBox.Size = new System.Drawing.Size(549, 23);
			this.ChatLogTextBox.TabIndex = 2;
			this.ChatLogTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatLogTextBoxKeyDown);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.ChatLog);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.ForeColor = System.Drawing.Color.Transparent;
			this.groupBox2.Location = new System.Drawing.Point(6, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(661, 276);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "###########";
			this.groupBox2.UseCompatibleTextRendering = true;
			// 
			// ChatLog
			// 
			this.ChatLog.BackColor = System.Drawing.Color.White;
			this.ChatLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ChatLog.Font = new System.Drawing.Font("Lucida Console", 8.25F);
			this.ChatLog.Location = new System.Drawing.Point(2, 16);
			this.ChatLog.LogName = "Chat";
			this.ChatLog.Name = "ChatLog";
			this.ChatLog.Size = new System.Drawing.Size(657, 255);
			this.ChatLog.TabIndex = 4;
			this.ChatLog.Text = "";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.ForeColor = System.Drawing.Color.Black;
			this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
			this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label2.Location = new System.Drawing.Point(10, -3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Chat Log";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MainLogTabPage
			// 
			this.MainLogTabPage.Controls.Add(this.groupBox1);
			this.MainLogTabPage.Location = new System.Drawing.Point(4, 24);
			this.MainLogTabPage.Name = "MainLogTabPage";
			this.MainLogTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.MainLogTabPage.Size = new System.Drawing.Size(673, 317);
			this.MainLogTabPage.TabIndex = 0;
			this.MainLogTabPage.Text = "Main Log";
			this.MainLogTabPage.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.MainLog);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.ForeColor = System.Drawing.Color.Transparent;
			this.groupBox1.Location = new System.Drawing.Point(6, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(661, 307);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "############";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// MainLog
			// 
			this.MainLog.BackColor = System.Drawing.Color.White;
			this.MainLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.MainLog.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MainLog.Location = new System.Drawing.Point(2, 16);
			this.MainLog.LogName = "Main";
			this.MainLog.Name = "MainLog";
			this.MainLog.Size = new System.Drawing.Size(657, 285);
			this.MainLog.TabIndex = 3;
			this.MainLog.Text = "";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.Location = new System.Drawing.Point(10, -3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Main Log";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TabControl
			// 
			this.TabControl.Controls.Add(this.MainLogTabPage);
			this.TabControl.Controls.Add(this.ChatLogTabPage);
			this.TabControl.Controls.Add(this.WhisperLogTabPage);
			this.TabControl.Controls.Add(this.CheatLogTabPage);
			this.TabControl.Controls.Add(this.AdminLogTabPage);
			this.TabControl.Controls.Add(this.ReportLogTabPage);
			this.TabControl.Controls.Add(this.MiscLogTabPage);
			this.TabControl.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TabControl.Location = new System.Drawing.Point(0, 27);
			this.TabControl.Margin = new System.Windows.Forms.Padding(3, 50, 3, 3);
			this.TabControl.Name = "TabControl";
			this.TabControl.SelectedIndex = 0;
			this.TabControl.Size = new System.Drawing.Size(681, 345);
			this.TabControl.TabIndex = 2;
			// 
			// WhisperLogTabPage
			// 
			this.WhisperLogTabPage.Controls.Add(this.groupBox7);
			this.WhisperLogTabPage.Location = new System.Drawing.Point(4, 24);
			this.WhisperLogTabPage.Name = "WhisperLogTabPage";
			this.WhisperLogTabPage.Size = new System.Drawing.Size(673, 317);
			this.WhisperLogTabPage.TabIndex = 9;
			this.WhisperLogTabPage.Text = "Whisper Log";
			this.WhisperLogTabPage.UseVisualStyleBackColor = true;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.WhisperLog);
			this.groupBox7.Controls.Add(this.label7);
			this.groupBox7.ForeColor = System.Drawing.Color.Transparent;
			this.groupBox7.Location = new System.Drawing.Point(6, 3);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(661, 307);
			this.groupBox7.TabIndex = 3;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "###########";
			this.groupBox7.UseCompatibleTextRendering = true;
			// 
			// WhisperLog
			// 
			this.WhisperLog.BackColor = System.Drawing.Color.White;
			this.WhisperLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.WhisperLog.Font = new System.Drawing.Font("Lucida Console", 8.25F);
			this.WhisperLog.Location = new System.Drawing.Point(2, 16);
			this.WhisperLog.LogName = "Whisper";
			this.WhisperLog.Name = "WhisperLog";
			this.WhisperLog.Size = new System.Drawing.Size(657, 285);
			this.WhisperLog.TabIndex = 4;
			this.WhisperLog.Text = "";
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.ForeColor = System.Drawing.Color.Black;
			this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
			this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label7.Location = new System.Drawing.Point(10, -3);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(93, 20);
			this.label7.TabIndex = 1;
			this.label7.Text = "Whisper Log";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// CheatLogTabPage
			// 
			this.CheatLogTabPage.Controls.Add(this.groupBox3);
			this.CheatLogTabPage.Location = new System.Drawing.Point(4, 24);
			this.CheatLogTabPage.Name = "CheatLogTabPage";
			this.CheatLogTabPage.Size = new System.Drawing.Size(673, 317);
			this.CheatLogTabPage.TabIndex = 4;
			this.CheatLogTabPage.Text = "Cheat Log";
			this.CheatLogTabPage.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.CheatLog);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.ForeColor = System.Drawing.Color.Transparent;
			this.groupBox3.Location = new System.Drawing.Point(6, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(661, 307);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "############";
			this.groupBox3.UseCompatibleTextRendering = true;
			// 
			// CheatLog
			// 
			this.CheatLog.BackColor = System.Drawing.Color.White;
			this.CheatLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CheatLog.Font = new System.Drawing.Font("Lucida Console", 8.25F);
			this.CheatLog.Location = new System.Drawing.Point(2, 16);
			this.CheatLog.LogName = "Cheat";
			this.CheatLog.Name = "CheatLog";
			this.CheatLog.Size = new System.Drawing.Size(657, 285);
			this.CheatLog.TabIndex = 5;
			this.CheatLog.Text = "";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.ForeColor = System.Drawing.Color.Black;
			this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
			this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label3.Location = new System.Drawing.Point(10, -3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "Cheat Log";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// AdminLogTabPage
			// 
			this.AdminLogTabPage.Controls.Add(this.groupBox4);
			this.AdminLogTabPage.Location = new System.Drawing.Point(4, 24);
			this.AdminLogTabPage.Name = "AdminLogTabPage";
			this.AdminLogTabPage.Size = new System.Drawing.Size(673, 317);
			this.AdminLogTabPage.TabIndex = 5;
			this.AdminLogTabPage.Text = "Admin Log";
			this.AdminLogTabPage.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.AdminLog);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.ForeColor = System.Drawing.Color.Transparent;
			this.groupBox4.Location = new System.Drawing.Point(6, 3);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(661, 307);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "############";
			this.groupBox4.UseCompatibleTextRendering = true;
			// 
			// AdminLog
			// 
			this.AdminLog.BackColor = System.Drawing.Color.White;
			this.AdminLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.AdminLog.Font = new System.Drawing.Font("Lucida Console", 8.25F);
			this.AdminLog.Location = new System.Drawing.Point(2, 16);
			this.AdminLog.LogName = "Admin";
			this.AdminLog.Name = "AdminLog";
			this.AdminLog.Size = new System.Drawing.Size(657, 285);
			this.AdminLog.TabIndex = 5;
			this.AdminLog.Text = "";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.ForeColor = System.Drawing.Color.Black;
			this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
			this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label4.Location = new System.Drawing.Point(10, -3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 20);
			this.label4.TabIndex = 2;
			this.label4.Text = "Admin Log";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ReportLogTabPage
			// 
			this.ReportLogTabPage.Controls.Add(this.groupBox6);
			this.ReportLogTabPage.Location = new System.Drawing.Point(4, 24);
			this.ReportLogTabPage.Name = "ReportLogTabPage";
			this.ReportLogTabPage.Size = new System.Drawing.Size(673, 317);
			this.ReportLogTabPage.TabIndex = 7;
			this.ReportLogTabPage.Text = "Report Log";
			this.ReportLogTabPage.UseVisualStyleBackColor = true;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.ReportLog);
			this.groupBox6.Controls.Add(this.label6);
			this.groupBox6.ForeColor = System.Drawing.Color.Transparent;
			this.groupBox6.Location = new System.Drawing.Point(6, 3);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(661, 307);
			this.groupBox6.TabIndex = 5;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "################";
			this.groupBox6.UseCompatibleTextRendering = true;
			// 
			// ReportLog
			// 
			this.ReportLog.BackColor = System.Drawing.Color.White;
			this.ReportLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ReportLog.Font = new System.Drawing.Font("Lucida Console", 8.25F);
			this.ReportLog.Location = new System.Drawing.Point(2, 16);
			this.ReportLog.LogName = "Report";
			this.ReportLog.Name = "ReportLog";
			this.ReportLog.Size = new System.Drawing.Size(657, 285);
			this.ReportLog.TabIndex = 5;
			this.ReportLog.Text = "";
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.ForeColor = System.Drawing.Color.Black;
			this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
			this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label6.Location = new System.Drawing.Point(10, -3);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(83, 20);
			this.label6.TabIndex = 2;
			this.label6.Text = "Report Log";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MiscLogTabPage
			// 
			this.MiscLogTabPage.Controls.Add(this.groupBox5);
			this.MiscLogTabPage.Location = new System.Drawing.Point(4, 24);
			this.MiscLogTabPage.Name = "MiscLogTabPage";
			this.MiscLogTabPage.Size = new System.Drawing.Size(673, 317);
			this.MiscLogTabPage.TabIndex = 8;
			this.MiscLogTabPage.Text = "Misc Log";
			this.MiscLogTabPage.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.MiscLog);
			this.groupBox5.Controls.Add(this.label5);
			this.groupBox5.ForeColor = System.Drawing.Color.Transparent;
			this.groupBox5.Location = new System.Drawing.Point(6, 3);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(661, 307);
			this.groupBox5.TabIndex = 6;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "################";
			this.groupBox5.UseCompatibleTextRendering = true;
			// 
			// MiscLog
			// 
			this.MiscLog.BackColor = System.Drawing.Color.White;
			this.MiscLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.MiscLog.Font = new System.Drawing.Font("Lucida Console", 8.25F);
			this.MiscLog.Location = new System.Drawing.Point(2, 16);
			this.MiscLog.LogName = "Misc";
			this.MiscLog.Name = "MiscLog";
			this.MiscLog.Size = new System.Drawing.Size(657, 285);
			this.MiscLog.TabIndex = 5;
			this.MiscLog.Text = "";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.ForeColor = System.Drawing.Color.Black;
			this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
			this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label5.Location = new System.Drawing.Point(10, -3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(93, 20);
			this.label5.TabIndex = 2;
			this.label5.Text = "Misc Log";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ServerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 368);
			this.Controls.Add(this.MainMenu);
			this.Controls.Add(this.TabControl);
			this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.MainMenu;
			this.MaximizeBox = false;
			this.Name = "ServerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Magestorm Server";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerFormFormClosing);
			this.Shown += new System.EventHandler(this.ServerForm_Shown);
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.ChatLogTabPage.ResumeLayout(false);
			this.ChatLogTabPage.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.MainLogTabPage.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.TabControl.ResumeLayout(false);
			this.WhisperLogTabPage.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.CheatLogTabPage.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.AdminLogTabPage.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ReportLogTabPage.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.MiscLogTabPage.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem ExitMenuItem;
        private MenuStrip MainMenu;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem AutoScrollMenuItem;
        internal TabPage ChatLogTabPage;
        private ComboBox ChatLogPrefixBox;
        public TextBox ChatLogTextBox;
        private GroupBox groupBox2;
        private Label label2;
		internal TabPage MainLogTabPage;
        private GroupBox groupBox1;
        private Label label1;
        private TabControl TabControl;
        private TabPage CheatLogTabPage;
        private GroupBox groupBox3;
        private Label label3;
        private TabPage AdminLogTabPage;
        private GroupBox groupBox4;
        private Label label4;
        private TabPage ReportLogTabPage;
        private GroupBox groupBox6;
        private Label label6;
        public LogBox MainLog;
        public LogBox ChatLog;
        public LogBox CheatLog;
        public LogBox AdminLog;
        public LogBox ReportLog;
        private TabPage MiscLogTabPage;
        private GroupBox groupBox5;
        public LogBox MiscLog;
        private Label label5;
        private TabPage WhisperLogTabPage;
        private GroupBox groupBox7;
        public LogBox WhisperLog;
        private Label label7;
		private ToolStripMenuItem ShowConfigurationToolStripMenuItem;
		private ToolStripMenuItem StartServerMenuItem;
    }
}

