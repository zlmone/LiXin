using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels.CourseManage;
using LiXinModels.CourseLearn;
using System.Web;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using LiXinModels;
using System.Configuration;
using LiXinModels.User;
using LiXinControllers.Filter;
using LiXinModels.SystemManage;

namespace LiXinControllers
{
    public partial class AttendceManageController : BaseController
    {
        #region 呈现页面

        /// <summary>
        ///  考勤列表呈现
        /// </summary>
        public ViewResult AttendceIndex(string p)
        {
            ViewBag.page = 1;
            ViewBag.Attway = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Attstate = 0;
            if (p != null && p != "" && p == "1")
            {
                if (Session["attpage"] != null)
                {
                    string sess = Session["attpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);

                    ViewBag.page = att[0];
                    ViewBag.Attway = att[1];
                    ViewBag.Attname = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.Attmust = att[3];
                    ViewBag.Attstate = att[4];
                    ViewBag.AttstartTime = att[5];
                    ViewBag.AttendTime = att[6];
                }
            }
            return View();
        }

        /// <summary>
        /// 考勤管理呈现
        /// </summary>
        /// <returns></returns>
        public ViewResult AttendceDetail(int id, string stat)
        {
            //int attSt=IAtt.GetAttStatus(id);
            //ViewData["attstat"] =Convert.ToString(attSt);
            return View();
        }

        /// <summary>
        /// 签到导入呈现
        /// </summary>
        public ViewResult ImportAttend()
        {
            return View();
        }

        /// <summary>
        /// 考勤录入呈现
        /// </summary>
        public ViewResult AttendceEdit()
        {
            return View();
        }

        #endregion

        #region 操作

        /// <summary>
        /// 根据way获取所有课程列表(有分页)
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAttendceList(int way, string name, string must, string isOpen, int state, string startTime, string endTime, int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            try
            {
                if (Session["attpage"] != null)
                {
                    Session.Remove("attpage");
                }
                Session["attpage"] = pageIndex + "㉿" + way + "㉿" + name + "㉿" + must + "㉿" + state + "㉿" + startTime + "㉿" + endTime;

                int totalCount = 0;
                if (!string.IsNullOrEmpty(startTime))
                {
                    where += string.Format(" and CONVERT(varchar(100),cc.StartTime,23) >= '{0}'", DateTime.Parse(startTime).ToString("yyyy-MM-dd"));
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += string.Format(" and CONVERT(varchar(100),cc.StartTime,23) <= '{0}'", DateTime.Parse(endTime).ToString("yyyy-MM-dd"));
                }

                if (!string.IsNullOrEmpty(must))
                {
                    if (!must.Contains(","))
                    {
                        where += string.Format(" and cc.IsMust ={0}", Convert.ToInt32(must));
                    }
                }
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and cc.CourseName LIKE '%{0}%'", name.ReplaceSql());
                }
                switch (state)
                {
                    case 0:
                        break;
                    case 1:
                        where += string.Format(" and cc.StartTime > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        break;
                    case 2:
                        where += string.Format(" and cc.StartTime <= '{0}' and cc.EndTime >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        break;
                    case 3:
                        where += string.Format(" and cc.EndTime < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        break;
                }

                List<Co_Course> attendceList = IAtt.GetAttendceList(way, out totalCount, pageIndex, pageSize, "desc", where);
                foreach (var item in attendceList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.RoomName = item.RoomName.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = attendceList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAttendUserList(int id, string uname, string dname, int CState, int OState, int OrderTime = 0, int pageSize = 20, int pageIndex = 1)
        {
            //补预定
            Sys_ParamConfig syscon = AllSystemConfigs.Where(p => p.ConfigType == 20).FirstOrDefault();

            string where = "1=1";
            string overwhere = " 1=1";
            string userIDs = "";
            if (OrderTime != 0)
            {
                var count = Convert.ToInt32(syscon.ConfigValue);
                //超过
                if (OrderTime == 1)
                {
                    overwhere += " and orderOverTime>=" + count;
                }
                else
                {
                    overwhere += " and orderOverTime<" + count;
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(uname))
                {
                    where += string.Format(" and su.Realname LIKE '%{0}%'", uname.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(dname))
                {
                    where += string.Format(" and sd.DeptName LIKE '%{0}%'", dname.ReplaceSql());
                }
                switch (CState)
                {
                    case 0:
                        break;
                    case 1:
                        where += string.Format(" AND su.CPA ='是' ");
                        break;
                    case 2:
                        where += string.Format(" AND su.CPA ='否' ");
                        break;
                }
                switch (OState)
                {
                    case 0:
                        break;
                    case 2:
                        where += string.Format(" AND cl.OrderStatus=3 ");
                        break;
                    case 1:
                        where += string.Format(" AND cl.OrderStatus=1 ");
                        break;
                }

                List<Cl_Attendce> attendceList = IAtt.GetAttendUserList(id, "asc", where, overwhere);
                List<Cl_Attendce> tempcu1 = attendceList.Where(p => p.OrderStatus == 1).ToList();
                List<Cl_Attendce> tempcu3 = attendceList.Where(p => p.OrderStatus == 3).ToList();
                //List<Cl_Attendce> tempAtt = attendceList.Where(p => p.StartTimeM != "" && p.EndTimeM != "").ToList();
                List<Cl_Attendce> tempAtt = attendceList.Where(p => (p.AttFlag == 1 && p.StartTimeM != "" && p.StartTime.Year != 2050) || (p.AttFlag == 2 && p.EndTimeM != "" && p.EndTime.Year != 2000) || (p.AttFlag == 3 && p.StartTimeM != "" && p.EndTimeM != "" && p.EndTime.Year != 2000 && p.StartTime.Year != 2050) || (p.AttFlag == 3 && p.StartTimeM != "" && p.StartTime.Year != 2050) || (p.AttFlag == 3 && p.EndTimeM != "" && p.EndTime.Year != 2000)).ToList();
                List<Cl_Attendce> tempAtt1 = tempAtt.Where(p => (Convert.ToDateTime(p.StartTime.ToString("yyyy-MM-dd HH:mm")) > Convert.ToDateTime(p.courseStart.ToString("yyyy-MM-dd HH:mm")) && p.StartTime.Year != 2050) || p.StartTime.Year == 2050).ToList();
                List<Cl_Attendce> tempAtt2 = tempAtt.Where(p => (Convert.ToDateTime(p.EndTime.ToString("yyyy-MM-dd HH:mm")) < Convert.ToDateTime(p.courseEnd.ToString("yyyy-MM-dd HH:mm")) && p.EndTime.Year != 2000) || p.EndTime.Year == 2000).ToList();
                foreach (var item in attendceList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.DeptName = item.DeptName.HtmlXssEncode();
                    item.actual = tempAtt.Count;
                    item.Ordersum = tempcu1.Count;
                    item.OrderOut = tempcu3.Count;
                    item.agosum = tempAtt1.Count;
                    item.lastsum = tempAtt2.Count;
                }

                List<Cl_Attendce> list1 = attendceList.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();
                return Json(new
                {
                    dataList = list1,
                    recordCount = attendceList.Count()
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0,
                    userID = userIDs
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 补预订
        /// </summary>
        /// <param name="cid">课程ID</param>
        /// <param name="uids">用户IDs</param>
        /// <returns></returns>
        [Filter.SystemLog("考勤管理 补预定", LogLevel.Info)]
        public JsonResult UpOrder(int cid, string uids)
        {
            try
            {
                if (!string.IsNullOrEmpty(uids))
                {
                    string sss = "";
                    string ssss = "";
                    var failedName = "";
                    //补预定
                    Sys_ParamConfig syscon = AllSystemConfigs.Where(p => p.ConfigType == 20).FirstOrDefault();
                    var userL = userBL.GetList();
                    if (uids.IndexOf(",") > -1)
                    {
                        string[] userList = Regex.Split(uids, ",", RegexOptions.IgnoreCase);
                        for (int i = 0; i < userList.Length; i++)
                        {

                            if (IAtt.Cl_MakeUpOrderCount(Convert.ToInt32(syscon.ConfigValue), Convert.ToInt32(userList[i]), DateTime.Now.Year))
                            {
                                IAtt.MakeUpOrder(cid, Convert.ToInt32(userList[i]));
                                ssss += userList[i] + ",";
                            }
                            else
                            {
                                sss += userList[i] + ",";
                                failedName = failedName + "，" + userL.Where(p => p.UserId == userList[i].StringToInt32()).FirstOrDefault().Realname;
                            }
                        }
                        if (sss != "" && ssss != "")
                        {
                            ssss = ssss.Remove(ssss.LastIndexOf(","), 1);
                            SendMess(cid, ssss, 7, "补预定申请", false);
                            return Json(new
                            {
                                result = 1,
                                content = "补预订成功,但部分用户：" + failedName.TrimEnd('，').TrimStart('，') + "，超出补预订次数限制"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else if (sss == "" && ssss != "")
                        {
                            ssss = ssss.Remove(ssss.LastIndexOf(","), 1);
                            SendMess(cid, ssss, 7, "补预定申请", false);
                            return Json(new
                            {
                                result = 1,
                                content = "补预订成功"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else if (sss != "" && ssss == "")
                        {
                            return Json(new
                            {
                                result = 0,
                                content = "超出补预订次数限制"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new
                            {
                                result = 0,
                                content = "补预订失败"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        if (IAtt.Cl_MakeUpOrderCount(Convert.ToInt32(syscon.ConfigValue), Convert.ToInt32(uids), DateTime.Now.Year))
                        {
                            IAtt.MakeUpOrder(cid, Convert.ToInt32(uids));
                            SendMess(cid, uids, 7, "补预定申请", false);
                            return Json(new
                            {
                                result = 1,
                                content = "补预订成功"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new
                            {
                                result = 0,
                                content = "超出补预订次数限制"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        result = 0,
                        content = "补预订失败"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = 0,
                    content = "补预订失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  导入签到表
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("考勤管理 导入签到表", LogLevel.Info)]
        public JsonResult SubmitImport(int id)
        {
            string filename = "";
            string resultName = "";
            string fcontent = "";
            string folder = ConfigurationManager.AppSettings["AttendceUrl"];
            HttpPostedFileBase excelfiles = Request.Files[0];
            if (null != excelfiles)
            {
                try
                {
                    filename = Path.GetFileName(excelfiles.FileName); //获得文件名
                    string fullPathname = Path.Combine(folder, filename);//文件后缀名
                    string suffix = excelfiles.FileName.Substring(excelfiles.FileName.LastIndexOf(".") + 1).ToLower();
                    resultName = Guid.NewGuid() + "." + suffix;
                    saveFile(excelfiles, folder, resultName);
                    List<DataTable> dtList = new Spreadsheet().LoadExcel(HttpContext.Server.MapPath(folder) + resultName);
                    if (dtList.Count > 0)
                    {
                        fcontent = IAtt.AddAttendces(id, dtList[0]);
                    }
                    return Json(new
                    {
                        result = 1,
                        content = fcontent
                    }, "text/html", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        result = 0,
                        content = "导入失败，模板中的数据存在错误，请更正后导入"
                    }, "text/html", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    result = 0,
                    content = "请选择要导入的数据模板"
                }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        public bool saveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            if (!Directory.Exists(HttpContext.Server.MapPath(filepath)))
            {
                Directory.CreateDirectory(filepath);
            }
            try
            {
                string a = HttpContext.Server.MapPath(filepath);
                postedFile.SaveAs(a + "\\" + saveName);
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 考勤录入
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="beginTime">开课时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public JsonResult SubmitAttend(int courseid, int userid, string beginTime, string endTime)
        {
            try
            {
                if (string.IsNullOrEmpty(beginTime))
                {
                    beginTime = "2050-1-1";
                }
                if (string.IsNullOrEmpty(endTime))
                {
                    endTime = "2000-1-1";
                }
                IAtt.AddAttendce(courseid, userid, beginTime, endTime, 0);
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 考勤结束 加上授课人的折算 update by huihh 2014-05-19
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        [Filter.SystemLog("考勤结束", LogLevel.Info)]
        public JsonResult GetScore(int courseid)
        {
            try
            {
                var course = Icourse.GetCo_Course(courseid);
                List<Cl_Attendce> attendceList = IAtt.GetAttendUserList(courseid);
                //考勤结束
                if (null != attendceList)
                {
                    foreach (var attend in attendceList)
                    {
                        if (attend.StartTime.Year != 1 && attend.EndTime.Year != 1)
                        {
                            IAtt.UpScore(courseid, attend.UserId, attend.StartTime, attend.EndTime);
                        }
                        else
                        {
                            string beginTime = "2050-1-1";
                            string endTime = "2000-1-1";
                            IAtt.AddAttendce(courseid, attend.UserId, beginTime, endTime, 1);
                        }

                    }
                    //IAtt.UpdateAttStatus(courseid);
                }

                #region 授课人
                if (course.Teacher != "")
                {
                    if (course.TeacherIsTeacher < 2)//外聘讲师没份
                    {
                        var config = freeConfigBL.GetFreeOtherList_New("  ApplyType=0 AND year=" + DateTime.Now.Year).FirstOrDefault();
                        var TrainGradeScore = Convert.ToDecimal(config.TrainGradeScore);
                        var ConvertMax = Convert.ToDecimal(config.ConvertMax);


                        var userGetScore = userApplyBL.GetScoreByteacher(course.Teacher.StringToInt32(), DateTime.Now.Year, "(typeForm=0 OR (typeForm=1 AND CreateUserID=0) OR typeForm=3)");

                        var CPA = Convert.ToString(userGetScore.CPA);
                        var Getscore = Convert.ToDecimal(userGetScore.tScore);
                        var GetCPAScore = Convert.ToDecimal(userGetScore.CPAScore);


                        var ConvertScore = course.CourseLength * config.ConvertBaseTo;

                        //所内学时是否超过限制
                        var tscore = Getscore + ConvertScore > TrainGradeScore ? TrainGradeScore - Getscore : ConvertScore;

                        //CPA学时是否超过限制
                        var CPAScore = GetCPAScore + ConvertScore > TrainGradeScore ? TrainGradeScore - GetCPAScore : ConvertScore;


                        tscore = tscore < 0 ? 0 : tscore;
                        CPAScore = CPAScore < 0 ? 0 : CPAScore;

                        if (!(tscore == 0 && CPAScore == 0))
                        {
                            var list = userApplyBL.GetFree_UserOtherApplyList_New(" typeForm=3 and  ApplyUserID=" + course.Teacher.StringToInt32() + " and ApplyID=" + courseid);
                            if (!list.Any())
                            {
                                Free_UserOtherApply model = new Free_UserOtherApply();
                                model.Year = DateTime.Now.Year;
                                model.ConvertScore = course.CourseLength;
                                model.Memo = "";
                                model.ApplyTime = DateTime.Now;
                                model.ApplyUserID = course.Teacher.StringToInt32();
                                model.ApplyType = 3;
                                model.Status = 1;
                                model.IsDelete = 0;
                                model.tScore = tscore;
                                model.CPAScore = (CPA == "是" && course.IsCPA == 1) ? CPAScore : 0;
                                model.GettScore = tscore;
                                model.GetCPAScore = (CPA == "是" && course.IsCPA == 1) ? CPAScore : 0;
                                model.ApplyName = "";
                                model.ApplyID = courseid;
                                model.typeForm = 3;
                                model.CreateUserID = 0;
                                model.ApproveStatus = 1;
                                model.DepApproveStatus = 1;
                                userApplyBL.AddFree_UserOtherApply(model);

                                var typeContent = "";
                                var scoreContent = "";
                                if (tscore != 0)
                                {
                                    typeContent = "内部培训其他形式学时";
                                    scoreContent = tscore.ToString("0.00");
                                }
                                if (CPA == "是" && course.IsCPA == 1)
                                {
                                    typeContent = typeContent == "" ? "CPA继续教育其他形式学时" : typeContent + "/CPA继续教育其他形式学时";
                                    scoreContent = scoreContent == "" ? model.GetCPAScore.ToString("0.00") : scoreContent + "/" + model.GetCPAScore.ToString("0.00");
                                }
                                SendEmailTeacherScore(model.ApplyUserID, course.CourseName, course.StartTime.ToString("yyyy年MM月dd日"), typeContent, scoreContent);
                            }
                        }
                    }
                }
                #endregion

                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 发送短信and邮件
        /// </summary>
        /// <param name="userIDs"></param>
        public void SendMess(int CouID, string userIDs, int xmlInt, string Mailtitle, bool isSend = true)
        {
            //发送消息
            List<Message> MessUser = IAtt.GetUserinfo(CouID, userIDs);
            var content = GetFormworkContent(xmlInt);

            var emailList = new List<KeyValuePair<string, string>>();
            var messlist = new List<KeyValuePair<string, string>>();

            if (MessUser.Count > 0)
            {
                foreach (Message mm in MessUser)
                {
                    Sys_User sysu = leaderBL.GetLeaderIdByUserId(mm.UserId);
                    var MContent = string.Format(content, sysu.Realname, mm.Realname, mm.CourseName);
                    List<string> telephone = new List<string>();
                    List<string> EmailAddress = new List<string>();
                    if (!string.IsNullOrWhiteSpace(sysu.Email))
                        // EmailAddress.Add(mm.Email);
                        emailList.Add(new KeyValuePair<string, string>(sysu.Email, MContent));
                    if (!string.IsNullOrWhiteSpace(sysu.MobileNum))
                        // telephone.Add(mm.MobileNum);
                        messlist.Add(new KeyValuePair<string, string>(sysu.MobileNum, MContent));

                }

            }
            if (emailList.Count > 0)
                SendEmail(emailList, Mailtitle);
            if (messlist.Count > 0 && isSend)
                SendMessage(messlist);
        }


        /// <summary>
        /// 导出考勤详情列表execl 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uname"></param>
        /// <param name="dname"></param>
        /// <param name="CState"></param>
        /// <param name="OState"></param>
        public void ExportAttendceList(int id, string uname, string dname, int CState, int OState)
        {
            string where = "1=1";
            string userIDs = "";

            if (!string.IsNullOrEmpty(uname))
            {
                where += string.Format(" and su.Realname LIKE '%{0}%'", uname.ReplaceSql());
            }
            if (!string.IsNullOrEmpty(dname))
            {
                where += string.Format(" and sd.DeptName LIKE '%{0}%'", dname.ReplaceSql());
            }
            switch (CState)
            {
                case 0:
                    break;
                case 1:
                    where += string.Format(" AND su.CPA ='是' ");
                    break;
                case 2:
                    where += string.Format(" AND su.CPA ='否' ");
                    break;
            }
            switch (OState)
            {
                case 0:
                    break;
                case 2:
                    where += string.Format(" AND cl.OrderStatus=3 ");
                    break;
                case 1:
                    where += string.Format(" AND cl.OrderStatus=1 ");
                    break;
            }

            List<Cl_Attendce> attendceList = IAtt.GetAttendUserList(id, "asc", where);
            List<Cl_Attendce> tempcu1 = attendceList.Where(p => p.OrderStatus == 1).ToList();
            List<Cl_Attendce> tempcu3 = attendceList.Where(p => p.OrderStatus == 3).ToList();
            List<Cl_Attendce> tempAtt = attendceList.Where(p => p.StartTimeM != "" && p.EndTimeM != "").ToList();
            List<Cl_Attendce> tempAtt1 = tempAtt.Where(p => Convert.ToDateTime(p.StartTime.ToString("yyyy-MM-dd HH:mm")) > Convert.ToDateTime(p.courseStart.ToString("yyyy-MM-dd HH:mm"))).ToList();
            List<Cl_Attendce> tempAtt2 = tempAtt.Where(p => Convert.ToDateTime(p.EndTime.ToString("yyyy-MM-dd HH:mm")) < Convert.ToDateTime(p.courseEnd.ToString("yyyy-MM-dd HH:mm"))).ToList();
            foreach (var item in attendceList)
            {
                item.Realname = item.Realname;
                item.DeptName = item.DeptName;
                item.actual = tempAtt.Count;
                item.Ordersum = tempcu1.Count;
                item.OrderOut = tempcu3.Count;
                item.agosum = tempAtt1.Count;
                item.lastsum = tempAtt2.Count;
                item.TrainGrade = item.TrainGrade;
                item.CPANo = item.CPANo.HtmlXssEncode();
                item.GetScore = item.GetScore;
            }


            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("部门/分所", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("是否CPA", typeof(string));
            dt.Columns.Add("CPA编号", typeof(string));
            dt.Columns.Add("是否预订课程", typeof(string));
            dt.Columns.Add("获得学时", typeof(decimal));
            dt.Columns.Add("上课考勤时间", typeof(string));
            dt.Columns.Add("下课考勤时间", typeof(string));

            var count = 1;
            foreach (Cl_Attendce item in attendceList)
            {
                dt.Rows.Add(count++, item.Realname, item.DeptName, item.TrainGrade, item.CPA, "'" + item.CPANo,
                            item.OrderStr, item.GetScore, item.StartTimeM, item.EndTimeM);
            }
            var dtList = new List<DataTable>();
            string strFileName = "考勤管理详情";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);

        }

        /// <summary>
        /// 考勤结束给授课人折算学时的时候发邮件
        /// </summary>
        public void SendEmailTeacherScore(int userID, string courseName, string courseTime, string typeContent, string scoreContent)
        {
            var model = userBL.Get(userID);
            var content = GetFormworkContent(21);

            var emailList = new List<KeyValuePair<string, string>>();
            var c = string.Format(content, model.Realname, courseTime, courseName, typeContent, scoreContent);
            emailList.Add(new KeyValuePair<string, string>(model.Email, c));

            if (emailList.Count > 0)
                SendEmail(emailList, "您所授的课程已经被折算学时");
        }
        #endregion
    }

}