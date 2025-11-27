using Microsoft.AspNetCore.Components;
using System.Data;
namespace GDA.Core.Components
{
    public partial class TablaModal<TItem> : ComponentBase
    {
        [Parameter] public string Titulo { get; set; } = "";
        [Parameter] public string Subtitulo { get; set; } = "";
        [Parameter] public bool Visible { get; set; }
        [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
        [Parameter] public string MensajeSinDatos { get; set; } = "No hay datos disponibles";

        [Parameter] public List<TItem> Items { get; set; } = new();
        [Parameter] public RenderFragment<TItem>? RowTemplate { get; set; }

        [Parameter] public DataTable DataTableItems { get; set; }
        [Parameter] public RenderFragment<DataRow> DataTableRowTemplate { get; set; }

        [Parameter] public DataRow SingleDataRow { get; set; }
        [Parameter] public RenderFragment<DataRow> SingleRowTemplate { get; set; }

        [Parameter] public RenderFragment Header { get; set; }

        private bool UsarDataTable => DataTableItems != null && DataTableItems.Rows.Count > 0;
        private bool UsarSingleRow => SingleDataRow != null ;

        private Task CerrarModal() => VisibleChanged.InvokeAsync(false);

        private bool TieneDatos()
        {
            return UsarDataTable || UsarSingleRow || (Items != null && Items.Any());
        }

        private RenderFragment RenderDataTableRows() => builder =>
        {
            if (DataTableItems == null || DataTableRowTemplate == null) return;

            int sequence = 0;
            foreach (DataRow row in DataTableItems.Rows)
            {
                builder.OpenElement(sequence++, "tr");
                builder.AddContent(sequence++, DataTableRowTemplate(row));
                builder.CloseElement();
            }
        };

    }
}