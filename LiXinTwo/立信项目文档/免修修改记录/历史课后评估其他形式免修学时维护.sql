DECLARE  @Pos INT--逗号位置
DECLARE @ConfigValue NVARCHAR(500)--课后评估的配置
DECLARE @tScoreStr  NVARCHAR(500),@MaxtScoreStr NVARCHAR(500),
@tScore  DECIMAL(18,2),@MaxtScore DECIMAL(18,2)--学时
,@maxStr NVARCHAR(max)

SET @maxStr='';

SELECT @ConfigValue=ConfigValue FROM Sys_ParamConfig
WHERE ConfigType=64 AND datename(year,LastUpdateTime)='2014' 

SET @Pos = CHARINDEX(',', @ConfigValue, 1)

set @tScoreStr=REPLACE(LTRIM(RTRIM(LEFT(@ConfigValue, @Pos - 1))), '''', '')
set @MaxtScoreStr=REPLACE(@ConfigValue,cast(@tScoreStr AS NVARCHAR(500))+',','')

SET @tScore=CAST(@tScoreStr AS DECIMAL(18,2))
SET @MaxtScore=CAST(@MaxtScoreStr AS DECIMAL(18,2))
--游标
	DECLARE surveyUser_cursor CURSOR FOR 
		SELECT distinct ObjectID,UserID FROM Survey_ReplyAnswer sra
		LEFT JOIN Co_Course cc ON cc.Id=sra.ObjectID
		WHERE sra.SourceFrom=0  AND cc.YearPlan=2014
	OPEN surveyUser_cursor
	      DECLARE @courseID INT,@userID INT
	   FETCH NEXT FROM  surveyUser_cursor INTO @courseID,@userID
	 WHILE(@@FETCH_STATUS=0)
	   BEGIN
	       --此课后评估是否有记录
	       IF((SELECT count(1) FROM Free_UserOtherApply WHERE ApplyID=@courseID AND ApplyUserID=@userID AND typeForm=2 AND IsDelete=0)=0)
	        BEGIN
	           --此人的课后评估所内学时总和是否超过最大限制
	           IF((SELECT isnull(sum(GettScore),0) FROM Free_UserOtherApply WHERE ApplyUserID=@userID AND typeForm=2 AND IsDelete=0)<@MaxtScore)
	            BEGIN
	              INSERT INTO dbo.Free_UserOtherApply
					(
					ApplyID,
					ApplyName,
					Year,
					ConvertScore,
					Memo,
					ApplyTime,
					ApplyUserID,
					ApplyType,
					Status,
					ApproveStatus,
					DepApproveStatus,
					IsDelete,
					tScore,
					CPAScore,
					GettScore,
					GetCPAScore,
					DepTrainReason,
					DepReason,
					typeForm,
					CreateUserID,
					CreateTime,
					fromID
					)
				VALUES 
					(
					@courseID,
					'',
					2014,
					0,
					'',
					getdate(),
					@userID,
					2,
					0,
					1,
					1,
					0,
					@tScore,
					0,
					@tScore,
					0,
					'',
					'',
					2,
					0,
					getdate(),
					0
					)
	            END 
	        END
	        
	       FETCH NEXT FROM  surveyUser_cursor INTO @courseID,@userID
	   END
	  
	CLOSE   surveyUser_cursor
	DEALLOCATE  surveyUser_cursor





