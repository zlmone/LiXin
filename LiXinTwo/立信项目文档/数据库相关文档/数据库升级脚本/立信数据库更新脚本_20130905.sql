/*修改配置：是否需要月度大纲*/
update sys_paramconfig set configvalue='0,0' where configtype=45

/*修改权限描述*/
update sys_right set rightdesc='联合上报考勤管理' where rightname ='DepMinAttendceList'

/*新增字段*/
ALTER TABLE Dep_CourseOrder ADD ApprovalUser nvarchar(50) DEFAULT ('');
go
ALTER TABLE Dep_CourseOrder ADD ApprovalLimitTime datetime;
go
ALTER TABLE Dep_CourseOrder ADD requestid int Default(0);
go
ALTER TABLE Dep_CourseOrder ADD FtriggerFlag int Default(0);
go

/****** Object:  View [dbo].[View_DepLeave] [部门分所请假审批]   Script Date: 09/06/2013 08:50:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[View_DepLeave]
    as
    select 
		a.Id,                                                                            --请假主键
		b.loginId as LoginName,                                                          --登录名
		a.IsLeave,                                                                       --是否请假(0：未请假；1：请假)
		a.ApprovalUser,                                                                  --审批人（HRID）
		(select username from sys_user where JobNum=ApprovalUser) as LoginId,            --审批人LoginID
		b.JobNum as LeaveUserID,                                                                   --申请人ID
		b.Realname,                                                                      --请假人姓名
		a.Reason,                                                                        --请假理由
		c.CourseName,                                                                    --课程名称
		convert(nvarchar(10),a.LeaveTime,120) as LeaveDate,                              --请假日期
		convert(nvarchar(10),a.LeaveTime,108) as LeaveTime,                              --请假时间
		convert(nvarchar(10),c.StartTime,120) as CourseStartDate,                        --课程开始日期
		convert(nvarchar(10),c.StartTime,108) as CourseStartTime,                        --课程开始时间
		convert(nvarchar(10),c.EndTime,120) as CourseEndDate,                            --课程开始日期  
		convert(nvarchar(10),c.EndTime,108) as CourseEndTime,                            --课程开始日期
		a.AppReason as ApprovalMemo,                                                     --审批理由
		a.ApprovalFlag,                                                                  --审批状态
		convert(nvarchar(10),a.ApprovalLimitTime,120) as ApprovalLimitDate,              --审批截止日期
		convert(nvarchar(10),a.ApprovalLimitTime,108) as ApprovalLimitTime,               --审批截止时间
		a.requestid,
		a.FtriggerFlag
	from Dep_CourseOrder as a 
	left join Sys_User as b on a.UserId=b.UserId 
	left join Dep_Course as c on a.CourseId=c.Id
	where a.IsLeave=1 and a.ApprovalFlag=0

GO

/*修改权限*/
update sys_right set path='MyApproval/MyApprovalIndex' where rightname='我的审批记录'
go
Delete from sys_right where rightname='DepMinAttendceList'
go
Delete from sys_right where rightname='DepLeaveApprovalManage'
go

/*课程表*/
ALTER TABLE Co_Course ADD DepCourseId int DEFAULT (0);
GO
UPDATE Co_Course SET DepCourseId=0 WHERE DepCourseId IS NULL
go

/*新增函数：一期、二期学时转换用*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW   [dbo].[View_DepCourseLearn] 
 AS
 WITH courseInfo AS
 ( 
	SELECT CourseId,cc.DepCourseId,cco.UserId,a.DeptId,dbo.F_GetDeptIDByCourse(cc.DepCourseId) linkDept,
	CASE WHEN dbo.F_IsAppStatus(CourseId,cco.UserId)=1 THEN GetScore ELSE 0 END GetScore,
	cc.CourseName,cc.IsMust,cc.StartTime,cc.EndTime,teacher.Realname teacherName,scr.RoomName,cc.IsYearPlan,cc.CourseLength,
	a.Realname, a.Sex, a.MobileNum, a.Email, a.TrainGrade,a.CPA,cc.YearPlan
	 FROM Cl_CourseOrder cco
	LEFT JOIN Co_Course cc ON cco.CourseId=cc.Id
	LEFT JOIN Sys_User a ON a.UserId= cco.UserId
	LEFT JOIN Sys_User teacher ON teacher.UserId= cc.Teacher
	LEFT JOIN Sys_ClassRoom scr ON scr.Id=cc.RoomId
	WHERE cc.DepCourseId>0
   )
   
   SELECT * FROM    courseInfo
   WHERE (SELECT COUNT(*) FROM dbo.F_SplitIDs(linkDept) WHERE ID=DeptId)>0
GO
/*新增函数：一期、二期学时转换用*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[F_GetDeptIDByCourse]
(
 @courseID INT
)

RETURNS NVARCHAR(max)     as
 BEGIN
       
		DECLARE @year INT,@deptID INT,@yearID INT,@deptidstr NVARCHAR(max)  
		SELECT @year=Year,@deptID=DeptId FROM Dep_Course
		WHERE Id=@courseID
		
		 	 SET @deptidstr=@deptID
			SELECT @deptidstr=@deptidstr+','+ltrim(str(dy.DeptId)) FROM Dep_LinkDepart dl
			LEFT JOIN   Dep_YearPlan   dy  ON dl.YearId=dy.Id
		   WHERE Year=@year AND (dy.DeptId=@deptID OR (dl.DeptId=@deptID and dl.ApprovalFlag=1) )
			
		RETURN    @deptidstr
END
GO

