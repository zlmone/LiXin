using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess;
using LiXinInterface;
using LiXinModels;

namespace LiXinBLL
{
    public class Sys_NotesBL : ISys_Notes
    {
        private Sys_NotesDB noteDB;
        public Sys_NotesBL()
        {
            noteDB = new Sys_NotesDB();
        }

        #region 公告
        /// <summary>
        /// 获取最新的公告
        /// </summary>
        /// <returns></returns>
        public List<Sys_Notes> GetLastNote(string where = "1=1")
        {
            return noteDB.GetLastNote(where);
        }

        /// <summary>
        /// 获取政策
        /// </summary>
        /// <param name="totalcount">总页数</param>
        /// <param name="startIndex">开始的页数（从1开始）</param>
        /// <param name="startLength">每页的大小</param>
        /// <param name="where">查询字段</param>
        /// <param name="orderSql">排序条件</param>
        /// <returns></returns>
        public List<Sys_Notes> GetListNote(out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string orderSql = " ORDER BY IsTop DESC ,sn.CreateTime DESC ")
        {
            var list = noteDB.GetListNote(startIndex, startLength, where, orderSql);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }
        public List<Sys_Notes> GetListNote(string where = "1=1")
        {
            return noteDB.GetListNote(where);
        }

        /// <summary>
        /// 增加政策
        /// </summary>
        /// <param name="model"></param>
        public void AddNote(Sys_Notes model)
        {
            noteDB.AddNote(model);
        }

        /// <summary>
        /// 更新政策
        /// </summary>
        /// <param name="model"></param>
        public void updateNote(Sys_Notes model)
        {
            noteDB.updateNote(model);
        }

        public void PublishNote(int noteID)
        {
            noteDB.PublishNote(noteID);
        }

        public void DeleteNote(string noteID)
        {
            noteDB.DeleteNote(noteID);
        }

        public void UpdateNumber(int noteID)
        {
            noteDB.UpdateNumber(noteID);
        }

        /// <summary>
        /// 是否首页显示
        /// </summary>
        /// <param name="NoteID"></param>
        /// <param name="AdFlag">0撤销 1显示></param>
        /// <returns></returns>
        public void UpdateAdFlag(int noteID, int AdFlag)
        {
            noteDB.UpdateAdFlag(noteID, AdFlag);
        }
        #endregion

        #region 公告统计

        /// <summary>
        /// 新增下载附件的信息
        /// </summary>     
        public void InsertSys_MyNoteDowmLoad(Sys_MyNoteDowmLoad model)
        {
            noteDB.InsertSys_MyNoteDowmLoad(model);
        }

        /// <summary>
        /// 新增浏览记录
        /// </summary>     
        public void InsertSys_MyNoteLook(Sys_MyNoteLook model)
        {
            noteDB.InsertSys_MyNoteLook(model);
        }

        /// <summary>
        /// 获取最新的5个公告
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Sys_Notes> GetTop5Note(int userID, string TrainGrade)
        {
            return noteDB.GetTop5Note(userID, TrainGrade);
        }

        /// <summary>
        /// 公告统计
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_Notes> GetNoteReport(out int totalCount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " publishtime desc")
        {
            var list = noteDB.GetNoteReport(startIndex, startLength, where, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 公告统计详情
        /// </summary>
        /// <param name="NoteID"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_Notes> GetNoteReportDetails(out int totalCount, int NoteID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1")
        {
            var list = noteDB.GetNoteReportDetails(NoteID, startIndex, startLength, where);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }


        /// <summary>
        ///  获取所有的背景图片
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<sys_NoteBackImage> GetImageList(out int totalCount, int startIndex = 0, int startLength = int.MaxValue, string where = " 1=1")
        {
            var list = noteDB.GetImageList(startIndex, startLength, where);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void Insertsys_NoteBackImage(sys_NoteBackImage model)
        {
            noteDB.Insertsys_NoteBackImage(model);
        }

        public List<sys_NoteBackImage> GetImageSingle(string where = " 1=1")
        {
            return noteDB.GetImageSingle(where);
        }

        /// <summary>
        /// 设置背景图片的用途
        /// </summary>
        public void UpdateImageType(int id, int type)
        {

            noteDB.UpdateImageType(id, type);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void DeteteImage(int id)
        {
            noteDB.DeteteImage(id);
        }

        /// <summary>
        /// 是否置顶
        /// </summary>
        /// <param name="isTop"></param>
        /// <param name="NoteID"></param>
        public void UpdateTopShow(int isTop, int NoteID)
        {
            string sql = " isTopShow=" + isTop + ", TopTime=getdate()";
            if (isTop==0)
            {
                sql = " isTopShow=0, TopTime=NULL";
            }
            noteDB.UpdateTopShow(sql, NoteID);
        }
        #endregion
    }
}
