using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

namespace GDA.Core.Ciudadano.Components.Pages.Usuario;

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