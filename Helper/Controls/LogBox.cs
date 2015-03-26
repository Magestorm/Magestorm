using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Helper
{
    public class LogBox : ExtendedRichTextBox
    {
        public readonly Object SyncRoot = new Object();

        public Timer PrintTimer;
        public ListCollection<LogMessage> Messages;
        public Boolean HasChanged;
        public Boolean IsScrollToCaret;

        public String LogName { get; set; }

        public String BaseLogDirectory
        {
            get
            {
                return String.Format("{0}\\Logs\\", Directory.GetCurrentDirectory());
            }
        }

        public String FileLogDirectory
        {
            get
            {
                return String.Format("{0}\\Logs\\{1}\\", Directory.GetCurrentDirectory(), LogName);
            }
        }

        public String FileName
        {
            get
            {
                return String.Format("{0}\\Logs\\{1}\\{2}.txt", Directory.GetCurrentDirectory(), LogName, DateTime.Now.ToString("MMM.dd.yyyy"));
            }
        }

        public LogBox()
        {
            IsScrollToCaret = true;
            HasChanged = true;
            Messages = new ListCollection<LogMessage>();

            PrintTimer = new Timer();
            PrintTimer.Tick += PrintTimerTick;
            PrintTimer.Interval = 5;
            PrintTimer.Enabled = true;
        }

        void PrintTimerTick(object sender, EventArgs e)
        {
            if (Messages.Count > 10)
            {
                for (Int32 i = Messages.Count - 1; i >= 0; i--)
                {
                    ProcessMessage(i);
                }
            }
            else
            {
                ProcessMessage(0);
            }

            if (HasChanged && IsScrollToCaret)
            {
                ScrollToCaret();
                HasChanged = false;
            }
        }

        public void ProcessMessage(Int32 index)
        {
            lock (SyncRoot)
            {
                LogMessage message = Messages[index];
                if (message == null) return;

                String timeStampText = String.Format("[{0} {1}] ", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
                String messageText = String.Format("{0}{1}", message.Text, Environment.NewLine);

                if (Lines.Length > 100)
                {
                    ReadOnly = false;
                    Select(0, GetFirstCharIndexFromLine(Lines.Length - 50));
                    SelectedText = "";
                    ReadOnly = true;
                }

                AppendText(timeStampText);
                Int32 colorStart = TextLength;
                AppendText(messageText);
                Select(colorStart, TextLength - colorStart);
                SelectionColor = message.TextColor;
                SelectionLength = 0;

                WriteStringToFile(String.Format("{0}{1}", timeStampText, messageText));

                Messages.RemoveAt(index);    

                HasChanged = true;
            }
        }

        public void PurgeMessages()
        {
            lock (SyncRoot)
            {
                for (Int32 i = Messages.Count - 1; i >= 0; i--)
                {
                    LogMessage message = Messages[i];
                    if (message == null) return;

                    String timeStampText = String.Format("[{0} {1}] ", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
                    String messageText = String.Format("{0}{1}", message.Text, Environment.NewLine);

                    WriteStringToFile(String.Format("{0}{1}", timeStampText, messageText));

                    Messages.RemoveAt(i);
                }
            }
        }

        public void WriteMessage(String text, Color color)
        {
            lock (SyncRoot)
            {
                Messages.Add(new LogMessage(text, color));
            }
        }

        private void WriteStringToFile(String text)
        {
            try
            {
                if (!Directory.Exists(BaseLogDirectory))
                {
                    Directory.CreateDirectory(BaseLogDirectory);
                }

                if (!Directory.Exists(FileLogDirectory))
                {
                    Directory.CreateDirectory(FileLogDirectory);
                }

                FileStream logStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                logStream.Seek(0, SeekOrigin.End);

                StreamWriter logWriter = new StreamWriter(logStream);

                logWriter.Write(text);
                logWriter.Close();
            }
            catch (Exception)
            {
                // Do Nothing
            }
        }
    }
}
