using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GDA.Core.Components.Forms;

public partial class InputCuit
{
    string montoPreFormat;
    decimal montoFinal;

    protected override async Task OnInitializedAsync()
    {

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await JS.InvokeVoidAsync("initCuitInputs");
            }
            catch { }

        }
    }

    public decimal GetValue()
    {

        montoFinal = decimal.Parse(montoPreFormat.Replace("-", ""));
        return montoFinal;

    }

}