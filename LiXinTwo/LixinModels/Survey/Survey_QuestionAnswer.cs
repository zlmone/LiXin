using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.Survey
{
    public class Survey_QuestionAnswer
    {
        /// <summary>
        /// 选项ID
        /// </summary>
        public int AnswerId
        {
            set;
            get;
        }
        /// <summary>
        /// 问题ID
        /// </summary>
        public int QuestionID
        {
            set;
            get;
        }

        public string AnswerContent
        {
            set;
            get;
        }
        /// <summary>
        /// 选项内容
        /// </summary>
        public string AnswerContentNohtml
        {

            get
            {
                return AnswerContent.HtmlDecode();
            }
        }
        /// <summary>
        /// 分值
        /// </summary>
        public int Score
        {
            set;
            get;
        }
        /// <summary>
        /// 选项排序
        /// </summary>
        public string ShowOrder
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

        /// <summary>
        /// 课后评估，问题与选项
        /// </summary>
        public Survey_Question sq { get; set; }
        public List<Survey_QuestionAnswer> sqalist { get; set; }
    }
}
