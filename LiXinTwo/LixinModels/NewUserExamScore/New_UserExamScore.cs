using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class New_UserExamScore
    {
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }

        /// <summary>
        /// Score
        /// </summary>		
        public decimal Score { get; set; }

        /// <summary>
        /// SumScore
        /// </summary>		
        public decimal SumScore { get; set; }
    }
}
