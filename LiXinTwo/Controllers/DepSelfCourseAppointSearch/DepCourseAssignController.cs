using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels.CourseLearn;
using System.Text.RegularExpressions;
using LiXinModels.User;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public partial class DepSelfCourseAppointSearchController
    {
        #region == 页面呈现 ==
        /// <summary>
        /// 指定人员弹出框页面
        /// </summary>
        /// <param name="courseId">部门分所课程Id</param>
        /// <returns></returns>
        public ActionResult DepCourseAssignUserList(int courseId)
        {
            var course = DepCourseBl.GetCo_Course(courseId);
            if (course != null)
            {
                if (!string.IsNullOrWhiteSpace(course.OpenLevel))
                {
                    ViewBag.OpenLevel = course.OpenLevel.Split(',').ToList();
                }
                else
                {
                    ViewBag.OpenLevel = "";
                }
            }
            ViewBag.trainGrade = SysTrainGradeBl.GetAllTrainGrade();
            ViewBag.CourseId = courseId;
            return View();
        }

        /// <summary>
        /// 可撤销人员弹出页面
        /// </summary>
        /// <param name="courseId">部门分所课程Id</param>
        /// <returns></returns>
        public ActionResult DepCourseDropAssignUserList(int courseId)
        {
            var course = DepCourseBl.GetCo_Course(courseId);
            if (course != null)
            {
                if (!string.IsNullOrEmpty(course.OpenLevel))
                {
                    ViewBag.OpenLevel = course.OpenLevel.Split(',').ToList();
                }
                else
                {
                    ViewBag.OpenLevel = "";
                }
            }
            ViewBag.trainGrade = SysTrainGradeBl.GetAllTrainGrade();
            ViewBag.CourseId = courseId;
            return View();
        }
        #endregion

        #region == 方法 ==
        /// <summary>
        /// 部门分所负责人-课程指定查询-获取可指定人员的列表 
        /// </summary>
        /// <param name="courseId">部门分所课程ID</param>
        /// <param name="userName">姓名</param>
        /// <param name="deptname">部门名称</param>
        /// <param name="trainGrade">培训级别</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        public JsonResult GetDepCourseAssignUserList(int courseId, string userName, string deptname = "", string trainGrade = "", 
                                                               int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = "")
        {
            string strWhere = " 1=1 ";
            strWhere += string.Format(@" and (Sys_User.DeptId in (
                       /* select ID from f_GetDeptChildByDeptID((select top 1 DeptId from Dep_Course cc where cc.Id={0}))
						union */
						select [Value] as ID from [SplitStringBySeparator]((select top 1 isnull(OpenDepartObject,'') from Dep_Course cc where cc.Id={0}), ',', 1)
                       ) --开放部门 
or ((select cc.OpenDepartObject from Dep_Course cc where cc.Id={0}) is null and Sys_User.DeptId = {1})
)
      and Sys_User.UserId not in (
							select UserId 
							from Dep_CourseOrder dco 
							where dco.CourseId = {0} 
								  And (   
										 (dco.orderstatus = 1 and dco.isleave = 0 )
										  or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
									  ) 
							)--去除已预订+指定的人员 
      and Sys_User.TrainGrade not in (
									select id from dbo.F_SplitIDs((select top 1 OpenLevel from Dep_Course cc where cc.Id={0}))
								  )--开放级别 
      ", courseId,CurrentUser.DeptId);

            strWhere += " AND Sys_User.IsDelete=0  AND Sys_User.IsTeacher<2  AND Sys_User.Status = 0  ";
            if (!string.IsNullOrWhiteSpace(userName))
            {
                strWhere += " AND Sys_User.Realname like '%" + userName.Trim().ReplaceSql() + "%' ";
            }
            if (!string.IsNullOrWhiteSpace(deptname))
            {
                strWhere += " AND DeptPath like '%" + deptname.Trim().ReplaceSql() + "%' ";
            }
            if (string.IsNullOrWhiteSpace(jsRenderSortField))
            {
                jsRenderSortField = " DeptPath desc,TrainGrade asc ";
            }
            if (!string.IsNullOrWhiteSpace(trainGrade))
            {
                strWhere += string.Format(" AND TrainGrade in (select [Value] from [SplitStringBySeparator]('{0}', ',', 1)) ",trainGrade);
            }
            strWhere += " And UserType in ('在职','见习','特批','聘用') ";
            int m_totalCount = 0;
            var listUser = IUserBL.GetList(out m_totalCount, strWhere, (pageIndex - 1) * pageSize, pageSize, " order by " + jsRenderSortField);
            return Json(new
            {
                result = 1,
                dataList = listUser,
                recordCount = m_totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取可撤销的指定人员的列表 
        /// </summary>
        /// <param name="courseId">部门分所课程ID</param>
        /// <param name="userName">姓名</param>
        /// <param name="deptname">部门名称</param>
        /// <param name="trainGrade">培训级别</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        public JsonResult GetCourseDropAssignUserList(int courseId, string userName, string deptname = "", string trainGrade = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "")
        {
            string strWhere = " 1=1 ";
            strWhere += string.Format(@" and Sys_User.UserId in (
							select UserId 
							from Dep_CourseOrder dco 
							where dco.CourseId ={0} and dco.IsAppoint=1 --指定
								  And (   
										 (dco.orderstatus = 1 and dco.isleave = 0 )
										  or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
									  ) 
							)--指定的人员  
                             ", courseId);

            strWhere += " AND Sys_User.IsDelete=0  AND Sys_User.IsTeacher<2  AND Sys_User.Status = 0  ";
            if (!string.IsNullOrWhiteSpace(userName))
            {
                strWhere += " AND Sys_User.Realname like '%" + userName.Trim().ReplaceSql() + "%' ";
            }
            if (!string.IsNullOrWhiteSpace(deptname))
            {
                strWhere += " AND DeptPath like '%" + deptname.Trim().ReplaceSql() + "%' ";
            }
            if (string.IsNullOrWhiteSpace(jsRenderSortField))
            {
                jsRenderSortField = " DeptPath desc,TrainGrade asc ";
            }
            if (!string.IsNullOrWhiteSpace(trainGrade))
            {
                strWhere += string.Format(" AND TrainGrade in (select [Value] from [SplitStringBySeparator]('{0}', ',', 1)) ", trainGrade);
            }
            strWhere += " And UserType in ('在职','见习','特批','聘用') ";
            int m_totalCount = 0;
            var listUser = IUserBL.GetList(out m_totalCount, strWhere, (pageIndex - 1) * pageSize, pageSize, " order by " + jsRenderSortField);
            return Json(new
            {
                result = 1,
                dataList = listUser,
                recordCount = m_totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交指定人员至 课程预定表
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="strUserIds">选择的人员集合</param>
        /// <returns></returns>
        [Filter.SystemLog("提交指定人员至 课程预定表", LogLevel.Info)]
        public JsonResult SubmitAssignUser(int courseId, string strUserIds)
        {
            int page = 1;
            if (Session["cdspage_DepSelfCourseAppointSearch"] != null)
            {
                string sess = Session["cdspage_DepSelfCourseAppointSearch"].ToString();
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }
            if (!string.IsNullOrWhiteSpace(strUserIds))
            {
                var course = DepCourseBl.GetCo_Course(courseId);
                if (course != null)
                {
                    var newAddUserIdList=new List<string>();
                    //课程【指定人员】报名
                    DepCourseOrderBl.AddDepSpecialCrowdUserToCourse(courseId, 1, strUserIds, out newAddUserIdList);

                    var oldOpenPersonList = String.IsNullOrWhiteSpace(course.OpenPerson)
                                                ? (new List<string>())
                                                : course.OpenPerson.Trim(',').Split(',').ToList();
                    oldOpenPersonList.AddRange(newAddUserIdList);
                    string newOpenPerson = oldOpenPersonList.Distinct().Aggregate("", (current, id) => current + (id + ",")).Trim(',');
                    course.OpenPerson = newOpenPerson;
                    DepCourseBl.UpdateCourse(course);//同步更新指定人员字段

                }
                return Json(new
                {
                    result = 1,
                    indexpage=page
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 0
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 撤销指定人员 
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="strUserIds">选择的人员集合</param>
        /// <param name="type"> 撤销类型</param>
        /// <param name="reason">撤销理由 </param>
        /// <returns></returns>
        [Filter.SystemLog("撤销指定人员", LogLevel.Info)]
        public JsonResult SubmitDropAssignUser(int courseId, string strUserIds,int type=1, string reason="")
        {
            int page = 1;
            if (Session["cdspage_DepSelfCourseAppointSearch"] != null)
            {
                string sess = Session["cdspage_DepSelfCourseAppointSearch"].ToString();
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }
            if (!string.IsNullOrWhiteSpace(strUserIds))
            {
                var course = DepCourseBl.GetCo_Course(courseId);
                if (course != null)
                {
                    //撤销指定人员
                    DepCourseOrderBl.DepDropAssignUser(
                        string.Format(
                            @" update Dep_CourseOrder set OrderStatus=0,DropType={2},DropReason='{3}',IsAppoint=0 
 where UserId in (select id from dbo.F_SplitIDs('{0}')) and CourseId={1} and IsAppoint=1 ", strUserIds.Trim(','), courseId, type, reason));

                    var newRemoveUserIdList = strUserIds.Trim(',').Split(',').ToList();
                    var oldOpenPersonList = String.IsNullOrWhiteSpace(course.OpenPerson)
                                                ? (new List<string>())
                                                : course.OpenPerson.Trim(',').Split(',').ToList();
                    oldOpenPersonList=oldOpenPersonList.Where(p=>!newRemoveUserIdList.Contains(p)).ToList();
                    string newOpenPerson = oldOpenPersonList.Distinct().Aggregate("", (current, id) => current + (id + ",")).Trim(',');
                    course.OpenPerson = newOpenPerson;
                    DepCourseBl.UpdateCourse(course);//同步更新指定人员字段
                    
                }
                return Json(new
                {
                    result = 1,
                    indexpage = page
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 0
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}