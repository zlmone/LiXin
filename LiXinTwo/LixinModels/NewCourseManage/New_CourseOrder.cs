using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewCourseManage
{
    public class New_CourseOrder
    {
        #region Model

        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// CourseId
        /// </summary>		
        public int CourseId { get; set; }

        /// <summary>
        /// ClassId
        /// </summary>		
        public int ClassId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>		
        public int UserId { get; set; }

        /// <summary>
        /// SubCourseID
        /// </summary>		
        public int SubCourseID { get; set; }

        /// <summary>
        /// OrderTime
        /// </summary>		
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// LearnStatus
        /// </summary>		
        public int LearnStatus { get; set; }

        /// <summary>
        /// TogetherScore
        /// </summary>		
        public int TogetherScore { get; set; }

        /// <summary>
        /// GroupScore
        /// </summary>		
        public int GroupScore { get; set; }

        /// <summary>
        /// CourseExamScore
        /// </summary>		
        public decimal CourseExamScore { get; set; }

        /// <summary>
        /// ExamScore
        /// </summary>		
        public decimal ExamScore { get; set; }

        /// <summary>
        /// TogetherMemo
        /// </summary>		
        public string TogetherMemo { get; set; }

        /// <summary>
        /// GroupMemo
        /// </summary>		
        public string GroupMemo { get; set; }

        /// <summary>
        /// GroupSubCourseID
        /// </summary>		
        public int GroupSubCourseID { get; set; }

        /// <summary>
        /// GroupLearnStatus
        /// </summary>		
        public int GroupLearnStatus { get; set; }

        /// <summary>
        /// Type
        /// </summary>		
        public int Type { get; set; }

        /// <summary>
        /// 课程考试的总分
        /// </summary>
        public decimal CourseExamSumScore { get; set; }

        /// <summary>
        /// 独立考试的总分
        /// </summary>
        public decimal ExamSumScore { get; set; }

        #endregion

        #region 扩展

        public string teachername { get; set; }

        public string roomname { get; set; }

        public string CourseName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string CourseTimeStr
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public int IsGroupTeach { get; set; }

        public string IsGroupTeachStr
        {
            get
            {
                if (IsGroupTeach == 0)
                {
                    return "无";
                }
                if (IsGroupTeach == 1)
                {
                    return "集中授课";
                }
                if (IsGroupTeach == 2)
                {
                    return "分组带教";
                }
                if (IsGroupTeach == 3)
                {
                    return "既集中授课又分组带教";
                }

                return "";
            }
        }

        public string CourseStatus
        {
            get
            {
                if (DateTime.Now < StartTime)
                {
                    return "未开始";
                }
                if (DateTime.Now >= StartTime && DateTime.Now <= EndTime)
                {
                    return "进行中";
                }
                if (DateTime.Now > EndTime)
                {
                    return "已结束";
                }
                return "";
            }
        }

        public int Way { get; set; }

        public int totalcount { get; set; }

        public int Sex { get; set; }

        public string ClassName { get; set; }

        public string Realname { get; set; }

        public string NumberID { get; set; }

        public string MobileNum { get; set; }

        public string classroomAddress { get; set; }

        public int PersonCount { get; set; }

        public string SexStr
        {
            get
            {
                if (Sex == 0)
                    return "男";
                else if (Sex == 1)
                    return "女";
                else
                    return "保密";
            }
        }

        public string NumberIDStr
        {
            get
            {
                return string.IsNullOrEmpty(this.NumberID) ? "" : this.NumberID;
            }
        }


        public string LoginId { get; set; }


        /// <summary>
        /// 实际考勤
        /// </summary>
        public int HaveAttend { get; set; }


        /// <summary>
        /// 应考勤
        /// </summary>
        public int AttendCount { get; set; }

        /// <summary>
        /// 奖励总和
        /// </summary>
        public int IsReward { get; set; }

        public string RewardStr
        {
            get
            {
                return this.IsReward > 0 ? "课程已评估" : "课程未评估";
            }
        }

        /// <summary>
        /// 是否在测试
        /// </summary>
        public int IsTest { get; set; }


        public decimal TogetherGroupScore
        {
            get
            {
                return this.TogetherScore + this.GroupScore + this.CourseExamScore;
            }
           
        }

        public decimal sumScore { get; set; }

        /// <summary>
        /// 集中授课评分级别
        /// </summary>
        public string togetherScoreLevel
        {
            get
            {
                return ((Enums.ScoreLevel)this.TogetherScore).ToString();
            }
        }

        /// <summary>
        /// 集中授课评分级别
        /// </summary>
        public string groupScoreLevel
        {
            get
            {
                return ((Enums.ScoreLevel)this.GroupScore).ToString();
            }
        }

        public string WayStr
        {
            get
            {
                var str = "";
                switch (this.Way)
                {
                    case 1:
                        str = "混合式培训课程";
                        break;
                    case 2:
                        str = "视频课程";
                        break;
                }
                return str;
            }
        }
        #endregion


        #region 考试字段
        /// <summary>
        /// 课程关联试卷表主键ID
        /// </summary>
        public int CoPaperID { get; set; }
        /// <summary>
        /// 试卷ID
        /// </summary>
        public int PaperId { get; set; }

        public int LevelScore { get; set; }


        /// <summary>
        /// 考试通过分数线(试卷总分*试卷百分比)
        /// </summary>
        public int LevelScoreStr { get; set; }

        public int tbExamstudentId { get; set; }


        public int DoExamStatus { get; set; }

        /// <summary>
        /// 考试考了几次数
        /// </summary>
        public int ExamTestTimes { get; set; }


        /// <summary>
        /// 考试总次数
        /// </summary>
        public int TestTimes { get; set; }

        /// <summary>
        /// 考试取得的总分
        /// </summary>
        public decimal GetexamScore { get; set; }


        /// <summary>
        /// 学员编号
        /// </summary>
        public string UserNum { get; set; }
        #endregion
    }
}
