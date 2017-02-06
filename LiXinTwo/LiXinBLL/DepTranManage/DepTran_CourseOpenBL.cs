using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DepTranManage;
using LiXinModels.DepTranManage;
using LiXinInterface.DepTranManage;
using LiXinModels.User;
using LiXinModels.CourseLearn;

namespace LiXinBLL.DepTranManage
{
    public class DepTran_CourseOpenBL : IDepTran_CourseOpen
    {
        private static DepTran_CourseOpenDB dep_CoOpenbl;
        public DepTran_CourseOpenBL()
        {
            dep_CoOpenbl = new DepTran_CourseOpenDB();
        }

        /// <summary>
        /// 新增考勤
        /// </summary>
        public int AddDepCourseOpen(DepTran_CourseOpen model)
        {
            dep_CoOpenbl.InsertDepTran_CourseOpen(model);
            return model.Id;
        }

        /// <summary>
        /// 修改考勤
        /// </summary>
        public bool UpdateCourseOpen(DepTran_CourseOpen model)
        {
            return dep_CoOpenbl.UpdateDepTran_CourseOpen(model);
        }

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public DepTran_CourseOpen GetCourseOpen(int courseID, int departId)
        {
            return dep_CoOpenbl.GetDepTran_CourseOpen(courseID, departId);
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
            return dep_CoOpenbl.UpDateDepTran_CourseOpen(courseId, departId, attFlag, approvalFlag);
        }

        /// <summary>
        /// 获得邮件短信发送相关信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public DepTran_CourseOpen CourseName(int courseId, int departId)
        {
            return dep_CoOpenbl.CourseName(courseId, departId);
        }

        /// <summary>
        /// 获得邮件短信被发送人的信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public DepTran_CourseOpen UserNmae(int courseId, int departId)
        {
            return dep_CoOpenbl.UserNmae(courseId, departId);
        }


        /// <summary>
        /// 视频转播课程预定查询 
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="coursename"></param>
        /// <param name="teachername"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public List<DepTran_CourseOpen> GetList(out int totalCount,int deptid, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " Id")
        {
            return dep_CoOpenbl.GetList(out totalCount,deptid, where, startIndex, startLength , jsRenderSortField);
        }


        public List<Sys_User> GetCourseSubscribeUserList(out int totalCount,int deptid, int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " UserId")
        {
            return dep_CoOpenbl.GetCourseSubscribeUserList(out totalCount, deptid,courseID, startIndex, startLength, where, jsRenderSortField);
        }

         /// <summary>
        /// 获取报名成功的人员，根据培训级别分
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int courseID,int deptid)
        {
            return dep_CoOpenbl.GetSuccessTrainGradeSubscribe(courseID, deptid);
        }
    }
}
