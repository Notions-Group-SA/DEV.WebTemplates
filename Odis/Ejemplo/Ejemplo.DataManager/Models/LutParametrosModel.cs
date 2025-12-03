using System.Data;
using Notions.Core.Utils.DataManager;

#pragma warning disable CS8618 
namespace Ejemplo.DataManager.Models;
public class LutParametrosModel
{
	#region Propiedades Publicas
	public String Codigo { get; set; }

	public String Valor { get; set; }

	public String Observaciones { get; set; }

	#endregion

	#region Constructors
	public LutParametrosModel(DataRow row)
	{
		Codigo = DataParser.ToString(row["Codigo"]);
		Valor = DataParser.ToString(row["Valor"]);
		Observaciones = DataParser.ToString(row["Observaciones"]);
		
	}
	#endregion
}
