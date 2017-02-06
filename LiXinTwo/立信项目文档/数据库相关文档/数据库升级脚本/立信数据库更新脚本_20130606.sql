
---���������ű�--2013-06-06--

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

--����Ȩ��--

set identity_insert Sys_Right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (172, '�鿴CPA�γ�����', 1, '�鿴CPA�γ�����', 'CourseManage/FromCourseCPAList', 50, NULL, 'MyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (173, '���������ʱ����', 1, '���������ʱ����', 'MyApply/MyTimeOutApplyDetail', 154, NULL, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (174, '���������ʱ�������� ', 1, '���������ʱ�������� ', 'MyApply/MyTimeOutApprovalDetail', 153, NULL, 'TrainManage')
GO

set identity_insert Sys_Right off

go

--���������ʱ����������ͼ(�ṩ��OA)--
--��ʱ���������ͼ
CREATE view [dbo].[View_TimeOutLeave]
as
select
	Id,                   --����
	b.loginId as LoginName, --���ѧԱ��¼��LoginID
	ApprovalUser,         --�����ˣ�HRID��
	(select username from sys_user where JobNum=ApprovalUser) as LoginId,--�����˵�¼��LoginID
	OutUserID,            --���ѧԱѧԱHRID
	Name,                 --���ѧԱѧԱ����
	ApprovalReason,       --��ѵ����Ա��������
	CourseName,           --�γ�����
	
	convert(nvarchar(10),MakeUpTime,120) as OutDate,--�����������
	convert(nvarchar(10),MakeUpTime,108) as outTime,--�������ʱ��
	
	convert(nvarchar(10),CourseStartTime,120) as CourseStartDate,--�γ̿�ʼ����
	convert(nvarchar(10),CourseStartTime,108) as CourseStartTime,--�γ̿�ʼʱ��
	
	convert(nvarchar(10),CourseEndTime,120) as CourseEndDate,--�γ̽�������
	convert(nvarchar(10),CourseEndTime,108) as CourseEndTime,--�γ̽���ʱ��
	
	ApprovalMemo, --��������
	ApprovalFlag, --����״̬
	convert(nvarchar(10),ApprovalDateTime,120) as ApprovalDate,--��������
	convert(nvarchar(10),ApprovalDateTime,108) as ApprovalTime,--����ʱ��
	requestid,
	FtriggerFlag
from Cl_TimeOutOrder as a left join Sys_User as b on a.UserId=b.UserId where ApprovalFlag=0
GO