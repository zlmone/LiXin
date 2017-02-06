using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;
using Retech.Data;
using System.Data;
using Retech.Core;
using LiXinModels.User;
namespace LiXinDataAccess.Survey
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
                var sql = string.Format(@"SELECT ss.*,sex.ExamTitle FROM Survey_Survey ss
LEFT JOIN Survey_Exampaper sex ON ss.PaperID=sex.ExampaperID
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
* FROM Survey_Survey WHERE IsDelete=0 AND {0}) result
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
                var sql = string.Format(@"INSERT INTO dbo.Survey_Survey
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
	                                    IsDelete
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
	                                    0
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
UPDATE dbo.Survey_Survey
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
                var sql = string.Format(@"UPDATE Survey_Survey
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
                string sql = string.Format(@"
SELECT ss.*,isnull(ssu.Score,0) Score,isnull(ssu.IsAccessed,0) IsAccessed,
IsDo=(SELECT COUNT(*) FROM Survey_ReplyAnswer WHERE ExampaperID=ss.PaperID AND ObjectID=ss.Id AND SourceFrom=1 and UserID=@userID
),
IsAccessed=(SELECT COUNT(*) FROM Survey_ReplyAnswer WHERE ExampaperID=ss.PaperID AND ObjectID=ss.Id AND SourceFrom=1 
AND (IsDeptAccessed=1 OR IsOfficeAccessed=2))
FROM Survey_Survey ss
LEFT JOIN Survey_SurveyUser ssu ON ss.Id=ssu.SurveyID
WHERE ((SELECT count(*) FROM dbo.F_SplitIDs(OpenGroup) WHERE ID IN (SELECT GroupId FROM Sys_GroupUser
WHERE UserId=@userID))>0 OR (SELECT count(*) FROM dbo.F_SplitIDs(OpenDepart) WHERE ID IN (SELECT DeptId FROM Sys_User
WHERE UserId=@userID))>0) and ss.PublishFlag=1 AND ss.IsDelete=0 and {0} ", where);
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

        #region 问卷审核(部门负责人)
        /// <summary>
        /// 问卷审核首页
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Survey_Survey> GetSurveyCheckForDept(int userID)
        {
            using (var conn = OpenConnection())
            {
                const string sql = @"--部门负责人下面所有的人员
with Alluser AS
(
	SELECT UserId, DeptId FROM Sys_User
	WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId=@userID)
	AND IsDelete=0
)
SELECT ss.*,
useNum=(SELECT  COUNT(DISTINCT userID) FROM Survey_ReplyAnswer WHERE ExampaperID=ss.PaperID AND ObjectID=ss.Id AND SourceFrom=1
and IsMaster=0
 and UserID IN (SELECT UserId FROM Alluser))
FROM Survey_Survey ss
WHERE (
(SELECT count(*) FROM dbo.F_SplitIDs(OpenGroup) WHERE ID IN (SELECT GroupId FROM Sys_GroupUser
WHERE UserId IN (SELECT UserId FROM Alluser)))>0 OR 
(SELECT count(*) FROM dbo.F_SplitIDs(OpenDepart) WHERE ID IN ((SELECT DeptId FROM Alluser)))>0) and ss.PublishFlag=1
AND ss.IsDelete=0";
                var param = new
                {
                    userID = userID
                };
                return conn.Query<Survey_Survey>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取问题基本情况
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public List<Survey_Question> GetQuestionBySurvey(int surveyID, int paperID, int userID)
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"
with Alluser AS
(
	SELECT UserId, DeptId FROM Sys_User
	WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId=@userID)
	AND IsDelete=0 
)

SELECT *,
useNum=(
SELECT count( DISTINCT userID) FROM Survey_ReplyAnswer
WHERE ObjectID=@surveyID AND ExampaperID=@paperID AND SourceFrom=1 AND QuestionID=suq.QuestionID
and IsMaster=0
AND UserID IN (SELECT UserId FROM Alluser)),
sumresult=(SELECT SubjectiveAnswer FROM Survey_ReplyAnswer WHERE SourceFrom=1 AND ObjectID=@surveyID
AND ExampaperID=@paperID AND QuestionID=0 AND UserID=@userID)
FROM Survey_Question suq
WHERE ExampaperID=@paperID
AND QuestionID in (SELECT  DISTINCT QuestionID FROM  Survey_ReplyAnswer WHERE SourceFrom=1 AND ObjectID=@surveyID
AND ExampaperID=@paperID) 
");
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID,
                    userID = userID
                };
                return conn.Query<Survey_Question>(sql, param).ToList();
            }
        }

        /// <summary>
        ///  获取回答的基本情况
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetReplayAnswer(int surveyID, int paperID, int questionID, int userID, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"
with Alluser AS
(
	SELECT UserId, DeptId FROM Sys_User
	WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId=@userID)
	AND IsDelete=0
)

