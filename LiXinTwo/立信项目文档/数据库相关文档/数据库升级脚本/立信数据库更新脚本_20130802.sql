
/*混合培训新增字段：课后多少小时内评估*/
ALTER TABLE New_CourseRoomRule ADD AfterEvlutionConfigTime decimal(18,2) DEFAULT (0);
go


update sys_right set ModuleName='NewMyTrain' where RightId=223 or RightId=224 or RightId=230 or RightId=231
GO

set identity_insert Sys_Right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (233, 'DeptMyCourseList', 0, '我已预订的课程（部门视频转播）', 'DepTranMyCourse/MyCourseList', 211, 7, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (234, 'DeptMyNoCourseList', 0, '我可预订的课程（部门视频转播）', 'DeptCourseCourseLearn/MyCourseSubscribe', 211, 9, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (236, '知识管理', 0, '知识管理', 'ReResourceManage/ReResourceManage', 1, 13, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (237, '知识中心', 0, '知识中心', 'ReResourceManage/ReMyResourceManage', 1, 1, 'Knowledge')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (238, '上传知识', 1, '上传知识', 'ReResourceManage/ResourceUpload', 236, 1, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (239, '编辑知识', 1, '编辑知识', 'ReResourceManage/UpdateResource', 236, 2, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (240, 'ReMyResourceListShow', 1, '知识详情', 'ReResourceManage/ReMyResourceListShow', 236, 3, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (241, 'PreReMyResourceListShow', 1, '知识详情', 'ReResourceManage/PreReMyResourceListShow', 237, 1, 'Knowledge')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (242, '部门群组人员编辑', 1, '部门群组人员编辑', 'DepTranDepartSetting/DepartSettingEdit', 219, 1, 'TrainManage')
GO

set identity_insert Sys_Right off

