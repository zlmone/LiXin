using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepTranManage;
using LiXinModels.User;
using LiXinModels.CourseLearn;

namespace LiXinInterface.DepTranManage
{
    public interface IDepTran_CourseOpen
    {
        /// <summary>
        /// 新增考勤
        /// </summary>
        int AddDepCourseOpen(DepTran_CourseOpen model);

        /// <summary>
        /// 修改考勤
        /// </summary>
        bool UpdateCourseOpen(DepTran_CourseOpen model);

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        DepTran_CourseOpen GetCourseOpen(int courseID, int departId);

        /// <summary>
        /// 更新审核内容
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <param name="attFlag"></param>
        /// <param name="approvalFlag"></param>
        /// <returns></returns>
        bool UpDateDepTran_CourseOpen(int courseId, int departId, int attFlag, int approvalFlag);

        /// <summary>
        /// 获得邮件短信发送相关信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        DepTran_CourseOpen CourseName(int courseId, int departId);

        /// <summary>
        /// 获得邮件短信被发送人的信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        DepTran_CourseOpen UserNmae(int courseId, int departId);


        /// <summary>
        /// 转播课程预定查询
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="coursename"></param>
        /// <param name="teachername"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        List<DepTran_CourseOpen> GetList(out int totalCount,int deptid, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " Id");


        List<Sys_User> GetCourseSubscribeUserList(out int totalCount, int deptid, int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " UserId");

        List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int courseID, int DepartId);
    }   
}
