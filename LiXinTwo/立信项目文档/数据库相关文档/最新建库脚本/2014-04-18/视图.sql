/****** Object:  View [dbo].[View_CheckUser]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_CheckUser]
as
  
 SELECT UserId, DeptId FROM Sys_User
 WHERE DeptId IN (
 SELECT ID FROM F_GetCheckDeptID(54))
AND IsDelete=0 AND   Status=0    
AND UserType IN ('在职','见习','特批','聘用')    
                      --SELECT * FROM    View_CheckUser
GO
/****** Object:  View [dbo].[View_TimeOutOrder]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--补预定视图
    CREATE view [dbo].[View_TimeOutOrder]
    as
    select
		Id,                   --主键
		b.loginId as LoginName, --登录名
		ApprovalUser,         --审批人（HRID）
		(select username from sys_user where JobNum=ApprovalUser) as LoginId,--审批人LoginID
		OutUserID,            --逾时学员HRID
		Name,                 --逾时学员姓名
		ApprovalReason,       --培训管理员审批理由
		CourseName,           --课程名称
		
		convert(nvarchar(10),OutTime,120) as OutDate,--逾时申请日期
		convert(nvarchar(10),OutTime,108) as outTime,--逾时申请时间
		
		convert(nvarchar(10),CourseStartTime,120) as CourseStartDate,--课程开始日期
		convert(nvarchar(10),CourseStartTime,108) as CourseStartTime,--课程开始时间
		
		convert(nvarchar(10),CourseEndTime,120) as CourseEndDate,--课程结束日期
		convert(nvarchar(10),CourseEndTime,108) as CourseEndTime,--课程结束时间
		
		convert(nvarchar(10),AttStartTime,120) as AttStartDate,--考勤开始日期
		convert(nvarchar(10),AttStartTime,108) as AttStartTime,--考勤开始时间
		
		convert(nvarchar(10),AttEndTime,120) as AttEndDate,--考勤结束日期
		convert(nvarchar(10),AttEndTime,108) as AttEndTime,--考勤结束时间
		
		ApprovalMemo, --审批理由
		ApprovalFlag, --审批状态
		convert(nvarchar(10),ApprovalDateTime,120) as ApprovalDate,--审批日期
		convert(nvarchar(10),ApprovalDateTime,108) as ApprovalTime,--审批时间
		requestid,
		FtriggerFlag
	from Cl_TimeOutOrder as a left join Sys_User as b on a.UserId=b.UserId where ApprovalFlag=0
GO
/****** Object:  View [dbo].[View_TimeOutLeave]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[View_TimeOutLeave]
as
select
	Id,                   --主键
	b.loginId as LoginName, --请假学员登录名LoginID
	ApprovalUser,         --审批人（HRID）
	(select username from sys_user where JobNum=ApprovalUser) as LoginId,--审批人登录名LoginID
	OutUserID,            --请假学员学员HRID
	Name,                 --请假学员学员姓名
	ApprovalReason,       --培训管理员审批理由
	CourseName,           --课程名称
	
	convert(nvarchar(10),MakeUpTime,120) as OutDate,--请假申请日期
	convert(nvarchar(10),MakeUpTime,108) as outTime,--请假申请时间
	
	convert(nvarchar(10),CourseStartTime,120) as CourseStartDate,--课程开始日期
	convert(nvarchar(10),CourseStartTime,108) as CourseStartTime,--课程开始时间
	
	convert(nvarchar(10),CourseEndTime,120) as CourseEndDate,--课程结束日期
	convert(nvarchar(10),CourseEndTime,108) as CourseEndTime,--课程结束时间
	
	ApprovalMemo, --审批理由
	ApprovalFlag, --审批状态
	convert(nvarchar(10),ApprovalDateTime,120) as ApprovalDate,--审批日期
	convert(nvarchar(10),ApprovalDateTime,108) as ApprovalTime,--审批时间
	requestid,
	FtriggerFlag
from Cl_TimeOutLeave as a left join Sys_User as b on a.UserId=b.UserId where ApprovalFlag=0
GO
/****** Object:  View [dbo].[View_OrderTimes]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create VIEW [dbo].[View_OrderTimes]
as
	SELECT * FROM (
	SELECT row_number()OVER(PARTITION BY UserId ORDER BY ApprovalDateTime ASC ) number ,* FROM(
	SELECT CourseId,cc.YearPlan, UserId,  ApprovalDateTime  FROM Cl_MakeUpOrder
	LEFT JOIN Co_Course cc ON cc.Id=Cl_MakeUpOrder.CourseId
	WHERE ( ApprovalFlag=1 AND  ApprovalDateTime<=ApprovalLimitTime) 
	--OR (ApprovalFlag=0 AND ApprovalLimitTime>=getdate()) 
	UNION
	SELECT CourseId,cc.YearPlan, UserId,  ApprovalDateTime  FROM Cl_TimeOutOrder
   LEFT JOIN Co_Course cc ON cc.Id=Cl_TimeOutOrder.CourseId
	WHERE ApprovalFlag=1 
	) t )result
GO
/****** Object:  View [dbo].[View_MakeUpOrder]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--补预定视图
    CREATE view [dbo].[View_MakeUpOrder] 
    as
    select
		Id,           --主键
		b.loginId as LoginName, --登录名
		ApprovalUser, --审批人（HRID）
		(select username from sys_user where JobNum=ApprovalUser) as LoginId,--审批人LoginID
		LeaveUserID,  --补预定人HRID
		Name,         --补预定人姓名
		CourseName,   --课程名称
		convert(nvarchar(10),LeaveTime,120) as LeaveDate,--补预定申请日期
		convert(nvarchar(10),LeaveTime,108) as LeaveTime,--补预定申请时间
		convert(nvarchar(10),CourseStartTime,120) as CourseStartDate,--课程开始日期
		convert(nvarchar(10),CourseStartTime,108) as CourseStartTime,--课程开始时间
		convert(nvarchar(10),CourseEndTime,120) as CourseEndDate,--课程结束日期
		convert(nvarchar(10),CourseEndTime,108) as CourseEndTime,--课程结束日期
		convert(nvarchar(10),AttStartTime,120) as AttStartDate,--考勤开始日期
		convert(nvarchar(10),AttStartTime,108) as AttStartTime,--考勤开始时间
		convert(nvarchar(10),AttEndTime,120) as AttEndDate,--考勤结束日期
		convert(nvarchar(10),AttEndTime,108) as AttEndTime,--考勤结束日期
		ApprovalMemo, --审批理由
		ApprovalFlag, --审批状态
		convert(nvarchar(10),ApprovalDateTime,120) as ApprovalDate,--审批日期
		convert(nvarchar(10),ApprovalDateTime,108) as ApprovalTime,--审批时间
		convert(nvarchar(10),ApprovalLimitTime,120) as ApprovalLimitDate,--审批截止日期
		convert(nvarchar(10),ApprovalLimitTime,108) as ApprovalLimitTime,--审批截止时间
		requestid,
		FtriggerFlag
	from Cl_MakeUpOrder as a left join Sys_User as b on a.UserId=b.UserId where ApprovalFlag=0
GO
/****** Object:  View [dbo].[View_Leave]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[View_Leave]
    as
    select 
		Id,           --主键
		b.loginId as LoginName, --登录名
		IsLeave,      --是否请假(0：未请假；1：请假)
		ApprovalUser, --审批人（HRID）
		(select username from sys_user where JobNum=ApprovalUser) as LoginId,--审批人LoginID
		LeaveUserID,  --申请人ID
		Name,         --请假人
		Reson,        --请假理由
		CourseName,   --课程名称
		convert(nvarchar(10),LeaveTime,120) as LeaveDate,--请假日期
		convert(nvarchar(10),LeaveTime,108) as LeaveTime,--请假时间
		convert(nvarchar(10),CourseStartTime,120) as CourseStartDate,--课程开始日期
		convert(nvarchar(10),CourseStartTime,108) as CourseStartTime,--课程开始时间
		convert(nvarchar(10),CourseEndTime,120) as CourseEndDate,--课程开始日期
		convert(nvarchar(10),CourseEndTime,108) as CourseEndTime,--课程开始日期
		ApprovalMemo, --审批理由
		ApprovalFlag, --审批状态
		convert(nvarchar(10),ApprovalDateTime,120) as ApprovalDate,--审批日期
		convert(nvarchar(10),ApprovalLimitTime,120) as ApprovalLimitDate,--审批截止日期
		convert(nvarchar(10),ApprovalLimitTime,108) as ApprovalLimitTime,--审批截止时间
		requestid,
		FtriggerFlag
	from Cl_CourseOrder as a left join Sys_User as b on a.UserId=b.UserId where IsLeave=1 and ApprovalFlag=0
GO
/****** Object:  View [dbo].[View_DepLeave]    Script Date: 04/18/2014 09:01:42 ******/
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
/****** Object:  View [dbo].[View_UserInfo]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_UserInfo]
as
SELECT su.UserId,su.IsMain,su.TrainGrade,  DeptId, su.DeptName, isnull(sg.GroupId,0) GroupId,isnull(sgp.GroupName,'')GroupName FROM Sys_User su
LEFT JOIN  Sys_GroupUser sg ON su.UserId=sg.UserId
LEFT JOIN Sys_Group sgp ON sg.GroupId=sgp.GroupId
WHERE su.IsDelete=0 AND su.Status=0
-- AND DeptId IN (SELECT DISTINCT DeptID FROM dbo.View_CheckUser)
GO
/****** Object:  View [dbo].[VIEW_CpaUserScore]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VIEW_CpaUserScore]
 as
 SELECT dc.YearPlan,dc.StartTime,dc.EndTime, dcs.UserID,0 tScore ,sum(GetLength) dscore,0 IsMust
	FROM  Dep_CpaLearnStatus dcs LEFT JOIN
	Dep_Course dc ON dc.Id = dcs.CourseID 
	 LEFT JOIN Dep_CourseDept AS dcd ON (dcd.CourseId=dcs.CourseId AND dcd.DepartId=dcs.DepartSetId) 
	 WHERE CpaFlag=0 AND  dcd.AttFlag=1 AND dcd.ApprovalFlag=1
	GROUP BY dcs.UserID, dc.YearPlan,dc.StartTime,dc.EndTime
	UNION
    SELECT  cc.YearPlan,cc.StartTime,cc.EndTime, ccs.UserID,sum(GetLength) tScore,0 dscore,CASE WHEN cc.Way=3 THEN cc.IsMust ELSE 0 END IsMust 
	FROM   Cl_CpaLearnStatus ccs LEFT JOIN
	       Co_Course cc ON cc.Id = ccs.CourseID 
	WHERE   CpaFlag = 2 OR (CpaFlag = 1 AND ccs.GradeStatus = 1)
	GROUP BY ccs.UserID, cc.YearPlan,cc.StartTime,cc.EndTime,Way,IsMust
GO
/****** Object:  View [dbo].[view_CPAScore]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_CPAScore]
AS
    /*集中 视频 折算成的*/
	SELECT  cc.CourseName, 0 courseType, cc.Way, cc.YearPlan, StartTime, EndTime, ccs.UserID,
	 sum(DISTINCT GetLength) tScore, 0 dScore, 0 cpaScore,0 IsMust
	 --, sys_user.DeptPath,  sys_user.TrainGrade, sys_user.realname
	FROM   Cl_CpaLearnStatus ccs LEFT JOIN
	       Co_Course cc ON cc.Id = ccs.CourseID 
	       --sys_user ON ccs.UserID = sys_user.UserId
	WHERE   CpaFlag = 2
	GROUP BY ccs.UserID, cc.YearPlan, StartTime, EndTime, cc.Way, cc.CourseName,cc.Id
	UNION
	/*部门*/
	 SELECT dc.CourseName, 0 courseType, 3, dc.YearPlan, StartTime, EndTime, dcs.UserID, 0 tScore, sum(GetLength) dScore, 0 cpaScore
	,0 IsMust
	FROM  Dep_CpaLearnStatus dcs LEFT JOIN
	Dep_Course dc ON dc.Id = dcs.CourseID 
	LEFT JOIN Dep_CourseDept AS dcd ON (dcd.CourseId=dcs.CourseId AND dcd.DepartId=dcs.DepartSetId) 
	 WHERE CpaFlag=0 AND  dcd.AttFlag=1 AND dcd.ApprovalFlag=1
	GROUP BY dcs.UserID, dc.YearPlan, StartTime, EndTime, dc.CourseName
	UNION
	/*cpa导入的*/ 
	SELECT cc.CourseName, 2 courseType, - 1, cc.YearPlan, StartTime, EndTime, ccs.UserID, 
	0 tScore, 0 dScore, sum(GetLength) cpaScore,cc.IsMust
	FROM   Cl_CpaLearnStatus ccs LEFT JOIN
	        Co_Course cc ON cc.Id = ccs.CourseID 
	WHERE     CpaFlag = 1 AND ccs.GradeStatus = 1  
    GROUP BY ccs.UserID, cc.YearPlan, StartTime, EndTime, cc.CourseName,cc.IsMust
