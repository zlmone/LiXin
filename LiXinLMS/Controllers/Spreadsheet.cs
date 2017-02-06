using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinModels;
using SpreadsheetGear;
using SpreadsheetGear.Charts;
using SpreadsheetGear.Data;
using SpreadsheetGear.Shapes;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace LiXinControllers
{
    public class Spreadsheet : Controller
    {
        #region 接口
        /// <summary>
        ///  导出Excel（新）
        /// </summary>
        /// <param name="columns">图形的数据集</param>
        /// <param name="allitem">表的数据集</param>
        /// <param name="response"></param>
        /// <param name="excelName">文件名称</param>
        /// <param name="sheetname">工作表名称</param>
        /// <param name="sheetname">开启多个sheet</param>
        public void ExportChart(List<ChartModel> columns, List<DataTable> allitem, HttpContextBase context,
                                string excelName = "report", string sheetname = "Sheet", bool OpenSheet = false)
        {
            Downfile(DrawChart(columns, allitem, sheetname, OpenSheet), context, excelName);
        }

        /// <summary>
        ///  导出Excel（扩展）
        /// </summary>
        /// <param name="sheets">sheet数据集合</param>
        /// <param name="context"></param>
        /// <param name="excelName">文件名称</param>
        public void ExportExcel(List<SheetModel> sheets, HttpContextBase context, string excelName = "report")
        {
            Downfile(BuildSheet(sheets), context, excelName);
        }

        /// <summary>
        /// 导出Excel模板
        /// </summary>
        /// <param name="title">第一行标题</param>
        /// <param name="subtitle">第二行标题</param>
        /// <param name="allitem">第三行数据源</param>
        /// <param name="response"></param>
        /// <param name="excelName">文件名称</param>
        /// <param name="sheetname">工作表名称</param>
        public void Template(string title, string subtitle, DataTable allitem, HttpContextBase context,
                             string excelName = "report", string sheetname = "Sheet")
        {
            Downfile(ExportExcel_One(title, subtitle, allitem, sheetname), context, excelName);
        }

        /// <summary>
        /// 读取Execl
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="sum">sheet数量</param>
        /// <returns></returns>
        public List<DataTable> LoadExcel(string path, int sum)
        {
            Stream fs = System.IO.File.OpenRead(path);
            List<DataTable> tableList = new List<DataTable>();
            NPOI.SS.UserModel.IWorkbook wb = WorkbookFactory.Create(fs);
            for (int i = 0; i < sum; i++)
            {
                NPOI.SS.UserModel.ISheet sheet = wb.GetSheetAt(i);
                DataTable dt = RenderFromExcel(sheet);
                tableList.Add(dt);
            }
            return tableList;
        }

        #endregion

        #region 实现方法

        public SpreadsheetGear.IWorkbook DrawChart(List<ChartModel> columns, List<DataTable> allitem, string sheetname,bool OpenSheet=false)
        {
            //创建新的workbook
            SpreadsheetGear.IWorkbook workbook = Factory.GetWorkbook();
            SpreadsheetGear.IWorksheets worksheets = workbook.Worksheets;
            IWorksheet worksheet = worksheets[0];
            worksheet.Name = sheetname+1;
            IWorksheetWindowInfo windowInfo = worksheet.WindowInfo;
            IShapes shapes = worksheet.Shapes;

            int high = 1;
            int Rowhigh = 17;
            double top = 0.5;
            double bottom = 15.5;

            for (int a = 0; a < columns.Count; a++)
            {
                int row = 0;
                int column = 0;
                int piecol = 0;

                string site = null;


                if (columns[a].series.Count > 0)
                {
                    row = columns[a].xAxis.Count;
                    column = columns[a].series.Count;
                }
                if (columns[a].pieseries.Count > 0)
                {
                    piecol = columns[a].pieseries.Count;
                }
                //+((17 + row) * j);

                string[,] arrayChart = null;
                if ((columns[a].charttype.ToString()) == "Pie")
                {
                    site = "A" + Rowhigh + ":B" + (piecol + Rowhigh);
                    arrayChart = new string[piecol + 1,2];
                    for (int j = 0; j < (piecol + 1); j++)
                    {
                        if (j == 0)
                        {
                            if (!string.IsNullOrEmpty(columns[a].title))
                            {
                                arrayChart[0, 0] = "";
                                arrayChart[0, 1] = columns[a].title;
                            }
                            else
                            {
                                arrayChart[0, 0] = "";
                                arrayChart[0, 1] = "";
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (i == 0)
                                {
                                    arrayChart[j, i] = columns[a].pieseries[j - 1].name;
                                }
                                else
                                {
                                    arrayChart[j, i] = columns[a].pieseries[j - 1].y.ToString();
                                }
                            }
                        }
                    }
                }
                else if ((columns[a].charttype.ToString()) == "Radar")
                {
                    if (row < 3)
                    {
                        row = 3;
                    }
                    string aaa = column / 26 == 0 ? ((char)((65 + column))).ToString() : "" + (char)((64 + column / 26)) + (char)((65 + column % 26));
                    site = "A" + Rowhigh + ":" + aaa + (row + Rowhigh);
                    if (column>25)
                    arrayChart = new string[(row + 1),(column + 1)];
                    for (int j = 0; j < row + 1; j++)
                    {
                        if (j == 0)
                        {
                            for (int i = 0; i < column + 1; i++)
                            {
                                if (i == 0)
                                {
                                    arrayChart[j, i] = "";
                                }
                                else
                                {
                                    arrayChart[j, i] = columns[a].series[i - 1].name;
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < column + 1; k++)
                            {
                                if (k == 0)
                                {
                                    if (j > columns[a].xAxis.Count)
                                    {
                                        arrayChart[j, k] = "";
                                    }
                                    else
                                    {
                                        arrayChart[j, k] = columns[a].xAxis[j - 1];
                                    }
                                }
                                else
                                {
                                    if (j > columns[a].xAxis.Count)
                                    {
                                        arrayChart[j, k] = "";
                                    }
                                    else
                                    {
                                        List<double> adata = columns[a].series[k - 1].data;
                                        arrayChart[j, k] = adata[j - 1].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    string aaa = column / 26 == 0 ? ((char)((65 + column))).ToString() : "" + (char)((64 + column / 26)) + (char)((65 + column % 26));
                    site = "A" + Rowhigh + ":" + aaa + (row + Rowhigh);
                    arrayChart = new string[(row + 1),(column + 1)];
                    for (int j = 0; j < row + 1; j++)
                    {
                        if (j == 0)
                        {
                            for (int i = 0; i < column + 1; i++)
                            {
                                if (i == 0)
                                {
                                    arrayChart[j, i] = "";
                                }
                                else
                                {
                                    arrayChart[j, i] = columns[a].series[i - 1].name;
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < column + 1; k++)
                            {
                                if (k == 0)
                                {
                                    arrayChart[j, k] = columns[a].xAxis[j - 1];
                                }
                                else
                                {
                                    List<double> adata = columns[a].series[k - 1].data;
                                    arrayChart[j, k] = adata[j - 1].ToString();
                                }
                            }
                        }
                    }
                }

                double chartleft = windowInfo.ColumnToPoints(0.15);
                double charttop = windowInfo.RowToPoints(top);
                double chartright = windowInfo.ColumnToPoints(7.85);
                double chartbottom = windowInfo.RowToPoints(bottom);

                SpreadsheetGear.Charts.IChart chart =
                    shapes.AddChart(chartleft, charttop, chartright - chartleft, chartbottom - charttop).Chart;
                IRange source = worksheet.Cells[site];
                source.Value = arrayChart;
                chart.SetSourceData(source, RowCol.Columns);

                if ((columns[a].charttype.ToString()) == "Column")
                {
                    chart.ChartType = ChartType.ColumnClustered;
                }
                else if ((columns[a].charttype.ToString()) == "Line")
                {
                    chart.ChartType = ChartType.Line;
                }
                else if ((columns[a].charttype.ToString()) == "Pie")
                {
                    chart.ChartType = ChartType.Pie;
                    ISeries seriesTotal = chart.SeriesCollection[0];

                    seriesTotal.HasDataLabels = true;
                    seriesTotal.DataLabels.ShowPercentage = true;
                    seriesTotal.DataLabels.ShowValue = false;
                }
                else if ((columns[a].charttype.ToString()) == "Radar")
                {
                    chart.ChartType = ChartType.Radar;
                }
                else if ((columns[a].charttype.ToString()) == "Bar")
                {
                    chart.ChartType = ChartType.BarClustered;
                }
                else if ((columns[a].charttype.ToString()) == "BarStacked")
                {
                    chart.ChartType = ChartType.BarStacked;
                }

                if (!string.IsNullOrEmpty(columns[a].title))
                {
                    chart.HasTitle = true;
                    chart.ChartTitle.Text = columns[a].title;
                    chart.ChartTitle.Font.Size = 12;
                }

                if ((columns[a].charttype.ToString()) == "Pie")
                {
                    Rowhigh += 17 + piecol;
                    top += 17 + piecol;
                    bottom += 17 + piecol;
                    high += 17 + piecol;
                }
                else
                {
                    Rowhigh += 17 + row;
                    top += 17 + row;
                    bottom += 17 + row;
                    high += 17 + row;
                }
            }
            //数据列表处理
            if (allitem.Count > 0)
            {
                if (OpenSheet)
                {
                    for (int c = 0; c < allitem.Count; c++)
                    {

                        if (columns.Count > 0)
                        {
                            worksheets.Add();
                            worksheet = worksheets[worksheets.Count - 1];
                            worksheet.Name = sheetname + worksheets.Count;
                        }
                        else
                        {
                            if (c > 0)
                            {
                                worksheets.Add();
                                worksheet = workbook.Worksheets[worksheets.Count - 1];
                                worksheet.Name = sheetname + worksheets.Count;
                            }
                        }
                        string allsite = "A1";
                        IRange cell = worksheet.Cells[allsite];
                        cell.CopyFromDataTable(allitem[c], SetDataFlags.None);
                        worksheet.UsedRange.Columns.AutoFit();
                        //high += allitem[c].Rows.Count;
                    }
                }
                else
                {
                    if (columns.Count > 0)
                    {
                        high += 2;
                    }
                    for (int b = 0; b < allitem.Count; b++)
                    {
                        if (b > 0)
                        {
                            high += 1;
                        }
                        string allsite = "A" + high;
                        IRange cell = worksheet.Cells[allsite];
                        cell.CopyFromDataTable(allitem[b], SetDataFlags.None);
                        worksheet.UsedRange.Columns.AutoFit();
                        high += allitem[b].Rows.Count;
                    }
                }
            }
            return workbook;
        }

        public SpreadsheetGear.IWorkbook ExportExcel_One(string title, string subtitle, DataTable allitem, string sheetname)
        {
            //创建新的workbook
            SpreadsheetGear.IWorkbook workbook = Factory.GetWorkbook();
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = sheetname;
            IWorksheetWindowInfo windowInfo = worksheet.WindowInfo;
            IShapes shapes = worksheet.Shapes;

            while (shapes.Count != 0)
                shapes[0].Delete();
            int itemcou = allitem.Columns.Count;
            string site = "A1:" + (char)((64 + itemcou)) + "1";
            string site1 = null;
            if (!string.IsNullOrEmpty(subtitle))
            {
                site1 = "A2:" + (char)((64 + itemcou)) + "2";
            }
            else
            {
                site1 = "A1:" + (char)((64 + itemcou)) + "1";
            }
            //数据处理
            if (!string.IsNullOrEmpty(title))
            {
                IRange cell = worksheet.Cells[site];
                cell.Merge();
                cell.HorizontalAlignment = HAlign.Center;
                cell.VerticalAlignment = VAlign.Center;
                cell.Value = title;
            }

            if (!string.IsNullOrEmpty(subtitle))
            {
                IRange cell1 = worksheet.Cells[site1];
                cell1.Merge();
                cell1.HorizontalAlignment = HAlign.Center;
                cell1.VerticalAlignment = VAlign.Center;
                cell1.Value = subtitle;
            }
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(subtitle))
            {
                IRange cell2 = worksheet.Cells["A3"];
                cell2.CopyFromDataTable(allitem, SetDataFlags.None);
            }
            else if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(subtitle))
            {
                IRange cell2 = worksheet.Cells["A2"];
                cell2.CopyFromDataTable(allitem, SetDataFlags.None);
            }
            else
            {
                IRange cell2 = worksheet.Cells["A1"];
                cell2.CopyFromDataTable(allitem, SetDataFlags.None);
            }
            worksheet.UsedRange.Columns.AutoFit();

            return workbook;
        }

        public SpreadsheetGear.IWorkbook BuildSheet(List<SheetModel> sheets)
        {
            //创建新的workbook
            SpreadsheetGear.IWorkbook workbook = Factory.GetWorkbook();
            SpreadsheetGear.IWorksheets worksheets = workbook.Worksheets;
            for (int i = 0; i < sheets.Count; i++)
            {
                int high = 1;
                if (i > 0)
                {
                    worksheets.Add();
                }
                IWorksheet worksheet = worksheets[i];
                worksheet.Name = sheets[i].SheetName == null ? "sheet" + (i + 1) : sheets[i].SheetName;
                IWorksheetWindowInfo windowInfo = worksheet.WindowInfo;
                IShapes shapes = worksheet.Shapes;
                List<DataModel> dataList = sheets[i].DataModels;
                for (int j = 0; j < dataList.Count; j++)
                {
                    if (dataList[j].datatype.ToString() != "DataTable")
                    {
                        high += 16;
                        string allsite = "A" + high;
                        IRange cell = worksheet.Cells[allsite];
                        cell.CopyFromDataTable(dataList[j].Dataseries, SetDataFlags.None);
                        worksheet.UsedRange.Columns.AutoFit();
                        
                        double top =high-16.5;
                        double bottom = high-1.5;
                        double chartleft = windowInfo.ColumnToPoints(0.15);
                        double charttop = windowInfo.RowToPoints(top);
                        double chartright = windowInfo.ColumnToPoints(7.85);
                        double chartbottom = windowInfo.RowToPoints(bottom);

                        SpreadsheetGear.Charts.IChart chart =shapes.AddChart(chartleft, charttop, chartright - chartleft, chartbottom - charttop).Chart;
                        string site = ((char)(65 + dataList[j].StartPoint.x)).ToString() + (high + dataList[j].StartPoint.y) + ":" + ((char)(64 + dataList[j].ColumnCount + dataList[j].StartPoint.x)).ToString() + (high + dataList[j].StartPoint.y + dataList[j].RowCount);
                        IRange source = worksheet.Cells[site];
                        chart.SetSourceData(source, RowCol.Columns);
                        switch (dataList[j].datatype.ToString())
                        {
                            case "Column":
                                chart.ChartType = ChartType.ColumnClustered;
                                break;
                            case "Line":
                                chart.ChartType = ChartType.Line;
                                break;
                            case "Radar":
                                chart.ChartType = ChartType.Radar;
                                break;
                            case "Bar":
                                chart.ChartType = ChartType.BarClustered;
                                break;
                            case "BarStacked":
                                chart.ChartType = ChartType.BarStacked;
                                break;
                            case "Pie":
                                chart.ChartType = ChartType.Pie;
                                ISeries seriesTotal = chart.SeriesCollection[0];
                                seriesTotal.HasDataLabels = true;
                                seriesTotal.DataLabels.ShowPercentage = true;
                                seriesTotal.DataLabels.ShowValue = false;
                                break;
                        }
                        high += dataList[j].Dataseries.Rows.Count+1 + sheets[i].space;
                    }
                    else
                    {
                        string allsite = "A" + high;
                        IRange cell = worksheet.Cells[allsite];
                        cell.CopyFromDataTable(dataList[j].Dataseries, SetDataFlags.None);
                        worksheet.UsedRange.Columns.AutoFit();
                        high += dataList[j].Dataseries.Rows.Count+1 + sheets[i].space;
                    }
                }
            }
            return workbook;
        }

        public void Downfile(SpreadsheetGear.IWorkbook workbook, HttpContextBase context, string excelName = "report")
        {
            context.Response.Clear();
            context.Response.ContentEncoding = Encoding.UTF8;
            if (context.Request.UserAgent.ToUpper().IndexOf("MSIE") > -1)
            {
                excelName = HttpUtility.UrlEncode(excelName, Encoding.UTF8);
            }
            //context.Response.ContentType = "application/vnd.ms-excel";
            //context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", excelName));
            //workbook.SaveToStream(context.Response.OutputStream, FileFormat.XLS97);
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", excelName));
            workbook.SaveToStream(context.Response.OutputStream, SpreadsheetGear.FileFormat.OpenXMLWorkbook);
            context.Response.End();
        }

        public DataTable RenderFromExcel(NPOI.SS.UserModel.ISheet sheet)
        {
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            dataRow[j] = row.GetCell(j);
                        }
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }
        #endregion

        /// <summary>
        /// 图model转DataModel
        /// </summary>
        /// <param name="chartitem"></param>
        /// <returns></returns>
        public DataModel ChangChartModelToDataModel(ChartModel chartitem)
        {
            DataModel datamodel = new DataModel();

            switch (chartitem.charttype)
            {
                case Charttype.Bar:
                    datamodel.datatype = Datatype.Bar;
                    break;
                case Charttype.BarStacked:
                    datamodel.datatype = Datatype.BarStacked;
                    break;
                case Charttype.Column:
                    datamodel.datatype = Datatype.Column;
                    break;
                case Charttype.Line:
                    datamodel.datatype = Datatype.Line;
                    break;
                case Charttype.Pie:
                    datamodel.datatype = Datatype.Pie;
                    break;
                case Charttype.Radar:
                    datamodel.datatype = Datatype.Radar;
                    break;
            }
            DataTable Data = new DataTable();
            #region 转换
            int row = 0;
            int column = 0;
            int piecol = 0;

            if (chartitem.series.Count > 0)
            {
                row = chartitem.xAxis.Count;
                column = chartitem.series.Count;
            }
            if (chartitem.pieseries.Count > 0)
            {
                piecol = chartitem.pieseries.Count;
            }

            if ((chartitem.charttype.ToString()) == "Pie")
            {
                for (int j = 0; j < (piecol + 1); j++)
                {
                    if (j == 0)
                    {
                        if (!string.IsNullOrEmpty(chartitem.title))
                        {
                            Data.Columns.Add(" ", typeof(string));
                            Data.Columns.Add(chartitem.title, typeof(string));
                        }
                        else
                        {
                            Data.Columns.Add(" ", typeof(string));
                            Data.Columns.Add(" ", typeof(string));
                        }
                    }
                    else
                    {
                        Data.Rows.Add(chartitem.pieseries[j - 1].name, chartitem.pieseries[j - 1].y);
                    }
                }
            }
            else if ((chartitem.charttype.ToString()) == "Radar")
            {
                if (row < 3)
                {
                    row = 3;
                }
                for (int j = 0; j < row + 1; j++)
                {
                    if (j == 0)
                    {
                        for (int i = 0; i < column + 1; i++)
                        {
                            if (i == 0)
                            {
                                Data.Columns.Add(" ", typeof(string));
                            }
                            else
                            {
                                Data.Columns.Add(chartitem.series[i - 1].name, typeof(string));
                            }
                        }
                    }
                    else
                    {
                        string[] arrayChart = new string[column + 1];
                        for (int k = 0; k < column + 1; k++)
                        {
                            if (k == 0)
                            {
                                if (j > chartitem.xAxis.Count)
                                {
                                    arrayChart[k] = "";
                                }
                                else
                                {
                                    arrayChart[k] = chartitem.xAxis[j - 1].ToString();
                                }
                            }
                            else
                            {
                                if (j > chartitem.xAxis.Count)
                                {
                                    arrayChart[k] = "";
                                }
                                else
                                {
                                    var adata = chartitem.series[k - 1].data;
                                    arrayChart[k] = adata[j - 1].ToString();
                                }
                            }
                        }
                        Data.Rows.Add(arrayChart);
                    }
                }
            }
            else
            {
                for (int j = 0; j < row + 1; j++)
                {
                    if (j == 0)
                    {
                        for (int i = 0; i < column + 1; i++)
                        {
                            if (i == 0)
                            {
                                Data.Columns.Add(" ", typeof(string));
                            }
                            else
                            {
                                Data.Columns.Add(chartitem.series[i - 1].name, typeof(string));
                            }
                        }
                    }
                    else
                    {
                        string[] arrayChart = new string[column + 1];
                        for (int k = 0; k < column + 1; k++)
                        {
                            if (k == 0)
                            {
                                arrayChart[k] = chartitem.xAxis[j - 1].ToString();
                            }
                            else
                            {
                                var adata = chartitem.series[k - 1].data;
                                arrayChart[k] = adata[j - 1].ToString();
                            }
                        }
                        Data.Rows.Add(arrayChart);
                    }
                }
            }
            #endregion
            datamodel.Dataseries = Data;
            datamodel.ColumnCount = Data.Columns.Count;
            datamodel.RowCount = Data.Rows.Count;

            return datamodel;
        }
    }
}