-- Server: 192.168.4.20
-- Table:  Sys_Right
-- Date:   2013-08-28 13:39

/* 新增权限 2013-8-28 */
set identity_insert sys_right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (324, 'DepCourseAppointDetail', 1, '课程指定查询', 'DepSelfCourseAppointSearch/DepCourseAppointDetail', 304, 1, 'DepManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (325, 'DepTranAttendceDetail', 1, '转播考勤详情', 'DepTranAttendce/DepTranAttendce', 212, 1, 'DepManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (326, 'DepAttendceDetail', 1, '考勤管理（部门分所）', 'DepAttendce/DepAttendce', 286, 1, 'DepManage')
GO

set identity_insert sys_right off
go

/*修改字段类型*/
alter table Dep_Course alter column PreAdviceConfigTime decimal(18,2)
go
alter table Dep_Course alter column AfterEvlutionConfigTime decimal(18,2)
go
alter table Dep_Course alter column Memo text
go

/*修改配置*/
update sys_paramconfig set configvalue='1;01;01;1;12;30' where configtype=49
go

/*新增系统配置*/
set identity_insert sys_paramconfig on
INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (58, '部门分所请假模板', 52, '<p>致尊敬的领导：</p><p>本人因{0}缘由，无法参加{CourseDate}的《{CourseName}》培训课程，特此请假！</p><p   style="text-align: right">请假人：{Myself}</p><p  style="text-align: right">{LeaveDate}</p>', '2013-09-01', NULL)
GO

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (60, '视频课程学分折算', 53, '50', '2013-09-01', NULL)
GO
set identity_insert sys_paramconfig off

/*修改人员表默认值*/
update Sys_User set ManageDeparts='' where ManageDeparts is NULL