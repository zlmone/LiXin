/****** Object:  Table [dbo].[Sys_LeaderConfig]    Script Date: 05/20/2013 08:50:05 ******/
/*
*
*新增表Sys_LeaderConfig ：上级指定表
*
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sys_LeaderConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](200) NULL,
	[UserId] [int] NULL,
	[CreateTime] [datetime] NULL,
	[LastUpdateTime] [datetime] NULL,
	[Memo] [nvarchar](1000) NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_SYS_LEADERCONFIG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GO

/****** Object:  Table [dbo].[Sys_UserLinkLeader]    Script Date: 05/20/2013 08:50:31 ******/
/*
*
*新增表Sys_LeaderConfig ：上级下级指定关联表
*
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sys_UserLinkLeader](
	[GroupId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_SYS_USERLINKLEADER] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
*
*表Co_Course 新增字段：1、AttFlag ：考勤方式（1：上课考勤；2：下课考勤；3：上、下课都考勤）
*
*/
alter   table   Co_Course   add   AttFlag   int default(1);
alter   table   Co_Course   add   AttStatus   int default(0);
update Co_Course set AttFlag=1,AttStatus=0 where AttFlag is NULL;

/*
*
*新增权限：1、上级指定管理；2、上级指定编辑
*
*/
set identity_insert sys_right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (168, '上级指定管理', 0, '上级指定管理', 'SystemManage/LeaderUserIndex', 86, 2, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (169, '上级指定编辑', 1, '上级指定编辑', 'SystemManage/LeaderUserEdit', 168, NULL, 'TrainManage')
GO

set identity_insert sys_right off


/*
*
*新增配置：全年在线测试完成次数
*
*/
set identity_insert sys_paramconfig on

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime) VALUES (32, '全年在线测试完成次数', 33, '10', '1905-06-22')
GO

set identity_insert sys_paramconfig off