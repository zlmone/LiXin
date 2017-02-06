using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewCourseManage
{
    public class UserSeatModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { set; get; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 所在部门
        /// </summary>
        public string DeptName { set; get; }
        /// <summary>
        /// 学号
        /// </summary>
        public string NumberID { set; get; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { set; get; }
        /// <summary>
        /// 班号
        /// </summary>
        public string ClassNo { set; get; }
        /// <summary>
        /// 班号
        /// </summary>
        public string GroupNo { set; get; }
    }
}
