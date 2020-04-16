// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Rock.Utility.Settings
{
    /// <summary>
    /// Returns information about the database associated with a Rock instance.
    /// </summary>
    public class RockInstanceDatabaseConfiguration
    {
        #region Fields

        private bool _VersionInfoRetrieved = false;
        private string _Version;
        private string _VersionFriendlyName;
        private string _DatabaseServerOperatingSystem;
        private string _DatabaseName = null;
        private string _ServerName = null;
        private PlatformSpecifier? _Platform;
        private bool? _SnapshotIsolationAllowed;
        private bool? _ReadCommittedSnapshotEnabled;
        private string _RecoverMode = null;
        private bool _ServiceObjectiveInfoRetrieved = false;
        private string _Edition = null;
        private string _ServiceObjective = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RockInstanceDatabaseConfiguration"/> class.
        /// </summary>
        public RockInstanceDatabaseConfiguration()
        {
            //
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RockInstanceDatabaseConfiguration"/> class.
        /// </summary>
        /// <param name="connectionString"></param>
        public RockInstanceDatabaseConfiguration( string connectionString )
        {
            SetConnectionString( connectionString );
        }

        #endregion

        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Set the database connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public void SetConnectionString( string connectionString )
        {
            if ( string.IsNullOrWhiteSpace( connectionString ) )
            {
                this.ConnectionString = null;
            }
            else
            {
                // Parse the connection string and store the server name and database name.
                var csBuilder = new System.Data.Odbc.OdbcConnectionStringBuilder( connectionString );

                object serverName;
                object databaseName;
                bool isValid;

                isValid = csBuilder.TryGetValue( "server", out serverName );

                if ( !isValid )
                {
                    csBuilder.TryGetValue( "data source", out serverName );
                }

                isValid = csBuilder.TryGetValue( "database", out databaseName );

                if ( !isValid )
                {
                    csBuilder.TryGetValue( "initial catalog", out databaseName );
                }

                _ServerName = serverName.ToStringSafe();
                _DatabaseName = databaseName.ToStringSafe();

                this.ConnectionString = connectionString;
            }

            // Reset all cached properties.
            _Platform = PlatformSpecifier.Unknown;
            _VersionInfoRetrieved = false;
            _Version = null;
            _VersionFriendlyName = null;
            _DatabaseServerOperatingSystem = null;
            _SnapshotIsolationAllowed = null;
            _ReadCommittedSnapshotEnabled = null;
            _Edition = null;
            _RecoverMode = null;

        }

        /// <summary>
        /// Gets the type of database platform on which the Rock database is hosted.
        /// </summary>
        public PlatformSpecifier Platform
        {
            get
            {
                if ( !_VersionInfoRetrieved )
                {
                    GetPlatformAndVersionInfo();
                }

                return _Platform.Value;
            }
        }

        /// <summary>
        /// Gets a flag indicating if READ COMMITTED SNAPSHOT isolation level is enabled for the database.
        /// If this isolation level is enabled, the database does not hold record locks during the reading phase of a transaction.
        /// </summary>
        public bool ReadCommittedSnapshotEnabled
        {
            get
            {
                if ( _ReadCommittedSnapshotEnabled == null )
                {
                    GetSnapshotSettings();
                }

                return _ReadCommittedSnapshotEnabled.Value;
            }
        }

        /// <summary>
        /// Gets a flag indicating if snapshot isolation is enabled for the database.
        /// If this feature is available, each transaction operates on a snapshot of the database in isolation from other concurrent operations.
        /// </summary>
        public bool SnapshotIsolationAllowed
        {
            get
            {
                if ( _SnapshotIsolationAllowed == null )
                {
                    GetSnapshotSettings();
                }

                return _SnapshotIsolationAllowed.Value;
            }
        }

        /// <summary>
        /// Gets the size of the database, measured in megabytes (MB).
        /// </summary>
        public decimal? DatabaseSize
        {
            get
            {
                // Query to retrieve the database size, but do not cache the result because it may change for each request.
                try
                {
                    var sql = @"
SELECT CAST( SUM(size* 8.0 ) / 1024.0 AS NUMERIC( 12, 2 ) ) AS 'Db Size (MB)'
FROM   sys.database_files
WHERE  data_space_id = 1
";
                    var reader = GetDataReader( sql, System.Data.CommandType.Text, null );

                    if ( reader != null )
                    {
                        reader.Read();

                        string size = reader.GetValue( 0 ).ToString();

                        decimal sizeInMb;

                        var isValid = decimal.TryParse( reader.GetValue( 0 ).ToString(), out sizeInMb );

                        if ( isValid )
                        {
                            return sizeInMb;
                        }
                    }
                }
                catch
                {
                    // Ignore errors and continue.
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the size of the database log, measured in megabytes (MB).
        /// </summary>
        public decimal? LogSize
        {
            get
            {
                // Query to retrieve the log size, but do not cache the result because it may change for each request.
                try
                {
                    var sql = @"
SELECT CAST( (size* 8.0)/ 1024.0 AS NUMERIC(12,2) ) AS 'Db Log Size (MB)'
FROM   sys.database_files
WHERE  data_space_id = 0
";
                    var reader = GetDataReader( sql, System.Data.CommandType.Text, null );

                    if ( reader != null )
                    {
                        reader.Read();

                        string size = reader.GetValue( 0 ).ToString();

                        decimal sizeInMb;

                        var isValid = decimal.TryParse( reader.GetValue( 0 ).ToString(), out sizeInMb );

                        if ( isValid )
                        {
                            return sizeInMb;
                        }
                    }
                }
                catch
                {
                    // Ignore errors and continue.
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the database platform version string.
        /// </summary>
        public string Version
        {
            get
            {
                if ( !_VersionInfoRetrieved )
                {
                    GetPlatformAndVersionInfo();
                }

                return _Version;
            }
        }

        /// <summary>
        /// Gets a user-friendly description of the database platform version.
        /// </summary>
        public string VersionFriendlyName
        {
            get
            {
                if ( !_VersionInfoRetrieved )
                {
                    GetPlatformAndVersionInfo();
                }

                return _VersionFriendlyName;
            }

        }

        /// <summary>
        /// Gets a description of the database server edition or product variant.
        /// </summary>
        public string Edition
        {
            get
            {
                if ( !_ServiceObjectiveInfoRetrieved )
                {
                    GetServiceObjectiveInfo();
                }

                return _Edition;
            }
        }

        /// <summary>
        /// Gets a description of the database server RecoverMode or product variant.
        /// </summary>
        public string RecoverMode
        {
            get
            {
                if ( _RecoverMode == null )
                {
                    try
                    {
                        var sql = @"
SELECT recovery_model_desc
FROM   sys.databases
WHERE  name = DB_NAME()
";

                        var reader = GetDataReader( sql, System.Data.CommandType.Text, null );

                        if ( reader != null )
                        {
                            reader.Read();

                            _RecoverMode = reader.GetValue( 0 ).ToString();
                        }
                    }
                    catch
                    {
                        // Ignore errors and continue.
                        _RecoverMode = "#ERROR#";
                    }
                }

                return _RecoverMode;
            }
        }

        /// <summary>
        /// Gets a description of the expected capability of the database platform, or null if the capability cannot be determined.
        /// </summary>
        public string ServiceObjective
        {
            get
            {
                if ( !_ServiceObjectiveInfoRetrieved )
                {
                    GetServiceObjectiveInfo();
                }

                return _ServiceObjective;
            }
        }

        /// <summary>
        /// Get the name of the operating system on which the database server is hosted.
        /// </summary>
        public string DatabaseServerOperatingSystem
        {
            get
            {
                if ( !_VersionInfoRetrieved )
                {
                    GetPlatformAndVersionInfo();
                }

                return _DatabaseServerOperatingSystem;
            }
        }

        /// <summary>
        /// Gets the name of the database server.
        /// </summary>
        public string ServerName
        {
            get
            {
                return _ServerName;
            }
        }

        /// <summary>
        /// Gets the name of the database instance.
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return _DatabaseName;
            }
        }

        private void GetPlatformAndVersionInfo()
        {
            _VersionInfoRetrieved = true;

            string version = string.Empty;
            string dbVersion = string.Empty;
            string editionAndPlatformInfo = string.Empty;

            try
            {
                var sql = @"
SELECT SERVERPROPERTY('productversion'), @@Version;
";

                var reader = GetDataReader( sql, System.Data.CommandType.Text, null );

                if ( reader != null )
                {
                    if ( reader.Read() )
                    {
                        version = reader[0].ToString();

                        var versionInfo = reader[1].ToString().SplitDelimitedValues( "\n" );

                        dbVersion = versionInfo[0];
                        editionAndPlatformInfo = versionInfo[3];
                    }
                }
            }
            catch
            {
                // Ignore and continue.
            }

            // Parse Version Description
            if ( dbVersion.StartsWith( "Microsoft SQL Azure", System.StringComparison.OrdinalIgnoreCase ) )
            {
                _Platform = PlatformSpecifier.AzureSql;
                _DatabaseServerOperatingSystem = "Azure";
            }
            else if ( dbVersion.StartsWith( "Microsoft SQL Server", System.StringComparison.OrdinalIgnoreCase ) )
            {
                _Platform = PlatformSpecifier.SqlServer;
            }
            else
            {
                _Platform = PlatformSpecifier.Other;
            }

            _Version = version;

            // Parse Version Friendly Name.
            if ( _Version.StartsWith( "11.0" ) )
            {
                _VersionFriendlyName = "SQL Server 2012";
            }
            else if ( _Version.StartsWith( "12.0" ) )
            {
                _VersionFriendlyName = "SQL Server 2014";
            }
            else if ( _Version.StartsWith( "13.0" ) )
            {
                _VersionFriendlyName = "SQL Server 2016";
            }
            else if ( _Version.StartsWith( "14.0" ) )
            {
                _VersionFriendlyName = "SQL Server 2017";
            }
            else if ( _Version.StartsWith( "15.0" ) )
            {
                _VersionFriendlyName = "SQL Server 2019";
            }
            else
            {
                _VersionFriendlyName = "Unknown";
            }

            // Parse OS Version
            if ( _Platform != PlatformSpecifier.AzureSql )
            {
                _DatabaseServerOperatingSystem = editionAndPlatformInfo.SplitDelimitedValues( " on " )
                    .ToList()
                    .LastOrDefault()
                    .ToStringSafe()
                    .Trim();
            }
        }

        private void GetSnapshotSettings()
        {
            // Get database snapshot isolation details.
            try
            {
                var sql = string.Format( @"
SELECT [snapshot_isolation_state]
       ,[is_read_committed_snapshot_on]
FROM   sys.databases WHERE [name] = '{0}'
"
, _DatabaseName );

                var reader = GetDataReader( sql, System.Data.CommandType.Text, null );

                if ( reader != null )
                {
                    while ( reader.Read() )
                    {
                        _SnapshotIsolationAllowed = reader[0].ToStringSafe().AsBoolean();
                        _ReadCommittedSnapshotEnabled = reader[1].ToStringSafe().AsBoolean();
                    }
                }
            }
            catch
            {
                _SnapshotIsolationAllowed = null;
                _ReadCommittedSnapshotEnabled = null;
            }

        }

        private void GetServiceObjectiveInfo()
        {
            try
            {
                var platform = this.Platform;

                if ( platform == PlatformSpecifier.SqlServer )
                {
                    var sql = @"SELECT SERVERPROPERTY('Edition');";

                    var reader = GetDataReader( sql, System.Data.CommandType.Text, null );

                    if ( reader != null )
                    {
                        reader.Read();

                        _Edition = reader.GetValue( 0 ).ToString();
                    }
                }
                else if ( platform == PlatformSpecifier.AzureSql )
                {
                    var sql = @"
SELECT slo.edition
       ,slo.service_objective
FROM sys.databases d
JOIN sys.database_service_objectives slo
ON d.database_id = slo.database_id
WHERE d.name = '<db_name>';
";
                    sql = sql.Replace( "<db_name>", _DatabaseName );

                    var reader = GetDataReader( sql, System.Data.CommandType.Text, null );

                    if ( reader != null )
                    {
                        reader.Read();

                        _Edition = reader.GetValue( 0 ).ToString();
                        _ServiceObjective = reader.GetValue( 1 ).ToString();

                    }
                }
                else
                {
                    _Edition = "(unknown)";
                }
            }
            catch
            {
                // Ignore errors and continue.
                _Edition = "#ERROR#";
            }
        }

        /// <summary>
        /// Gets a data reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private IDataReader GetDataReader( string query, CommandType commandType, Dictionary<string, object> parameters )
        {
            /* [2020-04-17] DL
             * This replicates the functionality of DbService.GetDataReader(), but removes the dependency
             * on the application configuration file connection string to make the class more test-friendly.
             * Entity Framework data access methods are intentionally avoided here, because attempting to retrieve
             * database properties may fail in situations where the data model does not match the target database.
             */

            if ( string.IsNullOrWhiteSpace( this.ConnectionString ) )
            {
                return null;
            }

            var con = new SqlConnection( this.ConnectionString );
            con.Open();

            var sqlCommand = new SqlCommand( query, con );
            sqlCommand.CommandType = commandType;

            if ( parameters != null )
            {
                foreach ( var parameter in parameters )
                {
                    var sqlParam = new SqlParameter();
                    sqlParam.ParameterName = parameter.Key.StartsWith( "@" ) ? parameter.Key : "@" + parameter.Key;
                    sqlParam.Value = parameter.Value;
                    sqlCommand.Parameters.Add( sqlParam );
                }
            }

            return sqlCommand.ExecuteReader( CommandBehavior.CloseConnection );
        }

        #region Enumerations

        /// <summary>
        /// A database server platform that is capable of hosting an instance of a Rock database.
        /// </summary>
        public enum PlatformSpecifier
        {
            /// <summary>
            /// The database platform is unknown.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// The database platform is an edition of Microsoft SQL Server.
            /// </summary>
            SqlServer = 1,

            /// <summary>
            /// The database is hosted on the Azure platform.
            /// </summary>
            AzureSql = 2,

            /// <summary>
            /// The database is hosted on an unspecified platform.
            /// </summary>
            Other = 3
        }

        #endregion
    }
}
