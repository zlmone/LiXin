
/*�����ѵ�����ֶΣ��κ����Сʱ������*/
ALTER TABLE New_CourseRoomRule ADD AfterEvlutionConfigTime decimal(18,2) DEFAULT (0);
go


update sys_right set ModuleName='NewMyTrain' where RightId=223 or RightId=224 or RightId=230 or RightId=231
GO

set identity_insert Sys_Right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (233, 'DeptMyCourseList', 0, '����Ԥ���Ŀγ̣�������Ƶת����', 'DepTranMyCourse/MyCourseList', 211, 7, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (234, 'DeptMyNoCourseList', 0, '�ҿ�Ԥ���Ŀγ̣�������Ƶת����', 'DeptCourseCourseLearn/MyCourseSubscribe', 211, 9, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (236, '֪ʶ����', 0, '֪ʶ����', 'ReResourceManage/ReResourceManage', 1, 13, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (237, '֪ʶ����', 0, '֪ʶ����', 'ReResourceManage/ReMyResourceManage', 1, 1, 'Knowledge')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (238, '�ϴ�֪ʶ', 1, '�ϴ�֪ʶ', 'ReResourceManage/ResourceUpload', 236, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (239, '�༭֪ʶ', 1, '�༭֪ʶ', 'ReResourceManage/UpdateResource', 236, 2, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (240, 'ReMyResourceListShow', 1, '֪ʶ����', 'ReResourceManage/ReMyResourceListShow', 236, 3, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (241, 'PreReMyResourceListShow', 1, '֪ʶ����', 'ReResourceManage/PreReMyResourceListShow', 237, 1, 'Knowledge')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (242, '����Ⱥ����Ա�༭', 1, '����Ⱥ����Ա�༭', 'DepTranDepartSetting/DepartSettingEdit', 219, 1, 'TrainManage')
GO

set identity_insert Sys_Right off

