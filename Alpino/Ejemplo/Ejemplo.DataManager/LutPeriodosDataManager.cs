using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Ejemplo.DataManager.Abstracts;
using Ejemplo.DataManager.Models;

namespace Ejemplo.DataManager;
public class LutPeriodosDataManager() : LutPeriodosAbstract()
{

	#region Metodos No Hechos por el DataTier
	//Ejemplos de Metodos
	
	//GetBy: El metodo que usamos siempre pero mucho mas simple a como lo venimos usando


//		public async Task<DataSet> GetByParam1ParamNull2Async(int param1, int? paramNull2)
//		{
//			object?[] arParams =
//			[
//				param1,
//				paramNull2
//			];
//
//			return await _dbManager.GetByAsync("Param1_ParamNull2", arParams);
//		}

		//GetListBy: *Nuevo Metodo* Devuelve lo mismo que el metodo GetBy, pero parseado a una lista de LutPeriodosModel

//		public List<LutPeriodosModel> GetListByParam1ParamNull2(int param1, int? paramNull2)
//		{
//			return this.GetByParam1ParamNull2Async(param1, paramNull2).Tables[0].Rows.Cast<DataRow>().Select(row => new LutPeriodosModel(row)).ToList();
//			return [.. Rows.Cast<DataRow>().Select(row => new LutPeriodosModel(row))];
//		}
	
	
	#endregion
}
