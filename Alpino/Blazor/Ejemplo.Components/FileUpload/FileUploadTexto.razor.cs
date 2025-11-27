using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace GDA.Core.Components
{
    public partial class FileUploadTexto
    {
        [Parameter] public List<FileWrapper> selectedFiles { get; set; } = new();
        [Parameter] public string GUID { get; set; }
        [Parameter] public string FilePath { get; set; }
        [Parameter] public bool TieneVencimiento { get; set; }
        [Parameter] public DateTime? FechaVencimiento { get; set; } = null;
        [Parameter] public EventCallback<DateTime?> FechaVencimientoChanged { get; set; }

        private int inputFileKey = 0;
        private string message = "";
        private bool mostrarErrorFecha = false;

        public class FileWrapper
        {
            public IBrowserFile File { get; set; }
            public string FullPath { get; set; }
            public string Name { get; set; }
            public byte[] Content { get; set; }
            public string Description { get; set; } = ""; 
        }

        public bool EsValido()
        {
            bool mostrarError = false;
            message = "";

            if (selectedFiles.Count == 0)
            {
                message = "No se han seleccionado archivos.";
                mostrarError = true;
            }

            if (TieneVencimiento && (FechaVencimiento == null || FechaVencimiento == default(DateTime)))
            {
                mostrarErrorFecha = true;
                mostrarError = true;
                StateHasChanged();
            }

            return !mostrarError;
        }

        private async Task HandleFilesSelection(InputFileChangeEventArgs e)
        {
            await ProcessFiles(e.GetMultipleFiles());
        }

        private async Task ProcessFiles(IEnumerable<IBrowserFile> files)
        {
            message = "";
            mostrarErrorFecha = false;

            try
            {
                foreach (var file in files)
                {
                    if (selectedFiles.Any(f => f.File.Name == file.Name))
                    {
                        continue; 
                    }

                    using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);

                    selectedFiles.Add(new FileWrapper
                    {
                        File = file,
                        Content = ms.ToArray(),
                        Name = $"{GUID}_{file.Name}",
                        Description = ""
                    });
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                message = $"Error al subir archivos:";
            }
        }

        private void UpdateFileDescription(int index, string description)
        {
            if (index >= 0 && index < selectedFiles.Count)
            {
                selectedFiles[index].Description = description ?? "";
            }
        }

        private void RemoveFile(string fileName)
        {
            message = "";
            mostrarErrorFecha = false;
            selectedFiles.RemoveAll(f => f.Name == fileName);
            inputFileKey++;
            StateHasChanged();
        }

        private string GetFileIcon(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".pdf" => "fas fa-file-pdf text-danger",
                ".doc" or ".docx" => "fas fa-file-word text-primary",
                ".xls" or ".xlsx" => "fas fa-file-excel text-success",
                ".ppt" or ".pptx" => "fas fa-file-powerpoint text-warning",
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => "fas fa-file-image text-info",
                ".txt" => "fas fa-file-alt text-secondary",
                ".zip" or ".rar" or ".7z" => "fas fa-file-archive text-dark",
                _ => "fas fa-file text-muted"
            };
        }

        private string FormatFileSize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int counter = 0;
            decimal number = bytes;

            while (Math.Round(number / 1024) >= 1)
            {
                number /= 1024;
                counter++;
            }

            return $"{number:n1} {suffixes[counter]}";
        }

        public async Task UploadFiles()
        {
            message = "";
            mostrarErrorFecha = false;

            if (!EsValido())
            {
                return;
            }

            try
            {
                var uploadPath = FilePath;
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var fileWrapper in selectedFiles)
                {
                    var uniqueFileName = fileWrapper.Name;
                    var filePath = Path.Combine(uploadPath, uniqueFileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    using var ms = new MemoryStream(fileWrapper.Content);
                    await ms.CopyToAsync(stream);

                    if (!string.IsNullOrEmpty(fileWrapper.Description))
                    {
                        var metaPath = Path.Combine(uploadPath, $"{uniqueFileName}.meta");
                        await File.WriteAllTextAsync(metaPath, fileWrapper.Description);
                    }
                }

                message = $"Se han subido {selectedFiles.Count} archivos correctamente.";
                selectedFiles.Clear();
                inputFileKey++;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
            }
        }

        private async Task NotificarCambio()
        {
            if (FechaVencimiento != null && FechaVencimiento != default(DateTime))
            {
                mostrarErrorFecha = false;
                message = "";
            }
            await FechaVencimientoChanged.InvokeAsync(FechaVencimiento);
        }

    }
}


