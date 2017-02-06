using LiXinBLL.DepCourseManage;
using LiXinDataAccess;
using LiXinDataAccess.DepAttendce;
using LiXinDataAccess.DepCourseManage;
using LiXinDataAccess.Report_Together;
using LiXinInterface.DepCourseManage;
using LiXinInterface.Report_Together;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using LiXinDataAccess.DeptPlanManage;
using LiXinDataAccess.Examination;
using LiXinModels.Examination.DBModel;
using MongoDB.Driver.Builders;
using LiXinModels.Report_Vedio;
using Retech.Core.Cache;
using Retech.Cache;
using System.Diagnostics;
using LiXinDataAccess.SystemManage;
using LiXinBLL.SystemManage;
using LiXinModels.DepAttendce;
using LiXinModels;


namespace LiXinBLL.Report_Together
{
    public class Report_DepCourseBL : IReport_DepCourse
    {
        private Report_DepCourseDB ireport_depcourse = new Report_DepCourseDB();
        private Dep_CourseDB idep_course = new Dep_CourseDB();
        private Dep_AttendceApprovalRecordDB iDep_AttendceApprovalRecordDB = new Dep_AttendceApprovalRecordDB();
        private Dep_CourseOrderDB iDep_CourseOrderDB = new Dep_CourseOrderDB();
        private VedioCourseLearnDB iVedioCourseLearnDB = new VedioCourseLearnDB();
        private Dep_YearPlanDB iDep_YearPlanDB = new Dep_YearPlanDB();
        private ExaminationDB examDB = new ExaminationDB();
        private zAttendceDB zaDb = new zAttendceDB();

        protected ICacheManager cacheManager = new MemoryCacheManager();

        private Report_FreeDB freeDB = new Report_FreeDB();
        #region 完成情况


