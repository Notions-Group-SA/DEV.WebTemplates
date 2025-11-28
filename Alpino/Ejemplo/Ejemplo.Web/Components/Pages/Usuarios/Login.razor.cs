using GDA.Core.Ciudadano.Utils.Auth;
using GDA.Core.DataManager;
using GDA.Core.DataManager.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using SGM.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GDA.Core.Ciudadano.Components.Pages.Usuario;

[IgnoreAntiforgeryToken]
public partial class Login
{
    [SupplyParameterFromForm]
    private AuthModel Model { get; set; } = new();
    [SupplyParameterFromQuery] public string token { get; set; }
    [SupplyParameterFromQuery] public string returnUrl { get; set; } = "Index";


    private SysVecinosDataManager _Ciudadano = new();
    private SysVecinosModel vecinosModel = new();
    private SysLoginVecinoDigitalDataManager _Login = new();
    private string MensajeError = "";
    string NombreMunicipio;
    string Titulo;
    string Leyenda;
    LutParametrosModel ParametrosModel = new();

    protected override async Task OnInitializedAsync()
    {
        ParametrosModel = await _Parametros.GetOneAsync("codMunicipio");
        NombreMunicipio = ParametrosModel.Valor;

        ParametrosModel = await _Parametros.GetOneAsync("GDA_LoginTitulo_Portal");
        Titulo = ParametrosModel.Valor;

        ParametrosModel = await _Parametros.GetOneAsync("GDA_LoginLeyenda_Portal");
        Leyenda = ParametrosModel.Valor;

        var httpContext = _HttpContextAccessor.HttpContext;

        if (!string.IsNullOrEmpty(token))
        {
            string usuario = _NgCrypto.decrypt(token);
            vecinosModel = await _Ciudadano.GetOneAsync(Convert.ToDecimal(usuario));
            if (vecinosModel != null)
            {

                AuthManager _auth = new(_HttpContextAccessor);
                _auth.Usuario = usuario;
                _auth.Nombre = vecinosModel.Nombre;
                _auth.Apellido = vecinosModel.Apellido;
                _auth.Celular = vecinosModel.Celular;
                _auth.Email = vecinosModel.Email;
                _auth.Dni = vecinosModel.DNI.ToString();
                _auth.Postback = "";
                _auth.setSession();

                _Navigation.NavigateTo(returnUrl, true);
            }
            else
            {
                _Navigation.NavigateTo("Login", true);
            }
        }
        else
        {
            if (httpContext != null)
            {
                var ssoToken = httpContext.Request.Query["sso_token"];
                if (!string.IsNullOrEmpty(ssoToken))
                {
                    await ProcessSSOToken(ssoToken);
                    return;
                }
            }
        }
    }

    async Task ProcessSSOToken(string token)
    {
        var returnUrl = "Index";
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("tu-clave-secreta-muy-segura-de-al-menos-32-caracteres");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "App2",
                ValidAudience = "App1",
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var User = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(User))
            {
                vecinosModel = await _Ciudadano.GetOneAsync(Convert.ToDecimal(User));

                if (vecinosModel == null)
                    return;

                AuthManager _auth = new(_HttpContextAccessor);
                _auth.Usuario = User;
                _auth.Nombre = vecinosModel.Nombre;
                _auth.Apellido = vecinosModel.Apellido;
                _auth.Celular = vecinosModel.PrefijoCelular + " " + vecinosModel.Celular;
                _auth.Email = vecinosModel.Email;
                _auth.Dni = User;
                _auth.Postback = "";
                _auth.setSession();

                var httpContext = _HttpContextAccessor.HttpContext;
                returnUrl = httpContext.Request.Query["returnurl"];

                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = "Index";

                httpContext.Response.Redirect(returnUrl);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }

    async Task onLogin()
    {
        if (await _Login.LoginAsync(Model.Nombre, Model.Clave) == false)
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