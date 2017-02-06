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
        public JsonResult GetAttendceList(int way, string name, string must, int state, string startTime, string endTime, int pageSize = 20, int pageIndex = 1)
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

                List<Co_Course> attendceList = IAtt.GetAttendceList(way, out totalCount, pageIndex, pageSize,"desc",where);
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
        public JsonResult GetAttendUserList(int id, string uname,string dname,int CState,int OState,int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            string userIDs = "";
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

                List<Cl_Attendce> attendceList = IAtt.GetAttendUserList(id,"asc", where);
                List<Cl_Attendce> tempcu1 = attendceList.Where(p => p.OrderStatus == 1).ToList();
                List<Cl_Attendce> tempcu3 = attendceList.Where(p => p.OrderStatus == 3).ToList();
                List<Cl_Attendce> tempAtt = attendceList.Where(p => p.StartTimeM != "" && p.EndTimeM != "").ToList();
                List<Cl_Attendce> tempAtt1 = tempAtt.Where(p => Convert.ToDateTime(p.StartTime.ToString("yyyy-MM-dd HH:mm")) > Convert.ToDateTime(p.courseStart.ToString("yyyy-MM-dd HH:mm"))).ToList();
                List<Cl_Attendce> tempAtt2 = tempAtt.Where(p => Convert.ToDateTime(p.EndTime.ToString("yyyy-MM-dd HH:mm")) < Convert.ToDateTime(p.courseEnd.ToString("yyyy-MM-dd HH:mm"))).ToList();
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
        public JsonResult UpOrder(int cid, string uids)
        {
            try
            {
                if (!string.IsNullOrEmpty(uids))
                {
                    string sss = "";
                    string ssss = "";
                    //补预定
                    Sys_ParamConfig syscon=AllSystemConfigs.Where(p => p.ConfigType == 20).FirstOrDefault();
                    if (uids.IndexOf(",") > -1)
                    {
                        string[] userList = Regex.Split(uids, ",", RegexOptions.IgnoreCase);
                        for (int i = 0; i < userList.Length; i++)
                        {
                            
                            if (IAtt.Cl_MakeUpOrderCount(Convert.ToInt32(syscon.ConfigValue), Convert.ToInt32(userList[i])))
                            {
                                IAtt.MakeUpOrder(cid, Convert.ToInt32(userList[i]));
                                ssss += userList[i]+",";
                            }
                            else
                            {
                                sss += userList[i] + ",";
                            }
                        }
                        if (sss != "" && ssss!="")
                        {
                            ssss = ssss.Remove(ssss.LastIndexOf(","), 1);
                            SendMess(cid, ssss, 7, "补预定申请",false);
                            return Json(new
                            {
                                result = 1,
                                content = "补预订成功,但部分用户超出补预订次数限制"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else if (sss == "" && ssss != "")
                        {
                            ssss = ssss.Remove(ssss.LastIndexOf(","), 1);
                            SendMess(cid, ssss, 7, "补预定申请",false);
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
                        if (IAtt.Cl_MakeUpOrderCount(Convert.ToInt32(syscon.ConfigValue), Convert.ToInt32(uids)))
                        {
                               IAtt.MakeUpOrder(cid, Convert.ToInt32(uids));
                            SendMess(cid, uids, 7, "补预定申请",false);
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
                    List<DataTable> dtList=new Spreadsheet().LoadExcel(HttpContext.Server.MapPath(folder)+resultName, 1);
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
                IAtt.AddAttendce(courseid, userid, beginTime, endTime);
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
        /// 考勤结束
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public JsonResult GetScore(int courseid)
        {
            try
            {
                List<Cl_Attendce> attendceList = IAtt.GetAttendUserList(courseid);
                if (null != attendceList)
                {
                    foreach (var attend in attendceList)
                    {
                        if (attend.StartTime.Year != 1 && attend.EndTime.Year != 1)
                        {
                            IAtt.UpScore(courseid, attend.UserId, attend.StartTime, attend.EndTime);
                        }
                    }
                    //IAtt.UpdateAttStatus(courseid);
                }
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
        public void SendMess(int CouID,string userIDs,int xmlInt,string Mailtitle,bool isSend=true )
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
        #endregion
    }
}