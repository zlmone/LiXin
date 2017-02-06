using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using LiXinCommon;
using LiXinModels.DepAttendce;
using LiXinControllers.Filter;
using LiXinModels.DepCourseLearn;
using LiXinModels;
using System.Text.RegularExpressions;

namespace LiXinControllers.DepAttendce
{
    public partial class DepAttendceController
    {
        #region 页面
        public ActionResult DepAttendceApprovalList()
        {
            return View();
        }

        public ActionResult DepAttendceApprovalCourseInfor()
        {
            string aa = Request.QueryString["courseID"] + "㉿" + Request.QueryString["depID"];
            int ApprovalFlag = Convert.ToInt32(Request.QueryString["ApprovalFlag"]);
            int AttFlag = Convert.ToInt32(Request.QueryString["AttFlag"]);
            ViewBag.AttFlag = AttFlag;
            ViewBag.ApprovalFlag = ApprovalFlag;
            var att = depCoDepBL.GetCourseDept(Convert.ToInt32(aa.Split('㉿')[0]), Convert.ToInt32(aa.Split('㉿')[1]));
            ViewBag.attApprovalFlag = att.ApprovalFlag;
            ViewBag.aa = aa;
            return View();
        }

        public ActionResult DepAttendceApprovalResource(int courseId, int deptId)
        {
            List<Dep_CourseAttFile> resource = new List<Dep_CourseAttFile>();
            resource = depAttFileBL.GetDepTran_CourseAttFileDownload(courseId, deptId, " IsDelete=0 ");
            ViewBag.resource = resource;
            return View();
        }

