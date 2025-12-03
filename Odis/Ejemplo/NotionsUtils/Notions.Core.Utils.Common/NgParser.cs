using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notions.Core.Utils.Common;
public class NgParser
{
    public  DataTable JsonToDataTable(string json)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        var jsonData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
        DataTable dataTable = new DataTable();

        if (jsonData != null && jsonData.Count > 0)
        {
            // Crear las columnas del DataTable a partir de las claves del primer diccionario
            foreach (var key in jsonData[0].Keys)
            {
                dataTable.Columns.Add(key);
            }

            // Llenar las filas
            foreach (var item in jsonData)
            {
                DataRow row = dataTable.NewRow();
                foreach (var key in item.Keys)
                {
                    row[key] = item[key];
                }
                dataTable.Rows.Add(row);
            }
        }

        return dataTable;   
    }
}
