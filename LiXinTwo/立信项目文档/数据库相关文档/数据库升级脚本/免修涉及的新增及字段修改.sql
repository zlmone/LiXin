INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ���������뿪��ʱ������', 60, '',getdate())
GO

INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ��������������ʱ������', 61, '',getdate())
GO

INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ���˼�������', 62, '',getdate())



INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4',getdate())
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2015-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2016-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2017-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2018-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2019-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2020-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2021-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2022-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2023-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('������ʽ������Ŀ--�κ�����', 64, '0.5,4','2024-01-01')

GO

INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������֯��ʽ����ѧʱ����', 65, '',getdate())
GO

INSERT INTO dbo.Sys_ParamConfig ( ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40',getdate(), 0)
 INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2015-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2016-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2017-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2018-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2019-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2020-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2021-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2022-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2023-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('��������--�Զ�����', 63, '07-01,40;07-01,40','2024-01-01')
GO

--------------------------------------------------------------------------------
--��������,������ʽ-������Ŀ����
CREATE TABLE Free_OtherApplyConfig
(
   ID INT IDENTITY PRIMARY KEY,
   ApplyContent NVARCHAR(200), --��������
   ConvertType INT,--������ѧʱ 0 CPAѧʱ 1 ����ѧʱ 2����
   ConvertBase DECIMAL(18,1),--ѧʱ���� ����
   ConvertBaseInfo NVARCHAR(50),--ѧʱ���� ������λ
   ConvertBaseTo DECIMAL(18,1),--ѧʱ���� Ҫ����ɵ�ѧʱ
   UploadTip  NVARCHAR(200), --�ϴ��ļ���ʾ
   ConvertMax DECIMAL(18,1),--����CPAѧʱ��ȵ��������
   TrainGradeScore NVARCHAR(500),--�����ڲ���ѵѧʱ��ȵ��������
   Memo NVARCHAR(200),--���˵��ģ��
   year INT ,
   CreateTime DATETIME,
   IsDelete INT��
   ApplyType INT --0 �ڿ��� 1 ������Ŀ
)
go

--��������,������������
CREATE TABLE Free_ApplyConfig
(
   ID INT IDENTITY PRIMARY KEY,
   FreeName NVARCHAR(200),--������Ŀ
   UploadTip  NVARCHAR(200), --�ϴ��ļ���ʾ
   ApplyType INT,--���õ����� 0 �������� 1 CPA���� 2����
   CPAFreeScore INT,--CPA����ѧʱ
   TogetherFreeScore INT,  --��������ѧʱ  
   Memo NVARCHAR(200)--���˵��ģ��,
   year INT,
   CreateTime DATETIME,
   IsDelete INT
)              
 go


--������ʽ����������
CREATE TABLE Free_UserOtherApply
(
  ID INT IDENTITY PRIMARY KEY,
  ApplyID INT, --�������� 0 �ڿ���  
               --ApplyType=1 Free_OtherApplyConfig����  
               -- ApplyType=2 Free_ApplyConfig����
            
  Year INT,--�������
  ConvertScore DECIMAL(18,1),
  Memo NVARCHAR(200),  --˵�����
  ApplyTime DATETIME,--����ʱ��
  ApplyUserID INT,   --������
  ApplyType INT, --1 ������ʽ 2 ����
  Status INT,--0 δ�ύ 1�ύ
  ApproveStatus INT,-- 0 ������ 1 ����ͨ�� 2 �����������˻� 2 �����������˻�
  tScore DECIMAL(18,1),--����ѧʱ
  CPAScore DECIMAL(18,1),--CPAѧʱ
  IsDelete INT
)
 go            

--������ʽ����  �ڿν�ʦ����
CREATE TABLE Free_UserApplyTeacherDetails
(
   ID INT IDENTITY PRIMARY KEY,
   userApplyID INT,   --Free_UserOtherApply ���
   ClassName NVARCHAR(200),--��ѵ������
   ConvertScore DECIMAL(18,1),
   tScore DECIMAL(18,1),--����ѧʱ
   CPAScore DECIMAL(18,1),--CPAѧʱ
   IsDelete INT
)
  go  
   
--��������֤���ĵ�
CREATE TABLE Free_UserApplyFiles
(
  ID INT IDENTITY PRIMARY KEY,
  userApplyID INT,   --Free_UserOtherApply ����
  ResourceName NVARCHAR(500),--��ʵ����
  RealName NVARCHAR(500),--�洢����
  IsDelete INT
)

go
--����
CREATE TABLE Free_Apporve
(
     ID INT IDENTITY PRIMARY KEY,
     userApplyID INT,   --ApproveType=1����2 Free_UserOtherApply ������
                        --ApproveType=3 Free_UserApplyOtherForm����
     DepTrainApprove INT,--����/�������������� ״̬ 0 �ܾ� 1 ͨ��        
     DepApprove INT, --�������������� ״̬ 0 �ܾ� 1 ͨ��
     DepTrainReason NVARCHAR(500),--����/�����˻�����
     DepReason  NVARCHAR(500), --�����˻�����
     CheackBatch INT,--����
     type INT,--1 ���� 2 ����
     ApproveUserID INT,
     ApproveTime DATETIME,
     ApproveType INT --1 ������ʽ 2 ���� 3������ʽ��Ŀ 
)
go     

--������ʽ��Ŀ����
CREATE TABLE  Free_UserApplyOtherForm
(
  ID INT IDENTITY PRIMARY KEY,
  OtherFromID int, --Free_OtherFroms������
  CourseName NVARCHAR(200),--�γ�����
  Address NVARCHAR(200),--��ѵ�ص�
  StartTime DATETIME,--��ѵ��ʼʱ��
  EndTime DATETIME,--��ѵ����ʱ��
  TogetherScore INT,--��������ѧʱ
  CPAScore INT,--����CPAѧʱ
  ApplyDateTime DATETIME,
  ApplyUserID INT,--������ID
  Status INT,--0 δ�ύ 1�ύ
  ApproveStatus INT-- 0 ������ 1 ����ͨ�� 2  �����ܾ�
)
 GO
 
  //��Ȩ������
  CREATE TABLE Free_DepApprove
  (
     ID INT IDENTITY PRIMARY KEY,
     UserID INT,--��Ȩ����������ID
     CreateUserID INT,--������
     CreateTime DATETIME,--����ʱ��
     ManageDeptIDs NVARCHAR(500),---������Ĳ���ID����  
     LeardID NVARCHAR(50) --�����˵�jobNumber
  )
 
 -----------------------------Ĭ��ֵ---------------------------------------------
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2014, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2015, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2016, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2017, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2018, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2019, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2020, getdate(), 0, 0)
  INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2021, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2022, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2023, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('������ѵ�ڿ��ˡ����ֻ������˻��ݽ��˼��е������ڲ���ѵ���ڿ���', 0, 0, NULL, 3, NULL, 20, '20', '', 2024, getdate(), 0, 0)