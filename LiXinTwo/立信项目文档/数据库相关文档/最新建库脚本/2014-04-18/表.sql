
/****** Object:  Table [dbo].[TrainGrade]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainGrade](
	[Grade] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tr_YearPlanCourse]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tr_YearPlanCourse](
	[YearId] [int] NULL,
	[CourseId] [int] NULL,
	[IsSystem] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tr_YearPlan]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tr_YearPlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LastUpdateTime] [datetime] NULL,
	[Year] [int] NULL,
	[UserID] [int] NULL,
	[PublishFlag] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_YEARPLAN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tr_MonthPlanCourse]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tr_MonthPlanCourse](
	[MonthId] [int] NULL,
	[CourseId] [int] NULL,
	[IsSystem] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tr_MonthPlan]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tr_MonthPlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NULL,
	[Month] [nvarchar](50) NULL,
	[LastUpdateTime] [datetime] NULL,
	[UserId] [int] NULL,
	[PublishFlag] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_MonthPlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_UserRole]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_UserRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_SYS_USERROLE] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_UserLinkLeader]    Script Date: 04/18/2014 08:59:00 ******/
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
/****** Object:  Table [dbo].[Sys_UserFingerInfor]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_UserFingerInfor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[UserHrId] [nvarchar](50) NULL,
	[Ssn] [nvarchar](100) NULL,
	[Name] [nvarchar](50) NULL,
	[FingerTemplate] [image] NULL,
	[FingerTemplate1] [nvarchar](3000) NULL,
	[FingerTemplate2] [nvarchar](3000) NULL,
 CONSTRAINT [PK_SYS_USERFINGERINFOR] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_User]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserOldId] [nvarchar](50) NULL,
	[JobNum] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Realname] [nvarchar](50) NOT NULL,
	[Ename] [nvarchar](50) NULL,
	[Sex] [int] NOT NULL,
	[Email] [nvarchar](200) NULL,
	[DeptId] [int] NOT NULL,
	[DeptCode] [nvarchar](200) NULL,
	[DeptName] [nvarchar](50) NULL,
	[PostId] [int] NOT NULL,
	[PostCode] [nvarchar](200) NULL,
	[PostName] [nvarchar](50) NULL,
	[PostLevel] [nvarchar](50) NULL,
	[PostGrade] [nvarchar](50) NULL,
	[FreezeTime] [datetime] NULL,
	[JoinDate] [datetime] NULL,
	[PasswordFailureTime] [datetime] NULL,
	[PasswordFailureCount] [int] NOT NULL,
	[PhotoUrl] [nvarchar](250) NULL,
	[Telephone] [nvarchar](50) NULL,
	[MobileNum] [nvarchar](50) NULL,
	[JobTitle] [nvarchar](200) NULL,
	[IdCardNo] [nvarchar](500) NULL,
	[LastLoginTime] [datetime] NULL,
	[UserType] [nvarchar](50) NULL,
	[PayGrade] [nvarchar](50) NULL,
	[GroupMobileNum] [nvarchar](50) NULL,
	[ProbationPayGrade] [nvarchar](50) NULL,
	[ProbationStart] [datetime] NULL,
	[ProbationEnd] [datetime] NULL,
	[TrainGrade] [nvarchar](50) NULL,
	[Major] [nvarchar](50) NULL,
	[GraduateSchool] [nvarchar](50) NULL,
	[VDeptId] [nvarchar](50) NULL,
	[SalaryFulfilsSystem] [nvarchar](50) NULL,
	[ApprovalRole] [nvarchar](50) NULL,
	[CPA] [nvarchar](50) NULL,
	[CPANo] [nvarchar](500) NULL,
	[CTA] [nvarchar](50) NULL,
	[CPV] [nvarchar](50) NULL,
	[ICLV] [nvarchar](50) NULL,
	[RealEstateValuers] [nvarchar](50) NULL,
	[OtherProfessional] [nvarchar](50) NULL,
	[ForeignOtherQualification] [nvarchar](50) NULL,
	[OtherProfessionsJobTitle] [nvarchar](50) NULL,
	[SectionLeader] [nvarchar](50) NULL,
	[LeaderID] [nvarchar](50) NULL,
	[CPAPractice] [nvarchar](50) NULL,
	[CPAStatus] [nvarchar](50) NULL,
	[CPARelationship] [nvarchar](50) NULL,
	[SubordinateDept] [nvarchar](50) NULL,
	[SubordinateSubstation] [nvarchar](50) NULL,
	[SalarySeries] [nvarchar](50) NULL,
	[IsTeacher] [int] NOT NULL,
	[Memo] [nvarchar](2000) NULL,
	[Status] [int] NULL,
	[IsDelete] [int] NOT NULL,
	[StaffType] [nvarchar](50) NULL,
	[LoginId] [nvarchar](50) NULL,
	[DeptPath] [nvarchar](500) NULL,
	[TrainMaster] [int] NULL,
	[IsMain] [int] NULL,
	[IsNew] [int] NULL,
	[IsInternExp] [int] NULL,
	[IsOurIntern] [int] NULL,
	[InternDept] [nvarchar](200) NULL,
	[NumberID] [nvarchar](50) NULL,
	[IsChangeNumberId] [int] NULL,
	[OldTrainGrade] [nvarchar](50) NULL,
	[ManageDeparts] [nvarchar](200) NULL,
 CONSTRAINT [PK_SYS_USER] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职工id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职工原ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'UserOldId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人员编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'JobNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Username'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'员工自助密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Realname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'英文名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Ename'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'DeptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'DeptCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'DeptName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'岗位ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PostId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'岗位代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PostCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'岗位名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PostName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'岗位级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PostLevel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'岗位等级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PostGrade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'冻结时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'FreezeTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'进立信日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'JoinDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码错误时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PasswordFailureTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码错误次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PasswordFailureCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'个人照片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PhotoUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Telephone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'MobileNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'JobTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'IdCardNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次登录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'LastLoginTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人员类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'薪酬级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'PayGrade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'短号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'GroupMobileNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'试用期薪酬级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'ProbationPayGrade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'试用期开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'ProbationStart'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'试用期结束时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'ProbationEnd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'培训级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'TrainGrade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所学专业' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Major'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'毕业院校' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'GraduateSchool'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属虚拟部门' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'VDeptId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'薪酬体系' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'SalaryFulfilsSystem'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'审批角色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'ApprovalRole'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册会计师' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'CPA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册会计师证书编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'CPANo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册税务师' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'CTA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册资产评估师' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'CPV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'土地估价师' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'ICLV'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'房地产估价师' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'RealEstateValuers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'其他专业技术资格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'OtherProfessional'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'国外其他执业资格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'ForeignOtherQualification'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'其他专业技术职称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'OtherProfessionsJobTitle'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门负责人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'SectionLeader'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直接上级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'LeaderID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPA执业年限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'CPAPractice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPA状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'CPAStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册会计师关系所在地' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'CPARelationship'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属事业部' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'SubordinateDept'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属分所' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'SubordinateSubstation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'薪酬级数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'SalarySeries'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Memo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'员工类别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'StaffType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否培训负责人（0：否；1：是）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_User', @level2type=N'COLUMN',@level2name=N'TrainMaster'
GO
/****** Object:  Table [dbo].[Sys_TrainGrade]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_TrainGrade](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[CreateUserID] [int] NULL,
	[OldGrade] [nvarchar](50) NULL,
	[NewGrade] [nvarchar](50) NULL,
	[Reason] [nvarchar](1000) NULL,
	[UpdateTime] [datetime] NULL,
	[Status] [int] NULL,
	[SubmitFlag] [int] NULL,
 CONSTRAINT [PK_Sys_TrainGrade] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_SortLinkGrade]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_SortLinkGrade](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SortID] [int] NULL,
	[GradeID] [int] NULL,
	[CourseID] [int] NULL,
 CONSTRAINT [PK_SYS_SORTLINKGRADE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_SortGradeCourse]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_SortGradeCourse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[OpenLeavel] [nvarchar](100) NULL,
	[Type] [int] NULL,
	[IsMust] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_SYS_SORTGRADECOURSE1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_Roles]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](200) NOT NULL,
	[RoleDesc] [nvarchar](1000) NOT NULL,
	[IsDefault] [int] NOT NULL,
	[Creater] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[IsDelete] [int] NOT NULL,
 CONSTRAINT [PK_SYS_ROLES] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_RoleRight]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_RoleRight](
	[RoleId] [int] NOT NULL,
	[RightId] [int] NOT NULL,
 CONSTRAINT [PK_SYS_ROLERIGHT] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[RightId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_Right]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Right](
	[RightId] [int] IDENTITY(1,1) NOT NULL,
	[RightName] [nvarchar](200) NOT NULL,
	[RightType] [int] NOT NULL,
	[RightDesc] [nvarchar](1000) NOT NULL,
	[Path] [nvarchar](250) NOT NULL,
	[ParentId] [int] NOT NULL,
	[ShowOrder] [int] NULL,
	[ModuleName] [nvarchar](50) NULL,
 CONSTRAINT [PK_SYS_RIGHT] PRIMARY KEY CLUSTERED 
(
	[RightId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_Post]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Post](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[PostCode] [nvarchar](50) NOT NULL,
	[PostName] [nvarchar](50) NOT NULL,
	[Remark] [nvarchar](2000) NOT NULL,
	[PostLevel] [int] NULL,
	[IsDelete] [int] NOT NULL,
 CONSTRAINT [PK_POST] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_PayGrade]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_PayGrade](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GradeName] [nvarchar](100) NULL,
 CONSTRAINT [PK_SYS_PAYGRADE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_ParamConfig]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_ParamConfig](
	[ConfigId] [int] IDENTITY(1,1) NOT NULL,
	[ConfigName] [nvarchar](200) NOT NULL,
	[ConfigType] [int] NOT NULL,
	[ConfigValue] [nvarchar](max) NULL,
	[LastUpdateTime] [datetime] NULL,
	[userType] [int] NULL,
 CONSTRAINT [PK_SYS_PARAMCONFIG] PRIMARY KEY CLUSTERED 
(
	[ConfigId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_NoteSort]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_NoteSort](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SortName] [nvarchar](50) NOT NULL,
	[ParentID] [int] NOT NULL,
	[IsDelete] [int] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_SYS_NOTESORT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_Notes]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Notes](
	[NoteId] [int] IDENTITY(1,1) NOT NULL,
	[SortID] [int] NULL,
	[Type] [int] NOT NULL,
	[NoteTitle] [nvarchar](200) NOT NULL,
	[NoteContent] [text] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[PublishFlag] [int] NOT NULL,
	[PublishTime] [datetime] NULL,
	[IsTop] [int] NOT NULL,
	[IsDelete] [int] NOT NULL,
	[Num] [int] NULL,
	[AdFlag] [int] NULL,
	[DepTrainFlag] [int] NULL,
	[OpenGroupFlag] [int] NULL,
	[OpenGroup] [nvarchar](max) NULL,
	[OpenDepartFlag] [int] NULL,
	[OpenDepart] [nvarchar](max) NULL,
	[ContentDesc] [text] NULL,
	[TopTime] [datetime] NULL,
	[TrainGrade] [nvarchar](50) NULL,
	[isTopShow] [int] NULL,
	[BackUrlName] [nvarchar](100) NULL,
 CONSTRAINT [PK_SYS_NOTES] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_NoteResource]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_NoteResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoteId] [int] NULL,
	[RealName] [nvarchar](100) NULL,
	[FileName] [nvarchar](100) NULL,
	[CreateDate] [datetime] NULL,
	[IsDelete] [int] NULL,
	[type] [int] NULL,
 CONSTRAINT [PK_SYS_NOTERESOURCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sys_NoteBackImage]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sys_NoteBackImage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RealName] [nvarchar](100) NULL,
	[FileName] [nvarchar](100) NULL,
	[defalutType] [int] NULL,
	[CreateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_MyNoteLook]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_MyNoteLook](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NULL,
	[UserID] [int] NULL,
	[LookTime] [datetime] NULL,
	[IsDelete] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_MyNoteDowmLoad]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_MyNoteDowmLoad](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ResourceID] [int] NULL,
	[NoteID] [int] NULL,
	[UserID] [int] NULL,
	[DownLoadTime] [datetime] NULL,
	[IsDelete] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_Log]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Log](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogType] [int] NOT NULL,
	[LogTitle] [nvarchar](100) NOT NULL,
	[LogContent] [nvarchar](1000) NOT NULL,
	[LogTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SYS_LOG] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_LeaderConfig]    Script Date: 04/18/2014 08:59:00 ******/
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
/****** Object:  Table [dbo].[Sys_GroupUser]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_GroupUser](
	[GroupId] [int] NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_Group]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Group](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](200) NOT NULL,
	[UserId] [int] NULL,
	[CreateTime] [datetime] NULL,
	[LastUpdateTime] [datetime] NULL,
	[Memo] [nvarchar](1000) NULL,
	[IsDelete] [int] NOT NULL,
 CONSTRAINT [PK_SYS_USERGROUP] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_Department]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_Department](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[DeptCode] [nvarchar](50) NOT NULL,
	[DeptName] [nvarchar](50) NOT NULL,
	[ParentDeptId] [int] NULL,
	[TypeNum] [int] NULL,
	[Remark] [nvarchar](2000) NULL,
	[DeptGrade] [int] NULL,
	[IsDelete] [int] NOT NULL,
	[OldDeptId] [int] NULL,
 CONSTRAINT [PK_SYS_DEPARTMENT] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门级数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sys_Department', @level2type=N'COLUMN',@level2name=N'DeptGrade'
GO
/****** Object:  Table [dbo].[Sys_ClassRoomResource]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_ClassRoomResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassRoomID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[RealName] [nvarchar](100) NULL,
	[IsDelete] [int] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_Sys_ClassRoomResource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_ClassRoom]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_ClassRoom](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoomName] [nvarchar](50) NULL,
	[Memo] [nvarchar](200) NULL,
	[RoomNumber] [int] NULL,
	[ColumnNumber] [int] NULL,
	[RowNumber] [int] NULL,
	[UserID] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_CLASSROOM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey_SurveyUser]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey_SurveyUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SurveyID] [int] NULL,
	[UserID] [int] NULL,
	[Score] [decimal](18, 1) NULL,
	[LastUpdateTime] [datetime] NULL,
	[IsAccessed] [int] NULL,
 CONSTRAINT [PK_SURVEY_SURVEYUSER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey_Survey]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Survey_Survey](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[PaperID] [int] NULL,
	[Memo] [nvarchar](1000) NULL,
	[OpenGroupFlag] [int] NULL,
	[OpenGroup] [varchar](500) NULL,
	[OpenDepartFlag] [int] NULL,
	[OpenDepart] [varchar](500) NULL,
	[PublishFlag] [int] NULL,
	[PublishTime] [datetime] NULL,
	[LastUpdateTime] [datetime] NULL,
	[UserID] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_SURVEY_SURVEY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Survey_ReplyAnswer]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey_ReplyAnswer](
	[AnswerId] [int] IDENTITY(1,1) NOT NULL,
	[SourceFrom] [int] NULL,
	[ObjectID] [int] NULL,
	[ExampaperID] [int] NULL,
	[QuestionID] [int] NULL,
	[SubjectiveAnswer] [nvarchar](500) NULL,
	[ObjectiveAnswer] [nvarchar](200) NULL,
	[UserID] [int] NULL,
	[Status] [int] NULL,
	[QuestionReply] [nvarchar](1000) NULL,
	[DeptID] [int] NULL,
	[IsDeptAccessed] [int] NULL,
	[IsOfficeAccessed] [int] NULL,
	[IsMaster] [int] NULL,
	[TeacherID] [int] NULL,
	[CourseRoomRuleId] [int] NULL,
	[SubmitTime] [datetime] NULL,
 CONSTRAINT [PK_SURVEY_REPLYANSWER] PRIMARY KEY CLUSTERED 
(
	[AnswerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey_QuestionAnswer]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey_QuestionAnswer](
	[AnswerId] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NULL,
	[AnswerContent] [nvarchar](500) NULL,
	[Score] [int] NULL,
	[Status] [int] NULL,
	[ShowOrder] [nvarchar](10) NULL,
 CONSTRAINT [PK_SURVEY_QUESTIONANSWER] PRIMARY KEY CLUSTERED 
(
	[AnswerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey_Question]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey_Question](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[ExampaperID] [int] NULL,
	[QuestionType] [int] NULL,
	[QuestionContent] [nvarchar](1000) NULL,
	[QuestionOrder] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[UserID] [int] NULL,
	[Status] [int] NULL,
	[LinkSortPayGrade] [text] NULL,
	[ObjectType] [int] NULL,
 CONSTRAINT [PK_SURVEY_QUESTION] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey_ExampaperCategory]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey_ExampaperCategory](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NULL,
	[ParentId] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_SURVEY_EXAMPAPERCATEGORY] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey_Exampaper]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey_Exampaper](
	[ExampaperID] [int] IDENTITY(1,1) NOT NULL,
	[SortID] [int] NULL,
	[ExamTitle] [nvarchar](50) NULL,
	[ExamType] [int] NULL,
	[UserID] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[ExamDescription] [nvarchar](200) NULL,
	[IsDelete] [int] NULL,
	[ExampaperUsage] [int] NULL,
 CONSTRAINT [PK_SURVEY_EXAMPAPER] PRIMARY KEY CLUSTERED 
(
	[ExampaperID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Survey_DeptSurvey]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey_DeptSurvey](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeptID] [int] NULL,
	[SurveyID] [int] NULL,
	[Status] [int] NULL,
	[SubmitTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Re_ResourceType]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Re_ResourceType](
	[ResourceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
	[TypeDec] [nvarchar](300) NULL,
	[ParentID] [int] NULL,
	[IsDelete] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Re_Resource]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Re_Resource](
	[ResourceID] [int] IDENTITY(1,1) NOT NULL,
	[ResourceName] [nvarchar](50) NOT NULL,
	[ResourceDesc] [nvarchar](500) NOT NULL,
	[ResourceTypeID] [int] NOT NULL,
	[Format] [int] NOT NULL,
	[Suffix] [nvarchar](10) NOT NULL,
	[Tag] [varchar](max) NOT NULL,
	[URL] [nvarchar](200) NOT NULL,
	[ThumbnailURL] [nvarchar](200) NULL,
	[ResultURL] [nvarchar](200) NULL,
	[isOpen] [int] NOT NULL,
	[isDownload] [int] NOT NULL,
	[UpTime] [datetime] NOT NULL,
	[UpUserID] [int] NOT NULL,
	[LastTime] [datetime] NOT NULL,
	[OpenNum] [int] NOT NULL,
	[DownNum] [int] NOT NULL,
	[CollectionNum] [int] NOT NULL,
	[Score] [float] NOT NULL,
	[ScoreNum] [int] NOT NULL,
	[Satisfaction] [float] NOT NULL,
	[SatisfactionNum] [int] NOT NULL,
	[Practical] [float] NOT NULL,
	[PracticalNum] [int] NOT NULL,
	[Attention] [float] NOT NULL,
	[AttentionNum] [int] NOT NULL,
	[IsRecommend] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[OpenFlag] [int] NOT NULL,
	[OpenGroupObject] [nvarchar](500) NOT NULL,
	[OpenDepartObject] [nvarchar](500) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Re_MyResource]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Re_MyResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ResourceId] [int] NOT NULL,
	[OpenTime] [datetime] NOT NULL,
	[Score] [int] NOT NULL,
	[Satisfaction] [int] NOT NULL,
	[Practical] [int] NOT NULL,
	[Attention] [int] NOT NULL,
	[IsCollection] [int] NOT NULL,
	[CollectionTime] [datetime] NULL,
	[IsDownload] [int] NOT NULL,
	[DownloadTime] [datetime] NULL,
	[Status] [int] NOT NULL,
	[FavoriteId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_UserExamScore]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_UserExamScore](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Score] [decimal](18, 2) NULL,
	[SumScore] [decimal](18, 2) NULL,
 CONSTRAINT [PK_New_UserExamScore] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_MidAttendce]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[New_MidAttendce](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[SubCourseId] [char](10) NULL,
	[UserId] [int] NULL,
	[CreateTime] [datetime] NULL,
	[Time] [int] NULL,
 CONSTRAINT [PK_NEW_MIDATTENDCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[New_LearnVideoInfor]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_LearnVideoInfor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LearnId] [int] NULL,
	[Progress] [decimal](18, 1) NULL,
	[ResourseId] [int] NULL,
	[AttendId] [int] NULL,
	[LearnTimes] [int] NULL,
 CONSTRAINT [PK_New_LearnVideoInfor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_GroupUser]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_GroupUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassId] [int] NULL,
	[GroupId] [int] NULL,
	[UserId] [int] NULL,
	[NumberID] [nvarchar](50) NULL,
 CONSTRAINT [PK_New_GroupUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_Group]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_Group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassId] [int] NULL,
	[GroupName] [nvarchar](50) NULL,
	[PersonCount] [int] NULL,
	[GroupNo] [nvarchar](10) NULL,
	[GroupIndex] [int] NULL,
 CONSTRAINT [PK_New_Group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CpaLearnStatus]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_CpaLearnStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseID] [int] NULL,
	[UserID] [int] NULL,
	[IsAttFlag] [int] NULL,
	[IsPass] [int] NULL,
	[Progress] [int] NULL,
	[LearnTimes] [int] NULL,
	[GetLength] [decimal](18, 2) NULL,
	[LastUpdateTime] [datetime] NULL,
	[CpaFlag] [int] NULL,
	[GradeStatus] [int] NULL,
 CONSTRAINT [PK_New_CpaLearnStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CourseRoomRule]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_CourseRoomRule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[RoomId] [int] NULL,
	[ClassIDs] [nvarchar](50) NULL,
	[TeacherId] [int] NULL,
	[PingId] [int] NULL,
	[Rules] [nvarchar](10) NULL,
	[SeatDetail] [text] NULL,
	[Type] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[PersonCount] [int] NULL,
	[GroupName] [nvarchar](50) NULL,
	[IsAttFlag] [int] NULL,
	[AfterEvlutionConfigTime] [decimal](18, 2) NULL,
 CONSTRAINT [PK_New_CourseRoomRule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CoursePaper]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_CoursePaper](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[PaperId] [int] NULL,
	[Length] [int] NULL,
	[Hour] [int] NULL,
	[TotalScore] [int] NULL,
	[LevelScore] [int] NULL,
	[TestTimes] [int] NULL,
 CONSTRAINT [PK_New_CoursePaper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CourseOrderDetail]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_CourseOrderDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[SubCourseID] [int] NULL,
	[LearnTime] [datetime] NULL,
	[LearnStatus] [int] NULL,
	[Type] [int] NULL,
	[IsLeave] [int] NULL,
	[ApprovalName] [nvarchar](50) NULL,
	[LeaveReason] [nvarchar](200) NULL,
 CONSTRAINT [PK_New_CourseOrderDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CourseOrder]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_CourseOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[ClassId] [int] NULL,
	[UserId] [int] NULL,
	[OrderTime] [datetime] NULL,
	[LearnStatus] [int] NULL,
	[TogetherScore] [int] NULL,
	[GroupScore] [int] NULL,
	[CourseExamScore] [decimal](18, 2) NULL,
	[ExamScore] [decimal](18, 2) NULL,
	[TogetherMemo] [nvarchar](200) NULL,
	[GroupMemo] [nvarchar](200) NULL,
	[Type] [int] NULL,
	[CourseExamSumScore] [decimal](18, 2) NULL,
	[ExamSumScore] [decimal](18, 2) NULL,
 CONSTRAINT [PK_New_CourseOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CourseFiles]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_CourseFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[Name] [nvarchar](500) NULL,
	[RealName] [nvarchar](500) NULL,
	[CreateDate] [datetime] NULL,
	[LoadTimes] [int] NULL,
	[Type] [int] NULL,
	[PackId] [int] NULL,
	[ResourceSize] [int] NULL,
	[IsDelete] [int] NULL,
	[Flag] [int] NULL,
 CONSTRAINT [PK_New_CourseFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CourseAdvice]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_CourseAdvice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[SubCourseID] [int] NULL,
	[AdviceContent] [nvarchar](1000) NULL,
	[AdviceTime] [datetime] NULL,
	[AnserUserId] [int] NULL,
	[AnserContent] [nvarchar](1000) NULL,
	[AnserTime] [datetime] NULL,
	[VisibleFlag] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_New_CourseAdvice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_Course]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseName] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[IsGroupTeach] [int] NULL,
	[Teachers] [nvarchar](200) NULL,
	[Classes] [nvarchar](200) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[IsPingTeacher] [nvarchar](200) NULL,
	[ScoreDistribute] [nvarchar](50) NULL,
	[Memo] [text] NULL,
	[IsPingCourse] [int] NULL,
	[GStartTime] [nvarchar](500) NULL,
	[GType] [int] NULL,
	[GGroupNumber] [int] NULL,
	[GGroupPersonCount] [int] NULL,
	[GGroupRule] [nvarchar](100) NULL,
	[IsDelete] [int] NULL,
	[Way] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[PublicFlag] [int] NULL,
	[IsTest] [int] NULL,
	[VideoLowLength] [int] NULL,
 CONSTRAINT [PK_New_Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_ClassRoom]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_ClassRoom](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoomName] [nvarchar](50) NULL,
	[Memo] [nvarchar](200) NULL,
	[RoomNumber] [int] NULL,
	[ColumnNumber] [int] NULL,
	[RowNumber] [int] NULL,
	[UserID] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[Address] [nvarchar](100) NULL,
	[IsDelete] [int] NULL,
	[DisSeat] [text] NULL,
	[Classes] [nvarchar](100) NULL,
	[PrePersonCount] [int] NULL,
	[SeatType] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_Class]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_Class](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](50) NULL,
	[PersonCount] [int] NULL,
	[Year] [int] NULL,
	[ClassNo] [nvarchar](10) NULL,
	[ClassIndex] [int] NULL,
	[IsDoDelete] [int] NULL,
 CONSTRAINT [PK_New_Class] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_Attendce]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[New_Attendce](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[SubCourseID] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[AttStatus] [int] NULL,
 CONSTRAINT [PK_NEW_ATTENDCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_UserOtherApply]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_UserOtherApply](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ApplyID] [int] NULL,
	[ApplyName] [nvarchar](200) NULL,
	[Year] [int] NULL,
	[ConvertScore] [decimal](18, 1) NULL,
	[Memo] [nvarchar](200) NULL,
	[ApplyTime] [datetime] NULL,
	[ApplyUserID] [int] NULL,
	[ApplyType] [int] NULL,
	[Status] [int] NULL,
	[ApproveStatus] [int] NULL,
	[DepApproveStatus] [int] NULL,
	[IsDelete] [int] NULL,
	[tScore] [decimal](18, 1) NULL,
	[CPAScore] [decimal](18, 1) NULL,
	[GettScore] [decimal](18, 1) NULL,
	[GetCPAScore] [decimal](18, 1) NULL,
	[DepTrainReason] [nvarchar](500) NULL,
	[DepReason] [nvarchar](500) NULL,
 CONSTRAINT [PK__Free_Use__3214EC276F1576F7] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_UserApplyTeacherDetails]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_UserApplyTeacherDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[userApplyID] [int] NULL,
	[ClassName] [nvarchar](200) NULL,
	[ConvertScore] [decimal](18, 1) NULL,
	[IsDelete] [int] NULL,
	[tScore] [decimal](18, 1) NULL,
	[CPAScore] [decimal](18, 1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_UserApplyOtherForm]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_UserApplyOtherForm](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OtherFromID] [int] NULL,
	[CourseName] [nvarchar](200) NULL,
	[Address] [nvarchar](200) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[TogetherScore] [int] NULL,
	[CPAScore] [int] NULL,
	[ApplyDateTime] [datetime] NULL,
	[ApplyUserID] [int] NULL,
	[Status] [int] NULL,
	[ApproveStatus] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_UserApplyFiles]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_UserApplyFiles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[userApplyID] [int] NULL,
	[ResourceName] [nvarchar](500) NULL,
	[RealName] [nvarchar](500) NULL,
	[IsDelete] [int] NULL,
	[ConvertName] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_OtherFroms]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_OtherFroms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[FromName] [nvarchar](500) NOT NULL,
	[FromType] [int] NOT NULL,
	[FromTime] [datetime] NOT NULL,
	[IsDelete] [int] NOT NULL,
 CONSTRAINT [PK_Free_OtherFroms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_OtherApplyConfig]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_OtherApplyConfig](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ApplyContent] [nvarchar](200) NULL,
	[ConvertType] [int] NULL,
	[ConvertBase] [int] NULL,
	[ConvertBaseInfo] [nvarchar](50) NULL,
	[ConvertBaseTo] [int] NULL,
	[UploadTip] [nvarchar](200) NULL,
	[ConvertMax] [int] NULL,
	[TrainGradeScore] [nvarchar](500) NULL,
	[Memo] [nvarchar](200) NULL,
	[year] [int] NULL,
	[CreateTime] [datetime] NULL,
	[IsDelete] [int] NULL,
	[ApplyType] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_DepApprove]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_DepApprove](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[CreateUserID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[ManageDeptIDs] [nvarchar](500) NULL,
	[LeardID] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_Apporve]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_Apporve](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[userApplyID] [int] NULL,
	[DepTrainApprove] [int] NULL,
	[DepApprove] [int] NULL,
	[DepTrainReason] [nvarchar](500) NULL,
	[DepReason] [nvarchar](500) NULL,
	[CheackBatch] [int] NULL,
	[type] [int] NULL,
	[ApproveUserID] [int] NULL,
	[ApproveTime] [datetime] NULL,
	[ApproveType] [int] NULL,
	[LookFile] [int] NULL,
	[DepLookFile] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Free_ApplyConfig]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Free_ApplyConfig](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FreeName] [nvarchar](200) NULL,
	[UploadTip] [nvarchar](200) NULL,
	[ApplyType] [int] NULL,
	[CPAFreeScore] [int] NULL,
	[TogetherFreeScore] [int] NULL,
	[Memo] [nvarchar](200) NULL,
	[year] [int] NULL,
	[CreateTime] [datetime] NULL,
	[IsDelete] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepTran_DepartUser]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepTran_DepartUser](
	[DepartSetId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_DEPTRAN_DEPARTUSER] PRIMARY KEY CLUSTERED 
(
	[DepartSetId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepTran_DepartLeaderConfig]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepTran_DepartLeaderConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepartSetName] [nvarchar](50) NULL,
	[MainUserIDs] [nvarchar](500) NULL,
	[CreateTime] [datetime] NULL,
	[Memo] [nvarchar](200) NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_DEPTRAN_DEPARTLEADERCONFIG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepTran_CourseOrder]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepTran_CourseOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[OrderTime] [datetime] NULL,
	[OrderStatus] [int] NULL,
	[LearnStatus] [int] NULL,
	[GetScore] [decimal](18, 2) NULL,
	[IsAppoint] [int] NULL,
	[PassStatus] [int] NULL,
	[AttScore] [decimal](18, 2) NULL,
	[EvlutionScore] [decimal](18, 2) NULL,
	[ExamScore] [decimal](18, 2) NULL,
	[DepartSetId] [int] NULL,
	[OrderTimes] [int] NULL,
 CONSTRAINT [PK_DEPTRAN_COURSEORDER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepTran_CourseOpen]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepTran_CourseOpen](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NOT NULL,
	[DepartId] [int] NOT NULL,
	[AttFlag] [int] NULL,
	[OpenFlag] [int] NULL,
	[SubmitTime] [datetime] NULL,
	[ApprovalFlag] [int] NULL,
	[UserId] [int] NULL,
	[LimitNumber] [int] NULL,
 CONSTRAINT [PK_DepTran_CourseOpen_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepTran_CourseAttFile]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepTran_CourseAttFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[DepartId] [int] NULL,
	[Type] [int] NULL,
	[Name] [nvarchar](500) NULL,
	[RealName] [nvarchar](500) NULL,
	[CreateTime] [datetime] NULL,
	[LoadTimes] [int] NULL,
	[FileSize] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_DEPTRAN_COURSEATTFILE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepTran_AttendceApprovalRecord]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepTran_AttendceApprovalRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[DeptId] [int] NULL,
	[UserId] [int] NULL,
	[ApprovalTime] [datetime] NULL,
	[ApprovalFlag] [int] NULL,
	[Reason] [nvarchar](200) NULL,
	[SubmitUserId] [int] NULL,
	[SubmitTime] [datetime] NULL,
 CONSTRAINT [PK_DEPTRAN_ATTENDCEAPPROVALREC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepTran_Attendce]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepTran_Attendce](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Status] [int] NULL,
	[ApprovalFlag] [int] NULL,
	[Reason] [nvarchar](500) NULL,
	[DepartSetId] [int] NULL,
 CONSTRAINT [PK_DEPTRAN_ATTENDCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_YearPlanCourse]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_YearPlanCourse](
	[YearId] [int] NULL,
	[CourseId] [int] NULL,
	[IsSystem] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_YearPlan]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_YearPlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LastUpdateTime] [datetime] NULL,
	[Year] [int] NULL,
	[UserID] [int] NULL,
	[PublishFlag] [int] NULL,
	[IsDelete] [int] NULL,
	[IsOpen] [int] NULL,
	[DeptId] [int] NULL,
 CONSTRAINT [PK_DEP_YEARPLAN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_SurveyUser]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Survey_SurveyUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SurveyID] [int] NULL,
	[UserID] [int] NULL,
	[Score] [decimal](18, 1) NULL,
	[LastUpdateTime] [datetime] NULL,
	[IsAccessed] [int] NULL,
 CONSTRAINT [PK_Dep_Survey_SurveyUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_Survey]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Survey_Survey](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[PaperID] [int] NULL,
	[Memo] [nvarchar](1000) NULL,
	[PublishFlag] [int] NULL,
	[PublishTime] [datetime] NULL,
	[LastUpdateTime] [datetime] NULL,
	[UserID] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[IsDelete] [int] NULL,
	[OpenGroupFlag] [int] NULL,
	[OpenGroup] [text] NULL,
	[OpenDepartFlag] [int] NULL,
	[OpenDepart] [text] NULL,
	[DeptID] [int] NULL,
 CONSTRAINT [PK_Dep_Survey_Survey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_ReplyAnswer]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Survey_ReplyAnswer](
	[AnswerId] [int] IDENTITY(1,1) NOT NULL,
	[SourceFrom] [int] NULL,
	[ObjectID] [int] NULL,
	[ExampaperID] [int] NULL,
	[QuestionID] [int] NULL,
	[SubjectiveAnswer] [nvarchar](500) NULL,
	[ObjectiveAnswer] [nvarchar](200) NULL,
	[UserID] [int] NULL,
	[Status] [int] NULL,
	[QuestionReply] [nvarchar](1000) NULL,
	[DeptID] [int] NULL,
	[IsDeptAccessed] [int] NULL,
	[IsOfficeAccessed] [int] NULL,
	[IsMaster] [int] NULL,
 CONSTRAINT [PK_Dep_Survey_ReplyAnswer] PRIMARY KEY CLUSTERED 
(
	[AnswerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_QuestionAnswer]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Survey_QuestionAnswer](
	[AnswerId] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NULL,
	[AnswerContent] [nvarchar](500) NULL,
	[Score] [int] NULL,
	[Status] [int] NULL,
	[ShowOrder] [nvarchar](10) NULL,
 CONSTRAINT [PK_Dep_Survey_QuestionANSWER] PRIMARY KEY CLUSTERED 
(
	[AnswerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_Question]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Survey_Question](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[ExampaperID] [int] NULL,
	[QuestionType] [int] NULL,
	[QuestionContent] [nvarchar](1000) NULL,
	[QuestionOrder] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[UserID] [int] NULL,
	[Status] [int] NULL,
	[LinkSortPayGrade] [text] NULL,
 CONSTRAINT [PK_Dep_Survey_Question] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_ExampaperCategory]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Survey_ExampaperCategory](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NULL,
	[ParentId] [int] NULL,
	[IsDelete] [int] NULL,
	[DeptID] [int] NULL,
 CONSTRAINT [PK_Dep_Survey_ExampaperCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_Exampaper]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Survey_Exampaper](
	[ExampaperID] [int] IDENTITY(1,1) NOT NULL,
	[SortID] [int] NULL,
	[ExamTitle] [nvarchar](50) NULL,
	[ExamType] [int] NULL,
	[UserID] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[ExamDescription] [nvarchar](200) NULL,
	[IsDelete] [int] NULL,
	[ExampaperUsage] [int] NULL,
	[DeptID] [int] NULL,
 CONSTRAINT [PK_Dep_SURVEY_EXAMPAPER] PRIMARY KEY CLUSTERED 
(
	[ExampaperID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_DeptSurvey]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Survey_DeptSurvey](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeptID] [int] NULL,
	[SurveyID] [int] NULL,
	[Status] [int] NULL,
	[SubmitTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_ParamConfig]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_ParamConfig](
	[ConfigId] [int] IDENTITY(1,1) NOT NULL,
	[ConfigName] [nvarchar](200) NULL,
	[ConfigType] [int] NULL,
	[ConfigValue] [nvarchar](500) NULL,
	[LastUpdateTime] [datetime] NULL,
	[DeptId] [int] NULL,
 CONSTRAINT [PK_DEP_PARAMCONFIG] PRIMARY KEY CLUSTERED 
(
	[ConfigId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_NoteSort]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_NoteSort](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SortName] [nvarchar](50) NULL,
	[ParentID] [int] NULL,
	[Type] [int] NULL,
	[IsDelete] [int] NULL,
	[DeptId] [int] NULL,
 CONSTRAINT [PK_DEP_NOTESORT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Notes]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Notes](
	[NoteId] [int] IDENTITY(1,1) NOT NULL,
	[SortID] [int] NULL,
	[NoteTitle] [nvarchar](200) NULL,
	[NoteContent] [text] NULL,
	[UserId] [int] NULL,
	[CreateTime] [datetime] NULL,
	[PublishFlag] [int] NULL,
	[PublishTime] [datetime] NULL,
	[IsTop] [int] NULL,
	[Type] [int] NULL,
	[AdFlag] [int] NULL,
	[Num] [int] NULL,
	[IsDelete] [int] NULL,
	[DeptId] [int] NULL,
 CONSTRAINT [PK_DEP_NOTES] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_MonthPlanCourse]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_MonthPlanCourse](
	[MonthId] [int] NULL,
	[CourseId] [int] NULL,
	[IsSystem] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_MonthPlan]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_MonthPlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeptId] [int] NULL,
	[Year] [int] NULL,
	[Month] [nvarchar](50) NULL,
	[LastUpdateTime] [datetime] NULL,
	[UserId] [int] NULL,
	[PublishFlag] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_Dep_MonthPlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_LinkDepart]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_LinkDepart](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[YearId] [int] NULL,
	[DeptId] [int] NULL,
	[ApprovalFlag] [int] NULL,
	[ApprovalTime] [datetime] NULL,
	[Reason] [nvarchar](200) NULL,
 CONSTRAINT [PK_DEP_LINKDEPART] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_DepartOpenCourse]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_DepartOpenCourse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[DeCourseId] [int] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_DEP_DEPARTOPENCOURSE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_CpaLearnStatus]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_CpaLearnStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseID] [int] NULL,
	[UserID] [int] NULL,
	[GetLength] [decimal](18, 2) NULL,
	[LastUpdateTime] [datetime] NULL,
	[CpaFlag] [int] NULL,
	[DepartSetId] [int] NULL,
 CONSTRAINT [PK_Dep_CpaLearnStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_CourseResource]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_CourseResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[ResourceName] [nvarchar](50) NULL,
	[RealName] [nvarchar](50) NULL,
	[ResourceType] [int] NULL,
	[ResourceSize] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[PackId] [int] NULL,
	[Flag] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_DEP_COURSERESOURCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Coursepaper]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Coursepaper](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaperId] [int] NULL,
	[CourseId] [int] NULL,
	[Length] [int] NULL,
	[Hour] [int] NULL,
	[TotalScore] [int] NULL,
	[LevelScore] [int] NULL,
	[TestTimes] [int] NULL,
	[IsMain] [int] NULL,
 CONSTRAINT [PK_Dep_Coursepaper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_CourseOrder]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_CourseOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[OrderTime] [datetime] NULL,
	[OrderStatus] [int] NULL,
	[LearnStatus] [int] NULL,
	[GetScore] [int] NULL,
	[PassStatus] [nvarchar](500) NULL,
	[AttScore] [decimal](18, 2) NULL,
	[EvlutionScore] [decimal](18, 2) NULL,
	[ExamScore] [decimal](18, 2) NULL,
	[DepartSetId] [int] NULL,
	[IsLeave] [int] NULL,
	[Reason] [nvarchar](200) NULL,
	[LeaveTime] [datetime] NULL,
	[ApprovalFlag] [int] NULL,
	[ApprovalTime] [datetime] NULL,
	[ApprovalUserId] [int] NULL,
	[OrderTimes] [int] NULL,
	[AppReason] [nvarchar](200) NULL,
	[IsAppoint] [int] NULL,
	[DropReason] [nvarchar](200) NULL,
	[DropType] [int] NULL,
	[ApprovalUser] [nvarchar](50) NULL,
	[ApprovalLimitTime] [datetime] NULL,
	[requestid] [int] NULL,
	[FtriggerFlag] [int] NULL,
 CONSTRAINT [PK_DEP_COURSEORDER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_CourseDept]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_CourseDept](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NOT NULL,
	[DepartId] [int] NOT NULL,
	[AttFlag] [int] NULL,
	[SubmitTime] [datetime] NULL,
	[OpenFlag] [int] NULL,
	[ApprovalFlag] [int] NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_Dep_CourseDept] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_CourseAttFile]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_CourseAttFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[DepartId] [int] NULL,
	[Type] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[RealName] [nvarchar](100) NULL,
	[CreateTime] [datetime] NULL,
	[LoadTimes] [int] NULL,
	[FileSize] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_DEP_COURSEATTFILE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_CourseAdvice]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_CourseAdvice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[AdviceContent] [nvarchar](1000) NULL,
	[AdviceTime] [datetime] NULL,
	[AnserUserId] [int] NULL,
	[AnserContent] [nvarchar](1000) NULL,
	[AnserTime] [datetime] NULL,
	[VisibleFlag] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_Dep_CourseAdvice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Course]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseName] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[YearPlan] [int] NULL,
	[Year] [int] NULL,
	[Month] [nvarchar](50) NULL,
	[Day] [int] NULL,
	[NextStartTime] [datetime] NULL,
	[OpenLevel] [nvarchar](50) NULL,
	[IsMust] [int] NULL,
	[Way] [int] NULL,
	[Teacher] [nvarchar](50) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Sort] [nvarchar](50) NULL,
	[CourseLength] [decimal](18, 1) NULL,
	[RoomId] [int] NULL,
	[NumberLimited] [int] NULL,
	[IsCPA] [int] NULL,
	[IsOrder] [int] NULL,
	[IsLeave] [int] NULL,
	[IsRT] [int] NULL,
	[AttFlag] [int] NULL,
	[AttStatus] [int] NULL,
	[OpenFlag] [int] NULL,
	[OpenGroupObject] [nvarchar](500) NULL,
	[OpenDepartObject] [nvarchar](500) NULL,
	[OpenPerson] [nvarchar](500) NULL,
	[IsTest] [int] NULL,
	[IsPing] [int] NULL,
	[SurveyPaperId] [nvarchar](50) NULL,
	[Memo] [text] NULL,
	[CourseFrom] [int] NULL,
	[StopOrderFlag] [int] NULL,
	[StopDucueFlag] [int] NULL,
	[ReturnTimes] [int] NULL,
	[AfterOrderTimes] [int] NULL,
	[Publishflag] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[PreAdviceConfigTime] [decimal](18, 1) NULL,
	[AfterEvlutionConfigTime] [decimal](18, 1) NULL,
	[CpaTeacher] [nvarchar](50) NULL,
	[PreCourseTime] [datetime] NULL,
	[CourseTimes] [int] NULL,
	[TrainDays] [int] NULL,
	[CourseAddress] [nvarchar](50) NULL,
	[CourseObjectMemo] [nvarchar](500) NULL,
	[IsDelete] [int] NULL,
	[CourseLengthDistribute] [nvarchar](50) NULL,
	[IsSystem] [int] NULL,
	[IsYearPlan] [int] NULL,
	[IsOpenSub] [int] NULL,
	[courseProvide] [nvarchar](500) NULL,
	[StudentRequirement] [text] NULL,
	[remark] [text] NULL,
	[AdFlag] [int] NULL,
	[IsOpenOthers] [int] NULL,
	[DeptId] [int] NULL,
	[OpenUserId] [int] NULL,
	[OpenSubmitTime] [datetime] NULL,
	[ApprovalUserId] [int] NULL,
	[ApprovalTime] [datetime] NULL,
	[OpenCourseId] [int] NULL,
	[OpenReason] [nvarchar](200) NULL,
	[AttUserId] [nvarchar](200) NULL,
 CONSTRAINT [PK_DEP_COURSE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_ClassRoom]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_ClassRoom](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoomName] [nvarchar](50) NULL,
	[Memo] [nvarchar](200) NULL,
	[RoomNumber] [int] NULL,
	[ColumnNumber] [int] NULL,
	[RowNumber] [int] NULL,
	[UserID] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[Address] [nvarchar](100) NULL,
	[IsDelete] [int] NULL,
	[DeptId] [int] NULL,
 CONSTRAINT [PK_DEP_CLASSROOM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_AttendceApprovalRecord]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_AttendceApprovalRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[DeptId] [int] NULL,
	[UserId] [int] NULL,
	[ApprovalTime] [datetime] NULL,
	[ApprovalFlag] [int] NULL,
	[Reason] [nvarchar](200) NULL,
	[SubmitUserId] [int] NULL,
	[SubmitTime] [datetime] NULL,
 CONSTRAINT [PK_Dep_AttendceApprovalRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Attendce]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dep_Attendce](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Status] [int] NULL,
	[ApprovalFlag] [int] NULL,
	[Reason] [nvarchar](500) NULL,
	[DepartSetId] [int] NULL,
 CONSTRAINT [PK_DEP_ATTENDCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Co_SystemLinkCourse]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Co_SystemLinkCourse](
	[SystemId] [int] NULL,
	[CourseId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Co_CourseSystem]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Co_CourseSystem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseSystemName] [nvarchar](50) NULL,
	[ParentID] [int] NULL,
	[Memo] [nvarchar](200) NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_COURSESYSTEM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Co_CourseResource]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Co_CourseResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[ResourceName] [nvarchar](500) NULL,
	[RealName] [nvarchar](500) NULL,
	[ResourceType] [int] NULL,
	[ResourceSize] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[PackId] [int] NULL,
	[Flag] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_COURSERESOURCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Co_CoursePaper]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Co_CoursePaper](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaperId] [int] NULL,
	[CourseId] [int] NULL,
	[Length] [int] NULL,
	[Hour] [decimal](18, 1) NULL,
	[TotalScore] [int] NULL,
	[LevelScore] [int] NULL,
	[TestTimes] [int] NULL,
	[IsMain] [int] NULL,
 CONSTRAINT [PK_Co_CoursePaper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Co_Course]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Co_Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[YearPlan] [int] NULL,
	[CourseName] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[Year] [int] NULL,
	[Month] [nvarchar](50) NULL,
	[Day] [int] NULL,
	[OpenLevel] [nvarchar](50) NULL,
	[IsMust] [int] NULL,
	[Way] [int] NULL,
	[Teacher] [nvarchar](50) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[NextStartTime] [datetime] NULL,
	[Sort] [nvarchar](50) NULL,
	[CourseLength] [decimal](18, 1) NULL,
	[RoomId] [int] NULL,
	[NumberLimited] [int] NULL,
	[IsCPA] [int] NULL,
	[IsOrder] [int] NULL,
	[IsLeave] [int] NULL,
	[IsRT] [int] NULL,
	[OpenFlag] [int] NULL,
	[OpenGroupObject] [nvarchar](500) NULL,
	[OpenDepartObject] [nvarchar](500) NULL,
	[IsTest] [int] NULL,
	[IsPing] [int] NULL,
	[SurveyPaperId] [nvarchar](50) NULL,
	[Memo] [text] NULL,
	[CourseFrom] [int] NULL,
	[StopOrderFlag] [int] NULL,
	[StopDucueFlag] [int] NULL,
	[ReturnTimes] [int] NULL,
	[AfterOrderTimes] [int] NULL,
	[Publishflag] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[PreAdviceConfigTime] [decimal](18, 1) NULL,
	[AfterEvlutionConfigTime] [decimal](18, 1) NULL,
	[PreCourseTime] [datetime] NULL,
	[CourseTimes] [int] NULL,
	[TrainDays] [int] NULL,
	[CourseAddress] [nvarchar](50) NULL,
	[CourseObjectMemo] [nvarchar](500) NULL,
	[IsDelete] [int] NULL,
	[OpenPerson] [nvarchar](500) NULL,
	[CpaTeacher] [nvarchar](50) NULL,
	[AttFlag] [int] NULL,
	[AttStatus] [int] NULL,
	[CourseLengthDistribute] [nvarchar](50) NULL,
	[IsSystem] [int] NULL,
	[IsYearPlan] [int] NULL,
	[IsOpenSub] [int] NULL,
	[courseProvide] [nvarchar](500) NULL,
	[studyRequirement] [text] NULL,
	[remark] [text] NULL,
	[AdFlag] [int] NULL,
	[DepCourseId] [int] NULL,
 CONSTRAINT [PK_COURSE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_VideoManage]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_VideoManage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Path] [nvarchar](200) NULL,
	[LastUpdateTime] [datetime] NULL,
	[Size] [int] NULL,
	[TopContent] [text] NULL,
	[LeftContent] [text] NULL,
	[RightContent] [text] NULL,
	[BottomContent] [text] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_CL_VIDEOMANAGE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_UserOverRecord]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_UserOverRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ReturnTimes] [int] NULL,
	[AfterOrderTimes] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_TimeOutOrder]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_TimeOutOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[ApprovalUser] [nvarchar](50) NULL,
	[OutUserID] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[ApprovalReason] [nvarchar](1000) NULL,
	[MakeUpTime] [datetime] NULL,
	[CourseName] [nvarchar](50) NULL,
	[OutTime] [datetime] NULL,
	[CourseStartTime] [datetime] NULL,
	[CourseEndTime] [datetime] NULL,
	[AttStartTime] [datetime] NULL,
	[AttEndTime] [datetime] NULL,
	[ApprovalMemo] [nvarchar](1000) NULL,
	[ApprovalFlag] [int] NULL,
	[ApprovalDateTime] [datetime] NULL,
	[requestid] [int] NULL,
	[FtriggerFlag] [int] NULL,
 CONSTRAINT [PK_CL_TIMEOUTORDER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_TimeOutLeave]    Script Date: 04/18/2014 08:59:00 ******/
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
/****** Object:  Table [dbo].[Cl_MidAttendce]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_MidAttendce](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[CreateTime] [datetime] NULL,
	[Time] [int] NULL,
 CONSTRAINT [PK_CL_MIDATTENDCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_MakeUpOrder]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_MakeUpOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[ApprovalUser] [nvarchar](50) NULL,
	[ApprovalMemo] [nvarchar](1000) NULL,
	[ApprovalFlag] [int] NULL,
	[ApprovalDateTime] [datetime] NULL,
	[ApprovalLimitTime] [datetime] NULL,
	[IsTimeOut] [int] NULL,
	[TimeOutMemo] [nvarchar](1000) NULL,
	[TimeOutUserID] [nvarchar](50) NULL,
	[TimeOutPassFlag] [int] NULL,
	[TimeOutApprovalMemo] [nvarchar](1000) NULL,
	[TimeOutDateTime] [datetime] NULL,
	[TimeOutApprovalDateTime] [datetime] NULL,
	[LeaveUserID] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[CourseName] [nvarchar](50) NULL,
	[LeaveTime] [datetime] NULL,
	[CourseStartTime] [datetime] NULL,
	[CourseEndTime] [datetime] NULL,
	[AttStartTime] [datetime] NULL,
	[AttEndTime] [datetime] NULL,
	[requestid] [int] NULL,
	[FtriggerFlag] [int] NULL,
 CONSTRAINT [PK_CL_MAKEUPORDER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_LearnVideoInfor]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_LearnVideoInfor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LearnId] [int] NULL,
	[Progress] [decimal](18, 1) NULL,
	[ResourseId] [int] NULL,
	[AttendId] [int] NULL,
	[LearnTimes] [int] NULL,
 CONSTRAINT [PK_CL_LEARNVIDEOINFOR] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_CpaLearnStatus]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_CpaLearnStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseID] [int] NULL,
	[UserID] [int] NULL,
	[IsAttFlag] [int] NULL,
	[IsPass] [int] NULL,
	[Progress] [int] NULL,
	[LearnTimes] [int] NULL,
	[GetLength] [decimal](18, 2) NULL,
	[LastUpdateTime] [datetime] NULL,
	[CpaFlag] [int] NULL,
	[GradeStatus] [int] NULL,
	[VedioTime] [decimal](18, 0) NULL,
 CONSTRAINT [PK_CL_CPALEARNSTATUS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_CourseOrder]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_CourseOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[OrderTime] [datetime] NULL,
	[OrderStatus] [int] NULL,
	[IsLeave] [int] NULL,
	[Reson] [nvarchar](1000) NULL,
	[ApprovalUser] [nvarchar](50) NULL,
	[ApprovalMemo] [nvarchar](3000) NULL,
	[LeaveTime] [datetime] NULL,
	[ApprovalFlag] [int] NULL,
	[ApprovalDateTime] [datetime] NULL,
	[ApprovalLimitTime] [datetime] NULL,
	[OrderTimes] [int] NULL,
	[LearnStatus] [int] NULL,
	[GetScore] [decimal](18, 2) NULL,
	[PassStatus] [int] NULL,
	[OrderEndTime] [datetime] NULL,
	[IsAppoint] [int] NULL,
	[LeaveUserID] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[CourseName] [nvarchar](50) NULL,
	[CourseStartTime] [datetime] NULL,
	[CourseEndTime] [datetime] NULL,
	[requestid] [int] NULL,
	[FtriggerFlag] [int] NULL,
	[MakeUpApprovalFlag] [int] NULL,
	[DropReason] [nvarchar](200) NULL,
	[DropType] [int] NULL,
	[AttScore] [decimal](18, 2) NULL,
	[AttFlag] [int] NULL,
 CONSTRAINT [PK_COURSEORDER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_CourseAdvice]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_CourseAdvice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[AdviceContent] [nvarchar](1000) NULL,
	[AdviceTime] [datetime] NULL,
	[AnserUserId] [int] NULL,
	[AnserContent] [nvarchar](1000) NULL,
	[AnserTime] [datetime] NULL,
	[VisibleFlag] [int] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_CourseAdvice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cl_Attendce]    Script Date: 04/18/2014 08:59:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cl_Attendce](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NULL,
	[UserId] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[OnlineStartTime] [datetime] NULL,
	[OnlineEndTime] [datetime] NULL,
	[LessLength] [decimal](18, 2) NULL,
	[ApprovalUser] [nvarchar](50) NULL,
	[ApprovalMemo] [nvarchar](1000) NULL,
	[ApprovalFlag] [int] NULL,
	[ApprovalDateTime] [datetime] NULL,
	[IsApp] [int] NULL,
	[Reason] [nvarchar](1000) NULL,
	[AppDateTime] [datetime] NULL,
	[AttStatus] [int] NULL,
	[FileName] [nvarchar](100) NULL,
	[FileRealName] [nvarchar](100) NULL,
 CONSTRAINT [PK_ATTENDCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Attendce_StartTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Attendce_StartTime]  DEFAULT ('2000-1-1') FOR [StartTime]
GO
/****** Object:  Default [DF_Attendce_EndTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Attendce_EndTime]  DEFAULT (((2000)-(1))-(1)) FOR [EndTime]
GO
/****** Object:  Default [DF_Attendce_LessLength]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Attendce_LessLength]  DEFAULT ((0)) FOR [LessLength]
GO
/****** Object:  Default [DF_Attendce_ApprovalUser]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Attendce_ApprovalUser]  DEFAULT ((0)) FOR [ApprovalUser]
GO
/****** Object:  Default [DF_Attendce_ApprovalMemo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Attendce_ApprovalMemo]  DEFAULT ('') FOR [ApprovalMemo]
GO
/****** Object:  Default [DF_Attendce_ApprovalFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Attendce_ApprovalFlag]  DEFAULT ((0)) FOR [ApprovalFlag]
GO
/****** Object:  Default [DF_Cl_Attendce_IsApp]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Cl_Attendce_IsApp]  DEFAULT ((0)) FOR [IsApp]
GO
/****** Object:  Default [DF_Cl_Attendce_Reason]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Cl_Attendce_Reason]  DEFAULT ('') FOR [Reason]
GO
/****** Object:  Default [DF_Cl_Attendce_AppDateTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Cl_Attendce_AppDateTime]  DEFAULT (((2000)-(1))-(1)) FOR [AppDateTime]
GO
/****** Object:  Default [DF_Cl_Attendce_AttStatus]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Cl_Attendce_AttStatus]  DEFAULT ((0)) FOR [AttStatus]
GO
/****** Object:  Default [DF_Cl_Attendce_FilePath]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Cl_Attendce_FilePath]  DEFAULT ('') FOR [FileName]
GO
/****** Object:  Default [DF_Cl_Attendce_FileRealName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_Attendce] ADD  CONSTRAINT [DF_Cl_Attendce_FileRealName]  DEFAULT ('') FOR [FileRealName]
GO
/****** Object:  Default [DF_CourseAdvice_AdviceContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseAdvice] ADD  CONSTRAINT [DF_CourseAdvice_AdviceContent]  DEFAULT ('') FOR [AdviceContent]
GO
/****** Object:  Default [DF_CourseAdvice_AnserContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseAdvice] ADD  CONSTRAINT [DF_CourseAdvice_AnserContent]  DEFAULT ('') FOR [AnserContent]
GO
/****** Object:  Default [DF_Courseorder_OrderTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_OrderTime]  DEFAULT (((2000)-(1))-(1)) FOR [OrderTime]
GO
/****** Object:  Default [DF_Courseorder_OrderStatus]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_OrderStatus]  DEFAULT ((0)) FOR [OrderStatus]
GO
/****** Object:  Default [DF_Courseorder_IsLeave]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_IsLeave]  DEFAULT ((0)) FOR [IsLeave]
GO
/****** Object:  Default [DF_Cl_CourseOrder_LeaveMemo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Cl_CourseOrder_LeaveMemo]  DEFAULT ('') FOR [Reson]
GO
/****** Object:  Default [DF_Courseorder_ApprovalUser]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_ApprovalUser]  DEFAULT ((0)) FOR [ApprovalUser]
GO
/****** Object:  Default [DF_Courseorder_ApprovalMemo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_ApprovalMemo]  DEFAULT ('') FOR [ApprovalMemo]
GO
/****** Object:  Default [DF_Courseorder_ApprovalFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_ApprovalFlag]  DEFAULT ((0)) FOR [ApprovalFlag]
GO
/****** Object:  Default [DF_Courseorder_ApprovalDateTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_ApprovalDateTime]  DEFAULT (((2000)-(1))-(1)) FOR [ApprovalDateTime]
GO
/****** Object:  Default [DF_Courseorder_ApprovalLimitTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_ApprovalLimitTime]  DEFAULT (((2000)-(1))-(1)) FOR [ApprovalLimitTime]
GO
/****** Object:  Default [DF_Courseorder_OrderTimes]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_OrderTimes]  DEFAULT ((0)) FOR [OrderTimes]
GO
/****** Object:  Default [DF_Courseorder_LearnStatus]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_LearnStatus]  DEFAULT ((0)) FOR [LearnStatus]
GO
/****** Object:  Default [DF_Courseorder_GetScore]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_GetScore]  DEFAULT ((0)) FOR [GetScore]
GO
/****** Object:  Default [DF_Courseorder_PassStatus]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Courseorder_PassStatus]  DEFAULT ((0)) FOR [PassStatus]
GO
/****** Object:  Default [DF_Cl_CourseOrder_OrderEndTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Cl_CourseOrder_OrderEndTime]  DEFAULT (((2000)-(1))-(1)) FOR [OrderEndTime]
GO
/****** Object:  Default [DF_Cl_CourseOrder_IsAppoint]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Cl_CourseOrder_IsAppoint]  DEFAULT ((0)) FOR [IsAppoint]
GO
/****** Object:  Default [DF_Cl_CourseOrder_FtriggerFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Cl_CourseOrder_FtriggerFlag]  DEFAULT ((0)) FOR [FtriggerFlag]
GO
/****** Object:  Default [DF_Cl_CourseOrder_MakeUpApprovalFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  CONSTRAINT [DF_Cl_CourseOrder_MakeUpApprovalFlag]  DEFAULT ((0)) FOR [MakeUpApprovalFlag]
GO
/****** Object:  Default [DF__Cl_Course__DropR__7EF6D905]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  DEFAULT ('') FOR [DropReason]
GO
/****** Object:  Default [DF__Cl_Course__DropT__7FEAFD3E]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  DEFAULT ((0)) FOR [DropType]
GO
/****** Object:  Default [DF__Cl_Course__AttSc__02C769E9]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  DEFAULT ((0)) FOR [AttScore]
GO
/****** Object:  Default [DF__Cl_Course__AttFl__03BB8E22]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CourseOrder] ADD  DEFAULT ((0)) FOR [AttFlag]
GO
/****** Object:  Default [DF_Cl_CpaLearnStatus_GetLength]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CpaLearnStatus] ADD  CONSTRAINT [DF_Cl_CpaLearnStatus_GetLength]  DEFAULT ((0)) FOR [GetLength]
GO
/****** Object:  Default [DF_Cl_CpaLearnStatus_CpaFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CpaLearnStatus] ADD  CONSTRAINT [DF_Cl_CpaLearnStatus_CpaFlag]  DEFAULT ((0)) FOR [CpaFlag]
GO
/****** Object:  Default [DF_Cl_CpaLearnStatus_GradeStatus]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_CpaLearnStatus] ADD  CONSTRAINT [DF_Cl_CpaLearnStatus_GradeStatus]  DEFAULT ((0)) FOR [GradeStatus]
GO
/****** Object:  Default [DF_Cl_LearnVideoInfor_LearnTimes]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_LearnVideoInfor] ADD  CONSTRAINT [DF_Cl_LearnVideoInfor_LearnTimes]  DEFAULT ((0)) FOR [LearnTimes]
GO
/****** Object:  Default [DF_Cl_MakeUpOrder_ApprovalUser]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_MakeUpOrder] ADD  CONSTRAINT [DF_Cl_MakeUpOrder_ApprovalUser]  DEFAULT ('') FOR [ApprovalUser]
GO
/****** Object:  Default [DF_Cl_MakeUpOrder_ApprovalMemo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_MakeUpOrder] ADD  CONSTRAINT [DF_Cl_MakeUpOrder_ApprovalMemo]  DEFAULT ('') FOR [ApprovalMemo]
GO
/****** Object:  Default [DF_Cl_MakeUpOrder_TimeOutMemo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_MakeUpOrder] ADD  CONSTRAINT [DF_Cl_MakeUpOrder_TimeOutMemo]  DEFAULT ('') FOR [TimeOutMemo]
GO
/****** Object:  Default [DF_Cl_MakeUpOrder_TimeOutUser]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_MakeUpOrder] ADD  CONSTRAINT [DF_Cl_MakeUpOrder_TimeOutUser]  DEFAULT ('') FOR [TimeOutUserID]
GO
/****** Object:  Default [DF_Cl_MakeUpOrder_TimeOutApprovalMemo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_MakeUpOrder] ADD  CONSTRAINT [DF_Cl_MakeUpOrder_TimeOutApprovalMemo]  DEFAULT ('') FOR [TimeOutApprovalMemo]
GO
/****** Object:  Default [DF_Cl_MakeUpOrder_FtriggerFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_MakeUpOrder] ADD  CONSTRAINT [DF_Cl_MakeUpOrder_FtriggerFlag]  DEFAULT ((0)) FOR [FtriggerFlag]
GO
/****** Object:  Default [DF_Cl_TimeOutLeave_FtriggerFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_TimeOutLeave] ADD  CONSTRAINT [DF_Cl_TimeOutLeave_FtriggerFlag]  DEFAULT ((0)) FOR [FtriggerFlag]
GO
/****** Object:  Default [DF_Cl_TimeOutOrder_FtriggerFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_TimeOutOrder] ADD  CONSTRAINT [DF_Cl_TimeOutOrder_FtriggerFlag]  DEFAULT ((0)) FOR [FtriggerFlag]
GO
/****** Object:  Default [DF_Cl_UserOverRecord_ReturnTimes]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_UserOverRecord] ADD  CONSTRAINT [DF_Cl_UserOverRecord_ReturnTimes]  DEFAULT ((0)) FOR [ReturnTimes]
GO
/****** Object:  Default [DF_Cl_UserOverRecord_AfterOrderTimes]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_UserOverRecord] ADD  CONSTRAINT [DF_Cl_UserOverRecord_AfterOrderTimes]  DEFAULT ((0)) FOR [AfterOrderTimes]
GO
/****** Object:  Default [DF_Cl_VideoManage_Name]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_Name]  DEFAULT ('') FOR [Name]
GO
/****** Object:  Default [DF_Cl_VideoManage_Path]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_Path]  DEFAULT ('') FOR [Path]
GO
/****** Object:  Default [DF_Cl_VideoManage_LastUpdateTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_LastUpdateTime]  DEFAULT (((2000)-(1))-(1)) FOR [LastUpdateTime]
GO
/****** Object:  Default [DF_Cl_VideoManage_Size]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_Size]  DEFAULT ((0)) FOR [Size]
GO
/****** Object:  Default [DF_Cl_VideoManage_TopContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_TopContent]  DEFAULT ('') FOR [TopContent]
GO
/****** Object:  Default [DF_Cl_VideoManage_LeftContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_LeftContent]  DEFAULT ('') FOR [LeftContent]
GO
/****** Object:  Default [DF_Cl_VideoManage_RightContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_RightContent]  DEFAULT ('') FOR [RightContent]
GO
/****** Object:  Default [DF_Cl_VideoManage_BottomContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_BottomContent]  DEFAULT ('') FOR [BottomContent]
GO
/****** Object:  Default [DF_Cl_VideoManage_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Cl_VideoManage] ADD  CONSTRAINT [DF_Cl_VideoManage_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Course_CourseName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_CourseName]  DEFAULT ('') FOR [CourseName]
GO
/****** Object:  Default [DF_Course_Code]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_Code]  DEFAULT ('') FOR [Code]
GO
/****** Object:  Default [DF_Course_Year]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_Year]  DEFAULT ((0)) FOR [Year]
GO
/****** Object:  Default [DF_Course_Month]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_Month]  DEFAULT ((0)) FOR [Month]
GO
/****** Object:  Default [DF_Course_Day]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_Day]  DEFAULT ((0)) FOR [Day]
GO
/****** Object:  Default [DF_Course_OpenLevel]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_OpenLevel]  DEFAULT ('') FOR [OpenLevel]
GO
/****** Object:  Default [DF_Course_IsMust]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_IsMust]  DEFAULT ((0)) FOR [IsMust]
GO
/****** Object:  Default [DF_Course_Way]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_Way]  DEFAULT ((0)) FOR [Way]
GO
/****** Object:  Default [DF_Course_Teacher]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_Teacher]  DEFAULT ('') FOR [Teacher]
GO
/****** Object:  Default [DF_Course_StartTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_StartTime]  DEFAULT (((2000)-(1))-(1)) FOR [StartTime]
GO
/****** Object:  Default [DF_Course_EndTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_EndTime]  DEFAULT (((2000)-(1))-(1)) FOR [EndTime]
GO
/****** Object:  Default [DF_Course_Sort]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_Sort]  DEFAULT ((0)) FOR [Sort]
GO
/****** Object:  Default [DF_Course_CourseLength]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_CourseLength]  DEFAULT ((0)) FOR [CourseLength]
GO
/****** Object:  Default [DF_Course_RoomId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_RoomId]  DEFAULT ((0)) FOR [RoomId]
GO
/****** Object:  Default [DF_Course_NumberLimited]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_NumberLimited]  DEFAULT ((0)) FOR [NumberLimited]
GO
/****** Object:  Default [DF_Course_IsCPA]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_IsCPA]  DEFAULT ((0)) FOR [IsCPA]
GO
/****** Object:  Default [DF_Course_IsOrder]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_IsOrder]  DEFAULT ((0)) FOR [IsOrder]
GO
/****** Object:  Default [DF_Course_IsLeave]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_IsLeave]  DEFAULT ((0)) FOR [IsLeave]
GO
/****** Object:  Default [DF_Course_IsRT]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_IsRT]  DEFAULT ((0)) FOR [IsRT]
GO
/****** Object:  Default [DF_Course_OpenFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_OpenFlag]  DEFAULT ((0)) FOR [OpenFlag]
GO
/****** Object:  Default [DF_Course_OpenGroupObject]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_OpenGroupObject]  DEFAULT ('') FOR [OpenGroupObject]
GO
/****** Object:  Default [DF_Course_OpenDepartObject]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_OpenDepartObject]  DEFAULT ('') FOR [OpenDepartObject]
GO
/****** Object:  Default [DF_Course_IsTest]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_IsTest]  DEFAULT ((0)) FOR [IsTest]
GO
/****** Object:  Default [DF_Course_IsPing]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_IsPing]  DEFAULT ((0)) FOR [IsPing]
GO
/****** Object:  Default [DF_Course_SurveyPaperId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_SurveyPaperId]  DEFAULT ('') FOR [SurveyPaperId]
GO
/****** Object:  Default [DF_Course_Memo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Course_Memo]  DEFAULT ('') FOR [Memo]
GO
/****** Object:  Default [DF_Co_Course_PreAdviceConfigTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Co_Course_PreAdviceConfigTime]  DEFAULT ((0)) FOR [PreAdviceConfigTime]
GO
/****** Object:  Default [DF_Co_Course_AfterEvlutionConfigTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Co_Course_AfterEvlutionConfigTime]  DEFAULT ((0)) FOR [AfterEvlutionConfigTime]
GO
/****** Object:  Default [DF_Co_Course_TrainDays]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Co_Course_TrainDays]  DEFAULT ((0)) FOR [TrainDays]
GO
/****** Object:  Default [DF_Co_Course_CourseAddress]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Co_Course_CourseAddress]  DEFAULT ('') FOR [CourseAddress]
GO
/****** Object:  Default [DF_Co_Course_CourseObjectMemo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  CONSTRAINT [DF_Co_Course_CourseObjectMemo]  DEFAULT ('') FOR [CourseObjectMemo]
GO
/****** Object:  Default [DF__Co_Course__AttFl__719CDDE7]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  DEFAULT ((1)) FOR [AttFlag]
GO
/****** Object:  Default [DF__Co_Course__AttSt__72910220]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  DEFAULT ((0)) FOR [AttStatus]
GO
/****** Object:  Default [DF__Co_Course__Cours__74794A92]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  DEFAULT ((0)) FOR [CourseLengthDistribute]
GO
/****** Object:  Default [DF__Co_Course__IsSys__756D6ECB]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  DEFAULT ((0)) FOR [IsSystem]
GO
/****** Object:  Default [DF__Co_Course__IsYea__76619304]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  DEFAULT ((0)) FOR [IsYearPlan]
GO
/****** Object:  Default [DF__Co_Course__IsOpe__793DFFAF]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  DEFAULT ((1)) FOR [IsOpenSub]
GO
/****** Object:  Default [DF__Co_Course__AdFla__04AFB25B]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  DEFAULT ((0)) FOR [AdFlag]
GO
/****** Object:  Default [DF__Co_Course__DepCo__4B0D20AB]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_Course] ADD  DEFAULT ((0)) FOR [DepCourseId]
GO
/****** Object:  Default [DF_Co_CoursePaper_IsMain]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_CoursePaper] ADD  CONSTRAINT [DF_Co_CoursePaper_IsMain]  DEFAULT ((0)) FOR [IsMain]
GO
/****** Object:  Default [DF_CourseResource_ResourceName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_CourseResource] ADD  CONSTRAINT [DF_CourseResource_ResourceName]  DEFAULT ('') FOR [ResourceName]
GO
/****** Object:  Default [DF_CourseResource_RealName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_CourseResource] ADD  CONSTRAINT [DF_CourseResource_RealName]  DEFAULT ('') FOR [RealName]
GO
/****** Object:  Default [DF_Co_CourseResource_Flag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_CourseResource] ADD  CONSTRAINT [DF_Co_CourseResource_Flag]  DEFAULT ((0)) FOR [Flag]
GO
/****** Object:  Default [DF_CourseSystem_CourseSystemName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Co_CourseSystem] ADD  CONSTRAINT [DF_CourseSystem_CourseSystemName]  DEFAULT ('') FOR [CourseSystemName]
GO
/****** Object:  Default [DF_Dep_Attendce_Status]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Attendce] ADD  CONSTRAINT [DF_Dep_Attendce_Status]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_Dep_Attendce_ApprovalFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Attendce] ADD  CONSTRAINT [DF_Dep_Attendce_ApprovalFlag]  DEFAULT ((0)) FOR [ApprovalFlag]
GO
/****** Object:  Default [DF_Dep_Attendce_Reason]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Attendce] ADD  CONSTRAINT [DF_Dep_Attendce_Reason]  DEFAULT ((0)) FOR [Reason]
GO
/****** Object:  Default [DF_Dep_Attendce_DepartSetId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Attendce] ADD  CONSTRAINT [DF_Dep_Attendce_DepartSetId]  DEFAULT ((0)) FOR [DepartSetId]
GO
/****** Object:  Default [DF_Dep_Course_DeptId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Course] ADD  CONSTRAINT [DF_Dep_Course_DeptId]  DEFAULT ((0)) FOR [DeptId]
GO
/****** Object:  Default [DF_Dep_Course_OpenUserId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Course] ADD  CONSTRAINT [DF_Dep_Course_OpenUserId]  DEFAULT ((0)) FOR [OpenUserId]
GO
/****** Object:  Default [DF_Dep_CourseOrder_OrderStatus]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_OrderStatus]  DEFAULT ((0)) FOR [OrderStatus]
GO
/****** Object:  Default [DF_Dep_CourseOrder_LearnStatus]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_LearnStatus]  DEFAULT ((0)) FOR [LearnStatus]
GO
/****** Object:  Default [DF_Dep_CourseOrder_GetScore]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_GetScore]  DEFAULT ((0)) FOR [GetScore]
GO
/****** Object:  Default [DF_Dep_CourseOrder_AttScore]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_AttScore]  DEFAULT ((0)) FOR [AttScore]
GO
/****** Object:  Default [DF_Dep_CourseOrder_EvlutionScore]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_EvlutionScore]  DEFAULT ((0)) FOR [EvlutionScore]
GO
/****** Object:  Default [DF_Dep_CourseOrder_ExamScore]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_ExamScore]  DEFAULT ((0)) FOR [ExamScore]
GO
/****** Object:  Default [DF_Dep_CourseOrder_DepartSetId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_DepartSetId]  DEFAULT ((0)) FOR [DepartSetId]
GO
/****** Object:  Default [DF_Dep_CourseOrder_IsLeave]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_IsLeave]  DEFAULT ((0)) FOR [IsLeave]
GO
/****** Object:  Default [DF_Dep_CourseOrder_Reason]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_Reason]  DEFAULT ('') FOR [Reason]
GO
/****** Object:  Default [DF_Dep_CourseOrder_ApprovalFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_ApprovalFlag]  DEFAULT ((0)) FOR [ApprovalFlag]
GO
/****** Object:  Default [DF_Dep_CourseOrder_ApprovalUserId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_ApprovalUserId]  DEFAULT ((0)) FOR [ApprovalUserId]
GO
/****** Object:  Default [DF_Dep_CourseOrder_OrderTimes]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_OrderTimes]  DEFAULT ((0)) FOR [OrderTimes]
GO
/****** Object:  Default [DF_Dep_CourseOrder_IsAppoint]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_IsAppoint]  DEFAULT ((0)) FOR [IsAppoint]
GO
/****** Object:  Default [DF__Dep_Cours__Appro__473C8FC7]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  DEFAULT ('') FOR [ApprovalUser]
GO
/****** Object:  Default [DF__Dep_Cours__reque__4830B400]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  DEFAULT ((0)) FOR [requestid]
GO
/****** Object:  Default [DF__Dep_Cours__Ftrig__4924D839]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  DEFAULT ((0)) FOR [FtriggerFlag]
GO
/****** Object:  Default [DF_Dep_CpaLearnStatus_GetLength]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CpaLearnStatus] ADD  CONSTRAINT [DF_Dep_CpaLearnStatus_GetLength]  DEFAULT ((0)) FOR [GetLength]
GO
/****** Object:  Default [DF_Dep_CpaLearnStatus_CpaFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_CpaLearnStatus] ADD  CONSTRAINT [DF_Dep_CpaLearnStatus_CpaFlag]  DEFAULT ((0)) FOR [CpaFlag]
GO
/****** Object:  Default [DF_Dep_Survey_Exampaper_ExamTitle]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_Exampaper] ADD  CONSTRAINT [DF_Dep_Survey_Exampaper_ExamTitle]  DEFAULT ('') FOR [ExamTitle]
GO
/****** Object:  Default [DF_Dep_Survey_Exampaper_ExamDescription]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_Exampaper] ADD  CONSTRAINT [DF_Dep_Survey_Exampaper_ExamDescription]  DEFAULT ('') FOR [ExamDescription]
GO
/****** Object:  Default [DF_Dep_Survey_Exampaper_Status]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_Exampaper] ADD  CONSTRAINT [DF_Dep_Survey_Exampaper_Status]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Dep_Survey_ExampaperCategory_CategoryName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_ExampaperCategory] ADD  CONSTRAINT [DF_Dep_Survey_ExampaperCategory_CategoryName]  DEFAULT ('') FOR [CategoryName]
GO
/****** Object:  Default [DF_Dep_Survey_Question_LinkSortPayGrade]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_Question] ADD  CONSTRAINT [DF_Dep_Survey_Question_LinkSortPayGrade]  DEFAULT ('') FOR [LinkSortPayGrade]
GO
/****** Object:  Default [DF_Dep_Survey_QuestionAnswer_AnswerContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_QuestionAnswer] ADD  CONSTRAINT [DF_Dep_Survey_QuestionAnswer_AnswerContent]  DEFAULT ('') FOR [AnswerContent]
GO
/****** Object:  Default [DF_Dep_Survey_ReplyAnswer_SubjectiveAnswer]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Dep_Survey_ReplyAnswer_SubjectiveAnswer]  DEFAULT ('') FOR [SubjectiveAnswer]
GO
/****** Object:  Default [DF_Dep_Survey_ReplyAnswer_ObjectiveAnswer]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Dep_Survey_ReplyAnswer_ObjectiveAnswer]  DEFAULT ('') FOR [ObjectiveAnswer]
GO
/****** Object:  Default [DF_Dep_Survey_ReplyAnswer_Status]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Dep_Survey_ReplyAnswer_Status]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_Dep_Survey_Survey_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Dep_Survey_Survey] ADD  CONSTRAINT [DF_Dep_Survey_Survey_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_DepTran_Attendce_DepartSetId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[DepTran_Attendce] ADD  CONSTRAINT [DF_DepTran_Attendce_DepartSetId]  DEFAULT ((0)) FOR [DepartSetId]
GO
/****** Object:  Default [DF_DepTran_CourseOpen_AttFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[DepTran_CourseOpen] ADD  CONSTRAINT [DF_DepTran_CourseOpen_AttFlag]  DEFAULT ((0)) FOR [AttFlag]
GO
/****** Object:  Default [DF_DepTran_CourseOpen_OpenFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[DepTran_CourseOpen] ADD  CONSTRAINT [DF_DepTran_CourseOpen_OpenFlag]  DEFAULT ((0)) FOR [OpenFlag]
GO
/****** Object:  Default [DF__DepTran_C__Appro__436BFEE3]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[DepTran_CourseOpen] ADD  DEFAULT ((0)) FOR [ApprovalFlag]
GO
/****** Object:  Default [DF_DepTran_CourseOpen_LimitNumber]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[DepTran_CourseOpen] ADD  CONSTRAINT [DF_DepTran_CourseOpen_LimitNumber]  DEFAULT ((0)) FOR [LimitNumber]
GO
/****** Object:  Default [DF_DepTran_CourseOrder_DepartSetId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[DepTran_CourseOrder] ADD  CONSTRAINT [DF_DepTran_CourseOrder_DepartSetId]  DEFAULT ((0)) FOR [DepartSetId]
GO
/****** Object:  Default [DF__New_Class__RoomN__32767D0B]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ('') FOR [RoomName]
GO
/****** Object:  Default [DF__New_ClassR__Memo__336AA144]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ('') FOR [Memo]
GO
/****** Object:  Default [DF__New_Class__RoomN__345EC57D]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [RoomNumber]
GO
/****** Object:  Default [DF__New_Class__Colum__3552E9B6]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [ColumnNumber]
GO
/****** Object:  Default [DF__New_Class__RowNu__36470DEF]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [RowNumber]
GO
/****** Object:  Default [DF__New_Class__IsDel__373B3228]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_New_ClassRoom_DisSeat]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  CONSTRAINT [DF_New_ClassRoom_DisSeat]  DEFAULT ('') FOR [DisSeat]
GO
/****** Object:  Default [DF__New_Class__Class__4DE98D56]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ('') FOR [Classes]
GO
/****** Object:  Default [DF__New_Class__PrePe__4EDDB18F]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [PrePersonCount]
GO
/****** Object:  Default [DF__New_Class__SeatT__4FD1D5C8]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [SeatType]
GO
/****** Object:  Default [DF_New_Course_LastUpdateTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_Course] ADD  CONSTRAINT [DF_New_Course_LastUpdateTime]  DEFAULT (getdate()) FOR [LastUpdateTime]
GO
/****** Object:  Default [DF_New_Course_PublicFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_Course] ADD  CONSTRAINT [DF_New_Course_PublicFlag]  DEFAULT ((0)) FOR [PublicFlag]
GO
/****** Object:  Default [DF_New_Course_VideoLowLength]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_Course] ADD  CONSTRAINT [DF_New_Course_VideoLowLength]  DEFAULT ((0)) FOR [VideoLowLength]
GO
/****** Object:  Default [DF__New_Course__Flag__5E54FF49]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_CourseFiles] ADD  DEFAULT ((0)) FOR [Flag]
GO
/****** Object:  Default [DF_New_CourseOrderDetail_IsLeave]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_CourseOrderDetail] ADD  CONSTRAINT [DF_New_CourseOrderDetail_IsLeave]  DEFAULT ((0)) FOR [IsLeave]
GO
/****** Object:  Default [DF_New_CourseRoomRule_GroupName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_CourseRoomRule] ADD  CONSTRAINT [DF_New_CourseRoomRule_GroupName]  DEFAULT ('') FOR [GroupName]
GO
/****** Object:  Default [DF__New_Cours__After__4B422AD5]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[New_CourseRoomRule] ADD  DEFAULT ((0)) FOR [AfterEvlutionConfigTime]
GO
/****** Object:  Default [DF__Re_MyReso__Score__5006DFF2]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_MyResource] ADD  DEFAULT ((-1)) FOR [Score]
GO
/****** Object:  Default [DF__Re_MyReso__Satis__50FB042B]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_MyResource] ADD  DEFAULT ((-1)) FOR [Satisfaction]
GO
/****** Object:  Default [DF__Re_MyReso__Pract__51EF2864]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_MyResource] ADD  DEFAULT ((-1)) FOR [Practical]
GO
/****** Object:  Default [DF__Re_MyReso__Atten__52E34C9D]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_MyResource] ADD  DEFAULT ((-1)) FOR [Attention]
GO
/****** Object:  Default [DF__Re_Resour__OpenN__53D770D6]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [OpenNum]
GO
/****** Object:  Default [DF__Re_Resour__Score__54CB950F]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [Score]
GO
/****** Object:  Default [DF__Re_Resour__Score__55BFB948]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [ScoreNum]
GO
/****** Object:  Default [DF__Re_Resour__Satis__56B3DD81]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [Satisfaction]
GO
/****** Object:  Default [DF__Re_Resour__Satis__57A801BA]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [SatisfactionNum]
GO
/****** Object:  Default [DF__Re_Resour__Pract__589C25F3]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [Practical]
GO
/****** Object:  Default [DF__Re_Resour__Pract__59904A2C]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [PracticalNum]
GO
/****** Object:  Default [DF__Re_Resour__Atten__5A846E65]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [Attention]
GO
/****** Object:  Default [DF__Re_Resour__Atten__5B78929E]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [AttentionNum]
GO
/****** Object:  Default [DF__Re_Resour__IsRec__5C6CB6D7]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [IsRecommend]
GO
/****** Object:  Default [DF__Re_Resour__Statu__5D60DB10]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Re_Resource] ADD  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_Survey_Exampaper_ExamTitle]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_Exampaper] ADD  CONSTRAINT [DF_Survey_Exampaper_ExamTitle]  DEFAULT ('') FOR [ExamTitle]
GO
/****** Object:  Default [DF_Survey_Exampaper_ExamDescription]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_Exampaper] ADD  CONSTRAINT [DF_Survey_Exampaper_ExamDescription]  DEFAULT ('') FOR [ExamDescription]
GO
/****** Object:  Default [DF_Survey_Exampaper_Status]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_Exampaper] ADD  CONSTRAINT [DF_Survey_Exampaper_Status]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Survey_ExampaperCategory_CategoryName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_ExampaperCategory] ADD  CONSTRAINT [DF_Survey_ExampaperCategory_CategoryName]  DEFAULT ('') FOR [CategoryName]
GO
/****** Object:  Default [DF_Survey_Question_LinkSortPayGrade]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_Question] ADD  CONSTRAINT [DF_Survey_Question_LinkSortPayGrade]  DEFAULT ('') FOR [LinkSortPayGrade]
GO
/****** Object:  Default [DF__Survey_Qu__Objec__467D75B8]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_Question] ADD  DEFAULT ((0)) FOR [ObjectType]
GO
/****** Object:  Default [DF_Survey_QuestionAnswer_AnswerContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_QuestionAnswer] ADD  CONSTRAINT [DF_Survey_QuestionAnswer_AnswerContent]  DEFAULT ('') FOR [AnswerContent]
GO
/****** Object:  Default [DF_Survey_ReplyAnswer_SubjectiveAnswer]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Survey_ReplyAnswer_SubjectiveAnswer]  DEFAULT ('') FOR [SubjectiveAnswer]
GO
/****** Object:  Default [DF_Survey_ReplyAnswer_ObjectiveAnswer]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Survey_ReplyAnswer_ObjectiveAnswer]  DEFAULT ('') FOR [ObjectiveAnswer]
GO
/****** Object:  Default [DF_Survey_ReplyAnswer_Status]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Survey_ReplyAnswer_Status]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF__Survey_Re__Teach__44952D46]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_ReplyAnswer] ADD  DEFAULT ((0)) FOR [TeacherID]
GO
/****** Object:  Default [DF__Survey_Re__Cours__4589517F]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_ReplyAnswer] ADD  DEFAULT ((0)) FOR [CourseRoomRuleId]
GO
/****** Object:  Default [DF_Survey_Survey_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Survey_Survey] ADD  CONSTRAINT [DF_Survey_Survey_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_ClassRoom_RoomName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ClassRoom] ADD  CONSTRAINT [DF_ClassRoom_RoomName]  DEFAULT ('') FOR [RoomName]
GO
/****** Object:  Default [DF_ClassRoom_Memo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ClassRoom] ADD  CONSTRAINT [DF_ClassRoom_Memo]  DEFAULT ('') FOR [Memo]
GO
/****** Object:  Default [DF_ClassRoom_RoomNumber]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ClassRoom] ADD  CONSTRAINT [DF_ClassRoom_RoomNumber]  DEFAULT ((0)) FOR [RoomNumber]
GO
/****** Object:  Default [DF_ClassRoom_ColumnCount]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ClassRoom] ADD  CONSTRAINT [DF_ClassRoom_ColumnCount]  DEFAULT ((0)) FOR [ColumnNumber]
GO
/****** Object:  Default [DF_ClassRoom_RowCount]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ClassRoom] ADD  CONSTRAINT [DF_ClassRoom_RowCount]  DEFAULT ((0)) FOR [RowNumber]
GO
/****** Object:  Default [DF_ClassRoom_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ClassRoom] ADD  CONSTRAINT [DF_ClassRoom_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Sys_Department_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Department] ADD  CONSTRAINT [DF_Sys_Department_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Sys_UserGroup_GroupName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Group] ADD  CONSTRAINT [DF_Sys_UserGroup_GroupName]  DEFAULT ('') FOR [GroupName]
GO
/****** Object:  Default [DF_Sys_Group_Memo]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Group] ADD  CONSTRAINT [DF_Sys_Group_Memo]  DEFAULT ('') FOR [Memo]
GO
/****** Object:  Default [DF_Sys_UserGroup_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Group] ADD  CONSTRAINT [DF_Sys_UserGroup_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Sys_Log_LogTitle]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Log] ADD  CONSTRAINT [DF_Sys_Log_LogTitle]  DEFAULT ('') FOR [LogTitle]
GO
/****** Object:  Default [DF_Sys_Log_LogContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Log] ADD  CONSTRAINT [DF_Sys_Log_LogContent]  DEFAULT ('') FOR [LogContent]
GO
/****** Object:  Default [DF_Sys_Log_LogTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Log] ADD  CONSTRAINT [DF_Sys_Log_LogTime]  DEFAULT (((2000)-(1))-(1)) FOR [LogTime]
GO
/****** Object:  Default [DF_Sys_Notes_Type]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF_Sys_Notes_Type]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF_Sys_Notes_NoteTitle]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF_Sys_Notes_NoteTitle]  DEFAULT ('') FOR [NoteTitle]
GO
/****** Object:  Default [DF_Sys_Notes_NoteContent]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF_Sys_Notes_NoteContent]  DEFAULT ('') FOR [NoteContent]
GO
/****** Object:  Default [DF_Sys_Notes_CreateTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF_Sys_Notes_CreateTime]  DEFAULT (((2000)-(1))-(1)) FOR [CreateTime]
GO
/****** Object:  Default [DF_Sys_Notes_PublishFlag]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF_Sys_Notes_PublishFlag]  DEFAULT ((0)) FOR [PublishFlag]
GO
/****** Object:  Default [DF_Sys_Notes_PublishTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF_Sys_Notes_PublishTime]  DEFAULT (((2000)-(1))-(1)) FOR [PublishTime]
GO
/****** Object:  Default [DF_Sys_Notes_IsTop]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF_Sys_Notes_IsTop]  DEFAULT ((0)) FOR [IsTop]
GO
/****** Object:  Default [DF_Sys_Notes_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF_Sys_Notes_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF__Sys_Notes__Num__00DF2177]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF__Sys_Notes__Num__00DF2177]  DEFAULT ((0)) FOR [Num]
GO
/****** Object:  Default [DF__Sys_Notes__AdFla__05A3D694]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF__Sys_Notes__AdFla__05A3D694]  DEFAULT ((0)) FOR [AdFlag]
GO
/****** Object:  Default [DF__Sys_Notes__DepTr__4865BE2A]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Notes] ADD  CONSTRAINT [DF__Sys_Notes__DepTr__4865BE2A]  DEFAULT ((1)) FOR [DepTrainFlag]
GO
/****** Object:  Default [DF_Sys_NoteSort_SortName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_NoteSort] ADD  CONSTRAINT [DF_Sys_NoteSort_SortName]  DEFAULT ('') FOR [SortName]
GO
/****** Object:  Default [DF_Sys_NoteSort_ParentID]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_NoteSort] ADD  CONSTRAINT [DF_Sys_NoteSort_ParentID]  DEFAULT ((0)) FOR [ParentID]
GO
/****** Object:  Default [DF_Sys_NoteSort_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_NoteSort] ADD  CONSTRAINT [DF_Sys_NoteSort_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Sys_NoteSort_Type]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_NoteSort] ADD  CONSTRAINT [DF_Sys_NoteSort_Type]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF_Sys_ParamConfig_ConfigType]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ParamConfig] ADD  CONSTRAINT [DF_Sys_ParamConfig_ConfigType]  DEFAULT ((0)) FOR [ConfigType]
GO
/****** Object:  Default [DF_Sys_ParamConfig_LastUpdateTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ParamConfig] ADD  CONSTRAINT [DF_Sys_ParamConfig_LastUpdateTime]  DEFAULT (((2000)-(1))-(1)) FOR [LastUpdateTime]
GO
/****** Object:  Default [DF__Sys_Param__userT__477199F1]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_ParamConfig] ADD  DEFAULT ((0)) FOR [userType]
GO
/****** Object:  Default [DF_Sys_PayGrade_GradeName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_PayGrade] ADD  CONSTRAINT [DF_Sys_PayGrade_GradeName]  DEFAULT ('') FOR [GradeName]
GO
/****** Object:  Default [DF__Post__ParentId__6477ECF3]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Post] ADD  CONSTRAINT [DF__Post__ParentId__6477ECF3]  DEFAULT ((0)) FOR [ParentId]
GO
/****** Object:  Default [DF_Sys_Post_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Post] ADD  CONSTRAINT [DF_Sys_Post_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Sys_Right_RightName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Right] ADD  CONSTRAINT [DF_Sys_Right_RightName]  DEFAULT ('') FOR [RightName]
GO
/****** Object:  Default [DF_Sys_Right_RightDesc]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Right] ADD  CONSTRAINT [DF_Sys_Right_RightDesc]  DEFAULT ('') FOR [RightDesc]
GO
/****** Object:  Default [DF_Sys_Right_Path]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Right] ADD  CONSTRAINT [DF_Sys_Right_Path]  DEFAULT ('') FOR [Path]
GO
/****** Object:  Default [DF_Sys_Right_ShowOrder]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Right] ADD  CONSTRAINT [DF_Sys_Right_ShowOrder]  DEFAULT ((0)) FOR [ShowOrder]
GO
/****** Object:  Default [DF_Sys_Right_ModuleName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Right] ADD  CONSTRAINT [DF_Sys_Right_ModuleName]  DEFAULT ('') FOR [ModuleName]
GO
/****** Object:  Default [DF_Sys_Roles_RoleName]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Roles] ADD  CONSTRAINT [DF_Sys_Roles_RoleName]  DEFAULT ('') FOR [RoleName]
GO
/****** Object:  Default [DF_Sys_Roles_RoleDesc]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Roles] ADD  CONSTRAINT [DF_Sys_Roles_RoleDesc]  DEFAULT ('') FOR [RoleDesc]
GO
/****** Object:  Default [DF_Sys_Roles_IsDefault]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Roles] ADD  CONSTRAINT [DF_Sys_Roles_IsDefault]  DEFAULT ((0)) FOR [IsDefault]
GO
/****** Object:  Default [DF_Sys_Roles_CreateTime]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Roles] ADD  CONSTRAINT [DF_Sys_Roles_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
/****** Object:  Default [DF_Sys_Roles_Status]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Roles] ADD  CONSTRAINT [DF_Sys_Roles_Status]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_Sys_Roles_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_Roles] ADD  CONSTRAINT [DF_Sys_Roles_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Sys_SortGradeCourse_Name]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_SortGradeCourse] ADD  CONSTRAINT [DF_Sys_SortGradeCourse_Name]  DEFAULT ('') FOR [Name]
GO
/****** Object:  Default [DF_Sys_SortGradeCourse_OpenLeavel]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_SortGradeCourse] ADD  CONSTRAINT [DF_Sys_SortGradeCourse_OpenLeavel]  DEFAULT ('') FOR [OpenLeavel]
GO
/****** Object:  Default [DF_Sys_SortGradeCourse_Type]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_SortGradeCourse] ADD  CONSTRAINT [DF_Sys_SortGradeCourse_Type]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  Default [DF_Sys_SortGradeCourse_IsMust]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_SortGradeCourse] ADD  CONSTRAINT [DF_Sys_SortGradeCourse_IsMust]  DEFAULT ((0)) FOR [IsMust]
GO
/****** Object:  Default [DF_Sys_SortGradeCourse_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_SortGradeCourse] ADD  CONSTRAINT [DF_Sys_SortGradeCourse_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Sys_User_Sex]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  CONSTRAINT [DF_Sys_User_Sex]  DEFAULT ((0)) FOR [Sex]
GO
/****** Object:  Default [DF_Sys_User_DeptId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  CONSTRAINT [DF_Sys_User_DeptId]  DEFAULT ((-1)) FOR [DeptId]
GO
/****** Object:  Default [DF_Sys_User_PostId]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  CONSTRAINT [DF_Sys_User_PostId]  DEFAULT ((-1)) FOR [PostId]
GO
/****** Object:  Default [DF_Sys_User_PasswordFailureCount]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  CONSTRAINT [DF_Sys_User_PasswordFailureCount]  DEFAULT ((0)) FOR [PasswordFailureCount]
GO
/****** Object:  Default [DF_Sys_User_IsTeacher]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  CONSTRAINT [DF_Sys_User_IsTeacher]  DEFAULT ((0)) FOR [IsTeacher]
GO
/****** Object:  Default [DF_Sys_User_Status]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  CONSTRAINT [DF_Sys_User_Status]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_Sys_User_IsDelete]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  CONSTRAINT [DF_Sys_User_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Sys_User_TrainMaster]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  CONSTRAINT [DF_Sys_User_TrainMaster]  DEFAULT ((0)) FOR [TrainMaster]
GO
/****** Object:  Default [DF__Sys_User__IsMain__7A3223E8]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT ((0)) FOR [IsMain]
GO
/****** Object:  Default [DF__Sys_User__IsNew__3DE82FB7]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT ((0)) FOR [IsNew]
GO
/****** Object:  Default [DF__Sys_User__IsInte__3EDC53F0]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT ((0)) FOR [IsInternExp]
GO
/****** Object:  Default [DF__Sys_User__IsOurI__3FD07829]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT ((0)) FOR [IsOurIntern]
GO
/****** Object:  Default [DF__Sys_User__Intern__40C49C62]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT ('') FOR [InternDept]
GO
/****** Object:  Default [DF__Sys_User__Number__41B8C09B]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT ('') FOR [NumberID]
GO
/****** Object:  Default [DF__Sys_User__IsChan__42ACE4D4]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT ((0)) FOR [IsChangeNumberId]
GO
/****** Object:  Default [DF__Sys_User__OldTra__43A1090D]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT ('') FOR [OldTrainGrade]
GO
/****** Object:  Default [DF_Sys_UserFingerInfor_Ssn]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_UserFingerInfor] ADD  CONSTRAINT [DF_Sys_UserFingerInfor_Ssn]  DEFAULT ('') FOR [Ssn]
GO
/****** Object:  Default [DF_Sys_UserFingerInfor_Name]    Script Date: 04/18/2014 08:59:00 ******/
ALTER TABLE [dbo].[Sys_UserFingerInfor] ADD  CONSTRAINT [DF_Sys_UserFingerInfor_Name]  DEFAULT ('') FOR [Name]
GO
