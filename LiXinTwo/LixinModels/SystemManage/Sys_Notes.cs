using System;
using LiXinCommon;

namespace LiXinModels
{
    [Serializable]
    public class Sys_Notes
    {
        #region model
        /// <summary>
        /// NoteId
        /// </summary>		
        public int NoteId
        {
            get;
            set;
        }
        /// <summary>
        /// NoteTitle
        /// </summary>		
        public string NoteTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 公告类别
        /// </summary>
        public int SortID { get; set; }

        /// <summary>
        /// NoteContent
        /// </summary>		
        public string NoteContent
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
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime
        {
            get;
            set;
        }

        public int publishflag
        {
            get;
            set;
        }

        public DateTime? publishtime
        {
            get;
            set;
        }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public int IsDelete
        {
            get;
            set;
        }

        //是否置顶
        public int IsTop
        {
            get;
            set;
        }

        /// <summary>
        /// 0:通知公告  1:FAQ
        /// </summary>
        public int Type { get; set; }

        public int Num { get; set; }

        /// <summary>
        /// 是否在首页切换显示
        /// </summary>
        public int AdFlag { set; get; }

        /// <summary>
        /// 是否在新进员工首页切换显示
        /// </summary>
        public int DepTrainFlag { set; get; }
        /// <summary>
        /// OpenGroupFlag
        /// </summary>		
        public int OpenGroupFlag
        {
            get;
            set;
        }
        /// <summary>
        /// OpenGroup
        /// </summary>		
        public string OpenGroup
        {
            get;
            set;
        }
        /// <summary>
        /// OpenDepartFlag
        /// </summary>		
        public int OpenDepartFlag
        {
            get;
            set;
        }
        /// <summary>
        /// OpenDepart
        /// </summary>		
        public string OpenDepart
        {
            get;
            set;
        }
        public string ContentDesc { get; set; }

      //  public string TrainGrade { get; set; }
        public string BackUrlName { get; set; }

        /// <summary>
        /// 是否置顶 1是 0否
        /// </summary>
        public int isTopShow { get; set; }

        /// <summary>
        /// 设置置顶的时间
        /// </summary>
        public DateTime TopTime { get; set; }
        #endregion

        #region 扩展

        public int DeptId { get; set; }

        public string NoHtmlNoteContent
        {
            get
            {
                return this.NoteContent == null ? "" : NoteContent.NoHtml();
            }
        }

        public string CreateTimeStr
        {
            get
            {
                return this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string publishtimeStr
        {
            get
            {
                return this.publishtime == null ? "--" : Convert.ToDateTime(this.publishtime).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


        public int totalcount
        {
            get;
            set;
        }

        public string publishFlagStr
        {
            get
            {
                return ((LiXinModels.Enums.PublishFlag)this.publishflag).ToString();
            }
        }

        //阅读次数
        public int LookCount { get; set; }

        //下载次数
        public int DownCount { get; set; }

        /// <summary>
        /// 报表的时候使用
        /// </summary>
        public string typeStr
        {
            get
            {
                return this.Type == 1 ? "阅读" : "下载";
            }
        }

        /// <summary>
        /// 阅读或者下载的时间
        /// </summary>
        public DateTime LookTime { get; set; }

        public string LookTimeStr
        {
            get
            {
                return this.LookTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string Realname { get; set; }

        public string DeptName { get; set; }

        public string TrainGrade { get; set; }
        #endregion
    }
}