        public List<Report_DepCourse> GetReport_Complete(int ConfigValue, int year, string whereDeptID = "", string depwhere = " 1=1", int deptMaxScore = 0)
        {

            var dep_course_list = idep_course.GetDep_CourseList(year);

            //考勤审批
            var list = ireport_depcourse.GetReport_Complete(year, whereDeptID);

            //计划课程 统计年度课程和学时
            var year_list = idep_course.GetYearCourse(year);
            //计算个人学时
            var get_list = iDep_CourseOrderDB.GetDep_CourseOrderList();

            var attend_list = iDep_AttendceApprovalRecordDB.GetDep_AttendceApprovalRecordListNew(year);

            var dep_yearplan = idep_course.GetDepYearListByWhere(" year=" + year);

            #region 去除免修
            var FreeList = freeDB.GetFreeUserList("  Year=" + year);
            var freeconfig = new Sys_ParamConfigBL().GetList(" ConfigType = 63 AND datename(year,LastUpdateTime)=" + year).FirstOrDefault();

            if (!(freeconfig == null || freeconfig.ConfigValue == ""))
            {
                var configvalue = freeconfig.ConfigValue.Split(';');
                var tDate = year + "-" + configvalue[0].Split(',')[0];
                var CPADate = year + "-" + configvalue[1].Split(',')[0];
                var userList = freeDB.GetUserByDate(tDate, CPADate);
                FreeList.AddRange(userList.Select(p => p.ApplyUserID).Distinct());
            }

            var Avg_length = ireport_depcourse.GetAvgCourseLength(year, deptMaxScore, string.Join(",", FreeList));
            #endregion

            /// <summary>
            /// 被联合的部门其主部门的名称
            /// </summary>
            var LinkDeptList = iDep_YearPlanDB.GetLinkDept();

            var attList = GetAvgLast(year, FreeList, depwhere);

            var ListStr = new List<string>();
            var newList = new List<Report_DepCourse>();
            foreach (var item in list)
            {
                var IsLink = false;
                var LinkDeptID = 0;
                //item.
                // item.DeptIds = string.Join(",", deptIdList);
                var attend = attList.Where(p => p.DeptId == item.DeptId);
                //有年度计划 未被删除 且发布
                if (item.IsDelete == 0 && item.PublishFlag == 1)
                {
                    #region
                    //统计年度计划
                    var dep_list_year = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year && p.CourseFrom == 0);

                    item.Dep_Course_Commencement = year_list.Where(p => p.DeptId == item.DeptId).Count();

                    item.Dep_Course_Commencement_length = year_list.Where(p => p.DeptId == item.DeptId).Sum(p => p.CourseLength);

                    var Sdep_yearplan = dep_yearplan.Where(p => p.DeptId == item.DeptId);
                    if (Sdep_yearplan.Count() != 0)
                    {
                        item.PlanTime = Sdep_yearplan.FirstOrDefault().LastUpdateTime.ToString("yyyy-MM-dd");

                        if (Sdep_yearplan.FirstOrDefault().IsOpen == 0)
                        {
                            item.PlanStr = "单体上报";
                        }
                        else if (Sdep_yearplan.FirstOrDefault().IsOpen == 1)
                        //else if (dep_yearplan.Where(p => p.DeptId == item.DeptId).FirstOrDefault().IsOpen == 1 && dep_yearplan.Where(p => p.DeptId == item.DeptId).FirstOrDefault().DeptId == item.DeptId)
                        {
                            item.PlanStr = "联合上报";
                        }
                        else
                        {
                            IsLink = true;
                            var deptname = LinkDeptList.Where(p => p.DepartmentId == item.DeptId);
                            LinkDeptID = deptname.FirstOrDefault().LinkDeptId;
                            item.PlanStr = "被联合上报(" + deptname.FirstOrDefault().DeptName + ")";
                        }
                    }
                    else
                    {
                        item.PlanTime = "N/A";
                        item.PlanStr = "N/A";
                    }
                    item.IsLink = IsLink;

                    item.IsComplete_length = attend_list.Where(p => p.DeptId == item.DeptId && p.ApprovalFlag == 1).Sum(p => p.CourseLength);

                    item.IsCompleteStr = item.IsComplete_length >= ConfigValue ? "是" : "否";

                    var dep_list = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year).ToList();
                    ///被联合的时候
                    var Linkdep_list = new List<Dep_Course>();
                    if (IsLink)
                    {
                        Linkdep_list = dep_course_list.Where(p => p.DeptId == LinkDeptID
                                                         && p.YearPlan == year).ToList();
                        var LinkYearPlanWai = Linkdep_list.Where(p => p.IsYearPlan == 0 && p.Publishflag == 1 && (p.Way == 1 || p.Way == 3));
                        item.IsLinkYearPlanWai = LinkYearPlanWai.Count();
                        item.IsLinkYearPlanWai_length = LinkYearPlanWai.Sum(p => p.CourseLength);

                        var LinkYearPlanNei = Linkdep_list.Where(p => p.IsYearPlan == 1 && p.Publishflag == 1 && (p.Way == 1 || p.Way == 3));
                        item.IsLinkYearPlanNei = LinkYearPlanNei.Count();
                        item.IsLinkYearPlanNei_length = LinkYearPlanNei.Sum(p => p.CourseLength);
                    }

                    item.ActualNum = dep_list.Where(p => p.Publishflag == 1 && (p.Way == 1 || p.Way == 3)).Count();

                    //计划外
                    var YearPlanWai = dep_list.Where(p => p.IsYearPlan == 0 && p.Publishflag == 1 && (p.Way == 1 || p.Way == 3));
                    item.IsYearPlanWai = YearPlanWai.Count();

                    item.IsYearPlanWai_length = YearPlanWai.Sum(p => p.CourseLength);

                    //计划内
                    var YearPlanNei = dep_list.Where(p => p.IsYearPlan == 1 && p.Publishflag == 1 && (p.Way == 1 || p.Way == 3));
                    item.IsYearPlanNei = YearPlanNei.Count();

                    item.IsYearPlanNei_length = YearPlanNei.Sum(p => p.CourseLength);



                    var SystemList = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year).ToList();
                    ///被联合的时候
                    var LinkSystemList = new List<Dep_Course>();
                    if (IsLink)
                    {
                        LinkSystemList = dep_course_list.Where(p => p.DeptId == LinkDeptID
                                                         && p.YearPlan == year).ToList();
                        //框架外
                        var LinkSystemWai = LinkSystemList.Where(p => (p.Way == 1 || p.Way == 3) && p.IsSystem == 0 && p.Publishflag == 1);
                        item.IsLinkSystemWai = LinkSystemWai.Count();
                        item.IsLinkSystemWai_length = LinkSystemWai.Sum(p => p.CourseLength);

                        //框架内
                        var LinkSystemNei = LinkSystemList.Where(p => p.IsSystem > 0 && p.Publishflag == 1 && (p.Way == 1 || p.Way == 3));
                        item.IsLinkSystemNei = LinkSystemNei.Count();
                        item.IsLinkSystemNei_length = LinkSystemNei.Sum(p => p.CourseLength);
                    }

                    //框架外
                    var SystemWai = SystemList.Where(p => (p.Way == 1 || p.Way == 3) && p.IsSystem == 0 && p.Publishflag == 1);
                    item.IsSystemWai = SystemWai.Count();

                    item.IsSystemWai_length = SystemWai.Sum(p => p.CourseLength);

                    //框架内
                    var SystemNei = SystemList.Where(p => p.IsSystem > 0 && p.Publishflag == 1 && (p.Way == 1 || p.Way == 3));
                    item.IsSystemNei = SystemNei.Count();

                    item.IsSystemNei_length = SystemNei.Sum(p => p.CourseLength);

                    //开放至全所
                    var OpenSub = SystemList.Where(p => p.Way == 2 && p.Publishflag == 1 && p.IsOpenOthers == 2);
                    item.IsOpenSub = OpenSub.Count();

