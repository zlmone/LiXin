using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Sys_TrainGrade
    {
        #region model

        /// <summary>
        /// ID
        /// </summary>		
        public int ID
        {
            get;
            set;
        }
        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID
        {
            get;
            set;
        }
        /// <summary>
        /// CreateUserID
        /// </summary>		
        public int CreateUserID
        {
            get;
            set;
        }
        /// <summary>
        /// OldGrade
        /// </summary>		
        public string OldGrade
        {
            get;
            set;
        }
        /// <summary>
        /// NewGrade
        /// </summary>		
        public string NewGrade
        {
            get;
            set;
        }
        /// <summary>
        /// Reason
        /// </summary>		
        public string Reason
        {
            get;
            set;
        }
        /// <summary>
        /// UpdateTime
        /// </summary>		
        public DateTime UpdateTime
        {       
            get;
            set;
        }
        /// <summary>
        /// 0未变更  1已变更
        /// </summary>		
        public int Status
        {
            get;
            set;
        }

        public int SubmitFlag
        {
            get;
            set;
        }

        public int TrainMaster
        {
            get;
            set;
        }

        public string DeptPath { set; get; }
        #endregion

        #region 扩展

        /// <summary>
        /// 职工id
        /// </summary>		
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 人员编号
        /// </summary>		
        public string JobNum
        {
            get;
            set;
        }

        /// <summary>
        /// 姓名
        /// </summary>		
        public string Realname
        {
            get;
            set;
        }

        /// <summary>
        /// 性别
        /// </summary>		
        public int Sex
        {
            get;
            set;
        }

        /// <summary>
        /// 部门名称
        /// </summary>		
        public string DeptName
        {
            get;
            set;
        }

        /// <summary>
        /// 薪酬级别
        /// </summary>		
        public string PayGrade
        {
            get;
            set;
        }

        /// <summary>
        /// 培训级别
        /// </summary>		
        public string TrainGrade
        {
            get;
            set;
        }


        /// <summary>
        /// 是否进行级别维护
        /// </summary>
        public bool IsUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// 总数
        /// </summary>
        public int totalCount
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已经进行级别维护
        /// </summary>
        public int IsTrainGrade
        {
            get;
            set;
        }

        /// <summary>
        /// 培训级别list
        /// </summary>
        public List<string> TraindGradeList
        {
            get;
            set;
        }

        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateName
        {
            get;
            set;
        }

        public string SexStr
        {
            get
            {
                if (Sex == 0)
                    return "男士";
                else if (Sex == 1)
                    return "女士";
                else
                    return "保密";
            }
        }

        public string StatusStr
        {
            get
            {
                if (this.Status == 0)
                    return "未变更";
                else
                    return "已变更";
            }
        }

        /// <summary>
        /// 是否能再次申请
        /// </summary>
        public int IsApply
        {
            get;
            set;
        }

        public string UpdateTimeStr
        {
            get
            {
                return this.UpdateTime.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 维护的次数
        /// </summary>
        public int UpdateCount
        {
            get;
            set;
        }

        /// <summary>
        /// 申请的状态
        /// </summary>
        public int ApprovalStatus
        {
            get;
            set;
        }


        public string TrainMasterStr
        {
            get
            {
                return this.TrainMaster == 1 ? "是" : "否";
            }
        }
        #endregion
    }
}
