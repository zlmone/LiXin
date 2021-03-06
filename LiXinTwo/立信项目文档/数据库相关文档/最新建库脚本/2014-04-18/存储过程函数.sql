
/****** Object:  UserDefinedFunction [dbo].[SplitStringBySeparator]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[CGF_FN_Search]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[F_SplitIDs]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_SplitIDs]
(
 @IDList nvarchar(4000)
)
RETURNS
@ParsedList table (ID NVARCHAR(500))
AS
BEGIN
            DECLARE @ID nvarchar(10), @Pos INT

                  SET @IDList = LTRIM(RTRIM(@IDList))+ ','

            SET @Pos = CHARINDEX(',', @IDList, 1)


            IF REPLACE(@IDList, ',', '') <> ''
            BEGIN
                        WHILE @Pos > 0
                        BEGIN
                                    SET @ID = REPLACE(LTRIM(RTRIM(LEFT(@IDList, @Pos - 1))), '''', '')
                                    IF @ID <> ''
                                    BEGIN
                                                INSERT INTO @ParsedList (ID)
                                                VALUES (@ID) --Use Appropriate conversion
                                    END
                                    SET @IDList = RIGHT(@IDList, LEN(@IDList) - @Pos)
                                    SET @Pos = CHARINDEX(',', @IDList, 1)
                        END
            END

            RETURN
END
GO
/****** Object:  StoredProcedure [dbo].[proc_GetOverallEval]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[GetSplitOfIndex]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*********************************************************************************
*函数名称 : GetSplitOfIndex
*功能描述 : 返回分割后的字符数组的长度
*           移植于产品化 
*作    者 : guohl 
*创建时间 : 20130128
***********************************************************************************/
create function [dbo].[GetSplitOfIndex]
(
  @String nvarchar(max),  --要分割的字符串
  @split nvarchar(10),  --分隔符号
  @index int --取第几个元素
)
returns nvarchar(1024)
 as
 begin
  declare @location int
  declare @start int
  declare @next int
  declare @seed int
 
  set @String=ltrim(rtrim(@String))
  set @start=1
  set @next=1
  set @seed=len(@split)
  
  set @location=charindex(@split,@String)
  while @location<>0 and @index>@next
  begin
    set @start=@location+@seed
    set @location=charindex(@split,@String,@start)
    set @next=@next+1
  end
  if @location =0 select @location =len(@String)+1 
  
  return substring(@String,@start,@location-@start)
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetSplitLength]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*********************************************************************************
*函数名称 : GetSplitLength
*功能描述 : 返回分割后的字符数组的长度
*           移植于产品化 
*作    者 : guohl 
*创建时间 : 20130128
***********************************************************************************/
create function [dbo].[GetSplitLength]
(
  @String nvarchar(max),  --要分割的字符串
  @Split nvarchar(10)  --分隔符号
)
returns int
as
begin
  declare @location int
  declare @start int
  declare @length int
 
  set @String=ltrim(rtrim(@String))
  set @location=charindex(@split,@String)
  set @length=1
  while @location<>0
  begin
    set @start=@location+1
    set @location=charindex(@split,@String,@start)
    set @length=@length+1
  end
  return @length
