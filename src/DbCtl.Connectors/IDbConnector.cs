using System;
using System.Threading;
using System.Threading.Tasks;

namespace DbCtl.Interfaces
{
    /// <summary>
    /// Primary connector interface to create the change log table, add change log entries and execute migration scripts.
    /// </summary>
    public interface IDbConnector : IDisposable
    {
        /// <summary>
        /// Creates a change log table in the managed database. Refer to the documentation for the structure of the database table.
        /// </summary>
        /// <param name="connectionString">Connection string to use when connecting to the managed database.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> CreateChangeLogTableAsync(string connectionString, CancellationToken cancellationToken);
        /// <summary>
        /// Adds a change log entry into the change log database table.
        /// </summary>
        /// <param name="connectionString">Connection string to use when connecting to the managed database.</param>
        /// <param name="entry">Change log entry to add to the database change log table.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> AddChangeLogEntryAsync(string connectionString, ChangeLogEntry entry, CancellationToken cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">Connection string to use when connecting to the managed database.</param>
        /// <param name="script">The contents of the migration script to execute against the database.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> ExecuteScriptAsync(string connectionString, string script, CancellationToken cancellationToken);
    }
}
