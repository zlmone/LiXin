
/*����������ֶ�*/
ALTER TABLE Sys_Notes ADD DepTrainFlag int DEFAULT (1);
go

/*���ñ����*/
update sys_paramconfig set configvalue='100,3;20,20,40,20;40,60' where configid=33
go
set identity_insert sys_paramconfig on 
insert into sys_paramconfig (configid,configname,configtype,configvalue,lastupdatetime,usertype) 
values (38,'�ۺ����֡�����ÿ�ο۷ֶ���',40,'0.1','2013-07-26',1)
go
set identity_insert sys_paramconfig off


/*Ȩ���޸�*/
UPDATE Sys_Right 
SET RightName='MyApprovalOutTimeLeaveInfor', 
RightType=0, 
RightDesc='��ʱ������������', 
Path='MyApproval/MyApprovalOutTimeLeaveInfor', 
ParentId=91, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=177
GO

UPDATE Sys_Right 
SET RightName='�༶ά��', 
RightType=0, 
RightDesc='�༶ά��', 
Path='NewClassManage/NewClassManage', 
ParentId=179, 
ShowOrder=2, 
ModuleName='TrainManage'
WHERE RightId=178
GO

UPDATE Sys_Right
SET RightName='�½�Ա����ѵ����', 
RightType=0, 
RightDesc='�½�Ա����ѵ����', 
Path='NewUserManage', 
ParentId=1, 
ShowOrder=11, 
ModuleName='TrainManage'
WHERE RightId=179
GO

UPDATE Sys_Right
SET RightName='�½�Ա��ά��', 
RightType=0, 
RightDesc='�½�Ա��ά��', 
Path='NewUserManage/NewUserManage', 
ParentId=179, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=180
GO

UPDATE Sys_Right
SET RightName='�ۺ����ֹ���', 
RightType=0, 
RightDesc='�ۺ����ֹ���', 
Path='NewAllScoreManager/NewAllScoreManagerList', 
ParentId=179, 
ShowOrder=6, 
ModuleName='TrainManage'
WHERE RightId=181
GO

UPDATE Sys_Right 
SET RightName='�ۺ�����', 
RightType=0, 
RightDesc='�ۺ�����', 
Path='NewAllScoreManager/NewAllScore', 
ParentId=181, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=182
GO

UPDATE Sys_Right 
SET RightName='�γ̿���', 
RightType=0, 
RightDesc='�γ̿���', 
Path='NewCourseManage/NewCourseManageList', 
ParentId=179, 
ShowOrder=4, 
ModuleName='TrainManage'
WHERE RightId=183
GO

UPDATE Sys_Right 
SET RightName='NewAttendceManage', 
RightType=0, 
RightDesc='���ڹ���', 
Path='NewAttendceManage/NewAttendceList', 
ParentId=179, 
ShowOrder=5, 
ModuleName='TrainManage'
WHERE RightId=184
GO

UPDATE Sys_Right 
SET RightName='��ʦ����(�½�Ա��)', 
RightType=0, 
RightDesc='��ʦ����(�½�Ա��)', 
Path='NewMyEvaluate/NewEvaluateUserToTeacher', 
ParentId=88, 
ShowOrder=3, 
ModuleName='NewMyTrain'
WHERE RightId=185
GO

UPDATE Sys_Right 
SET RightName='��ѵ�ɼ�¼��', 
RightType=0, 
RightDesc='��ѵ�ɼ�¼��', 
Path='NewUserManage/NewInputScore', 
ParentId=179, 
ShowOrder=9, 
ModuleName='TrainManage'
WHERE RightId=186
GO

UPDATE Sys_Right 
SET RightName='ѧԱ�ۺ����ֲ�ѯ', 
RightType=0, 
RightDesc='ѧԱ�ۺ����ֲ�ѯ', 
Path='NewQueryStatistics/StudentQueryStatisticsList', 
ParentId=179, 
ShowOrder=7, 
ModuleName='TrainManage'
WHERE RightId=187
GO

UPDATE Sys_Right 
SET RightName='����ά��', 
RightType=0, 
RightDesc='����ά��', 
Path='NewClassRoom/ClassRoomManage', 
ParentId=179, 
ShowOrder=3, 
ModuleName='TrainManage'
WHERE RightId=188
GO

