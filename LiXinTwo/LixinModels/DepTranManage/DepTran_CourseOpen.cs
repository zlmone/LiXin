using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepTranManage
{
    ///<summary>
    ///DepTran_CourseOpen
    ///</summary>
    public partial class DepTran_CourseOpen
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
        /// DepartId
        /// </summary>
        public int DepartId { get; set; }
        /// <summary>
        /// AttFlag
        /// </summary>
        public int AttFlag { get; set; }
        /// <summary>
        /// OpenFlag
        /// </summary>
        public int OpenFlag { get; set; }
        /// <summary>
        /// SubmitTime
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// ApprovalFlag
        /// </summary>
        public int ApprovalFlag { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public int UserId { get; set; }
         
        #endregion Model
        
        //public int CourseId { get; set; }

        //public string CourseName { get; set; }

        public string DepartSetName { get; set; }

        public DateTime CourseStartTime
        {
            get;
            set;
        }

        public string CourseStartTimeStr
        {
            get {
                return CourseStartTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public DateTime CourseEndTime
        {
            get;
            set;
        }

        public string CourseEndTimeStr
        {
            get
            {
                return CourseEndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public decimal CourseLength
        {
            get;
            set;
        }

        public string RoomName
        {
            get;
            set;
        }

        public string TeacherName
        {
            get;
            set;
        }

        public int IsMust { get; set; }

        public string IsMustStr
        {
            get {
                if (IsMust == 0)
                    return "必修";
                else
                    return "选修";
            }
        }

        #region 短信邮件信息
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 真实课程名
        /// </summary>
        public string CourseName { get; set; }

        public string MobileNum { get; set; }

        public string Email { get; set; }

        #endregion
    }
}
