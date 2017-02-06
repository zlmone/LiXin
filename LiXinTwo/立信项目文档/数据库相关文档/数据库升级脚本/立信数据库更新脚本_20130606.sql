
---立信升级脚本--2013-06-06--

ALTER TABLE Co_Course ADD IsOpenSub int DEFAULT (1);
go

update Co_Course set IsOpenSub=1 where IsOpenSub is NULL
go

ALTER TABLE Sys_User ADD IsMain int DEFAULT (0);
go

/****** Object:  Table [dbo].[Cl_TimeOutLeave]    Script Date: 06/08/2013 18:09:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Cl_TimeOutLeave](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[ApprovalUser] [nvarchar](50) NULL,
	[AppReason] [nvarchar](1000) NULL,
	[OutTime] [datetime] NULL,
	[TrainAppFlag] [int] NULL,
	[ApprovalReason] [nvarchar](1000) NULL,
	[OutUserID] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[MakeUpTime] [datetime] NULL,
	[CourseName] [nvarchar](50) NULL,
	[CourseStartTime] [datetime] NULL,
	[CourseEndTime] [datetime] NULL,
	[ApprovalMemo] [nvarchar](1000) NULL,
	[ApprovalFlag] [int] NULL,
	[ApprovalDateTime] [datetime] NULL,
	[requestid] [int] NULL,
	[FtriggerFlag] [int] NULL,
	[TrainAppDatetime] [datetime] NULL,
 CONSTRAINT [PK_CL_TIMEOUTLEAVE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--新增权限--

set identity_insert Sys_Right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (172, '查看CPA课程详情', 1, '查看CPA课程详情', 'CourseManage/FromCourseCPAList', 50, NULL, 'MyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (173, '请假申请逾时详情', 1, '请假申请逾时详情', 'MyApply/MyTimeOutApplyDetail', 154, NULL, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (174, '请假申请逾时审批详情 ', 1, '请假申请逾时审批详情 ', 'MyApply/MyTimeOutApprovalDetail', 153, NULL, 'TrainManage')
GO

set identity_insert Sys_Right off

go

--新增请假逾时申请审批视图(提供给OA)--
--逾时请假申请视图
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
from Cl_TimeOutOrder as a left join Sys_User as b on a.UserId=b.UserId where ApprovalFlag=0
GO