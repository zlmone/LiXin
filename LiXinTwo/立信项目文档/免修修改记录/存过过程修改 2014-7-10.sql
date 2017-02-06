ALTER PROC [dbo].[PROC_GetTogetUser]
(
   @Year INT 
)
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
		WHERE cc.IsDelete=0 AND  cc.Way=1 AND cc.CourseFrom=2   AND cc.YearPlan=@Year 
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
	  
	    SELECT courseID, DeptId,UserID FROM #infoUser
	    WHERE  UserID IN (SELECT UserID  FROM dbo.View_CheckUser)
	    --GROUP BY  courseID,DeptId



GO