SELECT *,
stuName=dbo.GetRealNameByUserID(userID)
FROM Survey_ReplyAnswer
WHERE ObjectID=@surveyID AND ExampaperID=@paperID AND SourceFrom=1 AND (QuestionID=@questionID or QuestionID=0)
AND UserID IN (SELECT UserId FROM Alluser)
and IsMaster=0
and {0}", where);
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID,
                    questionID = questionID,
                    userID = userID
                };
                return conn.Query<Survey_ReplyAnswer>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 部门负责人的采纳
        /// </summary>
        public void DeptAccessed(string query, int AnswerId)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE dbo.Survey_ReplyAnswer
SET {0}
WHERE AnswerId = @AnswerId", query);
                var param = new
                {
                    AnswerId = AnswerId
                };
                conn.Execute(sql, param);
            }
        }

        /// <summary>
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Sys_User> GetDeptUser(int surveyID, int paperID, int userID, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"with Alluser AS
(
	SELECT UserId, Realname, DeptId, DeptCode,dbo.GetDeptPathByDeptID(DeptId) DeptName,  TrainGrade FROM Sys_User
	WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId=@userID)
	AND IsDelete=0 
	
)

SELECT * FROM Alluser
WHERE UserId IN 
(
SELECT UserID
FROM Survey_ReplyAnswer
WHERE ObjectID=@surveyID AND ExampaperID=@paperID AND SourceFrom=1
and IsMaster=0
) and {0}", where);
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID,
                    userID = userID
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 学员的答案的问题
        /// </summary>
        public List<Survey_Question> GetSurvey_QuestionForStu(int surveyID, int paperID, int userID, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM Survey_Question sqe
WHERE sqe.QuestionID IN (
SELECT sr.QuestionID from
Survey_ReplyAnswer sr
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

        /// <summary>
        /// 学员的答案
        /// </summary>
        public List<Survey_ReplyAnswer> GetAnswerForStu(int surveyID, int paperID, int userID, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM
Survey_ReplyAnswer sr
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
        /// 维护部门状态表
        /// </summary>
        /// <param name="depID"></param>
        /// <param name="surveyID"></param>
        public void InsertSurvey_DeptSurvey(int depID, int surveyID)
        {
            using (var conn = OpenConnection())
            {
                const string sql = @"IF(SELECT COUNT(*) FROM Survey_DeptSurvey WHERE DeptID=@deptid AND SurveyID=@surveyid)=0
BEGIN
INSERT INTO dbo.Survey_DeptSurvey
	                            (
	                            DeptID,
	                            SurveyID,
	                            Status,
	                            SubmitTime
	                            )
                            VALUES 
	                            (
	                            @deptid,
	                            @surveyid,
	                            1,
	                            getdate()
	                            )
end";
                var param = new
                {
                    deptid = depID,
                    surveyid = surveyID
                };
                conn.Execute(sql, param);
            }
        }

        /// <summary>
        /// 星评题的分数
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetXinPAnswers(int QuestionID, int paperID, int surveyID,int userID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
with Alluser AS
(
	SELECT UserId, Realname, DeptId, DeptCode,dbo.GetDeptPathByDeptID(DeptId) DeptName,  TrainGrade FROM Sys_User
	WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId={3})
	AND IsDelete=0 
	
)
SELECT  *,dbo.GetRealNameByUserID(UserID) stuName  FROM Survey_ReplyAnswer
WHERE SourceFrom=1 AND QuestionID={0} AND ExampaperID={1} AND ObjectID={2}
AND UserID IN (SELECT UserId FROM Alluser)
", QuestionID, paperID, surveyID, userID);
                return conn.Query<Survey_ReplyAnswer>(sql).ToList();

            }
        }


        /// <summary>
        /// 星评题的分数,汇总
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetXinPAnswersOffice(int QuestionID, int paperID, int surveyID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT  *,dbo.GetRealNameByUserID(UserID) stuName  FROM Survey_ReplyAnswer
WHERE SourceFrom=1 AND QuestionID={0} AND ExampaperID={1} AND ObjectID={2}
AND IsOfficeAccessed<>0
", QuestionID, paperID, surveyID);
                return conn.Query<Survey_ReplyAnswer>(sql).ToList();

            }
        }

        #endregion

        #region 问卷审核（事务所）
        /// <summary>
        /// 问卷审核首页
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Survey_Survey> GetSurveyCheckForOffice()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT * ,
useNum=(SELECT count(DISTINCT userID) FROM Survey_ReplyAnswer WHERE ObjectID=ss.Id AND SourceFrom=1
and IsMaster=0
),
ShouldNumber=(SELECT count(1) FROM Sys_User
    WHERE  (ss.OpenGroupFlag=1 AND UserId IN (SELECT UserId FROM Sys_GroupUser WHERE GroupId IN (SELECT ID FROM dbo.F_SplitIDs(ss.OpenGroup))))
    OR  (ss.OpenDepartFlag=1 AND   DeptId IN (SELECT ID FROM dbo.F_SplitIDs(ss.OpenDepart))))
