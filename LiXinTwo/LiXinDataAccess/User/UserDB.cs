using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.User;
using LiXinModels.CourseManage;
using LiXinModels.CourseLearn;

namespace LiXinDataAccess.User
{
    /// <summary>
    ///     对用户进行操作的类
    /// </summary>
    public class UserDB : BaseRepository
    {
        /// <summary>
        ///     判断 用户信息 是否存在
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public bool Exists(string jobNum, string username, int userId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    "select count(1) from Sys_User WHERE IsDelete = 0 and (Username = @Username or JobNum = @JobNum) ";
                if (userId > 0)
                    sqlwhere += " and userId <> " + userId;
                var param = new
                                {
                                    Username = username,
                                    JobNum = jobNum
                                };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        ///     增加一条数据
        /// </summary>
        /// <param name="model"></param>
        public void Add(Sys_User model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"if((select count(*) from Sys_User where JobNum=@JobNum)=0)
BEGIN
insert into Sys_User( UserOldId,JobNum,Username,Password,Realname,Ename,Sex,Email,DeptId,DeptCode,DeptName,PostId,PostCode,PostName,PostLevel,PostGrade,FreezeTime,JoinDate,PasswordFailureTime,PasswordFailureCount,PhotoUrl,Telephone,MobileNum,JobTitle,IdCardNo,LastLoginTime,UserType,PayGrade,GroupMobileNum,ProbationPayGrade,ProbationStart,ProbationEnd,TrainGrade,Major,GraduateSchool,VDeptId,SalaryFulfilsSystem,ApprovalRole,CPA,CPANo,CTA,CPV,ICLV,RealEstateValuers,OtherProfessional,ForeignOtherQualification,OtherProfessionsJobTitle,SectionLeader,LeaderID,CPAPractice,CPAStatus,CPARelationship,SubordinateDept,SubordinateSubstation,SalarySeries,IsTeacher,Memo,Status,IsDelete,StaffType,LoginId,DeptPath,IsMain,IsNew,IsInternExp,IsOurIntern,InternDept,NumberID,IsChangeNumberId,OldTrainGrade,CPACreateDate,IsHistoryNew,NewYear) 
values (@UserOldId,@JobNum,@Username,@Password,@Realname,@Ename,@Sex,@Email,@DeptId,@DeptCode,@DeptName,@PostId,@PostCode,@PostName,@PostLevel,@PostGrade,@FreezeTime,@JoinDate,@PasswordFailureTime,@PasswordFailureCount,@PhotoUrl,@Telephone,@MobileNum,@JobTitle,@IdCardNo,@LastLoginTime,@UserType,@PayGrade,@GroupMobileNum,@ProbationPayGrade,@ProbationStart,@ProbationEnd,@TrainGrade,@Major,@GraduateSchool,@VDeptId,@SalaryFulfilsSystem,@ApprovalRole,@CPA,@CPANo,@CTA,@CPV,@ICLV,@RealEstateValuers,@OtherProfessional,@ForeignOtherQualification,@OtherProfessionsJobTitle,@SectionLeader,@LeaderID,@CPAPractice,@CPAStatus,@CPARelationship,@SubordinateDept,@SubordinateSubstation,@SalarySeries,@IsTeacher,@Memo,@Status,@IsDelete,@StaffType,@LoginId,@DeptPath,@IsMain,@IsNew,@IsInternExp,@IsOurIntern,@InternDept,@NumberID,@IsChangeNumberId,@OldTrainGrade,@CPACreateDate,@IsHistoryNew,@NewYear)
select @@IDENTITY END ";
                var param = new
                {
                    model.UserOldId,
                    model.JobNum,
                    model.Username,
                    model.Password,
                    model.Realname,
                    model.Ename,
                    model.Sex,
                    model.Email,
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
                    model.IsDelete,
                    model.StaffType,
                    model.LoginId,
                    model.DeptPath,
                    model.IsMain,
                    model.IsNew,
                    model.IsInternExp,
                    model.IsOurIntern,
                    model.InternDept,
                    model.NumberID,
                    model.IsChangeNumberId,
                    model.OldTrainGrade,
                    model.CPACreateDate,
                    model.IsHistoryNew,
                    model.NewYear
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.UserId = decimal.ToInt32(id);
            }
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool Update(Sys_User model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_User set 
                        UserOldId=@UserOldId,JobNum=@JobNum,Username=@Username,Password=@Password,Realname=@Realname,Ename=@Ename,
                        Sex=@Sex,Email=@Email,DeptId=@DeptId,DeptCode=@DeptCode,DeptName=@DeptName,PostId=@PostId,
                        PostCode=@PostCode,PostName=@PostName,PostLevel=@PostLevel,PostGrade=@PostGrade,
                        FreezeTime=@FreezeTime,JoinDate=@JoinDate,PasswordFailureTime=@PasswordFailureTime,
                        PasswordFailureCount=@PasswordFailureCount,
                        PhotoUrl=@PhotoUrl,Telephone=@Telephone,MobileNum=@MobileNum,JobTitle=@JobTitle,
                        IdCardNo=@IdCardNo,LastLoginTime=@LastLoginTime,UserType=@UserType,PayGrade=@PayGrade,
                        GroupMobileNum=@GroupMobileNum,ProbationPayGrade=@ProbationPayGrade,ProbationStart=@ProbationStart,
                        ProbationEnd=@ProbationEnd,
                        TrainGrade=@TrainGrade,Major=@Major,GraduateSchool=@GraduateSchool,VDeptId=@VDeptId,
                        SalaryFulfilsSystem=@SalaryFulfilsSystem,ApprovalRole=@ApprovalRole,CPA=@CPA,CPANo=@CPANo,
                        CTA=@CTA,CPV=@CPV,ICLV=@ICLV,RealEstateValuers=@RealEstateValuers,OtherProfessional=@OtherProfessional,
                        ForeignOtherQualification=@ForeignOtherQualification,OtherProfessionsJobTitle=@OtherProfessionsJobTitle,
                        SectionLeader=@SectionLeader,
                        LeaderID=@LeaderID,CPAPractice=@CPAPractice,CPAStatus=@CPAStatus,CPARelationship=@CPARelationship,
                        SubordinateDept=@SubordinateDept,SubordinateSubstation=@SubordinateSubstation,SalarySeries=@SalarySeries,
                        IsTeacher=@IsTeacher,Memo=@Memo,Status=@Status,IsDelete=@IsDelete,StaffType=@StaffType,
                        LoginId=@LoginId,DeptPath=@DeptPath,IsMain=@IsMain,	IsNew = @IsNew,
	IsInternExp = @IsInternExp,
	IsOurIntern = @IsOurIntern,
	InternDept = @InternDept,
	NumberID = @NumberID,
    IsChangeNumberId=@IsChangeNumberId,OldTrainGrade=@OldTrainGrade,CPACreateDate=@CPACreateDate,IsHistoryNew=@IsHistoryNew,NewYear=@NewYear
                    where UserId=@UserId
";
                var param = new
                {
                    model.UserOldId,
                    model.JobNum,
                    model.Username,
                    model.Password,
                    model.Realname,
                    model.Ename,
                    model.Sex,
                    model.Email,
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
                    model.IsDelete,
                    model.StaffType,
                    model.LoginId,
                    model.DeptPath,
                    model.UserId,
                    model.IsMain,
                    model.IsNew,
                    model.IsInternExp,
                    model.IsOurIntern,
                    model.InternDept,
                    model.NumberID,
                    model.IsChangeNumberId,
                    model.OldTrainGrade,
                    model.CPACreateDate,
                    model.IsHistoryNew,
                    model.NewYear
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        public bool Delete(string userIds)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    string.Format(
                        @"update Sys_User set IsDelete = 1 where UserId in ({0})
                                    delete Sys_UserRole where UserId in ({0})
                                    ",
                        userIds);
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        ///     根据ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Sys_User Get(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Sys_User where UserId=@userId";
                return connection.Query<Sys_User>(sqlstr, new
                                                              {
                                                                  userId
                                                              }).FirstOrDefault();
            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        /// <param name="strWhere">获取数据的Sql Where条件（不带WHERE）</param>
        public List<Sys_User> GetList(string strWhere = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select * from Sys_User where " + strWhere;
                return conn.Query<Sys_User>(sqlwhere).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Sys_User> GetList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY UserId DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Sys_User where " + where)
                        .First();
                string query =
                    string.Format(
                        @"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum
,Sys_User.*
,dbo.GetRoleNameByUserID(Sys_User.userid) as RoleName from Sys_User
    where " +
                        where +
                        @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)",
                        pageLength, orderBy);
                var parameters = new
                                     {
                                         pageCount = pageLength,
                                         pageIndex = startIndex / pageLength
                                     };
                return connection.Query<Sys_User>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public Sys_User GetUserByName(string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select * from Sys_User where Username = @Username and  IsDelete = 0";
                return conn.Query<Sys_User>(sqlwhere, new
                                                          {
                                                              Username = name
                                                          }).FirstOrDefault();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public Sys_User GetUserByLoginId(string loginId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select * from Sys_User where loginId = @loginId and  IsDelete = 0";
                return conn.Query<Sys_User>(sqlwhere, new
                {
                    loginId = loginId
                }).FirstOrDefault();
            }
        }

        /// <summary>
        ///     验证用户登录
        /// </summary>
        /// <returns></returns>
        public Sys_User LoginChecking(string username, string password)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere =
                    string.Format(
                        "select * from Sys_User where Username = @Username and Password= @Password and IsDelete = 0");
                var param = new
                                {
                                    Username = username,
                                    Password = password
                                };
                return connection.Query<Sys_User>(sqlwhere, param).FirstOrDefault();
            }
        }

        /// <summary>
        ///     设置用户状态
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateUserStatus(string userIds, int status, DateTime? FreezeTime)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string updatesql = @"UPDATE Sys_User SET Status =@status,FreezeTime=@FreezeTime  WHERE UserId in (" +
                                   userIds + ")";
                int result = connection.Execute(updatesql, new
                                                               {
                                                                   status,
                                                                   FreezeTime
                                                               });
                return result > 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <param name="FreezeTime"></param>
        /// <returns></returns>
        public bool UpdateUserPwdStatus(string userIds, int count, DateTime? time)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string updatesql =
                    @"UPDATE Sys_User SET PasswordFailureCount =@count,PasswordFailureTime=@time WHERE UserId in (" +
                    userIds + ")";
                int result = connection.Execute(updatesql, new
                                                               {
                                                                   count,
                                                                   time
                                                               });
                return result > 0;
            }
        }

