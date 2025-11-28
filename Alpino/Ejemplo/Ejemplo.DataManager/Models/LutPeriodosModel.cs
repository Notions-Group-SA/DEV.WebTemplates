using System.Data;
using Notions.Core.Utils.DataManager;

#pragma warning disable CS8618 
namespace Ejemplo.DataManager.Models;
public class LutPeriodosModel
{
	#region Propiedades Publicas
	public int Id { get; set; }

	public String? Descripcion { get; set; }

	public int? Dias { get; set; }

	#endregion

	#region Constructors
	public LutPeriodosModel(DataRow row)
	{
		Id = DataParser.ToInt(row["Id"]);
		Descripcion = DataParser.ToStringNullable(row["Descripcion"]);
		Dias = DataParser.ToIntNullable(row["Dias"]);
		
	}
	#endregion
}
