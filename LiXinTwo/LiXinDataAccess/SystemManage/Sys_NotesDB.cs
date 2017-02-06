using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System;
using LiXinModels.SystemManage;

namespace LiXinDataAccess
{
    public class Sys_NotesDB : BaseRepository
    {
        #region 公告
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
SELECT row_number() OVER({1}) number,count(*)OVER(PARTITION BY null) totalcount,sn.*,su.Realname FROM Sys_Notes sn,
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
                var sql = string.Format(@"SELECT * FROM Sys_Notes
WHERE IsDelete=0 and {0} {1}", where, orderSql);
                return conn.Query<Sys_Notes>(sql).ToList();
            }

        }

        /// <summary>
        /// 获取最新的公告
        /// </summary>
        /// <returns></returns>
        public List<Sys_Notes> GetLastNote(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT top(3)* FROM Sys_Notes  sn
LEFT JOIN Sys_User su on sn.UserId=su.UserId 
where PublishFlag=1 and Type=0 and sn.IsDelete=0 and {0}
order by PublishTime desc", where);
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
                string sql = @"INSERT INTO Sys_Notes
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
                                DepTrainFlag,
                                Num,
	                            OpenGroupFlag,
	                            OpenGroup,
	                            OpenDepartFlag,
	                            OpenDepart,
                                ContentDesc,
                                TrainGrade,
                                BackUrlName
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
                                @DepTrainFlag,
                                0,
                                @OpenGroupFlag,
	                            @OpenGroup,
	                            @OpenDepartFlag,
	                            @OpenDepart,
                                @ContentDesc,
                                @TrainGrade,
                                @BackUrlName
	                            )
SELECT @@IDENTITY as id";
                var param = new
                {
                    notetitle = model.NoteTitle,
                    notecontent = model.NoteContent,
                    SortID = model.SortID,
                    userid = model.UserId,
                    createtime = DateTime.Now,
                    publishtime = model.publishtime,
                    Type = model.Type,
                    AdFlag = model.AdFlag,
                    DepTrainFlag = model.DepTrainFlag,
                    OpenGroupFlag = model.OpenGroupFlag,
                    OpenGroup = model.OpenGroup,
                    OpenDepartFlag = model.OpenDepartFlag,
                    OpenDepart = model.OpenDepart,
                    ContentDesc = model.ContentDesc,
                    TrainGrade = model.TrainGrade,
                    BackUrlName = model.BackUrlName
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
                string sql = @"UPDATE Sys_Notes
                            SET NoteTitle = @notetitle,
	                            NoteContent = @notecontent,SortID=@SortID,AdFlag=@AdFlag,DepTrainFlag=@DepTrainFlag,
	                            OpenGroupFlag = @opengroupflag,OpenGroup = @opengroup,OpenDepartFlag = @opendepartflag,
                                OpenDepart = @opendepart,ContentDesc=@ContentDesc,TrainGrade=@TrainGrade,BackUrlName=@BackUrlName 
	                        WHERE NoteId = @noteid";
                var param = new
                {
                    notetitle = model.NoteTitle,
                    notecontent = model.NoteContent,
                    SortID = model.SortID,
                    noteid = model.NoteId,
                    AdFlag = model.AdFlag,
                    DepTrainFlag = model.DepTrainFlag,
                    OpenGroupFlag = model.OpenGroupFlag,
                    OpenGroup = model.OpenGroup,
                    OpenDepartFlag = model.OpenDepartFlag,
                    OpenDepart = model.OpenDepart,
                    ContentDesc = model.ContentDesc,
                    TrainGrade = model.TrainGrade,
                    BackUrlName = model.BackUrlName
                };
                conn.Execute(sql, param);
            }
        }

        public void PublishNote(int noteID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Sys_Notes
SET PublishTime=getdate(), PublishFlag=1
WHERE NoteId={0}", noteID);
                conn.Execute(sql);
            }
        }

        public void DeleteNote(string noteID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Sys_Notes
SET  IsDelete=1
WHERE NoteId in ({0})", noteID);
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
                string sql = string.Format(@"UPDATE Sys_Notes
SET Num=Num+1
WHERE NoteId={0}", noteID);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 是否首页显示
        /// </summary>
        /// <param name="NoteID"></param>
        /// <param name="AdFlag">0撤销 1显示></param>
        /// <returns></returns>
        public void UpdateAdFlag(int noteID, int AdFlag)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"UPDATE Sys_Notes
SET AdFlag=@AdFlag
WHERE NoteId=@NoteId";
                var param = new
                {
                    NoteId = noteID,
                    AdFlag = AdFlag
                };
                conn.Execute(sql, param);
            }
        }
        #endregion

        #region 公告统计信息

        /// <summary>
        /// 新增下载附件的信息
        /// </summary>     
        public void InsertSys_MyNoteDowmLoad(Sys_MyNoteDowmLoad model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Sys_MyNoteDowmLoad(NoteID,ResourceID,UserID,DownLoadTime,IsDelete)
	                     values( @NoteID,@ResourceID,@UserID,@DownLoadTime,0);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.NoteID,
                    model.ResourceID,
                    model.UserID,
                    model.DownLoadTime
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(model.ID);
            }
        }

        /// <summary>
        /// 新增浏览记录
        /// </summary>     
        public void InsertSys_MyNoteLook(Sys_MyNoteLook model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Sys_MyNoteLook(NoteID,UserID,LookTime,IsDelete)
	                     values( @NoteID,@UserID,@LookTime,0);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.NoteID,
                    model.UserID,
                    model.LookTime
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(model.ID);
            }
        }

        /// <summary>
        /// 获取最新的5个公告
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Sys_Notes> GetTop5Note(int userID, string TrainGrade)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT TOP 5 sn.*,su.Realname FROM Sys_Notes sn LEFT JOIN 
Sys_User su on sn.UserId=su.UserId where sn.IsDelete=0  and sn.PublishFlag=1 AND sn.type=0 and sn.AdFlag=1
AND  ((OpenGroupFlag=0 AND OpenDepartFlag=0 and (charindex(@TrainGrade,sn.TrainGrade)>0))	
or
( OpenGroupFlag=1 AND charindex(@TrainGrade,sn.TrainGrade)>0 and (SELECT count(*) FROM dbo.F_SplitIDs(OpenGroup) WHERE ID IN (SELECT GroupId FROM Sys_GroupUser
WHERE UserId=@userID))>0) OR (OpenDepartFlag=1  AND charindex(@TrainGrade,sn.TrainGrade)>0 AND (SELECT count(*) FROM dbo.F_SplitIDs(OpenDepart) WHERE ID IN (SELECT DeptId FROM Sys_User
WHERE UserId=@userID))>0))  
order BY PublishTime desc";
                var param = new
                {
                    userID = userID,
                    TrainGrade = TrainGrade
                };
                return conn.Query<Sys_Notes>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 公告统计
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_Notes> GetNoteReport(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " publishtime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number() OVER(ORDER BY {1}) number,* FROM(
SELECT count(*)OVER(PARTITION BY null) totalcount,
sn.NoteId,NoteTitle,PublishTime,
LookCount=(SELECT count(DISTINCT UserID) FROM Sys_MyNoteLook WHERE NoteID=sn.NoteId),
DownCount=(SELECT count(DISTINCT UserID) FROM Sys_MyNoteDowmLoad WHERE NoteID=sn.NoteId)
FROM Sys_Notes sn
WHERE PublishFlag=1 AND Type=0 AND IsDelete=0
AND AdFlag=1 and {0}) t)r
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
  ", where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_Notes>(sql, param).ToList();

            }
        }

        /// <summary>
        /// 公告统计详情
        /// </summary>
        /// <param name="NoteID"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_Notes> GetNoteReportDetails(int NoteID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
  SELECT row_number() OVER(ORDER BY UserID ,type,LookTime) number,count(*)OVER(PARTITION BY null) totalcount,* FROM (
   SELECT sn.NoteTitle,sml.UserID,syu.Realname,syu.DeptName,syu.TrainGrade,sml.LookTime,sml.NoteId,1 type FROM Sys_MyNoteLook sml
  LEFT JOIN Sys_User syu ON syu.UserId=sml.UserID
  LEFT JOIN  Sys_Notes sn   ON sml.NoteID=sn.NoteId
  WHERE sml.NoteID={1}
  UNION
  SELECT sn.NoteTitle,smd.UserID,syu.Realname,syu.DeptName,syu.TrainGrade,DownLoadTime,smd.NoteId,2 type FROM Sys_MyNoteDowmLoad smd
  LEFT JOIN Sys_User syu ON syu.UserId=smd.UserID
  LEFT JOIN  Sys_Notes sn   ON smd.NoteID=sn.NoteId
  WHERE smd.NoteID={1}
  )t where {0})r 
 WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where, NoteID);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_Notes>(sql, param).ToList();
            }
        }

        /// <summary>
        ///  获取所有的背景图片
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<sys_NoteBackImage> GetImageList(int startIndex = 0, int startLength = int.MaxValue, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number() OVER(ORDER BY defalutType desc,CreateTime) number,count(*)OVER(PARTITION BY null) totalcount
,* FROM sys_NoteBackImage where {0}
) t WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<sys_NoteBackImage>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void Insertsys_NoteBackImage(sys_NoteBackImage model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO sys_NoteBackImage(RealName,FileName,defalutType,CreateTime)
	                     values( @RealName,@FileName,@defalutType,@CreateTime);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.RealName,
                    model.FileName,
                    model.defalutType,
                    model.CreateTime
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(model.ID);
            }
        }

        public List<sys_NoteBackImage> GetImageSingle(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"select * from sys_NoteBackImage where " + where;
                return conn.Query<sys_NoteBackImage>(sql).ToList();
            }
        }

       
        /// <summary>
        /// 设置背景图片的用途
        /// </summary>
        public void UpdateImageType(int id, int type)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
UPDATE dbo.sys_NoteBackImage
SET defalutType = 0

UPDATE dbo.sys_NoteBackImage
SET defalutType = {0}
WHERE ID={1}", type, id);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void DeteteImage(int id)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"delete from sys_NoteBackImage where ID=" + id;
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 是否置顶
        /// </summary>
        /// <param name="isTop"></param>
        /// <param name="NoteID"></param>
        public void UpdateTopShow(string update, int NoteID)
        {
            using (var conn=OpenConnection())
            {
                var sql =string.Format(@"UPDATE Sys_Notes
SET {0}
WHERE NoteId={1}", update,NoteID);
                conn.Execute(sql);
            }
        }
        #endregion
    }
}
