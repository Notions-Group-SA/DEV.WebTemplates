using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GDA.Core.Components.Forms;

public partial class InputMonto
{
    string montoPreFormat;
    decimal montoFinal;



    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("initMoneyInputs");
        }
    }

    public decimal GetValue()
    {
        montoFinal = decimal.Parse(montoPreFormat.Replace(".", ""));
        return montoFinal;

    }


}