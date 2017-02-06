using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;
using LiXinModels.DepCourseLearn;
using LiXinModels.DepCourseManage;

namespace LiXinInterface.DepCourseLearn
{
    public interface IDep_CpaLearnStatus
    {
        /// <summary>
        /// 添加
        /// </summary>
        int InsertDepCPALearn(Dep_CpaLearnStatus model);

        /// <summary>
        /// 修改
        /// </summary>
        bool UpdateDepCPALearn(Dep_CpaLearnStatus model);

        /// <summary>
        /// 修改
        /// </summary>
        bool UpdateDepCPALearn(string Strsql);

        /// <summary>
        /// 获取单个实体
        /// </summary>
        Dep_CpaLearnStatus GetDepCPALearnByCourseId(int courseId, int userId, string whereStr = " 1 = 1 ");

        /// <summary>
        /// 查询
        /// </summary>
        List<Dep_CpaLearnStatus> GetDepCPALearn(string wherestr = "1=1");
    }
}