        /// <summary>
        ///     获取用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserStatus(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string selectSql = "select Status from Sys_User where UserId=@UserId";
                var param = new
                                {
                                    UserId = userId
                                };

                return connection.Query<int>(selectSql, param).FirstOrDefault();
            }
        }

        /// <summary>
        ///     获取用户冻结终止时间
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>返回是否是永久冻结 or 冻结时间</returns>
        public string GetUserFreezeTime(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string selectSql = "select FreezeTime from Sys_User where UserId=@UserId";
                var param = new
                                {
                                    UserId = userId
                                };
                DateTime freezeTime = connection.Query<DateTime>(selectSql, param).FirstOrDefault();
                if (freezeTime.Year == 1)
                {
                    return "1";
                }
                return freezeTime.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        public void AddRoleToUser(int userId, string roleIds)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"
                       DECLARE @RoleId int;--获取单个的id
                        DECLARE @nextId int;--单个的id的下标
                        SET @nextId=1;
                        DECLARE @LengthId int;--Id的集合中共有多少个id

                        DECLARE @DefaultRoleId int;--获取默认的角色的ID

                        SELECT @DefaultRoleId = roleId from Sys_Roles where IsDefault = 1 and IsDelete = 0 

                        delete from Sys_UserRole where userId=@UserId         

                        SELECT @LengthId=dbo.GetSplitLength(@roleIds,',');
                        while @nextId<=@LengthId
                          begin
                           --当前职位id
			                SET @RoleId = left(dbo.GetSplitOfIndex(@roleIds,',',@nextId), 100);			  
                              if(@RoleId<>'' and @RoleId<>'0' and @RoleId<> @DefaultRoleId)
			                   begin
                            INSERT INTO Sys_UserRole(UserId,RoleId) values(@UserId,@RoleId)
			                   end
                            SET @nextId=@nextId+1;
                          end 
";
                var param = new
                                {
                                    UserId = userId,
                                    roleIds
                                };
                conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        public void DeleteUserRole(int userId, int roleId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"delete Sys_UserRole where roleId=@RoleId and UserId=@UserId";
                var param = new
                                {
                                    UserId = userId,
                                    RoleId = roleId
                                };
                conn.Execute(sqlwhere, param);
            }
        }

        #region 讲师的相关操作

        /// <summary>
        /// 添加内部讲师
        /// </summary>
        /// <param name="userID"></param>
        public void InsertInnerTeacher(string userID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Sys_User
SET IsTeacher=1
WHERE UserId in ({0})", userID);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 添加外聘讲师
        /// </summary>
        /// <param name="model"></param>
        public void InsertOuterTeacher(Sys_User model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql =
                    @"INSERT INTO dbo.Sys_User
	                                            (
	                                            JobNum,
	                                            Username,
	                                            Password,
	                                            Realname,
	                                            Sex,
	                                            Email,
	                                            Telephone,
	                                            PayGrade,
	                                            IsTeacher,
	                                            IsDelete 

	                                            )
                                            VALUES 
	                                            (
	                                            '',
	                                            '',
	                                            '',
	                                            @realname,
	                                            @sex,
	                                            @email,
	                                            @telephone,
                                                @paygrade,
	                                            2,
	                                            0
	                                            );SELECT @@IDENTITY AS ID";
                var param = new
                                {
                                    realname = model.Realname,
                                    sex = model.Sex,
                                    email = model.Email,
                                    telephone = model.Telephone,
                                    paygrade = model.PayGrade
                                };
                dynamic item = conn.Query<dynamic>(sql, param).FirstOrDefault();
                model.UserId = decimal.ToInt32(item.ID);

            }
        }

        /// <summary>
        /// 删除讲师
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="teacher"></param>
        public void DeleteTeacher(int userID, int teacher)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql =
                    @" IF(@isteacher=1)
	                            BEGIN
		                            UPDATE Sys_User
		                            SET IsTeacher=0
		                            WHERE UserId=@userid
	                            END
                            ELSE
	                            BEGIN
	                                UPDATE Sys_User
	                                SET IsDelete=1
	                                WHERE UserId=@userid
	                            END";
                var param = new
                                {
                                    isteacher = teacher,
                                    userid = userID
                                };
                conn.Execute(sql, param);
            }
        }



        #endregion

        #region 指纹库同步时使用

        /// <summary>
        /// 根据HRID获取用户
        /// </summary>
        /// <param name="hrID"></param>
        /// <returns></returns>
        public Sys_User GetUserByHrID(string hrID)
        {
            using (var connection = OpenConnection())
            {
                var sqlstr = string.Format("select * from Sys_User where JobNum=@hrID");
                return connection.Query<Sys_User>(sqlstr, new
                {
                    hrID = hrID
                }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 判断 用户信息 是否存在
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <returns></returns>
        public bool ExistUserFinger(int userId)
        {
            using (var conn = OpenConnection())
            {
                var sqlwhere = string.Format("select count(*) from Sys_UserFingerInfor where UserId=@userId");
                var param = new
                {
                    userId
                };
                var count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        ///     增加一条数据
        /// </summary>
        /// <param name="model"></param>
        public void AddUserFinger(Sys_UserFinger model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Sys_UserFingerInfor (UserId,UserHrId,Ssn,Name,FingerTemplate,FingerTemplate1,FingerTemplate2) values (@userID,@hrID,@ssn,@name,@template,@template1,@template2)";
                var param = new
                                {
                                    userID = model.USERID,
                                    hrID = model.TITLE,
                                    ssn = model.SSN,
                                    name = model.NAME,
                                    template = model.FingerTemplate,
                                    template1 = model.FingerTemplate1,
                                    template2 = model.FingerTemplate2
                                };
                conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <param name="flag">0:HR同步；1：原系统同步 </param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateUserFinger(Sys_UserFinger model, int flag)
        {
            using (var conn = OpenConnection())
            {

                var sqlwhere = "";
                if (flag == 0)
                {
                    sqlwhere =
                        @"update Sys_UserFingerInfor set UserHrId=@hrID,Ssn=@ssn,Name=@name,FingerTemplate=@template where UserId=@userID";
                }
                else
                {
                    sqlwhere =
                        @"update Sys_UserFingerInfor set FingerTemplate1=@template1,FingerTemplate2=@template2 where UserId=@userID";
                }
                var param = new
                {
                    userID = model.USERID,
                    hrID = model.TITLE,
                    ssn = model.SSN,
                    name = model.NAME,
                    template = model.FingerTemplate,
                    template1 = model.FingerTemplate1,
                    template2 = model.FingerTemplate2
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        #endregion

        #region 个人中心

        /// <summary>
        /// 集中授课
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTogetherList(int uid, int year)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"SELECT temp.* FROM (
SELECT cco.*,cc.IsMust,cc.Way ,case WHEN (dbo.F_NewIsAppStatus(cc.Id,cco.UserId,cco.OrderStatus)=1)  THEN cco.GetScore  ELSE  0 END AS tmpScore 
FROM Cl_CourseOrder AS cco LEFT JOIN Co_Course AS cc ON cc.Id=cco.CourseId WHERE cco.UserId=@UserId and cc.YearPlan=@YearPlan AND cc.Way=1 and cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserId)) AS temp 
WHERE  temp.tmpScore>0");
                return conn.Query<Cl_CourseOrder>(sqlwhere, new { YearPlan = year, UserId = uid }).ToList();
            }
        }
        /// <summary>
        /// 视频授课
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetVideoList(int uid, int year)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"SELECT temp.* FROM (
SELECT ccs.CourseID,ccs.GetLength,cc.IsMust,cc.Way  
FROM Cl_CpaLearnStatus AS ccs 
LEFT JOIN Co_Course AS cc ON cc.Id=ccs.CourseId WHERE ccs.UserId=@UserId and cc.YearPlan=@YearPlan AND ccs.CpaFlag =0 and 
cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserId)) AS temp WHERE  temp.GetLength>0");
                return conn.Query<Cl_CourseOrder>(sqlwhere, new { YearPlan = year, UserId = uid }).ToList();
            }
        }

        /// <summary>
        /// CPA折算目标学时
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public decimal GetCPAScore(int userid, int year)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT sum(clc.GetLength)OVER(PARTITION BY null) AS passsum FROM Cl_CpaLearnStatus AS clc LEFT JOIN Co_Course AS cc ON cc.Id=clc.CourseId WHERE cc.YearPlan=@YearPlan AND cc.IsDelete=0 AND cc.CourseFrom=2 and clc.CpaFlag=1 and clc.GradeStatus=1 AND clc.GetLength>=0 and clc.UserID=@UserID and 
cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserID)
AND (SELECT count(1) FROM Sys_User WHERE UserId=@UserID AND CPA='是' )>0
";
                var param = new
                {
                    YearPlan = year,
                    UserID = userid
                };
                return conn.Query<decimal>(sql, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取个人信息和CPA学时和补预订次数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Sys_User GetmodelCAP(int uid, int year)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"SELECT su.UserId,su.Realname,su.CPA,su.Sex,su.TrainGrade,su.PhotoUrl,sd.DeptName,
(SELECT sum(GetLength) FROM Cl_CpaLearnStatus WHERE UserID=@UserId AND GetLength>=0 and CpaFlag!=0 ) as CPAScore,
(SELECT count(*) FROM View_OrderTimes WHERE UserId=@UserId AND YearPlan=@year) AS ordertimes FROM Sys_User AS su LEFT JOIN Sys_Department AS sd ON su.DeptId=sd.DepartmentId WHERE su.UserId=@UserId");
                return conn.Query<Sys_User>(sqlwhere, new
                {
                    UserId = uid,
                    year = year
                }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 学分前5名
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetTop5Credit()
        {
            using (IDbConnection conn = OpenConnection())
            {
                //string sqlwhere = "SELECT TOP 5 tab.UserId,su.Realname,sd.DeptName,sum(tab.GetScore) AS GetScore FROM (SELECT UserId,sum(GetScore) AS GetScore FROM Cl_CourseOrder WHERE GetScore>=0 Group by UserId union ALL SELECT UserId,sum(GetLength) AS GetScore FROM Cl_CpaLearnStatus WHERE GetLength>=0 and CpaFlag=0 Group by UserId) AS tab LEFT JOIN Sys_User AS su ON tab.UserId=su.UserId LEFT JOIN Sys_Department AS sd ON su.DeptId=sd.DepartmentId  Group by tab.UserId,su.Realname,sd.DeptName ORDER BY GetScore DESC";
                string sqlwhere = @"SELECT TOP 5 tab.UserId,su.Realname,sd.DeptName,
sum(tab.GetScore) AS GetScore 
FROM (
SELECT UserId,sum(GetScore) AS GetScore 
FROM Cl_CourseOrder WHERE GetScore>=0  and
(dbo.F_NewIsAppStatus(Cl_CourseOrder.Id,UserId,Cl_CourseOrder.OrderStatus)=1)
Group by UserId 
union ALL 
SELECT UserId,sum(GetLength) AS GetScore FROM Cl_CpaLearnStatus WHERE GetLength>=0 and CpaFlag=0 Group by UserId
) AS tab 
LEFT JOIN Sys_User AS su ON tab.UserId=su.UserId
LEFT JOIN Sys_Department AS sd ON su.DeptId=sd.DepartmentId  
Group by tab.UserId,su.Realname,sd.DeptName 
ORDER BY GetScore DESC";
                return conn.Query<Sys_User>(sqlwhere).ToList();
            }
        }


        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="photourl"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool UpdatePhoto(string photourl, int userid)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere = @"UPDATE dbo.Sys_User SET PhotoUrl = @PhotoUrl where UserId=@UserId";
                var param = new
                {
                    PhotoUrl = photourl,
                    UserId = userid
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 年度培训目标(集中授课)
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="bentime">年度计划开课时间</param>
        /// <param name="endtime">年度计划结束时间</param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyCourse(int userid, int year, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {

                //string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY cc.Id " + Order + ") numb,count(*)OVER(PARTITION BY null) totalcount,sum(clc.GetScore)OVER(PARTITION BY null) AS passsum,cc.Id,cc.CourseName,CourseLength,Way,StartTime,EndTime,sc.RoomName,CASE WHEN (clc.OrderStatus=3 AND clc.MakeUpApprovalFlag=1) OR clc.OrderStatus=1 then clc.GetScore ELSE 0 END GetScore,isnull(ccp.Id,0) AS CoPaperID,isnull(ccp.PaperId,0) AS PaperId FROM Cl_CourseOrder AS clc LEFT JOIN Co_Course AS cc ON cc.Id=clc.CourseId LEFT JOIN Sys_ClassRoom sc ON sc.Id=cc.RoomId LEFT JOIN Co_CoursePaper AS ccp ON (ccp.CourseId=clc.CourseId AND ccp.IsMain=0) WHERE cc.YearPlan=@YearPlan AND cc.IsDelete=0 AND cc.CourseFrom=2 AND clc.OrderStatus!=0 AND clc.GetScore>0 AND clc.UserId=@UserId and " + where + ") result WHERE numb BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                //由于格式问题，更改了一下，未做大的变动。。
                string sql = string.Format(@"select * from(SELECT * ,
sum(GetScore)OVER(PARTITION BY null) AS passsum
FROM 
(SELECT row_number()OVER(ORDER BY Id {1}) numb,count(*)OVER(PARTITION BY null) totalcount,* FROM(
	SELECT 	cc.Id,cc.CourseName,CourseLength,Way,StartTime,EndTime,sc.RoomName,cc.IsMust,
	CASE WHEN (dbo.F_NewIsAppStatus(cc.Id,@UserId,clc.OrderStatus)=1) then clc.GetScore  ELSE 0 END GetScore,
	isnull(ccp.Id,0) AS CoPaperID,isnull(ccp.PaperId,0) AS PaperId ,OrderStatus,MakeUpApprovalFlag
	FROM Cl_CourseOrder AS clc 
	LEFT JOIN Co_Course AS cc ON cc.Id=clc.CourseId LEFT JOIN Sys_ClassRoom sc ON sc.Id=cc.RoomId 
	LEFT JOIN Co_CoursePaper AS ccp ON (ccp.CourseId=clc.CourseId AND ccp.IsMain=0) 
	WHERE cc.YearPlan=@YearPlan AND cc.IsDelete=0 AND cc.CourseFrom=2 AND clc.OrderStatus!=0  AND clc.UserId=@UserId and {0} and cc.Way=1 AND cc.Id NOT IN ( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserId)) result WHERE  GetScore>0) a ) t
WHERE numb BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where, Order);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    YearPlan = year,
                    UserId = userid
                };
                return conn.Query<Co_CourseShow>(sql, param).ToList();
            }
        }
        /// <summary>
        /// 年度培训目标(视频授课)
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyVidioCourse(int userid, int year, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY cc.Id " + Order + @") numb,count(*)OVER(PARTITION BY null) totalcount,sum(clc.GetLength)OVER(PARTITION BY null) AS passsum,cc.Id,cc.CourseName,CourseLength,Way,StartTime,EndTime,cc.IsMust,sc.RoomName,clc.GetLength as GetScore,isnull(ccp.Id,0) AS CoPaperID,isnull(ccp.PaperId,0) AS PaperId FROM Cl_CpaLearnStatus AS clc LEFT JOIN Co_Course AS cc ON cc.Id=clc.CourseId LEFT JOIN Sys_ClassRoom sc ON sc.Id=cc.RoomId LEFT JOIN Co_CoursePaper AS ccp ON (ccp.CourseId=clc.CourseId and ccp.IsMain=0) WHERE cc.IsDelete=0 AND cc.CourseFrom=2 AND clc.CpaFlag=0 AND clc.IsPass=1 AND clc.GetLength>=0 and
  cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserId)  and cc.YearPlan=@YearPlan and clc.UserId=@UserId and " + where + ") result WHERE numb BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    YearPlan = year,
                    UserId = userid
                };
                return conn.Query<Co_CourseShow>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 我的年度培训目标(CPA)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="year"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyCPACoursebyway(int userid, int year, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY cc.Id " + Order + @") numb,count(*)OVER(PARTITION BY null) totalcount,sum(clc.GetLength)OVER(PARTITION BY null) AS passsum,cc.Id,cc.CourseName,CourseLength,Way,StartTime,EndTime,CourseAddress,clc.GetLength as GetScore,clc.CpaFlag FROM Cl_CpaLearnStatus AS clc LEFT JOIN Co_Course AS cc ON cc.Id=clc.CourseId WHERE cc.YearPlan=@YearPlan AND cc.IsDelete=0 AND cc.CourseFrom=2 and 
cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserID) 
and clc.CpaFlag=1 and clc.GradeStatus=1 AND clc.GetLength>=0 and clc.UserID=@UserID and " + where + ") result WHERE numb BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    YearPlan = year,
                    UserID = userid
                };
                return conn.Query<Co_CourseShow>(sql, param).ToList();
            }
        }

        /// <summary>
        /// CPA年度课程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bentime"></param>
        /// <param name="endtime"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyCPACourse(int userid, int year, string Order = "DESC", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"
WITH courseInfo AS
(
  SELECT sum(clcs.GetLength) GetLength,clcs.CourseID 
FROM Cl_CpaLearnStatus AS clcs 
LEFT JOIN Co_Course AS cc ON cc.Id=clcs.CourseId 
WHERE cc.YearPlan=@YearPlan AND cc.IsDelete=0 AND cc.CourseFrom=2 
and cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserId) 
and clcs.UserID=@UserId AND cc.Way=3 AND cc.IsMust=1 
and clcs.CpaFlag>0 and clcs.GradeStatus=1
GROUP BY clcs.CourseID  
)

SELECT row_number()OVER(ORDER BY temp.StartTime " + Order + @") numb,
sum(temp.GetLength)OVER(PARTITION BY null) AS passsum,*
FROM (SELECT isnull((SELECT GetLength FROM courseInfo WHERE CourseID=clc.CourseID),0) AS daode,
cc.CourseName,CourseLength,Way,StartTime,EndTime,clc.GetLength,clc.CpaFlag,CPAForm=0 
FROM Cl_CpaLearnStatus AS clc 
LEFT JOIN Co_Course AS cc ON cc.Id=clc.CourseId 
WHERE cc.YearPlan=@YearPlan AND cc.IsDelete=0 AND cc.CourseFrom=2 and 
cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserId) 
and clc.CpaFlag>0 and clc.GradeStatus=1 AND clc.GetLength>=0 and clc.UserID=@UserId  
UNION 
SELECT daode=0,dc.CourseName,CourseLength,Way=0,StartTime,EndTime,dcls.GetLength,dcls.CpaFlag,CPAForm=1 
FROM Dep_CpaLearnStatus AS dcls 
LEFT JOIN Sys_User AS su ON dcls.UserID=su.UserId
LEFT JOIN Dep_Course AS dc ON dc.Id=dcls.CourseID 
LEFT JOIN Dep_CourseDept AS dcd ON (dcd.CourseId=dcls.CourseID AND dcd.DepartId=su.DeptId)
WHERE dcd.AttFlag=1 AND dcd.ApprovalFlag=1 AND dc.IsDelete=0 AND dcls.GetLength>=0 AND dcls.UserID=@UserId AND dc.YearPlan=@YearPlan) AS temp 
WHERE " + where;
                var param = new
                {
                    YearPlan = year,
                    UserID = userid
                };
                return conn.Query<Co_CourseShow>(sql, param).ToList();
            }
        }

        /// <summary>
        /// CPA周期课程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bentime"></param>
        /// <param name="endtime"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyCPACourse(int userid, string bentime, string endtime, string Order = "DESC", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"
 WITH courseInfo AS
