using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notions.Core.Utils.Models.Fechas
{
    public class NgFechaNacimientoModel
    {
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }

    }

    public class NgMesesModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

    }
}
