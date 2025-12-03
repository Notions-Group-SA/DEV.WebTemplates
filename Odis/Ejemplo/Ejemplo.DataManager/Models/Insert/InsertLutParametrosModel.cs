using System.Data;
using Notions.Core.Utils.DataManager;

#pragma warning disable CS8618 
namespace Ejemplo.DataManager.Models.Insert;
public class InsertLutParametrosModel
{
	#region Propiedades Publicas
	public String Codigo { get; set; }

	public String Valor { get; set; }

	public String Observaciones { get; set; }

	#endregion

	#region Constructors
	public InsertLutParametrosModel(DataRow row)
	{
		Codigo = DataParser.ToString(row["Codigo"]);
		Valor = DataParser.ToString(row["Valor"]);
		Observaciones = DataParser.ToString(row["Observaciones"]);
		
	}
	#endregion
	#region Metodos

	public object?[] GetInsertParams()
	{
		return 
		[
			Codigo,
			Valor,
			Observaciones
		];
	}
	public object?[] GetUpdateParams()
	{
		return 
		[
			this.Codigo,
			this.Valor,
			this.Observaciones
		];
	}
	#endregion
}
