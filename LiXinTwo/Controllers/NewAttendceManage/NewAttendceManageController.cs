using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinModels.CourseManage;
using LiXinCommon;
using System.Text.RegularExpressions;
using LiXinModels.NewCourseManage;
using LixinModels.NewClassManage;
using LiXinInterface.NewCourseManage;
using System.Configuration;
using System.Web;
using System.Data;
using System.IO;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public partial class NewAttendceManageController : BaseController
    {
        protected INew_Attendce _attendceBL;
        protected INew_CourseRoomRule _courseRoomRuleBL;
        public NewAttendceManageController(INew_Attendce attendceBL, INew_CourseRoomRule courseRoomRuleBL)
        {
            _attendceBL = attendceBL;
            _courseRoomRuleBL = courseRoomRuleBL;
        }

        #region 呈现页面
        /// <summary>
        /// 考勤列表呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult NewAttendceList(string tp)
        {
            ViewBag.nowTime = DateTime.Now.Year + "-01-01 00:00";
            ViewBag.page = 1;
            ViewBag.cname = "请输入搜索内容";
            ViewBag.isGroupt = "-1";
            ViewBag.state = "0";
            if (tp != null && tp != "" && tp == "1")
            {
                if (Session["newcopage"] != null)
                {
                    string sess = Session["newcopage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.way = att[0];
                    ViewBag.cname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.cstartTime = att[2];
                    ViewBag.cendTime = att[3];
                    ViewBag.isGroupt = att[4];
                    ViewBag.state = att[5];
                    ViewBag.page = att[6];
                }
            }
            return View();
        }

        /// <summary>
        /// 考勤管理呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult NewAttendceDetail()
        {
            //ViewData["Classlist"] = AllClass;
            return View();
        }

        /// <summary>
        /// 考勤录入呈现
        /// </summary>
        public ActionResult NewAttendceEdit(int cid, int aid, int uid, int sid, int IsAttFlag)
        {
            ViewBag.courseId = cid;
            ViewBag.attId = aid;
            ViewBag.userId = uid;
            ViewBag.subcourseId = sid;
            ViewBag.isatt = IsAttFlag;
            return View();
        }

        /// <summary>
        /// 导入考勤呈现
        /// </summary>
        public ActionResult ImportNewAttend()
        {
            return View();
        }

        #endregion

        #region 操作

        /// <summary>
        /// 获取考勤人员列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNewAttendUserList(int id, string Uname, string number, int isGroup, int isatt, int pageSize = 20, int pageIndex = 1)
        {
            int totalCount = 0;
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" 1=1 ");
            if (!string.IsNullOrEmpty(Uname))
            {
                strWhere.AppendFormat(" AND su.Realname like '%{0}%' ", Uname.ReplaceSql());
            }
            if (!string.IsNullOrEmpty(number))
            {
                strWhere.AppendFormat(" AND ngu.NumberID like '%{0}%' ", number.ReplaceSql());
            }
            if (isGroup > -1)
            {
                strWhere.AppendFormat(" AND ncod.Type = {0}", isGroup);
            }

            List<New_CourseOrderDetail> listAtt = _attendceBL.GetNewAttendUserList(id, out totalCount, 1, int.MaxValue, "desc", strWhere.ToString());
            switch (isatt)
            {
                case 0://考勤未上传
                    listAtt = listAtt.Where(p => p.AttStatusStr == "考勤未上传").ToList();
                    break;
                case 1://正常
                    listAtt = listAtt.Where(p => p.AttStatusStr == "正常").ToList();
                    break;
                case 2://缺勤
                    listAtt = listAtt.Where(p => p.AttStatusStr == "缺勤").ToList();
                    break;
                case 3://迟到
                    listAtt = listAtt.Where(p => p.AttStatusStr == "迟到").ToList();
                    break;
                case 4://早退
                    listAtt = listAtt.Where(p => p.AttStatusStr == "早退").ToList();
                    break;
                case 5://迟到且早退
                    listAtt = listAtt.Where(p => p.AttStatusStr == "迟到并早退").ToList();
                    break;
            }
            List<New_CourseOrderDetail> list1 = listAtt.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();
            foreach (var item in list1)
            {
                item.Realname = item.Realname.HtmlXssEncode();
                item.RoomName = item.RoomName.HtmlXssEncode();
            }
            return Json(new
            {
                dataList = list1,
                recordCount = listAtt.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 考勤录入
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="beginTime">开课时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [Filter.SystemLog("新员工 考勤管理 考勤录入", LogLevel.Info)]
        public JsonResult SubmitNewAttend(int courseid, int attid, int userid, int IsAttFlag, int subcourseid, string beginTime, string endTime)
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
                New_CourseRoomRule coRoomModel = _courseRoomRuleBL.GetNew_CourseRoomRule(subcourseid);
                New_Attendce attmodel = new New_Attendce();

                attmodel.CourseId = courseid;
                attmodel.SubCourseID = subcourseid;
                attmodel.UserId = userid;
                attmodel.StartTime = Convert.ToDateTime(beginTime);
                attmodel.EndTime = Convert.ToDateTime(endTime);
                if (IsAttFlag == 0)
                {
                    if (Convert.ToDateTime(beginTime) <= Convert.ToDateTime(coRoomModel.StartTime.ToString("yyyy-MM-dd HH:mm")) && Convert.ToDateTime(endTime) >= Convert.ToDateTime(coRoomModel.EndTime.ToString("yyyy-MM-dd HH:mm")))
                    {
                        attmodel.AttStatus = 0;
                    }
                    else
                    {
                        attmodel.AttStatus = 1;
                    }
                }
                else if (IsAttFlag == 1)
                {
                    if (Convert.ToDateTime(beginTime) <= Convert.ToDateTime(coRoomModel.StartTime.ToString("yyyy-MM-dd HH:mm")))
                    {
                        attmodel.AttStatus = 0;
                    }
                    else
                    {
                        attmodel.AttStatus = 1;
                    }
                }
                else if (IsAttFlag == 2)
                {
                    if (Convert.ToDateTime(endTime) >= Convert.ToDateTime(coRoomModel.EndTime.ToString("yyyy-MM-dd HH:mm")))
                    {
                        attmodel.AttStatus = 0;
                    }
                    else
                    {
                        attmodel.AttStatus = 1;
                    }
                }
                else
                {
                    attmodel.AttStatus = 0;
                }

                if (attid > 0)
                {
                    attmodel.Id = attid;
                    var ret = _attendceBL.UpdateNewAttend(attmodel);
                    if (ret)
                    {
                        return Json(new
                        {
                            result = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            result = 0
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var attID = _attendceBL.AddNewAttend(attmodel);
                    if (attID > 0)
                    {
                        return Json(new
                        {
                            result = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            result = 0
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        /// 下载模板
        /// </summary>
        public void OutTemplate(int id)
        {
            int totalCount = 0;
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" 1=1 ");
            List<New_CourseOrderDetail> listAtt = _attendceBL.GetNewAttendUserList(id, out totalCount, 1, int.MaxValue, "desc", strWhere.ToString());
            DataTable outTable = new DataTable();
            outTable.Columns.Add("流水号", typeof(string));
            outTable.Columns.Add("用户ID", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("课程类型", typeof(string));
            outTable.Columns.Add("上课考勤时间", typeof(string));
            outTable.Columns.Add("下课考勤时间", typeof(string));
            foreach (New_CourseOrderDetail att in listAtt)
            {
                outTable.Rows.Add(
                    att.SubCourseID + "—" + att.IsAttFlag + "—" + att.UserId,
                    att.UserId,
                    att.Realname,
                    att.Type == 0 ? "集中授课" : "分组带教",
                    "",
                    "");
            }
            new Spreadsheet().Template(null, null, outTable, HttpContext, "考勤模板", "考勤模板");
        }

        /// <summary>
        ///  导入考勤表
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("新员工 考勤管理 导入考勤表", LogLevel.Info)]
        public JsonResult SubmitImport(int id)
        {
            string filename = "";
            string resultName = "";
            string folder = ConfigurationManager.AppSettings["AttendceUrl"];
            HttpPostedFileBase excelfiles = Request.Files[0];
            int cg = 0;
            int sb = 0;
            if (null != excelfiles)
            {
                //try
                //{
                filename = Path.GetFileName(excelfiles.FileName); //获得文件名
                string fullPathname = Path.Combine(folder, filename);//文件后缀名
                string suffix = excelfiles.FileName.Substring(excelfiles.FileName.LastIndexOf(".") + 1).ToLower();
                resultName = Guid.NewGuid() + "." + suffix;
                saveFile(excelfiles, folder, resultName);
                List<DataTable> dtList = new Spreadsheet().LoadExcel(HttpContext.Server.MapPath(folder) + resultName);
                if (dtList.Count > 0)
                {
                    for (int i = 0; i < dtList.Count; i++)
                    {
                        for (int j = 0; j < dtList[i].Rows.Count; j++)
                        {
                            string SwiftNumber = dtList[i].Rows[j][0].ToString();
                            if (!string.IsNullOrEmpty(SwiftNumber))
                            {
                                string[] NumberStr = Regex.Split(SwiftNumber, "—", RegexOptions.IgnoreCase);
                                int subcourseid = Convert.ToInt32(NumberStr[0]);
                                int IsAttFlag = Convert.ToInt32(NumberStr[1]);
                                int userid = Convert.ToInt32(NumberStr[2]);
                                string starttime = dtList[i].Rows[j][4].ToString();
                                string endtime = dtList[i].Rows[j][5].ToString();
                                if (starttime == "--" || string.IsNullOrEmpty(starttime) || starttime == "——")
                                {
                                    starttime = "2050-1-1";
                                }
                                if (endtime == "--" || string.IsNullOrEmpty(endtime) || endtime == "——")
                                {
                                    endtime = "2000-1-1";
                                }
                                New_CourseRoomRule coRoomModel = _courseRoomRuleBL.GetNew_CourseRoomRule(subcourseid);
                                New_Attendce attmodel = _attendceBL.GetNew_Attendce(id, userid, subcourseid);
                                if (attmodel != null)
                                {
                                    attmodel.StartTime = Convert.ToDateTime(starttime);
                                    attmodel.EndTime = Convert.ToDateTime(endtime);
                                    //判断考勤是否正常
                                    if (IsAttFlag == 0)
                                    {
                                        if (Convert.ToDateTime(starttime) <= Convert.ToDateTime(coRoomModel.StartTime.ToString("yyyy-MM-dd HH:mm")) 
                                            && Convert.ToDateTime(endtime) >= Convert.ToDateTime(coRoomModel.EndTime.ToString("yyyy-MM-dd HH:mm")))
                                        {
                                            attmodel.AttStatus = 0;
                                        }
                                        else
                                        {
                                            attmodel.AttStatus = 1;
                                        }
                                    }
                                    else if (IsAttFlag == 1)
                                    {
                                        if (Convert.ToDateTime(starttime) <= Convert.ToDateTime(coRoomModel.StartTime.ToString("yyyy-MM-dd HH:mm")))
                                        {
                                            attmodel.AttStatus = 0;
                                        }
                                        else
                                        {
                                            attmodel.AttStatus = 1;
                                        }
                                    }
                                    else if (IsAttFlag == 2)
                                    {
                                        if (Convert.ToDateTime(endtime) >= Convert.ToDateTime(coRoomModel.EndTime.ToString("yyyy-MM-dd HH:mm")))
                                        {
                                            attmodel.AttStatus = 0;
                                        }
                                        else
                                        {
                                            attmodel.AttStatus = 1;
                                        }
                                    }
                                    else
                                    {
                                        attmodel.AttStatus = 0;
                                    }

                                    var ret = _attendceBL.UpdateNewAttend(attmodel);
                                    if (ret)
                                    {
                                        cg++;
                                    }
                                    else
                                    {
                                        sb++;
                                    }
                                }
                                else
                                {
                                    attmodel = new New_Attendce();
                                    attmodel.CourseId = id;
                                    attmodel.SubCourseID = subcourseid;
                                    attmodel.UserId = userid;
                                    attmodel.StartTime = Convert.ToDateTime(starttime);
                                    attmodel.EndTime = Convert.ToDateTime(endtime);
                                    //判断考勤是否正常
                                    if (IsAttFlag == 0)
                                    {
                                        if (Convert.ToDateTime(starttime) <= Convert.ToDateTime(coRoomModel.StartTime.ToString("yyyy-MM-dd HH:mm")) && Convert.ToDateTime(endtime) >= Convert.ToDateTime(coRoomModel.EndTime.ToString("yyyy-MM-dd HH:mm")))
                                        {
                                            attmodel.AttStatus = 0;
                                        }
                                        else
                                        {
                                            attmodel.AttStatus = 1;
                                        }
                                    }
                                    else if (IsAttFlag == 1)
                                    {
                                        if (Convert.ToDateTime(starttime) <= Convert.ToDateTime(coRoomModel.StartTime.ToString("yyyy-MM-dd HH:mm")))
                                        {
                                            attmodel.AttStatus = 0;
                                        }
                                        else
                                        {
                                            attmodel.AttStatus = 1;
                                        }
                                    }
                                    else if (IsAttFlag == 2)
                                    {
                                        if (Convert.ToDateTime(endtime) >= Convert.ToDateTime(coRoomModel.EndTime.ToString("yyyy-MM-dd HH:mm")))
                                        {
                                            attmodel.AttStatus = 0;
                                        }
                                        else
                                        {
                                            attmodel.AttStatus = 1;
                                        }
                                    }
                                    else
                                    {
                                        attmodel.AttStatus = 0;
                                    }

                                    var attID = _attendceBL.AddNewAttend(attmodel);
                                    if (attID > 0)
                                    {
                                        cg++;
                                    }
                                    else
                                    {
                                        sb++;
                                    }
                                }

                            }
                        }
                    }
                }
                return Json(new
                {
                    result = 1,
                    content = "成功" + cg + "条，失败" + sb + "条"
                }, "text/html", JsonRequestBehavior.AllowGet);
                //}
                //catch (Exception ex)
                //{
                //    return Json(new
                //    {
                //        result = 0,
                //        content = "导入失败，模板中的数据存在错误，请更正后导入"
                //    }, "text/html", JsonRequestBehavior.AllowGet);
                //}
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
        /// 考勤正常
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        [Filter.SystemLog("新员工 考勤管理 一键考勤正常", LogLevel.Info)]
        public JsonResult updateStatus(int courseid)
        {
            try
            {
                int sum = 0;
                int totalCount = 0;
                StringBuilder strWhere = new StringBuilder();
                strWhere.Append(" 1=1 ");
                List<New_CourseOrderDetail> listAtt = _attendceBL.GetNewAttendUserList(courseid, out totalCount, 1, int.MaxValue, "desc", strWhere.ToString());
                if (null != listAtt)
                {
                    foreach (var attend in listAtt)
                    {
                        if (attend.attId == 0)
                        {
                            New_Attendce attmodel = new New_Attendce();
                            attmodel.CourseId = courseid;
                            attmodel.SubCourseID = attend.SubCourseID;
                            attmodel.UserId = attend.UserId;
                            attmodel.StartTime = attend.CoStartTime;
                            attmodel.EndTime = attend.CoEndTime;
                            attmodel.AttStatus = 0;
                            var attid = _attendceBL.AddNewAttend(attmodel);
                            if (attid > 0)
                            {
                                sum++;
                            }
                        }
                        else
                        {
                            if (attend.AttStatus == 1)
                            {
                                New_Attendce attmodel = new New_Attendce();
                                attmodel.CourseId = courseid;
                                attmodel.SubCourseID = attend.SubCourseID;
                                attmodel.UserId = attend.UserId;
                                attmodel.StartTime = attend.CoStartTime;
                                attmodel.EndTime = attend.CoEndTime;
                                attmodel.AttStatus = 0;
                                attmodel.Id = attend.attId;
                                var ret = _attendceBL.UpdateNewAttend(attmodel);
                                if (ret)
                                {
                                    sum++;
                                }
                            }
                        }
                    }
                }

                return Json(new
                {
                    result = 1,
                    content = sum
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = 0,
                    content = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
