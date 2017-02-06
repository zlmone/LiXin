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

        #endregion

        #region 扩展
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


        public string RealName
        {
            get;
            set;
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

        #endregion
    }
}
