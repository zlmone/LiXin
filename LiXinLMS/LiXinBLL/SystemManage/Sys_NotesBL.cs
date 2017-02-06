﻿using System;
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

        /// <summary>
        /// 获取最新的公告
        /// </summary>
        /// <returns></returns>
        public List<Sys_Notes> GetLastNote()
        {
            return noteDB.GetLastNote();
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
            var list = noteDB.GetListNote(startIndex, startLength, where,orderSql);
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
    }
}