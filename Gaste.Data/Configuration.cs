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
        public static SubdomainConfiguration GetSubdomainConfiguration(int siteID)
        {
            SubdomainConfiguration subdomainConfiguration = new SubdomainConfiguration();
            SqlDatabase sqlDatabase  = new SqlDatabase(ConfigurationManager.ConnectionStrings["GasteMaster"].ToString());
            using (DbCommand command = sqlDatabase.GetStoredProcCommand("GetSubdomainSettings"))
                {
                    sqlDatabase.AddInParameter (command, "@SubdomainID", SqlDbType.Int, siteID);
                    sqlDatabase.AddOutParameter(command, "@DomainID", SqlDbType.Int, siteID);
                    sqlDatabase.AddOutParameter(command, "@SubdomainName", SqlDbType.VarChar, 100);
                    sqlDatabase.AddOutParameter(command, "@SubdomainLogo", SqlDbType.VarChar, 300);
                    sqlDatabase.AddOutParameter(command, "@Enabled", SqlDbType.Bit,1);
                    sqlDatabase.AddOutParameter(command, "@DateCreated", SqlDbType.DateTime, 8);
                    sqlDatabase.AddOutParameter(command, "@SubdomainURL", SqlDbType.VarChar, 500);
                    sqlDatabase.AddOutParameter(command, "@DownloadPath", SqlDbType.VarChar, 500);
                    sqlDatabase.AddOutParameter(command, "@URLImageLink", SqlDbType.VarChar, 500);
                    sqlDatabase.AddOutParameter(command, "@URLFilePath", SqlDbType.VarChar, 500);  
                    sqlDatabase.ExecuteNonQuery(command);

                    subdomainConfiguration.SubdomainID   = siteID;
                    subdomainConfiguration.DomainID      = sqlDatabase.GetParameterValue(command, "DomainID") == DBNull.Value ? 0 : (int)sqlDatabase.GetParameterValue(command, "DomainID");
                    subdomainConfiguration.SubdomainName = string.IsNullOrWhiteSpace(sqlDatabase.GetParameterValue(command, "SubdomainName").ToString()) ? string.Empty : (string)sqlDatabase.GetParameterValue(command, "SubdomainName");
                    subdomainConfiguration.SubdomainLogo = string.IsNullOrWhiteSpace(sqlDatabase.GetParameterValue(command, "SubdomainLogo").ToString()) ? string.Empty : (string)sqlDatabase.GetParameterValue(command, "SubdomainLogo");
                    subdomainConfiguration.Enabled       = sqlDatabase.GetParameterValue(command, "Enabled") == DBNull.Value ? false : (bool)sqlDatabase.GetParameterValue(command, "Enabled");
                    subdomainConfiguration.DateCreated   = string.IsNullOrWhiteSpace(sqlDatabase.GetParameterValue(command, "DateCreated").ToString()) ? DateTime.MaxValue : (DateTime)sqlDatabase.GetParameterValue(command, "DateCreated");
                    subdomainConfiguration.SubdomainURL  = string.IsNullOrWhiteSpace(sqlDatabase.GetParameterValue(command, "SubdomainURL").ToString()) ? string.Empty : (string)sqlDatabase.GetParameterValue(command, "SubdomainURL");
                    subdomainConfiguration.DownloadPath  = string.IsNullOrWhiteSpace(sqlDatabase.GetParameterValue(command, "DownloadPath").ToString()) ? string.Empty : (string)sqlDatabase.GetParameterValue(command, "DownloadPath");
                    subdomainConfiguration.URLImageLink  = string.IsNullOrWhiteSpace(sqlDatabase.GetParameterValue(command, "URLImageLink").ToString()) ? string.Empty : (string)sqlDatabase.GetParameterValue(command, "URLImageLink");
                    subdomainConfiguration.URLFilePath   = string.IsNullOrWhiteSpace(sqlDatabase.GetParameterValue(command, "URLFilePath").ToString()) ? string.Empty : (string)sqlDatabase.GetParameterValue(command, "URLFilePath");
                }

            return subdomainConfiguration;
        }
    }
}
