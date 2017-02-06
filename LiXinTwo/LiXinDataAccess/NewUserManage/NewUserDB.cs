using LiXinModels;
using LiXinModels.User;
using Retech.Core;
using Retech.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LiXinDataAccess
{
    public class NewUserDB : BaseRepository
    {
        #region 增删改查
        /// <summary>
        /// 新进学员录入
        /// </summary>
        /// <param name="user"></param>
        public void AddNewUser(Sys_User model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Sys_User(DeptId,DeptCode,DeptName,PostId,PostCode,PostName,PostLevel,PostGrade,FreezeTime,JoinDate,UserOldId,PasswordFailureTime,PasswordFailureCount,PhotoUrl,Telephone,MobileNum,JobTitle,IdCardNo,LastLoginTime,UserType,PayGrade,JobNum,GroupMobileNum,ProbationPayGrade,ProbationStart,ProbationEnd,TrainGrade,Major,GraduateSchool,VDeptId,SalaryFulfilsSystem,ApprovalRole,Username,CPA,CPANo,CTA,CPV,ICLV,RealEstateValuers,OtherProfessional,ForeignOtherQualification,OtherProfessionsJobTitle,SectionLeader,Password,LeaderID,CPAPractice,CPAStatus,CPARelationship,SubordinateDept,SubordinateSubstation,SalarySeries,IsTeacher,Memo,Status,Realname,IsDelete,StaffType,LoginId,DeptPath,TrainMaster,IsMain,IsNew,IsInternExp,IsOurIntern,Ename,InternDept,Sex,Email)
	                     values( @DeptId,@DeptCode,@DeptName,@PostId,@PostCode,@PostName,@PostLevel,@PostGrade,@FreezeTime,@JoinDate,@UserOldId,@PasswordFailureTime,@PasswordFailureCount,@PhotoUrl,@Telephone,@MobileNum,@JobTitle,@IdCardNo,@LastLoginTime,@UserType,@PayGrade,@JobNum,@GroupMobileNum,@ProbationPayGrade,@ProbationStart,@ProbationEnd,@TrainGrade,@Major,@GraduateSchool,@VDeptId,@SalaryFulfilsSystem,@ApprovalRole,@Username,@CPA,@CPANo,@CTA,@CPV,@ICLV,@RealEstateValuers,@OtherProfessional,@ForeignOtherQualification,@OtherProfessionsJobTitle,@SectionLeader,@Password,@LeaderID,@CPAPractice,@CPAStatus,@CPARelationship,@SubordinateDept,@SubordinateSubstation,@SalarySeries,@IsTeacher,@Memo,@Status,@Realname,@IsDelete,@StaffType,@LoginId,@DeptPath,@TrainMaster,@IsMain,@IsNew,@IsInternExp,@IsOurIntern,@Ename,@InternDept,@Sex,@Email);SELECT @@IDENTITY as ID ";
                var param = new
                {
                    model.DeptId,
                    model.DeptCode,
                    model.DeptName,
                    model.PostId,
                    model.PostCode,
                    model.PostName,
                    model.PostLevel,
                    model.PostGrade,
                    model.FreezeTime,
                    model.JoinDate,
                    model.UserOldId,
                    model.PasswordFailureTime,
                    model.PasswordFailureCount,
                    model.PhotoUrl,
                    model.Telephone,
                    model.MobileNum,
                    model.JobTitle,
                    model.IdCardNo,
                    model.LastLoginTime,
                    model.UserType,
                    model.PayGrade,
                    model.JobNum,
                    model.GroupMobileNum,
                    model.ProbationPayGrade,
                    model.ProbationStart,
                    model.ProbationEnd,
                    model.TrainGrade,
                    model.Major,
                    model.GraduateSchool,
                    model.VDeptId,
                    model.SalaryFulfilsSystem,
                    model.ApprovalRole,
                    model.Username,
                    model.CPA,
                    model.CPANo,
                    model.CTA,
                    model.CPV,
                    model.ICLV,
                    model.RealEstateValuers,
                    model.OtherProfessional,
                    model.ForeignOtherQualification,
                    model.OtherProfessionsJobTitle,
                    model.SectionLeader,
                    model.Password,
                    model.LeaderID,
                    model.CPAPractice,
                    model.CPAStatus,
                    model.CPARelationship,
                    model.SubordinateDept,
                    model.SubordinateSubstation,
                    model.SalarySeries,
                    model.IsTeacher,
                    model.Memo,
                    model.Status,
                    model.Realname,
                    model.IsDelete,
                    model.StaffType,
                    model.LoginId,
                    model.DeptPath,
                    model.TrainMaster,
                    model.IsMain,
                    model.IsNew,
                    model.IsInternExp,
                    model.IsOurIntern,
                    model.Ename,
                    model.InternDept,
                    model.Sex,
                    model.Email
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.UserId = decimal.ToInt32(list.ID);
            }
        }

        /// <summary>
        /// 新进学员更新
        /// </summary>
        /// <param name="user"></param>
        public void UpdateNewUser(Sys_User model, string userID)
        {
            using (var conn = OpenConnection())
            {
                string strSql = string.Format(@"update Sys_User set 		                     		                     	                     
		                       IsInternExp = @IsInternExp, 		                     
		                       IsOurIntern = @IsOurIntern, 		                     		                     
		                       InternDept = @InternDept,
                               TrainGrade=@TrainGrade,
                               OldTrainGrade=TrainGrade,
                               IsNew=1,
                               IsHistoryNew=1,
                               NewYear=@Year		                     
		                   where UserId in ({0})", userID);

                var param = new
                {
                    model.IsInternExp,
                    model.IsOurIntern,
                    model.InternDept,
                    model.TrainGrade,
                    model.Sex,
                    model.Major,
                    model.GraduateSchool,
                    DateTime.Now.Year
                };
                conn.Execute(strSql, param);
            }
        }
        /// <summary>
        /// 新进学员更新
        /// </summary>
        /// <param name="user"></param>
        public void UpdateNewUserNoTrain(Sys_User model, string userID)
        {
            using (var conn = OpenConnection())
            {
                string strSql = string.Format(@"update Sys_User set 		                     		                     	                     
		                       IsInternExp = @IsInternExp, 		                     
		                       IsOurIntern = @IsOurIntern, 		                     		                     
		                       InternDept = @InternDept,
                               TrainGrade=@TrainGrade,
                               OldTrainGrade=TrainGrade,
                               IsNew=1,
                               IsHistoryNew=1,
                               NewYear=@Year		                     
		                   where UserId in ({0})", userID);

                var param = new
                {
                    model.IsInternExp,
                    model.IsOurIntern,
                    model.InternDept,
                    model.TrainGrade,
                    DateTime.Now.Year
                };
                conn.Execute(strSql, param);
            }
        }

        /// <summary>
        /// 新进员工列表
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetNewUserList(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  UserId desc")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"select * from(
SELECT row_number()OVER(ORDER BY CASE WHEN  DeptName='待分配部门' THEN 0 ELSE 1 END) number,count(*)OVER(PARTITION BY null) totalcount,* FROM(
SELECT su.*,nuse.Score examScore,nuse.SumScore examSumScore 
FROM dbo.Sys_User  su LEFT JOIN New_UserExamScore nuse ON su.UserId=nuse.UserID
 where  Status=0 AND IsDelete=0   and {1}
) t) result
 WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", jsRenderSortField, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 根据语句 人员ID更新
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        public void Update(int userId, string query)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Sys_User
SET {0}
WHERE UserId={1}", query, userId);
                conn.Execute(sql);

            }
        }
        #endregion

        #region 录入独立考试的学分
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertNew_UserExamScore(New_UserExamScore model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"
IF ((SELECT count(*) FROM New_UserExamScore WHERE UserID=@UserID)=0)
begin
INSERT INTO New_UserExamScore(UserID,Score,SumScore)
	                     values( @UserID,@Score,@SumScore);SELECT @@IDENTITY as ID 
End
Else
begin
   update New_UserExamScore
   set 		                     
    Score = @Score , 		                     
    SumScore = @SumScore   
    where  UserID = @UserID 
end
";

                var param = new
                {
                    UserID = model.UserID,
                    Score = model.Score,
                    SumScore = model.SumScore
                };
                conn.Execute(strSql, param);
                //dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                //model.Id = decimal.ToInt32(list.ID);	
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateNew_UserExamScore(New_UserExamScore model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update New_UserExamScore set 		                     		                     
		                       UserID = @UserID , 		                     
		                       Score = @Score , 		                     
		                       SumScore = @SumScore   
		                   where Id=@Id";

                var param = new
                {
                    model.Id,
                    model.UserID,
                    model.Score,
                    model.SumScore
                };
                conn.Execute(strSql, param);
            }

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public New_UserExamScore GetModel(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"select Id, UserID, Score, SumScore  from New_UserExamScore  
			                  where {0}", where);

                return conn.Query<New_UserExamScore>(sql).FirstOrDefault();
            }

        }

        /// <summary>
        /// 没有就新增  有就更新
        /// </summary>
        /// <param name="model"></param>
        public void UpdateOrInsert(string loginID, double score, double sumScore)
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"DECLARE @UserID INT 
SELECT  @UserID=UserId FROM Sys_User WHERE LoginId='{0}'
IF ((SELECT count(*) FROM New_UserExamScore WHERE UserID=@UserID)=0)
	BEGIN
        INSERT INTO New_UserExamScore(UserID,Score,SumScore)
	    values(@UserID,{1},{2})
	END
ELSE
	BEGIN
	    update New_UserExamScore 
	    set     Score = {1} , 		                     
		         SumScore ={2}   
		   where UserID = @UserID
	END", loginID, score, sumScore);

                conn.Execute(sql);
            }
        }

        #endregion

        /// <summary>
        /// 新员工的年度
        /// </summary>
        /// <returns></returns>
        public List<int> GetNewYear()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT  distinct NewYear FROM Sys_User
WHERE IsHistoryNew=1";
                return conn.Query<int>(sql).ToList();
            }
        }
    }

}
