using System.Data;
using Microsoft.Data.SqlClient;
using Ejemplo.DataManager.Models;
using Ejemplo.DataManager.Models.Insert;
using Notions.Core.Utils.DataManager;

namespace Ejemplo.DataManager.Abstracts;
public abstract class LutParametrosAbstract
{
	#region Inyeccion de Dependencia
	protected readonly DataEntityCore _dbManager = new("lut_Parametros");
	#endregion

	#region Metodos Publicos

	public async Task<Boolean> InsertAsync(InsertLutParametrosModel model, SqlTransaction? sqlTransaction = null)
	{
		await _dbManager.AddAsync(sqlTransaction, model.GetInsertParams());
		return true;
	}


	public async Task<Boolean> UpdateAsync(InsertLutParametrosModel model, SqlTransaction? sqlTransaction = null)
	{
		await _dbManager.UpdateAsync(sqlTransaction, model.GetUpdateParams());
		return true;
	}

	public async Task<Boolean> DeleteAsync(String codigo, SqlTransaction? sqlTransaction = null)
	{
		await _dbManager.DeleteAsync(sqlTransaction, codigo);
		return true;
	}

	public async Task<DataSet> GetAllAsync()
	{
		return await _dbManager.GetAllAsync();
	}

	public async Task<List<LutParametrosModel>> GetListAllAsync()
	{
		DataRowCollection Rows = (await this.GetAllAsync()).Tables[0].Rows;
		return [.. Rows.Cast<DataRow>().Select(row => new LutParametrosModel(row))];
	}

	public async Task<LutParametrosModel?> GetOneAsync(String codigo)
	{
		DataRowCollection Rows = (await _dbManager.GetAsync(codigo)).Tables[0].Rows;

		if (Rows.Count <= 0)
			return null;
		
		return new LutParametrosModel (Rows[0]);
	}

	public async Task<DataSet> GetByCodigoAsync(String codigo)
	{
		return await _dbManager.GetByAsync("Codigo", codigo);
	}

	public async Task<List<LutParametrosModel>> GetListByCodigoAsync(String codigo)
	{
		DataRowCollection Rows = (await this.GetByCodigoAsync(codigo)).Tables[0].Rows;
		return [.. Rows.Cast<DataRow>().Select(row => new LutParametrosModel(row))];
	}

	public async Task<Int64> GetByCodigoCantidadAsync(String codigo)
	{

		DataTable dt = (await _dbManager.GetByAsync("Codigo_Cantidad", codigo)).Tables[0];
		return Int64.Parse(dt.Rows[0]["Cantidad"]?.ToString() ?? throw new NullReferenceException("Tabla sin datos"));
	}

	#endregion

}
