using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.DepCourseLearn;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DeptPlanManage;
using LiXinInterface.DeptSurvey;
using LiXinInterface.Examination;
using LiXinInterface.Survey;
using LiXinInterface.User;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public partial class DepSelfCourseAppointSearchController : BaseController
    {
        #region == 接口实现 ==
        protected ICo_Course CoCourseBl;
        protected ICo_CoursePaper CoCoursePaperBl;

        protected IDep_Course DepCourseBl;
        protected IDep_CourseOrder DepCourseOrderBl;
        protected IDepCourseAdvice DepCourseAdviceBl;
        protected IDep_CourseResource DepCourseResourceBl;
        protected IDep_CoursePaper DepCoursePaperBl;

        protected IExampaper ExampaperBl;
        protected IExampaperSort ExampaperSortBl;
        protected IQuestion QuestionBl;
        protected IQuestionSort QuestionSortBl;
        protected IKnowledgeKey KnowledgeKeyBl;

        protected IDeptSurveyExampaper DepSurveyExampaperBl;
        protected IDepSurveyReplyAnswer DepSurveyReplyAnswerBl;
        protected IUser IUserBL;

        protected ICo_CourseResource CoCourseResource;
        protected ISurveyExampaper CoSurveyExampaperBl;

        protected IDep_YearPlan DepYearPlanBl;
        protected ISys_TrainGrade SysTrainGradeBl;
        protected IDepartment DepartmentBl;
        public DepSelfCourseAppointSearchController(
            ICo_Course coCourseBl
            ,ICo_CoursePaper coCoursePaperBl

            ,IDep_Course depCourseBl
            ,IDep_CourseOrder depCourseOrderBl
            ,IDepCourseAdvice depCourseAdviceBl
            ,IDep_CourseResource depCourseResourceBl
            ,IDep_CoursePaper depCoursePaperBl

            ,IExampaper exampaperBl
            ,IExampaperSort exampaperSortBl
            ,IQuestion questionBl
            ,IQuestionSort questionSortBl
            , IKnowledgeKey knowledgeKeyBl

            ,IDeptSurveyExampaper depSurveyExampaperBl
            ,IDepSurveyReplyAnswer depSurveyReplyAnswerBl
            ,IUser _IUserBL

            , ICo_CourseResource coCourseResource
            , ISurveyExampaper coSurveyExampaperBl

            , IDep_YearPlan depYearPlanBl
            , ISys_TrainGrade sysTrainGradeBl
            , IDepartment departmentBl)
        {
            CoCourseBl = coCourseBl;
            CoCoursePaperBl = coCoursePaperBl;

            DepCourseBl = depCourseBl;
            DepCourseOrderBl = depCourseOrderBl;
            DepCourseAdviceBl = depCourseAdviceBl;
            DepCourseResourceBl = depCourseResourceBl;
            DepCoursePaperBl = depCoursePaperBl;
            ExampaperBl = exampaperBl;
            ExampaperSortBl = exampaperSortBl;
            QuestionBl = questionBl;
            QuestionSortBl = questionSortBl;
            KnowledgeKeyBl = knowledgeKeyBl;
            DepSurveyExampaperBl = depSurveyExampaperBl;
            DepSurveyReplyAnswerBl = depSurveyReplyAnswerBl;
            IUserBL = _IUserBL;

            CoCourseResource = coCourseResource;
            CoSurveyExampaperBl = coSurveyExampaperBl;

            DepYearPlanBl = depYearPlanBl;
            SysTrainGradeBl = sysTrainGradeBl;
            DepartmentBl = departmentBl;
        }
        #endregion

        #region == 部门负责人部门指定查询 ==
        #region == 页面呈现 ==

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DepSelfCourseAppointSearch(string p)
        {
            ViewBag.currentDeptId = CurrentUser.DeptId;
            ViewBag.page = 1;
            ViewBag.courseName = "请输入搜索内容";
            ViewBag.teacherName = "请输入搜索内容";
            ViewBag.isOrder = -1;
            ViewBag.courseStatus = -1;
            if (!string.IsNullOrWhiteSpace(p) && p == "1")
            {
                if (Session["cdspage_DepSelfCourseAppointSearch"] != null)
                {
                    string sess = Session["cdspage_DepSelfCourseAppointSearch"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.courseName = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.teacherName = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.isOrder = att[3];
                    ViewBag.courseStatus = att[4];
                }
            }
            ViewBag.DeptidList = GetAllSubDepartments();
            return View();
        }


        //详情
        public ActionResult DepCourseAppointDetail(int id)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.model = DepCourseOrderBl.GetCourseById(id);
            return View();
        }

        
        #endregion

        #region == 方法 ==
        /// <summary>
        /// 部门负责人-获得部门指定查询列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="isOrder">课程性质2-指定 3-兼有 -1:指定+兼有</param>
        /// <param name="courseStatus">课程状态0-未开始 1-进行中 2-已结束 -1:全部</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetDepSelfCourseAppointSearchList(string courseName, string teacherName, int isOrder = -1
                                                         ,int courseStatus=-1,int deptid=0, int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " Id desc ")
        {
            if (Session["cdspage_DepSelfCourseAppointSearch"] != null)
            {
                Session.Remove("cdspage_DepSelfCourseAppointSearch");
            }
            Session["cdspage_DepSelfCourseAppointSearch"] = pageIndex + "㉿" + courseName + "㉿" + teacherName + "㉿" + isOrder + "㉿" + courseStatus ;
            var totalCount = 0;

            var openCourseList = DepCourseBl.DepSelfCourseAppointSearch(out totalCount, courseName, teacherName, isOrder,
                                                                        courseStatus,
                                                                        deptid,
                                                                        "", pageIndex, pageSize,
                                                                        " order by " + jsRenderSortField);

            //var openCourseList = DepCourseBl.DepSelfCourseAppointSearch(out totalCount, courseName, teacherName, isOrder,
            //                                                            courseStatus,
            //                                                            (CurrentUser == null ? 0 : CurrentUser.DeptId),
            //                                                            "", pageIndex, pageSize,
            //                                                            " order by " + jsRenderSortField);
            return Json(new
            {
                result = 1,
                dataList = openCourseList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询课程中学员的预订情况
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="realName">姓名</param>
        /// <param name="traingrade">培训级别</param>
        /// <param name="apply">报名状态</param>
        /// <param name="appoint">报名性质</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">索引</param>
        /// <param name="jsRenderSortField">排序字段</param>
        /// <returns></returns>
        public JsonResult GetDepCourseAppointUserList(int courseId, string realName, string traingrade, int apply, int appoint,
                                                       int pageSize =int.MaxValue, int pageIndex = 1, string jsRenderSortField = " u.UserId desc")
        {
            int totalCount = 0;
            string where = string.Format(@"  u.IsDelete = 0 and u.Status=0 AND cc.ID = {0} 
and u.DeptId in (
                        select ID from f_GetDeptChildByDeptID((select top 1 cc1.DeptId from Dep_Course cc1 where cc1.Id={0}))
						union
						select [Value] as ID from [SplitStringBySeparator]((select top 1 cc2.OpenDepartObject from Dep_Course cc2 where cc2.Id={0}), ',', 1)
                       ) --本部门及子部门+开放部门 
/* and u.TrainGrade in (
						select id from dbo.F_SplitIDs((select top 1 cc3.OpenLevel from Dep_Course cc3 where cc3.Id={0}))
					)--开放级别 
*/
and (dco.IsAppoint = {1} or {1} = 99 {3})
{2}
", courseId, appoint, (appoint == 0 ? " and dco.orderstatus <> 0 " : ""), (appoint == 2 ? " or dco.orderstatus=2 " : "")
);
            if (apply == 1)//已报名
            {  where += " and ((dco.orderstatus = 1 and dco.IsLeave = 0) or (dco.orderstatus = 1 and dco.IsLeave = 1 and dco.ApprovalFlag <> 1) or  dco.orderstatus = 2) ";}
            if (apply == 0)//未报名
            {  where += " and (dco.orderstatus is null) ";}
            if (apply == 2)//已请假
            {  where += " and (dco.orderstatus = 1 and dco.IsLeave = 1 and dco.ApprovalFlag = 1) ";}
            if (apply == 3)//已退订
            {  where += " and (dco.orderstatus = 0 ) ";}
            if (!string.IsNullOrWhiteSpace(traingrade))
            {   where +=
                    string.Format(
                        " AND u.TrainGrade in (select [Value] from [SplitStringBySeparator]('{0}', ',', 1))  and (u.traingrade is not null and u.traingrade <> '') ",
                        traingrade.Trim(','));
            }
            if (!string.IsNullOrWhiteSpace(realName))
            {  where += string.Format(" and u.RealName like '%{0}%'", realName.Trim().ReplaceSql());}
            var list = DepCourseOrderBl.GetDepCourseSubscribeUserList(out totalCount, courseId, where, (pageIndex - 1) * pageSize, pageSize, " order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    IsApply = item.IsApply,
                    UserId = item.UserId,
                    JobNum = item.JobNum,
                    Realname = item.Realname,
                    SexStr = item.SexStr,
                    TrainGrade = item.TrainGrade,
                    ApplyStr = item.CourseOrderStr,
                    ApplyPropertiesStr = item.CourseOrderStr != "已退订" ? item.DepApplyPropertiesStr : "——"
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        

        
    }

}
