using System;
using System.Collections.Generic;
using LiXinInterface.DepTranManage;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using LiXinCommon;
using System.IO;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using LiXinInterface.SystemManage;
using LiXinModels.DepTranManage;
using LiXinModels.DepTranAttendce;
using LiXinInterface.User;
using System.Text;
using LiXinInterface.DepCourseManage;
using LiXinModels.DepAttendce;
using LiXinModels.DepCourseManage;
using LiXinModels;
using LiXinModels.User;
using LiXinInterface.DepCourseLearn;

namespace LiXinControllers.DepAttendce
{
    public partial class DepAttendceController : BaseController
    {
        protected IDep_Attendce depAttBL;
        protected IUser userBL;
        protected IDep_Course coBL;
        protected IDep_CourseOrder depOrderBL;
        protected IDep_CourseDept depCoDepBL;
        protected IDep_CourseAttFile depAttFileBL;
        protected IDep_AttendceApprovalRecord depAttRecordBL;
        protected ISys_Leader leaderBL;
        protected IDep_CpaLearnStatus depCpaLearn;

        public DepAttendceController(IDep_Attendce _depAttBL, IUser _userBL, IDep_CourseOrder _depOrderBL, IDep_CourseDept _depCoDepBL, IDep_CourseAttFile _depAttFileBL, IDep_AttendceApprovalRecord _depAttRecordBL, ISys_Leader _leaderBL, IDep_Course _coBL, IDep_CpaLearnStatus _depCpaLearn)
        {
            depAttBL = _depAttBL;
            userBL = _userBL;
            depOrderBL = _depOrderBL;
            depCoDepBL = _depCoDepBL;
            depAttFileBL = _depAttFileBL;
            depAttRecordBL = _depAttRecordBL;
            leaderBL = _leaderBL;
            coBL = _coBL;
            depCpaLearn = _depCpaLearn;
        }

        #region 呈现页面

        /// <summary>
        ///  考勤列表呈现
        /// </summary>
        public ViewResult DepAttendceList()
        {
            int did = CurrentUser == null ? 0 : CurrentUser.DeptId;
            ViewBag.did = did;
            return View();
        }

