using Microsoft.Data.SqlClient;
using Notions.Core.Utils.DataManager.Exceptions;
using System.Data;

namespace Notions.Core.Utils.DataManager;

public class DataEntityCore(string entityName)
{
    #region Attributes and Constants

    readonly private string _table = entityName;
    private const string _Insert_Suffix = "_INSERT";
    private const string _Update_Suffix = "_UPDATE";
    private const string _UpdateBy_Suffix = "_UPDATE_BY_";
    private const string _Delete_Suffix = "_DELETE";
    private const string _DeleteBy_Suffix = "_DELETE_BY_";
    private const string _GetOne_Suffix = "_GET";
    private const string _GetAll_Suffix = "_GETALL";
    private const string _GetBy_Suffix = "_GETBY_";
    #endregion

    #region Methods

    #region Insert

    public async virtual Task<object> AddAsync(params object?[] oParams)
    {
        SqlConnection? sqlConnection = null;
        object? retValue = null;
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();
            using SqlCommand sqlComm = new("sp_" + _table + _Insert_Suffix, sqlConnection);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);

            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;

            await sqlComm.ExecuteScalarAsync();

            retValue = sqlComm.Parameters[^1].Value;

            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection); ConnectionFactoryCore.CloseConnection(sqlConnection);
            //close connection
            throw new DataEntityException("Error de data entity", ex);
        }

        return retValue;
    }

    public virtual async Task<object> AddAsync(SqlTransaction? sqlTransaccion, params object?[] oParams)
    {
        if(sqlTransaccion == null)
            return await AddAsync(oParams);

        try
        {
            using SqlCommand sqlComm = new("sp_" + _table + _Insert_Suffix, sqlTransaccion.Connection, sqlTransaccion);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);

            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;

            await sqlComm.ExecuteScalarAsync();
            return sqlComm.Parameters[^1].Value;
        }
        catch (Exception ex)
        {
            sqlTransaccion.Rollback();
            ConnectionFactoryCore.EndTransaction(sqlTransaccion);
            throw new DataEntityException("Error de data entity", ex);
        }
    }

    #endregion

    #region Update

    public virtual async Task<object> UpdateAsync(params object?[] oParams)
    {
        object retValue = 0;
        SqlConnection? sqlConnection = null;
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();
            using SqlCommand sqlComm = new("sp_" + _table + _Update_Suffix, sqlConnection);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);

            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;

            retValue = Convert.ToInt32(await sqlComm.ExecuteNonQueryAsync());
            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection);
            throw new DataEntityException("Error de data entity", ex);
        }

        return retValue;
    }
    public async virtual Task<object> UpdateAsync(SqlTransaction? sqlTransaccion, params object?[] oParams)
    {
        if (sqlTransaccion == null)
            return await UpdateAsync(oParams);

        try
        {
            using SqlCommand sqlComm = new("sp_" + _table + _Update_Suffix, sqlTransaccion.Connection, sqlTransaccion);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);

            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;
            return Convert.ToInt32(await sqlComm.ExecuteNonQueryAsync());
        }
        catch (Exception ex)
        {
            sqlTransaccion.Rollback();
            ConnectionFactoryCore.EndTransaction(sqlTransaccion);
            sqlTransaccion.Dispose();

            throw new DataEntityException("Error de data entity", ex);
        }

    }


    #endregion

    #region UpdateBy

    public virtual async Task<int> UpdateByAsync(string FilterName, params object?[] oParams)
    {
        SqlConnection? sqlConnection = null;
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();
            using SqlCommand sqlComm = new("sp_" + _table + _UpdateBy_Suffix + FilterName, sqlConnection);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);
            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;
            int value = Convert.ToInt32(await sqlComm.ExecuteNonQueryAsync());
            ConnectionFactoryCore.CloseConnection(sqlConnection);
            return value;
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection);
            throw new DataEntityException("Error de data entity", ex);
        }
    }


    public virtual async Task<int> UpdateByAsync(SqlTransaction sqlTransaccion, string FilterName, params object?[] oParams)
    {
        try
        {
            using SqlCommand sqlComm = new("sp_" + _table + _UpdateBy_Suffix + FilterName, sqlTransaccion.Connection, sqlTransaccion);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);
            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;

            return Convert.ToInt32(await sqlComm.ExecuteNonQueryAsync());
        }
        catch (Exception ex)
        {
            sqlTransaccion.Rollback();
            ConnectionFactoryCore.EndTransaction(sqlTransaccion);
            sqlTransaccion.Dispose();

            throw new DataEntityException("Error de data entity", ex);
        }
    }

    #endregion

    #region Delete

    public virtual async Task<int> DeleteAsync(object Id)
    {
        SqlConnection? sqlConnection = null;
        int retValue = 0;
        //si no me pasaron parametros disparo una excepcion
        string sp = "sp_" + _table + _Delete_Suffix;
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();
            using SqlCommand sqlComm = new(sp, sqlConnection);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);
            sqlComm.Parameters[1].Value = Id;

            retValue = Convert.ToInt32(await sqlComm.ExecuteNonQueryAsync());
            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection);
            throw new DataEntityException("Error de data entity", ex);
        }

        return retValue;
    }

    /// <summary>
    /// Método que elimina un registro en la base de datos.
    /// En la entidad cuyo nombre es pasado al constructor de la clase.
    /// </summary>
    /// <param name="Id">Identificador del registro en la tabla</param>
    /// <returns></returns>
    public virtual async Task<int> DeleteAsync(SqlTransaction? sqlTransaccion, object Id)
    {
        if (sqlTransaccion == null)
            return await DeleteAsync(Id);

        string sp = "sp_" + _table + _Delete_Suffix;
        try
        {
            using SqlCommand sqlComm = new(sp, sqlTransaccion.Connection, sqlTransaccion);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);
            sqlComm.Parameters[1].Value = Id;
            return Convert.ToInt32(await sqlComm.ExecuteNonQueryAsync());
        }
        catch (Exception ex)
        {
            sqlTransaccion.Rollback();
            ConnectionFactoryCore.EndTransaction(sqlTransaccion);
            throw new DataEntityException("Error de data entity", ex);
        }

    }

    #endregion

    #region DeleteBy
    public virtual async Task DeleteByAsync(string FilterName, params object?[] oParams)
    {
        SqlConnection? sqlConnection = null;
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();
            using SqlCommand sqlComm = new("sp_" + _table + _DeleteBy_Suffix + FilterName, sqlConnection);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);

            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;

            await sqlComm.ExecuteNonQueryAsync();
            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection); 
            throw new DataEntityException("Error de data entity", ex);
        }

    }

    public virtual void DeleteByAsync(SqlTransaction sqlTransaccion, string FilterName, params object?[] oParams)
    {
        try
        {
            using SqlCommand sqlComm = new("sp_" + _table + _DeleteBy_Suffix + FilterName, sqlTransaccion.Connection, sqlTransaccion);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);

            for (int i = 1; i < sqlComm.Parameters.Count; i++)

                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;

            sqlComm.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            sqlTransaccion.Rollback();
            ConnectionFactoryCore.EndTransaction(sqlTransaccion);
            sqlTransaccion.Dispose();
            throw new DataEntityException("Error de data entity", ex);
        }
    }

    #endregion

    #region GetAll

    public async Task<DataSet> GetAllAsync()
    {
        SqlConnection? sqlConnection = null;
        DataSet oDs = new();
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();

            using SqlCommand sqlComm = new("sp_" + _table + _GetAll_Suffix, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlCommandBuilder.DeriveParameters(sqlComm);
            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);
            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection);
            throw new DataEntityException("Error de data entity", ex);
        }
        return oDs;
    }

    #endregion

    #region Get
    public async Task<DataSet> GetAsync(object Id)
    {
        SqlConnection? sqlConnection = null;
        DataSet oDs = new();
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();
            using SqlCommand sqlComm = new("sp_" + _table + _GetOne_Suffix, sqlConnection);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);
            sqlComm.Parameters[1].Value = Id;
            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);
            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection);
            throw new DataEntityException("Error de data entity", ex);
        }

        return oDs;
    }
    public async Task<DataSet> GetAsync(SqlTransaction sqlTransaccion, object Id)
    {
        DataSet oDs = new();

        try
        {
            using SqlCommand sqlComm = new("sp_" + _table + _GetOne_Suffix, sqlTransaccion.Connection, sqlTransaccion);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);
            sqlComm.Parameters[1].Value = Id;
            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);
        }
        catch (Exception ex)
        {
            throw new DataEntityException("Error de data entity", ex);
        }
        return oDs;
    }

    public async Task<DataSet> GetAsync(string Id)
    {
        SqlConnection? sqlConnection = null;
        DataSet oDs = new();
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();
            using SqlCommand sqlComm = new("sp_" + _table + _GetOne_Suffix, sqlConnection);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);
            sqlComm.Parameters[1].Value = Id;
            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);
            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection); 
            throw new DataEntityException("Error de data entity", ex);
        }

        return oDs;

    }

    public async Task<DataSet> GetAsync(SqlTransaction sqlTransaccion, string Id)
    {
        DataSet oDs = new();
        try
        {
            using SqlCommand sqlComm = new("sp_" + _table + _GetOne_Suffix, sqlTransaccion.Connection, sqlTransaccion);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);
            sqlComm.Parameters[1].Value = Id;
            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);
        }
        catch (Exception ex)
        {
            throw new DataEntityException("Error de data entity", ex);
        }

        return oDs;

    }

    #endregion

    #region GetBy       
    public async Task<DataSet> GetByAsync(string FilterName, params object?[] oParams)
    {
        SqlConnection? sqlConnection = null;
        DataSet oDs = new();
        try
        {
            sqlConnection = await ConnectionFactoryCore.GetConnectionAsync();
            using SqlCommand sqlComm = new("sp_" + _table + _GetBy_Suffix + FilterName, sqlConnection);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);

            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;

            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);
            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection); 
            throw new DataEntityException("Error de data entity", ex);
        }

        return oDs;
    }

    public async Task<DataSet> GetByAsync(SqlTransaction sqlTransaccion, string FilterName, params object?[] oParams)
    {
        DataSet oDs = new();
        try
        {
            using SqlCommand sqlComm = new("sp_" + _table + _GetBy_Suffix + FilterName, sqlTransaccion.Connection, sqlTransaccion);
            sqlComm.CommandType = CommandType.StoredProcedure;
            SqlCommandBuilder.DeriveParameters(sqlComm);

            for (int i = 1; i < sqlComm.Parameters.Count; i++)
                sqlComm.Parameters[i].Value = oParams[i - 1] ?? DBNull.Value;

            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);
        }
        catch (Exception ex)
        {
            throw new DataEntityException("Error de data entity", ex);
        }

        return oDs;
    }

    #endregion

    #region GetExecSql
    public async Task<DataSet> GetExecSqlAsync(string query, string? conexion = null)
    {
        SqlConnection? sqlConnection = null;
        DataSet oDs = new();

        try
        {
            sqlConnection = conexion == null ? await ConnectionFactoryCore.GetConnectionAsync() : new SqlConnection(conexion);

            using SqlCommand sqlComm = new(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };


            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);

            ConnectionFactoryCore.CloseConnection(sqlConnection);
        }
        catch (Exception ex)
        {
            ConnectionFactoryCore.CloseConnection(sqlConnection);
            throw new DataEntityException("Error de data entity", ex);
        }

        return oDs;
    }

    public async Task<DataSet> GetExecSqlAsync(SqlTransaction sqlTransaction, string query)
    {
        DataSet oDs = new();

        try
        {
            using SqlCommand sqlComm = new(query, sqlTransaction.Connection, sqlTransaction)
            {
                CommandType = CommandType.Text
            };

            using SqlDataReader reader = await sqlComm.ExecuteReaderAsync();
            DataTable dataTable = new(_table);
            dataTable.Load(reader);
            oDs.Tables.Add(dataTable);
        }
        catch (Exception ex)
        {
            throw new DataEntityException("Error de data entity", ex);
        }

        return oDs;
    }

    #endregion

    #endregion
}
