using System;

namespace LiXinModels.User
{
    /// <summary>
    /// 指纹库的用户指纹信息
    /// </summary>
    public class Sys_UserFinger
    {
        public int Id { get; set; }
        /// <summary>
        /// 考勤系统中的人员ID
        /// </summary>
        public int USERID { set; get; }
        /// <summary>
        /// HR系统中的人员ID
        /// </summary>
        public string TITLE { set; get; }
        /// <summary>
        /// 人员工号
        /// </summary>
        public string SSN { set; get; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string NAME { set; get; }
        /// <summary>
        /// 指纹信息
        /// </summary>
        public byte[] FingerTemplate { set; get; }
        /// <summary>
        /// 指纹信息
        /// </summary>
        public string FingerTemplate1 { set; get; }
        /// <summary>
        /// 指纹信息
        /// </summary>
        public string FingerTemplate2 { set; get; }

        public int totalcount { set; get; }

        public string deptname { get; set; }

        public string Realname { get; set; }

        public int UserId { get; set; }
        /// <summary>
        /// 培训级别
        /// </summary>
        public string TrainGrade { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
    }
}