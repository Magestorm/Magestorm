using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MageServer
{
    public enum NetRequestMode
    {
        Magestorm,
        Chat,
    }

    public class NetRequest
    {
        public readonly NetRequestMode Mode;
        public readonly String Response;
        public readonly String ForwardIpAddress;
        public readonly Boolean Succeeded;

        public NetRequest(NetRequestMode mode, String url, String forwardIpAddress, params String[] args)
        {
            Succeeded = false;
            Mode = mode;
            ForwardIpAddress = forwardIpAddress;

            String arguments = String.Format("k={0}&m={1}", Properties.Settings.Default.WebKey, Mode);
            arguments = args.Aggregate(arguments, (current, t) => current + String.Format("&{0}", t));

            Byte[] postArray = Encoding.UTF8.GetBytes(arguments);

            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy = new WebProxy();
            request.Timeout = 8000;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postArray.Length;

            if (ForwardIpAddress != null)
            {
                request.Headers.Add("X-Forwarded-For", ForwardIpAddress);
            }

            try
            {
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(postArray, 0, postArray.Length);
                dataStream.Close();

                switch (mode)
                {
                    case NetRequestMode.Chat:
                    {
                        Response = "";
                        Succeeded = true;
                        break;
                    }
                    case NetRequestMode.Magestorm:
                    {
                        Stream stream = request.GetResponse().GetResponseStream();

                        if (stream == null)
                        {
                            throw new NullReferenceException();
                        }

                        using (StreamReader inStream = new StreamReader(stream))
                        {
                            Response = inStream.ReadLine();

                            if (Response != null)
                            {
                                if (Response.StartsWith("<response>") && Response.EndsWith("</response>"))
                                {
                                    Response = Response.Replace("<response>", "");
                                    Response = Response.Replace("</response>", "");
                                    Succeeded = true;
                                }
                                else
                                {
                                    throw new NotSupportedException();
                                }
                            }
                            else
                            {
                                throw new NullReferenceException();
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception)
            {
                Response = "";
                Succeeded = false;
            }
        }
    }
}
