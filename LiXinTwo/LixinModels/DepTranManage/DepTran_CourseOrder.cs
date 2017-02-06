using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepTranManage
{
    ///<summary>
    ///DepTran_CourseOrder
    ///</summary>
    public partial class DepTran_CourseOrder
    {
        #region Model
        public int Id { get; set; }
        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// OrderTime
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// OrderStatus
        /// </summary>
        /// <summary>
        /// 0：退订；1：预定；2：补预定 
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// LearnStatus
        /// </summary>
        public int LearnStatus { get; set; }
        /// <summary>
        /// GetScore
        /// </summary>
        public decimal GetScore { get; set; }
        /// <summary>
        /// IsAppoint
        /// </summary>
        public int IsAppoint { get; set; }
        /// <summary>
        /// PassStatus
        /// </summary>
        public int PassStatus { get; set; }

        /// <summary>
        /// 课程退订次数
        /// </summary>
        public int OrderTimes { get;set;}
        /// <summary>
        /// AttScore
        /// </summary>
        /// <summary>
        /// 考勤得分
        /// </summary>		
        public decimal AttScore { get; set; }
        /// <summary>
        /// EvlutionScore
        /// </summary>
        public decimal EvlutionScore { get; set; }
        /// <summary>
        /// ExamScore
        /// </summary>
        public decimal ExamScore { get; set; }

        /// <summary>
        /// DepartSetId
        /// </summary>
        public int DepartSetId { get; set; }

        #endregion

        #region 扩展字段
        /// <summary>
        /// 姓名
        /// </summary>
        public string Realname { set; get; }

        public string JobNum { get; set; }


        public int PaperId { get; set; }

        public int TestTimes { get; set; }
        public int LevelScore { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { set; get; }
        
        /// <summary>
        /// 分所名称
        /// </summary>
        public string DepartSetName { set; get; }

        /// <summary>
        /// 考勤状态(1：正常 2:缺勤 3:迟到 4:早退 5:迟到且早退)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 审批标志(0：未审批；1：审批通过；2：审批拒绝)
        /// </summary>
        public int ApprovalFlag { get; set; }

        /// <summary>
        /// 考勤结束标志 0：未提交；1：已提交 
        /// </summary>
        public int AttFlag { get; set; }

        /// <summary>
        /// 是否开放标志  0：不开放；1：开放 
        /// </summary>
        public int OpenFlag { get; set; }

        /// <summary>
        /// 报名人数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 考勤人数
        /// </summary>
        public int AttCount { get; set; }

        public int Sex { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 课程结束时间
        /// </summary>
        public DateTime EndTime { set; get; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartID { set; get; }

        /// <summary>
        /// 课程学时分配
        /// </summary>
        public string CourseLengthDistribute { get; set; }

        /// <summary>
        /// 课程学时
        /// </summary>
        public decimal CourseLength { set; get; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobileNum { get; set; }

        #endregion

        /// <summary>
        /// 课程关联coursepaper主键ID
        /// </summary>
        public int CoPaperID { get; set; }
        /// <summary>
        /// 考试通过分数线(试卷总分*试卷百分比)
        /// </summary>
        public int LevelScoreStr { get; set; }

        public int tbExamstudentId { get; set; }

        public int DoExamStatus { get; set; }

        /// <summary>
        /// 考试次数
        /// </summary>
        public int ExamTestTimes { get; set; }

        /// <summary>
        /// 考试取得的总分
        /// </summary>
        public decimal GetexamScore { get; set; }

        /// <summary>
        /// 是否必修
        /// </summary>
        public string StatusStr
        {
            get
            {
                return ((Enums.deptAttStatus)this.Status).ToString();
            }
        }
        public int totalcount { get; set; }
    }
}
