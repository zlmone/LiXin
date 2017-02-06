using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewQueryStatistics
{
    /// <summary>
    /// 严正申明，不要随便添加字段，我发现就删掉。。。要添加请大喊三声 神
    /// </summary>
    public class ShowScore
    {

        public Int64 number { get; set; }

        public int UserID { get; set; }

        public string NumberID { get; set; }

        public string Realname { get; set; }

        public string InternDeptStr { get; set; }


        public string DeptName { get; set; }

        /// <summary>
        /// 分组课程数
        /// </summary>
        public int groupCount { get; set; }

        /// <summary>
        /// 分组得分
        /// </summary>
        public int groupScore { get; set; }

        /// <summary>
        /// 集中课程数
        /// </summary>
        public int togetherCount { get; set; }

        /// <summary>
        /// 集中课程得分
        /// </summary>
        public int togetherScore { get; set; }




        /// <summary>
        /// 考试部分  课程数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 课程考试得分
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 课程考试总分
        /// </summary>
        public decimal sumScore { get; set; }

        /// <summary>
        /// 考试得分
        /// </summary>
        public decimal examScore { get; set; }

        /// <summary>
        /// 考试总分
        /// </summary>
        public decimal examSumScore { get; set; }

        /// <summary>
        /// 考勤得分
        /// </summary>
        public double SAttendScore { get; set; }

        /// <summary>
        /// 分组总和 （得分*数量）/(基准分*数量)
        /// </summary>
        public double SgroupSumScore { get; set; }
        /// <summary>
        /// 集中总和 （得分*数量）/(基准分*数量)
        /// </summary>
        public double StogetherSumScore { get; set; }

        /// <summary>
        /// 考试总和（得分/总分）*基准分
        /// </summary>
        public double SExamScore { get; set; }

        /// <summary>
        /// 奖励总和
        /// </summary>
        public double SRewardScore { get; set; }


        /// <summary>
        /// 综合评分
        /// </summary>
        public double SSumScore { get; set; }


        //迟到
        public int Chidao { get; set; }

        //Zaotui
        public int Zaotui { get; set; }

        //迟到并早退
        public int ChidaoZaotui { get; set; }

        //缺勤次数
        public int queqin { get; set; }

        /// <summary>
        /// 为0的时候 考勤未上传
        /// </summary>
        public int AttCount { get; set; }


        public string AttendStr
        {
            get
            {
                var str = "";

                if (Chidao > 0)
                {
                    str += ",迟到" + Chidao + "次";
                }
                if (Zaotui > 0)
                {
                    str += ",早退" + Zaotui + "次";
                }
                if (ChidaoZaotui > 0)
                {
                    str += ",迟到并早退" + ChidaoZaotui + "次";
                }
                if (queqin > 0)
                {
                    str += ",缺勤" + queqin + "次";
                }
                if (AttCount > 0)
                {
                    str += ",考勤未上传" + AttCount + "次";
                }

                return str == "" ? "考勤正常" : str.TrimStart(',');
            }
        }

        /// <summary>
        /// 混合培训课程考试（百分比换算之后）
        /// </summary>
        public double cSumScore { get; set; }

        /// <summary>
        /// 视频课程（百分比换算之后）
        /// </summary>
        public double vSumScore { get; set; }

        /// <summary>
        /// 独立考试成绩（百分比换算之后）
        /// </summary>
        public double eSumScore { get; set; }



    }
}
