using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;
using LiXinInterface.CourseManage;
using LiXinDataAccess;

using LiXinModels.CourseManage;


namespace LiXinBLL.CourseManage
{
    public class Co_CourseSystemBL : ICo_CourseSystem
    {
        private readonly Co_CourseSystemDB _courseSysDB = new Co_CourseSystemDB();

       /// <summary>
       /// 判断体系名称是否存在
       /// </summary>
       /// <param name="courseSystemName"></param>
       /// <param name="Id"></param>
       /// <returns></returns>
        public bool IsExist(string courseSystemName, int ParentID, int Id = 0)
        {
            return _courseSysDB.IsExist(courseSystemName,ParentID, Id);
        }
        /// <summary>
        ///     增加一条数据
        /// </summary>
        public void AddCourseSystem(Co_CourseSystem model)
        {
            _courseSysDB.AddCourseSystem(model);
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        public bool UpdateCourseSystem(Co_CourseSystem model)
        {
            return _courseSysDB.UpdateCourseSystem(model);
        }

        /// <summary>
        ///     删除一条数据
        /// </summary>
        public bool DeleteCourseSystem(int Id)
        {
            return _courseSysDB.DeleteCourseSystem(Id);
        }

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
        public Co_CourseSystem GetCourseSystem(int Id)
        {
            return _courseSysDB.GetCourseSystem(Id);
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Co_CourseSystem> GetCo_CourseSystemList(string strWhere = "1=1")
        {
            return _courseSysDB.GetCo_CourseSystemList(strWhere);
        }
    }
}
