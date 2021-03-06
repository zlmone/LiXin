
GO
/****** Object:  Table [dbo].[DepTran_AttendceApprovalRecord]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_AttendceApprovalRecord]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[DepTran_DepartLeaderConfig]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[DepTran_DepartUser]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_YearPlan]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_LinkDepart]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_MonthPlan]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_CourseDept]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Survey_Exampaper]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Survey_ExampaperCategory]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_CourseAdvice]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Survey_Question]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_CourseOrder]    Script Date: 08/23/2013 12:07:29 ******/
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
 CONSTRAINT [PK_DEP_COURSEORDER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dep_Survey_QuestionAnswer]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Survey_DeptSurvey]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Survey_SurveyUser]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[DepTran_CourseAttFile]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_YearPlanCourse]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_MonthPlanCourse]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_NoteSort]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_DepartOpenCourse]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Notes]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Survey_Survey]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_CourseAttFile]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_ClassRoom]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Coursepaper]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[DepTran_Attendce]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Course]    Script Date: 08/23/2013 12:07:29 ******/
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
	[Memo] [nvarchar](1000) NULL,
	[CourseFrom] [int] NULL,
	[StopOrderFlag] [int] NULL,
	[StopDucueFlag] [int] NULL,
	[ReturnTimes] [int] NULL,
	[AfterOrderTimes] [int] NULL,
	[Publishflag] [int] NULL,
	[LastUpdateTime] [datetime] NULL,
	[PreAdviceConfigTime] [int] NULL,
	[AfterEvlutionConfigTime] [int] NULL,
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
/****** Object:  Table [dbo].[DepTran_CourseOrder]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[DepTran_CourseOpen]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_Survey_ReplyAnswer]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  UserDefinedFunction [dbo].[SplitStringBySeparator]    Script Date: 08/23/2013 12:07:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
 **根据指定分隔符实现c#的split功能 add by yxt 
 **@Input-要分割的字符串 
 **@Separator-分隔符，默认是',' 
 **@RemoveEmptyEntries-是否去除空字符串0:保留,1:去除 默认1
*/
CREATE function [dbo].[SplitStringBySeparator]
(
    @Input nvarchar(max),   
    @Separator nvarchar(max)=',', 
    @RemoveEmptyEntries bit=1 )
returns @TABLE table 
(
    [Id] int identity(1,1),
    [Value] nvarchar(max)
) 
as
begin 
    declare @Index int, @Entry nvarchar(max)
    set @Index = charindex(@Separator,@Input)

    while (@Index>0)
    begin
        set @Entry=ltrim(rtrim(substring(@Input, 1, @Index-1)))
        
        if (@RemoveEmptyEntries=0) or (@RemoveEmptyEntries=1 and @Entry<>'')
            begin
                insert into @TABLE([Value]) Values(@Entry)
            end

        set @Input = substring(@Input, @Index+datalength(@Separator)/2, len(@Input))
        set @Index = charindex(@Separator, @Input)
    end
    
    set @Entry=ltrim(rtrim(@Input))
    if (@RemoveEmptyEntries=0) or (@RemoveEmptyEntries=1 and @Entry<>'')
        begin
            insert into @TABLE([Value]) Values(@Entry)
        end

    return
