

== agregar en el settings ==

{
  "NotionsServicesUtilsConf": {
    "HostUrl": "https://utils.gobdigital.com.ar"
  },
  ...
}


== copiar el javascript al proyecto ==

wwwroot\js\utils\downloadfile\downloadFileFromStream.js


== En el Programa.cs ==

en el region configuraciones

```csharp
var apiSettings = builder.Configuration.GetSection("NotionsServicesUtilsConf").Get<NotionsServicesUtilsConf>();
builder.Services.AddSingleton(new ExcelNoDTOClientService(apiSettings));
```

== Ejemplos ==

```csharp
async protected Task<Stream> GetFileStream()
    {
        DataTable dt = (await GetAll()).Tables[0];
        var randomBinaryData = await _exportarExcelService.ExportarAExcelByteArrayAsync(dt);
        var fileStream = new MemoryStream(randomBinaryData);

        return fileStream;
    }

    async public Task OnExportar()
    {        
        try
        {
            var fileStream = await GetFileStream();
            var fileName = "prueba.xls";

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
```