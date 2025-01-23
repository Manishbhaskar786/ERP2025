using Abp.Application.Services;
using System.Data;

namespace ERPack.Helpers
{
    public interface IExcelHelper : IApplicationService
    {
        DataTable ExportToDataTable(string sData);
        void CreateExcelDocument(DataTable dataTable, string excelFilePath);
    }
}
