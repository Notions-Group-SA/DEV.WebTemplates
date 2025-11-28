using Microsoft.AspNetCore.Authentication;

namespace Ejemplo.BackOffice.Components.Pages.Usuarios;

public partial class Logout
{
    protected override async Task OnInitializedAsync()
    {
        var httpContext = _HttpContextAccessor.HttpContext;

        if (httpContext != null)
        {
            await httpContext.SignOutAsync("Cookies");

            _Navigation.NavigateTo("Login", forceLoad: true);
        }
    }
}