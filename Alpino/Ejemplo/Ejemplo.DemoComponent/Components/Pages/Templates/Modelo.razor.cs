using Microsoft.AspNetCore.Components;

namespace Ejemplo.DemoComponent.Components.Pages.Templates;

public partial class Modelo
{
    #region Parametros

    [SupplyParameterFromQuery(Name = "t")]
    public string? TemplateId { get; set; }

    #endregion

    #region State Variables

    bool isLoading = true;
    bool isProcessing = false;
    bool isError = false;

    #endregion

    #region Form Variables

    #endregion

    #region Initialize Events

    protected override async Task OnInitializedAsync()
    {

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Delay(200);
            isLoading = false;
            StateHasChanged();
        }

    }

    protected override async Task OnParametersSetAsync()
    {
        //if (ComercioModel != null)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
    }

    #endregion

    #region Events

    #endregion

    #region Functions

    #endregion

    #region ViewModels


    #endregion
}