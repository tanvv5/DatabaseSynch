using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;
using System.Data.SqlClient;

namespace DatabaseSynch
{
    public static class DataSynchronizer
    {
        private static void Initialize
            (string table,
            string serverConnectionString,
            string clientConnectionString)
        {
            using (SqlConnection serverConnection = new
                SqlConnection(serverConnectionString))
            {
                using (SqlConnection clientConnection = new
                    SqlConnection(clientConnectionString))
                {
                    DbSyncScopeDescription scopeDescription = new
                        DbSyncScopeDescription(table);
                    DbSyncTableDescription tableDescription =
                        SqlSyncDescriptionBuilder.GetDescriptionForTable(table,
                            serverConnection);
                    scopeDescription.Tables.Add(tableDescription);
                    SqlSyncScopeProvisioning serverProvision = new
                        SqlSyncScopeProvisioning(serverConnection,
                            scopeDescription);
                    serverProvision.Apply();
                    SqlSyncScopeProvisioning clientProvision = new
                        SqlSyncScopeProvisioning(clientConnection,
                           scopeDescription);
                    clientProvision.Apply();
                }
            }
        }

        public static void Synchronize(string tableName,
            string serverConnectionString, string clientConnectionString)
        {
            Initialize(tableName, serverConnectionString, clientConnectionString);
            Synchronize(tableName, serverConnectionString,
                clientConnectionString, SyncDirectionOrder.DownloadAndUpload);
            CleanUp(tableName, serverConnectionString, clientConnectionString);
        }

        private static void Synchronize(string scopeName,
            string serverConnectionString,
            string clientConnectionString, SyncDirectionOrder syncDirectionOrder)
        {
            using (SqlConnection serverConnection = new
                SqlConnection(serverConnectionString))
            {
                using (SqlConnection clientConnection
                    = new SqlConnection(clientConnectionString))
                {
                    var agent = new SyncOrchestrator
                    {
                        LocalProvider = new
                            SqlSyncProvider(scopeName, clientConnection),
                        RemoteProvider = new SqlSyncProvider(scopeName, serverConnection),
                        Direction = syncDirectionOrder
                    };
                    (agent.RemoteProvider as RelationalSyncProvider).SyncProgress +=
                        new EventHandler<DbSyncProgressEventArgs>
                        (dbProvider_SyncProgress);
                    (agent.LocalProvider as RelationalSyncProvider).ApplyChangeFailed +=
                        new EventHandler<DbApplyChangeFailedEventArgs>(dbProvider_SyncProcessFailed);
                    (agent.RemoteProvider as RelationalSyncProvider).ApplyChangeFailed += new EventHandler<DbApplyChangeFailedEventArgs>
                    (dbProvider_SyncProcessFailed);
                    agent.Synchronize();
                }
            }
        }

        private static void CleanUp(string scopeName,
            string serverConnectionString,
            string clientConnectionString)
        {
            using (SqlConnection serverConnection = new
                SqlConnection(serverConnectionString))
            {
                using (SqlConnection clientConnection = new
                    SqlConnection(clientConnectionString))
                {
                    SqlSyncScopeDeprovisioning serverDeprovisioning = new
                         SqlSyncScopeDeprovisioning(serverConnection);
                    SqlSyncScopeDeprovisioning clientDeprovisioning = new
                        SqlSyncScopeDeprovisioning(clientConnection);
                    serverDeprovisioning.DeprovisionScope(scopeName);
                    serverDeprovisioning.DeprovisionStore();
                    clientDeprovisioning.DeprovisionScope(scopeName);
                    clientDeprovisioning.DeprovisionStore();
                }
            }
        }
        private static void dbProvider_SyncProcessFailed
        (object sender, DbApplyChangeFailedEventArgs e)
        {
            //Write your code here
        }

        private static void dbProvider_SyncProgress(object sender, DbSyncProgressEventArgs e)
        {
            //Write your code here
        }
    }
}
