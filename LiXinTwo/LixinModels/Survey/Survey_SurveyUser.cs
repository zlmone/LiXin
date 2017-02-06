using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;

namespace LiXinModels.Survey
{
    public class Survey_SurveyUser
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>		
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// SurveyID
        /// </summary>		
        public int SurveyID
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
        /// Score
        /// </summary>		
        public decimal Score
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
        /// IsAccessed
        /// </summary>		
        public int IsAccessed
        {
            get;
            set;
        }
        #endregion


    }

}
