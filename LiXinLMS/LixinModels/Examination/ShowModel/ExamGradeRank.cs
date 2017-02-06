using System;

namespace LiXinModels.Examination.ShowModel
{
    public class ExamGradeRank
    {
        #region model

        /// <summary>
        ///     考试ID
        /// </summary>
        public int examtionID
        {
            get;
            set;
        }

        /// <summary>
        ///     考试名称
        /// </summary>
        public string examinationTitle
        {
            set;
            get;
        }

        /// <summary>
        ///     工号
        /// </summary>
        public string jobnum
        {
            get;
            set;
        }

        /// <summary>
        ///     姓名
        /// </summary>
        public string realname
        {
            get;
            set;
        }

        /// <summary>
        ///     部门ID
        /// </summary>
        public int deptID
        {
            get;
            set;
        }

        /// <summary>
        ///     岗位ID
        /// </summary>
        public int postID
        {
            get;
            set;
        }

        /// <summary>
        ///     部门
        /// </summary>
        public string deptcode
        {
            get;
            set;
        }

        /// <summary>
        ///     岗位
        /// </summary>
        public string postcode
        {
            get;
            set;
        }

        /// <summary>
        ///     已答题数
        /// </summary>
        public int hasAnswerNumber
        {
            get;
            set;
        }

        /// <summary>
        ///     未答题数
        /// </summary>
        public int NotAnswerNumber
        {
            get;
            set;
        }

        /// <summary>
        ///     答对题数
        /// </summary>
        public int CorrectAnswerNumber
        {
            get;
            set;
        }

        /// <summary>
        ///     答错题数
        /// </summary
        public int WrongAnswerNumber
        {
            get;
            set;
        }

        /// <summary>
        ///     正确率
        /// </summary
        public double CorrectRate
        {
            get;
            set;
        }

        /// <summary>
        ///     错误率
        /// </summary>
        public double WrongRate
        {
            get;
            set;
        }

        /// <summary>
        ///     是否通过 1通过，0未通过
        /// </summary>
        public int IsPass
        {
            get;
            set;
        }

        /// <summary>
        ///     名次
        /// </summary
        public int Rank
        {
            get;
            set;
        }

        /// <summary>
        ///     获得的分数
        /// </summary>
        public int sumSocre
        {
            get;
            set;
        }

        /// <summary>
        ///     考试开始时间
        /// </summary>
        public DateTime StartDate
        {
            get;
            set;
        }

        /// <summary>
        ///     考试结束时间
        /// </summary>
        public DateTime EndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 是否百分制 0是 1不是
        /// </summary>
        public int PercentFlag
        {
            get;
            set;
        }


        public int questionScore
        {
            get;
            set;
        }
        #endregion


        public int ReallyScore
        {
            get
            {
                if (this.PercentFlag == 0)
                    return 100;
                return questionScore;

            }
        }

        /// <summary>
        /// 真正获得的分数，按百分制换算
        /// </summary>
        public double GetScore
        {
            get
            {
                //if (this.PercentFlag == 1)
                //{
                    
                //    return questionScore == 0 ? 0 : Math.Round((sumSocre / Convert.ToDouble(questionScore)) * 100,0);
                //}
                return Convert.ToDouble(sumSocre);
            }
        }



        /// <summary>
        ///     通过状态，冗余字段，防止它对状态有特殊处理
        /// </summary>
        public string passState
        {
            get
            {
                if (IsPass == 1)
                {
                    return "是";
                }
                else
                {
                    return "否";
                }
            }
        }
    }
}