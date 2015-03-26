using System;
using System.Globalization;
using System.Windows.Forms;
using Helper;
using Helper.Math;
using MageServer.Properties;

namespace MageServer.Forms
{
	public partial class ConfigurationForm : Form
	{
		public ConfigurationForm()
		{
			InitializeComponent();
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			ConfigToolTip.ShowAlways = true;
			ConfigToolTip.UseAnimation = true;
			ConfigToolTip.IsBalloon = true;
			ConfigToolTip.AutoPopDelay = 5000;
			ConfigToolTip.InitialDelay = 100;
			ConfigToolTip.ReshowDelay = 500;

			String[] version = Settings.Default.ServerVersion.Split('.');
			ServerVersionTextBox1.Text = version[0];
			ServerVersionTextBox2.Text = version[1];
			ServerVersionTextBox3.Text = version[2];
			ServerVersionTextBox4.Text = version[3];
			NotifyEmailTextBox.Text = Settings.Default.NotifyEmail;
			ListenPortTextBox.Text = Settings.Default.ListenPort.ToString(CultureInfo.CurrentCulture);
			LockedComboBox.SelectedIndex = Settings.Default.Locked ? 0 : 1;
			MessageOfTheDayTextBox.Text = Settings.Default.MessageOfTheDay;
			DatabaseHostTextBox.Text = Settings.Default.DatabaseHost;
			DatabasePortTextBox.Text = Settings.Default.DatabasePort.ToString(CultureInfo.CurrentCulture);
			DatabaseNameTextBox.Text = Settings.Default.DatabaseName;
			DatabaseUsernameTextBox.Text = Settings.Default.DatabaseUsername;
			DatabasePasswordTextBox.Text = Settings.Default.DatabasePassword;
			SubsciptionHostTextBox.Text = Settings.Default.SubscriptionHost;
			WebKeyTextBox.Text = Settings.Default.WebKey;
			ExpMultiplierTextBox.Text = Settings.Default.ExpMultiplier.ToString(CultureInfo.CurrentCulture);
			PlusExpBonusTextBox.Text = Settings.Default.PlusExpBonus.ToString(CultureInfo.CurrentCulture);
		}

