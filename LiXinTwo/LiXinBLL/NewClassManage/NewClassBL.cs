using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.NewClassManage;
using LiXinInterface.NewClassManage;
using LiXinModels.User;
using LixinModels.NewClassManage;

namespace LiXinBLL.NewClassManage
{
    public class NewClassBL : INewClass
    {
        private static readonly NewClassDB newClassDB = new NewClassDB();

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public New_Class GetModel(int id)
        {
            return newClassDB.GetModel(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        public int AddModel(New_Class model)
        {
            newClassDB.AddModel(model);
            return model.Id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(New_Class model)
        {
            return newClassDB.UpdateModel(model);
        }


      /// <summary>
      /// 批量删除班级连带删除组和人数据
      /// </summary>
      /// <param name="ids">ID</param>
      /// <returns>成功返回True，失败返回False</returns>
      public bool DeleteBatchModel(string ids)
      {
          return newClassDB.DeleteBatchModel(ids);
      }

        /// <summary>
      /// 点击结束分班将班级状态改为不可删除， 并将班级下的员工学号设为固定不可更改
        /// </summary>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateClassToNotDelete()
        {
            return newClassDB.UpdateClassToNotDelete();
        }

      /// <summary>
      /// 根据年限获得班级列表
      /// </summary>
      public List<New_Class> GetList(string where = "")
      {
          return newClassDB.GetList(where);
      }

       

        /// <summary>
        /// 获取课程信息集合
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">当前页</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns></returns>
        public List<New_Class> GetClassList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                            int pageLength = int.MaxValue, string orderBy = "ORDER BY Id asc")
        {
            return newClassDB.GetClassList(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取班级信息集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<New_Class> GetClassList(string where = " 1 = 1 ")
        {
            return newClassDB.GetClassList(where);
        }

        /// <summary>
        /// 根据年限获得将要插入班级的班号索引
        /// </summary>
        public int GetCurrentIndex(int year)
        {
            return newClassDB.GetCurrentIndex(year);
        }

        /// <summary>
        /// 当前班组列表
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<New_Class> GetCurrentClassList(out int totalcount, string year, int startIndex = 1, int startLength = int.MaxValue, string orderBy = "")
        {
            string where = "";
            if (!string.IsNullOrWhiteSpace(year))
            {
                where += string.Format(" and c.Year={0} ", year);
            }
            var list = newClassDB.GetCurrentClassList(where, startIndex, startLength, orderBy);
            totalcount = list.Count > 0 ? list[0].totalCount : 0;
            return list;
        }

        /// <summary>
        /// 获得指定班级人员列表
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<Sys_User> GetClassPersonsList(out int totalcount, int classId, string where = "", int startIndex = 1, int startLength = int.MaxValue, string orderBy = "")
        {

            var list = newClassDB.GetClassPersonsList(classId, where, startIndex, startLength, orderBy);
            totalcount = list.Count > 0 ? list[0].totalCount : 0;
            return list;
        }

        /// <summary>
        /// 根据年限获得已分班、未分班人数，已分班级数
        /// </summary>
        public Dictionary<string, int> GetClassAndPersonCount(int year)
        {
            return newClassDB.GetClassAndPersonCount(year);
        }
    }
}
