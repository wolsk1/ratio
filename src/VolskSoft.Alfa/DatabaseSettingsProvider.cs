namespace VolskSoft.Alfa
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    /// <summary>
    /// Settings provider for DB based configuration.
    /// </summary>
    internal class DatabaseSettingsProvider : SettingsProviderBase
    {
        #region Fields

        /// <summary>
        /// Name of the attribute that contain name fo the database to connect to.
        /// </summary>
        private const string DatabaseAttributeName = "db-name";

        /// <summary>
        /// Name of the attribute that contains password of the user to connect with.
        /// </summary>
        private const string PasswordAttributeName = "db-user-password";

        /// <summary>
        /// Name of the attribute that contains Db encryption key to get settings with.
        /// </summary>
        private const string PasswordSecurityKeyAttributeName = "db-encrypt-key";

        /// <summary>
        /// Name of the attribute that contains name of the Db server.
        /// </summary>
        private const string ServerAttributeName = "db-server";

        /// <summary>
        /// Name of the attribute that contains name of the user to connect with.
        /// </summary>
        private const string UserAttrbiuteName = "db-user-name";

        /// <summary>
        /// Name of the attrbiute that contains name of the Stored Procedure for settings retrieval.
        /// </summary>
        private const string SpAttrbiuteName = "db-sp-name";

        /// <summary>
        /// Contains name fo the database to connect to.
        /// </summary>
        private string database;

        /// <summary>
        /// Contains password of the user to connect with.
        /// </summary>
        private string password;

        /// <summary>
        /// Contains name of the Db server.
        /// </summary>
        private string server;

        /// <summary>
        /// Contains name of the user to connect with.
        /// </summary>
        private string user;

        /// <summary>
        /// Contains name of the Stored Procedure for settings retrieval.
        /// </summary>
        private string procedureName = "GetSettings";

        #endregion

        /// <summary>
        /// Gets provider's context string. (Used for visualization in error messages).
        /// </summary>
        /// <value></value>
        public override string Context
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "SERVER: {0}, DATABASE: {1}",
                    this.server,
                    this.database);
            }
        }

        /// <summary>
        /// Override this in child class to implement custom initialization.
        /// </summary>
        /// <param name="section">The section.</param>
        protected override void MemberwiseInitialize(ConfSection section)
        {
            this.server = GetSettingsValue(ServerAttributeName);
            this.database = GetSettingsValue(DatabaseAttributeName);
            this.user = GetSettingsValue(UserAttrbiuteName);
            this.password = GetSettingsValue(PasswordAttributeName);

            if (ContainsSettingsValue(SpAttrbiuteName))
            {
                this.procedureName = GetSettingsValue(SpAttrbiuteName);
            }

            if (ContainsSettingsValue(PasswordSecurityKeyAttributeName))
            {
                // Decrypt password
                string encryptKey = GetSettingsValue(PasswordSecurityKeyAttributeName);
                this.password = CryptoUtils.Encrypt(this.password, encryptKey);
            }

            ////TODO: rewrite this to use standart Sql connection construction mechnaism found in DBConnectionManager
            string sqlConnectionString =
                String.Format(
                    CultureInfo.InvariantCulture,
                    "Data Source=\"{0}\";Initial Catalog=\"{3}\";User Id=\"{1}\";Password=\"{2}\";",
                    this.server,
                    this.user,
                    this.password,
                    this.database);

            // Add connection string to settings collection.
            // Now there are no need to specify sql connection properties
            // in data base. Just read this this key from settings.
            Values.Add("SqlConnectionString", sqlConnectionString);

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = this.procedureName;
                    cmd.Connection = connection;

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader == null)
                        {
                            throw new InvalidOperationException("Error while reading settings data. SqlReader is null.");
                        }

                        while (reader.Read())
                        {
                            Values.Add(
                                reader.GetString(reader.GetOrdinal("key")),
                                reader.GetString(reader.GetOrdinal("value")));
                        }
                    }
                }
            }
        }
    }
}