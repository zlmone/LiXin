
/*����Ȩ�� ����λ��*/
set identity_insert Sys_Right on
INSERT INTO dbo.Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName)
VALUES (175, '�ҵ���Ƶ�γ�', 0, '�ҵ���Ƶ�γ�', 'MyCourse/VideoCourseList', 88, 124, '�ҵĿγ�')
set identity_insert Sys_Right off

INSERT INTO dbo.Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName)
VALUES (176, 'VideoCourseMain', 0, '�ҵ���Ƶ�γ�����', 'MyCourse/VideoCourseMain', 175, NULL, 'MyTrain')
go

UPDATE Sys_Right
SET ShowOrder=123
WHERE RightId=110


UPDATE Sys_Right
SET ShowOrder=122
WHERE RightId=106


UPDATE dbo.Sys_Right
SET RightName = '�鿴עЭ�γ�����'
	,RightDesc = '�鿴עЭ�γ�����'
WHERE RightId = 172
GO


UPDATE dbo.Sys_Right
SET RightName = '�鿴עЭ�γ�'
	,RightDesc = '�鿴עЭ�γ�'
WHERE RightId = 165
GO




