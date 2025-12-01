using Ejemplo.BackOffice.Utils.Auth;
using Ejemplo.DataManager.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ejemplo.BackOffice.Components.Pages.Usuarios;

[IgnoreAntiforgeryToken]
public partial class Login
{
    [SupplyParameterFromForm] private AuthModel Model { get; set; } = new();
    [SupplyParameterFromQuery] public string token { get; set; }
    [SupplyParameterFromQuery] public string returnUrl { get; set; } = "Index";


    private string MensajeError = "";
    private string Titulo = "";
    private string NombreMunicipio = "";
    private string Leyenda = "";

    protected override async Task OnInitializedAsync()
    {
       //detección por token SSO
    }

    async Task onLogin()
    {
        //lo hace asi para que el seter de clav encripte
        SysUsuariosModel usuario = new SysUsuariosModel() { Usuario = Model.Nombre, Clave = Model.Clave };

        if (await _Usuarios.GetByLoginAsync(usuario.Usuario, usuario.Clave) == false)
        {
            _Navigation.NavigateTo("LoginError", forceLoad: true);
        }

        //consulta  a la base

        AuthManager _auth = new(_HttpContextAccessor);
        _auth.Usuario = Model.Nombre;
        //otros datos _auth.Nombre = vecinosModel.Nombre;
        _auth.Postback = "";
        _auth.setSession();

        _Navigation.NavigateTo(returnUrl, forceLoad: true);
    }
}