end
/* 主要用于获取课程评估问卷ID
select [Value]=( select top 1 split2.[Value] 
				 from [dbo].[SplitStringBySeparator](split1.[Value], ',', 1) as split2 
				 order by split2.[Value] desc )
from [dbo].[SplitStringBySeparator](cc.SurveyPaperId, ';', 1) as split1
*/
GO
/****** Object:  Table [dbo].[Dep_Attendce]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_ParamConfig]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  Table [dbo].[Dep_CourseResource]    Script Date: 08/23/2013 12:07:29 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetYearDeptIDs]    Script Date: 08/23/2013 12:07:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetYearDeptIDs]
(
	@year INT
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    SELECT @STR=ISNULL(@STR+',','')+CONVERT(NVARCHAR(max),DeptId) FROM Dep_YearPlan WHERE IsDelete=0 AND Year=@year
    SELECT @STR=ISNULL(@STR+',','')+CONVERT(NVARCHAR(max),DeptId) FROM Dep_LinkDepart WHERE YearId IN (SELECT Id FROM Dep_YearPlan WHERE IsDelete=0 AND Year=@year) AND ApprovalFlag IN(0,1)
    IF(@STR IS NULL)
    SET @STR=''
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetLinkDeptIDs]    Script Date: 08/23/2013 12:07:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetLinkDeptIDs]
(
   @yearid INT
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    
    SELECT @STR=ISNULL(@STR+',','')+CONVERT(NVARCHAR(max),DeptId) FROM Dep_LinkDepart WHERE YearId=@yearid
    IF(@STR IS NULL)
    SET @STR=''
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetLinkDeptNameByYearID]    Script Date: 08/23/2013 12:07:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetLinkDeptNameByYearID]
(
   @yearid INT
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    
    SELECT @STR=ISNULL(@STR+',','')+Sys_Department.DeptName FROM Dep_LinkDepart 
    LEFT JOIN Sys_Department ON Sys_Department.DepartmentId=Dep_LinkDepart.DeptId 
    WHERE Dep_LinkDepart.YearId=@yearid AND Dep_LinkDepart.ApprovalFlag IN (0,1)
    IF(@STR IS NULL)
    SET @STR=''
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetDeptParentByDeptID]    Script Date: 08/23/2013 12:07:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetDeptParentByDeptID]
(
 @deptId NVARCHAR(max)    --当前部门
)
RETURNS
@ChildDeptList table (ID NVARCHAR(500), ParentDeptId INT,deptName nvarchar(50),LEVEL INT)
 AS
 BEGIN
  declare @i int   
    set @i = 1  
    -- 插入子部门  
    insert into @ChildDeptList
    SELECT DepartmentId ,ParentDeptId,DeptName,@i from Sys_Department where DepartmentId in (SELECT ID FROM dbo.F_SplitIDs(@deptId)) and IsDelete=0    
   
    while @@rowcount<>0    
    begin   
        set @i = @i + 1    
        insert into @ChildDeptList    
        select  a.DepartmentId,a.ParentDeptId,a.DeptName,@i    
        from Sys_Department as  a 
        join @ChildDeptList as b on a.DepartmentId=b.ParentDeptId and b.Level = @i-1  
        where a.IsDelete=0       
    end   
                         
              RETURN
   END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetDeptChildByDeptID]    Script Date: 08/23/2013 12:07:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetDeptChildByDeptID]
(
 @deptId NVARCHAR(max)    --当前部门
)
RETURNS
@ChildDeptList table (ID NVARCHAR(500),deptName nvarchar(50),LEVEL INT)
 AS
 BEGIN
  declare @i int   
    set @i = 1  
    -- 插入本部门  
    insert into @ChildDeptList
    SELECT  DepartmentId,DeptName,@i from Sys_Department where DepartmentId in (SELECT ID FROM dbo.F_SplitIDs(@deptId)) and IsDelete=0    
   
    while @@rowcount<>0    
    begin   
        set @i = @i + 1    
        insert into @ChildDeptList    
        select  a.DepartmentId,a.DeptName,@i    
        from Sys_Department as  a 
        join @ChildDeptList as b on a.ParentDeptId=b.ID and b.Level = @i-1  
        where a.IsDelete=0       
    end   
                         
              RETURN
   END
GO

/****** Object:  UserDefinedFunction [dbo].[f_GetNewMyCourseTeacherName]    Script Date: 08/23/2013 16:40:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[f_GetNewMyCourseTeacherName]
(
   @id  NVARCHAR(max),
   @userID int
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    SELECT @STR=ISNULL(@STR+',','')+Realname 
    FROM Sys_User 
    WHERE UserId IN (
		select distinct(ta.TeacherId)
		from New_CourseRoomRule as ta,New_CourseOrderDetail as tb 
		where ta.courseid= @id and ta.id=tb.SubCourseID and tb.UserID=@userID
	)
    RETURN @STR
END
GO

/****** Object:  Default [DF_Dep_Attendce_Status]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Attendce] ADD  CONSTRAINT [DF_Dep_Attendce_Status]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_Dep_Attendce_ApprovalFlag]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Attendce] ADD  CONSTRAINT [DF_Dep_Attendce_ApprovalFlag]  DEFAULT ((0)) FOR [ApprovalFlag]
GO
/****** Object:  Default [DF_Dep_Attendce_Reason]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Attendce] ADD  CONSTRAINT [DF_Dep_Attendce_Reason]  DEFAULT ((0)) FOR [Reason]
GO
/****** Object:  Default [DF_Dep_Attendce_DepartSetId]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Attendce] ADD  CONSTRAINT [DF_Dep_Attendce_DepartSetId]  DEFAULT ((0)) FOR [DepartSetId]
GO
/****** Object:  Default [DF_Dep_Course_DeptId]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Course] ADD  CONSTRAINT [DF_Dep_Course_DeptId]  DEFAULT ((0)) FOR [DeptId]
GO
/****** Object:  Default [DF_Dep_Course_OpenUserId]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Course] ADD  CONSTRAINT [DF_Dep_Course_OpenUserId]  DEFAULT ((0)) FOR [OpenUserId]
GO
/****** Object:  Default [DF_Dep_CourseOrder_OrderStatus]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_OrderStatus]  DEFAULT ((0)) FOR [OrderStatus]
GO
/****** Object:  Default [DF_Dep_CourseOrder_LearnStatus]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_LearnStatus]  DEFAULT ((0)) FOR [LearnStatus]
GO
/****** Object:  Default [DF_Dep_CourseOrder_GetScore]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_GetScore]  DEFAULT ((0)) FOR [GetScore]
GO
/****** Object:  Default [DF_Dep_CourseOrder_AttScore]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_AttScore]  DEFAULT ((0)) FOR [AttScore]
GO
/****** Object:  Default [DF_Dep_CourseOrder_EvlutionScore]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_EvlutionScore]  DEFAULT ((0)) FOR [EvlutionScore]
GO
/****** Object:  Default [DF_Dep_CourseOrder_ExamScore]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_ExamScore]  DEFAULT ((0)) FOR [ExamScore]
GO
/****** Object:  Default [DF_Dep_CourseOrder_DepartSetId]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_DepartSetId]  DEFAULT ((0)) FOR [DepartSetId]
GO
/****** Object:  Default [DF_Dep_CourseOrder_IsLeave]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_IsLeave]  DEFAULT ((0)) FOR [IsLeave]
GO
/****** Object:  Default [DF_Dep_CourseOrder_Reason]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_Reason]  DEFAULT ('') FOR [Reason]
GO
/****** Object:  Default [DF_Dep_CourseOrder_ApprovalFlag]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_ApprovalFlag]  DEFAULT ((0)) FOR [ApprovalFlag]
GO
/****** Object:  Default [DF_Dep_CourseOrder_ApprovalUserId]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_ApprovalUserId]  DEFAULT ((0)) FOR [ApprovalUserId]
GO
/****** Object:  Default [DF_Dep_CourseOrder_OrderTimes]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_OrderTimes]  DEFAULT ((0)) FOR [OrderTimes]
GO
/****** Object:  Default [DF_Dep_CourseOrder_IsAppoint]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_CourseOrder] ADD  CONSTRAINT [DF_Dep_CourseOrder_IsAppoint]  DEFAULT ((0)) FOR [IsAppoint]
GO
/****** Object:  Default [DF_Dep_Survey_Exampaper_ExamTitle]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_Exampaper] ADD  CONSTRAINT [DF_Dep_Survey_Exampaper_ExamTitle]  DEFAULT ('') FOR [ExamTitle]
GO
/****** Object:  Default [DF_Dep_Survey_Exampaper_ExamDescription]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_Exampaper] ADD  CONSTRAINT [DF_Dep_Survey_Exampaper_ExamDescription]  DEFAULT ('') FOR [ExamDescription]
GO
/****** Object:  Default [DF_Dep_Survey_Exampaper_Status]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_Exampaper] ADD  CONSTRAINT [DF_Dep_Survey_Exampaper_Status]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_Dep_Survey_ExampaperCategory_CategoryName]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_ExampaperCategory] ADD  CONSTRAINT [DF_Dep_Survey_ExampaperCategory_CategoryName]  DEFAULT ('') FOR [CategoryName]
GO
/****** Object:  Default [DF_Dep_Survey_Question_LinkSortPayGrade]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_Question] ADD  CONSTRAINT [DF_Dep_Survey_Question_LinkSortPayGrade]  DEFAULT ('') FOR [LinkSortPayGrade]
GO
/****** Object:  Default [DF_Dep_Survey_QuestionAnswer_AnswerContent]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_QuestionAnswer] ADD  CONSTRAINT [DF_Dep_Survey_QuestionAnswer_AnswerContent]  DEFAULT ('') FOR [AnswerContent]
GO
/****** Object:  Default [DF_Dep_Survey_ReplyAnswer_SubjectiveAnswer]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Dep_Survey_ReplyAnswer_SubjectiveAnswer]  DEFAULT ('') FOR [SubjectiveAnswer]
GO
/****** Object:  Default [DF_Dep_Survey_ReplyAnswer_ObjectiveAnswer]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Dep_Survey_ReplyAnswer_ObjectiveAnswer]  DEFAULT ('') FOR [ObjectiveAnswer]
GO
/****** Object:  Default [DF_Dep_Survey_ReplyAnswer_Status]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_ReplyAnswer] ADD  CONSTRAINT [DF_Dep_Survey_ReplyAnswer_Status]  DEFAULT ((0)) FOR [Status]
GO
/****** Object:  Default [DF_Dep_Survey_Survey_IsDelete]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[Dep_Survey_Survey] ADD  CONSTRAINT [DF_Dep_Survey_Survey_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_DepTran_Attendce_DepartSetId]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[DepTran_Attendce] ADD  CONSTRAINT [DF_DepTran_Attendce_DepartSetId]  DEFAULT ((0)) FOR [DepartSetId]
GO
/****** Object:  Default [DF_DepTran_CourseOpen_AttFlag]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[DepTran_CourseOpen] ADD  CONSTRAINT [DF_DepTran_CourseOpen_AttFlag]  DEFAULT ((0)) FOR [AttFlag]
GO
/****** Object:  Default [DF_DepTran_CourseOpen_OpenFlag]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[DepTran_CourseOpen] ADD  CONSTRAINT [DF_DepTran_CourseOpen_OpenFlag]  DEFAULT ((0)) FOR [OpenFlag]
GO
/****** Object:  Default [DF__DepTran_C__Appro__7ECCBBD3]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[DepTran_CourseOpen] ADD  DEFAULT ((0)) FOR [ApprovalFlag]
GO
/****** Object:  Default [DF_DepTran_CourseOpen_LimitNumber]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[DepTran_CourseOpen] ADD  CONSTRAINT [DF_DepTran_CourseOpen_LimitNumber]  DEFAULT ((0)) FOR [LimitNumber]
GO
/****** Object:  Default [DF_DepTran_CourseOrder_DepartSetId]    Script Date: 08/23/2013 12:07:29 ******/
ALTER TABLE [dbo].[DepTran_CourseOrder] ADD  CONSTRAINT [DF_DepTran_CourseOrder_DepartSetId]  DEFAULT ((0)) FOR [DepartSetId]
GO
