using System.Collections.Generic;

namespace LiXinModels.Examination.ShowModel
{
    /// <summary>
    ///     考试的信息
    /// </summary>
    public class ExamBaseInforShow
    {
        /// <summary>
        ///     考试标题
        /// </summary>
        public string ExamTitle { set; get; }

        /// <summary>
        ///     考试总分
        /// </summary>
        public int ExamScore { set; get; }

        /// <summary>
        ///     考试时长
        /// </summary>
        public int ExamLength { set; get; }

        /// <summary>
        ///     及格线
        /// </summary>
        public int PassScore { set; get; }

        /// <summary>
        ///     是否百分制
        /// </summary>
        public int PercentScore { set; get; }

        /// <summary>
        ///     显示方式
        /// </summary>
        public int ExamShowWay { set; get; }

        /// <summary>
        ///     开始时间
        /// </summary>
        public string ExamStartTime { set; get; }

        /// <summary>
        ///     结束时间
        /// </summary>
        public string ExamEndTime { set; get; }

        /// <summary>
        ///     是否发布成绩
        /// </summary>
        public int ResultFlag { set; get; }
    }

    /// <summary>
    ///     试卷
    /// </summary>
    public class ExampaperShow
    {
        public ExampaperShow()
        {
            QuestionList = new List<MQuestion>();
            StudentAnswer = new List<StudentAnswer>();
        }

        /// <summary>
        ///     Exampaper Title
        /// </summary>
        public string ExampaperTitle { set; get; }


        /// <summary>
        ///     试卷类型（0：普通试卷；1：随机试卷）
        /// </summary>
        public int ExampaperType { set; get; }

        /// <summary>
        ///     试卷显示方式
        /// </summary>
        public int ExampaperShowType { set; get; }

        /// <summary>
        ///     所含试题类型
        /// </summary>
        public string QuestionTypeStrShow { set; get; }

        /// <summary>
        ///     试卷中问题的列表
        /// </summary>
        public List<MQuestion> QuestionList { set; get; }

        /// <summary>
        ///     学员答案
        /// </summary>
        public List<StudentAnswer> StudentAnswer { set; get; }
    }

    /// <summary>
    ///     学员答案
    /// </summary>
    public class StudentAnswer
    {
        /// <summary>
        ///     问题ID
        /// </summary>
        public int Qid { set; get; }

        /// <summary>
        ///     问题类型
        /// </summary>
        public int QType { set; get; }

        /// <summary>
        ///     学员答案
        /// </summary>
        public string Answer { set; get; }
    }

    /// <summary>
    ///     试题
    /// </summary>
    public class MQuestion
    {
        public MQuestion()
        {
            FileUpload = new List<QuestionFile>();
            QuestionAnswer = new List<MQuestionAnswer>();
        }

        /// <summary>
        ///     问题类型
        /// </summary>
        public int QuestionID { set; get; }

        /// <summary>
        ///     问题类型
        /// </summary>
        public string QTypeShow
        {
            get { return ((QuestionType) QType).ToString(); }
        }

        /// <summary>
        ///     问题类型
        /// </summary>
        public int QType { set; get; }

        /// <summary>
        ///     问题难度
        /// </summary>
        public string QuestionLevel { set; get; }

        /// <summary>
        ///     题干
        /// </summary>
        public string QuestionContent { set; get; }

        /// <summary>
        ///     分值
        /// </summary>
        public int Score { set; get; }

        /// <summary>
        ///     当题型为情景题时：0：问答；1：单选；2：多选
        /// </summary>
        public int QAnswerType { set; get; }

        /// <summary>
        ///     题序
        /// </summary>
        public int QuestionOrder { set; get; }

        /// <summary>
        ///     学员答案
        /// </summary>
        public string UserAnswer { set; get; }

        /// <summary>
        ///     填空题时，空格的个数
        /// </summary>
        public int FillBlankCount { set; get; }

        /// <summary>
        ///     答案关键字
        /// </summary>
        public string QuestionAnswerKey { set; get; }

        /// <summary>
        ///     上传附件
        /// </summary>
        public List<QuestionFile> FileUpload { set; get; }

        /// <summary>
        ///     问题答案
        /// </summary>
        public List<MQuestionAnswer> QuestionAnswer { set; get; }
    }

    /// <summary>
    ///     试题附件
    /// </summary>
    public class QuestionFile
    {
        /// <summary>
        ///     上传附件，0：图片，1：音频，2：视频
        /// </summary>
        public int _fileType { set; get; }

        /// <summary>
        ///     上传名称之后的名称
        /// </summary>
        public string _fileName { set; get; }

        /// <summary>
        ///     真是姓名
        /// </summary>
        public string _realName { get; set; }
    }

    /// <summary>
    ///     问题答案
    /// </summary>
    public class MQuestionAnswer
    {
        /// <summary>
        ///     序号
        /// </summary>
        public int Order { set; get; }

        /// <summary>
        ///     序号:A、B、C、……
        /// </summary>
        public string OrderShow
        {
            get { return ((char) (64 + Order)).ToString(); }
        }

        /// <summary>
        ///    课程的考试里面使用 序号
        /// </summary>
        public int AnswerOrder { set; get; }  
        /// <summary>
        ///     课程的考试里面使用 序号:A、B、C、……
        /// </summary>
        public string AnswerOrderShow
        {
            get { return ((char)(64 + AnswerOrder)).ToString(); }
        }

        /// <summary>
        ///     序号:A、B、C、……
        /// </summary>
        public int QuID { set; get; }

        /// <summary>
        ///     选项内容
        /// </summary>
        public string AnswerContent { set; get; }

        /// <summary>
        ///     是否正确选项(0：非答案；1：正确答案)
        /// </summary>
        public int AnswerFlag { set; get; }

        /// <summary>
        ///     选项的类型(0：主观；1：单选；2：多选)
        /// </summary>
        public int AnswerType { set; get; }

        /// <summary>
        ///     问题的类型
        /// </summary>
        public int QType { set; get; }
    }
}