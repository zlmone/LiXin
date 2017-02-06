using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DepCourseManage;
using LiXinModels.DepCourseManage;
using LiXinInterface.DepCourseManage;

namespace LiXinBLL.DepCourseManage
{
    public class Dep_CourseDeptBL : IDep_CourseDept
    {
        private static Dep_CourseDeptDB dep_Codepbl;
        public Dep_CourseDeptBL()
        {
            dep_Codepbl = new Dep_CourseDeptDB();
        }

        /// <summary>
        /// 新增考勤审核关联
        /// </summary>
        public int AddCourseDept(Dep_CourseDept model)
        {
            dep_Codepbl.InsertDep_CourseDept(model);
            return model.Id;
        }

        /// <summary>
        /// 修改考勤审核关联
        /// </summary>
        public bool UpdateCourseDept(Dep_CourseDept model)
        {
            return dep_Codepbl.UpdateDep_CourseDept(model);
        }

        /// <summary>
        /// 获取单条考勤审核关联信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public Dep_CourseDept GetCourseDept(int courseID, int departId)
        {
            return dep_Codepbl.GetDep_CourseDept(courseID, departId);
        }

        /// <summary>
        /// 获得邮件短信发送相关信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public Dep_CourseDept CourseName(int courseId, int departId)
        {
            return dep_Codepbl.CourseName(courseId, departId);
        }

        /// <summary>
        /// 获得邮件短信被发送人的信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public Dep_CourseDept UserNmae(int courseId, int departId)
        {
            return dep_Codepbl.UserNmae(courseId, departId);
        }

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public Dep_CourseDept GetCourseOpen(int courseID, int departId)
        {
            return dep_Codepbl.GetDepTran_CourseOpen(courseID, departId);
        }

        /// <summary>
        /// 更新审核内容
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <param name="attFlag"></param>
        /// <param name="approvalFlag"></param>
        /// <returns></returns>
        public bool UpDateDepTran_CourseOpen(int courseId, int departId, int attFlag, int approvalFlag)
        {
            return dep_Codepbl.UpDateDepTran_CourseOpen(courseId, departId, attFlag, approvalFlag);
        }



    }
}
