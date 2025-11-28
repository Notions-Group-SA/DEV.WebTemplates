using Ejemplo.Web.Utils.Auth;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ejemplo.Web.Components.Pages.Usuarios;

[IgnoreAntiforgeryToken]
public partial class Login
{
    [SupplyParameterFromForm] private AuthModel Model { get; set; } = new();
    [SupplyParameterFromQuery] public string token { get; set; }
    [SupplyParameterFromQuery] public string returnUrl { get; set; } = "Index";


    private string MensajeError = "";


    protected override async Task OnInitializedAsync()
    {
    
        var httpContext = _HttpContextAccessor.HttpContext;

        //detección por token SSO
    }

    async Task onLogin()
    {
        if (await _Usuarios.LoginAsync(Model.Nombre, Model.Clave) == false)
        {
            _Navigation.NavigateTo("LoginError", forceLoad: true);
        }

        vecinosModel = await _Ciudadano.GetOneAsync(decimal.Parse(Model.Nombre));
        if (vecinosModel == null)
            return;

        AuthManager _auth = new(_HttpContextAccessor);
        _auth.Usuario = Model.Nombre;
        _auth.Nombre = vecinosModel.Nombre;
        _auth.Apellido = vecinosModel.Apellido;
        _auth.Celular = vecinosModel.PrefijoCelular + " " + vecinosModel.Celular;
        _auth.Email = vecinosModel.Email;
        _auth.Dni = Model.Nombre;
        _auth.Postback = "";
        _auth.setSession();

        _Navigation.NavigateTo("Index", forceLoad: true);
    }
}