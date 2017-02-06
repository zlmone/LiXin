using System;
using System.Collections.Generic;
using System.Data;

namespace LiXinModels
{

    #region 模型

    [Serializable]
    public class ChartModel
    {
        public ChartModel()
        {
            xAxis = new List<string>();
            series = new List<Series>();
            pieseries = new List<pieSeries>();
            reportFormat = 0;
            ymin = 0;
            PerName = "";
            PerY = "";
        }

        public string DivID { get; set; }
        //Column Line Pie Radar Bar BarStacked
        public Charttype charttype { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string yText { get; set; }
        public List<String> xAxis { get; set; }
        public List<Series> series { get; set; }
        public List<pieSeries> pieseries { get; set; }

        //线状图时候的y轴最小值
        public int ymin { get; set; }

        /// <summary>
        /// 报表的时候用的数据格式  为1  形如 总所,600,90%
        /// </summary>
        public int reportFormat { get; set; }

        /// <summary>
        /// X轴的前缀
        /// </summary>
        public string PerName { get; set; }

        /// <summary>
        /// Y轴的前缀
        /// </summary>
        public string PerY { get; set; }
    }

    public class Series
    {
        public Series()
        {
            data = new List<Double>();
        }

        public string name { get; set; }
        public List<Double> data { get; set; }
    }

    //饼状图数据
    public class pieSeries
    {
        public string name { get; set; }
        public Double y { get; set; }
    }

    public enum Charttype
    {
        Column,
        Line,
        Pie,
        Radar,
        Bar,
        BarStacked
    }
    #endregion
}