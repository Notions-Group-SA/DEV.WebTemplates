
namespace Ejemplo.Components.GoogleMaps;
public class DomicilioViewModel
{
    int? idLocalidad;
    public int? IdLocalidad
    {
        get
        {
            return idLocalidad;
        }
        set
        {
            if (idLocalidad != value)
            {
                idLocalidad = value;
                DescripcionLocalidad = null;
            }
        }
    }

    string? descripcionLocalidad;
    public string? DescripcionLocalidad
    {
        get
        {
            return descripcionLocalidad;
        }
        set
        {
            if (descripcionLocalidad != value)
            {
                descripcionLocalidad = value;
                Calle = null;
            }
        }
    }
    public string? EntreCalles;

    string? calle;
    public string? Calle
    {
        get
        {
            return calle;
        }
        set
        {
            if (calle != value)
            {
                calle = value;
                Numero = null;
            }
        }
    }

    public string? numero;
    public string? Numero
    {
        get
        {
            return numero;
        }
        set
        {
            if (numero != value)
            {
                numero = value;
                Lat = 0;
                Lng = 0;
            }
        }
    }

    public double Lat { get; set; }
    public double Lng { get; set; }

    public string Municipio { get; set; }
    public string Provincia { get; set; }
    public string Pais { get; set; }

    public string Direccion
    {
        get
        {
            if (string.IsNullOrEmpty(DescripcionLocalidad) == false)
                return $"{calle} {numero}, {DescripcionLocalidad}, {Municipio}, {Provincia}, {Pais}";
            return "";
        }
    }

    public CoordenadasGoogle Coordenadas
    {

        get
        {
            return new CoordenadasGoogle() { lat = Lat, lng = Lng };
        }
        set
        {
            if (value != null)
            {
                Lat = value.lat;
                Lng = value.lng;
            }
            else
            {
                Lat = 0;
                Lng = 0;
            }
        }
    }

    public bool TieneDireccion
    {
        get
        {
            return IdLocalidad.HasValue && !string.IsNullOrEmpty(Calle) && !string.IsNullOrEmpty(Numero);
        }
    }
}