                    item.IsOpenSub_length = OpenSub.Sum(p => p.CourseLength);

                    //item.UploadAttend_Num = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year && p.CourseFrom == 2 && p.ResourceCount>0).Count();

                    item.UploadRes_Num = SystemList.Where(p => (p.Way == 1 || p.Way == 3) && p.Publishflag == 1 && p.ResourceCount > 0).Count();

                    item.UploadAttend_Num = attend_list.Where(p => p.DeptId == item.DeptId && p.yearplan == year && p.ApprovalFlag == 1).Count();

                    item.ApprovalPass = attend_list.Where(p => p.DeptId == item.DeptId && p.ApprovalFlag == 1 && p.AttFlag == 1 && p.yearplan == year).Count();

                    item.ApprovalNoPass = attend_list.Where(p => p.DeptId == item.DeptId && p.ApprovalFlag == 1 && p.AttFlag == 0 && p.yearplan == year).Count();

                    //人均获取学时
                    if (Avg_length.Where(p => p.DeptId == item.DeptId).Count() != 0)
                    {
                        var t = Avg_length.Where(p => p.DeptId == item.DeptId).Select(p => p.Avg_Score).ToList();
                        item.Avg_GetLength = Convert.ToDecimal(ReportCommon.CalculateMedianDouble(t, 2));
                    }
                    item.Avg_Qq = attend.Any() ? attend.FirstOrDefault().Avg_Qq : 0;
                    item.Avg_Cq = attend.Any() ? attend.FirstOrDefault().Avg_Cq : 0;
                    item.Avg_Cd = attend.Any() ? attend.FirstOrDefault().Avg_Cd : 0;
                    #endregion
                }
                else
                {
                    #region

                    if (item.LinkDepart_Deptid != 0)
                    {
                        //var dep_list_year = dep_course_list.Where(p => p.DeptId == item.LinkDepart_Deptid && p.YearPlan == year && p.CourseFrom == 0);

                        if (dep_yearplan.Count() != 0)
                        {
                            if (dep_yearplan.Where(p => p.DeptId == item.LinkDepart_Deptid).Count() != 0)
                            {
                                item.PlanTime = item.PlanTime = dep_yearplan.Where(p => p.DeptId == item.LinkDepart_Deptid).FirstOrDefault().LastUpdateTime.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                item.PlanTime = "N/A";
                            }

                        }
                        else
                        {
                            item.PlanTime = "N/A";

                        }

                        IsLink = true;
                        var deptname = LinkDeptList.Where(p => p.DepartmentId == item.DeptId);
                        LinkDeptID = deptname.FirstOrDefault().LinkDeptId;

                        item.Dep_Course_Commencement_length = year_list.Where(p => p.DeptId == item.LinkDepart_Deptid).Sum(p => p.CourseLength);

                        item.PlanStr = "被联合上报(" + deptname.FirstOrDefault().DeptName + ")";

                        item.Dep_Course_Commencement = year_list.Where(p => p.DeptId == item.LinkDepart_Deptid).Count();
                    }
                    else
                    {
                        item.PlanTime = "N/A";

                        item.PlanStr = "N/A";

                        item.Dep_Course_Commencement = 0;

                        item.Dep_Course_Commencement_length = 0;

                    }

                    item.IsCompleteStr = attend_list.Where(p => p.DeptId == item.DeptId && p.ApprovalFlag == 1).Sum(p => p.CourseLength) >= ConfigValue ? "是" : "否";

                    item.IsComplete_length = attend_list.Where(p => p.DeptId == item.DeptId && p.ApprovalFlag == 1).Sum(p => p.CourseLength);

                    //计划内外的计算,如果是被联合的部门 还要讲主部门的加入
                    var dep_list = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year).ToList();
                    ///被联合的时候
                    var Linkdep_list = new List<Dep_Course>();
                    if (IsLink)
                    {
                        Linkdep_list = dep_course_list.Where(p => p.DeptId == LinkDeptID
                                                         && p.YearPlan == year).ToList();
                        var LinkYearPlanWai = Linkdep_list.Where(p => p.IsYearPlan == 0 && p.Publishflag == 1);
                        item.IsLinkYearPlanWai = LinkYearPlanWai.Count();
                        item.IsLinkYearPlanWai_length = LinkYearPlanWai.Sum(p => p.CourseLength);

                        var LinkYearPlanNei = Linkdep_list.Where(p => p.IsYearPlan == 1 && p.Publishflag == 1);
                        item.IsLinkYearPlanNei = LinkYearPlanNei.Count();
                        item.IsLinkYearPlanNei_length = LinkYearPlanNei.Sum(p => p.CourseLength);
                    }

                    item.ActualNum = dep_list.Where(p => p.Publishflag == 1 && (p.Way == 1 || p.Way == 3)).Count();

                    item.IsYearPlanWai = dep_list.Where(p => p.IsYearPlan == 0 && p.Publishflag == 1).Count();

