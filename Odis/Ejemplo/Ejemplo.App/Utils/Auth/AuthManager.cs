using Microsoft.AspNetCore.Authentication;

using System.Security.Claims;

namespace Ejemplo.App.Utils.Auth;

public class AuthManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        getSession();
    }

    #region Propiedades

    private string? usuario;
    private string? dni;
    private string? nombre;
    private string? apellido;
    private string? celular;
    private string? email;
    private string? postback;

    public string? Usuario
    {
        get { return usuario; }
        set
        {
            usuario = value;
            setSession();
        }
    }

    public string? Dni
    {
        get { return dni; }
        set
        {
            dni = value;
            setSession();
        }
    }

    public string? Nombre
    {
        get { return nombre; }
        set
        {
            nombre = value;
            setSession();
        }
    }

    public string? Apellido
    {
        get { return apellido; }
        set
        {
            apellido = value;
            setSession();
        }
    }

    public string? Celular
    {
        get { return celular; }
        set
        {
            celular = value;
            setSession();
        }
    }

    public string? Email
    {
        get { return email; }
        set
        {
            email = value;
            setSession();
        }
    }

    public string? Postback
    {
        get { return postback; }
        set
        {
            postback = value;
            setSession();
        }
    }

    #endregion

    #region Metodos

    public void setSession()
    {
        var claims = new List<Claim>
        {
            new Claim("Usuario", usuario ?? ""),
            new Claim("Dni",dni ??""),
            new Claim("Nombre", nombre ?? ""),
            new Claim("Apellido", apellido ?? ""),
            new Claim("Celular", celular ?? ""),
            new Claim("Email", email ?? ""),
            new Claim("Postback", postback ?? "")
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            httpContext.SignInAsync("Cookies", principal);
        }
    }

    public bool getSession()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        usuario = user?.FindFirst("Usuario")?.Value;
        dni = user?.FindFirst("Dni")?.Value;
        nombre = user?.FindFirst("Nombre")?.Value;
        apellido = user?.FindFirst("Apellido")?.Value;
        celular = user?.FindFirst("Celular")?.Value;
        email = user?.FindFirst("Email")?.Value;
        postback = user?.FindFirst("Postback")?.Value;

        if (usuario != null)
            return true;
        else
            return false;
    }

    #endregion
}
