using System.Collections.Generic;
using LiXinDataAccess.DepAttendce;
using LiXinModels.DepAttendce;
using LiXinInterface.DepCourseManage;

namespace LiXinBLL.DepCourseManage
{
    public class Dep_CourseAttFileBL : IDep_CourseAttFile
    {
        private static Dep_CourseAttFileDB dep_CoAttFilebl;
        public Dep_CourseAttFileBL()
        {
            dep_CoAttFilebl = new Dep_CourseAttFileDB();
        }

        /// <summary>
        /// 新增附件
        /// </summary>
        public int AddCourseAttFile(Dep_CourseAttFile model)
        {
            dep_CoAttFilebl.InsertDep_CourseAttFile(model);
            return model.Id;
        }

        /// <summary>
        /// 修改附件
        /// </summary>
        public bool UpdateCourseAttFile(Dep_CourseAttFile model)
        {
            return dep_CoAttFilebl.UpdateDep_CourseAttFile(model);
        }

        /// <summary>
        /// 获取单条附件信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public Dep_CourseAttFile GetCourseAttFile(int courseID, int departId)
        {
            return dep_CoAttFilebl.GetDep_CourseAttFile(courseID, departId);
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<Dep_CourseAttFile> GetDepTran_CourseAttFileDownload(int courseID, int departID,
                                                                            string where = " IsDelete=1 ")
        {
            return dep_CoAttFilebl.GetDep_CourseAttFileDownload(courseID, departID, where);
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFile(int id)
        {
            dep_CoAttFilebl.DeleteFile(id);
        }
    }
}
