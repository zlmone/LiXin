
/*新增权限 调整位置*/
set identity_insert Sys_Right on
INSERT INTO dbo.Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName)
VALUES (175, '我的视频课程', 0, '我的视频课程', 'MyCourse/VideoCourseList', 88, 124, '我的课程')
set identity_insert Sys_Right off

INSERT INTO dbo.Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName)
VALUES (176, 'VideoCourseMain', 0, '我的视频课程详情', 'MyCourse/VideoCourseMain', 175, NULL, 'MyTrain')
go

UPDATE Sys_Right
SET ShowOrder=123
WHERE RightId=110


UPDATE Sys_Right
SET ShowOrder=122
WHERE RightId=106


UPDATE dbo.Sys_Right
SET RightName = '查看注协课程详情'
	,RightDesc = '查看注协课程详情'
WHERE RightId = 172
GO


UPDATE dbo.Sys_Right
SET RightName = '查看注协课程'
	,RightDesc = '查看注协课程'
WHERE RightId = 165
GO




