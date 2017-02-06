/*
*
*表Co_Course 新增字段：1、CourseLengthDistribute ：课程学时分配方式；2、IsSystem：是否框架内；3、IsYearPlan：是否计划内
*
*/
alter   table   Co_Course   add   CourseLengthDistribute   nvarchar(50) default(0);
alter   table   Co_Course   add   IsSystem   int default(0);
alter   table   Co_Course   add   IsYearPlan   int default(0);
go
update Co_Course set CourseLengthDistribute='',IsSystem=0,IsYearPlan=0 where IsYearPlan is NULL;
go

/*
*
*新增权限：1、上级指定管理；2、上级指定编辑
*
*/
set identity_insert sys_right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (170, '授课课程管理', 0, '授课课程管理', 'AllTeacherCourse/ALLCourseList', 53, 6, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (171, '授课课程详情', 1, '授课课程详情', 'AllTeacherCourse/CourseMain', 170, NULL, 'TrainManage')
GO
set identity_insert sys_right off