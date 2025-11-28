using GDA.Core.DataManager.Models;
using GDA.Core.DataManager.Models.Insert;
using Microsoft.AspNetCore.Components;


namespace GDA.Core.Components
{
    public partial class ModalSwitchesComponent
    {
        #region Parametros
        [Parameter] public bool showModal { get; set; } = true;
        [Parameter] public string Usuario { get; set; } = string.Empty;
        [Parameter] public EventCallback<bool> ShowChanged { get; set; }
        [Parameter] public List<SysNotificacionesConfiguracionGeneralModel> ConfiguracionUsuario { get; set; } = new();
        [Parameter] public EventCallback<List<InsertSysNotificacionesConfiguracionGeneralModel>> OnConfiguracionChanged { get; set; }

        #endregion

        #region State Variables
        private bool esTodosSeleccionado => ConfiguracionesUsuario?.FirstOrDefault(x => x.Id == 0)?.IsEnabled ?? false;

        #endregion

        #region Form Variables
        private List<ConfigItem> ConfiguracionesUsuario = null;
        private List<LutTiposTicketsModel> TiposTickets = new();

        #endregion

        #region Initialize Events
        protected override async Task OnInitializedAsync()
        {
            TiposTickets = await _TipoTickets.GetListBy_ActivoAsync(true);
            CargarConfiguracion();
        }
        private void CargarConfiguracion()
        {
            if (TiposTickets == null || ConfiguracionUsuario == null)
                return;

            ConfiguracionesUsuario = new List<ConfigItem>();

            bool todosSeleccionado = ConfiguracionUsuario.Any(config => config.IdTipoTicket == 0);

            ConfiguracionesUsuario.Add(new ConfigItem
            {
                Id = 0,
                Name = "Todos",
                Description = "Recibir notificaciones de todos los tipos de tickets permitidos",
                IsEnabled = todosSeleccionado
            });

            foreach (LutTiposTicketsModel tipo in TiposTickets)
            {
                var configItem = new ConfigItem
                {
                    Id = tipo.Id,
                    Name = tipo.Descripcion,
                    Description = $"Recibir notificaciones de {tipo.Descripcion}",
                    IsEnabled = todosSeleccionado || ConfiguracionUsuario.Any(config => config.IdTipoTicket == tipo.Id)
                };
                ConfiguracionesUsuario.Add(configItem);
            }
        }
        #endregion

        #region Manejo de cambios en Modal
        private void OnTodosChanged(bool todosSeleccionado)
        {
            var todoItem = ConfiguracionesUsuario.FirstOrDefault(x => x.Id == 0);
            if (todoItem == null) return;

            todoItem.IsEnabled = todosSeleccionado;

            foreach (var config in ConfiguracionesUsuario.Where(x => x.Id != 0))
            {
                config.IsEnabled = todosSeleccionado;
            }

            StateHasChanged();
        }
        private void OnIndividualChanged(int configId, bool isEnabled)
        {
            var item = ConfiguracionesUsuario.FirstOrDefault(x => x.Id == configId);
            if (item != null)
            {
                item.IsEnabled = isEnabled;
            }

            var todoItem = ConfiguracionesUsuario.FirstOrDefault(x => x.Id == 0);
            if (todoItem != null)
            {
                var itemsIndividuales = ConfiguracionesUsuario.Where(x => x.Id != 0);
                todoItem.IsEnabled = itemsIndividuales.All(x => x.IsEnabled);
            }

            StateHasChanged();
        }
        private void OnSwitchChanged(int configId, bool isEnabled)
        {
            if (configId == 0)
            {
                OnTodosChanged(isEnabled);
            }
            else
            {
                OnIndividualChanged(configId, isEnabled);
            }
        }
        
        #endregion

        #region Guardar/Cerrar Modal
        private async Task CerrarModal()
        {
            await ShowChanged.InvokeAsync(false);
        }

        private async Task GuardarConfiguracion()
        {
            var todoItem = ConfiguracionesUsuario.FirstOrDefault(x => x.Id == 0);
            List<InsertSysNotificacionesConfiguracionGeneralModel> nuevaConfiguracion;
            List<SysNotificacionesConfiguracionGeneralModel> ConfiguracionActualizada= new();

            if (todoItem?.IsEnabled == true)
            {
                nuevaConfiguracion = new List<InsertSysNotificacionesConfiguracionGeneralModel>
                {
                    new InsertSysNotificacionesConfiguracionGeneralModel
                    {
                         Usuario = Usuario,
                         IdTipoTicket=0,
                         FechaAlta=DateTime.Now,
                         UsuarioAlta= Usuario                   
                    }
                };
            }
            else
            {
                nuevaConfiguracion = ConfiguracionesUsuario
                .Where(x => x.Id != 0 && x.IsEnabled)
                .Select(config => new InsertSysNotificacionesConfiguracionGeneralModel
                {
                    Usuario = Usuario,
                    IdTipoTicket = config.Id,
                    FechaAlta = DateTime.Now,
                    UsuarioAlta = Usuario
                })
                .ToList();
            }

            await OnConfiguracionChanged.InvokeAsync(nuevaConfiguracion);


            foreach (InsertSysNotificacionesConfiguracionGeneralModel config in nuevaConfiguracion)
            {
                var modeloCompleto = new SysNotificacionesConfiguracionGeneralModel
                {
                    IdTipoTicket = config.IdTipoTicket,
                    Usuario = config.Usuario,
                    FechaAlta = config.FechaAlta,
                    UsuarioAlta = config.UsuarioAlta,
                };

                ConfiguracionActualizada.Add(modeloCompleto);

            }
            ConfiguracionUsuario = ConfiguracionActualizada;
          
            await CerrarModal();
            //StateHasChanged();

        }

        #endregion

    }

    #region ViewModels
        public class ConfigItem
        {
            public int Id { get; set; } = -1;
            public string Name { get; set; }= string.Empty;
            public string Description { get; set; } = string.Empty;
            public bool IsEnabled { get; set; }
        }

    #endregion
}