using Ejemplo.DataManager;
using Ejemplo.Utils;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;

using Notions.Core.Security;

using Notions.Core.Utils.DataManager;

using System.Globalization;
using System.Text;

using Ejemplo.DemoComponent.Components;

var builder = WebApplication.CreateBuilder(args);

#region cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("https://localhost:7177", "https://*.gobdigital.com.ar")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
#endregion

#region restapi y swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region configuraciones
string pathBase = builder.Configuration["PathBase"];

ConnectionFactoryCore.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton(new ConnectionString(ConnectionFactoryCore.ConnectionString));
#endregion

#region utilidades de notions
builder.Services.AddSingleton(new NgCrypto());
builder.Services.AddSingleton(new NgRandom());
#endregion

#region entidades del datatier
builder.Services.AddSingleton(new LutParametrosDataManager());
builder.Services.AddSingleton<LutPeriodosDataManager>();
builder.Services.AddSingleton(new SysUsuariosDataManager());
#endregion

#region entidades de servicios - gateway
builder.Services.AddSingleton<ParametrosService>();
#endregion

#region cookie y session
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.IsEssential = true;//algunos navegadores bloquean las cookies que no son esenciales
        options.Cookie.Name = "Cookies";
        options.LoginPath = "/Login";
        options.Cookie.MaxAge = null;// TimeSpan.FromMinutes(30);
        //options.IdleTimeout = TimeSpan.FromDays(30); //tiempo de inactividad
        options.ReturnUrlParameter = "returnurl";
        options.Cookie.HttpOnly = true; //evita acceso de javascript
        options.Cookie.SameSite = SameSiteMode.Lax;// Lax para casos como OAuth, OpenID Connect, etc.
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use Always in production
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
#endregion

#region autentificación externar - despues de las cookies de autenticación
#endregion

#region netcore components
builder.Services.AddHttpClient();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
#endregion

var app = builder.Build();

#region cultureinfo
var cultureInfo = new CultureInfo("es-ES");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
#endregion

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();

#region https disabled
//app.UseHttpsRedirection();

var forwardingOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardingOptions.KnownNetworks.Clear();
forwardingOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardingOptions);
#endregion

#region pathBase
if (!app.Environment.IsDevelopment())
{
    if (string.IsNullOrEmpty(pathBase) == false)
        app.UsePathBase(pathBase);
    else
        throw new NotImplementedException("Falta configurar parametro PathBase");
}
#endregion

#region restapi y swagger
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
#endregion

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
