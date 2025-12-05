using Ejemplo.App.Utils.Auth;
using Ejemplo.DataManager;
using Ejemplo.DataManager.Models;
using GDA.Core.CiudadanoApp.Utils.Auth;
using GDA.Core.DataManager;
using GDA.Core.DataManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System;
using System.Net;
using System.Web;
using static Ejemplo.App.Components.Pages.Index;


namespace GDA.Core.CiudadanoApp.Components.Pages;

[Authorize]
partial class Index : ComponentBase
{
    #region entidades

    [Inject] LutTiposTicketsDataManager _TipoTicket { get; set; }

    private bool hasReclamos = true;
    private bool hasTurnos = true;
    private bool hasTramites = true;
    private bool hasDenuncias = true;
    private bool hasTelefonosUtiles = true;
    private bool hasFarmacias = true;
    private bool hasMisGestiones = true;
    private bool hasLicencia = true;
    private bool hasMedido = true;
    private bool hasHabilitaciones = true;
    private bool hasOjosAlerta = true;
    private bool hasBoleteria = true;
    private bool hasDillo = true;
    private bool hasTaxi = true;
    private bool hasJuzgado = true;
    private bool hasBotonesEmergencia = true;
    private bool hasBotonPoliciaLocal = true;
    private bool hasNovedades = false;
    private bool hasDefensaConsumidor = false;
    private bool hasObrasParticulares = false;
    private string url;
    string urlEncode;
    private string ReclamoTitulo;
    LutModulosCiudadanaDataManager _modulos;
    LutParametrosDataManager _param = new();
    LutParametrosModel parametrosModel = new();
    LutBotonesPanicoDataManager _botonPago = new();
    LutBotonesPanicoModel botonesPanicoModel = new();
    LutServiciosCiudadanoDataManager _servicios = new();
    List<LutServiciosCiudadanoModel> ListaServicios = new();
    AuthManager _auth;
    LutTiposTicketsModel TipoTicketModel = new();
    List<SysNoticiasModel> ListaNoticias = new();
    private List<CarouselItem> items = new List<CarouselItem>();
    bool isLoading = true;

    #endregion 

