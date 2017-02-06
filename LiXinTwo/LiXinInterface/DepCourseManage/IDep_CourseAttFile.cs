using System.Collections.Generic;
using LiXinModels.DepAttendce;

namespace LiXinInterface.DepCourseManage
{
    public interface IDep_CourseAttFile
    {
        /// <summary>
        /// 新增附件
        /// </summary>
        int AddCourseAttFile(Dep_CourseAttFile model);

        /// <summary>
        /// 修改附件
        /// </summary>
        bool UpdateCourseAttFile(Dep_CourseAttFile model);

        /// <summary>
        /// 获取单条附件信息
        /// </summary>
        Dep_CourseAttFile GetCourseAttFile(int courseID, int departId);

        /// <summary>
        /// 获取单条信息
        /// </summary>
        List<Dep_CourseAttFile> GetDepTran_CourseAttFileDownload(int courseID, int departID, string where = " IsDelete=1 ");


        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="id"></param>
        void DeleteFile(int id);
    }
}
