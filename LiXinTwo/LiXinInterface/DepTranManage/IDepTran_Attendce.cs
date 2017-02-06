using System.Collections.Generic;
using LiXinModels;
using LiXinModels.DepAttendce;
using LiXinModels.DepTranAttendce;
using LiXinModels.DepTranManage;
using LiXinModels.Examination.DBModel;

namespace LiXinInterface.DepTranManage
{
    public interface IDepTran_Attendce
    {
        /// <summary>
        /// 新增考勤
        /// </summary>
        int AddDepAttend(DepTran_Attendce model);

        /// <summary>
        /// 修改考勤
        /// </summary>
        bool UpdateDepAttend(DepTran_Attendce model);

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        DepTran_Attendce GetDepAttendce(int courseID, int userID);

        /// <summary>
        /// 获得数据列表(有分页)
        /// </summary>
        List<AttendceCourseModel> GetDepAttendceList(string depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1");

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        List<DepTran_CourseOrder> GetDepAttendUserList(int courseId, int depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");

        /// <summary>
        /// 选择人员数据列表
        /// </summary>
        List<DepTranDepartSetting> GetDepartUserList(int courseId, int depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1");

        /// <summary>
        /// 验证人员是否满足导入条件
        /// </summary>
        int IsExistAttendce(int courseId, int userid, int depIds);

        /// <summary>
        /// 获得差异数据列表(有分页)
        /// </summary>
        List<AttendceCourseModel> GetDifferenceList(int depId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1");

        List<Dep_Attendce> GetOneUserList(out int totalcount, int userId, int departId, int startIndex = 1, int startLength = int.MaxValue,
                                              string where = "1=1");

        /// <summary>
        /// 获得消息发送人员信息
        /// </summary>
        List<Message> GetUserinfo(int CourseId, string Users);

        tbExampaper GetExampaper(int id);

        /// <summary>
        /// 获取人的考勤详情
        /// </summary>
        /// <returns></returns>
        List<DepTran_CourseOrder> GetAttendceUserList(out int totalcount, int courseId, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1");

        /// <summary>
        /// 设置ApprovalFlag为1
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        bool UpdatePersonalAttendce(int courseId, int userId, int departId);

        /// <summary>
        /// 验证考勤状态为待考勤的人员个数
        /// </summary>
        int ValidaDepAttStatus(int courseid, int deptid);
    }
}
