using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.NewCourseManage;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_Attendce
    {
        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        List<New_CourseOrderDetail> GetNewAttendUserList(int courseId, out int totalcount,int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");

        /// <summary>
        /// 扩展GetNewAttendUserList 增加讲师姓名和ID
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="totalcount"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<New_CourseOrderDetail> GetNewAttendList(int courseId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");
        
        /// <summary>
        /// 新增考勤
        /// </summary>
        /// <param name="model"></param>
        int AddNewAttend(New_Attendce model);

        /// <summary>
        /// 修改考勤
        /// </summary>
        /// <param name="model"></param>
        bool UpdateNewAttend(New_Attendce model);

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        New_Attendce GetNew_Attendce(int courseID, int userID, int subCourseID);

        /// <summary>
        /// 获取考勤列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<New_Attendce> GetList(string where = "1=1");

        /// <summary>
        /// 获得我的考勤数据列表
        /// </summary>
        List<New_CourseOrderDetail> GetMyNewAttendList(int userID, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "ncod.CourseId desc", string where = "1=1");
    }
}
