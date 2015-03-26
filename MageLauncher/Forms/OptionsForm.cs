using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Helper;

namespace MageLauncher.Forms
{
    public partial class OptionsForm : Form
    {
        private readonly String _userPath;

        public OptionsForm()
        {
            _userPath = String.Format("{0}\\User.dat", Directory.GetCurrentDirectory());
            InitializeComponent();
        }

        private void CloseFormButtonClick(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void ForceUpdateButtonClick(object sender, EventArgs e)
        {
            Patch.ForceUpdate = true;
            Patch.Start();
        }

        private void OptionsFormShown(object sender, EventArgs e)
        {
            Int32 fullMouseMode = NativeMethods.GetPrivateProfileInt32("main", "fullmousemode", _userPath);
            if (fullMouseMode < 0 || fullMouseMode > 2) fullMouseMode = 1;
            FullMouseLookComboBox.SelectedIndex = fullMouseMode;

            KeysConverter kc = new KeysConverter();

            for (Int32 i = 0; i < 12; i++)
            {
                TextBox targetTextBox;

                switch (i)
                {
                    case 0:
                    {
                        targetTextBox = SpellKey0TextBox;
                        break;
                    }
                    case 1:
                    {
                        targetTextBox = SpellKey1TextBox;
                        break;
                    }
                    case 2:
                    {
                        targetTextBox = SpellKey2TextBox;
                        break;
                    }
                    case 3:
                    {
                        targetTextBox = SpellKey3TextBox;
                        break;
                    }
                    case 4:
                    {
                        targetTextBox = SpellKey4TextBox;
                        break;
                    }
                    case 5:
                    {
                        targetTextBox = SpellKey5TextBox;
                        break;
                    }
                    case 6:
                    {
                        targetTextBox = SpellKey6TextBox;
                        break;
                    }
                    case 7:
                    {
                        targetTextBox = SpellKey7TextBox;
                        break;
                    }
                    case 8:
                    {
                        targetTextBox = SpellKey8TextBox;
                        break;
                    }
                    case 9:
                    {
                        targetTextBox = SpellKey9TextBox;
                        break;
                    }
                    case 10:
                    {
                        targetTextBox = SpellKey10TextBox;
                        break;
                    }
                    case 11:
                    {
                        targetTextBox = SpellKey11TextBox;
                        break;
                    }
                    default:
                    {
                        targetTextBox = SpellKey0TextBox;
                        break;
                    }
                }

                Int32 key = NativeMethods.GetPrivateProfileInt32("spellkeys", String.Format("spellkey{0}", i), _userPath);

                if (key == -1 || key == 0)
                {
                    targetTextBox.Text = @"Unset";
                }
                else
                {
                    targetTextBox.Text = kc.ConvertToString(key);
                }
            }

        }

        private void FullMouseLookComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            NativeMethods.SetPrivateProfileString("main", "fullmousemode", FullMouseLookComboBox.SelectedIndex.ToString(CultureInfo.InvariantCulture), _userPath);
        }

        private void ViewPatchNotesButtonClick(object sender, EventArgs e)
        {
            Program.PatchNotesForm.Visible = true;
        }

        private void SpellKeyTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void SpellKeyTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox) sender;

            String spellKeyId = textBox.Name;
            spellKeyId = spellKeyId.Replace("SpellKey", "");
            spellKeyId = spellKeyId.Replace("TextBox", "");

            if (e.KeyCode == Keys.Escape)
            {
                textBox.Text = @"Unset";
                NativeMethods.SetPrivateProfileString("spellkeys", String.Format("spellkey{0}", spellKeyId), "0", _userPath);
            }
            else
            {
                KeysConverter kc = new KeysConverter();
                textBox.Text = kc.ConvertToString(e.KeyCode);
                NativeMethods.SetPrivateProfileString("spellkeys", String.Format("spellkey{0}", spellKeyId), ((Int32)e.KeyCode).ToString(CultureInfo.InvariantCulture), _userPath);
            }
        }

        private void OptionsFormFormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }
    }
}
