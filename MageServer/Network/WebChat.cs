using System;
using System.Text;
using System.Threading;
using Helper;
using MageServer.Properties;

namespace MageServer
{
    public static class WebChat
    {
        public class WebChatMessage
        {
			public readonly String Name;
			public readonly String Message;
			public readonly Int32 Level;
			public readonly Int32 UserId;
			public readonly Int64 Timestamp;

            public WebChatMessage (String name, String message, Int32 level, Int32 userId, Int64 timestamp)
            {
                StringBuilder newName = new StringBuilder(name.Length);
                StringBuilder newMessage = new StringBuilder(message.Length);

                for (int i = 0; i < name.Length; i++)
                {
                    newName.Append(Uri.HexEscape(name[i]));
                }

                for (int i = 0; i < message.Length; i++)
                {
                    newMessage.Append(Uri.HexEscape(message[i]));
                }

                Name = newName.ToString();
                Message = newMessage.ToString();
                Level = level;
                UserId = userId;
                Timestamp = timestamp;
            }
        }

	    private static readonly Object SyncRoot;

	    private static readonly String ChatPage;
	    private static readonly ListCollection<WebChatMessage> Messages;

	    private static readonly Thread WorkerThread;

	    private static Boolean HasPendingMessages
        {
            get { return Messages.Count > 0; }
        }

        static WebChat()
        {
            SyncRoot = new Object();
            ChatPage = String.Format("https://{0}/chat.php", Settings.Default.SubscriptionHost);
            Messages = new ListCollection<WebChatMessage>();

            WorkerThread = new Thread(ProcessWebChatMessage);
            WorkerThread.Start();
        }

	    private static void ProcessWebChatMessage()
        {
            while (WorkerThread != null)
            {
                if (HasPendingMessages)
                {
                    try
                    {
                        new NetRequest(NetRequestMode.Chat, ChatPage, null, 
                            String.Format("name={0}", Messages[0].Name), 
                            String.Format("message={0}{1}", Messages[0].Message, Environment.NewLine), 
                            String.Format("level={0}", Messages[0].Level),
                            String.Format("userid={0}", Messages[0].UserId),
                            String.Format("timestamp={0}", Messages[0].Timestamp));
                    }
                    catch (Exception ) { }

	                lock (SyncRoot)
	                {
		                Messages.RemoveAt(0);
	                }
                }

                Thread.Sleep(50);
            }
        }

        public static void QueueWebChatMessage(WebChatMessage message)
        {
            lock (SyncRoot)
            {
                Messages.Add(message);   
            }
        }
    }
}