
/*公告表新增字段*/
ALTER TABLE Sys_Notes ADD DepTrainFlag int DEFAULT (1);
go

/*配置表更新*/
update sys_paramconfig set configvalue='100,3;20,20,40,20;40,60' where configid=33
go
set identity_insert sys_paramconfig on 
insert into sys_paramconfig (configid,configname,configtype,configvalue,lastupdatetime,usertype) 
values (38,'综合评分—评估每次扣分多少',40,'0.1','2013-07-26',1)
go
set identity_insert sys_paramconfig off


/*权限修改*/
UPDATE Sys_Right 
SET RightName='MyApprovalOutTimeLeaveInfor', 
RightType=0, 
RightDesc='逾时申请审批详情', 
Path='MyApproval/MyApprovalOutTimeLeaveInfor', 
ParentId=91, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=177
GO

UPDATE Sys_Right 
SET RightName='班级维护', 
RightType=0, 
RightDesc='班级维护', 
Path='NewClassManage/NewClassManage', 
ParentId=179, 
ShowOrder=2, 
ModuleName='TrainManage'
WHERE RightId=178
GO

UPDATE Sys_Right
SET RightName='新进员工培训管理', 
RightType=0, 
RightDesc='新进员工培训管理', 
Path='NewUserManage', 
ParentId=1, 
ShowOrder=11, 
ModuleName='TrainManage'
WHERE RightId=179
GO

UPDATE Sys_Right
SET RightName='新进员工维护', 
RightType=0, 
RightDesc='新进员工维护', 
Path='NewUserManage/NewUserManage', 
ParentId=179, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=180
GO

UPDATE Sys_Right
SET RightName='综合评分管理', 
RightType=0, 
RightDesc='综合评分管理', 
Path='NewAllScoreManager/NewAllScoreManagerList', 
ParentId=179, 
ShowOrder=6, 
ModuleName='TrainManage'
WHERE RightId=181
GO

UPDATE Sys_Right 
SET RightName='综合评分', 
RightType=0, 
RightDesc='综合评分', 
Path='NewAllScoreManager/NewAllScore', 
ParentId=181, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=182
GO

UPDATE Sys_Right 
SET RightName='课程开设', 
RightType=0, 
RightDesc='课程开设', 
Path='NewCourseManage/NewCourseManageList', 
ParentId=179, 
ShowOrder=4, 
ModuleName='TrainManage'
WHERE RightId=183
GO

UPDATE Sys_Right 
SET RightName='NewAttendceManage', 
RightType=0, 
RightDesc='考勤管理', 
Path='NewAttendceManage/NewAttendceList', 
ParentId=179, 
ShowOrder=5, 
ModuleName='TrainManage'
WHERE RightId=184
GO

UPDATE Sys_Right 
SET RightName='讲师评价(新进员工)', 
RightType=0, 
RightDesc='讲师评价(新进员工)', 
Path='NewMyEvaluate/NewEvaluateUserToTeacher', 
ParentId=88, 
ShowOrder=3, 
ModuleName='NewMyTrain'
WHERE RightId=185
GO

UPDATE Sys_Right 
SET RightName='培训成绩录入', 
RightType=0, 
RightDesc='培训成绩录入', 
Path='NewUserManage/NewInputScore', 
ParentId=179, 
ShowOrder=9, 
ModuleName='TrainManage'
WHERE RightId=186
GO

UPDATE Sys_Right 
SET RightName='学员综合评分查询', 
RightType=0, 
RightDesc='学员综合评分查询', 
Path='NewQueryStatistics/StudentQueryStatisticsList', 
ParentId=179, 
ShowOrder=7, 
ModuleName='TrainManage'
WHERE RightId=187
GO

UPDATE Sys_Right 
SET RightName='教室维护', 
RightType=0, 
RightDesc='教室维护', 
Path='NewClassRoom/ClassRoomManage', 
ParentId=179, 
ShowOrder=3, 
ModuleName='TrainManage'
WHERE RightId=188
GO

