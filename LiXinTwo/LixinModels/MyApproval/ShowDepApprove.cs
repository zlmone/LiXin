using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class ShowDepApprove
    {
        public int ID { get; set; }

        public int UserId { get; set; }

        public string  Realname { get; set; }

        public int Sex { get; set; }

        public string TrainGrade { get; set; }

        public int DeptId { get; set; }

        public string DeptPath { get; set; }

        public string  ManageDeptIDs { get; set; }

        public string ManageDeptName { get; set; }

        public int totalCount { get; set; }


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

        /// <summary>
        /// 是否为审批人
        /// </summary>
        public string IsManage
        {
            get
            {
                return ManageDeptName == "N/A" ? "0" : "1";
            }
        }
    }
}
