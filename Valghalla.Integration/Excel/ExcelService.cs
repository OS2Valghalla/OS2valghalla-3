using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Extensions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Valghalla.Application.Excel;

namespace Valghalla.Integration.Excel
{
    internal class ExcelService : IExcelService
    {
        public ExcelService()
        {
        }

        public MemoryStream GetExcelStream(ExcelModel excelModel, CancellationToken cancellationToken)
        {
            MemoryStream stream = new MemoryStream();
            byte[] template = Templates.Resource.Blank;
            stream.Write(template, 0, template.Length);

            using (SpreadsheetDocument ssd = SpreadsheetDocument.Open(stream, true))
            {
                WorksheetPart part = SpreadsheetReader.GetWorksheetPartByName(ssd, "Export");
                CreateHeaderRowForExcel(excelModel, part);
                GenerateSheetdataForDetails(excelModel, part);
            }

            return stream;
        }

        private void CreateHeaderRowForExcel(ExcelModel excelModel, WorksheetPart part)
        {
            for (uint colIndex = 0; colIndex < excelModel.Headers.Count; colIndex++)
            {
                CreateCell(colIndex, 1, part, excelModel.Headers[Convert.ToInt32(colIndex)].PropertyType, excelModel.Headers[Convert.ToInt32(colIndex)].HeaderName);
            }
        }


        private void GenerateSheetdataForDetails(ExcelModel excelModel, WorksheetPart part)
        {
            for (uint rowIndex = 1; rowIndex <= excelModel.Rows.Count; rowIndex++)
            {
                GenerateRowForChildPartDetail(rowIndex + 1, excelModel.Headers, excelModel.Rows[Convert.ToInt32(rowIndex) - 1], part);
            }
        }

        private void GenerateRowForChildPartDetail(uint rowindex, IList<ExcelHeader> headers, Dictionary<string, string> row, WorksheetPart part)
        {
            for (uint colIndex = 0; colIndex < headers.Count; colIndex++)
            {
                CreateCell(colIndex, rowindex, part, headers[Convert.ToInt32(colIndex)].PropertyType, row[headers[Convert.ToInt32(colIndex)].PropertyName]);
            }
        }

        private Cell CreateCell(uint columnIndex, uint rowIndex, WorksheetPart part, Type type, string text)
        {
            Cell cell = FindCell(columnIndex, rowIndex, part);
            cell.DataType = ResolveCellDataTypeOnValue(type);
            cell.CellValue = new CellValue(text);
            return cell;
        }

        private EnumValue<CellValues> ResolveCellDataTypeOnValue(Type type)
        {
            switch (type)
            {
                case Type intType when intType == typeof(int):
                case Type decimalType when decimalType == typeof(decimal):
                    return CellValues.Number;
                default:
                    return CellValues.String;
            }
        }

        public static string GetColumnNameFromIndex(uint columnIndex)
        {
            int dividend = (int)columnIndex + 1;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        public static Cell FindCell(uint columnIndex, uint rowIndex, WorksheetPart worksheetPart)
        {
            var columnName = GetColumnNameFromIndex(columnIndex);
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = (columnName + rowIndex.ToString());

            //If the worksheet does not contain a row with the specified row index, insert one.
            Row row = FindRow(sheetData, rowIndex);

            //If there is not a cell with the specified column name, insert one.  
            IEnumerable<Cell> cells = row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference);
            if ((cells.Count() > 0))
            {
                return cells.First();
            }
            else
            {
                //Check the numerical value of the column portion of the cell reference.
                //Because the cells are in order, we add the new cell directly before first cell that is greater
                Cell refCell = null;

                foreach (Cell cell in row.Elements<Cell>())
                {
                    int colId = SpreadsheetReader.GetColumnIndex(SpreadsheetReader.ColumnFromReference(cell.CellReference.Value));
                    if (colId > columnIndex)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell();
                newCell.CellReference = cellReference;

                row.InsertBefore(newCell, refCell);

                return newCell;
            }
        }
        public static Row FindRow(SheetData sheetData, uint rowIndex)
        {
            Row row = null;
            uint index = rowIndex;

            //Make sure the row exists
            var match = sheetData.Elements<Row>().Where(r => r.RowIndex.Value == index);

            if (match.Count() != 0)
            {
                row = match.First();
            }
            else
            {
                row = new Row();
                row.RowIndex = index;

                //Get the position in the array to insert the row and insert it there
                int count = 0;
                foreach (var rowLoop in sheetData.Elements<Row>())
                {
                    if (rowLoop.RowIndex.Value > rowIndex)
                    {
                        sheetData.InsertAt(row, count);
                        return row;
                    }
                    count += 1;
                }

                sheetData.Append(row);
            }

            return row;
        }
    }
}