UPDATE Sys_Right 
SET RightName='编辑新进员工教室', 
RightType=1, 
RightDesc='编辑新进员工教室', 
Path='NewClassRoom/ClassRoomEdit', 
ParentId=188, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=189
GO

UPDATE Sys_Right 
SET RightName='新进员工教室详情', 
RightType=1, 
RightDesc='新进员工教室详情', 
Path='NewClassRoom/ClassRoomDetial', 
ParentId=188, 
ShowOrder=2, 
ModuleName='TrainManage'
WHERE RightId=190
GO

UPDATE Sys_Right 
SET RightName='班级人员调整', 
RightType=1, 
RightDesc='班级人员调整', 
Path='NewClassManage/ClassPersonManage', 
ParentId=178, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=191
GO

UPDATE Sys_Right 
SET RightName='我的综合评定', 
RightType=0, 
RightDesc='我的综合评定', 
Path='NewQueryStatistics/MyQueryStatistics', 
ParentId=1, 
ShowOrder=2, 
ModuleName='NewMyTrain'
WHERE RightId=192
GO

UPDATE Sys_Right 
SET RightName='编辑新进员工混合培训课程', 
RightType=1, 
RightDesc='编辑新进员工混合培训课程', 
Path='NewCourseManage/EditNewCourseTogether', 
ParentId=183, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=193
GO

UPDATE Sys_Right 
SET RightName='编辑新进员工集中授课课程', 
RightType=1, 
RightDesc='编辑新进员工集中授课课程', 
Path='NewCourseManage/CourseSeatSistribute', 
ParentId=183, 
ShowOrder=2, 
ModuleName='TrainManage'
WHERE RightId=194
GO

UPDATE Sys_Right 
SET RightName='新员工详情', 
RightType=0, 
RightDesc='新员工详情', 
Path='NewUserManage/UserDetail', 
ParentId=180, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=195
GO

UPDATE Sys_Right
SET RightName='编辑新进员工分组带教课程', 
RightType=1, 
RightDesc='编辑新进员工分组带教课程', 
Path='NewCourseManage/GroupCourseSeatSistribute', 
ParentId=183, 
ShowOrder=3, 
ModuleName='TrainManage'
WHERE RightId=196
GO

UPDATE Sys_Right 
SET RightName='编辑新进员工视频课程', 
RightType=1, 
RightDesc='编辑新进员工视频课程', 
Path='NewCourseManage/EditNewCourseVideo', 
ParentId=183, 
ShowOrder=4, 
ModuleName='TrainManage'
WHERE RightId=197
GO

UPDATE Sys_Right 
SET RightName='新进员工视频课程详情', 
RightType=1, 
RightDesc='新进员工视频课程详情', 
Path='NewCourseManage/NewCourseVideoDetial', 
ParentId=183, 
ShowOrder=5, 
ModuleName='TrainManage'
WHERE RightId=198
GO

UPDATE Sys_Right 
SET RightName='新进员工混合培训课程详情', 
RightType=1, 
RightDesc='新进员工混合培训课程详情', 
Path='NewCourseManage/NewCourseTogetherDetial', 
ParentId=183, 
ShowOrder=6, 
ModuleName='TrainManage'
WHERE RightId=199
GO

UPDATE Sys_Right 
SET RightName='个人综合评定', 
RightType=1, 
RightDesc='个人综合评定', 
Path='NewQueryStatistics/StudentQueryStatistics', 
ParentId=187, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=200
GO

UPDATE Sys_Right 
SET RightName='新员工考勤管理详情', 
RightType=1, 
RightDesc='新员工考勤管理详情', 
Path='NewAttendceManage/NewAttendceDetail', 
ParentId=184, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=201
GO

UPDATE Sys_Right 
SET RightName='MyNewCourse', 
RightType=0, 
RightDesc='我的新进培训课程列表', 
Path='NewMyCourse/CourseList', 
ParentId=228, 
ShowOrder=1, 
ModuleName='NewMyTrain'
WHERE RightId=202
GO

