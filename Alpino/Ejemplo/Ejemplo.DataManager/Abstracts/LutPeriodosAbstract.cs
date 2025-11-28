using System.Data;
using Microsoft.Data.SqlClient;
using Ejemplo.DataManager.Models;
using Ejemplo.DataManager.Models.Insert;
using Notions.Core.Utils.DataManager;

namespace Ejemplo.DataManager.Abstracts;
public abstract class LutPeriodosAbstract
{
	#region Inyeccion de Dependencia
	protected readonly DataEntityCore _dbManager = new("lut_Periodos");
	#endregion

	#region Metodos Publicos

	public async Task<int> InsertAsync(InsertLutPeriodosModel model, SqlTransaction? sqlTransaction = null)
	{
		int identity = DataParser.ToInt(await _dbManager.AddAsync(sqlTransaction, model.GetInsertParams()));
		model.Id = identity;

		return identity;
	}


	public async Task<Boolean> UpdateAsync(InsertLutPeriodosModel model, SqlTransaction? sqlTransaction = null)
	{
		await _dbManager.UpdateAsync(sqlTransaction, model.GetUpdateParams());
		return true;
	}

	public async Task<Boolean> DeleteAsync(int id, SqlTransaction? sqlTransaction = null)
	{
		await _dbManager.DeleteAsync(sqlTransaction, id);
		return true;
	}

	public async Task<DataSet> GetAllAsync()
	{
		return await _dbManager.GetAllAsync();
	}

	public async Task<List<LutPeriodosModel>> GetListAllAsync()
	{
		DataRowCollection Rows = (await this.GetAllAsync()).Tables[0].Rows;
		return [.. Rows.Cast<DataRow>().Select(row => new LutPeriodosModel(row))];
	}

	public async Task<LutPeriodosModel?> GetOneAsync(int id)
	{
		DataRowCollection Rows = (await _dbManager.GetAsync(id)).Tables[0].Rows;

		if (Rows.Count <= 0)
			return null;
		
		return new LutPeriodosModel (Rows[0]);
	}

	public async Task<DataSet> GetByIdAsync(int id)
	{
		return await _dbManager.GetByAsync("Id", id);
	}

	public async Task<List<LutPeriodosModel>> GetListByIdAsync(int id)
	{
		DataRowCollection Rows = (await this.GetByIdAsync(id)).Tables[0].Rows;
		return [.. Rows.Cast<DataRow>().Select(row => new LutPeriodosModel(row))];
	}

	public async Task<Int64> GetByIdCantidadAsync(int id)
	{

		DataTable dt = (await _dbManager.GetByAsync("Id_Cantidad", id)).Tables[0];
		return Int64.Parse(dt.Rows[0]["Cantidad"]?.ToString() ?? throw new NullReferenceException("Tabla sin datos"));
	}

	#endregion

}
