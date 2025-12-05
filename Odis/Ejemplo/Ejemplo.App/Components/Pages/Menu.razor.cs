using Ejemplo.App.Utils.Auth;
using Ejemplo.DataManager;
using Ejemplo.DataManager.Models;

using Microsoft.AspNetCore.Components;

namespace Ejemplo.App.Components.Pages;

partial class Menu
{
    #region entidades
    [Inject] IHttpContextAccessor httpContextAccessor { get; set; }

    LutParametrosModel parametrosModel = new();
    LutParametrosDataManager _param = new();
    AuthManager auth;
    string PoliticasPrivacidad;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        try
        {
            auth = new AuthManager(httpContextAccessor);

            parametrosModel = await _param.GetOneAsync("GDA_PoliticasPrivacidad");
            PoliticasPrivacidad = parametrosModel.Valor;

            StateHasChanged();
        }
        catch (Exception ex)
        {
        }
    }

    private void onLogout()
    {
        _Navigation.NavigateTo("Login?action=LOGOUT", true);
    }

    private void OnClickGestiones()
    {
        _Navigation.NavigateTo("MisGestiones", true);
    }

    private void OnClickServicios()
    {
        _Navigation.NavigateTo("Servicios", true);
    }
    private void OnClickTelefonos()
    {
        _Navigation.NavigateTo("Contacto", true);
    }
    private void OnClickNovedades()
    {
        _Navigation.NavigateTo("Novedades", true);
    }
}
