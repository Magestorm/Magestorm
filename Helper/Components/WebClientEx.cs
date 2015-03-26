using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Helper.Timing;

namespace Helper
{
    public class WebClientEx : WebClient
    {
        private Int64 LastProgressUpdate { get; set; }
        private Byte[] DataResult { get; set; }
        private String StringResult { get; set; }

        public WebException LastException;

        protected override void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
        {
            LastProgressUpdate = NativeMethods.PerformanceCount;

            base.OnDownloadProgressChanged(e);
        }
        protected override void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
        {
            LastProgressUpdate = 0;
            LastException = (WebException)e.Error;
            base.OnDownloadFileCompleted(e);
        }
        protected override void OnDownloadDataCompleted(DownloadDataCompletedEventArgs e)
        {
            try
            {
                DataResult = e.Result;
            }
            catch (Exception) {}
            
            LastProgressUpdate = 0;

            base.OnDownloadDataCompleted(e);
        }
        protected override void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
        {
            StringResult = e.Result;
            LastProgressUpdate = 0;

            base.OnDownloadStringCompleted(e);
        }

        public void DownloadFileAsyncTimeout(Uri address, string fileName)
        {
            if (IsBusy)
            {
                throw new NotSupportedException();
            }

            DownloadFileAsync(address, fileName);

            LastProgressUpdate = NativeMethods.PerformanceCount;

            while (IsBusy)
            {
                Thread.Sleep(1);

                Application.DoEvents();

                if (TimeHelper.DeltaSeconds(LastProgressUpdate, NativeMethods.PerformanceCount) <= 20 || LastProgressUpdate == 0)
                {
                    continue;
                }

                TimeoutAsync();
            }

            Int64 tUpdate = NativeMethods.PerformanceCount;

            while (LastProgressUpdate != 0)
            {
                Thread.Sleep(1);

                Application.DoEvents();

                if (TimeHelper.DeltaSeconds(tUpdate, NativeMethods.PerformanceCount) >= 5)
                {
                    TimeoutAsync();
                }
            }
        }
        public Byte[] DownloadDataAsyncTimeout(Uri address)
        {
            if (IsBusy)
            {
                throw new NotSupportedException();
            }

            DownloadDataAsync(address);

            LastProgressUpdate = NativeMethods.PerformanceCount;

            while (IsBusy)
            {
                Thread.Sleep(1);

                Application.DoEvents();

                if (TimeHelper.DeltaSeconds(LastProgressUpdate, NativeMethods.PerformanceCount) <= 20 || LastProgressUpdate == 0)
                {
                    continue;
                }

                TimeoutAsync();
            }

            Int64 tUpdate = NativeMethods.PerformanceCount;

            while (LastProgressUpdate != 0)
            {
                Thread.Sleep(1);

                Application.DoEvents();

                if (TimeHelper.DeltaSeconds(tUpdate, NativeMethods.PerformanceCount) >= 5)
                {
                    TimeoutAsync();
                }
            }

            return DataResult;
        }
        public String DownloadStringAsyncTimeout(Uri address)
        {
            if (IsBusy)
            {
                throw new NotSupportedException();
            }

            DownloadStringAsync(address);

            LastProgressUpdate = NativeMethods.PerformanceCount;

            while (IsBusy)
            {
                Thread.Sleep(1);

                Application.DoEvents();

                if (TimeHelper.DeltaSeconds(LastProgressUpdate, NativeMethods.PerformanceCount) <= 20 || LastProgressUpdate == 0)
                {
                    continue;
                }

                TimeoutAsync();
            }

            Int64 tUpdate = NativeMethods.PerformanceCount;

            while (LastProgressUpdate != 0)
            {
                Thread.Sleep(1);

                Application.DoEvents();

                if (TimeHelper.DeltaSeconds(tUpdate, NativeMethods.PerformanceCount) >= 5)
                {
                    TimeoutAsync();
                }
            }

            return StringResult;
        }

        public void TimeoutAsync()
        {
            CancelAsync();
            throw new TimeoutException();
        }
    }
}
