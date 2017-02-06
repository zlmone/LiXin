/*
*
*��Co_Course �����ֶΣ�1��CourseLengthDistribute ���γ�ѧʱ���䷽ʽ��2��IsSystem���Ƿ����ڣ�3��IsYearPlan���Ƿ�ƻ���
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
*����Ȩ�ޣ�1���ϼ�ָ������2���ϼ�ָ���༭
*
*/
set identity_insert sys_right on

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (170, '�ڿογ̹���', 0, '�ڿογ̹���', 'AllTeacherCourse/ALLCourseList', 53, 6, 'TrainManage')
GO

INSERT INTO Sys_Right (RightId, RightName, RightType, RightDesc, Path, ParentId, ShowOrder, ModuleName) VALUES (171, '�ڿογ�����', 1, '�ڿογ�����', 'AllTeacherCourse/CourseMain', 170, NULL, 'TrainManage')
GO
set identity_insert sys_right off