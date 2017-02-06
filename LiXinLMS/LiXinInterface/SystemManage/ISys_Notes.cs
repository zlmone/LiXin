using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;

namespace LiXinInterface
{
    public interface ISys_Notes
    {
        /// <summary>
        /// 获取最新的公告
        /// </summary>
        /// <returns></returns>
        List<Sys_Notes> GetLastNote();

        /// <summary>
        /// 获取政策
        /// </summary>
        /// <param name="totalcount">总页数</param>
        /// <param name="startIndex">开始的页数（从1开始）</param>
        /// <param name="startLength">每页的大小</param>
        /// <param name="where">查询字段</param>
        /// <param name="orderSql">排序条件</param>
        /// <returns></returns>
        List<Sys_Notes> GetListNote(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string orderSql = " ORDER BY IsTop DESC ,sn.CreateTime DESC ");

        /// <summary>
        /// 增加政策
        /// </summary>
        /// <param name="model">包含信息的model类</param>
        void AddNote(Sys_Notes model);


        /// <summary>
        /// 更新政策
        /// </summary>
        /// <param name="model"></param>
        void updateNote(Sys_Notes model);


        List<Sys_Notes> GetListNote(string where = "1=1");

        void PublishNote(int noteID);

        void DeleteNote(string noteID);

        void UpdateNumber(int noteID);
    }
}
