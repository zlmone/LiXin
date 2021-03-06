
GO
/****** Object:  Table [dbo].[Sys_ClassRoomResource]    Script Date: 07/24/2013 13:40:46 ******/
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
/****** Object:  UserDefinedFunction [dbo].[CGF_FN_Search]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[CGF_FN_Search](
	@inStr NVARCHAR(max),
	@fndStr NVARCHAR(max),
	@split NVARCHAR(max)
) 
RETURNS INT 
AS 
BEGIN 
DECLARE @i INT
Declare @t Table
(
    col NVARCHAR(max)
)
while(charindex(@split,@inStr)<>0)
	begin   
		INSERT @t(col) VALUES(substring(@inStr,1,charindex(@split,@inStr)-1))
		SET @inStr=stuff(@inStr,1,charindex(@split,@inStr),'')
	END
	INSERT @t(col) VALUES (@inStr)
	BEGIN
		IF((SELECT COUNT(*) FROM @t WHERE col IN (@fndStr)) >0)
		RETURN 1
		ELSE
		RETURN 0
	END
	RETURN 0
END
GO
/****** Object:  Table [dbo].[New_Class]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_CourseFiles]    Script Date: 07/24/2013 08:49:45 ******/
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
 CONSTRAINT [PK_New_CourseFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CoursePaper]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_Group]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_GroupUser]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  StoredProcedure [dbo].[proc_DeleteByNewClassId]    Script Date: 07/24/2013 08:50:02 ******/
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
GO
/****** Object:  StoredProcedure [dbo].[proc_GetOverallEval]    Script Date: 07/24/2013 08:50:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC  [dbo].[proc_GetOverallEval]
(
   @userID INT,--人员ID
   @type INT --类型 0 综合 1 考勤 2课后评估 3课堂表现 4在线测试 5课堂纪律
)   
AS
BEGIN
    DECLARE @SumScore FLOAT
END
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetOverallEval]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION  [dbo].[F_GetOverallEval]
(
   @userID INT,--人员ID
   @type INT --类型 0 综合 1 考勤 2课后评估 3课堂表现 4在线测试 5课堂纪律
)   
RETURNS  FLOAT AS 
BEGIN
   --定义一系列分数
    DECLARE @sumScore FLOAT
    ,@attendceScore   FLOAT   --考勤
    , @surveyScore   FLOAT   --课后评估
    , @expressScore   FLOAT   --课堂表现
    , @examScore   FLOAT   --在线测试 
    , @disciplineScore   FLOAT   --课堂纪律
    
    
    RETURN @SumScore
END
GO
/****** Object:  Table [dbo].[New_Course]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_CourseRoomRule]    Script Date: 07/24/2013 08:49:45 ******/
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
 CONSTRAINT [PK_New_CourseRoomRule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_Attendce]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_MidAttendce]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_CourseOrderDetail]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_ClassRoom]    Script Date: 07/24/2013 08:49:45 ******/
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
	[DisSeat] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[New_CourseOrder]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_CourseAdvice]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_CpaLearnStatus]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_LearnVideoInfor]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  Table [dbo].[New_UserExamScore]    Script Date: 07/24/2013 08:49:45 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetClassName]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetClassName]