GO
/****** Object:  View [dbo].[view_CourseOrderInfo]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_CourseOrderInfo]
 as
	SELECT nco.UserId,nco.CourseId,TogetherScore, GroupScore, CourseExamScore, ExamScore, CourseExamSumScore, ExamSumScore,nc.IsGroupTeach
	FROM  New_CourseOrder nco
	LEFT JOIN New_Course nc ON nco.CourseId=nc.Id
GO

/****** Object:  View [dbo].[View_TogetherAtt]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create VIEW [dbo].[View_TogetherAtt]
as
SELECT co.YearPlan, co.Id,cco.UserId,syu.DeptId,syu.TrainGrade,syu.PayGrade,syu.CPA FROM Cl_CourseOrder cco
LEFT JOIN Cl_Attendce ca ON cco.CourseId=ca.CourseId AND cco.UserId=ca.UserId
LEFT JOIN Co_Course co ON co.Id=cco.CourseId
LEFT JOIN Sys_User syu ON syu.UserId=cco.UserId
WHERE  ca.Id IS NULL 
AND cco.UserId IN (SELECT UserId FROM View_CheckUser)
AND cco.[OrderStatus] in (1,3)
  and ( cco.[IsLeave] = 0 
        or (cco.[IsLeave] = 1 and cco.[ApprovalFlag] <>1)
        or (cco.[IsLeave] = 1 and cco.[ApprovalFlag] = 1 
        and cco.[ApprovalDateTime] > cco.[ApprovalLimitTime])
      )
GO
/****** Object:  View [dbo].[View_DepCourseLearn]    Script Date: 04/18/2014 09:01:42 ******/
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
/****** Object:  View [dbo].[view_VedioLearnBase]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_VedioLearnBase]
as
SELECT  ccs.CourseID,clv.LearnTimes,ccs.UserID,ccs.IsPass,ccs.VedioTime,CpaFlag FROM  
Cl_CpaLearnStatus  ccs 
LEFT JOIN Cl_LearnVideoInfor clv ON ccs.Id=clv.LearnId
LEFT JOIN 
(SELECT * FROM Co_Course WHERE Way=2 AND Publishflag=1
	AND IsDelete=0  
	AND CourseFrom=2
) cc on cc.Id=ccs.CourseID
 where CpaFlag=0
 AND  UserID IN (SELECT UserId FROM dbo.View_CheckUser)
GO
/****** Object:  View [dbo].[View_VedioUserDept]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_VedioUserDept]
as
	SELECT  cc.Id,view_base.UserID,su.DeptId,sum(view_base.LearnTimes) LearnTimes,isnull(view_base.VedioTime,0) VedioTime  FROM
	 Co_Course  cc
    LEFT join view_VedioLearnBase view_base ON view_base.CourseID=cc.Id
	LEFT JOIN Sys_User su ON su.UserId=view_base.UserID
	WHERE Way=2 AND Publishflag=1
		AND cc.IsDelete=0  
		AND CourseFrom=2 
	     
     GROUP BY   cc.Id,view_base.UserID,su.DeptId,view_base.VedioTime
GO
/****** Object:  View [dbo].[View_VedioCourseLearn]    Script Date: 04/18/2014 09:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW  [dbo].[View_VedioCourseLearn]
as
		WITH vedioReally AS --人次
	(    
	    SELECT CourseID,sum(isnull(LearnTimes,0))  ReallyNumber,count(DISTINCT userID) LearnNumber  FROM view_VedioLearnBase
        GROUP BY courseID
	) ,passnumber AS
	(
	  
	    SELECT CourseID,count(DISTINCT userID) passNumber FROM view_VedioLearnBase
        WHERE ispass=1 AND CpaFlag=0
        GROUP BY courseID
	)
	
	SELECT   cc.Id CourseId,isnull(ccp.Id,0) CoursePaperId,  cc.Year,cc.YearPlan,cc.IsTest,cc.IsPing,cc.SurveyPaperId,  cc.CourseName, 
	cc.IsOpenSub,
	vr.LearnNumber,isnull(vr. ReallyNumber,0) ReallyNumber
	,pu.passNumber
     
	FROM Co_Course  cc
	LEFT JOIN Co_CoursePaper ccp ON cc.Id=ccp.CourseId
	left JOIN vedioReally vr ON vr.CourseID=cc.Id
    left JOIN   passnumber pu ON pu.CourseID=cc.Id
    WHERE Way=2 AND Publishflag=1
	AND IsDelete=0  
	AND CourseFrom=2
	GROUP BY  cc.Id ,cc.Year,cc.IsTest,cc.IsPing,cc.SurveyPaperId,  cc.CourseName,ReallyNumber,ccp.Id,cc.YearPlan
	,pu.passNumber,vr.LearnNumber,cc.IsOpenSub
GO
