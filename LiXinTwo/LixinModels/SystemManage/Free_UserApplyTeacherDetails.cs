using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Free_UserApplyTeacherDetails
    {
        #region Model

        public int ID { get; set; }

        public int userApplyID { get; set; }

        public string ClassName { get; set; }

        public decimal ConvertScore { get; set; }

        public int IsDelete { get; set; }

        public decimal tScore { get; set; }

        public decimal CPAScore { get; set; }

        #endregion

        #region 扩展

        public decimal TrainGradeScore { get; set; }

        public decimal ConvertMax { get; set; }

        public int CPA { get; set; }

        public decimal ConvertBaseTo { get; set; }

        public decimal ApprovetScore { get; set; }

        public decimal ApproveCPAScore { get; set; }

        public int key { get; set; }

        #endregion
    }

}