    protected override async Task OnInitializedAsync()
    {
        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
            _auth = new(_httpContextAccessor);
            _auth.getSession();

            parametrosModel = await _param.GetOneAsync("GDA_IP_WebServer");
            url = parametrosModel.Valor;

            version = Core.CiudadanoApp.AppVersion.Version;

            ListaServicios = await _servicios.GetListBy_ActivoAppInicioAsync(true, true);

            TipoTicketModel = await _TipoTicket.GetOneAsync(1);

            botonesPanicoModel = await _botonPago.GetOneAsync(7);
            hasBotonPoliciaLocal = botonesPanicoModel.Activo;

            ListaNoticias = await _Noticias.GetListAllAsync();
            if (ListaNoticias.Count > 0)
            {
                #region Novedades

                hasNovedades = true;
                CarouselItem item = new CarouselItem();
                foreach (SysNoticiasModel noticia in ListaNoticias)
                {
                    item = new CarouselItem();
                    item.Id = noticia.Id;
                    item.Description = noticia.Titulo;
                    item.ImageUrl = url + "/uploads/noticias/web/" + noticia.Imagen;
                    items.Add(item);
                }

                #endregion
            }

            #region Evaluo Modulos a mostrar

            if (_auth.Usuario == "30886698")
            {
                hasReclamos = true;
                hasTurnos = true;
                hasTramites = true;
                hasDenuncias = true;
                hasFarmacias = true;
                hasMisGestiones = true;
                hasTelefonosUtiles = true;
                hasLicencia = true;
                hasMedido = true;
                hasHabilitaciones = true;
                hasOjosAlerta = true;
                hasBoleteria = true;
                hasDillo = true;
                hasTaxi = true;
                hasJuzgado = true;
                hasBotonesEmergencia = true;
                hasDefensaConsumidor = true;
                hasObrasParticulares = true;

                return;
            }

            LutModulosCiudadanaModel modulo;
            _modulos = new LutModulosCiudadanaDataManager();
            List<LutModulosCiudadanaModel> lista = await _modulos.GetListAllAsync();

            modulo = lista.Find(p => p.Id == 1);
            if (modulo != null)
                hasReclamos = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 2);
            if (modulo != null)
                hasTurnos = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 3);
            if (modulo != null)
                hasTramites = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 4);
            if (modulo != null)
                hasDenuncias = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 15);
            if (modulo != null)
                hasFarmacias = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 17);
            if (modulo != null)
                hasTelefonosUtiles = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 10);
            if (modulo != null)
                hasLicencia = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 8);
            if (modulo != null)
                hasMedido = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 11);
            if (modulo != null)
                hasHabilitaciones = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 16);
            if (modulo != null)
                hasOjosAlerta = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 9);
            if (modulo != null)
                hasBoleteria = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 19);
            if (modulo != null)
                hasDillo = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 18);
            if (modulo != null)
                hasTaxi = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 7);
            if (modulo != null)
                hasJuzgado = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 5);
            if (modulo != null)
                hasBotonesEmergencia = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 21);
            if (modulo != null)
                hasDefensaConsumidor = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 23);
            if (modulo != null)
                hasObrasParticulares = modulo.Activo && modulo.AppInicio;

            modulo = lista.Find(p => p.Id == 24);
            if (modulo != null)
                hasMisGestiones = modulo.Activo && modulo.AppInicio;

            #endregion

        }
        catch (Exception ex)
        {

        }

        StateHasChanged();

    }

    protected async Task OnClickPolicia()
    {
        redirectBoton(1);
    }
    protected async Task OnClickPoliciaLocal()
    {
        redirectBoton(7);
    }
    protected async Task OnClickBomberos()
    {
        redirectBoton(2);
    }
    protected async Task OnClickAmbulancia()
    {
        redirectBoton(3);
    }
    private async Task redirectBoton(int IdBoton)
    {
        botonesPanicoModel = await _botonPago.GetOneAsync(IdBoton);
        urlEncode = "?tel=" + botonesPanicoModel.Telefono + "&apiAsyncTask=" + HttpUtility.UrlEncode(url + "/api/BotonPanico/SetEvento?Usuario=" + _auth.Usuario + "&Telefono=" + _auth.Celular + "&Fecha=" + DateTime.Now.ToString("dd/MM/yyyy") + "&IdBoton=1&Finalizo=true&CoordenadaExacta=true");
        _Navigation.NavigateTo(urlEncode, true);
    }
    private void OnClickComercios()
    {
        _Navigation.NavigateTo("Comercios", true);
    }
    private void OnClickMedido()
    {
        _Navigation.NavigateTo("Siem", true);
    }
    private void OnClickReclamos()
    {
        _Navigation.NavigateTo("Reclamos", true);
    }
    private void OnClickTurnos()
    {
        _Navigation.NavigateTo("Turnos", true);
    }
    private void OnClickTramites()
    {
        _Navigation.NavigateTo("Tramites", true);
    }
    private void OnClickMultas()
    {
        _Navigation.NavigateTo("JuzgadoFaltas", true);
    }
    private void OnClickGestiones()
    {
        _Navigation.NavigateTo("MisGestiones", true);
    }
    private void OnClickFarmacias()
    {
        _Navigation.NavigateTo("Farmacias", true);
    }
    private void OnClickTelefonos()
    {
        _Navigation.NavigateTo("Contacto", true);
    }

    private void OnClickObrasParticulares()
    {
        _Navigation.NavigateTo("ObrasParticulares", true);
    }
    private void OnClickServicios()
    {
        _Navigation.NavigateTo("Servicios", true);
    }
}
