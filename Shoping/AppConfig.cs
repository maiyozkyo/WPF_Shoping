using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shoping
{
    class AppConfig
    {
        public static string Username { get; set; } = "";
        public static string Password { get; set; } = "";
        public static string Server { get; set; } = "";
        public static string Database { get; set; } = "";
        public static string Entropy { get; set; } = "";
        public static string PasswordIn64 { get; set; } = "";

        public static void Reload()
        {
            var config = System.Configuration.ConfigurationManager.AppSettings;
            Username = config["Username"] ?? "";
            PasswordIn64 = config["Password"] ?? "";
            var entropyIn64 = config["Entropy"] ?? "";
            Server = config["Server"] ?? "";
            Database = config["Database"] ?? "";

            if (PasswordIn64.Length != 0)
            {
                var passwordInBytes = Convert.FromBase64String(PasswordIn64);
                var entropyInBytes = Convert.FromBase64String(entropyIn64);

                var unecryptedPassword = ProtectedData.Unprotect(passwordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);

                Password = Encoding.UTF8.GetString(unecryptedPassword);
            }
        }

        public static void Save()
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["Username"].Value = Username;
            config.AppSettings.Settings["Password"].Value = PasswordIn64;
            config.AppSettings.Settings["Entropy"].Value = Entropy;
            config.Save(ConfigurationSaveMode.Full);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        public static string ConnectionString()
        {

            var builder = new SqlConnectionStringBuilder();
            builder.TrustServerCertificate = true;
            builder.UserID = AppConfig.Username;
            builder.Password = AppConfig.Password;
            builder.DataSource = AppConfig.Server;
            builder.InitialCatalog = AppConfig.Database;

            string connectionString = builder.ConnectionString;

            return connectionString;
        }
    }
}
