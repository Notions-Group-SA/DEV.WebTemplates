using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GDA.Core.Components.Forms;

public partial class InputDni
{
    string montoPreFormat;
    decimal montoFinal;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("initDniInputs");
        }
    }


    public decimal GetValue()
    {
        montoFinal = decimal.Parse(montoPreFormat.Replace(".", ""));
        return montoFinal;

    }

}