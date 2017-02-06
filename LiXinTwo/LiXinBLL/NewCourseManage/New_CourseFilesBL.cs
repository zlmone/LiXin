using LiXinDataAccess.NewCourseManage;
using LiXinInterface.NewCourseManage;
using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.NewCourseManage
{
    public class New_CourseFilesBL : INew_CourseFiles
    {

        private static New_CourseFilesDB new_coursefilesbl;
         public New_CourseFilesBL()
        {
            new_coursefilesbl = new New_CourseFilesDB();
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddCourseFiles(New_CourseFiles model)
        {
            new_coursefilesbl.AddCourseFiles(model);
        }

        /// <summary>
        /// 获取单个信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_CourseFiles GetSingleCourseFile(int courseID, string wherestr = "1=1")
        {
            return new_coursefilesbl.GetSingleCourseFile(courseID, wherestr);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateCourseFiles(New_CourseFiles model)
        {
            return new_coursefilesbl.UpdateCourseFiles(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteCourseFiles(string Ids)
        {
            return new_coursefilesbl.DeleteCourseFiles(Ids);
        }

         /// <summary>
        /// 根据课程ID找出资源
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<New_CourseFiles> GetCourseResourceList(int courseId)
        {
            return new_coursefilesbl.GetCourseResourceList(courseId);
        }

        public List<New_CourseFiles> GetVedioList(int courseid, int Userid)
        {
            return new_coursefilesbl.GetVedioList(courseid, Userid);
        }


        public bool UpdateLoadTimes(int id)
        {
            return new_coursefilesbl.UpdateLoadTimes(id);
        }

    }
}
