/*�޸����ã��Ƿ���Ҫ�¶ȴ��*/
update sys_paramconfig set configvalue='0,0' where configtype=45

/*�޸�Ȩ������*/
update sys_right set rightdesc='�����ϱ����ڹ���' where rightname ='DepMinAttendceList'

/*�����ֶ�*/
ALTER TABLE Dep_CourseOrder ADD ApprovalUser nvarchar(50) DEFAULT ('');
go
ALTER TABLE Dep_CourseOrder ADD ApprovalLimitTime datetime;
go
ALTER TABLE Dep_CourseOrder ADD requestid int Default(0);
go
ALTER TABLE Dep_CourseOrder ADD FtriggerFlag int Default(0);
go

/****** Object:  View [dbo].[View_DepLeave] [���ŷ����������]   Script Date: 09/06/2013 08:50:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[View_DepLeave]
    as
    select 
		a.Id,                                                                            --�������
		b.loginId as LoginName,                                                          --��¼��
		a.IsLeave,                                                                       --�Ƿ����(0��δ��٣�1�����)
		a.ApprovalUser,                                                                  --�����ˣ�HRID��
		(select username from sys_user where JobNum=ApprovalUser) as LoginId,            --������LoginID
		b.JobNum as LeaveUserID,                                                                   --������ID
		b.Realname,                                                                      --���������
		a.Reason,                                                                        --�������
		c.CourseName,                                                                    --�γ�����
		convert(nvarchar(10),a.LeaveTime,120) as LeaveDate,                              --�������
		convert(nvarchar(10),a.LeaveTime,108) as LeaveTime,                              --���ʱ��
		convert(nvarchar(10),c.StartTime,120) as CourseStartDate,                        --�γ̿�ʼ����
		convert(nvarchar(10),c.StartTime,108) as CourseStartTime,                        --�γ̿�ʼʱ��
		convert(nvarchar(10),c.EndTime,120) as CourseEndDate,                            --�γ̿�ʼ����  
		convert(nvarchar(10),c.EndTime,108) as CourseEndTime,                            --�γ̿�ʼ����
		a.AppReason as ApprovalMemo,                                                     --��������
		a.ApprovalFlag,                                                                  --����״̬
		convert(nvarchar(10),a.ApprovalLimitTime,120) as ApprovalLimitDate,              --������ֹ����
		convert(nvarchar(10),a.ApprovalLimitTime,108) as ApprovalLimitTime,               --������ֹʱ��
		a.requestid,
		a.FtriggerFlag
	from Dep_CourseOrder as a 
	left join Sys_User as b on a.UserId=b.UserId 
	left join Dep_Course as c on a.CourseId=c.Id
	where a.IsLeave=1 and a.ApprovalFlag=0

GO

/*�޸�Ȩ��*/
update sys_right set path='MyApproval/MyApprovalIndex' where rightname='�ҵ�������¼'
go
Delete from sys_right where rightname='DepMinAttendceList'
go
Delete from sys_right where rightname='DepLeaveApprovalManage'
go

/*�γ̱�*/
ALTER TABLE Co_Course ADD DepCourseId int DEFAULT (0);
GO
UPDATE Co_Course SET DepCourseId=0 WHERE DepCourseId IS NULL
go

/*����������һ�ڡ�����ѧʱת����*/
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
/*����������һ�ڡ�����ѧʱת����*/
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

