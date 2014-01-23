using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using Gaste.Data.Objects;
using System.Configuration;
using System.Data.Common;

namespace Gaste.Data
{
    public class Configuration
    {
        const string QUERY_CONFIGURATION = @"SELECT Site.SiteName, Site.SiteLogo, Site.Enabled, Site.DateCreated, Site.SiteURL, SiteParameter.SiteParameterName,
        SiteParameter.SiteParameterValue, SiteParameter.SiteParameterType, SiteParameter.SiteParameterDescription
        FROM Site INNER JOIN SiteParameter ON Site.SiteID = SiteParameter.SiteID WHERE Site.SiteID = {0} AND SiteParameter.IsEnabled = 1";

        public static SiteConfiguration GetSiteConfiguration(int siteID)
        {
            SiteConfiguration siteConfiguration = new SiteConfiguration();

            SqlDatabase sqlDatabase  = new SqlDatabase(ConfigurationManager.ConnectionStrings["GasteMaster"].ToString());
            string query = string.Format(QUERY_CONFIGURATION,siteID);
            using (DbCommand command = sqlDatabase.GetSqlStringCommand(query))
                {
                    using (IDataReader reader = sqlDatabase.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            siteConfiguration.SiteID       = siteID;
                            siteConfiguration.DownloadPath = reader["SiteParameterValue"].ToString();
                            siteConfiguration.SiteURL      = reader["SiteURL"].ToString();
                            return siteConfiguration;
                        }
                    }
                }

            return siteConfiguration;
        }
    }
}
