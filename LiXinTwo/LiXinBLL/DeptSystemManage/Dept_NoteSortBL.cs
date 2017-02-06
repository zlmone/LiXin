using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess;
using LiXinModels.SystemManage;
using LiXinInterface;

namespace LiXinBLL
{
    public class Dept_NoteSortBL : IDept_NoteSort
    {
        private Dept_NoteSortDB _noteSortDB = new Dept_NoteSortDB();
        /// <summary>
        /// 判断类别名称是否存在
        /// </summary>
        /// <param name="courseSystemName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExist(int departId, string sortName, int type = 0, int Id = 0, int ParentID = 0)
        {
            return _noteSortDB.IsExist(departId, sortName, type, Id, ParentID);
        }

        /// <summary>
        ///     增加一条数据
        /// </summary>
        public void AddSys_NoteSort(Sys_NoteSort model)
        {
            _noteSortDB.AddSys_NoteSort(model);
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        public bool UpdateSys_NoteSort(Sys_NoteSort model)
        {
            return _noteSortDB.UpdateSys_NoteSort(model);
        }

        /// <summary>
        ///     删除一条数据
        /// </summary>
        public bool DeleteSys_NoteSort(int Id)
        {
            return _noteSortDB.DeleteSys_NoteSort(Id);
        }

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
        public Sys_NoteSort GetSys_NoteSort(int departId, int Id)
        {
            return _noteSortDB.GetSys_NoteSort(departId, Id);
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Sys_NoteSort> GetSys_NoteSortList(string strWhere = "1=1")
        {
            return _noteSortDB.GetSys_NoteSortList(strWhere);
        }

        /// <summary>
        /// 是否有下级
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public int IsHaveChild(int parentID)
        {
            return _noteSortDB.IsHaveChild(parentID);
        }
    }
}