FROM Survey_Survey ss
WHERE PublishFlag=1";
                return conn.Query<Survey_Survey>(sql).ToList();
            }
        }

        /// <summary>
        /// 树
        /// </summary>
        /// <returns></returns>
        public List<Sys_Department> GetDeptForDepartment(int surveyID, int paperID)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT sd.*,
IsSubmit=(SELECT count(*)  FROM Survey_ReplyAnswer   sr
LEFT JOIN   Survey_DeptSurvey sds ON  sr.DeptID= sds.DeptID
WHERE sr.DeptID= sd.DepartmentId
AND SourceFrom=1 AND IsOfficeAccessed<>0
AND ObjectID=@surveyID AND ExampaperID=@paperID
 AND sds.Id>0
--AND (SELECT COUNT(*) FROM Survey_DeptSurvey WHERE SurveyID=4 AND DeptID=sd.DepartmentId)>0
)FROM Sys_Department sd";
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID
                };
                return conn.Query<Sys_Department>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取问题事务所
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        public List<Survey_Question> GetQuestionSurveyForOffice(int surveyID, int paperID, int DeptID)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT *,
useNum=(SELECT count(DISTINCT userID) FROM Survey_ReplyAnswer
WHERE IsOfficeAccessed<>0 AND SourceFrom=1 AND ObjectID=@surveyID  AND ExampaperID=@paperID AND DeptID=@DeptID AND QuestionID=sq.QuestionID 
AND (SELECT COUNT(*) FROM Survey_DeptSurvey WHERE SurveyID=@surveyID AND DeptID=@DeptID)>0
),
sumresult=(SELECT SubjectiveAnswer FROM Survey_ReplyAnswer WHERE SourceFrom=1 AND ObjectID=@surveyID
AND ExampaperID=@paperID AND QuestionID=0  AND DeptID=@DeptID)
FROM Survey_Question sq WHERE ExampaperID=@paperID
AND sq.QuestionID in (SELECT  DISTINCT QuestionID FROM  Survey_ReplyAnswer WHERE SourceFrom=1 AND ObjectID=@surveyID
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
                var sql = string.Format(@"SELECT *,dbo.GetRealNameByUserID(userID) stuName FROM Survey_ReplyAnswer
WHERE IsOfficeAccessed<>0 AND SourceFrom=1 AND ObjectID=@surveyID  AND ExampaperID=@paperID AND DeptID= @deptID
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
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Sys_User> GetStudentForOffice(int surveyID, int paperID, int deptID, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT UserId, Realname, DeptId, DeptCode, TrainGrade FROM Sys_User
WHERE UserId IN (
SELECT  userID FROM Survey_ReplyAnswer
WHERE IsOfficeAccessed<>0 AND SourceFrom=1 AND ObjectID=@surveyID  AND ExampaperID=@paperID
AND (SELECT COUNT(*) FROM Survey_DeptSurvey WHERE SurveyID=@surveyID  )>0
) and {0}", where);
                var param = new
                {
                    surveyID = surveyID,
                    paperID = paperID
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }
        #endregion

        #region 人员提交明细
        /// <summary>
        /// 此调查中所有未参加的人数
        /// </summary>
        /// <param name="surveyID"></param>
        /// <returns></returns>
        public List<Survey_user> GetUserNoJoin(int surveyID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " userID desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE  @OpenGroupFlag INT,  @OpenGroup NVARCHAR(500)  ,@OpenDepartFlag INT , @OpenDepart NVARCHAR(500)
SELECT @OpenGroupFlag=OpenGroupFlag, @OpenGroup=OpenGroup, @OpenDepartFlag=OpenDepartFlag
, @OpenDepart=OpenDepart FROM Survey_Survey
WHERE Id={0}

SELECT * FROM (
SELECT row_number()OVER(ORDER BY {2}) number,count(*)OVER(PARTITION BY null) totalcount, * FROM (
SELECT sg.UserId,su.Realname, Sex,DeptPath,TrainGrade,CPA,su.DeptName FROM Sys_GroupUser   sg
LEFT JOIN Sys_User su ON sg.UserId=su.UserId
WHERE GroupId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenGroup))  AND @OpenGroupFlag=1
UNION
SELECT UserId,Realname,Sex,DeptPath,TrainGrade,CPA,DeptName FROM Sys_User
WHERE DeptId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenDepart))
AND @OpenDepartFlag=1
AND IsDelete=0 AND Status=0  
) t
WHERE UserId not IN (SELECT UserID FROM Survey_ReplyAnswer  WHERE ObjectID={0} AND SourceFrom=1 ) 
AND {1}
 ) r WHERE   number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", surveyID, where, jsRenderSortField);

                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Survey_user>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 调查已经参加的人数
        /// </summary>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public List<Survey_user> GetUserAnswer(int SurveyID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " SubmitTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(                
SELECT row_number()OVER(ORDER BY {2}) number,count(*)OVER(PARTITION BY null) totalcount,* FROM(
SELECT  DISTINCT su.userID,su.Realname, Sex,DeptPath,TrainGrade,CPA,max(SubmitTime) SubmitTime,su.DeptName FROM Survey_ReplyAnswer    sr
 LEFT JOIN Sys_User su ON sr.UserId=su.UserId
 WHERE ObjectID={0} AND SourceFrom=1 and sr.Status =1
 GROUP BY  su.userID,su.Realname, Sex,DeptPath,TrainGrade,CPA,su.DeptName
) t where {1}
) r WHERE     number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", SurveyID, where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Survey_user>(sql, param).ToList();
            }
        }
        #endregion

    }
}
