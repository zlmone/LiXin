/****** Object:  Table [dbo].[Sys_LeaderConfig]    Script Date: 05/20/2013 08:50:05 ******/
/*
*
*������Sys_LeaderConfig ���ϼ�ָ����
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
*������Sys_LeaderConfig ���ϼ��¼�ָ��������
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
*��Co_Course �����ֶΣ�1��AttFlag �����ڷ�ʽ��1���Ͽο��ڣ�2���¿ο��ڣ�3���ϡ��¿ζ����ڣ�
*
*/
alter   table   Co_Course   add   AttFlag   int default(1);
alter   table   Co_Course   add   AttStatus   int default(0);
update Co_Course set AttFlag=1,AttStatus=0 where AttFlag is NULL;

/*
*
*����Ȩ�ޣ�1���ϼ�ָ������2���ϼ�ָ���༭
*
*/
set identity_insert sys_right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (168, '�ϼ�ָ������', 0, '�ϼ�ָ������', 'SystemManage/LeaderUserIndex', 86, 2, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (169, '�ϼ�ָ���༭', 1, '�ϼ�ָ���༭', 'SystemManage/LeaderUserEdit', 168, NULL, 'TrainManage')
GO

set identity_insert sys_right off


/*
*
*�������ã�ȫ�����߲�����ɴ���
*
*/
set identity_insert sys_paramconfig on

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime) VALUES (32, 'ȫ�����߲�����ɴ���', 33, '10', '1905-06-22')
GO

set identity_insert sys_paramconfig off