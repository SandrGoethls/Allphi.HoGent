using Microsoft.JSInterop;
using OfficeOpenXml;

namespace AllPhi.HoGent.Blazor.Helpers
{
    public class ExcelExportHelper : IExcelExportHelper
    {
        private readonly IJSRuntime JS;

        public ExcelExportHelper(IJSRuntime js)
        {
            JS = js;
        }

        public async Task ExportDataAsync<T>(IEnumerable<T> data, string format, string fileName)
        {

            switch (format.ToLower())
            {
                case "csv":
                    await ExportToCsv(data, fileName);
                    break;
                case "xlsx":
                    await ExportToExcelAsync(data, fileName);
                    break;
                default:
                    throw new ArgumentException("Onbekend bestandsformaat", nameof(format));
            }
        }

        private async Task ExportToCsv<T>(IEnumerable<T> data, string fileName)
        {
            //var result = new StringBuilder();

            //// Reflecteer over de eigenschappen van T en maak een headerrij
            //var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //result.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            //// Voeg rijen toe voor elke record
            //foreach (var record in data)
            //{
            //    var values = properties.Select(p => p.GetValue(record, null)?.ToString());
            //    result.AppendLine(string.Join(",", values.Select(v => $"\"{v?.Replace("\"", "\"\"")}\"")));
            //}

            //// Converteer de StringBuilder naar een byte array
            //var byteArray = Encoding.UTF8.GetBytes(result.ToString());
            //var base64Data = Convert.ToBase64String(byteArray);

            //// Roep JSInterop aan om het bestand te downloaden
            //await JS.InvokeAsync<object>(
            //    "saveAsFile",
            //    fileName,
            //    base64Data
            //);
        }

        private async Task ExportToExcelAsync<T>(IEnumerable<T> data, string fileName = "Export.xlsx")
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // Nodig als je EPPlus gebruikt
            using var package = new ExcelPackage();

            var worksheet = package.Workbook.Worksheets.Add("Data");
            worksheet.Cells["A1"].LoadFromCollection(data, PrintHeaders: true);

            // Datums correct formatteren voor Excel
            worksheet.Column(1).Style.Numberformat.Format = "dd-mm-yyyy hh:mm:ss";

            // Auto breedte van kolommen
            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();
            worksheet.Column(7).AutoFit();

            var stream = new MemoryStream(package.GetAsByteArray());

            await JS.InvokeAsync<object>("saveAsFile",fileName,Convert.ToBase64String(stream.ToArray()));
        }
    }
}
