using LiXinDataAccess.NewCourseManage;
using LiXinInterface.NewCourseManage;
using LiXinModels.NewCourseManage;
using System.Collections.Generic;

namespace LiXinBLL.NewCourseManage
{
    public class New_CourseBL :INew_Course
    {
        private static New_CourseDB new_coursebl;
        public New_CourseBL()
        {
            new_coursebl = new New_CourseDB();
        }

        public List<New_Course> GetNew_CourseList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC")
        {
            return new_coursebl.GetNew_CourseList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取考勤列表(课程管理)
        /// </summary>
        /// <returns></returns>
        public List<New_Course> GetNewCourseList(int way, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "LastUpdateTime desc", string where = "1=1")
        {
            var list = new_coursebl.GetCourseList(way, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        public List<New_Course> GetNewAllScoreManager(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                            int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC")
        {
            var list= new_coursebl.GetNewAllScoreManager( where, startIndex, pageLength, orderBy);
            totalCount = list.Count == 0 ? 0 : list[0].totalcount;
            return list;
        }

        /// <summary>
        /// 获取单个课程信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_Course GetSingleCourse(int courseID,string wherestr = "1=1")
        {
            return new_coursebl.GetSingleCourse(courseID, wherestr);
        }

        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="model"></param>
        public void AddCourse(New_Course model)
        {
            new_coursebl.AddCourse(model);
        }

        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="model">课程信息</param>
        public bool UpdateCourse(New_Course model)
        {
            return new_coursebl.UpdateCourse(model);
        }

        /// <summary>
        /// 验证课程编号是否重名
        /// </summary>
        /// <param name="courseCode"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExistCourseCode(string courseCode,int Id = 0, int way = 1)
        {
            return new_coursebl.IsExistCourseCode(courseCode,Id, way);
        }

        /// <summary>
        /// 根据课程ID 找出教室 讲师
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_Course GetCourseByCourseRoomRule(int courseID)
        {
            return new_coursebl.GetCourseByCourseRoomRule(courseID);
        }

        /// <summary>
        /// 讲师端-获取学员对我的评价列表(add by yxt 2013-07-06)
        /// <param name="totalCount">总数</param>
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<New_CourseRoomRule> GetPingByUserToTeacherList(out int totalCount, string where = "", int startIndex = 1, int startLength = int.MaxValue, string orderBy = "")
        {
            var list = new_coursebl.GetPingByUserToTeacherList(where, startIndex, startLength, orderBy);
            totalCount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }


        public List<New_Course> GetStudyList(out int totalCount, int userid, string where = " 1=1", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC")
        {
            return new_coursebl.GetStudyList(out totalCount, userid, where, startIndex, pageLength, orderBy);
        }
    }
}