UPDATE Sys_Right
SET RightName='我的混合培训课程详情', 
RightType=1, 
RightDesc='我的混合培训课程详情', 
Path='NewMyCourse/MyCourse', 
ParentId=202, 
ShowOrder=1, 
ModuleName='NewMyTrain'
WHERE RightId=203
GO

UPDATE Sys_Right 
SET RightName='NewMyTeachCourse', 
RightType=0, 
RightDesc='我的授课课程(新进员工)', 
Path='NewTeacherCourse/CourseList', 
ParentId=88, 
ShowOrder=4, 
ModuleName='MyTrain'
WHERE RightId=204
GO

UPDATE Sys_Right 
SET RightName='我的新进课程授课详情', 
RightType=1, 
RightDesc='我的新进课程授课详情', 
Path='NewTeacherCourse/TeacherCourse', 
ParentId=204, 
ShowOrder=1, 
ModuleName='MyTrain'
WHERE RightId=205
GO

UPDATE Sys_Right 
SET RightName='MyNewVideoCourse', 
RightType=0, 
RightDesc='我的新进自学课程', 
Path='NewMyCourse/StudyCourseList', 
ParentId=228, 
ShowOrder=2, 
ModuleName='NewMyTrain'
WHERE RightId=206
GO

UPDATE Sys_Right
SET RightName='我的学时及考勤获取', 
RightType=0, 
RightDesc='我的学时及考勤获取', 
Path='DepTranMyCourse/MyDepTranScore', 
ParentId=211, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=207
GO

UPDATE Sys_Right 
SET RightName='部门/分所员工学时及考勤获取', 
RightType=0, 
RightDesc='部门/分所员工学时及考勤获取', 
Path='DepTranMyCourse/DepUserScore', 
ParentId=211, 
ShowOrder=2, 
ModuleName='TrainManage'
WHERE RightId=208
GO

UPDATE Sys_Right 
SET RightName='NewMyTrainIndex', 
RightType=0, 
RightDesc='新进员工培训首页', 
Path='Home/NewMyTrainIndex', 
ParentId=1, 
ShowOrder=1, 
ModuleName='NewMyTrain'
WHERE RightId=209
GO

UPDATE Sys_Right 
SET RightName='个人学时详情', 
RightType=1, 
RightDesc='个人学时详情', 
Path='DepTranMyCourse/DepUserScoreDetails', 
ParentId=208, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=210
GO

UPDATE Sys_Right 
SET RightName='部门分所课程转播', 
RightType=0, 
RightDesc='部门分所课程转播', 
Path='#', 
ParentId=1, 
ShowOrder=12, 
ModuleName='TrainManage'
WHERE RightId=211
GO

UPDATE Sys_Right 
SET RightName='转播考勤管理', 
RightType=0, 
RightDesc='转播考勤管理', 
Path='DepTranAttendce/AttendceManageList', 
ParentId=211, 
ShowOrder=4, 
ModuleName='TrainManage'
WHERE RightId=212
GO

UPDATE Sys_Right 
SET RightName='我的班组', 
RightType=1, 
RightDesc='我的班组', 
Path='NewClassManage/MyClassRoom', 
ParentId=1, 
ShowOrder=6, 
ModuleName='NewMyTrain'
WHERE RightId=213
GO

UPDATE Sys_Right 
SET RightName='课后评估统计', 
RightType=0, 
RightDesc='课后评估统计', 
Path='NewMyEvaluate/NewEvaluateAllUserToTeacher', 
ParentId=179, 
ShowOrder=10, 
ModuleName='TrainManage'
WHERE RightId=214
GO

UPDATE Sys_Right 
SET RightName='NewTeacherEvlutionDetail', 
RightType=1, 
RightDesc='讲师详情', 
Path='NewMyEvaluate/NewEvaluateUserToTeacherDetail', 
ParentId=185, 
ShowOrder=1, 
ModuleName='MyTrain'
WHERE RightId=215
GO