end
GO
/****** Object:  StoredProcedure [dbo].[proc_DeleteByNewClassId]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[F_GetOverallEval]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetNewMyCourseTeacherName]    Script Date: 04/18/2014 09:00:48 ******/
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
		select (ta.TeacherId)
		from New_CourseRoomRule as ta,New_CourseOrderDetail as tb 
		where ta.courseid= @id and ta.id=tb.SubCourseID and tb.UserID=@userID
	)
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetLinkDeptNameByYearID]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetLinkDeptIDs]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetDeptParentByDeptID]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[F_GetDeptIDByCourse]    Script Date: 04/18/2014 09:00:48 ******/
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
		 	--被联合
		 	IF((SELECT COUNT(*) FROM Dep_YearPlan WHERE DeptId=@deptID AND Year=@year AND IsDelete=0 AND PublishFlag=1)=0)
		 	BEGIN
				 SELECT @deptidstr=@deptidstr+','+ltrim(str(DeptId)) FROM Dep_LinkDepart WHERE YearId IN (
		                      SELECT dl.YearId FROM Dep_LinkDepart dl
				               LEFT JOIN   Dep_YearPlan   dy  ON dl.YearId=dy.Id
				              WHERE Year=2013 AND dl.DeptId=@deptID   and dl.ApprovalFlag=1
	                ) AND DeptId!=@deptID AND dbo.Dep_LinkDepart.ApprovalFlag=1

	                
				   
				    SELECT @deptidstr=@deptidstr+','+ltrim(str(dy.DeptId)) FROM Dep_LinkDepart dl
					LEFT JOIN   Dep_YearPlan   dy  ON dl.YearId=dy.Id
				   WHERE Year=@year AND dl.DeptId=@deptID AND dl.ApprovalFlag=1
			 END
			--联合
			 ELSE
			       SELECT @deptidstr=@deptidstr+','+ltrim(str(dl.DeptId)) FROM Dep_LinkDepart dl
					LEFT JOIN   Dep_YearPlan   dy  ON dl.YearId=dy.Id
				   WHERE Year=@year AND dy.DeptId=@deptID AND dl.ApprovalFlag=1
	   
		
		RETURN    @deptidstr
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetDeptChildByDeptID]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetDepCourseOpenObject]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[f_GetDepCourseOpenObject]
(
  @courseID INT 
)
RETURNS NVARCHAR(max) AS
 BEGIN	
	DECLARE @OpenFlag INT,@OpenDepartObject NVARCHAR(500), @OpenLevel   NVARCHAR(50),@OpenLevel1   NVARCHAR(50)
	,@OpenDepart NVARCHAR(MAX),@openObject NVARCHAR(max) ,@openObject1 NVARCHAR(max)
  
  
	SET  @OpenDepart='' 
	SET  @openObject=''
	SET @OpenLevel=''
	SET @OpenLevel1=''
	SELECT @OpenFlag=OpenFlag , @OpenDepartObject=OpenDepartObject,@OpenLevel= OpenLevel
	FROM Dep_Course WHERE Id=@courseID
	
	IF(@OpenFlag=0)
    	BEGIN
	        SET @openObject=  @OpenLevel
    	END
   
	  
	iF(@OpenFlag=2)
    	BEGIN

              SELECT @openObject=@openObject+','+DeptName FROM (
             SELECT DISTINCT DeptName FROM Sys_User
             	WHERE DeptId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenDepartObject))
             	) t
             	
             	SET   @openObject=substring(@openObject,2,len(@openObject))
             	--SET   @OpenLevel=''
             	   SELECT @OpenLevel1=@OpenLevel1+','+TrainGrade FROM (
                SELECT DISTINCT TrainGrade FROM Sys_User
             	WHERE  TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))  
             	  )r ORDER BY  TrainGrade
             	 SET   @OpenLevel1=substring(@OpenLevel1,2,len(@OpenLevel1))
             	SET   @openObject=@openObject+':'+@OpenLevel1	
             	
          
	    END
	  
    RETURN @openObject
  END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetCourseTeacherName]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetClassName]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[F_GetCheckDeptID]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_GetCheckDeptID]
(
   @type INT 
)

 RETURNS  @DeptList table (ID INT)    AS
 BEGIN
       DECLARE @value NVARCHAR(max)
       SELECT @value=ConfigValue FROM Sys_ParamConfig WHERE ConfigType=54
      INSERT INTO    @DeptList
          SELECT ID FROM dbo.F_SplitIDs(@value)
          RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[F_DepTranAttdenceStatus]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[F_GetTogtherSigle]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_GetTogtherSigle]
(
 @CourseID INT
)
RETURNS
@ParsedList table (UserId INT,DeptId int)
AS
 BEGIN
	DECLARE @IsOpenSub INT,@OpenLevel NVARCHAR(500),@OpenFlag INT, @OpenDepartObject NVARCHAR(500), @OpenGroupObject  NVARCHAR(50)
	,@sumCount  INT,@Id INT,@OpenPerson NVARCHAR(500),@IsOrder INT;
   
	
		SELECT @Id=Id,@OpenLevel=OpenLevel,@OpenFlag=OpenFlag,@OpenDepartObject=OpenDepartObject,
		@OpenGroupObject=OpenGroupObject,@OpenPerson=OpenPerson,@IsOrder=IsOrder FROM dbo.Co_Course cc
		WHERE cc.IsDelete=0 AND  cc.Way=1 AND cc.CourseFrom=2
	    AND Id=@CourseID
         IF(@IsOrder=2)
         BEGIN
           INSERT into   @ParsedList
             SELECT  UserId,DeptId FROM Sys_User
		        where TrainGrade not IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        and IsDelete=0 AND Status=0
         END
         ELSE
         BEGIN
           iF(@OpenFlag=0)  
		     INSERT into   @ParsedList
		        SELECT  UserId,DeptId FROM Sys_User WHERE (TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		         OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson)))
		        AND  IsDelete=0 AND Status=0  
		     
		     ELSE IF (@OpenFlag=1 )
		       INSERT into   @ParsedList
		          SELECT   UserId,DeptId FROM Sys_User WHERE ((TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		          AND UserId IN (select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(@OpenGroupObject))))
		           OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson)))
		          AND  IsDelete=0 AND Status=0  
		     
		     ELSE IF ( @OpenFlag=2 )               
		       INSERT into   @ParsedList
		        SELECT  UserId,DeptId FROM Sys_User WHERE ((TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        AND DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject)))
		         OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson)))
		        AND  IsDelete=0 AND Status=0    
		     
		     ELSE IF ( @OpenFlag=3  ) 
		       INSERT into   @ParsedList
		         SELECT  UserId,DeptId FROM Sys_User WHERE ((TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		         AND (
		               UserId IN (select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(@OpenGroupObject))) OR
		                DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject))
		        ))  OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson))) AND  IsDelete=0 AND Status=0 
		    END
     RETURN
 END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetTeacherName]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetRoomNameTest]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetRoomNameByNew_CourseRoomRuleId]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetRoomNameByCourseIdAndUseriId]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetRoomName]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetQuestionAnswer]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[f_GetQuestionAnswer]
