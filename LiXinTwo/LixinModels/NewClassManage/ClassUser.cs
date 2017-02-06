using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewClassManage
{
    public class ClassUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { set; get; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string RealName { set; get; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { set; get; }
        /// <summary>
        /// 所在部门ID
        /// </summary>
        public int DeptId { set; get; }
        /// <summary>
        /// 所在部门名称
        /// </summary>
        public string DeptName { set; get; }
        /// <summary>
        /// 毕业院校
        /// </summary>
        public string GraduateSchool { set; get; }
        /// <summary>
        /// 是否有实习经验 
        /// </summary>
        public int IsInternExp { set; get; }
        /// <summary>
        /// 是否在我所实习
        /// </summary>
        public int IsOurIntern { set; get; }
        /// <summary>
        /// 实习部门
        /// </summary>
        public string InternDept{set;get;}
        /// <summary>
        /// 所在小组ID
        /// </summary>
        public int GroupId{set;get;}
        /// <summary>
        /// 学号
        /// </summary>
        public string NumberID{set;get;}
        /// <summary>
        /// 所学专业
        /// </summary>
        public string Major { set; get; }
    }
}
