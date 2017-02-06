using System.Collections.Generic;
using LiXinDataAccess.DepAttendce;
using LiXinModels.DepAttendce;
using LiXinModels.DepCourseManage;
using LiXinInterface.DepCourseManage;
using LiXinModels.User;

namespace LiXinBLL.DepCourseManage
{
    public class Dep_AttendceBL : IDep_Attendce
    {
        private static Dep_AttendceDB dep_attendcebl;
        private static Dep_AttendceApprovalDB dep_attendceApprovalBl;
        public Dep_AttendceBL()
        {
            dep_attendcebl = new Dep_AttendceDB();
            dep_attendceApprovalBl = new Dep_AttendceApprovalDB();
        }

        /// <summary>
        /// 新增考勤
        /// </summary>
        /// <param name="model"></param>
        public int AddDepAttend(Dep_Attendce model)
        {
            dep_attendcebl.InsertDep_Attendce(model);
            return model.Id;
        }

        /// <summary>
        /// 修改考勤
        /// </summary>
        /// <param name="model">考勤信息</param>
        public bool UpdateDepAttend(Dep_Attendce model)
        {
            return dep_attendcebl.UpdateDep_Attendce(model);
        }

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public Dep_Attendce GetDepAttendce(int courseID, int userID)
        {
            return dep_attendcebl.GetDep_Attendce(courseID, userID);
        }

        /// <summary>
        /// 获得数据列表(部门)
        /// </summary>
        public List<DepAttendceCourseModel> GetDepAttendceList(string depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1")
        {
            var list = dep_attendcebl.GetDep_AttendceList(depId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 课前建议方法
        /// </summary>
        /// <param name="depId"></param>
        /// <param name="totalcount"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<DepAttendceCourseModel> GetDepAttendceListForAdvice(string depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1")
        {
            var list = dep_attendcebl.GetDep_AttendceListForAdvice(depId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        public List<Dep_CourseOrder> GetDepAttendUserList(int courseId, int deptId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = dep_attendcebl.GetDep_AttendUserList(courseId, deptId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }


        /// <summary>
        /// 获得考勤人员数据列表 倒出execl专用方法 2013-10-21 毛佳源添加
        /// </summary>
        public List<Dep_CourseOrder> GetDep_AttendUserListForExport(out int totalcount,int courseId, int deptId, string Order = "asc", string where = "1=1")
        {
            var list = dep_attendcebl.GetDep_AttendUserListForExport(courseId, deptId,  Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 选择人员数据列表
        /// </summary>
        public List<Sys_User> GetDepUserList(int courseId, int depId, out int totalCount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            var list = dep_attendcebl.GetDeptUserList(courseId, depId, startIndex, startLength, where);
            totalCount = list.Count > 0 ? list[0].totalCount : 0;
            return list;
        }

        /// <summary>
        /// 验证人员是否满足导入条件
        /// </summary>
        public int IsExistAttendce(int courseId, int userid, int depId)
        {
            return dep_attendcebl.IsExistAttendce(userid, depId);
        }

        /// <summary>
        /// 获得差异数据列表(部门)
        /// </summary>
        public List<DepAttendceCourseModel> GetDifferenceList(int depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1")
        {
            var list = dep_attendcebl.GetDifferenceList(depId, startIndex, startLength, Order,where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获得差异数据列表(主办人)
        /// </summary>
        public List<DepAttendceCourseModel> GetDifferenceListByCo(int cid, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1")
        {
            var list = dep_attendcebl.GetDifferenceListByCo(cid, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }


        public List<DepAttendceCourseModel> GetDepAttendceApprovalList(string whereFlag, int startIndex = 1,
                                                                       int startLength = int.MaxValue,
                                                                       string order = " a.SubmitTime desc",
                                                                       string where = "1=1")
        {
            return dep_attendceApprovalBl.GetDepTranAttendceApprovalList(whereFlag, startIndex, startLength, order, where);
        }

        /// <summary>
        /// 获取人员数据列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="depatId"></param>
        /// <returns></returns>
        public List<DepAttendceCourseModel> GetUserList(int courseId, int depatId)
        {
            return dep_attendceApprovalBl.GetUserList(courseId, depatId);
        }

       

        public DepAttendceCourseModel GetCl_Attendce(int CourseId, int UserId)
        {
            return dep_attendcebl.GetCl_Attendce(CourseId, UserId);
        }

        /// <summary>
        /// 设置个人ApprovalFlag为1
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdatePersonalAttendce(int courseId, int userId)
        {
            return dep_attendcebl.UpdatePersonalAttendce(courseId, userId);
        }

        /// <summary>
        /// 获得课程数据列表(主办人)
        /// </summary>
        public List<DepAttendceCourseModel> GetDepAttendceListByUser(int userId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1")
        {
            var list = dep_attendcebl.GetDep_AttendceListByUser(userId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 根据CourseID获得部门数据
        /// </summary>
        /// <param name="cID"></param>
        /// <returns></returns>
        public List<Sys_Department> GetDepartByCId(int cID)
        {
            return dep_attendcebl.GetDepartByCId(cID);
        }

        /// <summary>
        /// 验证考勤状态为待考勤的人员个数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public int ValidaAttStatus(int courseid, int deptid)
        {
            return dep_attendcebl.ValidaAttStatus(courseid, deptid);
        }


        /// <summary>
        /// 删除考勤记录
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        public void DeleteAttendForCourseAndUser(int courseid, int userid)
        {
            dep_attendcebl.DeleteAttendForCourseAndUser(courseid,userid);
        }


        public void DeleteCourseOrderForCourseAndUser(int courseid, int userid)
        {
            dep_attendcebl.DeleteCourseOrderForCourseAndUser(courseid,userid);
        }
    }
}
