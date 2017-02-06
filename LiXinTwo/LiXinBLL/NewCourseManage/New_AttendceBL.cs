using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.NewCourseManage;
using LiXinInterface.NewCourseManage;
using LiXinModels.NewCourseManage;

namespace LiXinBLL.NewCourseManage
{
    public class New_AttendceBL : INew_Attendce
    {
        private static New_AttendceDB new_attendcebl;
        public New_AttendceBL()
        {
            new_attendcebl = new New_AttendceDB();
        }


        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        public List<New_CourseOrderDetail> GetNewAttendUserList(int courseId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list=new_attendcebl.GetNewAttendUserList(courseId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 扩展GetNewAttendUserList方法 增加讲师ID和姓名
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrderDetail> GetNewAttendList(int courseId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = new_attendcebl.GetNewAttendList(courseId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 新增考勤
        /// </summary>
        /// <param name="model"></param>
        public int AddNewAttend(New_Attendce model)
        {
            new_attendcebl.InsertNew_Attendce(model);
            return model.Id;
        }

        /// <summary>
        /// 修改考勤
        /// </summary>
        /// <param name="model">考勤信息</param>
        public bool UpdateNewAttend(New_Attendce model)
        {
            return new_attendcebl.UpdateNew_Attendce(model);
        }

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_Attendce GetNew_Attendce(int courseID, int userID, int subCourseID)
        {
            return new_attendcebl.GetNew_Attendce(courseID, userID, subCourseID);
        }

        /// <summary>
        /// 获取考勤列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_Attendce> GetList(string where = "1=1")
        {
            return new_attendcebl.GetList(where);
        }

        /// <summary>
        /// 获得我的考勤数据列表
        /// </summary>
        public List<New_CourseOrderDetail> GetMyNewAttendList(int userID, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "ncod.CourseId desc", string where = "1=1")
        {
            var list = new_attendcebl.GetMyNewAttendList(userID, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }
    }
}
