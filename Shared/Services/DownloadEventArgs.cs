using System;

namespace Shared.Services
{
    public class DownloadEventArgs : EventArgs 
    {
        public string Url { get; set; }

        public string LocalPath { get; set; }
    }
}
