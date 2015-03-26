namespace MageLauncher
{
    partial class LauncherForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
            this.PatchStatusLabel = new System.Windows.Forms.Label();
            this.OptionsButton = new MageLauncher.ImageButton();
            this.ExitButton = new MageLauncher.ImageButton();
            this.ProgressBar = new MageLauncher.LauncherProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // PatchStatusLabel
            // 
            this.PatchStatusLabel.AutoSize = true;
            this.PatchStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.PatchStatusLabel.Location = new System.Drawing.Point(26, 52);
            this.PatchStatusLabel.Name = "PatchStatusLabel";
            this.PatchStatusLabel.Size = new System.Drawing.Size(188, 16);
            this.PatchStatusLabel.TabIndex = 15;
            this.PatchStatusLabel.Text = "Checking Client Version ...";
            // 
            // OptionsButton
            // 
            this.OptionsButton.BackColor = System.Drawing.Color.Transparent;
            this.OptionsButton.DisabledImage = null;
            this.OptionsButton.Image = ((System.Drawing.Image)(resources.GetObject("OptionsButton.Image")));
            this.OptionsButton.Location = new System.Drawing.Point(289, 49);
            this.OptionsButton.MouseDownImage = ((System.Drawing.Image)(resources.GetObject("OptionsButton.MouseDownImage")));
            this.OptionsButton.MouseEnterImage = ((System.Drawing.Image)(resources.GetObject("OptionsButton.MouseEnterImage")));
            this.OptionsButton.MouseLeaveImage = ((System.Drawing.Image)(resources.GetObject("OptionsButton.MouseLeaveImage")));
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(68, 28);
            this.OptionsButton.TabIndex = 16;
            this.OptionsButton.Click += new System.EventHandler(this.OptionsButtonClick);
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.Transparent;
            this.ExitButton.DisabledImage = null;
            this.ExitButton.Image = ((System.Drawing.Image)(resources.GetObject("ExitButton.Image")));
            this.ExitButton.Location = new System.Drawing.Point(363, 48);
            this.ExitButton.MouseDownImage = ((System.Drawing.Image)(resources.GetObject("ExitButton.MouseDownImage")));
            this.ExitButton.MouseEnterImage = ((System.Drawing.Image)(resources.GetObject("ExitButton.MouseEnterImage")));
            this.ExitButton.MouseLeaveImage = ((System.Drawing.Image)(resources.GetObject("ExitButton.MouseLeaveImage")));
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(68, 28);
            this.ExitButton.TabIndex = 14;
            this.ExitButton.Click += new System.EventHandler(this.ExitButtonClick);
            // 
            // ProgressBar
            // 
            this.ProgressBar.BackColor = System.Drawing.Color.Transparent;
            this.ProgressBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ProgressBar.BackgroundImage")));
            this.ProgressBar.ImageBorderPixelSize = 2;
            this.ProgressBar.Location = new System.Drawing.Point(29, 23);
            this.ProgressBar.Max = ((long)(9));
            this.ProgressBar.Min = ((long)(0));
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.ProgressBorderImage = ((System.Drawing.Image)(resources.GetObject("ProgressBar.ProgressBorderImage")));
            this.ProgressBar.ProgressImage = ((System.Drawing.Image)(resources.GetObject("ProgressBar.ProgressImage")));
            this.ProgressBar.ShowPercentage = true;
            this.ProgressBar.Size = new System.Drawing.Size(402, 22);
            this.ProgressBar.Step = ((long)(1));
            this.ProgressBar.TabIndex = 7;
            this.ProgressBar.TabStop = false;
            this.ProgressBar.Text = "0%";
            this.ProgressBar.Value = ((long)(0));
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(462, 86);
            this.Controls.Add(this.OptionsButton);
            this.Controls.Add(this.PatchStatusLabel);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ProgressBar);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Pericles", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LauncherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Magestorm Launcher";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            ((System.ComponentModel.ISupportInitialize)(this.ProgressBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public LauncherProgressBar ProgressBar;
        private ImageButton ExitButton;
        public System.Windows.Forms.Label PatchStatusLabel;
        private ImageButton OptionsButton;
    }
}

