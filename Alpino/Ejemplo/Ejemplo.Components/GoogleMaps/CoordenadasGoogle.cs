namespace Ejemplo.Components.GoogleMaps;

public class CoordenadasGoogle
{
    public double lat { get; set; }
    public double lng { get; set; }

    public override bool Equals(object? obj)
    {
        var other = obj as CoordenadasGoogle;
        return lat == other?.lat && lng == other?.lng;
    }

    public bool HayQueResolver()
    {
        return this.Equals(new CoordenadasGoogle { lat = 0, lng = 0 });
    }
}