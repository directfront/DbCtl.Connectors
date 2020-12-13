using System;
using System.Threading;
using System.Threading.Tasks;

namespace DbCtl.Interfaces
{
    public interface IDbConnector : IDisposable
    {
        Task<int> CreateChangeLogTableAsync(string connectionString, CancellationToken cancellationToken);
        Task<int> AddChangeLogEntryAsync(string connectionString, ChangeLogEntry entry, CancellationToken cancellationToken);
        Task<int> ExecuteScriptAsync(string connectionString, string script, CancellationToken cancellationToken);
    }
}
