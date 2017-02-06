using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Cl_TimeOutLeave
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>		
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// CourseId
        /// </summary>		
        public int CourseId
        {
            get;
            set;
        }
        /// <summary>
        /// UserId
        /// </summary>		
        public int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// ApprovalUser
        /// </summary>		
        public string ApprovalUser
        {
            get;
            set;
        }
        /// <summary>
        /// AppReason
        /// </summary>		
        public string AppReason
        {
            get;
            set;
        }
        /// <summary>
        /// OutTime
        /// </summary>		
        public DateTime OutTime
        {
            get;
            set;
        }
        /// <summary>
        /// TrainAppFlag
        /// </summary>		
        public int TrainAppFlag
        {
            get;
            set;
        }
        /// <summary>
        /// ApprovalReason
        /// </summary>		
        public string ApprovalReason
        {
            get;
            set;
        }
        /// <summary>
        /// OutUserID
        /// </summary>		
        public string OutUserID
        {
            get;
            set;
        }
        /// <summary>
        /// Name
        /// </summary>		
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// MakeUpTime
        /// </summary>		
        public DateTime MakeUpTime
        {
            get;
            set;
        }
        /// <summary>
        /// CourseName
        /// </summary>		
        public string CourseName
        {
            get;
            set;
        }
        /// <summary>
        /// CourseStartTime
        /// </summary>		
        public DateTime CourseStartTime
        {
            get;
            set;
        }
        /// <summary>
        /// CourseEndTime
        /// </summary>		
        public DateTime CourseEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// ApprovalMemo
        /// </summary>		
        public string ApprovalMemo
        {
            get;
            set;
        }
        /// <summary>
        /// ApprovalFlag  默认为-1
        /// </summary>		
        public int ApprovalFlag
        {
            get;
            set;
        }
        /// <summary>
        /// ApprovalDateTime
        /// </summary>		
        public DateTime ApprovalDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// requestid
        /// </summary>		
        public int requestid
        {
            get;
            set;
        }
        /// <summary>
        /// FtriggerFlag
        /// </summary>		
        public int FtriggerFlag
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 申请时间
        /// </summary>
        public string TimeOutApplyTime
        {
            get
            {
                return this.OutTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        public string ApprovalStatusStr
        {
            get
            {
                if (this.ApprovalFlag == -1)
                {
                    return "待审批";
                }
                return ((Enums.ApprovalStatus)ApprovalFlag).ToString();
            }
        }

    }
}
