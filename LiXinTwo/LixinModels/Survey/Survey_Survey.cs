using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
namespace LiXinModels.Survey
{
    public class Survey_Survey
    {
        #region model
        /// <summary>
        /// Id
        /// </summary>		
        public int Id
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
        /// PaperID
        /// </summary>		
        public int PaperID
        {
            get;
            set;
        }
        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo
        {
            get;
            set;
        }
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
        /// <summary>
        /// PublishFlag
        /// </summary>		
        public int PublishFlag
        {
            get;
            set;
        }
        /// <summary>
        /// PublishTime
        /// </summary>		
        public DateTime PublishTime
        {
            get;
            set;
        }
        /// <summary>
        /// LastUpdateTime
        /// </summary>		
        public DateTime LastUpdateTime
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
        /// StartTime
        /// </summary>		
        public DateTime StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// EndTime
        /// </summary>		
        public DateTime EndTime
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

        /// <summary>
        /// Score
        /// </summary>		
        public decimal Score
        {
            get;
            set;
        }

        /// <summary>
        /// IsAccessed
        /// </summary>		
        public int IsAccessed
        {
            get;
            set;
        }


        public int DeptID { get; set; }

        #endregion

        #region 扩展
        public int totalCount
        {
            get;
            set;
        }
        public string ExamTitle
        {
            get;
            set;
        }
        public string PublishTimeStr
        {
            get
            {
                return this.PublishTime.ToString() == "0001/1/1 0:00:00" ? "--" : this.PublishTime.ToString("yyyy-MM-dd");
            }
        }

        public string status
        {
            get
            {
                var str = "";
                if (this.PublishFlag == 0)
                {
                    str = "未发布";
                }
                else
                {
                    str = "已发布";
                }
                return str;
            }
        }

        public int chbOpenFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 是否在期限内
        /// </summary>
        public string IsInDate
        {
            get
            {
                var now = DateTime.Now.Date;
                if (now >= this.StartTime && now <= this.EndTime)
                {
                    return "是";
                }
                else
                {
                    return "否";
                }
            }
        }


        /// <summary>
        /// 是否可以发布
        /// </summary>
        public string IsPublish
        {
            get
            {
                var now = DateTime.Now.Date;
                if (now <= this.EndTime)
                {
                    return "是";
                }
                else
                {
                    return "否";
                }
            }
        }


        public string Statustr
        {
            get
            {
                return IsAccessed == 0 ? "否" : "是";

            }
        }
        public string Scorestr
        {
            get
            {
                return this.Score == 0 ? "--" : this.Score.ToString();
            }
        }
        public string DoTime
        {
            get
            {
                return this.StartTime.ToString("yyyy-MM-dd") + "-" + this.EndTime.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 是否做了
        /// </summary>
        public int IsDo
        {
            get;
            set;
        }

        public int useNum
        {
            get;
            set;
        }

        public string startstr
        {
            get
            {
                return this.StartTime.ToString("yyyy-MM-dd");
            }
        }
        public string endstr
        {
            get
            {
                return this.EndTime.ToString("yyyy-MM-dd");
            }
        }

        public string MemoNohtml
        {
            get
            {
                return this.Memo.HtmlDecode().NoHtml();
            }
        }

        public string DoStatu
        {
            get
            {
                var now = DateTime.Now.Date;
                if (this.IsDo == 0)
                {
                    if (now < this.StartTime)
                    {
                        return "未开始";
                    }
                    else if (now >= this.StartTime && now <= this.EndTime)
                    {
                        return "进行中";
                    }
                    else
                    {
                        return "已关闭";
                    }
                }
                else
                {

                    if (now >= this.StartTime && now <= this.EndTime)
                    {
                        return "进行中";
                    }
                    else
                    {
                        return "已关闭";
                    }
                }
            }
        }

        /// <summary>
        ///发布人
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 发布部门
        /// </summary>
        public string DeptName { get; set; }
        #endregion


        public int ShouldNumber { get; set; }

        public Survey_Exampaper examPaper
        {
            get;
            set;
        }


    }
}
