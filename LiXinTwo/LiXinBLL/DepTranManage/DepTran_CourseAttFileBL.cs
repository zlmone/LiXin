using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DepTranAttendce;
using LiXinModels.DepTranAttendce;
using LiXinInterface.DepTranManage;

namespace LiXinBLL.DepTranManage
{
    public class DepTran_CourseAttFileBL : IDepTran_CourseAttFile
    {
        private static DepTran_CourseAttFileDB dep_CoAttFilebl;
        public DepTran_CourseAttFileBL()
        {
            dep_CoAttFilebl = new DepTran_CourseAttFileDB();
        }

        /// <summary>
        /// 新增附件
        /// </summary>
        public int AddCourseAttFile(DepTran_CourseAttFile model)
        {
            dep_CoAttFilebl.InsertDepTran_CourseAttFile(model);
            return model.Id;
        }

        /// <summary>
        /// 修改附件
        /// </summary>
        public bool UpdateCourseAttFile(DepTran_CourseAttFile model)
        {
            return dep_CoAttFilebl.UpdateDepTran_CourseAttFile(model);
        }

        /// <summary>
        /// 获取单条附件信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public DepTran_CourseAttFile GetCourseAttFile(int courseID, int departId)
        {
            return dep_CoAttFilebl.GetDepTran_CourseAttFile(courseID, departId);
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<DepTran_CourseAttFile> GetDepTran_CourseAttFileDownload(int courseID, int departID,
                                                                            string where = " IsDelete=1 ")
        {
            return dep_CoAttFilebl.GetDepTran_CourseAttFileDownload(courseID, departID, where);
        }
    }
}