(
  SELECT sum(clcs.GetLength) GetLength,clcs.CourseID 
	FROM Cl_CpaLearnStatus AS clcs 
	LEFT JOIN Co_Course AS cc ON cc.Id=clcs.CourseId 
	WHERE ( cc.StartTime BETWEEN '" + bentime + "' AND '" + endtime + @"' )  AND cc.IsDelete=0 AND cc.CourseFrom=2 
	and cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserId) 
	and clcs.UserID=@UserId AND cc.Way=3 AND cc.IsMust=1 
	and clcs.CpaFlag>0 and clcs.GradeStatus=1
	GROUP BY clcs.CourseID  
)
SELECT row_number()OVER(ORDER BY temp.StartTime " + Order + @") numb,
sum(temp.GetLength)OVER(PARTITION BY null) AS passsum,*
FROM (SELECT isnull((
SELECT GetLength FROM courseInfo WHERE CourseID=clc.CourseID),0) AS daode,
cc.CourseName,CourseLength,Way,StartTime,EndTime,clc.GetLength,clc.CpaFlag,CPAForm=0,cc.YearPlan  
FROM Cl_CpaLearnStatus AS clc 
LEFT JOIN Co_Course AS cc ON cc.Id=clc.CourseId 
WHERE ( cc.StartTime BETWEEN '" + bentime + "' AND '" + endtime + @"' ) AND cc.IsDelete=0 AND cc.CourseFrom=2 and 
cc.Id NOT IN( SELECT CourseId FROM View_DepCourseLearn WHERE UserId=@UserID) 
and clc.CpaFlag>0 and clc.GradeStatus=1 AND clc.GetLength>=0 and clc.UserID=@UserID 
UNION 
SELECT daode=0,dc.CourseName,CourseLength,Way=0,StartTime,EndTime,dcls.GetLength,dcls.CpaFlag,CPAForm=1,dc.YearPlan  
FROM Dep_CpaLearnStatus AS dcls 
LEFT JOIN Dep_Course AS dc ON dc.Id=dcls.CourseID 
WHERE  dc.IsDelete=0 AND dcls.GetLength>=0 AND dcls.UserID=@UserID AND 
( dc.StartTime BETWEEN '" + bentime + "' AND '" + endtime + @"' )) AS temp WHERE " + where;

                var param = new
                {
                    UserID = userid
                };
                return conn.Query<Co_CourseShow>(sql, param).ToList();
            }
        }
        #endregion


        /// <summary>
        /// 找出人员表的人员分类
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserType()
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = " select DISTINCT  [UserType] from [Sys_User]";

                return connection.Query<string>(sql).ToList();
            }
        }

        /// <summary>
        /// 以当前人为领导的部门
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<int> GetLeardForUserID(int userID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = @" SELECT distinct DeptId from Sys_User
WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId=" + userID + ")";

                return connection.Query<int>(sql).ToList();
            }
        }

        //		/// <summary>
        //		/// CPA年度课程
        //		/// </summary>
        //		public List<Co_CourseShow> GetCPACourseShow(int userid, int year, string Order = "DESC", string where = "1=1")
        //		{
        //			using (var conn = OpenConnection())
        //			{
        //				string sql = string.Format(@"SELECT row_number()OVER(ORDER BY {0} desc) numb,* FROM view_CPAScore
        //WHERE UserID={1} AND YearPlan={2} and {3}", Order, userid, year, where);
        //				return conn.Query<Co_CourseShow>(sql).ToList();
        //			}
        //		}
    }
}