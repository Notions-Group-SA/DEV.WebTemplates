using System.Data;
using Notions.Core.Utils.DataManager;

#pragma warning disable CS8618 
namespace Ejemplo.DataManager.Models;

public class SysUsuariosModel
{
	#region Propiedades Publicas
	public String Usuario { get; set; }

	public String Clave { get; set; }

	public bool Activo { get; set; }

	public DateTime FechaAlta { get; set; }

	public String UsuarioAlta { get; set; }

	public DateTime? FechaModificacion { get; set; }

	public String? UsuarioModificacion { get; set; }

	#endregion

	#region Constructors
	public SysUsuariosModel(DataRow row)
	{
		Usuario = DataParser.ToString(row["Usuario"]);
		Clave = DataParser.ToString(row["Clave"]);
		Activo = DataParser.ToBool(row["Activo"]);
		FechaAlta = DataParser.ToDateTime(row["Fecha_Alta"]);
		UsuarioAlta = DataParser.ToString(row["Usuario_Alta"]);
		FechaModificacion = DataParser.ToDateTimeNullable(row["Fecha_Modificacion"]);
		UsuarioModificacion = DataParser.ToStringNullable(row["Usuario_Modificacion"]);
		
	}
	#endregion
}
