
--�γ�Ԥ������ӿ��ڻ�õ�ѧʱ--
ALTER TABLE Cl_CourseOrder ADD AttScore decimal(18,2) DEFAULT (0);
GO

--�γ�Ԥ�����Ƿ�������--
ALTER TABLE Cl_CourseOrder ADD AttFlag int DEFAULT (0);
GO

update Cl_CourseOrder set AttScore=0,AttFlag=1 where AttScore is NULL or AttFlag is NULL
GO

--�γ̱��Ƿ�Ͷ�ŵ���ҳ�л���--
ALTER TABLE Co_Course ADD AdFlag int DEFAULT (0);
GO

update Co_Course set AdFlag=0 where AdFlag is NULL
GO

--������Ƿ�Ͷ�ŵ���ҳ�л���--
ALTER TABLE Sys_Notes ADD AdFlag int DEFAULT (0);
GO

update Sys_Notes set AdFlag=0 where AdFlag is NULL
GO

ALTER FUNCTION [dbo].[F_IsAppStatus]
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
    --Ԥ��״̬
    SELECT @type=OrderStatus  FROM Cl_CourseOrder
    WHERE CourseId=@courseID AND UserId=@userID
    
    SELECT @sumcount=ConfigValue FROM Sys_ParamConfig
				WHERE ConfigType=20	
    IF(@type=3)  --��Ԥ��
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