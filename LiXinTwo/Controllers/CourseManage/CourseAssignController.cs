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
    public partial class CourseManageController : BaseController
    {
        public ActionResult CourseAssignUserList(int courseId)
        {
            var course = _courseBL.GetCo_Course(courseId);
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
            ViewBag.trainGrade = _sys_TrainBL.GetAllTrainGrade();
            ViewBag.CourseId = courseId;
            return View();
        }

        public ActionResult CourseDropAssignUserList(int courseId)
        {
            var course = _courseBL.GetCo_Course(courseId);
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
            ViewBag.trainGrade = _sys_TrainBL.GetAllTrainGrade();
            ViewBag.CourseId = courseId;
            return View();
        }

        /// <summary>
        /// 获取可指定人员的列表 
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userName"></param>
        /// <param name="trainGrade"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetCourseAssignUserList(int courseId, string userName, string deptname = "", string trainGrade = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "")
        {
            int totalCount = 0;
            string strWhere = " 1=1 ";

            var leaderId = CurrentUser.JobNum;
            if (CurrentUser.TrainMaster == 1 && CurrentUser.LeaderID != "")
                leaderId = CurrentUser.LeaderID;
            string where = string.Format(@"  sys_user.IsDelete = 0 AND co_course.ID = {0}
and ((sys_user.userid in (select userid from sys_group where groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
	or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null
)
or (sys_user.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
	or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null
)) 
and (sys_user.TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) or cl_courseorder.IsAppoint is not null)
  AND sys_user.LeaderID = '{1}' and (cl_courseorder.DropType=0 or cl_courseorder.DropType is NULL)
", courseId, leaderId);
            var listUserIds = _courseOrderBL.GetCourseSubscribeUserList(out totalCount, courseId, where, 1, int.MaxValue).Select(i => i.UserId);
            if (listUserIds.Count() > 0)
            {
                string temp = string.Join(",", listUserIds);
                if (!string.IsNullOrEmpty(temp))
                {
                    strWhere += " AND UserId Not In (select id from dbo.F_SplitIDs('" + temp + "'))";
                }

            }

            strWhere += " AND IsDelete=0  AND IsTeacher<2 AND Status = 0  AND LeaderID= '" + leaderId + "'";
            if (!string.IsNullOrEmpty(userName))
            {
                strWhere += " AND realName like '%" + userName.ReplaceSql() + "%' ";
            }
            if (!string.IsNullOrEmpty(deptname))
            {
                strWhere += " AND deptpath like '%" + deptname.ReplaceSql() + "%' ";
            }
            if (string.IsNullOrWhiteSpace(jsRenderSortField))
            {
                jsRenderSortField = "deptpath desc,trainGrade asc";
            }
            if (!string.IsNullOrEmpty(trainGrade))
            {
                string m_trainGrade = "";
                var str = trainGrade.Split(',');
                foreach (var item in str)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        m_trainGrade += "'" + item + "',";
                    }
                }
                if (m_trainGrade.Length > 0)
                {
                    m_trainGrade = m_trainGrade.Substring(0, m_trainGrade.Length - 1);
                }
                strWhere += " AND trainGrade in (" + m_trainGrade + ") ";
            }
            strWhere += " And UserType in ('在职','见习','特批','聘用')";
            int m_totalCount = 0;
            var listUser = _userBL.GetList(out m_totalCount, strWhere, (pageIndex - 1) * pageSize, pageSize, " order by " + jsRenderSortField);

            int n = 0;
            var itemArray = new object[listUser.Count()];
            foreach (var item in listUser)
            {
                var temp = new
                {
                    UserId = item.UserId,
                    Realname = item.Realname.HtmlXssEncode(),
                    TrainGrade = item.TrainGrade,
                    Telephone = item.MobileNum,
                    Emailstr = item.Emailstr.HtmlXssEncode(),
                    deptpath = item.DeptPath
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = m_totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取可撤销的指定人员的列表 
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userName"></param>
        /// <param name="trainGrade"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetCourseDropAssignUserList(int courseId, string userName, string deptname = "", string trainGrade = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "")
        {
            int totalCount = 0;
            string strWhere = " 1=1 ";

            var leaderId = CurrentUser.JobNum;
            if (CurrentUser.TrainMaster == 1 && CurrentUser.LeaderID != "")
                leaderId = CurrentUser.LeaderID;
            string where = string.Format(@"  sys_user.IsDelete = 0 AND co_course.ID = {0}
and ((sys_user.userid in (select userid from sys_group where groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
	or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null
)
or (sys_user.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
	or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null
)) 
and (sys_user.TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) or cl_courseorder.IsAppoint is not null)
  AND sys_user.LeaderID = '{1}' and cl_courseorder.IsAppoint=1 and cl_courseorder.IsLeave=0
", courseId, leaderId);
            var listUserIds = _courseOrderBL.GetCourseSubscribeUserList(out totalCount, courseId, where, 1, int.MaxValue).Select(i => i.UserId);
            if (listUserIds.Count() > 0)
            {
                string temp = string.Join(",", listUserIds);
                if (!string.IsNullOrEmpty(temp))
                {
                    strWhere += " AND UserId In (select id from dbo.F_SplitIDs('" + temp + "'))";
                }

            }

            strWhere += " AND IsDelete=0  AND IsTeacher<2 AND Status = 0  AND LeaderID= '" + leaderId + "'";
            if (!string.IsNullOrEmpty(userName))
            {
                strWhere += " AND realName like '%" + userName.ReplaceSql() + "%' ";
            }
            if (!string.IsNullOrEmpty(deptname))
            {
                strWhere += " AND deptpath like '%" + deptname.ReplaceSql() + "%' ";
            }
            if (string.IsNullOrWhiteSpace(jsRenderSortField))
            {
                jsRenderSortField = "deptpath desc,trainGrade asc";
            }
            if (!string.IsNullOrEmpty(trainGrade))
            {
                string m_trainGrade = "";
                var str = trainGrade.Split(',');
                foreach (var item in str)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        m_trainGrade += "'" + item + "',";
                    }
                }
                if (m_trainGrade.Length > 0)
                {
                    m_trainGrade = m_trainGrade.Substring(0, m_trainGrade.Length - 1);
                }
                strWhere += " AND trainGrade in (" + m_trainGrade + ") ";
            }
            strWhere += " And UserType in ('在职','见习','特批','聘用')";
            int m_totalCount = 0;
            var listUser = _userBL.GetList(out m_totalCount, strWhere, (pageIndex - 1) * pageSize, pageSize, " order by " + jsRenderSortField);

            int n = 0;
            var itemArray = new object[listUser.Count()];
            foreach (var item in listUser)
            {
                var temp = new
                {
                    UserId = item.UserId,
                    Realname = item.Realname.HtmlXssEncode(),
                    TrainGrade = item.TrainGrade,
                    Telephone = item.MobileNum,
                    Emailstr = item.Emailstr.HtmlXssEncode(),
                    deptpath = item.DeptPath
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = m_totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交指定人员至 课程预定表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="strUserIds"></param>
        /// <returns></returns>
         [Filter.SystemLog("提交指定人员至 课程预定表", LogLevel.Info)]
        public JsonResult SubmitAssignUser(int courseId, string strUserIds)
        {
            int page = 1;
            if (Session["cdspage"] != null)
            {
                string sess = Session["cdspage"].ToString();
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }
            if (!string.IsNullOrEmpty(strUserIds))
            {
                var course = _courseBL.GetCo_Course(courseId);
                if (course != null)
                {
                    //课程【特殊人群/指定】报名
                    _courseOrderBL.AddSpecialCrowdUserToCourse(courseId, course.CourseName, course.StartTime, course.EndTime, 1, strUserIds, AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble());

                    //var content = GetFormworkContent(1);
                    //if (!string.IsNullOrWhiteSpace(content))
                    //{
                    //    var userList = _userBL.GetList(" sys_user.userid in (select id from dbo.F_SplitIDs('" + strUserIds + "')) and sys_user.isteacher< 2 and sys_user.isdelete = 0 and (sys_user.realname is not null or sys_user.realname <> '')");
                    //    var messList = new List<KeyValuePair<string, string>>();
                    //    var emailList = new List<KeyValuePair<string, string>>();
                    //    for (int i = 0; i < userList.Count; i++)
                    //    {
                    //        if (!string.IsNullOrWhiteSpace(userList[i].Realname))
                    //        {
                    //            var c = string.Format(content,
                    //                                userList[i].Realname,
                    //                                CurrentUser.Realname,
                    //                                DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    //                                course.CourseName,
                    //                                course.StartTime.ToString("yyyy-MM-dd HH:mm"));
                    //            if (!string.IsNullOrWhiteSpace(userList[i].MobileNum))
                    //                messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, c));
                    //            if (!string.IsNullOrWhiteSpace(userList[i].Email))
                    //                emailList.Add(new KeyValuePair<string, string>(userList[i].Email, c));
                    //        }
                    //    }
                    //    if (messList.Count > 0)
                    //        SendMessage(messList);
                    //    if (emailList.Count > 0)
                    //        SendEmail(emailList, "有人给代报了课程，快来看看吧！");
                    //}
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
        /// 提交指定人员至 课程预定表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="strUserIds"></param>
        /// <param name="type"> </param>
        /// <param name="reason"> </param>
        /// <returns></returns>
        [Filter.SystemLog("提交指定人员至 课程预定表", LogLevel.Info)] 
        public JsonResult SubmitDropAssignUser(int courseId, string strUserIds, int type = 1, string reason = "")
        {
            int page = 1;
            if (Session["cdspage"] != null)
            {
                string sess = Session["cdspage"].ToString();
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }
            if (!string.IsNullOrEmpty(strUserIds))
            {
                var course = _courseBL.GetCo_Course(courseId);
                if (course != null)
                {
                    //撤销部门指定人员
                    _courseOrderBL.DropAssignUser(
                        string.Format(
                            "update Cl_CourseOrder set OrderStatus=0,DropType={2},DropReason='{3}',IsAppoint=0 where UserId in (select id from dbo.F_SplitIDs('{0}')) and CourseId={1} and IsAppoint=1", strUserIds, courseId, type, reason));

                    //var content = GetFormworkContent(8);
                    //if (!string.IsNullOrWhiteSpace(content))
                    //{
                    //    var userList = _userBL.GetList(" sys_user.userid in (select id from dbo.F_SplitIDs('" + strUserIds + "')) and sys_user.isteacher< 2 and sys_user.isdelete = 0 and (sys_user.realname is not null or sys_user.realname <> '')");
                    //    var messList = new List<KeyValuePair<string, string>>();
                    //    var emailList = new List<KeyValuePair<string, string>>();
                    //    foreach (var t in userList)
                    //    {
                    //        if (string.IsNullOrWhiteSpace(t.Realname)) continue;
                    //        var c = string.Format(content,t.Realname,course.CourseName);
                    //        if (!string.IsNullOrWhiteSpace(t.MobileNum))
                    //            messList.Add(new KeyValuePair<string, string>(t.MobileNum, c));
                    //        if (!string.IsNullOrWhiteSpace(t.Email))
                    //            emailList.Add(new KeyValuePair<string, string>(t.Email, c));
                    //    }
                    //    if (messList.Count > 0)
                    //        SendMessage(messList);
                    //    if (emailList.Count > 0)
                    //        SendEmail(emailList, "课程撤销指定通知");
                    //}
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

       


        #region 2013-9-16 是否确定发送短信
        /// <summary>
        /// 提交指定人员至 课程预定表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="strUserIds"></param>
        /// <returns></returns>
        public JsonResult SubmitAssignUserSendMessage(int courseId, string strUserIds, int type=0)
        {
            //int page = 1;
            //if (Session["cdspage"] != null)
            //{
            //    string sess = Session["cdspage"].ToString();
            //    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
            //    page = Convert.ToInt32(att[0]);
            //}
            if (!string.IsNullOrEmpty(strUserIds))
            {
                var course = _courseBL.GetCo_Course(courseId);
                if (course != null)
                {
                    //课程【特殊人群/指定】报名
                   // _courseOrderBL.AddSpecialCrowdUserToCourse(courseId, course.CourseName, course.StartTime, course.EndTime, 1, strUserIds, AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble());

                    var content = GetFormworkContent(1);
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var userList = _userBL.GetList(" sys_user.userid in (select id from dbo.F_SplitIDs('" + strUserIds + "')) and sys_user.isteacher< 2 and sys_user.isdelete = 0 and (sys_user.realname is not null or sys_user.realname <> '')");
                        var messList = new List<KeyValuePair<string, string>>();
                        var emailList = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < userList.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(userList[i].Realname))
                            {
                                var c = string.Format(content,
                                                    userList[i].Realname,
                                                    CurrentUser.Realname,
                                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                                                    course.CourseName,
                                                    course.StartTime.ToString("yyyy-MM-dd HH:mm"));
                                if (!string.IsNullOrWhiteSpace(userList[i].MobileNum))
                                    messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, type == 0 ? c : c.Replace("教育培训部", CurrentUser.DeptName)));
                                if (!string.IsNullOrWhiteSpace(userList[i].Email))
                                    emailList.Add(new KeyValuePair<string, string>(userList[i].Email, type == 0 ? c : c.Replace("教育培训部", CurrentUser.DeptName)));
                            }
                        }
                        if (messList.Count > 0)
                            SendMessage(messList);
                        if (emailList.Count > 0)
                            SendEmail(emailList, "有人给代报了课程，快来看看吧！");
                    }
                }
                return Json(new
                {
                    result = 1,
                    //indexpage = page
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 0
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 提交指定人员至 课程预定表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="strUserIds"></param>
        /// <param name="type"> </param>
        /// <param name="reason"> </param>
        /// <returns></returns>
        public JsonResult SubmitDropAssignUserSendMessage(int courseId, string strUserIds, int type = 1, string reason = "")
        {
            ////int page = 1;
            ////if (Session["cdspage"] != null)
            ////{
            ////    string sess = Session["cdspage"].ToString();
            ////    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
            ////    page = Convert.ToInt32(att[0]);
            ////}
            if (!string.IsNullOrEmpty(strUserIds))
            {
                var course = _courseBL.GetCo_Course(courseId);
                if (course != null)
                {
                    //撤销部门指定人员
                    //_courseOrderBL.DropAssignUser(
                    //    string.Format(
                    //        "update Cl_CourseOrder set OrderStatus=0,DropType={2},DropReason='{3}',IsAppoint=0 where UserId in (select id from dbo.F_SplitIDs('{0}')) and CourseId={1} and IsAppoint=1", strUserIds, courseId, type, reason));

                    var content = GetFormworkContent(8);
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var userList = _userBL.GetList(" sys_user.userid in (select id from dbo.F_SplitIDs('" + strUserIds + "')) and sys_user.isteacher< 2 and sys_user.isdelete = 0 and (sys_user.realname is not null or sys_user.realname <> '')");
                        var messList = new List<KeyValuePair<string, string>>();
                        var emailList = new List<KeyValuePair<string, string>>();
                        foreach (var t in userList)
                        {
                            if (string.IsNullOrWhiteSpace(t.Realname)) continue;
                            var c = string.Format(content, t.Realname, course.CourseName);
                            if (!string.IsNullOrWhiteSpace(t.MobileNum))
                                messList.Add(new KeyValuePair<string, string>(t.MobileNum, c));
                            if (!string.IsNullOrWhiteSpace(t.Email))
                                emailList.Add(new KeyValuePair<string, string>(t.Email, c));
                        }
                        if (messList.Count > 0)
                            SendMessage(messList);
                        if (emailList.Count > 0)
                            SendEmail(emailList, "课程撤销指定通知");
                    }
                }
                return Json(new
                {
                    result = 1,
                    //indexpage = page
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