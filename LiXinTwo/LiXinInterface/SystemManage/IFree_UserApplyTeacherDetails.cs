using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.SystemManage
{
    public interface IFree_UserApplyTeacherDetails
    {


        bool AddFree_UserApplyTeacherDetails(Free_UserApplyTeacherDetails model);

        bool UpdateFree_UserApplyTeacherDetails(Free_UserApplyTeacherDetails model);

        bool DeleteFree_UserApplyTeacherDetails(string id);

        List<Free_UserApplyTeacherDetails> GetFree_UserApplyTeacherDetailsList(string where = " 1 = 1 ");


        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void InsertFree_BatchTeacherDetails(Free_BatchTeacherDetails model);

        /// <summary>
        /// 得到一个对象实体 
        /// </summary>
        List<Free_BatchTeacherDetails> GetFree_BatchTeacherDetails(string where = "1=1");

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        void DeleteFree_BatchTeacherDetails(string IDlist);

    }
}
