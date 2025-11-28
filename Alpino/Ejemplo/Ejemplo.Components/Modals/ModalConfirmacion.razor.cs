using Microsoft.AspNetCore.Components;

namespace Ejemplo.Components.Modals;

public partial class ModalConfirmacion
{
    [Parameter] public string Titulo { get; set; } = "";
    [Parameter] public string Mensaje { get; set; } = "";
    [Parameter] public string BotonConfirmar { get; set; }
    [Parameter] public EventCallback OnConfirmar { get; set; }
    [Parameter] public bool Visible { get; set; }
    [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter] public string CancelClass { get; set; } = "btn-secondary btn-simple";
    [Parameter] public string ConfirmClass { get; set; } = "btn-success";

    private Task CerrarModal() => VisibleChanged.InvokeAsync(false);

    private async Task Confirmar()
    {
        await OnConfirmar.InvokeAsync();
        await CerrarModal();
    }

}
