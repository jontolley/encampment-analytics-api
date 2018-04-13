using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;

namespace EncampmentAnalyticsApi.Models
{
    public partial class RegistrationDBEntities : DbContext
    {
        public RegistrationDBEntities(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Create a new EF6 dynamic data context using the specified provider connection string.
        /// </summary>
        /// <param name="connectionString">Provider connection string to use. Usually a standart ADO.NET connection string.</param>
        /// <returns></returns>
        public static RegistrationDBEntities Create(string connectionString)
        {
            var entityBuilder = new EntityConnectionStringBuilder();

            // use your ADO.NET connection string
            entityBuilder.ProviderConnectionString = connectionString;

            entityBuilder.Provider = "System.Data.SqlClient";

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/Database.DWH.DWModel.csdl|res://*/Database.DWH.DWModel.ssdl|res://*/Database.DWH.DWModel.msl";

            return new RegistrationDBEntities(entityBuilder.ConnectionString);
        }

        /// <summary>
        /// Create a new EF6 dynamic data context using the specified provider connection string.
        /// </summary>
        /// <param name="providerConnectionString">Provider connection string to use. Usually a standart ADO.NET connection string.</param>
        /// <returns></returns>
        public static RegistrationDBEntities CreateProduction()
        {
            var environmentConnectionString = Environment.GetEnvironmentVariable("connectionStrings:registrationDB_EF6", EnvironmentVariableTarget.Machine);

            var connectionString = !string.IsNullOrEmpty(environmentConnectionString)
                                    ? environmentConnectionString
                                    : ConfigurationManager.ConnectionStrings["RegistrationDBEntities"].ConnectionString;

            var entityBuilder = new EntityConnectionStringBuilder();

            // use your ADO.NET connection string
            entityBuilder.ProviderConnectionString = connectionString;

            entityBuilder.Provider = "System.Data.SqlClient";

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/Models.EncampmentModel.csdl|res://*/Models.EncampmentModel.ssdl|res://*/Models.EncampmentModel.msl";

            return new RegistrationDBEntities(entityBuilder.ConnectionString);
        }
    }
}