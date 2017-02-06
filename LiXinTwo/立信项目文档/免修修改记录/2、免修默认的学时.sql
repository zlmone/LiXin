INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式、免修申请开放时间配置', 60, '',getdate())
GO

INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式、免修审批开放时间配置', 61, '',getdate())
GO

INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他形式考核级别配置', 62, 'A,B,C,D;A,B,C,D;A,B,C,D',getdate())


INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '0,0',getdate())
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

 
 -----------------------------默认值---------------------------------------------
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2014, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2015, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2016, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2017, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2018, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人',2, 0, NULL, 3, NULL, 20, '20', '', 2019, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2020, getdate(), 0, 0)
  INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2021, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2022, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人', 2, 0, NULL, 3, NULL, 20, '20', '', 2023, getdate(), 0, 0)
 INSERT INTO dbo.Free_OtherApplyConfig (ApplyContent, ConvertType, ConvertBase, ConvertBaseInfo, ConvertBaseTo, UploadTip, ConvertMax, TrainGradeScore, Memo, year, CreateTime, IsDelete, ApplyType)
VALUES ('担当培训授课人、研讨会主持人或演讲人及承担我所内部培训的授课人',2, 0, NULL, 3, NULL, 20, '20', '', 2024, getdate(), 0, 0)
                                       
-------------------------------------------------------------------------------
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0',getdate())
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2015-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2016-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2017-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2018-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2019-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2020-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2021-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2022-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2023-01-01')
INSERT INTO dbo.Sys_ParamConfig (ConfigName, ConfigType, ConfigValue,LastUpdateTime)
VALUES ('其他有组织形式折算学时限制', 65, '999,0','2024-01-01')
                        
                        
                        


