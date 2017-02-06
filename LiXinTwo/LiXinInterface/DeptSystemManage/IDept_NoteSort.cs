using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;

namespace LiXinInterface
{
    public interface IDept_NoteSort
    {
        /// <summary>
        /// 判断类别名称是否存在
        /// </summary>
        /// <param name="courseSystemName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool IsExist(int departId, string sortName, int type, int Id = 0, int ParentID = 0);

        /// <summary>
        ///     增加一条数据
        /// </summary>
        void AddSys_NoteSort(Sys_NoteSort model);

        /// <summary>
        ///     更新一条数据
        /// </summary>
        bool UpdateSys_NoteSort(Sys_NoteSort model);

        /// <summary>
        ///     删除一条数据
        /// </summary>
        bool DeleteSys_NoteSort(int Id);

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
        Sys_NoteSort GetSys_NoteSort(int departId, int Id);

        /// <summary>
        ///     获得数据列表
        /// </summary>
        List<Sys_NoteSort> GetSys_NoteSortList(string strWhere = "1=1");

        /// <summary>
        /// 是否有下级
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        int IsHaveChild(int parentID);

    }
}
