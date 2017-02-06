using System;
using System.Collections.Generic;
using System.Data;

namespace LiXinModels
{

    #region 模型

    [Serializable]
    public class SheetModel
    {
        public SheetModel()
        {
            DataModels = new List<DataModel>();
            space = 0;
        }

        public string SheetName { get; set; }
        /// <summary>
        /// 间隔行数
        /// </summary>
        public int space { get; set; }
        public List<DataModel> DataModels { get; set; }
    }

    public class DataModel
    {
        public DataModel()
        {
            StartPoint = new Point() { x =0, y = 0};
            ColumnCount = 0;
            RowCount = 0;
        }

        public Datatype datatype { get; set; }
        public Point StartPoint { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get; set; }
        public DataTable Dataseries { get; set; }
    }

    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public enum Datatype
    {
        /// <summary>
        /// 竖向柱状图
        /// </summary>
        Column,
        /// <summary>
        /// 线性
        /// </summary>
        Line,
        /// <summary>
        /// 饼状
        /// </summary>
        Pie,
        /// <summary>
        /// 雷达图
        /// </summary>
        Radar,
        /// <summary>
        /// 横向柱状图
        /// </summary>
        Bar,
        /// <summary>
        /// 堆栈型柱状图
        /// </summary>
        BarStacked,
        /// <summary>
        /// 纯表格，不是图
        /// </summary>
        DataTable
    }

    #endregion
}