using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepAttendce;
using LiXinModels.DepCourseManage;
using LiXinModels.User;

namespace LiXinInterface.DepCourseManage
{
    public interface IDep_Attendce
    {
        /// <summary>
        /// 新增考勤
        /// </summary>
        int AddDepAttend(Dep_Attendce model);

        /// <summary>
        /// 修改考勤
        /// </summary>
        bool UpdateDepAttend(Dep_Attendce model);

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        Dep_Attendce GetDepAttendce(int courseID, int userID);

        /// <summary>
        /// 获得数据列表(部门)
        /// </summary>
        List<DepAttendceCourseModel> GetDepAttendceList(string depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1");


        /// <summary>
        /// 获得数据列表(部门) 课前建议方法
        /// </summary>
        List<DepAttendceCourseModel> GetDepAttendceListForAdvice(string depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1");

        /// <summary>
        /// 获得差异数据列表(主办人)
        /// </summary>
        List<DepAttendceCourseModel> GetDifferenceListByCo(int cid, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1");

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        List<Dep_CourseOrder> GetDepAttendUserList(int courseId, int deptId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");


        /// <summary>
        /// 获得考勤人员数据列表 倒出execl专用方法 2013-10-21 毛佳源添加
        /// </summary>
        List<Dep_CourseOrder> GetDep_AttendUserListForExport(out int totalcount, int courseId, int deptId, string Order = "asc", string where = "1=1");
        
        /// <summary>
        /// 选择人员数据列表
        /// </summary>
        List<Sys_User> GetDepUserList(int courseId, int depId, out int totalCount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1");

        /// <summary>
        /// 验证人员是否满足导入条件
        /// </summary>
        int IsExistAttendce(int courseId, int userid, int depId);

        /// <summary>
        /// 获得差异数据列表(有分页)
        /// </summary>
        List<DepAttendceCourseModel> GetDifferenceList(int depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1");

        /// <summary>
        /// 获得审批的数据列表(有分页)
        /// </summary>
        /// <param name="startIndex">当前页</param>
        /// <param name="startLength">条数</param>
        /// <param name="order">排序方式</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        List<DepAttendceCourseModel> GetDepAttendceApprovalList(string whereFlag, int startIndex = 1,
                                                                int startLength = int.MaxValue,
                                                                string order = " a.SubmitTime desc",
                                                                string where = "1=1");

        /// <summary>
        /// 获取人员数据列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="depatId"></param>
        /// <returns></returns>
        List<DepAttendceCourseModel> GetUserList(int courseId, int depatId);
       


        /// <summary>
        /// 获取单个考勤实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        DepAttendceCourseModel GetCl_Attendce(int CourseId, int UserId);

        /// <summary>
        /// 设置个人ApprovalFlag为1
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UpdatePersonalAttendce(int courseId, int userId);

        /// <summary>
        /// 获得课程数据列表(主办人)
        /// </summary>
        List<DepAttendceCourseModel> GetDepAttendceListByUser(int userId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1");

        /// <summary>
        /// 根据CourseID获得部门数据
        /// </summary>
        List<Sys_Department> GetDepartByCId(int cID);

        /// <summary>
        /// 验证考勤状态为待考勤的人员个数
        /// </summary>
        int ValidaAttStatus(int courseid, int deptid);


        /// <summary>
        /// 删除考勤记录
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        void DeleteAttendForCourseAndUser(int courseid, int userid);

        /// <summary>
        /// 删除预定记录
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        void DeleteCourseOrderForCourseAndUser(int courseid, int userid);
    }
}
