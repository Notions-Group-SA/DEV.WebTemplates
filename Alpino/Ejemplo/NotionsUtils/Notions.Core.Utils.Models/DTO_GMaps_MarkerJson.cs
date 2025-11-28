namespace Notions.Core.Utils.Models;

public class DTO_GMaps_MarkerJson
{
    public string id { get; set; }
    public DTO_GMaps_Coordenada position { get; set; }
    public string label { get; set; }
    public string icon { get; set; }
    public bool anda { get; set; }
}