(
   @ids  NVARCHAR(max)
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    
    SELECT @STR=ISNULL(@STR+'--','')+AnswerContent FROM  Survey_QuestionAnswer WHERE AnswerId IN (SELECT ID FROM dbo.F_SplitIDs(@ids))
    IF(@STR IS NULL)
    SET @STR=''
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetYearDeptIDs]    Script Date: 04/18/2014 09:00:48 ******/
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
/****** Object:  UserDefinedFunction [dbo].[f_GetVedioUserList]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetVedioUserList]
(
   @id  NVARCHAR(max),
   @DepartmentId NVARCHAR(max)
)
RETURNS NVARCHAR(max) AS
BEGIN
    DECLARE @STR VARCHAR(max)
    SELECT @STR=ISNULL(@STR+',','')+ltrim(Str(Cl_CpaLearnStatus.UserID)) FROM Cl_CpaLearnStatus 
		left join sys_user on Cl_CpaLearnStatus.UserID=sys_user.userid
    WHERE 
		Cl_CpaLearnStatus.courseid =@id and CpaFlag=0 and sys_user.DeptId =@DepartmentId
    RETURN @STR
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetChildResourceTypeIds]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--由父级分类ID获取所有子集分类ID递归函数 （返回集合包含父级ID）   
Create function [dbo].[GetChildResourceTypeIds](@TypeID int)    
returns @t table(ResourceTypeID int,ParentID int,Level int)    
as   
begin   
    declare @i int   
    set @i = 1    
    insert into @t select @TypeID,@TypeID,0 --当前级，本级，如果不要的话可以注释掉或再加个参数来选择操作    
    insert into @t select ResourceTypeID,ParentID,@i from Re_ResourceType where ParentID = @TypeID and IsDelete=0    
   
    while @@rowcount<>0    
    begin   
        set @i = @i + 1    
        insert into @t    
        select  a.ResourceTypeID,a.ParentID,@i    
        from Re_ResourceType as  a 
        join @t as b on a.ParentID=b.ResourceTypeID and b.Level = @i-1  
        where a.IsDelete=0       
    end   
    return   
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetSystemTree]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*********************************************************************************
*函数名称 : GetSystemTree
*功能描述 : 返回体系的结构，形如 级别1>级别2>级别3 
*参数：体系的ID
*作    者 : huihh 
*创建时间 : 20130220
***********************************************************************************/
CREATE FUNCTION [dbo].[GetSystemTree](@id INT)
RETURNS NVARCHAR(4000)
AS
BEGIN
DECLARE @parentID INT,@SystemName NVARCHAR(4000)
SET @SystemName=''
SELECT @SystemName=CourseSystemName,@parentID=parentID FROM Co_CourseSystem
WHERE Id=@id

WHILE (@parentID>0)
BEGIN
    SELECT @SystemName=CourseSystemName+'>'+@SystemName,@parentID=parentID  FROM Co_CourseSystem
    WHERE Id=@parentID
END
return @SystemName
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetRoleNameByUserID]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*********************************************************************************
*函数名称 : GetRoleNameByUserID
*功能描述 : 根据UserID返回角色名称，用“，”隔开
*作    者 : guohl
*创建时间 : 20130125
***********************************************************************************/
Create FUNCTION [dbo].[GetRoleNameByUserID]
(@UserID int)
RETURNS NVARCHAR(MAX)
AS 
    BEGIN 

        DECLARE @str NVARCHAR(MAX) 
        SET @str = '' 
        --SELECT  @str = @str + ',' + r.RoleName
        --FROM    dbo.Sys_UserRole ur
        --        LEFT JOIN dbo.Sys_Roles r ON r.RoleId = ur.RoleId 
        --WHERE  r.IsDelete=0 and (( ur.UserID = @UserID and IsDefault=0 ) or IsDefault=1)
        
        SELECT  @str = @str + ',' + r.RoleName
        FROM    dbo.Sys_Roles r where (r.RoleId in (
			Select RoleId from Sys_UserRole where userid = @UserID
        ) or r.isdefault = 1 )and r.isdelete = 0 
        
        if(@str='')
         begin
          SELECT  @str=','+RoleName from Sys_Roles where IsDelete=0 and IsDefault=1
         end   
        IF ( LEN(@str) > 1 ) 
            BEGIN
                SET @str = RIGHT(@str, LEN(@str) - 1) 
            END
        RETURN(@str)
    END