UPDATE Sys_Right 
SET RightName='�༭�½�Ա������', 
RightType=1, 
RightDesc='�༭�½�Ա������', 
Path='NewClassRoom/ClassRoomEdit', 
ParentId=188, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=189
GO

UPDATE Sys_Right 
SET RightName='�½�Ա����������', 
RightType=1, 
RightDesc='�½�Ա����������', 
Path='NewClassRoom/ClassRoomDetial', 
ParentId=188, 
ShowOrder=2, 
ModuleName='TrainManage'
WHERE RightId=190
GO

UPDATE Sys_Right 
SET RightName='�༶��Ա����', 
RightType=1, 
RightDesc='�༶��Ա����', 
Path='NewClassManage/ClassPersonManage', 
ParentId=178, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=191
GO

UPDATE Sys_Right 
SET RightName='�ҵ��ۺ�����', 
RightType=0, 
RightDesc='�ҵ��ۺ�����', 
Path='NewQueryStatistics/MyQueryStatistics', 
ParentId=1, 
ShowOrder=2, 
ModuleName='NewMyTrain'
WHERE RightId=192
GO

UPDATE Sys_Right 
SET RightName='�༭�½�Ա�������ѵ�γ�', 
RightType=1, 
RightDesc='�༭�½�Ա�������ѵ�γ�', 
Path='NewCourseManage/EditNewCourseTogether', 
ParentId=183, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=193
GO

UPDATE Sys_Right 
SET RightName='�༭�½�Ա�������ڿογ�', 
RightType=1, 
RightDesc='�༭�½�Ա�������ڿογ�', 
Path='NewCourseManage/CourseSeatSistribute', 
ParentId=183, 
ShowOrder=2, 
ModuleName='TrainManage'
WHERE RightId=194
GO

UPDATE Sys_Right 
SET RightName='��Ա������', 
RightType=0, 
RightDesc='��Ա������', 
Path='NewUserManage/UserDetail', 
ParentId=180, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=195
GO

UPDATE Sys_Right
SET RightName='�༭�½�Ա��������̿γ�', 
RightType=1, 
RightDesc='�༭�½�Ա��������̿γ�', 
Path='NewCourseManage/GroupCourseSeatSistribute', 
ParentId=183, 
ShowOrder=3, 
ModuleName='TrainManage'
WHERE RightId=196
GO

UPDATE Sys_Right 
SET RightName='�༭�½�Ա����Ƶ�γ�', 
RightType=1, 
RightDesc='�༭�½�Ա����Ƶ�γ�', 
Path='NewCourseManage/EditNewCourseVideo', 
ParentId=183, 
ShowOrder=4, 
ModuleName='TrainManage'
WHERE RightId=197
GO

UPDATE Sys_Right 
SET RightName='�½�Ա����Ƶ�γ�����', 
RightType=1, 
RightDesc='�½�Ա����Ƶ�γ�����', 
Path='NewCourseManage/NewCourseVideoDetial', 
ParentId=183, 
ShowOrder=5, 
ModuleName='TrainManage'
WHERE RightId=198
GO

UPDATE Sys_Right 
SET RightName='�½�Ա�������ѵ�γ�����', 
RightType=1, 
RightDesc='�½�Ա�������ѵ�γ�����', 
Path='NewCourseManage/NewCourseTogetherDetial', 
ParentId=183, 
ShowOrder=6, 
ModuleName='TrainManage'
WHERE RightId=199
GO

UPDATE Sys_Right 
SET RightName='�����ۺ�����', 
RightType=1, 
RightDesc='�����ۺ�����', 
Path='NewQueryStatistics/StudentQueryStatistics', 
ParentId=187, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=200
GO

UPDATE Sys_Right 
SET RightName='��Ա�����ڹ�������', 
RightType=1, 
RightDesc='��Ա�����ڹ�������', 
Path='NewAttendceManage/NewAttendceDetail', 
ParentId=184, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=201
GO

