using GDA.Core.Ciudadano.Utils.Auth;
using GDA.Core.DataManager.Models;

namespace GDA.Core.Ciudadano.Components.Pages.Usuario;

public partial class Perfil
{
    #region State Variables

    AuthManager _auth;

    #endregion

    #region Form Variables

    SysVecinosModel VecinoModel { get; set; } = new();
    SysLoginVecinoDigitalModel LoginModel { get; set; } = new SysLoginVecinoDigitalModel();

    #endregion

    protected override async Task OnInitializedAsync()
    {
        _auth = new AuthManager(_HttpContextAccessor);
        VecinoModel = await _Ciudadanos.GetOneAsync(decimal.Parse(_auth.Usuario ?? "0"));
        LoginModel = await _LoginVecino.GetOneAsync(_auth.Usuario ?? "0");

        if (VecinoModel == null)
            return;
    }

}