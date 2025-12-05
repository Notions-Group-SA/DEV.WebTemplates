
using Ejemplo.Web.Utils.Auth;

namespace Ejemplo.Web.Components.Layout;

public partial class MainLayout
{
    #region entidades

    string UserNombre = "Dario Schener";
    string UserEmail = "schenerdario@gmail.com";


    AuthManager _auth; //nomenclatura inyección

    #endregion

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _auth = new(_HttpContextAccessor);

            //consulta a la base
            /*
            var model = await _auth.GetUserInfo(_auth.Usuario);
            UserNombre = model.nombre;
            UserEmail = model.email;
            */
        }
        catch (Exception)
        {

        }
        
        await base.OnInitializedAsync();
        StateHasChanged();
    }

}