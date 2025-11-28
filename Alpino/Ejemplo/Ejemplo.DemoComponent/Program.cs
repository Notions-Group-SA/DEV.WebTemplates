using Ejemplo.Components;
using Ejemplo.DemoComponent;
using Ejemplo.DemoComponent.Components;
using GDA.Core.BackOffice.Funcionarios.Components;
using GDA.Core.DataManager;
using GDA.Core.Utils;
using GDA.Core.Utils.Notificaciones;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Notions.Core.Security;
using Notions.Core.Utils.DataManager;
using Notions.Core.Utils.ExcelAPIClient.Services;
using SGM.Core.Services.Notions;
using SGM.Services;
using SGM.Services.GDI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region configuraciones

string pathBase = builder.Configuration["PathBase"];

ConnectionFactoryCore.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton(new ConnectionString(ConnectionFactoryCore.ConnectionString));

#endregion

#region utilidades de notions
builder.Services.AddSingleton(new NgCrypto());
#endregion

#region entidades del datatier
builder.Services.AddSingleton(new LutParametrosDataManager());
builder.Services.AddSingleton(new SysVecinosDataManager());
builder.Services.AddSingleton(new LutLocalidadesDataManager());
builder.Services.AddSingleton(new LutModulosDataManager());
builder.Services.AddSingleton(new LutModulosCiudadanaDataManager());
builder.Services.AddSingleton(new SysIncidentesDataManager());
builder.Services.AddSingleton(new LutMotivosIncidenteDataManager());
builder.Services.AddSingleton(new LutEstadosIndicenteEjecucionDataManager());
builder.Services.AddSingleton(new LutTiposComerciosDataManager());
builder.Services.AddSingleton(new SysIncidentesFilesDataManager());
builder.Services.AddSingleton(new SysTiposTasaxCausaDataManager());
builder.Services.AddSingleton(new LutEstadosCausaDataManager());
builder.Services.AddSingleton(new SysCausasDataManager());
builder.Services.AddSingleton(new LutEstadosIndicenteDataManager());
builder.Services.AddSingleton(new SGM_Lut_TipoSociedad_DocumentacionRequerida());
builder.Services.AddSingleton(new SGM_Sys_RubrosComercio_DocumentacionRequerida());
builder.Services.AddSingleton(new SGM_Sys_TipoRiesgo_DocumentosRequeridos());
builder.Services.AddSingleton(new SysComerciosDocumentosDataManager());
builder.Services.AddSingleton(new SysComerciosCodigosActividadDataManager());
builder.Services.AddSingleton(new LutMotivosIncidenteEtiquetasDataManager());
builder.Services.AddSingleton(new LutBarriosDataManager());
builder.Services.AddSingleton(new SGM_Lut_TiposDocumentos());
builder.Services.AddSingleton(new SGM_Sys_Comercios_Documentos());
builder.Services.AddSingleton(new SysComerciosDataManager());
builder.Services.AddSingleton(new SysComerciosApoderadosDataManager());
builder.Services.AddSingleton(new LutComprasTiposSociedadDataManager());
builder.Services.AddSingleton(new SysIncidentesEtiquetasDataManager());
builder.Services.AddSingleton(new SysTurnosDataManager());
builder.Services.AddSingleton(new SysProfesionalesDocumentosDataManager());
builder.Services.AddSingleton(new SysProfesionalesDataManager());
builder.Services.AddSingleton(new LutTiposProfesionalesDataManager());
builder.Services.AddSingleton(new NotificacionPushService());
builder.Services.AddSingleton(new LutTiposTicketsDataManager());
builder.Services.AddSingleton(new LutTiposIncidenteDataManager());
builder.Services.AddSingleton(new LutMotivosIncidenteDataManager());
builder.Services.AddSingleton(new LutCanalesIndicenteDataManager());
builder.Services.AddSingleton(new LutOrigenTicketDataManager());
builder.Services.AddSingleton(new SysIncidentesPendientesDataManager());
builder.Services.AddSingleton(new LutTiposUsuarioDataManager());
builder.Services.AddSingleton(new LutSectoresDataManager());
builder.Services.AddSingleton(new LutDireccionesDataManager());
builder.Services.AddSingleton(new LutSecretariasDataManager());
builder.Services.AddSingleton(new SysNotasIncidentesDataManager());
builder.Services.AddSingleton(new SysUsuariosDataManager());
builder.Services.AddSingleton(new SysLoginVecinoDigitalDataManager());
builder.Services.AddSingleton(new LutMotivosCierreIncidenteDataManager());
builder.Services.AddSingleton(new SysTareasIncidentesDataManager());
builder.Services.AddSingleton(new LutMotivosCierreTareasIncidenteDataManager());
builder.Services.AddSingleton(new SysIncidentesFilesDataManager());
builder.Services.AddSingleton(new SysIncidentesMensajesDataManager());
builder.Services.AddSingleton(new LutOrigenTicketDataManager());
builder.Services.AddSingleton(new LutTiposDocumentosDataManager());
builder.Services.AddSingleton(new SysIncidentesFilesEventosDataManager());
builder.Services.AddSingleton(new SysDocumentosProfesionalesEventosDataManager());
builder.Services.AddSingleton(new LutMotivosIncidenteDocumentacionRequeridaDataManager());
builder.Services.AddSingleton(new SysIncidentesFilesEventosDocumentosDataManager());
builder.Services.AddSingleton(new LutRubrosComerciosDataManager());
builder.Services.AddSingleton(new SysInmueblesDataManager());
builder.Services.AddSingleton(new LutTiposInmueblesDataManager());
builder.Services.AddSingleton(new SysTitularesInmuebleDataManager());
builder.Services.AddSingleton(new SysInmueblesFilesDataManager());
builder.Services.AddSingleton(new LutTiposDocumentosInmueblesDataManager());
builder.Services.AddSingleton(new LutCodigosActividadComercioDataManager());
builder.Services.AddSingleton(new SysComerciosFotosDataManager());
builder.Services.AddSingleton(new LutTiposRiesgoComerciosDataManager());
builder.Services.AddSingleton(new SysIncidentesEtiquetasDataManager());
builder.Services.AddSingleton(new SysNotificacionesConfiguracionGeneralDataManager());
builder.Services.AddSingleton(new LutTiposTareaIncidenteDataManager());
builder.Services.AddSingleton(new SysNotificacionesFuncionariosDataManager());
builder.Services.AddSingleton(new SysErroresDataManager());
builder.Services.AddSingleton(new SysIncidentesFilesTmpDataManager());
builder.Services.AddSingleton(new LutSeguridadZonasDataManager());
#endregion

#region entidades de servicios - gateway
builder.Services.AddSingleton(new GDI_Deuda());
builder.Services.AddSingleton<TicketService>();
builder.Services.AddSingleton<FuncionarioService>();
builder.Services.AddSingleton(new ExcelNoDTOClientService());
builder.Services.AddSingleton<ParametrosService>();
builder.Services.AddScoped<GMapsService>();
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
builder.Services.AddAuthentication()
    .AddJwtBearer("JWT", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "App2", // Identificador de la aplicación origen
            ValidAudience = "App1", // Identificador de la aplicación destino
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tu-clave-secreta-muy-segura-de-al-menos-32-caracteres"))
        };
    });
#endregion

#region netcore components
builder.Services.AddHttpClient();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
#endregion

var app = builder.Build();

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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
