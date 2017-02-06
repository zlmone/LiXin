using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;
using LiXinModels.CourseLearn;
using System.Data;
using LiXinModels;

namespace LiXinInterface.AttendceManage
{
    public interface IAttendce
    {
        /// <summary>
        /// 获取考勤列表(有分页)
        /// </summary>
        List<Co_Course> GetAttendceList(int way, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");

        /// <summary>
        /// 获得考勤人员数据列表(有分页)
        /// </summary>
        /// <returns></returns>
        List<Cl_Attendce> GetAttendUserList(int CourseId,string Order = "asc", string where = "1=1");

        /// <summary>
        /// 导入签到表
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="DataTable"></param>
        string AddAttendces(int CourseId, DataTable dtlist);

        /// <summary>
        /// 考勤录入
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        void AddAttendce(int CourseId, int userid, string starttime, string endtime);

        /// <summary>
        /// 考勤结束计算学时
        /// </summary>
        void UpScore(int CourseId, int userid, DateTime starttime, DateTime endtime);

        /// <summary>
        /// 补预订
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="userid">用户ID</param>
        void MakeUpOrder(int CourseId, int userid);

        /// <summary>
        /// 判断补预订是否超出规定值
        /// </summary>
        bool Cl_MakeUpOrderCount(int count, int userid);

        /// <summary>
        /// 判断是否第一次学习课程
        /// </summary>
        /// <param name="CourseId">课程</param>
        /// <param name="UserId">用户</param>
        /// <returns></returns>
        bool ExistAtts(int CourseId, int UserId);

        /// <summary>
        /// 获得消息发送人员信息
        /// </summary>
        List<Message> GetUserinfo(int CourseId, string Users);

        /// <summary>
        /// 获取课程考勤状态
        /// </summary>
        int GetAttStatus(int CourseId);

        /// <summary>
        /// 更新课程考勤状态
        /// </summary>
        bool UpdateAttStatus(int CourseId);
    }
}
