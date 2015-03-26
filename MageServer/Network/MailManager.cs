using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Helper;
using MageServer.Properties;

namespace MageServer
{
    public static class MailManager
    {
	    private static readonly ListCollection<MailMessage> Messages;
	    private static readonly SmtpClient MailClient;
	    private static readonly Thread WorkerThread;

        public static Boolean HasPendingMail
        {
            get { return Messages.Count > 0; }
        }

        static MailManager()
        {
            MailClient = new SmtpClient {
                                            Host = "127.0.0.1",
                                            Port = 25,
                                            Credentials = new NetworkCredential(),
                                            EnableSsl = false
                                        };

            Messages = new ListCollection<MailMessage>();

            WorkerThread = new Thread(ProcessMail);
            WorkerThread.Start();
        }

	    private static void ProcessMail()
        {
            while (WorkerThread != null)
            {
                if (HasPendingMail)
                {
                    try
                    {
                        MailClient.Send(Messages[0]);
                    }
                    catch (Exception ex)
                    {
                        Program.ServerForm.MainLog.WriteMessage(String.Format("[Mail Error] {0}", ex.InnerException.Message), Color.Red);
                    }

                    Messages.RemoveAt(0);
                }

                Thread.Sleep(100);
            } 
        }

        public static void QueueMail(String subject, String body)
        {
                MailMessage message = new MailMessage
                                      {
                                         From = new MailAddress("notify@magestorm.net", "Magestorm Server"),
                                         Subject = subject,
                                         Body = body
                                      };

                message.To.Add(Settings.Default.NotifyEmail);

                Messages.Add(message); 
        }
    }
}
