using System.Collections.Generic;
using LiXinModels.CourseLearn;
using LiXinDataAccess.DepCourseLearn;
using LiXinModels.DepCourseLearn;
using LiXinInterface.DepCourseLearn;

namespace LiXinBLL.DepCourseLearn
{
    public class Dep_CpaLearnStatusBL : IDep_CpaLearnStatus
    {
        private static Dep_CpaLearnStatusDB dep_CpaLearnDb;
        public Dep_CpaLearnStatusBL()
        {
            dep_CpaLearnDb = new Dep_CpaLearnStatusDB();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public int InsertDepCPALearn(Dep_CpaLearnStatus model)
        {
            dep_CpaLearnDb.InsertDepCPALearn(model);
            return model.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public bool UpdateDepCPALearn(Dep_CpaLearnStatus model)
        {
            return dep_CpaLearnDb.UpdateDepCPALearn(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public bool UpdateDepCPALearn(string Strsql)
        {
            return dep_CpaLearnDb.UpdateDepCPALearn(Strsql);
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public Dep_CpaLearnStatus GetDepCPALearnByCourseId(int courseId, int userId, string whereStr = " 1 = 1 ")
        {
            return dep_CpaLearnDb.GetDepCPALearnByCourseId(courseId, userId, whereStr);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Dep_CpaLearnStatus> GetDepCPALearn(string wherestr = "1=1")
        {
            return dep_CpaLearnDb.GetDepCPALearn(wherestr);
        }
        
    }
}
