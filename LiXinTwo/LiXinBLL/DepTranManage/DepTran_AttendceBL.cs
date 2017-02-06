using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DepTranAttendce;
using LiXinDataAccess.Examination;
using LiXinModels;
using LiXinModels.DepAttendce;
using LiXinModels.DepTranAttendce;
using LiXinModels.CourseManage;
using LiXinInterface.DepTranManage;
using LiXinModels.DepTranManage;
using LiXinModels.Examination.DBModel;

namespace LiXinBLL.DepTranManage
{
    public class DepTran_AttendceBL : IDepTran_Attendce
    {
        private static DepTran_AttendceDB dep_attendcebl;
        private readonly ExampaperDB paperDB;
        public DepTran_AttendceBL()
        {
            dep_attendcebl = new DepTran_AttendceDB();
            paperDB = new ExampaperDB();
        }

        /// <summary>
        /// 新增考勤
        /// </summary>
        /// <param name="model"></param>
        public int AddDepAttend(DepTran_Attendce model)
        {
            dep_attendcebl.InsertDepTran_Attendce(model);
            return model.Id;
        }

        /// <summary>
        /// 修改考勤
        /// </summary>
        /// <param name="model">考勤信息</param>
        public bool UpdateDepAttend(DepTran_Attendce model)
        {
            return dep_attendcebl.UpdateDepTran_Attendce(model);
        }

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public DepTran_Attendce GetDepAttendce(int courseID, int userID)
        {
            return dep_attendcebl.GetDepTran_Attendce(courseID, userID);
        }

        /// <summary>
        /// 获得数据列表(有分页)
        /// </summary>
        public List<AttendceCourseModel> GetDepAttendceList(string depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1")
        {
            var list = dep_attendcebl.GetDepTran_AttendceList(depId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        public List<DepTran_CourseOrder> GetDepAttendUserList(int courseId, int deptId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = dep_attendcebl.GetDepTran_AttendUserList(courseId, deptId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 选择人员数据列表
        /// </summary>
        public List<DepTranDepartSetting> GetDepartUserList(int courseId, int depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            var list = dep_attendcebl.GetDepartUserList(courseId, depId, startIndex, startLength, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 验证人员是否满足导入条件
        /// </summary>
        public int IsExistAttendce(int courseId, int userid, int depId)
        {
            return dep_attendcebl.IsExistAttendce(courseId, userid, depId);
        }

        /// <summary>
        /// 获得差异数据列表(有分页)
        /// </summary>
        public List<AttendceCourseModel> GetDifferenceList(int depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1")
        {
            var list = dep_attendcebl.GetDifferenceList(depId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        public List<Dep_Attendce> GetOneUserList(out int totalcount, int userId, int departId, int startIndex = 1, int startLength = int.MaxValue,
                                                     string where = "1=1")
        {
            var list = dep_attendcebl.GetOneUserList(userId, departId, startIndex, startLength, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获得消息发送人员信息
        /// </summary>
        /// <returns></returns>
        public List<Message> GetUserinfo(int CourseId, string Users)
        {
            List<Message> Utemp = dep_attendcebl.GetUserinfo(CourseId, Users);
            return Utemp;
        }

        public tbExampaper GetExampaper(int id)
        {
            return paperDB.GetExampaper(id);
        }

        /// <summary>
        /// 获取人的考勤详情
        /// </summary>
        /// <returns></returns>
        public List<DepTran_CourseOrder> GetAttendceUserList(out int totalcount, int courseId, int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1")
        {
            var list = dep_attendcebl.GetAttendceUserList(courseId, startIndex, startLength, where);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 设置ApprovalFlag为1
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public bool UpdatePersonalAttendce(int courseId, int userId, int departId)
        {
            return dep_attendcebl.UpdatePersonalAttendce(courseId, userId, departId);
        }

        /// <summary>
        /// 验证考勤状态为待考勤的人员个数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public int ValidaDepAttStatus(int courseid, int deptid)
        {
            return dep_attendcebl.ValidaDepAttStatus(courseid, deptid);
        }
    }
}
