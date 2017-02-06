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
        List<Sys_Notes> GetLastNote(string where = "1=1");

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

        /// <summary>
        /// 是否首页显示
        /// </summary>
        /// <param name="NoteID"></param>
        /// <param name="AdFlag">0撤销 1显示></param>
        /// <returns></returns>
        void UpdateAdFlag(int noteID, int AdFlag);

        /// <summary>
        /// 新增下载附件的信息
        /// </summary>     
        void InsertSys_MyNoteDowmLoad(Sys_MyNoteDowmLoad model);

        /// <summary>
        /// 新增浏览记录
        /// </summary>     
        void InsertSys_MyNoteLook(Sys_MyNoteLook model);

        /// <summary>
        /// 获取最新的5个公告
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Sys_Notes> GetTop5Note(int userID, string TrainGrade);

        /// <summary>
        /// 公告统计
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Sys_Notes> GetNoteReport(out int totalCount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " publishtime desc");

        /// <summary>
        /// 公告统计详情
        /// </summary>
        /// <param name="NoteID"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Sys_Notes> GetNoteReportDetails(out int totalCount, int NoteID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1");

        /// <summary>
        ///  获取所有的背景图片
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        List<sys_NoteBackImage> GetImageList(out int totalCount, int startIndex = 0, int startLength = int.MaxValue, string where = " 1=1");

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void Insertsys_NoteBackImage(sys_NoteBackImage model);

        List<sys_NoteBackImage> GetImageSingle(string where = " 1=1");


        /// <summary>
        /// 设置背景图片的用途
        /// </summary>
        void UpdateImageType(int id, int type);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        void DeteteImage(int id);

        /// <summary>
        /// 是否置顶
        /// </summary>
        /// <param name="isTop"></param>
        /// <param name="NoteID"></param>
        void UpdateTopShow(int isTop, int NoteID);
    }
}
