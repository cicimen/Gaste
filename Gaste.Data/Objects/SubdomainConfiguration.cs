using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaste.Data.Objects
{
    public class SubdomainConfiguration
    {
        public int SubdomainID { get; set; }
        public int DomainID { get; set; }
        public string SubdomainName { get; set; }
        public string SubdomainLogo { get; set; }
        public bool Enabled { get; set; }
        public DateTime DateCreated { get; set; }
        public string SubdomainURL { get; set; }
        public string DownloadPath { get; set; }
        public string URLImageLink { get; set; }
        public string URLFilePath { get; set; }
    }
}
