

== Uso ==

.razor
```
<Chart ConfigChart="dataChart" @ref="prueba"/>
```

asegurarse tener copiado en el proyecto lo siguiente:
```
   js/components/charts/main.js 

   @code {
    ConfigChart? dataChart=new ();

    Chart prueba;

    protected override async Task OnInitializedAsync()
    {
        //Ejemplo de consulta 
        DataTable dt1 = EjemploConsulta();
        var dataChart = new ConfigChart(dt1, "Titulo", "Label", "Data");
        dataChart.type = ChartType.line.ToString();

        //asignar 
        this.dataChart = dataChart;

        await Task.CompletedTask;
    }

    ...
    }
```
este js está en el proyecto de Components


==Ejemplo==
``` ejemplo
            ejemplo = new ConfigChart()
            {
                type = ChartType.line.ToString(),
                data = new DataChar
                {
                    labels = new List<string> { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio" },
                    datasets = new List<DataSetChart>
                    {
                        new DataSetChart
                        {
                            label = "Ejemplo 1",
                            data = new List<int> { 28, 58, 39, 45, 30, 55, 68 },
                            borderColor = "rgba(241,95,121, 0.2)",
                            backgroundColor = "rgba(241,95,121, 0.5)",
                            pointBorderColor = "rgba(241,95,121, 0.3)",
                            pointBackgroundColor = "rgba(241,95,121, 0.2)",
                            pointBorderWidth = 1
                        },
                        new DataSetChart
                        {
                            label = "Ejemplo 2",
                            data = new List<int> { 40, 28, 50, 48, 63, 39, 41 },
                            borderColor = "rgba(140,147,154, 0.2)",
                            backgroundColor = "rgba(140,147,154, 0.2)",
                            pointBorderColor = "rgba(140,147,154, 0)",
                            pointBackgroundColor = "rgba(140,147,154, 0.9)",
                            pointBorderWidth = 1
                        }
                    }
                    },
                options = new List<OptionChart>
                {
                    new OptionChart { responsive = true, legend = false }
                }
                };
```