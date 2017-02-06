using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Free_DepApprove
    {
        #region Model
        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }

        /// <summary>
        /// CreateUserID
        /// </summary>		
        public int CreateUserID { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// DeptIDs
        /// </summary>		
        public string ManageDeptIDs { get; set; }

        public string LeardID { get; set; }
        #endregion

        public int totalCount { get; set; }
        public string Realname { get; set; }
        public int Sex { get; set; }
        public string DeptPath { get; set; }
        public string createName { get; set; }
        public string DeptName { get; set; }
        public string  TrainGrade { get; set; }
        public string ManageDeptName { get; set; }
        public string SexStr
        {
            get
            {
                if (Sex == 0)
                    return "男";
                else if (Sex == 1)
                    return "女";
                else
                    return "保密";
            }
        }
    }
}
