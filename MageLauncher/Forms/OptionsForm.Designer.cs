namespace MageLauncher.Forms
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ViewPatchNotesButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ForceUpdateButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SpellKey11TextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SpellKey10TextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SpellKey9TextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SpellKey8TextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SpellKey7TextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.SpellKey6TextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.SpellKey5TextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SpellKey4TextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SpellKey3TextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SpellKey2TextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SpellKey1TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SpellKey0TextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.FullMouseLookComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CloseFormButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ViewPatchNotesButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ForceUpdateButton);
            this.groupBox1.Font = new System.Drawing.Font("Pericles", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(2, 286);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Troubleshooting";
            // 
            // ViewPatchNotesButton
            // 
            this.ViewPatchNotesButton.Font = new System.Drawing.Font("Pericles", 7F);
            this.ViewPatchNotesButton.Location = new System.Drawing.Point(233, 22);
            this.ViewPatchNotesButton.Name = "ViewPatchNotesButton";
            this.ViewPatchNotesButton.Size = new System.Drawing.Size(120, 24);
            this.ViewPatchNotesButton.TabIndex = 4;
            this.ViewPatchNotesButton.Text = "View Patch Notes";
            this.ViewPatchNotesButton.UseVisualStyleBackColor = true;
            this.ViewPatchNotesButton.Click += new System.EventHandler(this.ViewPatchNotesButtonClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Pericles", 7F);
            this.label2.ForeColor = System.Drawing.Color.DarkGreen;
            this.label2.Location = new System.Drawing.Point(6, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(356, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Try this if your game will not run, or is acting unexpectedly.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Pericles", 7F);
            this.label1.ForeColor = System.Drawing.Color.DarkGreen;
            this.label1.Location = new System.Drawing.Point(40, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "This will force a download of the latest version.";
            // 
            // ForceUpdateButton
            // 
            this.ForceUpdateButton.Font = new System.Drawing.Font("Pericles", 7F);
            this.ForceUpdateButton.Location = new System.Drawing.Point(24, 22);
            this.ForceUpdateButton.Name = "ForceUpdateButton";
            this.ForceUpdateButton.Size = new System.Drawing.Size(97, 24);
            this.ForceUpdateButton.TabIndex = 0;
            this.ForceUpdateButton.Text = "Force Update";
            this.ForceUpdateButton.UseVisualStyleBackColor = true;
            this.ForceUpdateButton.Click += new System.EventHandler(this.ForceUpdateButtonClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.FullMouseLookComboBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Font = new System.Drawing.Font("Pericles", 9F);
            this.groupBox3.Location = new System.Drawing.Point(2, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(376, 280);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Input";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SpellKey11TextBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.SpellKey10TextBox);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.SpellKey9TextBox);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.SpellKey8TextBox);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.SpellKey7TextBox);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.SpellKey6TextBox);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.SpellKey5TextBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.SpellKey4TextBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.SpellKey3TextBox);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.SpellKey2TextBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.SpellKey1TextBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.SpellKey0TextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(10, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 202);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Custom Spell Keys";
            // 
            // SpellKey11TextBox
            // 
            this.SpellKey11TextBox.Location = new System.Drawing.Point(247, 169);
            this.SpellKey11TextBox.Name = "SpellKey11TextBox";
            this.SpellKey11TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey11TextBox.TabIndex = 23;
            this.SpellKey11TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey11TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey11TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(198, 172);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 16);
            this.label10.TabIndex = 22;
            this.label10.Text = "Key 12:";
            // 
            // SpellKey10TextBox
            // 
            this.SpellKey10TextBox.Location = new System.Drawing.Point(247, 140);
            this.SpellKey10TextBox.Name = "SpellKey10TextBox";
            this.SpellKey10TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey10TextBox.TabIndex = 21;
            this.SpellKey10TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey10TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey10TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(200, 143);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 16);
            this.label11.TabIndex = 20;
            this.label11.Text = "Key 11:";
            // 
            // SpellKey9TextBox
            // 
            this.SpellKey9TextBox.Location = new System.Drawing.Point(247, 111);
            this.SpellKey9TextBox.Name = "SpellKey9TextBox";
            this.SpellKey9TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey9TextBox.TabIndex = 19;
            this.SpellKey9TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey9TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey9TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(196, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 16);
            this.label12.TabIndex = 18;
            this.label12.Text = "Key 10:";
            // 
            // SpellKey8TextBox
            // 
            this.SpellKey8TextBox.Location = new System.Drawing.Point(247, 82);
            this.SpellKey8TextBox.Name = "SpellKey8TextBox";
            this.SpellKey8TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey8TextBox.TabIndex = 17;
            this.SpellKey8TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey8TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey8TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(203, 85);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 16);
            this.label13.TabIndex = 16;
            this.label13.Text = "Key 9:";
            // 
            // SpellKey7TextBox
            // 
            this.SpellKey7TextBox.Location = new System.Drawing.Point(247, 53);
            this.SpellKey7TextBox.Name = "SpellKey7TextBox";
            this.SpellKey7TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey7TextBox.TabIndex = 15;
            this.SpellKey7TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey7TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey7TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(203, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 16);
            this.label14.TabIndex = 14;
            this.label14.Text = "Key 8:";
            // 
            // SpellKey6TextBox
            // 
            this.SpellKey6TextBox.Location = new System.Drawing.Point(247, 24);
            this.SpellKey6TextBox.Name = "SpellKey6TextBox";
            this.SpellKey6TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey6TextBox.TabIndex = 13;
            this.SpellKey6TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey6TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey6TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(203, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 16);
            this.label15.TabIndex = 12;
            this.label15.Text = "Key 7:";
            // 
            // SpellKey5TextBox
            // 
            this.SpellKey5TextBox.Location = new System.Drawing.Point(55, 169);
            this.SpellKey5TextBox.Name = "SpellKey5TextBox";
            this.SpellKey5TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey5TextBox.TabIndex = 11;
            this.SpellKey5TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey5TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey5TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 16);
            this.label9.TabIndex = 10;
            this.label9.Text = "Key 6:";
            // 
            // SpellKey4TextBox
            // 
            this.SpellKey4TextBox.Location = new System.Drawing.Point(55, 140);
            this.SpellKey4TextBox.Name = "SpellKey4TextBox";
            this.SpellKey4TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey4TextBox.TabIndex = 9;
            this.SpellKey4TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey4TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey4TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 16);
            this.label8.TabIndex = 8;
            this.label8.Text = "Key 5:";
            // 
            // SpellKey3TextBox
            // 
            this.SpellKey3TextBox.Location = new System.Drawing.Point(55, 111);
            this.SpellKey3TextBox.Name = "SpellKey3TextBox";
            this.SpellKey3TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey3TextBox.TabIndex = 7;
            this.SpellKey3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey3TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey3TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 16);
            this.label7.TabIndex = 6;
            this.label7.Text = "Key 4:";
            // 
            // SpellKey2TextBox
            // 
            this.SpellKey2TextBox.Location = new System.Drawing.Point(55, 82);
            this.SpellKey2TextBox.Name = "SpellKey2TextBox";
            this.SpellKey2TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey2TextBox.TabIndex = 5;
            this.SpellKey2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey2TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey2TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Key 3:";
            // 
            // SpellKey1TextBox
            // 
            this.SpellKey1TextBox.Location = new System.Drawing.Point(55, 53);
            this.SpellKey1TextBox.Name = "SpellKey1TextBox";
            this.SpellKey1TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey1TextBox.TabIndex = 3;
            this.SpellKey1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey1TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey1TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Key 2:";
            // 
            // SpellKey0TextBox
            // 
            this.SpellKey0TextBox.Location = new System.Drawing.Point(55, 24);
            this.SpellKey0TextBox.Name = "SpellKey0TextBox";
            this.SpellKey0TextBox.Size = new System.Drawing.Size(105, 23);
            this.SpellKey0TextBox.TabIndex = 1;
            this.SpellKey0TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SpellKey0TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SpellKeyTextBoxKeyDown);
            this.SpellKey0TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpellKeyTextBoxKeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Key 1:";
            // 
            // FullMouseLookComboBox
            // 
            this.FullMouseLookComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FullMouseLookComboBox.Font = new System.Drawing.Font("Pericles", 8F);
            this.FullMouseLookComboBox.FormattingEnabled = true;
            this.FullMouseLookComboBox.Items.AddRange(new object[] {
            "Disabled",
            "Normal",
            "Inverted"});
            this.FullMouseLookComboBox.Location = new System.Drawing.Point(10, 40);
            this.FullMouseLookComboBox.Name = "FullMouseLookComboBox";
            this.FullMouseLookComboBox.Size = new System.Drawing.Size(111, 23);
            this.FullMouseLookComboBox.TabIndex = 3;
            this.FullMouseLookComboBox.SelectedIndexChanged += new System.EventHandler(this.FullMouseLookComboBoxSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Pericles", 7F);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(10, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Full Mouse Look";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CloseFormButton
            // 
            this.CloseFormButton.Font = new System.Drawing.Font("Pericles", 7F);
            this.CloseFormButton.Location = new System.Drawing.Point(2, 379);
            this.CloseFormButton.Name = "CloseFormButton";
            this.CloseFormButton.Size = new System.Drawing.Size(376, 30);
            this.CloseFormButton.TabIndex = 4;
            this.CloseFormButton.Text = "Close";
            this.CloseFormButton.UseVisualStyleBackColor = true;
            this.CloseFormButton.Click += new System.EventHandler(this.CloseFormButtonClick);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 414);
            this.Controls.Add(this.CloseFormButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Magestorm Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsFormFormClosing);
            this.Shown += new System.EventHandler(this.OptionsFormShown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ForceUpdateButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox FullMouseLookComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CloseFormButton;
        private System.Windows.Forms.Button ViewPatchNotesButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox SpellKey0TextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SpellKey11TextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox SpellKey10TextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox SpellKey9TextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox SpellKey8TextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox SpellKey7TextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox SpellKey6TextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox SpellKey5TextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox SpellKey4TextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox SpellKey3TextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox SpellKey2TextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox SpellKey1TextBox;
        private System.Windows.Forms.Label label5;
    }
}