UPDATE Sys_Right 
SET RightName='NewTeacherEvlutionManageDetail', 
RightType=1, 
RightDesc='评价详情', 
Path='NewMyEvaluate/NewEvaluateAllUserToTeacherDetail', 
ParentId=214, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=216
GO

UPDATE Sys_Right 
SET RightName='MyBroadcastList', 
RightType=0, 
RightDesc='转播课程', 
Path='DepTrainMyBroadcastCourse/MyBroadcastList', 
ParentId=211, 
ShowOrder=5, 
ModuleName='TrainManage'
WHERE RightId=217
GO

UPDATE Sys_Right 
SET RightName='MyBroadcastCourseDetail', 
RightType=1, 
RightDesc='课程详情', 
Path='DepTrainMyBroadcastCourse/BroadcastCourseMain', 
ParentId=217, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=218
GO

UPDATE Sys_Right 
SET RightName='部门人员管理', 
RightType=0, 
RightDesc='部门人员管理', 
Path='DepTranDepartSetting/DepartSettingManage', 
ParentId=211, 
ShowOrder=3, 
ModuleName='TrainManage'
WHERE RightId=219
GO

UPDATE Sys_Right 
SET RightName='转播课程管理', 
RightType=0, 
RightDesc='转播课程管理', 
Path='DeptCourseManage/DeptCourseManage', 
ParentId=211, 
ShowOrder=6, 
ModuleName='TrainManage'
WHERE RightId=220
GO

UPDATE Sys_Right 
SET RightName='DepTranCourseDetail', 
RightType=1, 
RightDesc='课程详情', 
Path='DeptCourseManage/DeptCourseDetail', 
ParentId=220, 
ShowOrder=1, 
ModuleName='TrainManage'
WHERE RightId=221
GO

UPDATE Sys_Right 
SET RightName='MySurveyList', 
RightType=0, 
RightDesc='我的需求（新员工）', 
Path='Survey/MySurveyList', 
ParentId=1, 
ShowOrder=7, 
ModuleName='NewMyTrain'
WHERE RightId=222
GO

UPDATE Sys_Right 
SET RightName='NewDoSurvey', 
RightType=1, 
RightDesc='参与调查', 
Path='Survey/NewDoSurvey', 
ParentId=222, 
ShowOrder=1, 
ModuleName=''
WHERE RightId=223
GO

UPDATE Sys_Right 
SET RightName='NewBrowseSurveyResult', 
RightType=1, 
RightDesc='查看调查详情', 
Path='Survey/NewBrowseSurveyResult', 
ParentId=222, 
ShowOrder=2, 
ModuleName=''
WHERE RightId=224
GO

/*新增权限*/

set identity_insert Sys_Right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (225, '转播考勤审批', 0, '', 'DepTranAttendce/DepTranAttendceApprovalList', 211, 1, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (226, '转播考勤管理详情', 1, '', 'DepTranAttendce/DepTranAttendce', 212, 2, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (227, '转播考勤审批详情', 1, '', 'DepTranAttendce/DepTranAttendceApprovalCourseInfor', 225, 3, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (228, 'MyNewCourseList', 0, '我的课程（新员工）', 'MyNewCourseList', 1, 3, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (229, 'NewMyPropose', 0, '我的课前建议与评估', 'NewMypropose/MyproposeMain', 1, 6, 'NewMyTrain')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (230, 'NewMyProposeDetail', 1, '我的课前建议', 'NewMypropose/Mypropose', 229, 1, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (231, 'NewMyAfterCourseDetail', 1, '我的评估详情', 'NewMypropose/AfterCourse', 229, 2, '')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (232, 'MyNewAttendceList', 0, '我的考勤（新员工）', 'NewMypropose/MyNewAttendceList', 1, 7, 'NewMyTrain')
GO

set identity_insert Sys_Right off




