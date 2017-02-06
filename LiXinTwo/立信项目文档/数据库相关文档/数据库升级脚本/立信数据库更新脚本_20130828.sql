-- Server: 192.168.4.20
-- Table:  Sys_Right
-- Date:   2013-08-28 13:39

/* ����Ȩ�� 2013-8-28 */
set identity_insert sys_right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (324, 'DepCourseAppointDetail', 1, '�γ�ָ����ѯ', 'DepSelfCourseAppointSearch/DepCourseAppointDetail', 304, 1, 'DepManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (325, 'DepTranAttendceDetail', 1, 'ת����������', 'DepTranAttendce/DepTranAttendce', 212, 1, 'DepManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (326, 'DepAttendceDetail', 1, '���ڹ������ŷ�����', 'DepAttendce/DepAttendce', 286, 1, 'DepManage')
GO

set identity_insert sys_right off
go

/*�޸��ֶ�����*/
alter table Dep_Course alter column PreAdviceConfigTime decimal(18,2)
go
alter table Dep_Course alter column AfterEvlutionConfigTime decimal(18,2)
go
alter table Dep_Course alter column Memo text
go

/*�޸�����*/
update sys_paramconfig set configvalue='1;01;01;1;12;30' where configtype=49
go

/*����ϵͳ����*/
set identity_insert sys_paramconfig on
INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (58, '���ŷ������ģ��', 52, '<p>���𾴵��쵼��</p><p>������{0}Ե�ɣ��޷��μ�{CourseDate}�ġ�{CourseName}����ѵ�γ̣��ش���٣�</p><p   style="text-align: right">����ˣ�{Myself}</p><p  style="text-align: right">{LeaveDate}</p>', '2013-09-01', NULL)
GO

INSERT INTO Sys_ParamConfig (ConfigId, ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType) VALUES (60, '��Ƶ�γ�ѧ������', 53, '50', '2013-09-01', NULL)
GO
set identity_insert sys_paramconfig off

/*�޸���Ա��Ĭ��ֵ*/
update Sys_User set ManageDeparts='' where ManageDeparts is NULL