        public ActionResult DepAttendceApprovalAllowed()
        {
            string a = Request.QueryString["type"];
            ViewBag.a = a;
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取考勤审批课程列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="ismust">是否必修课</param>
        /// <param name="isopen">是否开放</param>
        /// <param name="appFlag">审批状态</param>
        /// <param name="startTime">开课时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageSize">条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        public JsonResult GetDepAttendceApprovalList(int kind, string courseName, int ismust, int isopen, int appFlag,
                                                     string startTime, string endTime, int pageSize = 20,
                                                     int pageIndex = 1)
        {
            const string order = " a.SubmitTime desc";
            var where = "1=1";
            string whereFlag = " 1=1 ";
            if (!string.IsNullOrEmpty(courseName))
            {
                where += string.Format(" and temp.CourseName like '%{0}%' ", courseName.ReplaceSql());
                whereFlag += string.Format(" and c.CourseName like '%{0}%' ", courseName.ReplaceSql());
            }
            if (ismust != -1)
            {
                where += string.Format(" and temp.IsMust={0}", ismust);
                whereFlag += string.Format(" and c.IsMust={0}", ismust);
            }
            if (isopen != -1)
            {
                where += string.Format(" and temp.OpenFlag={0}", isopen);
                whereFlag += string.Format(" and a.OpenFlag={0}", isopen);
            }


            where += string.Format(" and temp.StartTime between '{0}' and '{1}' ", (string.IsNullOrEmpty(startTime) ? "2000-1-1" : startTime), (string.IsNullOrEmpty(endTime) ? "2050-1-1" : endTime));
            whereFlag += string.Format(" and c.StartTime between '{0}' and '{1}' ", (string.IsNullOrEmpty(startTime) ? "2000-1-1" : startTime), (string.IsNullOrEmpty(endTime) ? "2050-1-1" : endTime));
            switch (kind)
            {
                case 0:
                    whereFlag += " and ((a.AttFlag=1)or(a.AttFlag=0 and a.ApprovalFlag=1)) ";
                    break;
                case 1:
                    whereFlag += " and ((a.AttFlag=1 and a.ApprovalFlag=0)or(a.AttFlag=1 and a.ApprovalFlag is null)) ";
                    break;
                case 2:
                    whereFlag += " and (a.AttFlag=1 and a.ApprovalFlag=1) ";
                    break;
                case 3:
                    whereFlag += " and (a.AttFlag=0 and a.ApprovalFlag=1) ";
                    break;
            }
            List<DepAttendceCourseModel> list = new List<DepAttendceCourseModel>();
            list = depAttBL.GetDepAttendceApprovalList(whereFlag, pageIndex, pageSize, order, where);
            return Json(new { dataList = list, recordCount = list.Count > 0 ? list[0].Totalcount : 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取考勤审批详情中人员列表
        /// </summary>
        /// <param name="realName">姓名</param>
        /// <param name="isMust">出勤状态</param>
        /// <param name="isOpen">审批状态</param>
        /// <param name="courseId">课程名称</param>
        /// <param name="deptId">部门ID</param>
        /// <param name="pageSize">条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        public JsonResult GetDepAttendceUserList(string realName, int isMust, int isOpen, int courseId, string deptId,
                                                 int pageSize = 20, int pageIndex = 1)
        {
            var where = " 1=1 ";
            if (!string.IsNullOrEmpty(realName))
            {
                where += string.Format(" and su.Realname like '%{0}%' ", realName.ReplaceSql());
            }
            if (isMust != -1)
            {
                where += string.Format(" and dta.Status={0} ", isMust);
            }
            int totalCount = 0;
            var list = depAttBL.GetDepAttendUserList(courseId, deptId.StringToInt32(), out totalCount, pageIndex, pageSize, "asc", where);
            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    realName = item.Realname,
                    Sex = item.Sex,
                    Telephone = item.Telephone,
                    Email = item.Email,
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    EndTime = item.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    Status = item.Status,
                    UserId = item.UserId,
                    CourseId = item.CourseId,
                    ApprovalFlag = item.AttFlag,
                    deptId = item.DepartID,
                    CourseLengthDistribute = item.CourseLengthDistribute,
                    CourseLength = item.CourseLength,
                    DeptName = item.DeptName,
                    MobileNum = item.MobileNum
                };
                itemArray[n] = temp;
                n++;
            }
            return Json(new { dataList = itemArray, recordCount = list.Count > 0 ? list[0].totalcount : 0 },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取审批表格
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetRecord(int courseId, int departId, int pageSize = 20, int pageIndex = 1)
        {
            int totalCount = 0;
            var list = depAttRecordBL.GetDepTranApprovalRecordlist(courseId, departId, out totalCount, pageIndex,
                                                                   pageSize);
            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    Realname = item.Realname,
                    SubmitRealname = item.SubmitRealname,
                    ApprovalTimeStr = item.ApprovalTimeStr,
                    SubmitTimeStr = item.SubmitTimeStr,
                    Reason = item.Reason,
                    ApprovalFlag = item.ApprovalFlag
                };
                itemArray[n] = temp;
                n++;
            }
            return Json(new { dataList = itemArray, recordCount = list.Count > 0 ? totalCount : 0 },
                       JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发送短信and邮件
        /// </summary>
        [Filter.SystemLog("考勤审批管理 审批", LogLevel.Info)]
        public JsonResult SendEmailMessage(int courseId, int departId, int type, int sendMessage)
        {
            var Do = "";
            Do = type == 1 ? "审核通过" : "审核拒绝";
            try
            {
                var content = GetFormworkContent(10);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var course = depCoDepBL.CourseName(courseId, departId);
                    var userInfo = depCoDepBL.UserNmae(courseId, departId);
                    var messList = new List<KeyValuePair<string, string>>();
                    var emailList = new List<KeyValuePair<string, string>>();
                    var c = string.Format(content,
                                          userInfo.Realname,
                                          course.CourseName,
                                          DateTime.Now,
                                          Do);
                    if (!string.IsNullOrWhiteSpace(userInfo.MobileNum))
                    {
                        messList.Add(new KeyValuePair<string, string>(userInfo.MobileNum, c.Replace("教育培训部", CurrentUser.DeptName)));
                    }
                    if (!string.IsNullOrWhiteSpace(userInfo.Email))
                    {
                        emailList.Add(new KeyValuePair<string, string>(userInfo.Email, c.Replace("教育培训部", CurrentUser.DeptName)));
                    }
                    if (sendMessage == 1)
                    {
                        if (messList.Count > 0)
                            SendMessage(messList);
                    }
                    else if (sendMessage == 2)
                    {
                        if (emailList.Count > 0)
                            SendEmail(emailList, "您的课程考勤已经被审批，请登录查看详细信息！");
                    }
                    else if (sendMessage == 3)
                    {
                        if (messList.Count > 0)
                            SendMessage(messList);
                        if (emailList.Count > 0)
                            SendEmail(emailList, "您的课程考勤已经被审批，请登录查看详细信息！");
                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 插入审批信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <param name="reason"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CourseAttendce(int courseId, int departId, string reason, int type)
        {
            var list = depCoDepBL.GetCourseOpen(courseId, departId);
            var submintUser = list.UserId;
            var submintTime = list.SubmitTime;
            var isApproval = false;
            var isInsert = false;
            DateTime timeNow = System.DateTime.Now;
            int userId = CurrentUser.UserId;
            int reasult;
            //通过
            if (type == 0)
            {
                reasult = 1;
                isApproval = depCoDepBL.UpDateDepTran_CourseOpen(courseId, departId, 1, 1);
            }
            else
            {
                reasult = 2;
                isApproval = depCoDepBL.UpDateDepTran_CourseOpen(courseId, departId, 0, 1);
            }
            if (isApproval == true)
            {
                isInsert = depAttRecordBL.Insert(courseId, departId, userId, timeNow, reasult, reason, submintUser,
                                                 submintTime);
            }

            return isInsert;
        }

        /// <summary>
        /// 插入所获得的学分
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        public bool DataDeal(int courseId, int departId, int type)
        {
            string[] precent = new string[3];
            decimal score;//考勤
            //decimal EvlutionScore = 0;//课后评估
            //decimal ExamScore = 0;//在线测试
            var list = depAttBL.GetUserList(courseId, departId);

            var depCPAlist=depCpaLearn.GetDepCPALearn();//全部折算CPA信息--by gecc (2013-12-5)
            //读取CPA配置--by gecc (2013-12-5)
            Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 55).FirstOrDefault();
            string[] cpaType = Regex.Split(cpazq.ConfigValue, ",", RegexOptions.IgnoreCase);

            int n = 0;
            var itemArray = new object[list.Count()];
            var isScore = false;
            int IsLeaveP;
            int ApprovalFlagP;
            if (type == 0)
            {
                foreach (var item in list)
                {
                    IsLeaveP = item.IsLeave;
                    ApprovalFlagP = item.ApprovalFlag;
                    precent = item.CourseLengthDistribute.Split(';');
                    if (item.IsPing == 0 && item.IsTest == 0)
                    {
                        if (item.Status == 1 || item.Status == 3 || item.Status == 4 || item.Status == 5)
                        {
                            score = item.CourseLength;
                            //ExamScore = item.CourseLength * Convert.ToDecimal(precent[1]) / 100;
                            //EvlutionScore = item.CourseLength * Convert.ToDecimal(precent[2]) / 100;
                        }
                        else
                        {
                            score = 0;
                        }
                    }
                    else
                    {
                        if (item.Status == 1 || item.Status == 3 || item.Status == 4 || item.Status == 5)
                        {
                            score = item.CourseLength * Convert.ToDecimal(precent[0]) / 100;
                        }
                        else
                        {
                            score = 0;
                        }
                    }
                    if (IsLeaveP == 1 && ApprovalFlagP == 1)
                    {
                        score = 0;
                    }
                    if (type == 0)
                    {
                        depAttBL.UpdatePersonalAttendce(item.CourseId, item.UserId);
                    }
                    //begin  全部折算CPA信息--by gecc (2013-12-5)
                    bool cpatemp = false;
                    if (item.SubordinateSubstation.Contains("上海"))
                    {
                        //部门是否开放
                        if (cpaType[0] == "1")
                        {
                            cpatemp = true;
                        }
                    }
                    else
                    {
                        //分所是否开放
                        if (cpaType[1] == "1")
                        {
                            cpatemp = true;
                        }
                    }
                    //end
                    var isExist = depOrderBL.ExistDeptTran_courseOrder(item.CourseId, item.UserId);
                    if (isExist == false)
                    {
                        if (type == 0)
                        {
                            if (item.Status == 1 || item.Status == 3 || item.Status == 4 || item.Status == 5)
                            {
                                isScore = depOrderBL.InsertDeptTran_courseOrder(item.CourseId, item.UserId,
                                                                                item.DepartSetId,
                                                                                Convert.ToDecimal(score));
                                                                                //Convert.ToDecimal(EvlutionScore),
                                //Convert.ToDecimal(ExamScore));
                                #region 折算CPA--by gecc (2013-12-5)

                                //判断是否是CPA人员 and 是否开放CPA折算
                                if (cpatemp == true && item.IsCPA == 1)
                                {
                                    var depCPA = depCPAlist.Where(p => p.CourseID == item.CourseId && p.UserID == item.UserId).FirstOrDefault();
                                    if (depCPA != null)
                                    {
                                        depCPA.GetLength = depCPA.GetLength + Convert.ToDecimal(score);
                                        depCpaLearn.UpdateDepCPALearn(depCPA);
                                    }
                                    else
                                    {
                                        depCpaLearn.InsertDepCPALearn(new Dep_CpaLearnStatus
                                        {
                                            CourseID = item.CourseId,
                                            UserID = item.UserId,
                                            CpaFlag = 0,
                                            GetLength = Convert.ToDecimal(score),
                                            DepartSetId = item.DepartSetId
                                        });
                                    }
                                }

                                #endregion Model
                            }
                            else
                            {
                                isScore = depOrderBL.InsertDeptTran_courseOrder(item.CourseId, item.UserId,
                                                                                item.DepartSetId,
                                                                                0);
                            }
                        }
                        else
                        {
                            isScore = depOrderBL.InsertDeptTran_courseOrder(item.CourseId, item.UserId, item.DepartSetId,
                                                                            0);
                        }
                    }
                    else
                    {
                        if (type == 0)
                        {
                            if (item.Status == 1 || item.Status == 3 || item.Status == 4 || item.Status == 5)
                            {
                                isScore = depOrderBL.DepTran_CourseOrderUsers(item.CourseId, item.UserId,
                                                                              Convert.ToDecimal(score), 1);
                                                                              //Convert.ToDecimal(EvlutionScore),
                                //Convert.ToDecimal(ExamScore));
                                #region 折算CPA--by gecc (2013-12-5)

                                //判断是否是CPA人员 and 是否开放CPA折算
                                if (cpatemp == true && item.IsCPA == 1)
                                {
                                    var depCPA = depCPAlist.Where(p => p.CourseID == item.CourseId && p.UserID == item.UserId).FirstOrDefault();
                                    if (depCPA != null)
                                    {
                                        depCPA.GetLength = depCPA.GetLength + Convert.ToDecimal(score);
                                        depCpaLearn.UpdateDepCPALearn(depCPA);
                                    }
                                    else
                                    {
                                        depCpaLearn.InsertDepCPALearn(new Dep_CpaLearnStatus
                                        {
                                            CourseID = item.CourseId,
                                            UserID = item.UserId,
                                            CpaFlag = 0,
                                            GetLength = Convert.ToDecimal(score),
                                            DepartSetId = item.DepartSetId
                                        });
                                    }
                                }

                                #endregion Model
                            }
                            else
                            {
                                isScore = depOrderBL.DepTran_CourseOrderUsers(item.CourseId, item.UserId,
                                                                              0, 0);
                            }
                        }
                        else
                        {
                            isScore = depOrderBL.DepTran_CourseOrderUsers(item.CourseId, item.UserId,
                                                                          0, 0);
                        }
                    }
                }
            }
            else
            {
                isScore = true;
            }

            return isScore;
        }
        #endregion

        #region == 附件下载 ==
        /// <summary>
        /// 附件列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="deptId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetFileList(int courseId, int deptId, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                var list = depAttFileBL.GetDepTran_CourseAttFileDownload(courseId, deptId, " IsDelete=0 ");
                int totalCount = list.Count;
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { dataList = list, recordCount = totalCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { dataList = new List<Dep_CourseAttFile>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">附件的真实文件名</param>
        /// <returns></returns>
        public JsonResult IsExistFile(string path)
        {
            if (!System.IO.File.Exists(Server.MapPath(DepTranFileUrl + path)))
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="path">附件的真实文件名</param>
        /// <param name="name">附件名</param>
        public void LoadPrincipleFile(string path, string name, int type = 0)
        {
            var url = type == 0 ? DepTranFileUrl + path : path;
            if (System.IO.File.Exists(Server.MapPath(url)))
            {
                var filePath = Server.MapPath(url); //路径 
                //以字符流的形式下载文件
                var fs = new FileStream(filePath, FileMode.Open);
                var bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开
                var downloadfilename = name;

                if (Request.UserAgent.ToLower().IndexOf("msie") > -1)
                {
                    downloadfilename = HttpUtility.UrlPathEncode(downloadfilename);
                }

                if (Request.UserAgent.ToLower().IndexOf("firefox") > -1)
                {
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + downloadfilename + "\"");
                }

                else
                {
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + downloadfilename);
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(name, Encoding.UTF8));
                }

                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            else
            {
                Response.Write("此文件已不存在");
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteFile(int id)
        {
            try
            {
                depAttFileBL.DeleteFile(id);
                return Json(new
               {
                   result = 1
               }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
               {
                   result = 0
               }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


    }
}