		private void ServerVersionTextBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!e.KeyChar.ToString(CultureInfo.CurrentCulture).IsNumber(false) && e.KeyChar != (Char)Keys.Back)
			{
				ConfigToolTip.SetToolTip(ServerVersionTextBox1, "This box may only contain numbers.");
				ConfigToolTip.ToolTipIcon = ToolTipIcon.Error;
				ConfigToolTip.ToolTipTitle = "Numbers Only";
				e.Handled = true;
			}
			else
			{
				ConfigToolTip.RemoveAll();
			}
		}

		private void ServerVersionTextBox2_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!e.KeyChar.ToString(CultureInfo.CurrentCulture).IsNumber(false) && e.KeyChar != (Char)Keys.Back)
			{
				ConfigToolTip.SetToolTip(ServerVersionTextBox2, "This box may only contain numbers.");
				ConfigToolTip.ToolTipIcon = ToolTipIcon.Error;
				ConfigToolTip.ToolTipTitle = "Numbers Only";
				e.Handled = true;
			}
			else
			{
				ConfigToolTip.RemoveAll();
			}
		}

		private void ServerVersionTextBox3_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!e.KeyChar.ToString(CultureInfo.CurrentCulture).IsNumber(false) && e.KeyChar != (Char)Keys.Back)
			{
				ConfigToolTip.SetToolTip(ServerVersionTextBox3, "This box may only contain numbers.");
				ConfigToolTip.ToolTipIcon = ToolTipIcon.Error;
				ConfigToolTip.ToolTipTitle = "Numbers Only";
				e.Handled = true;
			}
			else
			{
				ConfigToolTip.RemoveAll();
			}
		}

		private void ServerVersionTextBox4_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!e.KeyChar.ToString(CultureInfo.CurrentCulture).IsNumber(false) && e.KeyChar != (Char)Keys.Back)
			{
				ConfigToolTip.SetToolTip(ServerVersionTextBox4, "This box may only contain numbers.");
				ConfigToolTip.ToolTipIcon = ToolTipIcon.Error;
				ConfigToolTip.ToolTipTitle = "Numbers Only";
				e.Handled = true;
			}
			else
			{
				ConfigToolTip.RemoveAll();
			}
		}

		private void ListenPortTextBox_TextChanged(object sender, EventArgs e)
		{
			if (ListenPortTextBox.Text.IsNumber(false))
			{
				Int32 port;
				Int32.TryParse(ListenPortTextBox.Text, out port);

				if (port > 65535)
				{
					ConfigToolTip.SetToolTip(ListenPortTextBox, "You must set a valid range from 1 to 65535.");
					ConfigToolTip.ToolTipIcon = ToolTipIcon.Error;
					ConfigToolTip.ToolTipTitle = "Invalid Port Range";
					ListenPortTextBox.Text = @"10622";
	
				}
				else
				{
					ConfigToolTip.RemoveAll();
				}
			}
		}

		private void ListenPortTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!e.KeyChar.ToString(CultureInfo.CurrentCulture).IsNumber(false) && e.KeyChar != (Char)Keys.Back)
			{
				ConfigToolTip.SetToolTip(ListenPortTextBox, "This box may only contain numbers.");
				ConfigToolTip.ToolTipIcon = ToolTipIcon.Error;
				ConfigToolTip.ToolTipTitle = "Numbers Only";
				e.Handled = true;
			}
			else
			{
				ConfigToolTip.RemoveAll();
			}
		}

		private void DatabasePortTextBox_TextChanged(object sender, EventArgs e)
		{
			if (DatabasePortTextBox.Text.IsNumber(false))
			{
				Int32 port;
				Int32.TryParse(DatabasePortTextBox.Text, out port);

				if (port > 65535)
				{
					ConfigToolTip.SetToolTip(DatabasePortTextBox, "You must set a valid range from 1 to 65535.");
					ConfigToolTip.ToolTipIcon = ToolTipIcon.Error;
					ConfigToolTip.ToolTipTitle = "Invalid Port Range";
					ListenPortTextBox.Text = @"3306";

				}
				else
				{
					ConfigToolTip.RemoveAll();
				}
			}
		}

		private void DatabasePortTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!e.KeyChar.ToString(CultureInfo.CurrentCulture).IsNumber(false) && e.KeyChar != (Char)Keys.Back)
			{
				ConfigToolTip.SetToolTip(DatabasePortTextBox, "This box may only contain numbers.");
				ConfigToolTip.ToolTipIcon = ToolTipIcon.Error;
				ConfigToolTip.ToolTipTitle = "Numbers Only";
				e.Handled = true;
			}
			else
			{
				ConfigToolTip.RemoveAll();
			}
		}

		private void MessageOfTheDayTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (Char)Keys.Enter)
			{
				e.Handled = true;
			}
		}

		private void SaveConfigurationButton_Click(object sender, EventArgs e)
		{
			Int32 port;
			Single num;

			String previousMessageOfTheDay = Settings.Default.MessageOfTheDay;
			Single previousExpMultiplier = Settings.Default.ExpMultiplier;

			String version = String.Format("{0}.{1}.{2}.{3}", ServerVersionTextBox1.Text, ServerVersionTextBox2.Text, ServerVersionTextBox3.Text, ServerVersionTextBox4.Text);
			Settings.Default.ServerVersion = version;
			Settings.Default.NotifyEmail = NotifyEmailTextBox.Text;
			Settings.Default.ListenPort = Int32.TryParse(ListenPortTextBox.Text, out port) ? port : 10622;
			Settings.Default.Locked = LockedComboBox.SelectedIndex == 0;
			Settings.Default.MessageOfTheDay = MessageOfTheDayTextBox.Text;
			Settings.Default.DatabaseHost = DatabaseHostTextBox.Text;
			Settings.Default.DatabasePort = Int32.TryParse(DatabasePortTextBox.Text, out port) ? port : 3306;
			Settings.Default.DatabaseName = DatabaseNameTextBox.Text;
			Settings.Default.DatabaseUsername = DatabaseUsernameTextBox.Text;
			Settings.Default.DatabasePassword = DatabasePasswordTextBox.Text;
			Settings.Default.SubscriptionHost = SubsciptionHostTextBox.Text;
			Settings.Default.WebKey = WebKeyTextBox.Text;
			Settings.Default.ExpMultiplier = Single.TryParse(ExpMultiplierTextBox.Text, out num) ? num : 1.0f;
			Settings.Default.PlusExpBonus = Single.TryParse(PlusExpBonusTextBox.Text, out num) ? num : 0.5f;
			Settings.Default.Save();

			if (!MathHelper.FloatEqual(previousExpMultiplier, Settings.Default.ExpMultiplier))
			{
				MySQL.ServerSettings.SetExpMultiplier(Settings.Default.ExpMultiplier);
				Network.SendTo(GamePacket.Outgoing.System.DirectTextMessage(null, String.Format("[System] All Arenas now have an EXP Bonus of {0}%.", (Settings.Default.ExpMultiplier - 1f) * 100f)), Network.SendToType.All);
			}

			if (String.Compare(previousMessageOfTheDay, Settings.Default.MessageOfTheDay, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase) == 0)
			{
				Network.SendTo(GamePacket.Outgoing.System.DirectTextMessage(null, String.Format("Message of the Day: {0}", Settings.Default.MessageOfTheDay)), Network.SendToType.All);
			}

			MessageBox.Show(@"Your settings have been saved.", @"Magestorm Server", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