UPDATE Sys_Right 
SET RightName='MyNewCourse', 
RightType=0, 
RightDesc='�ҵ��½���ѵ�γ��б�', 
Path='NewMyCourse/CourseList', 
ParentId=228, 
ShowOrder=1, 
ModuleName='NewMyTrain'
WHERE RightId=202
GO

UPDATE Sys_Right
SET RightName='�ҵĻ����ѵ�γ�����', 
RightType=1, 
RightDesc='�ҵĻ����ѵ�γ�����', 
Path='NewMyCourse/MyCourse', 
ParentId=202, 
ShowOrder=1, 
ModuleName='NewMyTrain'
WHERE RightId=203
GO

UPDATE Sys_Right 
SET RightName='NewMyTeachCourse', 
RightType=0, 
RightDesc='�ҵ��ڿογ�(�½�Ա��)', 
Path='NewTeacherCourse/CourseList', 
ParentId=88, 
ShowOrder=4, 
ModuleName='MyTrain'
WHERE RightId=204
GO

UPDATE Sys_Right 
SET RightName='�ҵ��½��γ��ڿ�����', 
RightType=1, 
RightDesc='�ҵ��½��γ��ڿ�����', 
Path='NewTeacherCourse/TeacherCourse', 
ParentId=204, 
ShowOrder=1, 
ModuleName='MyTrain'
WHERE RightId=205
GO

UPDATE Sys_Right 
SET RightName='MyNewVideoCourse', 
RightType=0, 
RightDesc='�ҵ��½���ѧ�γ�', 
Path='NewMyCourse/StudyCourseList', 
ParentId=228, 
ShowOrder=2, 
ModuleName='NewMyTrain'
WHERE RightId=206
GO

UPDATE Sys_Right
SET RightName='�ҵ�ѧʱ�����ڻ�ȡ', 
RightType=0, 
RightDesc='�ҵ�ѧʱ�����ڻ�ȡ', 
Path='DepTranMyCourse/MyDepTranScore', 
ParentId=211, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=207
GO

UPDATE Sys_Right 
SET RightName='����/����Ա��ѧʱ�����ڻ�ȡ', 
RightType=0, 
RightDesc='����/����Ա��ѧʱ�����ڻ�ȡ', 
Path='DepTranMyCourse/DepUserScore', 
ParentId=211, 
ShowOrder=2, 
ModuleName='TrainManage'
WHERE RightId=208
GO

UPDATE Sys_Right 
SET RightName='NewMyTrainIndex', 
RightType=0, 
RightDesc='�½�Ա����ѵ��ҳ', 
Path='Home/NewMyTrainIndex', 
ParentId=1, 
ShowOrder=1, 
ModuleName='NewMyTrain'
WHERE RightId=209
GO

UPDATE Sys_Right 
SET RightName='����ѧʱ����', 
RightType=1, 
RightDesc='����ѧʱ����', 
Path='DepTranMyCourse/DepUserScoreDetails', 
ParentId=208, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=210
GO

UPDATE Sys_Right 
SET RightName='���ŷ����γ�ת��', 
RightType=0, 
RightDesc='���ŷ����γ�ת��', 
Path='#', 
ParentId=1, 
ShowOrder=12, 
ModuleName='TrainManage'
WHERE RightId=211
GO

UPDATE Sys_Right 
SET RightName='ת�����ڹ���', 
RightType=0, 
RightDesc='ת�����ڹ���', 
Path='DepTranAttendce/AttendceManageList', 
ParentId=211, 
ShowOrder=4, 
ModuleName='TrainManage'
WHERE RightId=212
GO

UPDATE Sys_Right 
SET RightName='�ҵİ���', 
RightType=1, 
RightDesc='�ҵİ���', 
Path='NewClassManage/MyClassRoom', 
ParentId=1, 
ShowOrder=6, 
ModuleName='NewMyTrain'
WHERE RightId=213
GO

UPDATE Sys_Right 
SET RightName='�κ�����ͳ��', 
RightType=0, 
RightDesc='�κ�����ͳ��', 
Path='NewMyEvaluate/NewEvaluateAllUserToTeacher', 
ParentId=179, 
ShowOrder=10, 
ModuleName='TrainManage'
WHERE RightId=214
GO

