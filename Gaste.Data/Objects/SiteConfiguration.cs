using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaste.Data.Objects
{
    public class SiteConfiguration
    {
        public int SiteID {get;set;}
        public string SiteName { get; set; }
        public string SiteLogo { get; set; }
        public bool Enabled { get; set; }
        public DateTime DateCreated { get; set; }
        public string SiteURL { get; set; }
        public string DownloadPath { get; set; }
    }
}
