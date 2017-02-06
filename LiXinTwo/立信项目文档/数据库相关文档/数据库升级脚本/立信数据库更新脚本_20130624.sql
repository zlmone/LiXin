
set identity_insert Sys_Right on
INSERT INTO dbo.Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName)
VALUES (177, 'MyApprovalOutTimeLeaveInfor', 0, '', 'MyApproval/MyApprovalOutTimeLeaveInfor', 91, NULL, 'TrainManage')

set identity_insert Sys_Right off
GO
set identity_insert Sys_Right off

ALTER TABLE Cl_CourseOrder ADD DropReason NVARCHAR(200) DEFAULT ('');
GO


ALTER TABLE Cl_CourseOrder ADD DropType int DEFAULT (0);
GO


ALTER TABLE Co_Course
ADD courseProvide NVARCHAR(500),studyRequirement  TEXT, remark  TEXT
GO


ALTER TABLE Sys_Notes
ADD Num INT DEFAULT((0))
GO

ALTER FUNCTION F_IsAppStatus
(
  @courseID INT ,
  @userID INT 
)
RETURNS INT AS
BEGIN
    DECLARE @type INT
    DECLARE @number INT
     DECLARE @count INT
    DECLARE @isApp INT
    DECLARE @sumcount INT
    SET @type=-1
     SET @number=0
     SET @count=0
     SET @isApp=0
     SET @sumcount=0
    --Ô¤¶¨×´Ì¬
    SELECT @type=OrderStatus  FROM Cl_CourseOrder
    WHERE CourseId=@courseID AND UserId=@userID
    
    SELECT @sumcount=ConfigValue FROM Sys_ParamConfig
				WHERE ConfigType=20	
    IF(@type=3)  --²¹Ô¤¶¨
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
		                  SET @isApp=0
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

ALTER VIEW View_OrderTimes
as
	SELECT * FROM (
	SELECT row_number()OVER(PARTITION BY UserId ORDER BY ApprovalDateTime ASC ) number ,* FROM(
	SELECT CourseId, UserId,  ApprovalDateTime  FROM Cl_MakeUpOrder
	WHERE ( ApprovalFlag=1 AND  ApprovalDateTime<=ApprovalLimitTime) 
	--OR (ApprovalFlag=0 AND ApprovalLimitTime>=getdate()) 
	UNION
	SELECT CourseId, UserId,  ApprovalDateTime  FROM Cl_TimeOutOrder
	WHERE ApprovalFlag=1 
	) t )result
   


