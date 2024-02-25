using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace Nautilus
{
    public class CreateExcelFile
    {
        public System.Drawing.Color AltColor ;
        public bool doStyle;
        public bool doHeader;
        public IList<double> colWidths;
        public UInt32 FontIndex = 0;
        public bool TruncateLongStrings = true;

        /// <summary>
        /// Create an Excel file, and write it to a file.
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="excelFilename">Name of file to be written.</param>
        /// <returns>True if successful, false if something went wrong.</returns>
        public bool CreateExcelDocument(DataSet ds, string excelFilename)
        {
            try
            {
                using (var document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(ds, document);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        private void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet)
        {
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            var workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            workbookStylesPart.Stylesheet = GenerateStyleSheet(); //stylesheet for nice formatting;
            workbookStylesPart.Stylesheet.Save();

            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            foreach (DataTable dt in ds.Tables)
            {
                var newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet();

                //let's specify the column width
                var columns = new Columns();
                for (var colInx = 0; colInx < dt.Columns.Count; colInx++)
                {
                    double col_width;
                    try
                    {
                        col_width = colWidths[colInx]; //if done properly, this will have a specified value
                    }
                    catch (Exception)
                    {
                        col_width = 20; //in case of derp moment by programmer, don't crash, give it generic value
                    }
                    var column = CustomColumnWidth(colInx, col_width);
                    columns.Append(column);
                }
                newWorksheetPart.Worksheet.Append(columns);

                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new SheetData());

                // save worksheet
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                newWorksheetPart.Worksheet.Save();

                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet
                    {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = worksheetNumber,
                    Name = dt.TableName
                });
                worksheetNumber++;
            }
            spreadsheet.WorkbookPart.Workbook.Save();
        }
        
        private void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            var style = 1; //no fill with borders, centered
            const int style_header = 2; //header fill with borders, centered
            const int no_style = 4; //blank fill, no borders, centered

            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            var numberOfColumns = dt.Columns.Count;
            var IsNumericColumn = new bool[numberOfColumns];

            var excelColumnNames = new string[numberOfColumns];
            for (var n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            
            //  Create the Header row in our Excel Worksheet
            uint rowIndex = 1;

            var headerRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(headerRow);

            for (var colInx = 0; colInx < numberOfColumns; colInx++)
            {
                var col = dt.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow, doHeader && doStyle ? style_header : no_style);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
            }
            
            //keep track of artist for color grouping
            var artist = "";
            
            //  Now, step through each row of data in our DataTable...
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                //choose row color based on artist name
                var new_art = dr.ItemArray[0].ToString();
                if (new_art != artist)
                {
                    //3 - item fill with borders, centered
                    //1 - no fill with borders, centered
                    style = style == 3 ? 1 : 3;
                }

                for (var colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    //let's trim the values to fit within the cells for a nice formatting
                    var cellValue = dr.ItemArray[colInx].ToString();
                    var limit = 30; //artist, song, album, and all others will always be shorter
                    
                    switch (colInx)
                    {
                        case 9:
                            limit = 15; //genre needs a shorter string
                            break;
                        case 34:
                            limit = 100; //file path needs a longer string
                            break;
                    }
                    if (cellValue.Length > limit && TruncateLongStrings)
                    {
                        cellValue = cellValue.Substring(0, limit - 2) + "...";
                    }

                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        double cellNumericValue;
                        if (!double.TryParse(cellValue, out cellNumericValue)) continue;
                        cellValue = cellNumericValue.ToString(CultureInfo.InvariantCulture);
                        AppendNumericCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow, doStyle? style : no_style);
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex, cellValue, newExcelRow, doStyle? style : no_style);
                    }
                }
                artist = new_art;
            }
        }

        private static Column CustomColumnWidth(int columnIndex, double columnWidth)
        {
            // This creates a Column variable for a zero-based column-index (eg 0 = Excel Column A), with a particular column width.
            var column = new Column
                {
                    Min = (UInt32) columnIndex + 1,
                    Max = (UInt32) columnIndex + 1,
                    Width = columnWidth,
                    CustomWidth = true
                };
            return column;
        } 

        private static void AppendTextCell(string cellReference, string cellStringValue, OpenXmlElement excelRow, int style)
        {
            //  Add a new Excel Cell to our Row 
            var cell = new Cell { CellReference = cellReference, DataType = CellValues.String, StyleIndex = (UInt32)style };
            var cellValue = new CellValue {Text = cellStringValue};
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, OpenXmlElement excelRow, int style)
        {
            //  Add a new Excel Cell to our Row 
            var cell = new Cell { CellReference = cellReference, StyleIndex = (UInt32)style };
            var cellValue = new CellValue {Text = cellStringValue};
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //  eg  GetExcelColumnName(0) should return "A"
            
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString(CultureInfo.InvariantCulture);

            var firstChar = (char)('A' + (columnIndex / 26) - 1);
            var secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }

        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                        new Font(                                 // Index 0 - Arial Normal
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Arial" }), 
                        
                        new Font(                                 // Index 1 - Calibri Normal
                        new FontSize {Val = 11},
                        new Color {Rgb = new HexBinaryValue {Value = "000000"}},
                        new FontName {Val = "Calibri"}),

                        new Font(                                 // Index 2 - Tahoma Normal
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Tahoma" }),

                        new Font(                                 // Index 3 - Times New Roman Normal
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Times New Roman" }),

                        new Font(                                 // Index 4 - Arial Bold
                        new Bold(),
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Arial" }),

                        new Font(                                 // Index 5 - Calibri Bold
                        new Bold(),
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Calibri" }),

                        new Font(                                 // Index 6 - Tahoma Bold
                        new Bold(),
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Tahoma" }),

                        new Font(                                 // Index 7 - Times New Roman Bold
                        new Bold(),
                        new FontSize { Val = 11 },
                        new Color { Rgb = new HexBinaryValue { Value = "000000" } },
                        new FontName { Val = "Times New Roman" })),

                new Fills(
                        new Fill(                                       // Index 0 - The default fill (none)
                        new PatternFill {PatternType = PatternValues.None}),
                        
                        new Fill(                                       // Index 1 - The default fill of gray 125 (required)
                        new PatternFill {PatternType = PatternValues.Gray125}),
                        
                        new Fill(                                       // Index 2 - Header color
                        new PatternFill(
                            new ForegroundColor {Rgb = new HexBinaryValue {Value = "d3d3d3"}}
                            ) {PatternType = PatternValues.Solid}),
                        
                            new Fill(                                       // Index 3 - Items color
                        new PatternFill(
                            new ForegroundColor { Rgb = new HexBinaryValue(AltColor.R.ToString("X2") + AltColor.G.ToString("X2") + AltColor.B.ToString("X2")) }
                            ) {PatternType = PatternValues.Solid})),

                new Borders(
                        new Border(                                     // Index 0 - No borders
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                        new Border(                                     // Index 1 - Side borders only
                        new LeftBorder(
                            new Color {Auto = true}
                            ) {Style = BorderStyleValues.Thin},
                        new RightBorder(
                            new Color {Auto = true}
                            ) {Style = BorderStyleValues.Thin},
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    
                        new Border(                                     // Index 2 - All borders
                        new LeftBorder(
                            new Color {Auto = true}
                            ) {Style = BorderStyleValues.Thin},
                        new RightBorder(
                            new Color {Auto = true}
                            ) {Style = BorderStyleValues.Thin},
                        new TopBorder(
                            new Color {Auto = true}
                            ) {Style = BorderStyleValues.Thin},
                        new BottomBorder(
                            new Color {Auto = true}
                            ) {Style = BorderStyleValues.Thin},
                            new DiagonalBorder())),

                new CellFormats(

                        new CellFormat { FontId = FontIndex, FillId = 0, BorderId = 0, ApplyAlignment = true },   // Index 0 - default
                        
                        new CellFormat(                                 // Index 1 - No fill - side borders
                        new Alignment
                            {
                                Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center
                            }
                        ) { FontId = FontIndex, FillId = 0, BorderId = 1, ApplyAlignment = true },

                        new CellFormat(                                 // Index 2 - Header fill, all borders
                        new Alignment
                            {
                                Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center
                            }
                        ) { FontId = (FontIndex + 4), FillId = 2, BorderId = 2, ApplyAlignment = true },
            
                        new CellFormat(                                 // Index 3 - Items fill, side borders
                        new Alignment { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                        ) { FontId = FontIndex, FillId = 3, BorderId = 1, ApplyBorder = true },
                        
                        new CellFormat(                                 // Index 4 - No fill - no borders
                        new Alignment
                            {
                                Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center
                            }
                        ) { FontId = FontIndex, FillId = 0, BorderId = 0, ApplyAlignment = true })); 
        }
    }
}
