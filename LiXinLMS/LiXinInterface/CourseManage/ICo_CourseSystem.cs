using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;
using LiXinModels.CourseManage;
namespace LiXinInterface.CourseManage
{
    public interface ICo_CourseSystem
    {

       /// <summary>
       /// 判断体系名称是否存在
       /// </summary>
       /// <param name="courseSystemName"></param>
       /// <param name="Id"></param>
       /// <returns></returns>
        bool IsExist(string courseSystemName, int ParentID, int Id = 0);

        /// <summary>
        ///     增加一条数据
        /// </summary>
       void AddCourseSystem(Co_CourseSystem model);

        /// <summary>
        ///     更新一条数据
        /// </summary>
       bool UpdateCourseSystem(Co_CourseSystem model);

        /// <summary>
        ///     删除一条数据
        /// </summary>
       bool DeleteCourseSystem(int Id);

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
       Co_CourseSystem GetCourseSystem(int Id);

        /// <summary>
        ///     获得数据列表
        /// </summary>
       List<Co_CourseSystem> GetCo_CourseSystemList(string strWhere = "1=1");
    }
}
