using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Helper;
using MageServer.Properties;

namespace MageServer.Forms
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        private void ChatLogTextBoxKeyDown(Object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                {
                    if (ChatLogTextBox.Text.Length > 0)
                    {
                        String prefix = "";

                        if (ChatLogPrefixBox.Text != @"[Blank]")
                        {
                            prefix = ChatLogPrefixBox.Text;
                        }

                        String name = prefix.Replace(":", "").Trim();
                        String message = String.Format("{0} {1}", prefix, ChatLogTextBox.Text).Trim();

                        Program.ServerForm.ChatLog.WriteMessage(message, Color.MediumVioletRed);
                        
                        if (name != "")
                        {
                            WebChat.QueueWebChatMessage(new WebChat.WebChatMessage(name, ChatLogTextBox.Text, 2, 0, DateTime.Now.GetUnixTime()));

                            for (Int32 i = 0; i < PlayerManager.Players.Count; i++)
                            {
                                Network.Send(PlayerManager.Players[i], GamePacket.Outgoing.System.DirectTextMessage(PlayerManager.Players[i], message));
                            }
                        }
                        else
                        {
                            for (Int32 i = 0; i < PlayerManager.Players.Count; i++)
                            {
                                Network.Send(PlayerManager.Players[i], GamePacket.Outgoing.System.DirectTextMessage(PlayerManager.Players[i], String.Format("{0}", message)));
                            }
                        }



                        ChatLogTextBox.Text = "";
                    }

                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    break;
                }
            }
        }

        private void ServerFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.IsUserExit) return;

            if (PlayerManager.Players.Count > 0)
            {
                if (MessageBox.Show(@"There are still players connected.  Are you sure you want to exit?", @"Magestorm Server", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) == DialogResult.Yes)
                {
                    Program.ServerForm.MainLog.WriteMessage(@"Server shutting down...", Color.Red);

                    lock (PlayerManager.Players.SyncRoot)
                    {
						Settings.Default.Locked = true;

                        for (Int32 i = 0; i < PlayerManager.Players.Count; i++)
                        {
                            PlayerManager.Players[i].DisconnectReason = Resources.Strings_Disconnect.Shutdown;
                            PlayerManager.Players[i].Disconnect = true;
                        }
                    }

                    while (PlayerManager.Players.Count > 0)
                    {
                        Application.DoEvents();
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

			MySQL.OnlineAccounts.SetAllOffline();
			MySQL.OnlineCharacters.SetAllOffline();

            PurgeAllLogMessages();

            Environment.Exit(0);
        }

        private void AutoScrollToolStripMenuItemClick(object sender, EventArgs e)
        {
            AutoScrollMenuItem.Checked = !AutoScrollMenuItem.Checked;

            MainLog.IsScrollToCaret = AutoScrollMenuItem.Checked;
            ChatLog.IsScrollToCaret = AutoScrollMenuItem.Checked;
            WhisperLog.IsScrollToCaret = AutoScrollMenuItem.Checked;
            CheatLog.IsScrollToCaret = AutoScrollMenuItem.Checked;
            AdminLog.IsScrollToCaret = AutoScrollMenuItem.Checked;
            ReportLog.IsScrollToCaret = AutoScrollMenuItem.Checked;
            MiscLog.IsScrollToCaret = AutoScrollMenuItem.Checked;
        }

        public void PurgeAllLogMessages()
        {
            MainLog.PurgeMessages();
            ChatLog.PurgeMessages();
            WhisperLog.PurgeMessages();
            CheatLog.PurgeMessages();
            AdminLog.PurgeMessages();
            ReportLog.PurgeMessages();
            MiscLog.PurgeMessages();
        }

		private void ShowConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ConfigurationForm configurationForm = new ConfigurationForm();
			configurationForm.ShowDialog();
			Focus();
		}

		private void StartServerMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.ServerStarted)
			{
				StartServerMenuItem.Enabled = false;
				Program.StartServer();
			}
		}

		private void ExitMenuItem_Click(object sender, EventArgs e)
		{
			Program.ServerForm.Close();
		}

		private void ServerForm_Shown(object sender, EventArgs e)
		{
			if (Program.Arguments != null && Program.Arguments.Length > 0)
			{
				if (String.Compare(Program.Arguments[0], "-restart", CultureInfo.CurrentCulture, CompareOptions.IgnoreCase) == 0)
				{
					if (!Program.ServerStarted)
					{
						StartServerMenuItem.Enabled = false;
						Program.StartServer();
					}
				}	
			}
		}
    }
}