using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Survey
{
    public class Survey_Exampaper
    {
        public Survey_Exampaper()
        {
            Questions = new List<Survey_Question>();
        }
        #region model

        /// <summary>
        /// 问卷ID
        /// </summary>
        public int ExampaperID
        {
            set;
            get;
        }

        /// <summary>
        /// 问卷标题
        /// </summary>
        public string ExamTitle
        {
            set;
            get;
        }

        /// <summary>
        /// 问卷类型（0：课后评估问卷；1：普通问卷；……）
        /// </summary>
        public int ExamType
        {
            set;
            get;
        }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int UserID
        {
            set;
            get;
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int SortID
        {
            set;
            get;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime LastUpdateTime
        {
            set;
            get;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string ExamDescription
        {
            get;
            set;
        }
        /// <summary>
        /// 问卷使用状态
        /// </summary>
        public int ExampaperUsage
        {
            set;
            get;
        }
        /// <summary>
        /// 状态（0：正常；1：删除）
        /// </summary>
        public int IsDelete
        {
            set;
            get;
        }

        public int DeptID { get; set; }
        #endregion

        #region 扩展字段

        /// <summary>
        /// 创建时间呈现
        /// </summary>
        public string LastUpdateTimeShow
        {
            get
            {
                return LastUpdateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 问卷类型显示
        /// </summary>
        public string ExamTypeShow
        {
            get
            {
                return ((Enums.ExamType)this.ExamType).ToString();
            }
        }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string SortName
        {
            set;
            get;
        }

        public string CategoryName
        {
            get;
            set;
        }


        public List<Survey_Question> Questions
        {
            get;
            set;
        }

        public int SingleCount
        {
            get;
            set;
        }
        public int MultipeCount
        {
            get;
            set;
        }
        public int zhuguanCount
        {
            get;
            set;
        }
        public int xingpCount
        {
            get;
            set;
        }

        #endregion

    }
}
