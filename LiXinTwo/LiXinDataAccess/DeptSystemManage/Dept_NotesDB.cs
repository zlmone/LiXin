using System.Collections.Generic;
using System.Data;
using System.Linq;
using LiXinModels.SystemManage;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System;

namespace LiXinDataAccess
{
    public class Dept_NotesDB : BaseRepository
    {
        /// <summary>
        /// 获取政策
        /// </summary>
        /// <param name="startIndex">开始的页数（从1开始）</param>
        /// <param name="startLength">每页的大小</param>
        /// <param name="where">查询字段</param>
        /// <param name="orderSql">排序条件</param>
        /// <returns></returns>
        public List<Sys_Notes> GetListNote(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string orderSql = " ORDER BY IsTop DESC ,sn.CreateTime DESC ")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT * FROM 
(
SELECT row_number() OVER({1}) number,count(*)OVER(PARTITION BY null) totalcount,sn.*,su.Realname FROM Dep_Notes sn,
Sys_User su WHERE sn.UserId=su.UserId and sn.IsDelete=0 {0}
)  result
WHERE  result.number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
  ", where, orderSql);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_Notes>(sql, param).ToList();
            }
        }

        public List<Sys_Notes> GetListNote(string where = "1=1", string orderSql = " order by PublishTime desc ")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM Dep_Notes
WHERE IsDelete=0 and {0} {1}", where, orderSql);
                return conn.Query<Sys_Notes>(sql).ToList();
            }

        }

        /// <summary>
        /// 获取最新的公告
        /// </summary>
        /// <returns></returns>
        public List<Sys_Notes> GetLastNote()
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT top(3)* FROM Dep_Notes WHERE PublishFlag=1 and Type=0 and IsDelete=0 order by PublishTime desc");
                return conn.Query<Sys_Notes>(sql).ToList();
            }

        }

        /// <summary>
        /// 增加政策
        /// </summary>
        /// <param name="model"></param>
        public void AddNote(Sys_Notes model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"INSERT INTO Dep_Notes
	                            (
	                            NoteTitle,
	                            NoteContent,
                                SortID,
	                            UserId,    
	                            CreateTime,
                                publishflag,
                                publishtime,
	                            IsDelete,
                                Type,
                                AdFlag,
                                Num,
                                DeptId
	                            )
                            VALUES 
	                            (
	                            @notetitle,
	                            @notecontent,
                                @SortID,
	                            @userid,
	                            @createtime,
                                0,
                                @publishtime,
	                            0,
                                @Type,
                                @AdFlag,
                                0,
                                @DeptId
	                            )
SELECT @@IDENTITY as id";
                var param = new
                {
                    notetitle = model.NoteTitle,
                    notecontent = model.NoteContent,
                    SortID=model.SortID,
                    userid = model.UserId,
                    createtime = DateTime.Now,
                    publishtime = model.publishtime,
                    Type=model.Type,
                    AdFlag=model.AdFlag,
                    DeptId=model.DeptId
                };
                decimal id = conn.Query<decimal>(sql, param).FirstOrDefault();
                model.NoteId = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 更新政策
        /// </summary>
        /// <param name="model"></param>
        public void updateNote(Sys_Notes model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @" UPDATE Dep_Notes
                            SET NoteTitle = @notetitle,
	                            NoteContent = @notecontent,SortID=@SortID,AdFlag=@AdFlag 
	                        WHERE NoteId = @noteid and DeptId = @DeptId";
                var param = new
                {
                    notetitle = model.NoteTitle,
                    notecontent = model.NoteContent,
                    SortID=model.SortID,
                    noteid = model.NoteId,
                    AdFlag=model.AdFlag,
                    DeptId=model.DeptId
                };
                conn.Execute(sql, param);
            }
        }

        public void PublishNote(int noteID,int departId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Dep_Notes
SET PublishTime=getdate(), PublishFlag=1
WHERE NoteId={0} and DeptId={1}", noteID,departId);
                conn.Execute(sql);
            }
        }

        public void DeleteNote(string noteID, int departId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Dep_Notes
SET  IsDelete=1
WHERE NoteId in ({0}) and DeptId={1}", noteID,departId);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 修改FAQ浏览次数
        /// </summary>
        /// <param name="noteID"></param>
        public void UpdateNumber(int noteID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Dep_Notes
SET Num=Num+1
WHERE NoteId={0}", noteID);
                conn.Execute(sql);
            }
        }

        public List<Sys_NoteResource> GetNoteResourceNote(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT * FROM Sys_NoteResource
WHERE IsDelete=0 and {0}", where);
                return conn.Query<Sys_NoteResource>(sql).ToList();
            }

        }

    }
}
