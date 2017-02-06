using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class ShowFreeUserDetails
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public string Realname { get; set; }

        public string DeptName { get; set; }

        public string TrainGrade { get; set; }

        public string CPA { get; set; }

        public string CPANO { get; set; }

        public decimal tScore { get; set; }

        public decimal CPAScore { get; set; }

        public decimal GettScore { get; set; }

        public decimal GetCPAScore { get; set; }
    }
}
