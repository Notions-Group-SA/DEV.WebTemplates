using System.Data;
using Microsoft.Data.SqlClient;
using Ejemplo.DataManager.Models;
using Ejemplo.DataManager.Models.Insert;
using Notions.Core.Utils.DataManager;

namespace Ejemplo.DataManager.Abstracts;
public abstract class SysUsuariosAbstract
{
	#region Inyeccion de Dependencia
	protected readonly DataEntityCore _dbManager = new("sys_Usuarios");
	#endregion

	#region Metodos Publicos

	public async Task<Boolean> InsertAsync(InsertSysUsuariosModel model, SqlTransaction? sqlTransaction = null)
	{
		await _dbManager.AddAsync(sqlTransaction, model.GetInsertParams());
		return true;
	}


	public async Task<Boolean> UpdateAsync(InsertSysUsuariosModel model, SqlTransaction? sqlTransaction = null)
	{
		await _dbManager.UpdateAsync(sqlTransaction, model.GetUpdateParams());
		return true;
	}

	public async Task<Boolean> DeleteAsync(String usuarios, SqlTransaction? sqlTransaction = null)
	{
		await _dbManager.DeleteAsync(sqlTransaction, usuarios);
		return true;
	}

	public async Task<DataSet> GetAllAsync()
	{
		return await _dbManager.GetAllAsync();
	}

	public async Task<List<SysUsuariosModel>> GetListAllAsync()
	{
		DataRowCollection Rows = (await this.GetAllAsync()).Tables[0].Rows;
		return [.. Rows.Cast<DataRow>().Select(row => new SysUsuariosModel(row))];
	}

	public async Task<SysUsuariosModel?> GetOneAsync(String usuarios)
	{
		DataRowCollection Rows = (await _dbManager.GetAsync(usuarios)).Tables[0].Rows;

		if (Rows.Count <= 0)
			return null;
		
		return new SysUsuariosModel (Rows[0]);
	}

	public async Task<DataSet> GetByActivoAsync(bool activo)
	{
		return await _dbManager.GetByAsync("Activo", activo);
	}

	public async Task<List<SysUsuariosModel>> GetListByActivoAsync(bool activo)
	{
		DataRowCollection Rows = (await this.GetByActivoAsync(activo)).Tables[0].Rows;
		return [.. Rows.Cast<DataRow>().Select(row => new SysUsuariosModel(row))];
	}

	public async Task<Int64> GetByActivoCantidadAsync(bool activo)
	{

		DataTable dt = (await _dbManager.GetByAsync("Activo_Cantidad", activo)).Tables[0];
		return Int64.Parse(dt.Rows[0]["Cantidad"]?.ToString() ?? throw new NullReferenceException("Tabla sin datos"));
	}

	public async Task<DataSet> GetByUsuariosAsync(String usuarios)
	{
		return await _dbManager.GetByAsync("Usuarios", usuarios);
	}

	public async Task<List<SysUsuariosModel>> GetListByUsuariosAsync(String usuarios)
	{
		DataRowCollection Rows = (await this.GetByUsuariosAsync(usuarios)).Tables[0].Rows;
		return [.. Rows.Cast<DataRow>().Select(row => new SysUsuariosModel(row))];
	}

	public async Task<Int64> GetByUsuariosCantidadAsync(String usuarios)
	{

		DataTable dt = (await _dbManager.GetByAsync("Usuarios_Cantidad", usuarios)).Tables[0];
		return Int64.Parse(dt.Rows[0]["Cantidad"]?.ToString() ?? throw new NullReferenceException("Tabla sin datos"));
	}

	#endregion

}
