using System;

namespace LiXinModels.SystemManage
{
    [Serializable]
    public class Sys_GroupUser
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 用户ID 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 人员编号
        /// </summary>
        public string JobNum { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 培训级别
        /// </summary>
        public string TrainGrade { get; set; }

        /// <summary>
        /// 薪酬级别
        /// </summary>
        public string PayGrade { get; set; }

        /// <summary>
        /// 短号
        /// </summary>
        public string GroupMobileNum { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobileNum { set; get; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        public string DeptPath { get; set; }
        
        public string SexStr
        {
            get
            {
                if (Sex == 0)
                    return "男士";
                else if (Sex == 1)
                    return "女士";
                else
                    return "——";
            }
        }
        public string TrainGradeStr
        {
            get
            {
                if (string.IsNullOrEmpty(TrainGrade))
                    return "——";
                return TrainGrade;
            }
        }
        public string PayGradeStr
        {
            get
            {
                if (string.IsNullOrEmpty(PayGrade))
                    return "——";
                return PayGrade;
            }
        }
        public string GroupMobileNumStr
        {
            get
            {
                if (string.IsNullOrEmpty(GroupMobileNum))
                    return "——";
                return GroupMobileNum;
            }
        }
        public string TelephoneStr
        {
            get
            {
                if (string.IsNullOrEmpty(Telephone))
                    return "——";
                return Telephone;
            }
        }
        public string EmailStr
        {
            get
            {
                if (string.IsNullOrEmpty(Email))
                    return "——";
                return Email;
            }
        }
        public string DeptStr
        {
            get
            {
                if (string.IsNullOrEmpty(DeptName))
                    return "——";
                return DeptName;
            }
        }

        public int totalcount
        {
            get;
            set;
        }
    }
}