UPDATE Sys_Right 
SET RightName='NewTeacherEvlutionDetail', 
RightType=1, 
RightDesc='��ʦ����', 
Path='NewMyEvaluate/NewEvaluateUserToTeacherDetail', 
ParentId=185, 
ShowOrder=1, 
ModuleName='MyTrain'
WHERE RightId=215
GO

UPDATE Sys_Right 
SET RightName='NewTeacherEvlutionManageDetail', 
RightType=1, 
RightDesc='��������', 
Path='NewMyEvaluate/NewEvaluateAllUserToTeacherDetail', 
ParentId=214, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=216
GO

UPDATE Sys_Right 
SET RightName='MyBroadcastList', 
RightType=0, 
RightDesc='ת���γ�', 
Path='DepTrainMyBroadcastCourse/MyBroadcastList', 
ParentId=211, 
ShowOrder=5, 
ModuleName='TrainManage'
WHERE RightId=217
GO

UPDATE Sys_Right 
SET RightName='MyBroadcastCourseDetail', 
RightType=1, 
RightDesc='�γ�����', 
Path='DepTrainMyBroadcastCourse/BroadcastCourseMain', 
ParentId=217, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=218
GO

UPDATE Sys_Right 
SET RightName='������Ա����', 
RightType=0, 
RightDesc='������Ա����', 
Path='DepTranDepartSetting/DepartSettingManage', 
ParentId=211, 
ShowOrder=3, 
ModuleName='TrainManage'
WHERE RightId=219
GO

UPDATE Sys_Right 
SET RightName='ת���γ̹���', 
RightType=0, 
RightDesc='ת���γ̹���', 
Path='DeptCourseManage/DeptCourseManage', 
ParentId=211, 
ShowOrder=6, 
ModuleName='TrainManage'
WHERE RightId=220
GO

UPDATE Sys_Right 
SET RightName='DepTranCourseDetail', 
RightType=1, 
RightDesc='�γ�����', 
Path='DeptCourseManage/DeptCourseDetail', 
ParentId=220, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=221
GO

UPDATE Sys_Right 
SET RightName='MySurveyList', 
RightType=0, 
RightDesc='�ҵ�������Ա����', 
Path='Survey/MySurveyList', 
ParentId=1, 
ShowOrder=7, 
ModuleName='NewMyTrain'
WHERE RightId=222
GO

UPDATE Sys_Right 
SET RightName='NewDoSurvey', 
RightType=1, 
RightDesc='�������', 
Path='Survey/NewDoSurvey', 
ParentId=222, 
ShowOrder=1, 
ModuleName=''
WHERE RightId=223
GO

UPDATE Sys_Right 
SET RightName='NewBrowseSurveyResult', 
RightType=1, 
RightDesc='�鿴��������', 
Path='Survey/NewBrowseSurveyResult', 
ParentId=222, 
ShowOrder=2, 
ModuleName=''
WHERE RightId=224
GO

/*����Ȩ��*/

set identity_insert Sys_Right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (225, 'ת����������', 0, '', 'DepTranAttendce/DepTranAttendceApprovalList', 211, 1, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (226, 'ת�����ڹ�������', 1, '', 'DepTranAttendce/DepTranAttendce', 212, 2, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (227, 'ת��������������', 1, '', 'DepTranAttendce/DepTranAttendceApprovalCourseInfor', 225, 3, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (228, 'MyNewCourseList', 0, '�ҵĿγ̣���Ա����', 'MyNewCourseList', 1, 3, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (229, 'NewMyPropose', 0, '�ҵĿ�ǰ����������', 'NewMypropose/MyproposeMain', 1, 6, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (230, 'NewMyProposeDetail', 1, '�ҵĿ�ǰ����', 'NewMypropose/Mypropose', 229, 1, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (231, 'NewMyAfterCourseDetail', 1, '�ҵ���������', 'NewMypropose/AfterCourse', 229, 2, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (232, 'MyNewAttendceList', 0, '�ҵĿ��ڣ���Ա����', 'NewMypropose/MyNewAttendceList', 1, 7, 'NewMyTrain')
GO

set identity_insert Sys_Right off




