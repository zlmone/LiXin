using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;
using Retech.Data;
using System.Data;
using Retech.Core;
using LiXinModels.User;

namespace LiXinDataAccess.DeptSurvey
{
    public class Survey_SurveyDB : BaseRepository
    {
        #region 调查管理

        /// <summary>
        /// 单个查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Survey_Survey Get(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT ss.*,sex.ExamTitle FROM Dep_Survey_Survey ss
LEFT JOIN Dep_Survey_Exampaper sex ON ss.PaperID=sex.ExampaperID
WHERE Id={0}", id);
                return conn.Query<Survey_Survey>(sql).FirstOrDefault();
            }
        }
        /// <summary>
        /// 查询调查管理的基本信息
        /// </summary>
        public List<Survey_Survey> GetSurveyList(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " PublishTime desc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY {1}) number,count(*)OVER(PARTITION BY null) totalcount,
* FROM Dep_Survey_Survey WHERE IsDelete=0 AND {0}) result
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Survey_Survey>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 插入调查
        /// </summary>
        /// <param name="model"></param>
        public void InsertSurvey_Survey(Survey_Survey model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"INSERT INTO dbo.Dep_Survey_Survey
	                                    (
	                                    Name,
	                                    PaperID,
	                                    Memo,
	                                    OpenGroupFlag,
	                                    OpenGroup,
	                                    OpenDepartFlag,
	                                    OpenDepart,
	                                    PublishFlag,
	                                    LastUpdateTime,
	                                    UserID,
	                                    StartTime,
	                                    EndTime,
	                                    IsDelete,
                                        DeptID
	                                    )
                                    VALUES 
	                                    (
	                                    @name,
	                                    @paperid,
	                                    @memo,
	                                    @opengroupflag,
	                                    @opengroup,
	                                    @opendepartflag,
	                                    @opendepart,
	                                    @publishflag,
	                                    getdate(),
	                                    @userid,
	                                    @starttime,
	                                    @endtime,
	                                    0,
                                        @DeptID
	                                    );SELECT @@IDENTITY AS ID");
                var param = new
                {
                    name = model.Name,
                    paperid = model.PaperID,
                    memo = model.Memo,
                    opengroupflag = model.OpenGroupFlag,
                    opengroup = model.OpenGroup,
                    opendepartflag = model.OpenDepartFlag,
                    opendepart = model.OpenDepart,
                    publishflag = model.PublishFlag,
                    userid = model.UserID,
                    starttime = model.StartTime,
                    endtime = model.EndTime,
                    model.DeptID
                };
                dynamic list = conn.Query<dynamic>(sql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.ID);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        public void UpdateSurvey_Survey(Survey_Survey model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"
UPDATE dbo.Dep_Survey_Survey
SET 
	Name = @name,
	PaperID = @paperid,
	Memo = @memo,
	OpenGroupFlag = @opengroupflag,
	OpenGroup = @opengroup,
	OpenDepartFlag = @opendepartflag,
	OpenDepart = @opendepart,
	PublishFlag = @publishflag,
	LastUpdateTime = getdate(),
	UserID = @userid,
	StartTime = @starttime,
	EndTime = @endtime,
	IsDelete = 0
WHERE Id = @id");
                var param = new
                {
                    Id = model.Id,
                    isdelete = model.IsDelete,
                    name = model.Name,
                    paperid = model.PaperID,
                    memo = model.Memo,
                    opengroupflag = model.OpenGroupFlag,
                    opengroup = model.OpenGroup,
                    opendepartflag = model.OpenDepartFlag,
                    opendepart = model.OpenDepart,
                    publishflag = model.PublishFlag,
                    userid = model.UserID,
                    starttime = model.StartTime,
                    endtime = model.EndTime,
                };
                conn.Execute(sql, param);
            }
        }

        /// <summary>
        /// 发布或者删除。。。去除冗余字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="where"></param>
        public void PublishOrDelete(int id, string where)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Dep_Survey_Survey