                    item.IsYearPlanWai_length = dep_list.Where(p => p.IsYearPlan == 0 && p.Publishflag == 1).Sum(p => p.CourseLength);

                    item.IsYearPlanNei = dep_list.Where(p => p.IsYearPlan == 1 && p.Publishflag == 1).Count();

                    item.IsYearPlanNei_length = dep_list.Where(p => p.IsYearPlan == 1 && p.Publishflag == 1).Sum(p => p.CourseLength);


                    var SystemList = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year).ToList();

                    ///被联合的时候
                    var LinkSystemList = new List<Dep_Course>();
                    if (IsLink)
                    {
                        LinkSystemList = dep_course_list.Where(p => p.DeptId == LinkDeptID
                                                         && p.YearPlan == year).ToList();
                        //框架外
                        var LinkSystemWai = LinkSystemList.Where(p => (p.Way == 1 || p.Way == 3) && p.IsSystem == 0 && p.Publishflag == 1);
                        item.IsLinkSystemWai = LinkSystemWai.Count();

                        item.IsLinkSystemWai_length = LinkSystemWai.Sum(p => p.CourseLength);

                        //框架内
                        var LinkSystemNei = LinkSystemList.Where(p => p.IsSystem > 0 && p.Publishflag == 1 && (p.Way == 1 || p.Way == 3));
                        item.IsLinkSystemNei = LinkSystemNei.Count();

                        item.IsLinkSystemNei_length = LinkSystemNei.Sum(p => p.CourseLength);
                    }

                    item.IsSystemWai = SystemList.Where(p => p.IsSystem == 0 && p.Publishflag == 1).Count();

                    item.IsSystemWai_length = SystemList.Where(p => p.IsSystem == 0 && p.Publishflag == 1).Sum(p => p.CourseLength);

                    item.IsSystemNei = SystemList.Where(p => p.IsSystem > 0 && p.Publishflag == 1).Count();

                    item.IsSystemNei_length = SystemList.Where(p => p.IsSystem > 0 && p.Publishflag == 1).Sum(p => p.CourseLength);

                    item.IsOpenSub = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year && p.Way == 2 && p.Publishflag == 1 && p.IsOpenOthers == 2).Count();

                    item.IsOpenSub_length = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year && p.Way == 2 && p.Publishflag == 1 && p.IsOpenOthers == 2).Sum(p => p.CourseLength);

                    //item.UploadAttend_Num = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year && p.CourseFrom == 2 && p.ResourceCount>0).Count();

                    item.UploadRes_Num = dep_course_list.Where(p => p.DeptId == item.DeptId && p.YearPlan == year && p.Way == 1 && p.Publishflag == 1 && p.ResourceCount > 0).Count();

                    item.UploadAttend_Num = attend_list.Where(p => p.DeptId == item.DeptId && p.yearplan == year && p.ApprovalFlag == 1).Count();

                    item.ApprovalPass = attend_list.Where(p => p.DeptId == item.DeptId && p.ApprovalFlag == 1 && p.AttFlag == 1 && p.yearplan == year).Count();

                    item.ApprovalNoPass = attend_list.Where(p => p.DeptId == item.DeptId && p.ApprovalFlag == 1 && p.AttFlag == 0 && p.yearplan == year).Count();

                    if (Avg_length.Where(p => p.DeptId == item.DeptId).Count() != 0)
                    {
                        var t = Avg_length.Where(p => p.DeptId == item.DeptId).Select(p => p.Avg_Score).ToList();
                        item.Avg_GetLength = Convert.ToDecimal(ReportCommon.CalculateMedianDouble(t, 2));
                    }
                    item.Avg_Qq = attend.Any() ? attend.FirstOrDefault().Avg_Qq : 0;
                    item.Avg_Cq = attend.Any() ? attend.FirstOrDefault().Avg_Cq : 0;
                    item.Avg_Cd = attend.Any() ? attend.FirstOrDefault().Avg_Cd : 0;
                    #endregion
                }

                newList.Add(item);
            }


            return newList;
        }
        public List<Report_DepCourse> GetReport_Complete(int year, string deptid)
        {
            return ireport_depcourse.GetReport_Complete(year, deptid);
        }



        /// <summary>
        /// 算平均出勤率 组装数据
        /// </summary>
        /// <returns></returns>
        public List<Report_DepCourse> GetAvgLast(int year, List<int> userIDList, string depwhere = " 1=1")
        {
            var list = iDep_AttendceApprovalRecordDB.GetAttendce(year, depwhere);
            var newAllList = new List<Dep_AttendceApprovalRecord>();
            foreach (var item in list)
            {
                if (userIDList.Count(p => p == item.UserId) == 0)
                {
                    newAllList.Add(item);
                }
            }
            var TogetherNumber = cacheManager.Get("DepCourse_number", 1440, () => { return (new Report_DepCourseBL()).GetDepCourseUser(); });

            var newList = new List<Report_DepCourse>();
            var newDepList = new List<Report_DepCourse>();
            ///每个部门 所有课程的却缺勤 迟到 总人数
            foreach (var item in newAllList.Select(p => new { p.CourseId, p.DeptId, p.Way }).Distinct())
            {
                //var deptIDList = list.Where(p => p.DeptName == item.DeptName).Select(p => p.DeptId).Distinct();
                var number = TogetherNumber.Where(p => p.courseID == item.CourseId && p.DeptID == item.DeptId);
                var model = new Report_DepCourse();
                var single = newAllList.Where(p => p.DeptId == item.DeptId && p.CourseId == item.CourseId);
                var sumcount = single.Count();
                model.CourseId = item.CourseId;
                model.DeptId = item.DeptId;
                model.Qq_Num = single.Where(p => p.Status == 2).Count();
                model.Cd_Num = single.Where(p => p.Status == 3).Count();
                model.sumNum = single.Where(p => p.Status != 2).Count();
                model.Pation_OrderNum = number.Any() ? number.FirstOrDefault().count : 0;

                if (item.Way == 3)
                {
                    model.Avg_Cq = single.Count() == 0 ? 0 : Math.Round(Convert.ToDouble(single.Count() - model.Qq_Num) / Convert.ToDouble(single.Count()), 4, MidpointRounding.AwayFromZero);
                    model.Avg_Qq = single.Count() == 0 ? 0 : Math.Round(Convert.ToDouble(model.Qq_Num) / Convert.ToDouble(single.Count()), 4, MidpointRounding.AwayFromZero);
                    model.Avg_Cd = model.sumNum == 0 ? 0 : Math.Round(Convert.ToDouble(model.Cd_Num) / Convert.ToDouble(model.sumNum), 4, MidpointRounding.AwayFromZero);
                }
                else
                {
                    model.Avg_Cq = sumcount == 0 ? 0 : Math.Round(Convert.ToDouble(model.sumNum) / Convert.ToDouble(sumcount), 4, MidpointRounding.AwayFromZero);
                    model.Avg_Qq = sumcount == 0 ? 0 : Math.Round(Convert.ToDouble(model.Qq_Num) / Convert.ToDouble(sumcount), 4, MidpointRounding.AwayFromZero);
                    model.Avg_Cd = sumcount == 0 ? 0 : Math.Round(Convert.ToDouble(model.Cd_Num) / Convert.ToDouble(model.sumNum), 4, MidpointRounding.AwayFromZero);

                }
                newList.Add(model);
            }

            foreach (var DeptId in newList.Select(p => p.DeptId).Distinct())
            {
                var single = newList.Where(p => p.DeptId == DeptId);
                var model = new Report_DepCourse();
                model.DeptId = DeptId;
                model.Avg_Qq = ReportCommon.CalculateMedianDouble(single.Select(p => p.Avg_Qq).ToList(), 4);
                model.Avg_Cq = ReportCommon.CalculateMedianDouble(single.Select(p => p.Avg_Cq).ToList(), 4);
                model.Avg_Cd = ReportCommon.CalculateMedianDouble(single.Select(p => p.Avg_Cd).ToList(), 4);
                newDepList.Add(model);
            }
            return newDepList;

        }
        #endregion

        #region 学习情况明细


        /// <summary>
        /// 学习情况明细
        /// </summary>
        /// <param name="where"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetReport_CompleteDetail(string where, Sys_ParamConfig config, int year)
        {
            var dep_courseorder = iDep_CourseOrderDB.GetReport_CompleteDetail(year);

            var GetCheckUserList = ireport_depcourse.GetCheckUserList(where);

            var studentList = examDB.GetAllList<tbExamSendStudent>(Query.And(Query.EQ("SourceType", 5)), 1, 10, "UserID", "RelationID", "Number");

            studentList = studentList.Where(p => dep_courseorder.Select(t => t.CourseId).Contains(p.RelationID)).ToList();


            #region 免修
            var FreeList = freeDB.GetFreeUserList("  Year=" + year);
            if (!(config == null || config.ConfigValue == ""))
            {
                var configvalue = config.ConfigValue.Split(';');
                var tDate = year + "-" + configvalue[0].Split(',')[0];
                var CPADate = year + "-" + configvalue[1].Split(',')[0];
                var userList = freeDB.GetUserByDate(tDate, CPADate);
                FreeList.AddRange(userList.Select(p => p.ApplyUserID).Distinct());
            }
            #endregion

            foreach (var item in GetCheckUserList)
            {
                var list = dep_courseorder.Where(p => p.UserId == item.UserId).ToList();

                if (list.Count() != 0)
                {
                    //item.Dep_DeptName = list[0].DeptName;
                    //item.RealName = list[0].Realname;
                    //item.IsCPA = list[0].CPA;
                    item.CanYu = list.Where(p => p.Status != 0).Count();
                    item.GetLength = list.Where(p => p.AttendceApprovalFlag == 1).Sum(p => p.AttScore + p.ExamScore + p.EvlutionScore);
                    item.Qj_Num = list.Where(p => p.IsLeave == 1 && p.ApprovalFlag == 1).Count();
                    item.Td_Num = list.Where(p => p.OrderStatus == 0).Count();
                    item.Cd_Num = list.Where(p => p.Status == 3).Count();
                    item.Qq_Num = list.Where(p => p.Status == 2).Count();
                    item.Advice_Num = list.Where(p => p.AttendCount != 0).Count();
                    item.AfterAnswer_Num = list.Where(p => p.AfterCount != 0).Count();
                    item.OnlineTest_Num = studentList.Where(p => p.UserID == item.UserId && p.Number == 1).Count();
                    item.Research_Num = list[0].DeptSurveyCount;
                    item.IsFree = FreeList.Count(p => p == item.UserId) == 0 ? 0 : 1;
                }
            }

            return GetCheckUserList;
        }

        #endregion

        #region 师资情况

        /// <summary>
        /// 师资情况列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetReport_Teachers(Sys_ParamConfig config, string where = "", int year = 2013)
        {
            var avgAnswer = iDep_CourseOrderDB.GetAvgSubjectiveAnswer(year);

            var list = ireport_depcourse.GetReport_Teachers(where);

            #region 免修
            var FreeList = freeDB.GetFreeUserList("  Year=" + year);
            if (!(config == null || config.ConfigValue == ""))
            {
                var configvalue = config.ConfigValue.Split(';');
                var tDate = year + "-" + configvalue[0].Split(',')[0];
                var CPADate = year + "-" + configvalue[1].Split(',')[0];
                var userList = freeDB.GetUserByDate(tDate, CPADate);
                FreeList.AddRange(userList.Select(p => p.ApplyUserID).Distinct());
            }
            #endregion

            foreach (var item in list)
            {
                if (avgAnswer.Where(p => p.TeacherId == Convert.ToInt32(item.TeacherId)).Count() != 0)
                {
                    var t = avgAnswer.Where(p => p.TeacherId == Convert.ToInt32(item.TeacherId)).Select(p => p.SubjectiveAnswer).ToList();
                    item.Dep_Survey_ReplyAnswer = ReportCommon.CalculateMedianDouble(t, 2);
                }
                else
                {
                    item.Dep_Survey_ReplyAnswer = 0;
                }
                item.IsFree = FreeList.Count(p => p == Convert.ToInt32(item.TeacherId)) == 0 ? 0 : 1;
            }
            return list;
        }

        /// <summary>
        /// 讲师上课的课程
        /// </summary>
        /// <returns></returns>
        public List<Dep_Course> GetDepCourseSurvey(int teacher, string where = " 1=1")
        {
            var list = ireport_depcourse.GetDepCourseTeacher(teacher, where);
            var surveyList = ireport_depcourse.GetDepSurvey();

            foreach (var item in list)
            {
                if (item.IsPing == 0)
                {
                    item.Survey_TeacherScoreDouble = -1;
                }
                else
                {
                    var SurveyList = item.SurveyPaperId.Split(';').ToList();
                    item.Survey_TeacherScoreDouble = -1;
                    for (int i = 0; i < SurveyList.Count; i++)
                    {
                        if (SurveyList[i] != null && SurveyList[i] != "")
                        {
                            var type = SurveyList[i].Split(',')[0].StringToInt32();
                            var paperID = SurveyList[i].Split(',')[1].StringToInt32();
                            var userScore = 0.00;
                            var newList = surveyList.Where(p => p.ObjectID == item.Id);
                            if (newList.Count() == 0)
                            {
                                userScore = 0;
                            }
                            else
                            {
                                var first = newList.Where(p => p.ExampaperID == paperID).FirstOrDefault();
                                if (first != null)
                                {
                                    userScore = first.AveragePing;
                                }
                                else
                                {
                                    userScore = 0;
                                }
                            }
                            switch (type)
                            {

                                case 2://讲师
                                    item.Survey_TeacherScoreDouble = userScore;
                                    // item.Survey_CourseScore = "N/A";
                                    break;
                            }
                        }
                    }
                }
            }
            return list;
        }

        #endregion

        #region 各部门/分所单门课程报名、参与情况表

        /// <summary>
        /// 各部门/分所单门课程报名、参与情况表
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetSingleCoursePation(int courseid, string where, string dwhere = "1=1", string StartTime = "", string EndTime = "")
        {
            var newList = new List<Report_DepCourse>();

            #region 集中授课

            var list = ireport_depcourse.GetSingleCoursePation(courseid, where);


            if (list.Count() != 0)
            {

                var att_list = ireport_depcourse.GetSingleCourseAndAttend(courseid);

                var attned_single = zaDb.GetUserAttendByCourseid(courseid);

                var order_list = ireport_depcourse.GetSingleCourse_OrderNum(courseid);



                foreach (var item in list)
                {
                    if (newList.Where(p => p.DeptName == item.DeptName).Count() == 0)
                    {
                        var singlelist = list.Where(p => p.DeptName == item.DeptName);
                        var deptIDList = singlelist.Select(p => p.DeptId).Distinct();
                        var t = att_list.Where(p => deptIDList.Contains(p.DeptId)).ToList();

                        item.DeptIds = string.Join(",", deptIDList);
                        item.IsMain = 0;

                        var GetLengthList = t.Where(p => (p.OrderStatus == 1 || p.OrderStatus == 3) &&
                                                   (p.IsLeave == 0 || (p.IsLeave == 1 && p.ApprovalFlag != 1)
                                                   || (p.IsLeave == 1 && p.ApprovalFlag == 1 && p.ApprovalDateTime > p.ApprovalLimitTime))).Select(p => p.AllScore).ToList();
                        item.Avg_GetLength = Convert.ToDecimal(ReportCommon.CalculateMedianDouble(GetLengthList, 2));
                        //报名人数 只需预定
                        item.Enter_OrderNum = t.Where(p => (string.IsNullOrEmpty(StartTime) ? true : p.OrderTime >= Convert.ToDateTime(StartTime))
                                                   && (string.IsNullOrEmpty(EndTime) ? true : p.OrderTime <= Convert.ToDateTime(EndTime))
                                                   && (p.OrderStatus == 1 || p.OrderStatus == 3) &&
                                                   (p.IsLeave == 0 || (p.IsLeave == 1 && p.ApprovalFlag != 1)
                                                   || (p.IsLeave == 1 && p.ApprovalFlag == 1 && p.ApprovalDateTime > p.ApprovalLimitTime))
                                                   ).Count();
                        //      ((AttFlag=1 and StartTime!=) 
                        //or (AttFlag=2 and EndTime!='2000-1-1') 
                        //or (AttFlag=3 and (StartTime!='2050-1-1' or EndTime!='2000-1-1')))

                        //实际参加人数 有考勤
                        item.Actual_OrderNum = t.Where(p => p.attID > 0 && (
                            (p.AttFlag == 1 && p.Attend_StartTime != Convert.ToDateTime("2050-1-1")) ||
                            (p.AttFlag == 2 && p.Attend_EndTime != Convert.ToDateTime("2000-1-1")) ||
                            (p.AttFlag == 3 && (p.Attend_StartTime != Convert.ToDateTime("2050-1-1") || p.Attend_EndTime != Convert.ToDateTime("2000-1-1")))
                            )).Count();
                        //最后添加
                        if (order_list.Where(p => deptIDList.Contains(p.DeptId)).Count() != 0)
                        {   //应参加人数
                            item.Pation_OrderNum = order_list.Where(p => deptIDList.Contains(p.DeptId)).Sum(p => p.Pation_OrderNum);
                        }
                        else
                        {
                            item.Pation_OrderNum = 0;
                        }
                        //请假人数
                        item.IsLeave = t.Where(p => p.IsLeave == 1 && p.ApprovalFlag == 1 && p.ApprovalDateTime <= p.ApprovalLimitTime)
                            .Where(p => (string.IsNullOrEmpty(StartTime) ? true : p.LeaveTime >= Convert.ToDateTime(StartTime))
                                                   && (string.IsNullOrEmpty(EndTime) ? true : p.LeaveTime <= Convert.ToDateTime(EndTime))).Count();

                        item.Un_OrderNum = t.Where(p => p.OrderStatus == 0)
                            .Where(p => (string.IsNullOrEmpty(StartTime) ? true : p.ApprovalDateTime >= Convert.ToDateTime(StartTime))
                                                   && (string.IsNullOrEmpty(EndTime) ? true : p.ApprovalDateTime <= Convert.ToDateTime(EndTime))).Count();

                        item.Bu_OrderNum = t.Where(p => p.OrderStatus == 3 && p.Bu_ApprovalFlag == 1).Count();

                        if (attned_single.Count() != 0)
                        {
                            var single = attned_single.Where(p => deptIDList.Contains(p.deptid));
                            item.Cd_Num = single.Any() ? single.Sum(p => p.co_cd) : 0;

                            item.Qq_Num = single.Any() ? single.Sum(p => p.co_qq) : 0;

                            item.Zt_Num = single.Any() ? single.Sum(p => p.co_zt) : 0;

                            item.Cd_Zt_Num = single.Any() ? single.Sum(p => p.co_cd_zt) : 0;
                        }
                        else
                        {
                            item.Cd_Num = 0;

                            item.Qq_Num = 0;

                            item.Zt_Num = 0;

                            item.Cd_Zt_Num = 0;
                        }
                        newList.Add(item);
                    }
                }
            }

            #endregion

            #region 转播课程
            var timewhere = "1=1";
            //AND OrderTime>={} AND OrderTime<=
            if (!string.IsNullOrEmpty(StartTime))
            {
                timewhere += " AND OrderTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                timewhere += " AND OrderTime<='" + EndTime + "'";
            }
            var RtList = ireport_depcourse.GetRTCourse(courseid, dwhere, where, timewhere);
            var RtAtt = ireport_depcourse.GetRTAttend(courseid);
            var courseIDlist = RtList.Select(p => new { p.DeptId, p.Enter_OrderNum, p.Actual_OrderNum, p.Pation_OrderNum }).Distinct();
            var liststr = new List<string>();
            foreach (var item in RtList)
            {
                if (!liststr.Contains(item.DeptName))
                {
                    var model = new Report_DepCourse();
                    liststr.Add(item.DeptName);
                    var single = RtList.Where(p => p.DeptName == item.DeptName);
                    var DeptIDList = single.Select(p => p.DeptId);
                    var courseinfo = courseIDlist.Where(p => DeptIDList.Contains(p.DeptId));
                    var att = RtAtt.Where(p => single.Select(t => t.UserId).Contains(p.UserId));
                    model.DeptIds = string.Join(",", DeptIDList);
                    model.DeptName = item.DeptName;
                    model.IsMain = 1;
                    model.Enter_OrderNum = courseinfo.Sum(p => p.Enter_OrderNum);
                    model.Actual_OrderNum = courseinfo.Sum(p => p.Actual_OrderNum);
                    model.Pation_OrderNum = courseinfo.Sum(p => p.Pation_OrderNum);
                    model.Avg_GetLength = Convert.ToDecimal(ReportCommon.CalculateMedianDouble(single.Select(p => p.GetRTScore).ToList(), 2));

                    //考勤
                    model.IsLeave = -1;//没有请假的概念
                    model.Bu_OrderNum = -1;//没有补预定的概念
                    model.Un_OrderNum = single.Where(p => p.OrderStatus == 0)
                        .Where(p => (string.IsNullOrEmpty(StartTime) ? true : p.OrderTime >= Convert.ToDateTime(StartTime))
                                                   && (string.IsNullOrEmpty(EndTime) ? true : p.OrderTime <= Convert.ToDateTime(EndTime))).Count();
                    if (att.Count() != 0)
                    {
                        //var single = att.Where(p => p.deptid == item.DeptId);
                        model.Cd_Num = single.Any() ? att.Where(p => p.Att_Status == 3).Count() : 0;

                        model.Qq_Num = single.Any() ? att.Where(p => p.Att_Status == 2).Count() : 0;

                        model.Zt_Num = single.Any() ? att.Where(p => p.Att_Status == 4).Count() : 0;

                        model.Cd_Zt_Num = single.Any() ? att.Where(p => p.Att_Status == 5).Count() : 0;
                    }
                    else
                    {
                        model.Cd_Num = 0;

                        model.Qq_Num = 0;

                        model.Zt_Num = 0;

                        model.Cd_Zt_Num = 0;
                    }
                    //model
                    newList.Add(model);
                }
            }
            #endregion
            return newList.Where(p => p.Actual_OrderNum > 0).ToList();
        }

        /// <summary>
        /// 薪酬级别
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetPayGrade(string where)
        {
            return ireport_depcourse.GetPayGrade(where);
        }
        #endregion

        #region 公共
        /// <summary>
        /// 实际开课数
        /// </summary>
        /// <param name="yearid"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetDep_Course_DeptNumFor_ShiJi(int yearid, string deptIDWhere = " 1=1")
        {
            return ireport_depcourse.GetDep_Course_DeptNumFor_ShiJi(yearid, deptIDWhere);
        }

        /// <summary>
        /// 年度计划开课
        /// </summary>
        /// <param name="yearid"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetDep_Course_DeptNumFor_YearPlan(int yearid, string deptIDWhere = " 1=1")
        {
            return ireport_depcourse.GetDep_Course_DeptNumFor_YearPlan(yearid, deptIDWhere);
        }

        /// <summary>
        /// 总所实际开设课程学时
        /// </summary>
        /// <param name="yearid"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetDep_Course_DeptNumFor_ShiJi_Length(int yearid, string deptIDWhere = " 1=1")
        {
            return ireport_depcourse.GetDep_Course_DeptNumFor_ShiJi_Length(yearid, deptIDWhere);
        }


        public List<Report_DepCourse> GetDep_Course_DeptNumFor_YearPlan_Length(int yearid, string deptIDWhere = " 1=1")
        {
            return ireport_depcourse.GetDep_Course_DeptNumFor_YearPlan_Length(yearid, deptIDWhere);
        }



        /// <summary>
        /// 每个部门分所应参加的人数
        /// </summary>
        /// <returns></returns>
        public List<fVedioJoinNumber> GetDepCourseUser()
        {
            return ireport_depcourse.GetDepCourseUser();
        }
        #endregion


    }


}
