INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式、免修申请开放时间配置', 60, '',getdate())
GO

INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式、免修审批开放时间配置', 61, '',getdate())
GO

INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式考核级别配置', 62, '',getdate())



INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4',getdate())
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2015-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2016-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2017-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2018-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2019-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2020-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2021-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2022-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2023-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式申请项目--课后评估', 64, '0.5,4','2024-01-01')

GO

INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '',getdate())
GO

INSERT INTO dbo.Sys_ParamConfig ( ConfigName, ConfigType, ConfigValue, LastUpdateTime, userType)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40',getdate(), 0)
 INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2015-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2016-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2017-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2018-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2019-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2020-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2021-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2022-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2023-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('免修配置--自动折算', 63, '07-01,40;07-01,40','2024-01-01')
GO

--------------------------------------------------------------------------------
--免修配置,其他形式-其他项目配置
CREATE TABLE Free_OtherApplyConfig
(
   ID INT IDENTITY PRIMARY KEY,
   ApplyContent NVARCHAR(200), --申请内容
   ConvertType INT,--可折算学时 0 CPA学时 1 所内学时 2所有
   ConvertBase DECIMAL(18,1),--学时折算 基数
   ConvertBaseInfo NVARCHAR(50),--学时折算 计量单位
   ConvertBaseTo DECIMAL(18,1),--学时折算 要折算成的学时
   UploadTip  NVARCHAR(200), --上传文件提示
   ConvertMax DECIMAL(18,1),--可折CPA学时年度的最高限制
   TrainGradeScore NVARCHAR(500),--可折内部培训学时年度的最高限制
   Memo NVARCHAR(200),--情况说明模板
   year INT ,
   CreateTime DATETIME,
   IsDelete INT，
   ApplyType INT --0 授课人 1 其他项目
)
go

--免修配置,免修申请配置
CREATE TABLE Free_ApplyConfig
(
   ID INT IDENTITY PRIMARY KEY,
   FreeName NVARCHAR(200),--免修项目
   UploadTip  NVARCHAR(200), --上传文件提示
   ApplyType INT,--适用的申请 0 所内免修 1 CPA免修 2所有
   CPAFreeScore INT,--CPA免修学时
   TogetherFreeScore INT,  --所内免修学时  
   Memo NVARCHAR(200)--情况说明模板,
   year INT,
   CreateTime DATETIME,
   IsDelete INT
)              
 go


--其他形式，免修申请
CREATE TABLE Free_UserOtherApply
(
  ID INT IDENTITY PRIMARY KEY,
  ApplyID INT, --申请类型 0 授课人  
               --ApplyType=1 Free_OtherApplyConfig主键  
               -- ApplyType=2 Free_ApplyConfig主键
            
  Year INT,--申请年度
  ConvertScore DECIMAL(18,1),
  Memo NVARCHAR(200),  --说明情况
  ApplyTime DATETIME,--申请时间
  ApplyUserID INT,   --申请人
  ApplyType INT, --1 其他形式 2 免修
  Status INT,--0 未提交 1提交
  ApproveStatus INT,-- 0 待审批 1 审批通过 2 分所负责人退回 2 总所负责人退回
  tScore DECIMAL(18,1),--所内学时
  CPAScore DECIMAL(18,1),--CPA学时
  IsDelete INT
)
 go            

--其他形式申请  授课讲师详情
CREATE TABLE Free_UserApplyTeacherDetails
(
   ID INT IDENTITY PRIMARY KEY,
   userApplyID INT,   --Free_UserOtherApply 外键
   ClassName NVARCHAR(200),--培训班名称
   ConvertScore DECIMAL(18,1),
   tScore DECIMAL(18,1),--所内学时
   CPAScore DECIMAL(18,1),--CPA学时
   IsDelete INT
)
  go  
   
--个人申请证明文档
CREATE TABLE Free_UserApplyFiles
(
  ID INT IDENTITY PRIMARY KEY,
  userApplyID INT,   --Free_UserOtherApply 主键
  ResourceName NVARCHAR(500),--真实名称
  RealName NVARCHAR(500),--存储名称
  IsDelete INT
)

go
--审批
CREATE TABLE Free_Apporve
(
     ID INT IDENTITY PRIMARY KEY,
     userApplyID INT,   --ApproveType=1或者2 Free_UserOtherApply 主键，
                        --ApproveType=3 Free_UserApplyOtherForm主键
     DepTrainApprove INT,--部门/分所负责人审批 状态 0 拒绝 1 通过        
     DepApprove INT, --总所负责人审批 状态 0 拒绝 1 通过
     DepTrainReason NVARCHAR(500),--部门/分所退回理由
     DepReason  NVARCHAR(500), --总所退回理由
     CheackBatch INT,--批次
     type INT,--1 分所 2 总所
     ApproveUserID INT,
     ApproveTime DATETIME,
     ApproveType INT --1 其他形式 2 免修 3其他形式项目 
)
go     

--其他形式项目申请
CREATE TABLE  Free_UserApplyOtherForm
(
  ID INT IDENTITY PRIMARY KEY,
  OtherFromID int, --Free_OtherFroms的主键
  CourseName NVARCHAR(200),--课程名称
  Address NVARCHAR(200),--培训地点
  StartTime DATETIME,--培训开始时间
  EndTime DATETIME,--培训结束时间
  TogetherScore INT,--申请所内学时
  CPAScore INT,--申请CPA学时
  ApplyDateTime DATETIME,
  ApplyUserID INT,--申请人ID
  Status INT,--0 未提交 1提交
  ApproveStatus INT-- 0 待审批 1 审批通过 2  审批拒绝
)
 GO
 
  //授权审批人
  CREATE TABLE Free_DepApprove
  (
     ID INT IDENTITY PRIMARY KEY,
     UserID INT,--授权给的审批人ID
     CreateUserID INT,--创建人
     CreateTime DATETIME,--创建时间
     ManageDeptIDs NVARCHAR(500),---所管理的部门ID集合  
     LeardID NVARCHAR(50) --创建人的jobNumber
  )
 
 -----------------------------默认值---------------------------------------------
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2014, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2015, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2016, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2017, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2018, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2019, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2020, getdate(), 0, 0)
  INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2021, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2022, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2023, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 0, 0, NULL, 3, NULL, 20, '20', '', 2024, getdate(), 0, 0)