        /// <summary>
        /// 考勤详情
        /// </summary>
        /// <returns></returns>
        public ViewResult DepAttendce(int id, int way,int did)
        {
            ViewBag.flag = 0;
            string times = "-1";
            Dep_Course comodel = coBL.GetCo_Course(id);
            if (comodel.Way == 3)
            {
                Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 51).FirstOrDefault();
                times = cpazq.ConfigValue == "" ? "0" : cpazq.ConfigValue;
            }
            else
            {
                Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 46).FirstOrDefault();
                times = cpazq.ConfigValue == "" ? "0" : cpazq.ConfigValue;
            }
            if (times != "-1")
            {
                DateTime endtime = comodel.EndTime;
                endtime = endtime.AddHours(Convert.ToInt32(times));
                if (DateTime.Now > endtime)
                {
                    ViewBag.flag = 1;
                }
            }
            ViewBag.did = did;
            ViewBag.cid = id;
            ViewBag.way = way;
            return View();
        }

        /// <summary>
        /// 考勤
        /// </summary>
        /// <returns></returns>
        public ViewResult AttendceStatus(int cid, int uid,int did,int IsAttFlag,string uids,int way)
        {
            ViewBag.cid=cid;
            ViewBag.uid = uid;
            ViewBag.did = did;
            ViewBag.isatt = IsAttFlag;
            ViewBag.uids = uids;
            ViewBag.way = way;
            return View();
        }
        
        /// <summary>
        /// 选择人员
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult SelectUser(int id,int did, string type = "checkbox", int pageSize = 10)
        {
            ViewBag.id = id;
            ViewBag.did = did;
            ViewBag.type = type;
            ViewBag.pageSize = pageSize;
            return View();
        }

        /// <summary>
        /// 导入考勤呈现
        /// </summary>
        public ActionResult ImportDepAttend()
        {
            return View();
        }
        /// <summary>
        /// 上传附件呈现
        /// </summary>
        public ActionResult UploadFile()
        {
            return View();
        }

        /// <summary>
        ///  考勤列表呈现
        /// </summary>
        public ViewResult DepAttendceListByU()
        {
            return View();
        }

        /// <summary>
        ///  考勤详情
        /// </summary>
        public ViewResult DepAttendceByU(int cid, int way)
        {
            ViewBag.flag = 0;
            Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 46).FirstOrDefault();
            string times = cpazq.ConfigValue == "" ? "0" : cpazq.ConfigValue;
            Dep_Course comodel = coBL.GetCo_Course(cid);
            if (times != "-1")
            {
                DateTime endtime = comodel.EndTime;
                endtime = endtime.AddHours(Convert.ToInt32(times));
                if (DateTime.Now > endtime)
                {
                    ViewBag.flag = 1;
                }
            }
            ViewBag.did = comodel.DeptId;
            ViewBag.cid = cid;
            ViewBag.way = way;
            return View();
        }
        
        #endregion

        #region 操作

        /// <summary>
        /// 获取所有课程列表(有分页)
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepAttendceList(int did,string courseName, string isMust,int attState, int CoState, string StartTime, string EndTime, int pageSize = 20, int pageIndex = 1)
        {
            if (did > 0)
            {
                string where = "1=1";
                try
                {
                    int totalCount = 0;
                    if (!string.IsNullOrEmpty(courseName))
                    {
                        where += string.Format(" AND temp.CourseName like '%{0}%' ", courseName.ReplaceSql());
                    }
                    if (!string.IsNullOrEmpty(isMust))
                    {
                        if (!isMust.Contains(","))
                        {
                            where += string.Format(" AND temp.IsMust={0}", Convert.ToInt32(isMust));
                        }
                    }

                    if (!string.IsNullOrEmpty(StartTime))
                    {
                        where += string.Format(" and CONVERT(varchar(100),temp.StartTime,23) >= '{0}'", DateTime.Parse(StartTime).ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrEmpty(EndTime))
                    {
                        where += string.Format(" and CONVERT(varchar(100),temp.EndTime,23) <= '{0}'", DateTime.Parse(EndTime).ToString("yyyy-MM-dd"));
                    }

                    switch (CoState)
                    {
                        case 0:
                            break;
                        case 1:
                            where += string.Format(" and temp.StartTime > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            break;
                        case 2:
                            where += string.Format(" and temp.StartTime <= '{0}' and temp.EndTime >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            break;
                        case 3:
                            where += string.Format(" and temp.EndTime < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            break;
                    }
                    if (attState > -1)
                    {
                        switch (attState)
                        {
                            case 0:
                                where += string.Format(" and temp.AttFlag =0 and temp.ApprovalFlag =0 ");
                                break;
                            case 1:
                                where += string.Format(" and temp.AttFlag =1 and temp.ApprovalFlag =1 ");
                                break;
                            case 2:
                                where += string.Format(" and temp.AttFlag =0 and temp.ApprovalFlag =1 ");
                                break;
                            case 3:
                                where += string.Format(" and temp.AttFlag =1 and temp.ApprovalFlag =0 ");
                                break;
                        }
                    }

                    var deptattendceList = depAttBL.GetDepAttendceList(did.ToString(), out totalCount, pageIndex, pageSize, "StartTime DESC", where);
                    foreach (var item in deptattendceList)
                    {
                        item.CourseName = item.CourseName.HtmlXssEncode();
                        item.RoomName = item.RoomName.HtmlXssEncode();
                    }
                    return Json(new
                    {
                        dataList = deptattendceList,
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
            else
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取差异列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDifferenceList(int did, string courseName, int deptype = 0, int cid = 0, int pageSize = 20, int pageIndex = 1)
        {
            if (did > 0)
            {
                string where = "1=1";
                try
                {
                    int totalCount = 0;
                    List<DepAttendceCourseModel> differenceList = new List<DepAttendceCourseModel>();
                    if (!string.IsNullOrEmpty(courseName))
                    {
                        where += string.Format(" AND cc.CourseName like '%{0}%' ", courseName.ReplaceSql());
                    }
                    if (cid>0)
                    {
                        if (deptype == 0)
                        {
                            where += string.Format(" AND sd.DepartmentId={0} ", did);
                        }
                        differenceList = depAttBL.GetDifferenceListByCo(cid, out totalCount, pageIndex, pageSize, "cc.StartTime DESC", where);
                    }
                    else
                    {
                        differenceList = depAttBL.GetDifferenceList(did, out totalCount, pageIndex, pageSize, "cc.StartTime DESC", where);
                    }
                    foreach (var item in differenceList)
                    {
                        item.CourseName = item.CourseName.HtmlXssEncode();
                    }
                    return Json(new
                    {
                        dataList = differenceList,
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
            else
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 下载差异表
        /// </summary>
        public void OutDiffer(int did, string courseName, int deptype = 0, int cid = 0)
        {
            int totalCount = 0;
            string where = "1=1";
            List<DepAttendceCourseModel> differenceList = new List<DepAttendceCourseModel>();
            if (!string.IsNullOrEmpty(courseName))
            {
                where += string.Format(" AND cc.CourseName like '%{0}%' ", courseName.ReplaceSql());
            }
            if (cid > 0)
            {
                if (deptype == 0)
                {
                    where += string.Format(" AND sd.DepartmentId={0} ", did);
                }
                differenceList = depAttBL.GetDifferenceListByCo(cid, out totalCount, 1, int.MaxValue, "cc.StartTime DESC", where);
            }
            else
            {
                differenceList = depAttBL.GetDifferenceList(did, out totalCount, 1, int.MaxValue, "cc.StartTime DESC", where);
            }
            DataTable outTable = new DataTable();
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("部门名称", typeof(string));
            outTable.Columns.Add("出席人数", typeof(string));
            outTable.Columns.Add("考勤人数", typeof(string));
            outTable.Columns.Add("正常人数", typeof(string));
            outTable.Columns.Add("缺勤人数", typeof(string));
            outTable.Columns.Add("待考勤人数", typeof(string));
            outTable.Columns.Add("迟到人数", typeof(string));
            outTable.Columns.Add("早退人数", typeof(string));
            outTable.Columns.Add("迟到且早退人数", typeof(string));

            foreach (DepAttendceCourseModel model in differenceList)
            {
                outTable.Rows.Add(model.CourseName, model.DepartSetName + "(" + model.DeptCode + ")", model.CoCount, model.AttCount, model.AttOkCount, model.AttNoCount, model.NoAttCount, model.chidaoCount, model.zaotuiCount, model.chizaoCount);
            }
            new Spreadsheet().Template(null, null, outTable, HttpContext, "考勤差异表", "差异表");
        }

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDeptAttendUserList(int id, int deptId, string uname, int AState, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                string where = "1=1";
                if (!string.IsNullOrEmpty(uname))
                {
                    where += string.Format(" and su.Realname LIKE '%{0}%'", uname.ReplaceSql());
                }
                if (AState>-1)
                {
                    if (AState == 0)
                    {
                        where += string.Format(" AND (dta.Status IS null or dta.Status=0) ");
                    }
                    else
                    {
                        where += string.Format(" AND dta.Status={0}", AState);
                    }
                }

                List<Dep_CourseOrder> attendceList = depAttBL.GetDepAttendUserList(id, deptId,out totalCount, pageIndex, pageSize, "asc", where);
                Dep_CourseDept model = depCoDepBL.GetCourseDept(id, deptId);
                foreach (var item in attendceList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = attendceList,
                    recordCount = totalCount,
                    subflag = model == null ? 0 : (model.AttFlag==1?1:0)
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0,
                    subflag =0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获得考勤审批记录列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDeptAttendRecordList(int id, int deptId, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                List<Dep_AttendceApprovalRecord> attendceRecordList = depAttRecordBL.GetDepTranApprovalRecordlist(id, deptId, out totalCount, pageIndex, pageSize);
                foreach (var item in attendceRecordList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.SubmitRealname = item.SubmitRealname.HtmlXssEncode();
                    item.Reason = item.Reason.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = attendceRecordList,
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
        /// 下载模板
        /// </summary>
        public void OutTemplate()
        {
            DataTable outTable = new DataTable();
            outTable.Columns.Add("用户ID", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("部门", typeof(string));
            outTable.Columns.Add("考勤状态(正常/缺勤/迟到/早退/迟到且早退)", typeof(string));
            new Spreadsheet().Template(null, null, outTable, HttpContext, "部门分所考勤模板", "考勤模板");
        }

        /// <summary>
        ///  导入签到表
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitImportAtt(int cid, int did, int way)
        {
            string filename = "";
            string resultName = "";
            string folder = ConfigurationManager.AppSettings["DepTranFileUrl"];
            int cg = 0;
            int sb = 0;
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
                        for (int i = 0; i < dtList.Count; i++)
                        {
                            for (int j = 0; j < dtList[i].Rows.Count; j++)
                            {
                                string uid = dtList[i].Rows[j][0].ToString();
                                string stat = dtList[i].Rows[j][3].ToString();
                                if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(stat))
                                {
                                    int userid = Convert.ToInt32(uid);
                                    int departsetid = depAttBL.IsExistAttendce(cid, userid, did);
                                    if (departsetid > 0)
                                    {
                                        if (stat == "正常")
                                        {
                                            UpdateStatus(cid, userid, did, 1, way);
                                            cg++;
                                        }
                                        else if (stat == "缺勤")
                                        {
                                            UpdateStatus(cid, userid, did, 2, way);
                                            cg++;
                                        }
                                        else if (stat == "迟到")
                                        {
                                            UpdateStatus(cid, userid, did, 3, way);
                                            cg++;
                                        }
                                        else if (stat == "早退")
                                        {
                                            UpdateStatus(cid, userid, did, 4, way);
                                            cg++;
                                        }
                                        else if (stat == "迟到且早退")
                                        {
                                            UpdateStatus(cid, userid, did, 5, way);
                                            cg++;
                                        }
                                        else
                                        {
                                            sb++;
                                        }
                                    }
                                    else
                                    {
                                        sb++;
                                    }
                                }
                                else
                                {
                                    sb++;
                                }
                            }
                        }
                    }
                    return Json(new
                    {
                        result = 1,
                        content = "成功" + cg + "条，失败" + sb + "条"
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

        /// <summary>
        ///  上传附件
        /// </summary>
        /// <returns></returns>
        public void UploadFiles()
        {
            string result = "";
            string folder = ConfigurationManager.AppSettings["DepTranFileUrl"];
            //Request.ContentType = "multipart/form-data; charset=utf-8";
            int filesum = Request.Files.Count;
            if (filesum>0)
            {
                try
                {
                    for (int i = 0; i < filesum; i++)
                    {
                        
                        HttpPostedFileBase excelfiles = Request.Files[i];
                        string filename = "";
                        string resultName = "";
                        filename =Path.GetFileName(excelfiles.FileName); //获得文件名
                        string fullPathname = Path.Combine(folder, filename);//文件后缀名
                        string suffix = excelfiles.FileName.Substring(excelfiles.FileName.LastIndexOf(".") + 1).ToLower();
                        resultName = Guid.NewGuid() + "." + suffix;
                        saveFile(excelfiles, folder, resultName);
                        result += excelfiles.FileName + "|" + resultName + "|" + excelfiles.ContentLength / 1024 + "㉿";
                    }
                    result = result.Remove(result.LastIndexOf("㉿"), 1);
                    Response.Write(result);
                }
                catch (Exception ex)
                {
                    Response.Write("error");
                }
            }
            else
            {
                Response.Write("error");
            }
        }

        /// <summary>
        ///  附件数据录入
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitFile(int cid, int did,string file)
        {
            try
            {
                file = file.Remove(file.LastIndexOf("㉿"), 1);
                string[] files = Regex.Split(file, "㉿", RegexOptions.IgnoreCase);
                for (int i = 0; i < files.Length; i++)
                {
                    string[] str = files[i].Split(new string[]{"|"},StringSplitOptions.RemoveEmptyEntries);
                    var attFile = new Dep_CourseAttFile
                    {
                        CourseId = cid,
                        DepartId = did,
                        Type = 0,
                        Name = str[1],
                        RealName = str[0],
                        CreateTime = DateTime.Now,
                        LoadTimes = 0,
                        FileSize = Convert.ToInt32(str[2]),
                        IsDelete = 0
                    };
                    depAttFileBL.AddCourseAttFile(attFile);
                }
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
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
        /// 考勤发布
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public JsonResult AttendEnd(int courseid, int did)
        {
            
            
                try
                {
                    int sum = depAttBL.ValidaAttStatus(courseid, did);
                    if (sum < 1)
                    {
                        var model = depCoDepBL.GetCourseDept(courseid, did);
                        if (model != null)
                        {
                            model.AttFlag = 1;
                            model.ApprovalFlag = 0;
                            model.SubmitTime = DateTime.Now;
                            model.UserId = CurrentUser == null ? 0 : CurrentUser.UserId;
                            depCoDepBL.UpdateCourseDept(model);
                        }
                        else
                        {
                            var open = new Dep_CourseDept
                            {
                                CourseId = courseid,
                                DepartId = did,
                                AttFlag = 1,
                                OpenFlag = 0,
                                SubmitTime = DateTime.Now,
                                UserId = CurrentUser == null ? 0 : CurrentUser.UserId,
                                ApprovalFlag = 0
                            };
                            depCoDepBL.AddCourseDept(open);
                        }
                        return Json(new
                        {
                            result = 1
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            result = 2
                        }, JsonRequestBehavior.AllowGet);
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
        /// 选择人员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="realName"></param>
        /// <param name="deptName"></param>
        /// <param name="sex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetUserInfoList(int id,int did, string realName, string deptName, int sex, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                string where = " 1=1 ";

                if (!string.IsNullOrWhiteSpace(deptName))
                    where += string.Format(" and DeptName like '%{0}%' ", deptName.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(realName))
                    where += string.Format(" and Realname like '%{0}%' ", realName.ReplaceSql());
                if (sex != 99)
                    where += " and Sex = " + sex;
                where += " And UserType<>'离职人员'";
                var list = depAttBL.GetDepUserList(id, did, out totalCount, pageIndex, pageSize, where);
                foreach (var item in list)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.DeptName = item.DeptName.HtmlXssEncode();
                }

                return Json(new
                {
                    dataList = list,
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
        /// 插入人员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public JsonResult SubmitUser(int courseid, string userids, int Way)
        {
            try
            {
                int a = 0;
                string[] userid = Regex.Split(userids, ",", RegexOptions.IgnoreCase);
                
                for (int i = 0; i < userid.Length; i++)
                {
                    string[] us = Regex.Split(userid[i], "_", RegexOptions.IgnoreCase);
                    us[1] = "0";
                    Sys_User usermodel = userBL.Get(Convert.ToInt32(us[0]));
                    var course = new Dep_CourseOrder
                    {
                        CourseId = courseid,
                        UserId =Convert.ToInt32(us[0]),
                        OrderTime = DateTime.Now,
                        OrderStatus = 1,
                        LearnStatus=0,
                        GetScore=0,
                        PassStatus="0",
                        AttScore=0,
                        EvlutionScore=0,
                        ExamScore=0,
                        DepartSetId = usermodel.DeptId,
                        IsAppoint=2
                    };
                    depOrderBL.InsertDep_CourseOrder(course);
                    if (course.Id>0)
                    {
                        var model=depAttBL.GetDepAttendce(courseid, Convert.ToInt32(us[0]));
                        if (model == null)
                        {
                            var attendce = new Dep_Attendce
                            {
                                CourseId = courseid,
                                UserId = Convert.ToInt32(us[0]),
                                StartTime = DateTime.Now,
                                EndTime = DateTime.Now,
                                Status = 0,
                                ApprovalFlag = 0,
                                Reason = "",
                                DepartSetId = usermodel.DeptId
                            };
                            int rett = depAttBL.AddDepAttend(attendce);
                        }
                    }
                    a++;
                }
                return Json(new
                {
                    result = 1,
                    content = a
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

        /// <summary>
        /// 考勤
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="departSetId"></param>
        /// <param name="isAttFlag"></param>
        /// <returns></returns>
        public JsonResult SubmitStatus(int courseid, int userid, int departSetId, int isAttFlag, string uids, int Way)
        {
            try
            {
                if (uids == "0" && userid > 0 )
                {
                    UpdateStatus(courseid, userid, departSetId, isAttFlag, Way);
                    return Json(new
                    {
                        result = 1
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string[] uid = Regex.Split(uids, ",", RegexOptions.IgnoreCase);
                    for (int i = 0; i < uid.Length; i++)
                    {
                        string[] us = Regex.Split(uid[i], "_", RegexOptions.IgnoreCase);
                        UpdateStatus(courseid, Convert.ToInt32(us[0]), Convert.ToInt32(us[1]), isAttFlag, Way);
                    }
                    return Json(new
                    {
                        result = 1
                    }, JsonRequestBehavior.AllowGet);
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
        /// 更新考勤状态
        /// </summary>
        /// <param name="userIDs"></param>
        public void UpdateStatus(int courseid, int userid, int departSetId, int isAttFlag,int way)
        {
            var coursemodel = depOrderBL.GetDep_CourseOrderByCourseIdAndUserId(courseid, userid);
            if (coursemodel == null)
            {
                var course = new Dep_CourseOrder
                {
                    CourseId = courseid,
                    UserId = userid,
                    OrderTime = DateTime.Now,
                    OrderStatus = 1,
                    LearnStatus = 0,
                    GetScore = 0,
                    PassStatus = "0",
                    AttScore = 0,
                    EvlutionScore = 0,
                    ExamScore = 0,
                    DepartSetId = departSetId,
                    IsAppoint=2 
                };
                depOrderBL.InsertDep_CourseOrder(course);
            }
            else
            {
                if (coursemodel.OrderStatus == 0)
                {
                    coursemodel.OrderStatus = 1;
                    coursemodel.IsAppoint = 2;
                    depOrderBL.UpdateDep_CourseOrder(coursemodel);
                }
            }
            var attmodel = depAttBL.GetDepAttendce(courseid, userid);
            if (attmodel == null)
            {
                var attendce = new Dep_Attendce
                {
                    CourseId = courseid,
                    UserId = userid,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Status = isAttFlag,
                    ApprovalFlag = 0,
                    Reason = "",
                    DepartSetId = departSetId
                };
                depAttBL.AddDepAttend(attendce);
            }
            else
            {
                attmodel.Status = isAttFlag;
                depAttBL.UpdateDepAttend(attmodel);
            }
        }

        /// <summary>
        /// 获取所有课程列表(主办人)
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepAttendceListByU(string courseName, string isMust, int CoState, string StartTime, string EndTime, int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            try
            {
                int totalCount = 0;
                int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
                if (!string.IsNullOrEmpty(courseName))
                {
                    where += string.Format(" AND cc.CourseName like '%{0}%' ", courseName.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(isMust))
                {
                    if (!isMust.Contains(","))
                    {
                        where += string.Format(" AND cc.IsMust={0}", Convert.ToInt32(isMust));
                    }
                }

                if (!string.IsNullOrEmpty(StartTime))
                {
                    where += string.Format(" and CONVERT(varchar(100),cc.StartTime,23) >= '{0}'", DateTime.Parse(StartTime).ToString("yyyy-MM-dd"));
                }
                if (!string.IsNullOrEmpty(EndTime))
                {
                    where += string.Format(" and CONVERT(varchar(100),cc.EndTime,23) <= '{0}'", DateTime.Parse(EndTime).ToString("yyyy-MM-dd"));
                }

                switch (CoState)
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

                var deptattendceList = depAttBL.GetDepAttendceListByUser(uid, out totalCount, pageIndex, pageSize, "StartTime DESC", where);
                foreach (var item in deptattendceList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.RoomName = item.RoomName.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = deptattendceList,
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


        #endregion

        #region 部门树
        /// <summary>
        /// 获取树形结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetdeptByCo(int cid)
        {
            List<Sys_Department> depColist = depAttBL.GetDepartByCId(cid);
            var treeStr = new StringBuilder();
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview\">");
            treeStr.Append("<li id=\"tree0\" class='pNote'>");
            treeStr.Append("<ul>");
            foreach (Sys_Department dept in depColist)
            {
                treeStr.AppendFormat(
                    "<li id='tree{0}' class='pNote'><a title=\"{1}\" dir='dir' onclick=\"selectAtt({0},this);\" id=\"{0}\">{1}({2})</a>",
                    dept.DepartmentId, dept.DeptName, dept.DeptCode);
                treeStr.Append("</li>");
            }
            treeStr.Append("</ul>");
            treeStr.Append("</li>");
            treeStr.AppendLine("</ul>");
            return Json(treeStr.ToString(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 2013-10-11 导出考勤详情列表execl 毛佳源
        /// <summary>
        /// 2013-10-11 导出考勤详情列表execl
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uname"></param>
        /// <param name="dname"></param>
        /// <param name="CState"></param>
        /// <param name="OState"></param>
        public void ExportAttendceList(int id, int deptId, string uname, int AState)
        {
            int totalCount = 0;
            string where = "1=1";
            if (!string.IsNullOrEmpty(uname))
            {
                where += string.Format(" and su.Realname LIKE '%{0}%'", uname.ReplaceSql());
            }
            if (AState > -1)
            {
                if (AState == 0)
                {
                    where += string.Format(" AND (dta.Status IS null or dta.Status=0) ");
                }
                else
                {
                    where += string.Format(" AND dta.Status={0}", AState);
                }
            }

            List<Dep_CourseOrder> attendceList = depAttBL.GetDep_AttendUserListForExport(out totalCount,id, deptId, "asc", where);

            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("部门/分所", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("是否CPA", typeof(string));
            dt.Columns.Add("CPA编号", typeof(string));
            dt.Columns.Add("是否预订课程", typeof(string));
            dt.Columns.Add("获得学时", typeof(decimal));
            dt.Columns.Add("考勤状态", typeof(string));
            //dt.Columns.Add("上课考勤时间", typeof(string));
            //dt.Columns.Add("下课考勤时间", typeof(string));

            var count = 1;
            foreach (Dep_CourseOrder item in attendceList)
            {
                dt.Rows.Add(count++, item.Realname, item.DeptName, item.TrainGrade, item.CPA, "'" + item.CPANo,
                    item.IsAppoint == 2 ? "否" : "是", item.CourseApprovalFlag == 1 ? item.AllScore : 0, item.StatusStr);
            }
            var dtList = new List<DataTable>();
            string strFileName = "考勤管理详情";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);

        }
        #endregion

        public JsonResult fDeleteUser(int courseid,int userid)
        {
            depAttBL.DeleteAttendForCourseAndUser(courseid,userid);

            depAttBL.DeleteCourseOrderForCourseAndUser(courseid,userid);

            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
