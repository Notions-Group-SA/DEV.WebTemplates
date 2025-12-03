using Microsoft.Data.SqlClient;
using System.Data;

namespace Notions.Core.Utils.DataManager;

public static class ConnectionFactoryCore
{
    public static String ConnectionString { get; set; } = String.Empty;

    public async static Task<SqlConnection> GetConnectionAsync()
    {
        SqlConnection _sqlConnection = new(ConnectionString);
        if (_sqlConnection.State == ConnectionState.Closed)
            await _sqlConnection.OpenAsync();
        Console.WriteLine("Connection Opened");

        return _sqlConnection;
    }

    public async static void CloseConnection(SqlConnection? _sqlConnection)
    {
        if (_sqlConnection?.State == ConnectionState.Open)
            await _sqlConnection.CloseAsync();
        _sqlConnection?.Dispose();
        Console.WriteLine("Connection Closed");
    }

    public async static Task<SqlTransaction> BeginTransactionAsync()
    {
        SqlConnection _sqlConnection = await GetConnectionAsync();
        Console.WriteLine("Transaction Started");
        return (SqlTransaction)await _sqlConnection.BeginTransactionAsync();
    }

    public async static Task<SqlTransaction> BeginTransactionAsync(SqlConnection _sqlConnection)
    {
        Console.WriteLine("Transaction Started");
        return (SqlTransaction)await _sqlConnection.BeginTransactionAsync();
    }

    public async static Task<SqlTransaction> BeginTransactionAsync(IsolationLevel _isolationLevel)
    {
        SqlConnection _sqlConnection = await GetConnectionAsync();
        Console.WriteLine("Transaction Started");
        return (SqlTransaction)await _sqlConnection.BeginTransactionAsync(_isolationLevel);
    }

    public async static void EndTransaction(SqlTransaction _sqlTransaction)
    {
        if (_sqlTransaction.Connection?.State == ConnectionState.Open)
            await _sqlTransaction.Connection.CloseAsync();
        _sqlTransaction.Connection?.Dispose();
        _sqlTransaction.Dispose();
        Console.WriteLine("Transaction Ended");
    }

}