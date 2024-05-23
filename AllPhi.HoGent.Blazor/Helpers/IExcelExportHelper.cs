namespace AllPhi.HoGent.Blazor.Helpers
{
    public interface IExcelExportHelper
    {
        Task ExportDataAsync<T>(IEnumerable<T> data, string format, string fileName);
    }
}
