using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.Survey
{
    public class Survey_Question
    {
        public Survey_Question()
        {
            Answers = new List<Survey_QuestionAnswer>();
            ReplyAnswer = new Survey_ReplyAnswer();
        }
        #region model
        /// <summary>
        /// 问题ID
        /// </summary>
        public int QuestionID
        {
            set;
            get;
        }

        /// <summary>
        /// 问卷ID
        /// </summary>
        public int ExampaperID
        {
            set;
            get;
        }

        /// <summary>
        /// 问题类型（0：单选；1：多选；2：主观）
        /// </summary>
        public int QuestionType
        {
            set;
            get;
        }

        public string QuestionContent
        {
            set;
            get;
        }

        public string QuestionContentNoHtml
        {
            get
            {
                return this.QuestionContent.HtmlDecode();
            }
        }
        /// <summary>
        /// 题干
        /// </summary>
        public string LinkSortPayGradeNoClick
        {
            get
            {
                var link = "";
                if (this.QuestionType == 2)
                {
                    link = string.IsNullOrEmpty(this.LinkSortPayGrade) ? "" : this.LinkSortPayGrade.Replace("<a", "<span").Replace("</a>", "</span>").Replace("onclick", "isonclick").Replace("style=\"cursor:pointer\"", "");
                }
                return link;
            }
        }



        /// <summary>
        /// 问题题序
        /// </summary>
        public int QuestionOrder
        {
            set;
            get;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime UpdateTime
        {
            set;
            get;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public int UserID
        {
            set;
            get;
        }
        /// <summary>
        /// 状态（0：正常；1：删除）
        /// </summary>
        public int Status
        {
            set;
            get;
        }


        public string LinkSortPayGrade
        {
            get;
            set;
        }

        /// <summary>
        /// 评估针对的对象 1 讲师  0 课程 默认为1
        /// </summary>
        public int ObjectType { get; set; }
        #endregion

        public List<Survey_QuestionAnswer> Answers
        {
            get;
            set;
        }

        public string QuestionTypeStr
        {
            get
            {
                return ((Enums.QuestionType)this.QuestionType).ToString();
            }
        }

        public Survey_ReplyAnswer ReplyAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// 有几个人做了
        /// </summary>
        public int useNum
        {
            get;
            set;
        }

        public string sumresult
        {
            get;
            set;
        }

    }
}