GO
/****** Object:  UserDefinedFunction [dbo].[GetRealNameByUserID]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[GetRealNameByUserID]
(
@userID int
)
returns nvarchar(50)
as
begin
declare @realName nvarchar(50)
set @realName=''
select @realName= realName from dbo.Sys_User where userID=@userID
return @realName
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetDeptTwoPathByDeptID]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[GetDeptTwoPathByDeptID]
(
 @deptId int    --当前部门
)
returns varchar(1000) as
begin 
declare @parnetID INT,@parnet INT,@deptName varchar(500),@twoName varchar(500),@lastName varchar(500)
,@twodeptName varchar(500),@path NVARCHAR(500)
set @lastName=''
SET @twoName=''
set @parnetID=@deptId
SET @path=''
while(@parnetID>1)
BEGIN
 
  select  @parnetID=parentdeptid,@deptName=deptName from sys_department where departmentid=@parnetID
   SELECT @parnet=ParentDeptId,@twodeptName=deptName FROM sys_department where departmentid=@parnetID
     SET  @path= @deptName+'/'+@path
  IF(@parnet=1)
     SET  @twoName=@deptName+'/'+@twoName
  IF(@parnet>1)
      SET  @lastName=@deptName+'/'+@lastName
  
   
end
if(len(@path)>0)
BEGIN
     IF( charindex('上海',@path)>0 )
    BEGIN
        IF(len(@lastName)>0)
		set  @lastName=LEFT(@lastName,charindex('/',@lastName,1)-1)
     END
    ELSE
     BEGIN
      IF(len(@twoName)>0)
       	set  @lastName= substring (@twoName,1,len(@twoName)-1)   
     END   
END
    IF(len(@lastName)=0)
    BEGIN
       set @lastName=@path
    END
     RETURN @lastName
 END                             
--SELECT charindex('上海','上海浩浩浩浩')
GO
/****** Object:  UserDefinedFunction [dbo].[GetDeptPathByDeptID]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[GetDeptPathByDeptID]
(
 @deptId int    --当前部门
)
returns varchar(1000) as
begin 
declare @parnetID int
declare @deptName varchar(500)
declare @lastName varchar(500)

set @lastName=''
set @parnetID=@deptId
while(@parnetID>1)
begin
 select  @parnetID=parentdeptid,@deptName=deptName from sys_department where departmentid=@parnetID
set  @lastName=@deptName+'/'+@lastName
end
if(len(@lastName)>0)
		set  @lastName= substring (@lastName,1,len(@lastName)-1)
return @lastName
end
GO
/****** Object:  UserDefinedFunction [dbo].[F_NewIsAppStatus]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_NewIsAppStatus]
(
  @courseID INT ,
  @userID INT ,
  @type INT
)
RETURNS INT AS
BEGIN
    --DECLARE @type INT=-1
    DECLARE @number INT=0
     DECLARE @count INT=0
    DECLARE @isApp INT=0
    DECLARE @sumcount INT

  
    IF(@type=3)  --补预定
     BEGIN           
      
	      IF((SELECT count(*) FROM Cl_MakeUpOrder WHERE CourseId=@courseID AND UserId=@userID)>0)
	       BEGIN	
	            SELECT @sumcount=ConfigValue FROM Sys_ParamConfig
				WHERE ConfigType=20	
				
--		         SELECT @number=number,@count=count(1) FROM View_OrderTimes
--		         WHERE CourseId=@courseID AND UserId=@userID
				SELECT @number=number,@count=count(1) FROM (
				SELECT row_number()OVER(PARTITION BY UserId ORDER BY ApprovalDateTime ASC ) number ,CourseId FROM(
				SELECT CourseId, UserId,  ApprovalDateTime  FROM Cl_MakeUpOrder
				LEFT JOIN Co_Course cc ON cc.Id=Cl_MakeUpOrder.CourseId
				WHERE ( ApprovalFlag=1 AND  ApprovalDateTime<=ApprovalLimitTime) 
			    AND CourseId=@courseID AND UserId=@userID
				UNION
				SELECT CourseId, UserId,  ApprovalDateTime  FROM Cl_TimeOutOrder
				WHERE ApprovalFlag=1 
				AND CourseId=@courseID AND UserId=@userID
				) t)result GROUP BY number
				

		          IF(@count>0)
		          BEGIN
		               IF(@number<=CAST(@sumcount AS INT))
		                    SET @isApp=1
		              ELSE
		                  SET @isApp=2
		          END
		             else
		             SET @isApp=0
		     END 
		     ELSE
		        SET @isApp=0
      END
	    ELSE  
	       SET @isApp=1
       RETURN @isApp
END
GO
/****** Object:  UserDefinedFunction [dbo].[F_IsAppStatus]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_IsAppStatus]
(
  @courseID INT ,
  @userID INT 
)
RETURNS INT AS
BEGIN
    DECLARE @type INT=-1
    DECLARE @number INT=0
     DECLARE @count INT=0
    DECLARE @isApp INT=0
    DECLARE @sumcount INT=0
    --预定状态
    SELECT @type=OrderStatus  FROM Cl_CourseOrder
    WHERE CourseId=@courseID AND UserId=@userID
    
    SELECT @sumcount=ConfigValue FROM Sys_ParamConfig
				WHERE ConfigType=20	
    IF(@type=3)  --补预定
     BEGIN
	      IF((SELECT count(*) FROM Cl_MakeUpOrder WHERE CourseId=@courseID AND UserId=@userID)>0)
	       BEGIN	
	
		         SELECT @number=number,@count=count(*) FROM View_OrderTimes
		         WHERE CourseId=@courseID AND UserId=@userID
		          GROUP BY number
		          IF(@count>0)
		          BEGIN
		               IF(@number<=CAST(@sumcount AS INT))
		                    SET @isApp=1
		              ELSE
		                  SET @isApp=2
		          END
		             else
		             SET @isApp=0
		     END 
		     ELSE
		        SET @isApp=0
      END
	    ELSE  
	       SET @isApp=1
       RETURN @isApp
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetDeptIdPathByDeptID]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[GetDeptIdPathByDeptID]
(
 @deptId int    --当前部门
)
returns varchar(max) AS
 BEGIN
  declare @columns nvarchar(max)
  
  SET @columns = ''   
    begin   
        select @columns = @columns + ID + ',' FROM f_GetDeptChildByDeptID(@deptId)        
    end   
    if(len(@columns)>0)
		set  @columns= substring (@columns,1,len(@columns)-1)           
    RETURN @columns
   END
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetVdeioOpenObject]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetVdeioOpenObject]
(
  @courseID INT 
)
RETURNS NVARCHAR(max) AS
 BEGIN	
	DECLARE @OpenFlag INT, @OpenGroupObject NVARCHAR(50),@OpenDepartObject NVARCHAR(50), @OpenLevel   NVARCHAR(50)
	, @OpenGroup NVARCHAR(max),@OpenDepart NVARCHAR(MAX),@openObject NVARCHAR(max) ,@openObject1 NVARCHAR(max)
	 ,@IsOpenSub INT    ,@OpenSubStr NVARCHAR(max)
	SET  @OpenGroup=''
	SET  @OpenDepart='' 
	SET  @openObject=''
	SET  @openObject1=''
	SET  @OpenSubStr=''
	SELECT @OpenFlag=OpenFlag ,@OpenGroupObject=OpenGroupObject, @OpenDepartObject=OpenDepartObject,@OpenLevel= OpenLevel ,
	@IsOpenSub=IsOpenSub
	FROM Co_Course WHERE Id=@courseID
	
	IF(@OpenFlag=0)
    	BEGIN
	        SET @openObject=  @OpenLevel
    	END
	iF(@OpenFlag=1 or @OpenFlag=3)
    	BEGIN
            WITH INFO AS
            (
	     	    SELECT DISTINCT GroupName,TrainGrade  FROM View_UserInfo
	         	WHERE GroupId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenGroupObject))
	         	AND TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel)) 
             )
             
             SELECT @openObject=name+','+@openObject FROM (
             SELECT LEFT(name,LEN(name)-1) +':'+TrainGrade name FROM (
             SELECT 
              (SELECT GroupName+',' FROM  info WHERE TrainGrade=A.TrainGrade FOR XML path('')) AS name,TrainGrade
               
              FROM INFO   A
              GROUP BY  TrainGrade
             ) t  )r

     END
	  
	iF(@OpenFlag=2 or @OpenFlag=3)
    	BEGIN
    	    WITH INFO AS
            (
	     	     SELECT DISTINCT DeptName,TrainGrade FROM View_UserInfo
             	WHERE DeptId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenDepartObject))
             	AND TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel)) 
             )
             
             SELECT @openObject1=name+','+@openObject1 FROM (
             SELECT LEFT(name,LEN(name)-1) +':'+TrainGrade name FROM (
             SELECT 
              (SELECT DeptName+',' FROM  info WHERE TrainGrade=A.TrainGrade FOR XML path('')) AS name,TrainGrade
               
              FROM INFO   A
              GROUP BY  TrainGrade
             ) t  )r
          
	    END
	    
	    IF(@IsOpenSub=1)
	          SET @OpenSubStr=';全部分所'
   
	 IF(@OpenFlag=3)
	 BEGIN
	   	SET @openObject=substring(@openObject,0,len(@openObject))
     	SET @openObject1= substring(@openObject1,0,len(@openObject1)) 
	    SET @openObject=   @openObject+';'+ @openObject1 +@OpenSubStr
	   END
	  ELSE IF(@OpenFlag=2)
	    BEGIN
		  	SET @openObject=substring(@openObject,0,len(@openObject))
	     	SET @openObject1=  substring(@openObject1,0,len(@openObject1)) 
		    SET   @openObject=  @openObject1  +@OpenSubStr
		 END
	     
	  RETURN @openObject+@OpenSubStr
  END
GO
/****** Object:  StoredProcedure [dbo].[PROC_GetDepCourseUser]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[PROC_GetDepCourseUser]
AS
   
  if object_id('tempdb..#infoUser') is not null 
    drop table #infoUser
    
    CREATE TABLE #infoUser
    (
        courseID INT, 
        userID INT,
        DeptID INT 
        
    )
    
if object_id('tempdb..#courseInfo') is not null 
    drop table #courseInfo
    
    CREATE TABLE #courseInfo
    (
        number INT,
        Id INT,
        OpenLevel NVARCHAR(500),
        OpenFlag int,
        OpenDepartObject NVARCHAR(500),
        OpenGroupObject  NVARCHAR(500),
        OpenPerson NVARCHAR(500),
        IsOrder INT,
        DeptId INT
    )
   
	DECLARE @IsOpenSub INT,@OpenLevel NVARCHAR(500),@OpenFlag INT, @OpenDepartObject NVARCHAR(500), @OpenGroupObject  NVARCHAR(50)
	,@sumCount  INT,@sql NVARCHAR(max),@sql1 NVARCHAR(100),@i INT,@Id INT,@OpenPerson NVARCHAR(500),@IsOrder INT,@DeptId int;
	SET @i=1;
	
	INSERT into #courseInfo
		SELECT ROW_NUMBER()OVER(ORDER BY Id) number, Id,OpenLevel,OpenFlag,OpenDepartObject,OpenGroupObject
		,OpenPerson,IsOrder,DeptId FROM Dep_Course cc
		WHERE cc.IsDelete=0 AND cc.Way IN (1,3)   AND cc.CourseFrom=2 AND cc.Publishflag=1
	    --AND Id=409
	 
	  while @i<=(SELECT COUNT(*) FROM #courseInfo)
	  BEGIN
	     SELECT @OpenLevel=OpenLevel,@OpenFlag=OpenFlag,@OpenDepartObject=OpenDepartObject,@OpenGroupObject=OpenGroupObject
     	,@OpenPerson=OpenPerson,@Id=Id,@IsOrder=IsOrder,@DeptId=DeptId FROM #courseInfo WHERE number=@i
	 IF(@IsOrder=2)
	   BEGIN
	    INSERT into   #infoUser
		        SELECT @Id, UserId,DeptId FROM Sys_User
		        where  IsDelete=0 AND Status=0
	            AND ((DeptId=@DeptId AND TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel)))
	              OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson)))
	   END
	 ELSE
	   BEGIN
	      iF(@OpenFlag=0)  
		     INSERT into   #infoUser
		        SELECT @Id, UserId,DeptId FROM Sys_User WHERE (TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		         OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson)))
		        AND  IsDelete=0 AND Status=0  
		     ELSE IF ( @OpenFlag=2 )               
		       INSERT into   #infoUser
		        SELECT @Id, UserId,DeptId FROM Sys_User WHERE ((TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        AND DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject)))
		         OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson)))
		        AND  IsDelete=0 AND Status=0    
		   END
	     SET @i=@i+1
	  END
	  
	    SELECT courseID, DeptId,count(UserID) count FROM #infoUser
	    WHERE  UserID IN (SELECT UserID  FROM dbo.View_CheckUser)
	    GROUP BY  courseID,DeptId
GO
/****** Object:  UserDefinedFunction [dbo].[f_GetVedioNumber]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[f_GetVedioNumber]
(
  @courseID int
)
 RETURNs INT AS
 BEGIN
	DECLARE @IsOpenSub INT,@OpenLevel NVARCHAR(500),@OpenFlag INT, @OpenDepartObject NVARCHAR(500), @OpenGroupObject  NVARCHAR(500)
	,@sumCount  INT
	SELECT @OpenLevel=OpenLevel,@OpenFlag=OpenFlag,@OpenDepartObject=OpenDepartObject,@OpenGroupObject=OpenGroupObject
	,@IsOpenSub=IsOpenSub FROM dbo.Co_Course
	WHERE Id=@courseID
	
	        IF(@OpenFlag=0)  
		     
		        SELECT @sumCount=count(1) FROM Sys_User WHERE TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        AND  IsDelete=0 AND Status=0  
		         AND DeptId IN (SELECT DeptId FROM dbo.View_CheckUser)
		        OR (IsMain=1 AND  @IsOpenSub=1 AND  IsDelete=0 AND Status=0  AND DeptId IN (SELECT DeptId FROM dbo.View_CheckUser))
		       
		     ELSE IF (@OpenFlag=1 )
		       
		        SELECT @sumCount=count(1) FROM Sys_User WHERE TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        AND UserId IN (select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(@OpenGroupObject)))
		        AND  IsDelete=0 AND Status=0  
		           AND DeptId IN (SELECT DeptId FROM dbo.View_CheckUser)
		          OR (IsMain=1 AND  @IsOpenSub=1 AND  IsDelete=0 AND Status=0 AND DeptId IN (SELECT DeptId FROM dbo.View_CheckUser))
		      
		     ELSE IF ( @OpenFlag=2 )               
		     
		        SELECT @sumCount=count(1) FROM Sys_User WHERE TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        AND DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject) )
		         AND DeptId IN (SELECT DeptId FROM dbo.View_CheckUser)
		        AND  IsDelete=0 AND Status=0    OR (IsMain=1 AND  @IsOpenSub=1 AND  IsDelete=0 AND Status=0  AND DeptId IN (SELECT DeptId FROM dbo.View_CheckUser))
		    
		     ELSE IF ( @OpenFlag=3  ) 
		     
		         SELECT @sumCount=count(1) FROM Sys_User WHERE TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		         AND (
		               UserId IN (select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(@OpenGroupObject))) OR
		                DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject) )  
		        ) AND  IsDelete=0 AND Status=0 
		               AND DeptId IN (SELECT DeptId FROM dbo.View_CheckUser)
		           OR ( IsMain=1 AND  @IsOpenSub=1 AND  IsDelete=0 AND Status=0  AND DeptId IN (SELECT DeptId FROM dbo.View_CheckUser))
		  
	          RETURN @sumCount
	 END
GO
/****** Object:  StoredProcedure [dbo].[PROC_GetVedioUser]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[PROC_GetVedioUser]
AS
            if object_id('tempdb..#infoUser') is not null 
    drop table #infoUser
    
    CREATE TABLE #infoUser
    (
        courseID INT,
        IsOpenSub INT ,
        userID INT,
        DeptID INT 
        
    )
    
if object_id('tempdb..#courseInfo') is not null 
    drop table #courseInfo
    
    CREATE TABLE #courseInfo
    (
        number INT,
        Id INT,
        OpenLevel NVARCHAR(500),
        OpenFlag int,
        OpenDepartObject NVARCHAR(500),
        OpenGroupObject  NVARCHAR(500),
        IsOpenSub int
    )
   
	DECLARE @IsOpenSub INT,@OpenLevel NVARCHAR(500),@OpenFlag INT, @OpenDepartObject NVARCHAR(500), @OpenGroupObject  NVARCHAR(50)
	,@sumCount  INT,@sql NVARCHAR(max),@sql1 NVARCHAR(100),@i INT,@Id int;
	SET @i=1;
	
	INSERT into #courseInfo
		SELECT ROW_NUMBER()OVER(ORDER BY Id) number, Id,OpenLevel,OpenFlag,OpenDepartObject,OpenGroupObject
		,IsOpenSub FROM dbo.Co_Course cc
		WHERE cc.IsDelete=0 and
		 cc.Publishflag=1 and cc.CourseFrom=2 
		  AND  cc.Way=2
	 
	 
	  while @i<=(SELECT COUNT(*) FROM #courseInfo)
	  BEGIN
	     SELECT @OpenLevel=OpenLevel,@OpenFlag=OpenFlag,@OpenDepartObject=OpenDepartObject,@OpenGroupObject=OpenGroupObject
	,@IsOpenSub=IsOpenSub,@Id=Id FROM #courseInfo WHERE number=@i
	      iF(@OpenFlag=0)  
		     INSERT into   #infoUser
		        SELECT @Id,@IsOpenSub, UserId,DeptId FROM Sys_User WHERE TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        AND  IsDelete=0 AND Status=0  OR (IsMain=1 AND  @IsOpenSub=1 AND  IsDelete=0 AND Status=0)
		     
		     ELSE IF (@OpenFlag=1 )
		       INSERT into   #infoUser
		          SELECT @Id,@IsOpenSub,  UserId,DeptId FROM Sys_User WHERE TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		          AND UserId IN (select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(@OpenGroupObject)))
		          AND  IsDelete=0 AND Status=0    OR (IsMain=1 AND  @IsOpenSub=1 AND  IsDelete=0 AND Status=0)
		     
		     ELSE IF ( @OpenFlag=2 )               
		       INSERT into   #infoUser
		        SELECT @Id, @IsOpenSub, UserId,DeptId FROM Sys_User WHERE TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        AND DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject) )
		        AND  IsDelete=0 AND Status=0    OR (IsMain=1 AND  @IsOpenSub=1 AND  IsDelete=0 AND Status=0)
		     
		     ELSE IF ( @OpenFlag=3  ) 
		       INSERT into   #infoUser
		         SELECT @Id, @IsOpenSub, UserId,DeptId FROM Sys_User WHERE TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		         AND (
		               UserId IN (select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(@OpenGroupObject))) OR
		                DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject) )
		        ) AND  IsDelete=0 AND Status=0    OR( IsMain=1 AND  @IsOpenSub=1 AND  IsDelete=0 AND Status=0)
	           
	     SET @i=@i+1
	  END
	  
	    SELECT courseID, DeptId,count(UserID) count FROM #infoUser
	    WHERE  UserID IN (SELECT UserID  FROM dbo.View_CheckUser)
	    GROUP BY  courseID,DeptId ,IsOpenSub
GO
/****** Object:  StoredProcedure [dbo].[PROC_GetTogetUser]    Script Date: 04/18/2014 09:00:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[PROC_GetTogetUser]
AS
     if object_id('tempdb..#infoUser') is not null 
    drop table #infoUser
    
    CREATE TABLE #infoUser
    (
        courseID INT, 
        userID INT,
        DeptID INT 
        
    )
    
if object_id('tempdb..#courseInfo') is not null 
    drop table #courseInfo
    
    CREATE TABLE #courseInfo
    (
        number INT,
        Id INT,
        OpenLevel NVARCHAR(500),
        OpenFlag int,
        OpenDepartObject NVARCHAR(500),
        OpenGroupObject  NVARCHAR(500),
        OpenPerson NVARCHAR(500),
        IsOrder int
    )
   
	DECLARE @IsOpenSub INT,@OpenLevel NVARCHAR(500),@OpenFlag INT, @OpenDepartObject NVARCHAR(500), @OpenGroupObject  NVARCHAR(50)
	,@sumCount  INT,@sql NVARCHAR(max),@sql1 NVARCHAR(100),@i INT,@Id INT,@OpenPerson NVARCHAR(500),@IsOrder INT;
	SET @i=1;
	
	INSERT into #courseInfo
		SELECT ROW_NUMBER()OVER(ORDER BY Id) number, Id,OpenLevel,OpenFlag,OpenDepartObject,OpenGroupObject
		,OpenPerson,IsOrder FROM dbo.Co_Course cc
		WHERE cc.IsDelete=0 AND  cc.Way=1 AND cc.CourseFrom=2
	    --AND Id=409
	 
	  while @i<=(SELECT COUNT(*) FROM #courseInfo)
	  BEGIN
	     SELECT @OpenLevel=OpenLevel,@OpenFlag=OpenFlag,@OpenDepartObject=OpenDepartObject,@OpenGroupObject=OpenGroupObject
	,@OpenPerson=OpenPerson,@Id=Id,@IsOrder=IsOrder FROM #courseInfo WHERE number=@i
     IF(@IsOrder=2)
     	BEGIN
     	     INSERT into   #infoUser
		        SELECT @Id, UserId,DeptId FROM Sys_User
		        where TrainGrade not IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        and IsDelete=0 AND Status=0
	    END
	 ELSE
    	BEGIN
	      iF(@OpenFlag=0)  
		     INSERT into   #infoUser
		        SELECT @Id, UserId,DeptId FROM Sys_User WHERE (TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		         OR UserId IN (SELECT ID FROM    dbo.F_SplitIDs(@OpenPerson)))
		        AND  IsDelete=0 AND Status=0  
		     
		     ELSE IF (@OpenFlag=1 )
		       INSERT into   #infoUser
		          SELECT @Id,  UserId,DeptId FROM Sys_User WHERE ((TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		          AND UserId IN (select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(@OpenGroupObject))))
		           OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson)))
		          AND  IsDelete=0 AND Status=0  
		     
		     ELSE IF ( @OpenFlag=2 )               
		       INSERT into   #infoUser
		        SELECT @Id, UserId,DeptId FROM Sys_User WHERE ((TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		        AND DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject)))
		         OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson)))
		        AND  IsDelete=0 AND Status=0    
		     
		     ELSE IF ( @OpenFlag=3  ) 
		       INSERT into   #infoUser
		         SELECT @Id, UserId,DeptId FROM Sys_User WHERE ((TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))
		         AND (
		               UserId IN (select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(@OpenGroupObject))) OR
		                DeptId IN (SELECT id FROM  dbo.F_SplitIDs(@OpenDepartObject))
		        ))  OR UserId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenPerson))) AND  IsDelete=0 AND Status=0 
	        END   
	     SET @i=@i+1
	  END
	  
	    SELECT courseID, DeptId,count(UserID) count FROM #infoUser
	    WHERE  UserID IN (SELECT UserID  FROM dbo.View_CheckUser)
	    GROUP BY  courseID,DeptId
GO
