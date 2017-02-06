using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using LiXinSync.User;

namespace LiXinSync
{
    /// <summary>
    /// 
    /// </summary>
    public class SyncInfo
    {
        DataAccess da;
        private LiXinBLL.User.UserBL userBL = new LiXinBLL.User.UserBL();
        private LiXinBLL.User.DepartmentBL deptBL = new LiXinBLL.User.DepartmentBL();
        private LiXinBLL.Sys_TrainGradeBL gradeBL = new LiXinBLL.Sys_TrainGradeBL();
        private LiXinBLL.CourseManage.Sys_PayGradeBL payGradeBL = new LiXinBLL.CourseManage.Sys_PayGradeBL();

        /// <summary>
        /// HR同步的字符串连接
        /// </summary>
        protected string HrConnectStr = "";
        /// <summary>
        /// 指纹库同步的字符串连接
        /// </summary>
        protected string UserFingerConnectStr = "";
        /// <summary>
        /// 原培训系统指纹库同步的字符串连接
        /// </summary>
        protected string TrainUserFingerConnectStr = "";
        /// <summary>
        /// 原培训培训级别同步字符串
        /// </summary>
        protected string TrainGradeSqlConnectStr = "";

        public SyncInfo()
        {
            HrConnectStr = ConfigurationManager.AppSettings["HRSqlConnection"];
            UserFingerConnectStr = ConfigurationManager.AppSettings["UserFingerSqlConnection"];
            TrainUserFingerConnectStr = ConfigurationManager.AppSettings["TrainUserFingerSqlConnection"];
            TrainGradeSqlConnectStr = ConfigurationManager.AppSettings["TrainGradeSqlConnection"];
        }

        #region 同步人员模块
        /// <summary>
        /// 同步人员模块
        /// </summary>
        /// <param name="flag">0：自动执行；1：手动执行；</param>
        public void Sync(int flag = 0)
        {
            da = new SqlDataAccess(HrConnectStr);

            var deptList = new List<LiXinModels.User.Sys_Department>();
            var hrDeptList = new List<LiXinSync.User.Sys_DeptHR>();
            SyncDept(deptList, hrDeptList);

            SyncUser(deptList, flag);
        }

        /// <summary>
        /// 同步部门
        /// </summary>
        private void SyncDept(List<LiXinModels.User.Sys_Department> deptList, List<LiXinSync.User.Sys_DeptHR> hrDeptList)
        {
            da = new SqlDataAccess(HrConnectStr);
            hrDeptList.AddRange(BaseModel.GetModelsBySql<LiXinSync.User.Sys_DeptHR>(string.Format("select * from {0} ", ConfigurationManager.AppSettings["HRDeptTable"]), da));
            deptList.AddRange(deptBL.GetAllList("Sys_Department.IsDelete = 0"));
            foreach (var item in hrDeptList)
            {
                var model = deptList.Find(p => p.OldDeptId == item.dept_id);
                if (model == null)
                {
                    model = new LiXinModels.User.Sys_Department
                    {
                        DeptName = item.CONTEnT,
                        DeptCode = item.DEPT_CODE,
                        DeptGrade = item.Grade,
                        ParentDeptId = 0,
                        IsDelete = 0,
                        Remark = "",
                        TypeNum = 0,
                        OldDeptId = item.dept_id
                    };
                    deptBL.Add(model);
                    deptList.Add(model);
                }
                else
                {
                    model.DeptName = item.CONTEnT;
                    model.DeptCode = item.DEPT_CODE;
                    model.DeptGrade = item.Grade;
                    model.OldDeptId = item.dept_id;

                    deptBL.Update(model);
                }
            }

            foreach (var item in hrDeptList)
            {
                var model = deptList.Find(p => p.OldDeptId == item.dept_id);
                var tmp = deptList.Find(p => p.DeptCode == item.PDEPT_CODE);
                if (tmp != null)
                    model.ParentDeptId = tmp.DepartmentId;
                else
                    model.ParentDeptId = 0;
                deptBL.Update(model);
            }


            #region 计算部门的路径
            try
            {
                foreach (var item in deptList)
                {
                    var path = item.DeptName;
                    var parentId = item.ParentDeptId;
                    while (parentId > 1)
                    {
                        var tmp = deptList.Find(p => p.DepartmentId == parentId);
                        path = tmp.DeptName + "/" + path;
                        parentId = tmp.ParentDeptId;
                    }
                    item.DeptPath = path;
                }
            }
            catch
            {

            }
            #endregion
        }

