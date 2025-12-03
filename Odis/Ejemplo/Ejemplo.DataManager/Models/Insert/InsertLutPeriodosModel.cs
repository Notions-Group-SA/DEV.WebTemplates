using System.Data;
using Notions.Core.Utils.DataManager;

#pragma warning disable CS8618 
namespace Ejemplo.DataManager.Models.Insert;
public class InsertLutPeriodosModel
{
	#region Propiedades Publicas
	public int? Id { get; set; }

	public String? Descripcion { get; set; }

	public int? Dias { get; set; }

	#endregion

	#region Constructors
	public InsertLutPeriodosModel(DataRow row)
	{
		Id = DataParser.ToInt(row["Id"]);
		Descripcion = DataParser.ToStringNullable(row["Descripcion"]);
		Dias = DataParser.ToIntNullable(row["Dias"]);
		
	}
	#endregion
	#region Metodos

	public object?[] GetInsertParams()
	{
		return 
		[
			Descripcion,
			Dias,
			0

		];
	}
	public object?[] GetUpdateParams()
	{
		return 
		[
			this.Id,
			this.Descripcion,
			this.Dias
		];
	}
	#endregion
}
