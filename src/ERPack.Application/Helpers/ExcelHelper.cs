using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ERPack.Estimates;
using ERPack.Estimates.Dto;
using ERPack.Workorders;
using ERPack.Workorders.Dto;
using ERPack.WorkOrders;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERPack.Helpers
{
    public class ExcelHelper : ERPackAppServiceBase, IExcelHelper
    {
        public ExcelHelper()
        {
           
        }

        public DataTable ExportToDataTable(string sData)
        {
            DataTable table = new DataTable();

            // Prepare Doc

            HtmlNodeCollection hc = GetNodeHtmlCollection(sData, "//tr");
            HtmlNodeCollection ths = GetNodeHtmlCollection(sData, "//th");

            #region  Table 

            foreach (HtmlNode nodeTh in ths)
            {
                if (!string.IsNullOrEmpty(nodeTh.InnerText) && nodeTh.InnerText.Trim() != "Actions")
                {
                    table.Columns.Add(nodeTh.InnerText.Trim());
                }
            }

            foreach (HtmlNode node in hc)
            {
                HtmlNodeCollection tds = GetNodeHtmlCollection(node.InnerHtml, "//td");
                if (tds == null) continue;
                int iCount = 0;
                DataRow dr = table.NewRow();
                try
                {
                    tds.RemoveAt(0);
                    tds.RemoveAt(tds.Count - 1);
                    foreach (HtmlNode nodeTd in tds)
                    {
                        dr[iCount] = nodeTd.InnerText;
                        iCount++;
                    }
                }
                catch
                {

                    continue;
                }
                table.Rows.Add(dr);//this will add the row at the end of the datatable
            }

            #endregion
            table.TableName = "Table1";
            return table;


        }

        public void CreateExcelDocument(DataTable dataTable, string excelFilePath)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet()
                {
                    Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = dataTable.TableName ?? "Sheet1"
                };
                sheets.Append(sheet);

                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                Row headerRow = new Row();
                foreach (DataColumn column in dataTable.Columns)
                {
                    Cell cell = new Cell()
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(column.ColumnName)
                    };
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Row newRow = new Row();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Cell cell = new Cell()
                        {
                            DataType = DetermineCellDataType(dataRow[column]), // Adjust the data type based on the data
                            CellValue = new CellValue(dataRow[column].ToString())
                        };
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
            }
        }
        private static EnumValue<CellValues> DetermineCellDataType(object data)
        {
            Type dataType = data.GetType();

            if (dataType == typeof(int) || dataType == typeof(long) ||
                dataType == typeof(short) || dataType == typeof(byte) ||
                dataType == typeof(uint) || dataType == typeof(ulong) ||
                dataType == typeof(ushort) || dataType == typeof(sbyte))
            {
                return CellValues.Number;
            }
            else if (dataType == typeof(float) || dataType == typeof(double) ||
                     dataType == typeof(decimal))
            {
                return CellValues.Number;
            }
            else if (dataType == typeof(DateTime))
            {
                // Note: For Date, you might need to set the cell's style to a date format
                return CellValues.Date;
            }
            else if (dataType == typeof(bool))
            {
                return CellValues.Boolean;
            }
            else if (dataType == typeof(string))
            {
                return CellValues.String;
            }
            // Add more data types if needed

            return CellValues.String; // Default
        }

        /// <summary>
        /// Returns all occurence of Inner Html Value of HTML Page matching XPath tag.
        /// </summary>
        /// <param name="sHtml">String containing HTML Data</param>
        /// <param name="sXPath">String containing XPath value to parse Html</param>
        /// <returns>Returns collection of InnerTML value on Successfull, otherwise null</returns>
        public static HtmlNodeCollection GetNodeHtmlCollection(string sHtml, string sXPath)
        {
            try
            {
                HtmlDocument hDoc = new HtmlDocument();
                hDoc.LoadHtml(sHtml);
                return hDoc.DocumentNode.SelectNodes(sXPath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
