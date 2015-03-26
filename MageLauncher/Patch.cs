using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Windows.Forms;
using Helper;
using Helper.Timing;
using MageLauncher.Properties;
using Ionic.Zip;

namespace MageLauncher
{
    public class FileHash
    {
        public String FullPath
        {
            get
            {
                return String.Format("{0}{1}", Program.BaseDirectory, FileName);
            }
        }

        public FileHash(String fileName, String hash)
        {
            FileName = fileName;
            Hash = hash;
        }

        public String FileName;
        public String Hash;
    }

    public static class Patch
    {
        public static WebClientEx Web;

        public static String LocalVersion;
        public static String ServerVersion;
        public static Boolean ForceUpdate;

        static Patch()
        {
            Web = new WebClientEx
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore),
                Proxy = new WebProxy(),
                Encoding = Encoding.Default,

            };

            Web.DownloadProgressChanged += OnWebDownloadProgressChanged;

            LocalVersion = GetLocalVersion();
            ServerVersion = GetServerVersion();
        }

        public static void OnWebDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Program.MainForm.ProgressBar.Max = e.TotalBytesToReceive;
            Program.MainForm.ProgressBar.Value = e.BytesReceived;
            Program.MainForm.ProgressBar.Refresh();
        }

        public static String GetLocalVersion()
        {
            String localVersion;

            FileInfo eFileInfo = new FileInfo(String.Format("{0}{1}", Program.BaseDirectory, "version.txt"));

            if (eFileInfo.Exists)
            {
                using (FileStream eFile = File.Open(eFileInfo.FullName, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(eFile))
                    {
                        localVersion = streamReader.ReadLine();
                    }
                } 
            }
            else
            {
                try
                {
                    eFileInfo.Create();
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show(Resources.MessageBox_Message_Patch_Error, Resources.MessageBox_Title_Patch_Error, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Environment.Exit(0);
                }
                
                localVersion = "";
            }

            return localVersion;
        }

        public static String GetServerVersion()
        {
            try
            {
                return Encoding.ASCII.GetString(Web.DownloadDataAsyncTimeout(new Uri(Resources.URL_Version)));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Boolean PatchZip()
        {
            try
            {
                FileInfo pFile = new FileInfo(String.Format("{0}\\Magestorm.zip", Path.GetTempPath()));

                Uri fUri = new Uri(Resources.URL_Patch);

                if (pFile.Directory != null && !pFile.Directory.Exists)
                {
                    pFile.Directory.Create();
                }

                Program.MainForm.PatchStatusLabel.Text = @"Downloading the latest version ...";

                Web.DownloadFileAsyncTimeout(fUri, pFile.FullName);

                using (ZipFile zFile = new ZipFile(pFile.FullName))
                {
                    zFile.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                    zFile.Encryption = EncryptionAlgorithm.None;

                    zFile.ExtractAll(Program.BaseDirectory);
                }

                File.Delete(pFile.FullName);

                FileInfo eFileInfo = new FileInfo(String.Format("{0}{1}", Program.BaseDirectory, "version.txt"));

                if (eFileInfo.Exists)
                {
                    using (FileStream eFile = File.Open(eFileInfo.FullName, FileMode.Open))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(eFile))
                        {
                            streamWriter.WriteLine(ServerVersion);
                        }
                    }
                } 
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static void Start()
        {
            Web.CancelAsync();

            Program.MainForm.ProgressBar.Max = 1;

            if (LocalVersion != ServerVersion || ForceUpdate)
            {
                ForceUpdate = false;

                Program.MainForm.PatchStatusLabel.Text = @"Connecting to the Patch Server ...";

                if (PatchZip())
                {
                    LocalVersion = GetLocalVersion();
                }

                if (ForceUpdate) return;

                if (LocalVersion != ServerVersion)
                {
                    MessageBox.Show(Resources.MessageBox_Message_Patch_Error, Resources.MessageBox_Title_Patch_Error, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Program.MainForm.ProgressBar.Reset();

                    Program.MainForm.PatchStatusLabel.Text = @"Patching Error!";
                    return;
                }

                Program.PatchNotesForm.Visible = true;
            }
            
            Program.MainForm.PatchStatusLabel.Text = @"Magestorm is up to date!";
            Program.MainForm.ProgressBar.DoFullStep();

            TimeHelper.SpinApplication(1000);

            while (Program.OptionForm.Visible || Program.PatchNotesForm.Visible)
            {
                TimeHelper.SpinApplication(1000);
            }

            Program.MainForm.PatchStatusLabel.Text = @"Launching Magestorm...";

            TimeHelper.SpinApplication(1000);

            ProcessStartInfo sInfo = new ProcessStartInfo(".\\Magestorm.exe");
            Process dProcess = new Process
            {
                StartInfo = sInfo
            };

            try
            {
                if (dProcess.Start())
                {
                    Application.Exit();
                }
                else
                {
                    throw new ApplicationException();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}{1}{2}", Resources.MessageBox_Message_Error_Launching, Environment.NewLine, ex.Message), Resources.MessageBox_Title_Launcher, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