SET {1}
WHERE Id={0}", id, where);
                conn.Execute(sql);
            }
        }
        #endregion

        #region 参与需求调查
        /// <summary>
        ///  参与需求调查首页
        /// </summary>
        /// <returns></returns>
        public List<Survey_Survey> GetDoSurveyList(int userID, string where = " 1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT ss.*,isnull(ssu.Score,0) Score,isnull(ssu.IsAccessed,0) IsAccessed,
IsDo=(SELECT COUNT(*) FROM Dep_Survey_ReplyAnswer WHERE ExampaperID=ss.PaperID AND ObjectID=ss.Id AND SourceFrom=1 and UserID=@userID),
IsAccessed=(SELECT COUNT(*) FROM Dep_Survey_ReplyAnswer WHERE ExampaperID=ss.PaperID AND ObjectID=ss.Id AND SourceFrom=1 
AND (IsDeptAccessed=1 OR IsOfficeAccessed=2)),
su.Realname,su.DeptName
FROM Dep_Survey_Survey ss
LEFT JOIN Dep_Survey_SurveyUser ssu ON ss.Id=ssu.SurveyID
LEFT JOIN Sys_User su ON su.UserId= ss.UserID
WHERE ((SELECT count(*) FROM dbo.F_SplitIDs(OpenGroup) WHERE ID IN (SELECT GroupId FROM Sys_GroupUser
WHERE UserId=@userID))>0 OR (SELECT count(*) FROM dbo.F_SplitIDs(OpenDepart) WHERE ID IN (SELECT DeptId FROM Sys_User
WHERE UserId=@userID))>0) and ss.PublishFlag=1 AND ss.IsDelete=0 
and {0}", where);
                var param = new
                {
                    userID = userID
                };
                return conn.Query<Survey_Survey>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取部门负责人所在的部门ID
        /// </summary>
        /// <returns></returns>
        public int GetDeptID(int userID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT DeptId FROM Sys_User
WHERE JobNum=(
SELECT LeaderID FROM Sys_User
WHERE UserId={0})", userID);
                return conn.Query<int>(sql).FirstOrDefault();

            }
        }
        #endregion

        #region 问卷汇总
        /// <summary>
        /// 问卷汇总
        /// </summary>
        /// <param name="userID">发布人ID</param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <param name="jsRenderSortField">排序</param>
        public List<Survey_Survey> GetSurveyReport(int userID, int DeptID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id ")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from(
SELECT  row_number()OVER(ORDER BY {0} ) number,count(*)OVER(PARTITION BY null) totalCount,  *
 FROM (SELECT  * ,
useNum=(SELECT count(DISTINCT userID) FROM Dep_Survey_ReplyAnswer WHERE ObjectID=ss.Id AND SourceFrom=1
and IsMaster=0)
FROM Dep_Survey_Survey ss
WHERE PublishFlag=1 AND ss.UserID={2} and  {1}) r
)t
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", jsRenderSortField, where, userID, DeptID);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Survey_Survey>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 树
        /// </summary>
        /// <returns></returns>
        public List<Sys_Department> GetDeptForDepartment(int surveyID, int paperID, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
     DECLARE @OpenDepart NVARCHAR(max) 
 SELECT @OpenDepart=OpenDepart FROM Dep_Survey_Survey WHERE Id=@surveyID
SELECT sd.*,
IsSubmit=(SELECT count(*)  FROM Dep_Survey_ReplyAnswer
WHERE DeptID= sd.DepartmentId
AND SourceFrom=1 AND ObjectID=@surveyID AND ExampaperID=@paperID
)FROM Sys_Department sd
where {0}", where);
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID
                };
                return conn.Query<Sys_Department>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 根据部门获取问题
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        public List<Survey_Question> GetQuestionSurveyByDeptID(int surveyID, int paperID, int DeptID)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT *,
useNum=(SELECT count(DISTINCT userID) FROM Dep_Survey_ReplyAnswer
WHERE SourceFrom=1 AND ObjectID=@surveyID  AND ExampaperID=@paperID AND DeptID=@DeptID AND QuestionID=sq.QuestionID 
)
FROM Dep_Survey_Question sq WHERE ExampaperID=@paperID
AND sq.QuestionID in (SELECT  DISTINCT QuestionID FROM  Dep_Survey_ReplyAnswer WHERE SourceFrom=1 AND ObjectID=@surveyID
AND ExampaperID=@paperID)
";
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID,
                    DeptID = DeptID
                };
                return conn.Query<Survey_Question>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取答案详情
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetOfficeReplayAnswer(int surveyID, int paperID, int questionID, int userID, int deptID, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT *,dbo.GetRealNameByUserID(userID) stuName FROM Dep_Survey_ReplyAnswer
WHERE SourceFrom=1 AND ObjectID=@surveyID  AND ExampaperID=@paperID AND DeptID= @deptID
AND QuestionID=@questionID and {0}", where);
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID,
                    questionID = questionID,
                    userID = userID,
                    deptID = deptID
                };
                return conn.Query<Survey_ReplyAnswer>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 星评题的分数,汇总
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetXinPAnswersOffice(int QuestionID, int paperID, int surveyID, int DeptID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT  *,dbo.GetRealNameByUserID(UserID) stuName  FROM Dep_Survey_ReplyAnswer
WHERE SourceFrom=1 AND QuestionID={0} AND ExampaperID={1} AND ObjectID={2}  and UserId IN (SELECT UserId FROM Sys_User
WHERE DeptId={3})
", QuestionID, paperID, surveyID, DeptID);
                return conn.Query<Survey_ReplyAnswer>(sql).ToList();

            }
        }

        /// <summary>
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Sys_User> GetStudentByDept(int surveyID, int paperID, int deptID, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
    DECLARE @OpenDepart NVARCHAR(max) 
 SELECT @OpenDepart=OpenDepart FROM Dep_Survey_Survey WHERE Id=@surveyID

SELECT UserId, Realname, DeptId, DeptCode, TrainGrade FROM Sys_User
WHERE UserId IN (
SELECT  userID FROM Dep_Survey_ReplyAnswer
WHERE SourceFrom=1 AND ObjectID=@surveyID  AND ExampaperID=@paperID) and {0}", where);
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 学员的答案
        /// </summary>
        public List<Survey_ReplyAnswer> GetAnswerForStu(int surveyID, int paperID, int userID, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM
Dep_Survey_ReplyAnswer sr
WHERE SourceFrom=1 and ObjectID=@surveyID AND sr.ExampaperID=@paperID
and IsMaster=0
AND sr.UserID=@userID  AND IsMaster=0
 and {0}", where);
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID,
                    userID = userID
                };
                return conn.Query<Survey_ReplyAnswer>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 学员的答案的问题
        /// </summary>
        public List<Survey_Question> GetSurvey_QuestionForStu(int surveyID, int paperID, int userID, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM Dep_Survey_Question sqe
WHERE sqe.QuestionID IN (
SELECT sr.QuestionID from
Dep_Survey_ReplyAnswer sr
WHERE SourceFrom=1 and ObjectID=@surveyID AND sr.ExampaperID=@paperID
and IsMaster=0
AND sr.UserID=@userID  AND IsMaster=0 and {0} )
", where);
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID,
                    userID = userID
                };
                return conn.Query<Survey_Question>(sql, param).ToList();
            }
        }
        #endregion
    }
}