        /// <summary>
        /// 同步人员
        /// </summary>
        /// <param name="flag">0：自动执行；1：手动执行；</param>
        private void SyncUser(List<LiXinModels.User.Sys_Department> deptList, int flag)
        {
            da = new SqlDataAccess(HrConnectStr);

            //hr中的用户数据
            var hrUserList = BaseModel.GetModelsBySql<LiXinSync.User.Sys_UserHR>(string.Format("select * from {0} ", ConfigurationManager.AppSettings["HRUserTable"]), da);

            //系统中用户数据
            var userList = userBL.GetList("Sys_User.IsDelete = 0");

            //系统中的薪酬级别
            var payGradeList = payGradeBL.GetSys_PayGradeList();

            if (userList.Count > 0)
            {
                #region 用户表中已存在用户，执行修改操作
                if (flag == 0)
                {
                    #region 自动执行时，从用户变更表中，读取数据，根据修改的用户来进行更新
                    //hr中的用户变更数据
                    var hrUserChangeList = BaseModel.GetModelsBySql<LiXinSync.User.Sys_UserChangeHR>(string.Format("select tId,colName,tableName,makeDate,modifyType from {0} where sys1=1 and tableName = 'A01' ", ConfigurationManager.AppSettings["HRUserChangeTable"], DateTime.Now.Date.AddDays(-2)), da);
                    foreach (var userChange in hrUserChangeList.OrderBy(p => p.makeDate))
                    {
                        try
                        {
                            var model = userList.Find(p => p.JobNum == userChange.tId);
                            var item = hrUserList.Find(p => p.A0188 == userChange.tId);
                            var deptTmp = deptList.Find(p => p.OldDeptId == Convert.ToInt32(item.DEPT_ID));
                            if (item == null)
                                continue;
                            if (model == null)
                                model = new LiXinModels.User.Sys_User();

                            //维护薪酬级别
                            if (!string.IsNullOrEmpty(item.A01105))
                            {
                                if (payGradeList.Find(p => p.GradeName == item.A01105) == null)
                                {
                                    var tmp = new LiXinModels.CourseManage.Sys_PayGrade
                                                  {
                                                      GradeName = item.A01105
                                                  };
                                    payGradeBL.AddSys_PayGrade(tmp);
                                    if (tmp.Id > 0)
                                        payGradeList.Add(tmp);
                                }
                            }

                            model.Username = item.A01155 ?? "";
                            model.ApprovalRole = item.A01077 ?? "";//去掉2013-6-8

                            model.CPANo = item.A01113 ?? "";
                            model.CPAPractice = item.A01189 ?? "";//去掉2013-6-8
                            model.CPARelationship = item.A01260 ?? "";//去掉2013-6-8
                            model.CPAStatus = item.A01158 ?? "";//去掉2013-6-8
                            model.CPA = model.CPAStatus == "执业" && model.CPARelationship != "--" && model.CPARelationship != "立信所外" ? "是" : "否";
                            model.CPV = item.A01119 ?? "";
                            model.CTA = item.A01118 ?? "";
                            model.LoginId = item.A01155 ?? "";

                            model.DeptName = deptTmp == null ? "" : deptTmp.DeptName;
                            model.DeptCode = deptTmp == null ? "" : deptTmp.DeptCode;
                            model.DeptId = deptTmp == null ? 0 : deptTmp.DepartmentId;
                            model.DeptPath = deptTmp == null ? "" : deptTmp.DeptPath;

                            model.Email = item.Email ?? "";
                            model.ForeignOtherQualification = item.A01123 ?? "";
                            model.GraduateSchool = item.A0160 ?? "";
                            model.GroupMobileNum = item.A01149 ?? "";
                            model.ICLV = item.A01120 ?? "";//去掉2013-11-7
                            model.IdCardNo = item.A0177 ?? "";
                            model.JobNum = item.A0188 ?? "";
                            model.JobTitle = item.A01042 ?? "";
                            model.LeaderID = item.A011572 ?? "";
                            model.Major = item.A0161 ?? "";
                            model.MobileNum = item.MobileTel ?? "";
                            model.OtherProfessional = item.A01122 ?? "";
                            model.OtherProfessionsJobTitle = item.A01128 ?? "";
                            model.PayGrade = item.A01105 ?? (string.IsNullOrEmpty(model.PayGrade) ? model.PayGrade : "");
                            model.PostGrade = item.A01088 ?? "";
                            model.PostLevel = item.A01048 ?? "";
                            model.ProbationEnd = Convert.ToDateTime(string.IsNullOrEmpty(item.A0144) ? "2000-01-01" : item.A0144);
                            model.ProbationPayGrade = item.A01202 ?? "";
                            model.PostName = item.GW_NAME ?? "";
                            model.ProbationStart = Convert.ToDateTime(string.IsNullOrEmpty(item.A01199) ? "2000-01-01" : item.A01199);
                            model.RealEstateValuers = item.A01121 ?? "";
                            model.Realname = item.A0101 ?? "";
                            model.Ename = item.A0101 ?? "";
                            model.SalaryFulfilsSystem = item.A01099 ?? "";//去掉2013-11-7
                            model.SalarySeries = item.A01263 ?? "";
                            model.SectionLeader = item.A01157 ?? "";
                            model.Sex = (item.A0107 ?? "男") == "男" ? 0 : 1;
                            model.StaffType = item.A01147 ?? "";
                            model.SubordinateDept = item.A01261 ?? "";
                            model.SubordinateSubstation = item.A012162 ?? "";
                            model.Telephone = item.A0143 ?? "";
                            model.UserOldId = item.A0189 ?? "";
                            model.VDeptId = item.vdept_id ?? "";
                            model.UserType = item.A0191 ?? "";
                            model.IsMain = model.SubordinateSubstation.Contains("上海") ? 0 : 1;

                            //免修的时候用到的
                            model.JoinDate = Convert.ToDateTime(string.IsNullOrEmpty(item.A01047) ? "2000-01-01" : item.A01047);
                            model.CPACreateDate = Convert.ToDateTime(string.IsNullOrEmpty(item.A01115) ? "2000-01-01" : item.A01115);
                            //model.IsNew = model.IsNew;
                            model.IsNew = 0;
                            //添加判断是否为新进员工--by huihh
                            if (string.Equals(model.DeptName, "待分配部门") && model.IsHistoryNew != 2)
                            {
                                model.IsNew = 1;
                                model.IsInternExp = 0;
                                model.IsOurIntern = 0;
                                model.TrainGrade = model.TrainGrade == "T" ? "T" : model.TrainGrade;
                                model.OldTrainGrade = "T";
                                model.IsHistoryNew = 1;
                                model.NewYear = DateTime.Now.Year;
                            }

                            //model.Status = 0;
                            //姓名或者域帐号为空，帐号直接冻结
                            if (string.IsNullOrWhiteSpace(item.A0101) || string.IsNullOrWhiteSpace(item.A01155))
                                model.Status = 2;
                            else
                                model.Status = model.Status == 2 ? 0 : (model.Status ?? 0);

                            if (userChange.modifyType == "updated")
                            {
                                userBL.Update(model);
                            }
                            else if (userChange.modifyType == "deleted")
                            {
                                model.IsDelete = 1;
                                userBL.Update(model);
                            }
                            else if (userChange.modifyType == "inserted")
                            {
                                var traingrade = "T";
                                switch (item.A01187)
                                {
                                    case "1":
                                        traingrade = "A";
                                        break;
                                    case "2":
                                        traingrade = "B";
                                        break;
                                    case "3":
                                        traingrade = "C";
                                        break;
                                    case "4":
                                        traingrade = "D";
                                        break;
                                    case "5":
                                        traingrade = "N/A";
                                        break;
                                    default:
                                        traingrade = string.IsNullOrEmpty(item.A01187) ? "T" : item.A01187;
                                        break;
                                }
                                model.TrainGrade = traingrade;
                                model.OldTrainGrade = traingrade;
                                //添加判断是否为新进员工--by huihh
                                if (string.Equals(model.DeptName, "待分配部门"))
                                {
                                    model.TrainGrade = "T";
                                    model.OldTrainGrade = "T";
                                }
                                model.Password = "E99A18C428CB38D5F260853678922E03";
                                userBL.Add(model);
                            }

                            da.ExecuteSql(string.Format("UPDATE {0} SET   sys1=0 WHERE tId = '{1}'", ConfigurationManager.AppSettings["HRUserChangeTable"], userChange.tId));
                        }
                        catch
                        {

                        }
                    }
                    #endregion
                }
                else
                {
                    #region 手动执行，从用户表中，直接更新数据
                    foreach (var item in hrUserList)
                    {
                        try
                        {
                            var model = userList.Find(p => p.JobNum == item.A0188);
                            var deptTmp = deptList.Find(p => p.OldDeptId == Convert.ToInt32(item.DEPT_ID));
                            if (item == null)
                                continue;
                            if (model == null)
                                model = new LiXinModels.User.Sys_User();

                            //维护薪酬级别
                            if (!string.IsNullOrEmpty(item.A01105))
                            {
                                if (payGradeList.Find(p => p.GradeName == item.A01105) == null)
                                {
                                    var tmp = new LiXinModels.CourseManage.Sys_PayGrade
                                                  {
                                                      GradeName = item.A01105
                                                  };
                                    payGradeBL.AddSys_PayGrade(tmp);
                                    if (tmp.Id > 0)
                                        payGradeList.Add(tmp);
                                }
                            }

                            model.Username = item.A01155 ?? "";
                            model.ApprovalRole = item.A01077 ?? "";
                            //  model.CPA = string.IsNullOrEmpty(item.A01111) ? "否" : (item.A01111 == "是" ? "是" : "否");
                            model.CPANo = item.A01113 ?? "";
                            model.CPAPractice = item.A01189 ?? "";
                            model.CPARelationship = item.A01260 ?? "";
                            model.CPAStatus = item.A01158 ?? "";
                            model.CPA = model.CPAStatus == "执业" && model.CPARelationship != "--" && model.CPARelationship != "立信所外" ? "是" : "否";
                            model.CPV = item.A01119 ?? "";
                            model.CTA = item.A01118 ?? "";
                            model.LoginId = item.A01155 ?? "";

                            model.DeptName = deptTmp == null ? "" : deptTmp.DeptName;
                            model.DeptCode = deptTmp == null ? "" : deptTmp.DeptCode;
                            model.DeptId = deptTmp == null ? 0 : deptTmp.DepartmentId;
                            model.DeptPath = deptTmp == null ? "" : deptTmp.DeptPath;

                            model.Email = item.Email ?? "";
                            model.ForeignOtherQualification = item.A01123 ?? "";//去掉2013-11-7
                            model.GraduateSchool = item.A0160 ?? "";
                            model.GroupMobileNum = item.A01149 ?? "";
                            model.ICLV = item.A01120 ?? "";
                            model.IdCardNo = item.A0177 ?? "";
                            model.JobNum = item.A0188 ?? "";
                            model.JobTitle = item.A01042 ?? "";
                            model.LeaderID = item.A011572 ?? "";
                            model.Major = item.A0161 ?? "";
                            model.MobileNum = item.MobileTel ?? "";
                            model.OtherProfessional = item.A01122 ?? "";
                            model.OtherProfessionsJobTitle = item.A01128 ?? "";
                            model.PayGrade = item.A01105 ?? (string.IsNullOrEmpty(model.PayGrade) ? model.PayGrade : "");
                            model.PostGrade = item.A01088 ?? "";//去掉2013-11-7
                            model.PostLevel = item.A01048 ?? "";//去掉2013-11-7
                            model.ProbationEnd = Convert.ToDateTime(string.IsNullOrEmpty(item.A0144) ? "2000-01-01" : item.A0144);//去掉2013-11-7
                            model.ProbationPayGrade = item.A01202 ?? "";//去掉2013-11-7
                            model.PostName = item.GW_NAME ?? "";
                            model.ProbationStart = Convert.ToDateTime(string.IsNullOrEmpty(item.A01199) ? "2000-01-01" : item.A01199);//去掉2013-11-7
                            model.RealEstateValuers = item.A01121 ?? "";
                            model.Realname = item.A0101 ?? "";
                            model.Ename = item.A0101 ?? "";
                            model.SalaryFulfilsSystem = item.A01099 ?? "";
                            model.SalarySeries = item.A01263 ?? "";
                            model.SectionLeader = item.A01157 ?? "";
                            model.Sex = (item.A0107 ?? "男") == "男" ? 0 : 1;
                            model.StaffType = item.A01147 ?? "";
                            model.SubordinateDept = item.A01261 ?? "";
                            model.SubordinateSubstation = item.A012162 ?? "";
                            model.Telephone = item.A0143 ?? "";
                            model.UserOldId = item.A0189 ?? "";
                            model.VDeptId = item.vdept_id ?? "";
                            model.UserType = item.A0191 ?? "";
                            model.IsMain = model.SubordinateSubstation.Contains("上海") ? 0 : 1;

                            //免修的时候用到的
                            model.JoinDate = Convert.ToDateTime(string.IsNullOrEmpty(item.A01047) ? "2000-01-01" : item.A01047);
                            model.CPACreateDate = Convert.ToDateTime(string.IsNullOrEmpty(item.A01115) ? "2000-01-01" : item.A01115);
                            //model.IsNew = model.IsNew;
                            model.IsNew = 0;
                            //添加判断是否为新进员工--by huihh
                            if (string.Equals(model.DeptName, "待分配部门") && model.IsHistoryNew !=2)
                            {
                                model.IsNew = 1;
                                model.IsInternExp = 0;
                                model.IsOurIntern = 0;
                                model.TrainGrade = model.TrainGrade == "T" ? "T" : model.TrainGrade;
                                model.OldTrainGrade = "T";
                                model.IsHistoryNew = 1;
                                model.NewYear = DateTime.Now.Year;
                            }
                            //model.Status = 0;
                            //姓名或者域帐号为空，帐号直接冻结
                            if (string.IsNullOrWhiteSpace(item.A0101) || string.IsNullOrWhiteSpace(item.A01155))
                                model.Status = 2;
                            else
                                model.Status = model.Status == 2 ? 0 : (model.Status ?? 0);

                            if (model.UserId > 0)
                            {
                                userBL.Update(model);
                            }
                            else
                            {
                                var traingrade = "T";
                                switch (item.A01187)
                                {
                                    case "1":
                                        traingrade = "A";
                                        break;
                                    case "2":
                                        traingrade = "B";
                                        break;
                                    case "3":
                                        traingrade = "C";
                                        break;
                                    case "4":
                                        traingrade = "D";
                                        break;
                                    case "5":
                                        traingrade = "N/A";
                                        break;
                                    default:
                                        traingrade = string.IsNullOrEmpty(item.A01187) ? "T" : item.A01187;
                                        break;
                                }
                                model.TrainGrade = traingrade;
                                model.OldTrainGrade = traingrade;
                                model.Password = "E99A18C428CB38D5F260853678922E03";
                                //添加判断是否为新进员工--by huihh
                                if (string.Equals(model.DeptName, "待分配部门"))
                                {
                                    model.TrainGrade = "T";
                                    model.OldTrainGrade = "T";
                                }
                                userBL.Add(model);
                            }
                        }
                        catch
                        {
                        }
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region 初始同步，执行添加操作
                foreach (var item in hrUserList)
                {
                    try
                    {
                        //维护薪酬级别
                        if (!string.IsNullOrEmpty(item.A01105))
                        {
                            if (payGradeList.Find(p => p.GradeName == item.A01105) == null)
                            {
                                var tmp = new LiXinModels.CourseManage.Sys_PayGrade
                                              {
                                                  GradeName = item.A01105
                                              };
                                payGradeBL.AddSys_PayGrade(tmp);
                                if (tmp.Id > 0)
                                    payGradeList.Add(tmp);
                            }
                        }

                        var traingrade = "T";
                        switch (item.A01187)
                        {
                            case "1":
                                traingrade = "A";
                                break;
                            case "2":
                                traingrade = "B";
                                break;
                            case "3":
                                traingrade = "C";
                                break;
                            case "4":
                                traingrade = "D";
                                break;
                            case "5":
                                traingrade = "N/A";
                                break;
                            default:
                                traingrade = string.IsNullOrEmpty(item.A01187) ? "T" : item.A01187;
                                break;
                        }
                        //姓名或者域帐号为空，帐号直接冻结
                        var userstatus = 0;
                        if (string.IsNullOrWhiteSpace(item.A0101) || string.IsNullOrWhiteSpace(item.A01155))
                            userstatus = 2;
                        var deptTmp = deptList.Find(p => p.OldDeptId == Convert.ToInt32(item.DEPT_ID));
                        var model = new LiXinModels.User.Sys_User
                        {
                            Username = item.A01155 ?? "",//缺少
                            Password = "E99A18C428CB38D5F260853678922E03",
                            Status = userstatus,

                            ApprovalRole = item.A01077 ?? "",//去掉2013-6-8
                            CPANo = item.A01113 ?? "",//缺少
                            CPAPractice = item.A01189 ?? "",
                            CPARelationship = item.A01260 ?? "",//去掉2013-6-8
                            CPAStatus = item.A01158 ?? "",
                            CPA = item.A01158 == "执业" && item.A01260 != "--" && item.A01260 != "立信所外" ? "是" : "否",

                            CPV = item.A01119 ?? "",//去掉2013-11-7
                            CTA = item.A01118 ?? "",//去掉2013-11-7
                            LoginId = item.A01155 ?? "",//缺少

                            DeptName = deptTmp == null ? "" : deptTmp.DeptName,
                            DeptCode = deptTmp == null ? "" : deptTmp.DeptCode,
                            DeptId = deptTmp == null ? 0 : deptTmp.DepartmentId,
                            DeptPath = deptTmp == null ? "" : deptTmp.DeptPath,

                            Email = item.Email ?? "",
                            ForeignOtherQualification = item.A01123 ?? "",//去掉2013-6-8
                            GraduateSchool = item.A0160 ?? "",
                            GroupMobileNum = item.A01149 ?? "",
                            ICLV = item.A01120 ?? "",//去掉2013-6-8
                            IdCardNo = item.A0177 ?? "",
                            JobNum = item.A0188 ?? "",//缺少
                            JobTitle = item.A01042 ?? "",
                            LeaderID = item.A011572 ?? "",
                            Major = item.A0161 ?? "",
                            MobileNum = item.MobileTel ?? "",
                            OtherProfessional = item.A01122 ?? "",//去掉2013-6-8
                            OtherProfessionsJobTitle = item.A01128 ?? "",//去掉2013-6-8
                            PayGrade = item.A01105 ?? "",
                            PostGrade = item.A01088 ?? "",//去掉2013-6-8
                            PostLevel = item.A01048 ?? "",//去掉2013-11-7
                            ProbationEnd =
                                Convert.ToDateTime(string.IsNullOrEmpty(item.A0144)
                                                       ? "2000-01-01"
                                                       : item.A0144),
                            ProbationPayGrade = item.A01202 ?? "",
                            PostName = item.GW_NAME ?? "",
                            ProbationStart =
                                Convert.ToDateTime(string.IsNullOrEmpty(item.A01199)
                                                       ? "2000-01-01"
                                                       : item.A01199),
                            RealEstateValuers = item.A01121 ?? "",//去掉2013-6-8
                            Realname = item.A0101 ?? "",
                            Ename = item.A0101 ?? "",
                            SalaryFulfilsSystem = item.A01099 ?? "",//去掉2013-6-8
                            SalarySeries = item.A01263 ?? "",//去掉2013-6-8
                            SectionLeader = item.A01157 ?? "",
                            Sex = (item.A0107 ?? "男") == "男" ? 0 : 1,
                            StaffType = item.A01147 ?? "",//去掉2013-6-8
                            SubordinateDept = item.A01261 ?? "",
                            SubordinateSubstation = item.A012162 ?? "",//去掉2013-6-8
                            Telephone = item.A0143 ?? "",
                            TrainGrade = traingrade,
                            OldTrainGrade = traingrade,
                            UserOldId = item.A0189 ?? "",//去掉2013-6-8
                            VDeptId = item.vdept_id ?? "",//去掉2013-6-8
                            UserType = item.A0191 ?? "",
                            IsDelete = 0,
                            IsMain = (item.A012162 ?? "").Contains("上海") ? 0 : 1
                        };
                        //免修的时候用到的
                        model.JoinDate = Convert.ToDateTime(string.IsNullOrEmpty(item.A01047) ? "2000-01-01" : item.A01047);
                        model.CPACreateDate = Convert.ToDateTime(string.IsNullOrEmpty(item.A01115) ? "2000-01-01" : item.A01115);

                        model.IsNew = 0;
                        //添加判断是否为新进员工--by huihh
                        if (string.Equals(model.DeptName, "待分配部门"))
                        {
                            model.IsNew = 1;
                            model.IsInternExp = 0;
                            model.IsOurIntern = 0;
                            model.TrainGrade = "T";
                            model.OldTrainGrade = "T";
                            model.IsHistoryNew = 1;
                            model.NewYear = DateTime.Now.Year;
                        }
                        userBL.Add(model);
                    }
                    catch
                    {

                    }
                }
                #endregion
            }
        }

        #endregion

        #region 考勤同步相关

        /// <summary>
        /// 同步人员的指纹信息
        /// </summary>
        public bool SyncUserFinger()
        {
            try
            {
                da = new SqlDataAccess(UserFingerConnectStr);
                //获取用户指纹信息
                var ufList =
                    BaseModel.GetModelsBySql<Sys_UserFinger>(
                        string.Format(
                            "select a.USERID,a.TITLE,a.SSN,a.NAME,b.TEMPLATE4 as FingerTemplate from USERINFO as a,TEMPLATE as b where a.USERID=b.USERID"),
                        da);
                foreach (var item in ufList)
                {
                    //维护培训系统中人员的指纹信息
                    var user = userBL.GetUserByHrID(item.TITLE);
                    if (user != null)
                    {
                        if (userBL.ExistUserFinger(user.UserId))
                        {
                            //已存在
                            userBL.UpdateUserFinger(new LiXinModels.User.Sys_UserFinger
                            {
                                NAME = user.Realname,
                                SSN = item.SSN,
                                FingerTemplate = item.FingerTemplate,
                                TITLE = item.TITLE,
                                USERID = user.UserId
                            }, 0);
                        }
                        else
                        {
                            //不存在
                            userBL.AddUserFinger(new LiXinModels.User.Sys_UserFinger
                            {
                                NAME = user.Realname,
                                SSN = item.SSN,
                                FingerTemplate = item.FingerTemplate,
                                FingerTemplate1 = "",
                                FingerTemplate2 = "",
                                TITLE = item.TITLE,
                                USERID = user.UserId
                            });
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 同步原培训系统人员的指纹信息
        /// </summary>
        public bool SyncTrainUserFinger()
        {
            try
            {
                da = new SqlDataAccess(TrainUserFingerConnectStr);
                //获取用户指纹信息
                var ufList = BaseModel.GetModelsBySql<TrainUserFinger>(string.Format("select RealName,FINGERPRINT1 as FingerPrint1,FINGERPRINT2 as FingerPrint2 from UserInfo"), da);

                //获取系统中的人员信息
                var userList = userBL.GetList(string.Format(" IsDelete=0 and IsTeacher<2 and Status=0"));

                foreach (var item in ufList)
                {
                    if (string.IsNullOrEmpty(item.FingerPrint1) && string.IsNullOrEmpty(item.FingerPrint2))
                    {
                        continue;
                    }
                    //维护培训系统中人员的指纹信息
                    var tempList1 = ufList.Where(p => p.RealName == item.RealName);
                    var tempList2 = userList.Where(p => p.Realname == item.RealName);
                    if (tempList1.Count() == 1 && tempList2.Count() == 1)
                    {
                        var user = userList.FirstOrDefault(p => p.Realname == item.RealName);
                        if (userBL.ExistUserFinger(user.UserId))
                        {
                            //已存在
                            userBL.UpdateUserFinger(new LiXinModels.User.Sys_UserFinger
                                                        {
                                                            FingerTemplate1 = item.FingerPrint1,
                                                            FingerTemplate2 = item.FingerPrint2,
                                                            USERID = user.UserId
                                                        }, 1);
                        }
                        else
                        {
                            //不存在
                            userBL.AddUserFinger(new LiXinModels.User.Sys_UserFinger
                                                     {
                                                         NAME = user.Realname,
                                                         SSN = "",
                                                         FingerTemplate1 = item.FingerPrint1,
                                                         FingerTemplate2 = item.FingerPrint2,
                                                         TITLE = "",
                                                         USERID = user.UserId
                                                     });
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 同步培训级别
        //同步培训级别
        public void SyncGrade()
        {
            da = new SqlDataAccess(TrainGradeSqlConnectStr);
            //hr中的用户数据
            var trainGradeList = BaseModel.GetModelsBySql<Employee>("SELECT NAME,type_name FROM EMPLOYEE", da);

            //系统中用户数据
            var userList = userBL.GetList("Sys_User.IsDelete = 0");

            var grade = new List<string>();
            foreach (var item in trainGradeList)
            {
                if (!grade.Contains(item.type_name))
                {
                    grade.Add(item.type_name);
                    gradeBL.InsertGrade(item.type_name);
                }
                var user = userList.FirstOrDefault(p => p.Realname == item.Name);
                if (user != null)
                {
                    user.TrainGrade = item.type_name;
                    userBL.Update(user);
                }
            }

        }

        #endregion
    }
}
