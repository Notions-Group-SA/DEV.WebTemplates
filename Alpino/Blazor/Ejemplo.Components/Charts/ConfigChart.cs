using System.Data;

namespace GDA.Core.Components.Charts;

public enum ChartType
{
    line,
    bar,
    pie,
    doughnut,
    radar,
    polarArea
}

public class ConfigChart
{
    public string type { get; set; } = ChartType.line.ToString();
    public DataChar data { get; set; } = new DataChar();
    public List<OptionChart> options { get; set; } = new List<OptionChart>();

    public ConfigChart() { }
    public ConfigChart(DataTable dt, string titulo, string xNombre, string yNombre) 
    {
        type = ChartType.line.ToString();
        data = new DataChar
        {
            labels = new List<string>(),
            datasets = new List<DataSetChart>(){
                        new DataSetChart
                        {
                            label = titulo,
                            data = new List<int> {  },
                            borderColor = "rgba(241,95,121, 0.2)",
                            backgroundColor = "rgba(241,95,121, 0.5)",
                            pointBorderColor = "rgba(241,95,121, 0.3)",
                            pointBackgroundColor = "rgba(241,95,121, 0.2)",
                            pointBorderWidth = 1
                        }, }
        };
        options = new List<OptionChart>
        {
            new OptionChart { responsive = true, legend = false }
        };

        data.labels.Clear();
        data.datasets[0].data.Clear();
        foreach (DataRow dataRow in dt.Rows)
        {
            data.labels.Add(dataRow[xNombre].ToString());
            data.datasets[0].data.Add(Convert.ToInt32(dataRow[yNombre]));
        }
   }

    public ConfigChart(List<DataChart> dt, string titulo)
    {
        type = ChartType.line.ToString();
        data = new DataChar
        {
            labels = new List<string>(),
            datasets = new List<DataSetChart>(){
                        new DataSetChart
                        {
                            label = titulo,
                            data = new List<int> {  },
                            borderColor = "rgba(241,95,121, 0.2)",
                            backgroundColor = "rgba(241,95,121, 0.5)",
                            pointBorderColor = "rgba(241,95,121, 0.3)",
                            pointBackgroundColor = "rgba(241,95,121, 0.2)",
                            pointBorderWidth = 1
                        }, }
        };
        options = new List<OptionChart>
        {
            new OptionChart { responsive = true, legend = false }
        };

        data.labels.Clear();
        data.datasets[0].data.Clear();
        foreach (DataChart dataRow in dt)
        {
            data.labels.Add(dataRow.Label);
            data.datasets[0].data.Add(Convert.ToInt32(dataRow.Data));
        }
    }
}

public class DataChar
{
    public List<string> labels { get; set; } = new List<string>();
    public List<DataSetChart> datasets { get; set; } = new List<DataSetChart>();
}

public class DataSetChart
{
    public string label { get; set; }
    public List<int> data { get; set; }
    public string borderColor { get; set; }
    public string backgroundColor { get; set; }
    public string pointBorderColor { get; set; }
    public string pointBackgroundColor { get; set; }
    public int pointBorderWidth { get; set; }
}

public class OptionChart
{
    public bool responsive { get; set; }
    public bool legend { get; set; }
}

public class DataChart
{
    public string Label { get; set; }
    public int Data {  get; set; }
}
