


--教室表：添加地址--
ALTER TABLE Sys_ClassRoom ADD Address nvarchar(100) DEFAULT ('');
GO

--用户表：添加是否新进员工--
ALTER TABLE Sys_User ADD IsNew int;
GO

--用户表：添加新进员工学号--
ALTER TABLE Sys_User ADD NumberID nvarchar(50) null;
GO

--用户表：添加新进员工学号是否固定--
ALTER TABLE Sys_User ADD IsChangeNumberId int null;
GO


/****** 新进员工班级表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[New_Class](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[PersonCount] [int] NULL,
	[Year] [int] NULL,
	[ClassNo] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NULL,
	[ClassIndex] [int] NULL,
	[IsDoDelete] [int] NULL,
 CONSTRAINT [PK_New_Class] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** 新进员工小组表 ******/
CREATE TABLE [dbo].[New_Group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassId] [int] NULL,
	[GroupName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[PersonCount] [int] NULL,
	[GroupNo] [nvarchar](10) COLLATE Chinese_PRC_CI_AS NULL,
	[GroupIndex] [int] NULL,
 CONSTRAINT [PK_New_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** 新进员工小组人员表 ******/
create table New_GroupUser 
(
   Id                   int                            null,
   ClassId              int                            null,
   GroupId              int                            null,
   UserId               int                            null,
   NumberID             nvarchar(50)                   null
);
GO

/****** 新进员工课程表 ******/
create table New_Course 
(
   Id                   int                            null,
   CourseName           nvarchar(50)                   null,
   Code                 nvarchar(50)                   null,
   IsGroupTeach         int                            null,
   Teachers             nvarchar(200)                   null,
   Classes              nvarchar(200)                   null,
   StartTime            datetime                       null,
   EndTime              datetime                       null,
   IsPingTeacher        int                            null,
   ScoreDistribute      nvarchar(50)                   null,
   Memo                 text                           null,
   IsPingCourse         int                            null,
   GStartTime           nvarchar(500)                  null,
   GType                int                            null,
   GGroupNumber         int                            null,
   GGroupPersonCount    int                            null,
   GGroupRule           nvarchar(100)                  null
);
GO

/****** 新进员工课程试卷表 ******/
create table New_CoursePaper 
(
   Id                   int                            null,
   CourseId             int                            null,
   PaperId              int                            null,
   Length               int                            null,
   Hour                 int                            null,
   TotalScore           int                            null,
   LevelScore           int                            null,
   TestTimes            int                            null
);

GO

/****** 新员工课程教室分配规则 ******/
create table New_CourseRoomRule 
(
   Id                   int                            null,
   RoomId               int                            null,
   ClassIDs             nvarchar(50)                   null,
   Rules                nvarchar(10)                   null,
   SeatDetail           text                           null
);
GO


/****** 新进员工课程附件表 ******/
create table New_CourseFiles 
(
   Id                   int                            null,
   CourseId             int                            null,
   Name                 nvarchar(50)                   null,
   RealName             nvarchar(100)                  null,
   CreateDate           datetime                       null,
   LoadTimes            int                            null,
   Type                 int                            null,
   PackId               char(10)                       null,
   ResourceSize         char(10)                       null,
   IsDelete             int                            null
);
GO

/****** 分组带教规则 ******/
create table New_GroupTeachRoomRule 
(
   Id                   int                            null,
   RoomId               int                            null,
   Teachers             nvarchar(50)                   null,
   Rules                nvarchar(10)                   null,
   SeatDetail           text                           null
);
GO

/****** 教室附件表 ******/
create table Sys_ClassRoomResource 
(
   Id                   int                            null,
   Name                 nvarchar(50)                   null,
   RealName             nvarchar(100)                  null,
   IsDelete             int                            null,
   CreateDate           datetime                       null
);
GO


/****** 对象:  StoredProcedure [dbo].[proc_DeleteByNewClassId]    脚本日期: 07/03/2013 08:56:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--删除班级连带删除组和人数据 Ids是班级ID集合{1,2,3...}
CREATE proc [dbo].[proc_DeleteByNewClassId]
(
@Ids nvarchar(max)
)
as
begin tran
exec('
--删除人员
delete from  New_GroupUser  where ClassId in('+@Ids+'
	)
--删除组
delete from  New_Group  where ClassId in ('+@Ids+'
	)
--删除班级
delete from New_Class  where  Id in('+@Ids+'
		)
')
	if @@error<>0
	begin rollback tran
	return 0
	end
	else
	begin commit tran
	return 1
end