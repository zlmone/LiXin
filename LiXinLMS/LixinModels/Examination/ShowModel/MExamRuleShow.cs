namespace LiXinModels.Examination.ShowModel
{
    public class MExamRuleShow
    {
        /// <summary>
        ///     分值
        /// </summary>
        public int QuestingScore { set; get; }

        /// <summary>
        ///     类型
        /// </summary>
        public string QuestionType { set; get; }

        /// <summary>
        ///     题库
        /// </summary>
        public string QuestionSort { set; get; }

        /// <summary>
        ///     难度
        /// </summary>
        public int Leveleasy { set; get; }

        public int Levelcommon { set; get; }
        public int Levelhard { set; get; }

        public string qita { set; get; }

        public string qitaone { set; get; }
    }
}