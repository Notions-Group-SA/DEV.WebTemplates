using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notions.Core.Utils.Models;

public class DTO_GMaps_Zona
{
    public string id { get; set; }
    public string tipo { get; set; }
    public string localidad { get; set; }
    public string label { get; set; }
    public List<DTO_GMaps_Coordenada> paths { get; set; } = new List<DTO_GMaps_Coordenada>();
    public string strokeColor { get; set; }
    public string strokeOpacity { get; set; } = "0.8";
    public string strokeWeight { get; set; } = "3";
    public string fillColor { get; set; }
    public string fillOpacity { get; set; } = "0.35";
    public DTO_GMaps_MarkerJson centro { get; set; }
}
