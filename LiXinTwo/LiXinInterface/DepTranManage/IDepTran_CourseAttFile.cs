using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepTranAttendce;

namespace LiXinInterface.DepTranManage
{
    public interface IDepTran_CourseAttFile
    {
        /// <summary>
        /// 新增附件
        /// </summary>
        int AddCourseAttFile(DepTran_CourseAttFile model);

        /// <summary>
        /// 修改附件
        /// </summary>
        bool UpdateCourseAttFile(DepTran_CourseAttFile model);

        /// <summary>
        /// 获取单条附件信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        DepTran_CourseAttFile GetCourseAttFile(int courseID, int departId);

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<DepTran_CourseAttFile> GetDepTran_CourseAttFileDownload(int courseID, int departID, string where = " 1=1");
    }
}
