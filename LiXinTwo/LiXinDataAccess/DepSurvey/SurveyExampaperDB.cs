using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LiXinModels.Survey;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.DeptSurvey
{
    public partial class SurveyExampaperDB : BaseRepository
    {
        /// <summary>
        /// 根据分类ID获取问卷
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public Survey_Exampaper GetServeyExamPaper(int paperID)
        {
            using (var connection = OpenConnection())
            {
                const string sqlstr = "select * from Dep_Survey_Exampaper where IsDelete=0 AND ExampaperID=@paperID";
                return connection.Query<Survey_Exampaper>(sqlstr, new
                {
                    paperID
                }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取所有问卷
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Survey_Exampaper> GetServeyExamPaperList(string where = " Dep_Survey_Exampaper.IsDelete=0 ", string orderBy = " order by Dep_Survey_Exampaper.ExampaperID desc ")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select Dep_Survey_Exampaper.*,Dep_Survey_ExampaperCategory.CategoryName as SortName from Dep_Survey_Exampaper,Dep_Survey_ExampaperCategory where Dep_Survey_ExampaperCategory.CategoryId=Dep_Survey_Exampaper.SortID and {0} {1} ", where, orderBy);
                return connection.Query<Survey_Exampaper>(query).ToList();
            }
        }

        /// <summary>
        /// 获取所有问卷(分页)
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Survey_Exampaper> GetServeyExamPaperListPaging(out int totalCount, string where = " Dep_Survey_Exampaper.IsDelete=0 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by Dep_Survey_Exampaper.LastUpdateTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Dep_Survey_Exampaper where " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over(order by {1} ) as rowNum,Dep_Survey_Exampaper.*,Dep_Survey_ExampaperCategory.CategoryName as SortName from Dep_Survey_Exampaper,Dep_Survey_ExampaperCategory
    where Dep_Survey_ExampaperCategory.CategoryId=Dep_Survey_Exampaper.SortID and " + where + @" 
) result 
where rowNum between  @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", pageLength, orderBy);
                var parameters = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return connection.Query<Survey_Exampaper>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 增加问卷
        /// </summary>
        /// <param name="model">问卷实体</param>
        public bool AddServeyExamPaper(Survey_Exampaper model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Dep_Survey_Exampaper (ExamTitle,ExamType,UserID,SortID,LastUpdateTime,ExamDescription,ExampaperUsage,IsDelete,DeptID) values (@ExamTitle,@ExamType,@UserID,@SortID,@LastUpdateTime,@ExamDescription,@ExampaperUsage,@IsDelete,@DeptID) SELECT @@Identity as Id";
                var param = new
                {
                    model.ExamTitle,
                    model.ExamType,
                    model.UserID,
                    model.SortID,
                    model.LastUpdateTime,
                    model.ExamDescription,
                    model.ExampaperUsage,
                    model.IsDelete,
                    model.DeptID
                };
                var id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.ExampaperID = decimal.ToInt32(id);
                return model.ExampaperID > 0;
            }
        }

        /// <summary>
        /// 修改问卷
        /// </summary>
        /// <param name="model">问卷</param>
        public bool EditServeyExamPaper(Survey_Exampaper model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Dep_Survey_Exampaper set ExamTitle=@ExamTitle,ExamType=@ExamType,UserID=@UserID,SortID=@SortID,LastUpdateTime=getdate(),ExamDescription=@ExamDescription,ExampaperUsage=@ExampaperUsage,IsDelete=@IsDelete where ExampaperID=@ExampaperID ";
                var param = new
                {
                    ExamTitle = model.ExamTitle,
                    ExamType = model.ExamType,
                    UserID = model.UserID,
                    SortID = model.SortID,
                    ExamDescription = model.ExamDescription,
                    ExampaperUsage = model.ExampaperUsage,
                    IsDelete = model.IsDelete,
                    ExampaperID = model.ExampaperID
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="id">问卷ID</param>
        /// <returns></returns>
        public bool DeleteServeyExamPaper(int id)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Dep_Survey_Exampaper set IsDelete=1 where ExampaperID=@id";
                var param = new
                {
                    id
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="ids">问卷ID</param>
        /// <returns></returns>
        public bool DeleteServeyExamPaper(string ids)
        {
            using (var conn = OpenConnection())
            {
                string sqlwhere =
                  string.Format(@"update Dep_Survey_Exampaper set IsDelete=1 where ExampaperID in ({0})", ids);

                var result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 判断问卷是否存在
        /// </summary>
        /// <param name="paperName">问卷名称</param>
        /// <param name="paperID">问卷ID</param>
        /// <param name="sortID">分类ID</param>
        public bool IsExsitServeyExamPaper(string paperName, int paperID, int sortID)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"select count(*) from Dep_Survey_Exampaper where ExamTitle=@paperName and ExampaperID<>@paperID and SortID=@sortID";
                var param = new
                {
                    paperName,
                    paperID,
                    sortID
                };
                var count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count == 0;
            }
        }

        #region 删除问卷 问题 答案

        /// <summary>
        /// 根据问卷Id删除问题,是真删哦。。
        /// </summary>
        /// <param name="exampaperId">问卷Id</param>
        /// <returns></returns>
        public bool DeleteByExampaperId(int exampaperId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string deleteSql = @"UPDATE Dep_Survey_Question
SET Status=1
WHERE ExampaperID= @eId";
                var param = new
                {
                    eId = exampaperId
                };
                return connection.Execute(deleteSql, param) > 0;
            }
        }

        /// <summary>
        /// 根据问卷Id删除问题下的题目,是真删哦。。
        /// </summary>
        /// <param name="exampaperId">问卷Id</param>
        /// <returns></returns>
        public bool DeleteAnswerById(int exampaperId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string deleteSql = @"UPDATE Dep_Survey_QuestionAnswer
set  Status=1
WHERE QuestionID in (
SELECT QuestionID FROM Dep_Survey_Question
WHERE ExampaperID=@eId)";
                var param = new
                {
                    eId = exampaperId
                };
                return connection.Execute(deleteSql, param) > 0;
            }
        }

        #endregion

        #region 查询 问卷 问题 答案
        /// <summary>
        /// 根据问卷ID，查询出问卷的基本信息
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public List<Survey_Exampaper> GetSurveyByID(int paperID)
        {
            using (var conn = OpenConnection())
            {
                const string sql = @"SELECT se.*,sec.CategoryName FROM Dep_Survey_Exampaper se
                                     LEFT JOIN Dep_Survey_ExampaperCategory sec ON se.SortID=sec.CategoryId
                                     WHERE ExampaperID=@paperID";
                var param = new
                {
                    paperID = paperID
                };
                return conn.Query<Survey_Exampaper>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 根据问题ID，查询出问卷的问题
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public List<Survey_Question> GetQuestionByID(int paperID)
        {
            using (var conn = OpenConnection())
            {
                const string sql = @"SELECT * FROM Dep_Survey_Question
                                    WHERE ExampaperID=@paperID and Status=0";
                var param = new
                {
                    paperID = paperID
                };
                return conn.Query<Survey_Question>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 根据答案ID，查询出问题的答案
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public List<Survey_QuestionAnswer> GetQuestionAnswerByID(int paperID)
        {
            using (var conn = OpenConnection())
            {
                const string sql = @"SELECT * FROM Dep_Survey_QuestionAnswer
                                    WHERE QuestionID  IN (SELECT QuestionID  FROM Dep_Survey_Question
                                    WHERE ExampaperID=@paperID)";
                var param = new
                {
                    paperID = paperID
                };
                return conn.Query<Survey_QuestionAnswer>(sql, param).ToList();
            }
        }
        #endregion

    }
}
