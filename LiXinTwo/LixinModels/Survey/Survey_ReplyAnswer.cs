using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Survey
{
    public class Survey_ReplyAnswer
    {
        #region model
        public DateTime SubmitTime { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public int AnswerId
        {
            set;
            get;
        }
        /// <summary>
        /// 来源（0：课后评估； 1 调查……）
        /// </summary>
        public int SourceFrom
        {
            set;
            get;
        }
        /// <summary>
        /// 对象ID（SourceFrom为0时：课程ID；1 调查ID……）
        /// </summary>
        public int ObjectID
        {
            set;
            get;
        }
        /// <summary>
        /// 试卷ID
        /// </summary>
        public int ExampaperID
        {
            set;
            get;
        }
        /// <summary>
        /// 回答人ID
        /// </summary>
        public int UserID
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
        /// <summary>
        /// 主观题回答
        /// </summary>
        public string SubjectiveAnswer
        {
            set;
            get;
        }
        /// <summary>
        /// 客观题回答
        /// </summary>
        public string ObjectiveAnswer
        {
            set;
            get;
        }

        /// <summary>
        /// 0：暂存；1：提交 
        /// </summary>
        public int Status
        {
            set;
            get;
        }

        /// <summary>
        /// QuestionReply
        /// </summary>		
        public string QuestionReply
        {
            get;
            set;
        }
        /// <summary>
        /// 人员所在的部门ID
        /// </summary>		
        public int DeptID
        {
            get;
            set;
        }
        /// <summary>
        ///是否被部门采纳  0否 1采纳
        /// </summary>		
        public int IsDeptAccessed
        {
            get;
            set;
        }
        /// <summary>
        /// 是否被事务所采纳  0否 1推荐给事务所   2被事务所采纳
        /// </summary>		
        public int IsOfficeAccessed
        {
            get;
            set;
        }
        /// <summary>
        /// 是否为部门负责人
        /// </summary>		
        public int IsMaster
        {
            get;
            set;
        }

        /// <summary>
        /// 课程教室分配规则ID 关联New_CourseRoomRule 表的Id 
        /// </summary>		
        public int CourseRoomRuleId
        {
            get;
            set;
        }

        
        #endregion

        public List<string> objectAnswerList
        {
            get
            {
               return (this.ObjectiveAnswer==""||this.ObjectiveAnswer==null)?new List<string>():this.ObjectiveAnswer.Split(',').ToList();
            }

        }
        public List<string> subjectiveAnswerList
        {
            get
            {
                return (this.SubjectiveAnswer == "" || this.SubjectiveAnswer == null) ? new List<string>() : this.SubjectiveAnswer.Split(',').ToList();
            }

        }

        public List<Survey_Question> QuestionList
        {
            get;
            set;
        }

        public string  stuName
        {
            get;
            set;
        }

        /// <summary>
        /// 星评题的平均分
        /// </summary>
        public double avgXinP
        {
            get;
            set;
        }

        /// <summary>
        /// 星评题的总分
        /// </summary>
        public int sumXinP
        {
            get;
            set;
        }


        /// <summary>
        /// 回答的人数
        /// </summary>
        public int userNum
        {
            get;
            set;
        }

        /// <summary>
        /// 主观题回答信息显示
        /// </summary>
        public string SubjectiveAnswerStr
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.SubjectiveAnswer) ? "--" : this.SubjectiveAnswer;
            }
        }
        public string QuestionReplyStr
        {
            get
            {
                return string.IsNullOrEmpty(this.QuestionReply) ? "--" : this.QuestionReply;
            }
        }

        public string Realname { get; set; }

        public string DeptName { get; set; }

        public int totalcount { get; set; }
    }
}