(
   @ids  NVARCHAR(max)
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    
    SELECT @STR=ISNULL(@STR+',','')+ClassName FROM  New_Class WHERE Id IN (SELECT ID FROM dbo.F_SplitIDs(@ids))
    IF(@STR IS NULL)
    SET @STR=''
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetTeacherName]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetTeacherName]
(
   @ids  NVARCHAR(max)
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    SELECT @STR=ISNULL(@STR+',','')+Realname FROM Sys_User WHERE UserId IN (SELECT ID FROM dbo.F_SplitIDs(@ids))
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetCourseTeacherName]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetCourseTeacherName]
(
   @id  NVARCHAR(max),
   @type NVARCHAR(20) --0:集中授课；1：分组带教；0,1:两者
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    SELECT @STR=ISNULL(@STR+',','')+Realname 
    FROM Sys_User 
    WHERE UserId IN (
		SELECT distinct (TeacherId) 
		FROM New_CourseRoomRule 
		WHERE CourseId=@id AND Type in (select ID from dbo.F_SplitIDs(@type))
	)
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetRoomName]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetRoomName]
(
   @id  NVARCHAR(max),
   @type NVARCHAR(20) --0:集中授课；1：分组带教；0,1:两者
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    SELECT @STR=ISNULL(@STR+',','')+RoomName FROM New_ClassRoom WHERE Id IN (SELECT RoomId FROM New_CourseRoomRule WHERE CourseId=@id AND Type in (select ID from dbo.F_SplitIDs(@type)))
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[F_DepTranAttdenceStatus]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_DepTranAttdenceStatus]
(
  @CourseId INT,
  @depIds  NVARCHAR(max)
)
RETURNS INT
BEGIN
	DECLARE @sum INT --报名人数
	,@count INT --实际到达人数
	,@not int --未审批人数
	,@yes int --审批通过人数
	,@no int --审批拒绝人数
	,@result int --返回值

	SET @result=2


	SELECT @sum=COUNT(1) FROM DepTran_CourseOrder WHERE CourseId=@CourseId AND DepartSetId IN (SELECT ID FROM dbo.F_SplitIDs(@depIds))
	SELECT @count=COUNT(1) FROM DepTran_Attendce WHERE CourseId=@CourseId AND DepartSetId IN (SELECT ID FROM dbo.F_SplitIDs(@depIds))
	SELECT @not=COUNT(1) FROM DepTran_Attendce WHERE CourseId=@CourseId AND ApprovalFlag=0 AND DepartSetId IN (SELECT ID FROM dbo.F_SplitIDs(@depIds))
	SELECT @yes=COUNT(1) FROM DepTran_Attendce WHERE CourseId=@CourseId AND ApprovalFlag=1 AND DepartSetId IN (SELECT ID FROM dbo.F_SplitIDs(@depIds))
	SELECT @no=COUNT(1) FROM DepTran_Attendce WHERE CourseId=@CourseId AND ApprovalFlag=2 AND DepartSetId IN (SELECT ID FROM dbo.F_SplitIDs(@depIds))
	
	IF(@sum=@count)
	BEGIN
	    IF(@sum=@not)
	       SET @result=0
	    ELSE IF(@sum=@yes)
	       SET @result=1
	    ELSE IF(@sum=@no)
	       SET @result=2
	END
	ELSE IF(@sum=0)
	   SET @result=0
	
	RETURN @result
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetRoomNameTest]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetRoomNameTest]
(
   @id  int,
   @teacherid int
   --@type int
)
RETURNS NVARCHAR(max) AS
BEGIN
    --DECLARE @STR VARCHAR(max)
    --SELECT @STR=ISNULL(@STR+',','')+RoomName FROM New_ClassRoom WHERE Id IN (SELECT RoomId FROM New_CourseRoomRule WHERE CourseId=@id AND Type=@type)
    --RETURN @STR
     declare @RoomNameString varchar(4000)
  	--SELECT @RoomNameString =isnull(@RoomNameString+', ','')+Rtrim(Ltrim(isnull(RoomName,''))) from  New_ClassRoom
  	SELECT @RoomNameString=ISNULL(@RoomNameString+',','')+RoomName from  New_ClassRoom
	Where Id in (
	--dbo.F_SplitIDs(
					--select * from New_ClassRoom where Id in
					--(
					  select RoomId from New_CourseRoomRule where courseid=@id and teacherid=@teacherid
					  
					--)
	--			)
	)
	RETURN @RoomNameString
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetRoomNameByCourseIdAndUseriId]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[f_GetRoomNameByCourseIdAndUseriId]
(
   @CourseId  int,
   @UseriId int
   --@type int
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    SELECT @STR=ISNULL(@STR+',','')+RoomName FROM New_ClassRoom WHERE Id IN (SELECT RoomId FROM New_CourseRoomRule WHERE Id in
		(
			select  SubCourseID from New_CourseOrderDetail  
		 where  UserId=@UseriId and courseid=@CourseId    
		)
    )
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetRoomNameByNew_CourseRoomRuleId]    Script Date: 07/24/2013 08:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[f_GetRoomNameByNew_CourseRoomRuleId]
(
   @id  NVARCHAR(max)
   --@type int
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    SELECT @STR=ISNULL(@STR+',','')+RoomName FROM New_ClassRoom WHERE Id IN (SELECT RoomId FROM New_CourseRoomRule WHERE Id=@id)
    RETURN @STR
END
GO
/****** Object:  View [dbo].[view_CourseOrderInfo]    Script Date: 07/24/2013 08:49:46 ******/
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
/****** Object:  Default [DF__New_Class__RoomN__2DF1BF10]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ('') FOR [RoomName]
GO
/****** Object:  Default [DF__New_ClassR__Memo__2EE5E349]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ('') FOR [Memo]
GO
/****** Object:  Default [DF__New_Class__RoomN__2FDA0782]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [RoomNumber]
GO
/****** Object:  Default [DF__New_Class__Colum__30CE2BBB]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [ColumnNumber]
GO
/****** Object:  Default [DF__New_Class__RowNu__31C24FF4]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [RowNumber]
GO
/****** Object:  Default [DF__New_Class__IsDel__32B6742D]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
/****** Object:  Default [DF_New_ClassRoom_DisSeat]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_ClassRoom] ADD  CONSTRAINT [DF_New_ClassRoom_DisSeat]  DEFAULT ('') FOR [DisSeat]
GO
/****** Object:  Default [DF_New_Course_LastUpdateTime]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_Course] ADD  CONSTRAINT [DF_New_Course_LastUpdateTime]  DEFAULT (getdate()) FOR [LastUpdateTime]
GO
/****** Object:  Default [DF_New_Course_PublicFlag]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_Course] ADD  CONSTRAINT [DF_New_Course_PublicFlag]  DEFAULT ((0)) FOR [PublicFlag]
GO
/****** Object:  Default [DF_New_Course_VideoLowLength]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_Course] ADD  CONSTRAINT [DF_New_Course_VideoLowLength]  DEFAULT ((0)) FOR [VideoLowLength]
GO
/****** Object:  Default [DF_New_CourseOrderDetail_IsLeave]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_CourseOrderDetail] ADD  CONSTRAINT [DF_New_CourseOrderDetail_IsLeave]  DEFAULT ((0)) FOR [IsLeave]
GO
/****** Object:  Default [DF_New_CourseRoomRule_GroupName]    Script Date: 07/24/2013 08:49:45 ******/
ALTER TABLE [dbo].[New_CourseRoomRule] ADD  CONSTRAINT [DF_New_CourseRoomRule_GroupName]  DEFAULT ('') FOR [GroupName]
GO

/*用户表新增字段*/
ALTER TABLE Sys_User ADD IsNew int DEFAULT (0);
GO
ALTER TABLE Sys_User ADD IsInternExp int DEFAULT (0);
GO
ALTER TABLE Sys_User ADD IsOurIntern int DEFAULT (0);
GO
ALTER TABLE Sys_User ADD InternDept nvarchar(200) DEFAULT ('');
GO
ALTER TABLE Sys_User ADD NumberID nvarchar(50) DEFAULT ('');
GO
ALTER TABLE Sys_User ADD IsChangeNumberId int DEFAULT (0);
GO
ALTER TABLE Sys_User ADD OldTrainGrade nvarchar(50) DEFAULT ('');
GO

/*调查回复表新增字段*/
ALTER TABLE Survey_ReplyAnswer ADD TeacherID int DEFAULT (0);
GO
ALTER TABLE Survey_ReplyAnswer ADD CourseRoomRuleId int DEFAULT (0);
GO

/*调查问题表新增字段*/
ALTER TABLE Survey_Question ADD ObjectType int DEFAULT (0);
GO


/*权限表新增配置*/
set identity_insert Sys_Right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (178, '班级维护', 0, '班级维护', 'NewClassManage/NewClassManage', 179, 2, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (179, '新进员工培训管理', 0, '新进员工培训管理', 'NewUserManage', 1, 11, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (180, '新进员工维护', 0, '新进员工维护', 'NewUserManage/NewUserManage', 179, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (181, '综合评分管理', 0, '综合评分管理', 'NewAllScoreManager/NewAllScoreManagerList', 179, 6, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (182, '综合评分', 0, '综合评分', 'NewAllScoreManager/NewAllScore', 181, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (183, '课程开设', 0, '课程开设', 'NewCourseManage/NewCourseManageList', 179, 4, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (184, 'NewAttendceManage', 0, '考勤管理', 'NewAttendceManage/NewAttendceList', 179, 5, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (185, '讲师评价信息', 0, '学员对讲师的评价信息', 'NewMyEvaluate/NewEvaluateUserToTeacher', 1, 4, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (186, '培训成绩录入', 0, '新员工培训成绩录入', 'NewUserManage/NewInputScore', 179, 9, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (187, '学员综合评分查询', 0, '学员综合评分查询', 'NewQueryStatistics/StudentQueryStatisticsList', 179, 7, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (188, '教室维护', 0, '教室维护', 'NewClassRoom/ClassRoomManage', 179, 3, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (189, '编辑新进员工教室', 1, '编辑新进员工教室', 'NewClassRoom/ClassRoomEdit', 188, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (190, '新进员工教室详情', 1, '新进员工教室详情', 'NewClassRoom/ClassRoomDetial', 188, 2, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (191, '班级人员调整', 1, '班级人员调整', 'NewClassManage/ClassPersonManage', 178, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (192, '我的综合评定', 0, '我的综合评定', 'NewQueryStatistics/MyQueryStatistics', 1, 2, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (193, '编辑新进员工混合培训课程', 1, '编辑新进员工混合培训课程', 'NewCourseManage/EditNewCourseTogether', 183, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (194, '编辑新进员工集中授课课程', 1, '编辑新进员工集中授课课程', 'NewCourseManage/CourseSeatSistribute', 183, 2, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (195, '新员工详情', 0, '新员工详情', 'NewUserManage/UserDetail', 180, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (196, '编辑新进员工分组带教课程', 1, '编辑新进员工分组带教课程', 'NewCourseManage/GroupCourseSeatSistribute', 183, 3, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (197, '编辑新进员工自学课程', 1, '编辑新进员工自学课程', 'NewCourseManage/EditNewCourseVideo', 183, 4, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (198, '新进员工自学课程详情', 1, '新进员工自学课程详情', 'NewCourseManage/NewCourseVideoDetial', 183, 5, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (199, '新进员工混合培训课程详情', 1, '新进员工混合培训课程详情', 'NewCourseManage/NewCourseTogetherDetial', 183, 6, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (200, '个人综合评定', 1, '个人综合评定', 'NewQueryStatistics/StudentQueryStatistics', 187, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (201, '新员工考勤管理详情', 1, '新员工考勤管理详情', 'NewAttendceManage/NewAttendceDetail', 184, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (202, 'MyNewCourse', 0, '我的新进培训课程列表', 'NewMyCourse/CourseList', 1, 3, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (203, '我的混合培训课程详情', 1, '我的混合培训课程详情', 'NewMyCourse/MyCourse', 202, 1, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (204, 'NewMyTeachCourse', 0, '我的新进授课课程', 'NewTeacherCourse/CourseList', 1, 5, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (205, '我的新进课程授课详情', 1, '我的新进课程授课详情', 'NewTeacherCourse/TeacherCourse', 204, 1, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (206, '我的自学课程', 0, '我的新进自学课程', 'NewMyCourse/StudyCourseList', 1, 6, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (207, '我的学时及考勤获取', 0, '我的学时及考勤获取', 'DepTranMyCourse/MyDepTranScore', 211, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (208, '部门/分所员工学时及考勤获取', 0, '部门/分所员工学时及考勤获取', 'DepTranMyCourse/DepUserScore', 211, 2, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (209, 'NewMyTrainIndex', 0, '新进员工培训首页', 'Home/NewMyTrainIndex', 1, 1, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (210, '个人学时详情', 1, '个人学时详情', 'DepTranMyCourse/DepUserScoreDetails', 208, NULL, 'SystemManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (211, '部门分所课程转播', 0, '部门分所课程转播', '#', 1, 12, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (212, '部门分所考勤管理', 0, '', 'DepTranAttendce/DepTranAttendceList', 211, NULL, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (213, '我的班组', 0, '我的班组', 'NewClassManage/MyClassRoom', 1, 6, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (214, '课后评估统计', 0, '课后评估统计', 'NewMyEvaluate/NewEvaluateAllUserToTeacher', 179, 10, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (215, 'NewTeacherEvlutionDetail', 1, '讲师详情', 'NewMyEvaluate/NewEvaluateUserToTeacherDetail', 185, 1, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (216, 'NewTeacherEvlutionManageDetail', 1, '评价详情', 'NewMyEvaluate/NewEvaluateAllUserToTeacherDetail', 214, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (217, 'MyBroadcastList', 0, '转播课程', 'DepTrainMyBroadcastCourse/MyBroadcastList', 211, 5, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (218, 'MyBroadcastCourseDetail', 1, '课程详情', 'DepTrainMyBroadcastCourse/BroadcastCourseMain', 217, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (219, '部门人员管理', 0, '部门人员管理', 'DepTranDepartSetting/DepartSettingManage', 211, 3, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (220, '转播课程管理', 0, '转播课程管理', 'DeptCourseManage/DeptCourseManage', 211, 6, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (221, 'DepTranCourseDetail', 1, '课程详情', 'DeptCourseManage/DeptCourseDetail', 220, 1, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (222, 'MySurveyList', 0, '我的需求（新员工）', 'Survey/MySurveyList', 1, 7, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (223, 'NewDoSurvey', 1, '参与调查', 'Survey/NewDoSurvey', 222, 1, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (224, 'NewBrowseSurveyResult', 1, '查看调查详情', 'Survey/NewBrowseSurveyResult', 222, 2, '')
GO

set identity_insert Sys_Right off

go

/*配置表新增字段*/
ALTER TABLE Sys_ParamConfig ADD userType int DEFAULT (0);
GO

/*配置表新增配置*/
set identity_insert Sys_ParamConfig on

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (33, '综合评分—总分和奖励', 34, '100,20;20,20,40,20;0', '2013-07-05 16:44:09', NULL)
GO

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (34, '综合评分—考勤', 35, '0,1,3,1;1,4,-1,3;2,1,3,2;2,4,-1,1', '2013-07-05 16:47:48', NULL)
GO

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (35, '综合评分—课后评估', 36, '1,3,4;4,-1,2', '2013-07-05 16:48:09', NULL)
GO

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (36, '分所培训设置', 37, '0', '1905-06-22', NULL)
GO

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (37, '综合评分—考勤每次扣分多少', 38, '', '1905-06-22', NULL)
GO

set identity_insert Sys_ParamConfig off
go

