using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LiXinCommon;
using LiXinControllers;
using LiXinInterface.Examination;
using LiXinInterface.User;
using LiXinModels.Examination.DBModel;
using LiXinModels.Examination.ShowModel;
using LiXinModels.User;

namespace LiXinExam.Controllers
{
    public class ExaminationController : BaseController
    {
        protected IDepartment _departmentBL;
        protected IExamTest _examTestBL;
        protected IExamination _examinationBL;
        protected IExampaper _exampaperBL;
        protected IQuestion _questionBL;
        protected IUser _userBL;

        public ExaminationController(IExamination examinationBL, IUser userBL, IExampaper exampaperBL,
                                     IDepartment department, IQuestion questionBL, IExamTest examTestBL)
        {
            _examinationBL = examinationBL;
            _userBL = userBL;
            _exampaperBL = exampaperBL;
            _departmentBL = department;
            _questionBL = questionBL;
            _examTestBL = examTestBL;
        }

        #region 考试安排

        public ActionResult ExaminationList()
        {
            return View();
        }

        /// <summary>
        /// </summary>
        /// <param name="examBeginTimeStart"></param>
        /// <param name"examBeginTimeEnd"></param>
        /// <param name="examinationTitle"></param>
        /// <param name="startFlag">0:未开始 1：进行中 2：已结束 -1：全部</param>
        /// <param name="orderbyName">0：创建时间 1：考试开始时间 2：考试次数 </param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="orderbyFlag">0：不desc 1：desc </param>
        /// <returns></returns>
        public JsonResult GetExaminationList(string examBeginTimeStart, string examBeginTimeEnd, string examinationTitle,
                                             int startFlag = -1,
                                             int orderbyFlag = 0, int orderbyName = 0, int pageSize = 10,
                                             int pageIndex = 1)
        {
            IEnumerable<tbExamination> result =
                _examinationBL.GetAllExamination().Where(p => p.ExaminationTitle.Contains(examinationTitle));
            if (!string.IsNullOrEmpty(examBeginTimeStart))
            {
                result = result.Where(p => p.ExamBeginTime.ToLocalTime() >= examBeginTimeStart.StringToDate(0));
            }
            if (!string.IsNullOrEmpty(examBeginTimeEnd))
            {
                result = result.Where(p => p.ExamBeginTime.ToLocalTime() <= examBeginTimeEnd.StringToDate(1));
            }
            switch (startFlag)
            {
                case -1:
                    break;
                case 0:
                    result = result.Where(p => DateTime.Now < p.ExamBeginTime.ToLocalTime() && p.PublishedFlag == 1);
                    break;
                case 1:
                    result =
                        result.Where(
                            p =>
                            p.ExamBeginTime.ToLocalTime() < DateTime.Now && DateTime.Now < p.ExamEndTime.ToLocalTime() &&
                            p.PublishedFlag == 1);
                    break;
                case 2:
                    result = result.Where(p => p.ExamEndTime.ToLocalTime() < DateTime.Now && p.PublishedFlag == 1);
                    break;
                case 3:
                    result = result.Where(p => p.PublishedFlag == 0);
                    break;
            }

            switch (orderbyName)
            {
                case 0:
                    if (orderbyFlag == 0)
                        result = result.OrderBy(p => p.CreateTime);
                    else if (orderbyFlag == 1)
                        result = result.OrderByDescending(p => p.CreateTime);
                    break;
                case 1:
                    if (orderbyFlag == 0)
                        result = result.OrderBy(p => p.ExamBeginTime);
                    else if (orderbyFlag == 1)
                        result = result.OrderByDescending(p => p.ExamBeginTime);
                    break;
                case 2:
                    if (orderbyFlag == 0)
                        result = result.OrderBy(p => p.TestTimes);
                    else if (orderbyFlag == 1)
                        result = result.OrderByDescending(p => p.TestTimes);
                    break;
            }

            int totalCount = result.Count();
            result = result.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var itemArray = new object[result.Count()];
            int n = 0;
            foreach (tbExamination item in result)
            {
                string strStatus = "";
                string examTotalScore = "";
                int intPassScore = 0;
                if (item.PublishedFlag == 1)
                {
                    if (item.ExamEndTime.ToLocalTime() > DateTime.Now && DateTime.Now > item.ExamBeginTime.ToLocalTime())
                    {
                        strStatus = "进行中";
                    }
                    else if (item.ExamEndTime.ToLocalTime() < DateTime.Now)
                    {
                        strStatus = "已结束";
                    }
                    else
                    {
                        strStatus = "未开始";
                    }
                }
                else
                {
                    strStatus = "未发布";
                }
                if (item.PercentFlag == 1)//如果不是百分制，那么 通过分数 需要乘以 百分比
                {
                    tbExampaper paper = _exampaperBL.GetExampaper(item.PaperID);
                    intPassScore = Convert.ToInt32(paper.ExampaperScore * item.PassScore * 0.01);
                    examTotalScore = paper.ExampaperScore.ToString();
                }
                else
                {
                    examTotalScore = "100";
                    intPassScore = Convert.ToInt32(item.PassScore);
                }

                var temp = new
                    {
                        item._id,
                        item.ExaminationTitle,
                        item.ExamLength,
                        ExamBeginTime = item.ExamBeginTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
                        ExamEndTime = item.ExamEndTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
                        item.TestTimes,
                        intPassScore,
                        examTotalScore,
                        ExamStaus = strStatus,
                        AuthExamHtml =
                            item.PublishedFlag == 1 ? "disabled='disabled'" : " onclick='AuthExam(" + item._id + ")' ",
                        DetailExamHtml = " onclick='DetailExam(" + item._id + ")' ",
                        ModifyExamHtml =
                            item.PublishedFlag == 1 ? "disabled='disabled'" : " onclick='ModifyExam(" + item._id + ")' ",
                        DeleteExamHtml =
                            item.PublishedFlag == 1 ? "disabled='disabled'" : " onclick='DeleteExam(" + item._id + ")' ",
                        PublishExamHtml =
                            (strStatus == "未发布" && item.PublishedFlag == 0)
                                ? "onclick='PublishExam(" + item._id + ")'"
                                : "disabled='disabled'"
                    };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray.ToList(),
                recordCount = totalCount
            },
                        JsonRequestBehavior.AllowGet);
        }

        //public JsonResult PublishExam(int examId)
        //{
        //    //信息完整，有考生，有批阅人，结束时间大于当前时间
        //    _examinationBL.
        //}

        #endregion

        #region 考试授权

        public ActionResult AuthorizedExamManage(int examinationId)
        {
            ViewBag.examinationId = examinationId;
            return View();
        }

        public ActionResult AuthorizedExam(int examinationId)
        {
            ViewData["examinationId"] = examinationId;
            ViewData["personids"] = examinationId == 0
                                        ? ""
                                        : string.Join(",",
                                                      _examinationBL.GettbExamSendStudent(examinationId).Select(
                                                          p => p.UserID).Distinct());
            return View();
        }

        /// <summary>
        ///     idsFlag  0：部门  1：岗位  2：人员
        /// </summary>
        /// <param name="newIds"> </param>
        /// <param name="idsFlag"></param>
        /// <param name="oldIds"> </param>
        /// <returns></returns>
        public JsonResult ExamAddAuthorizedPerson(string newIds, int idsFlag, string oldIds)
        {
            if (string.IsNullOrEmpty(newIds.Trim()))
                return Json(oldIds, JsonRequestBehavior.AllowGet);
            List<int> newidslist =
                newIds.Trim().Split(',').Distinct().Where(id => !string.IsNullOrEmpty(id)).Select(int.Parse).ToList();
            string NewIds = newidslist.Aggregate("", (current, id) => current + (id + ","));
            if (!string.IsNullOrEmpty(NewIds))
                NewIds = NewIds.Remove(NewIds.Length - 1);

            List<int> newuser;
            if (idsFlag == 0)
                newuser = _userBL.GetList(" DeptId in (" + NewIds + ") ").Select(p => p.UserId).Distinct().ToList();
            else if (idsFlag == 1)
                newuser = _userBL.GetList(" PostId in (" + NewIds + ") ").Select(p => p.UserId).Distinct().ToList();
            else if (idsFlag == 2)
                newuser = newidslist;
            else
                newuser = new List<int>();

            if (newuser.Count == 0)
                return Json(new
                {
                    result = oldIds,
                    Isnull = 1
                }, JsonRequestBehavior.AllowGet);

            var oldidslist = new List<int>();
            if (!string.IsNullOrEmpty(oldIds))
                oldidslist =
                    oldIds.Trim()
                          .Split(',')
                          .Distinct()
                          .Where(id => !string.IsNullOrEmpty(id))
                          .Select(int.Parse)
                          .ToList();
            oldidslist.AddRange(newuser);
            string result = oldidslist.Distinct().Aggregate("", (current, id) => current + (id + ","));
            if (!string.IsNullOrEmpty(result))
                result = result.Remove(result.Length - 1);
            //Request.Cookies

            return Json(new
            {
                result,
                Isnull = 0
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExamDelAuthorizedPerson(string newIds, string oldIds)
        {
            List<string> result =
                oldIds.Trim().Split(',').Distinct().Where(id => !string.IsNullOrEmpty(id)).ToList();
            List<string> personids = newIds.Trim().Split(',').Distinct().Where(id => !string.IsNullOrEmpty(id)).ToList();
            result.RemoveAll(personids.Contains);

            string r = result.Distinct().Aggregate("", (current, id) => current + (id + ","));
            if (!string.IsNullOrEmpty(r))
                r = r.Remove(r.Length - 1);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAuthorizedExamUserList(string userIds, string personName, string personCode,
                                                    string personPost, string personDepartment, int pageSize = 20,
                                                    int pageIndex = 1)
        {
            if (string.IsNullOrEmpty(userIds.Trim()))
                return Json(new
                {
                    result = 1,
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                },
                            JsonRequestBehavior.AllowGet);

            List<Sys_User> keyList =
                _userBL.GetList(" UserId in (" + userIds + ") ").Where(
                    p =>
                    p.Realname.Contains(personName) && p.JobNum.Contains(personCode) &&
                    p.DeptCode.Contains(personDepartment)).ToList();
            int totalCount = keyList.Count;
            keyList = keyList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                result = 1,
                dataList = keyList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangtbExamSendStudent(string personids, int examId)
        {
            try
            {
                return Json(new
                    {
                        result = _examinationBL.SaveExamSendStudent(examId,
                                                                    personids.Split(',').Distinct().Where(
                                                                        p => !string.IsNullOrEmpty(p)).
                                                                              Select(int.Parse))
                                     ? 1
                                     : 0
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = 0,
                    content = ex.ToString()
                });
            }
        }


        /// <summary>
        ///     先判断是新增还是修改，还是直接发布
        ///     如果examId =0 那么是新增考试，需要判断personids
        ///     如果examId!=0 并且 persionids !=""那么是 修改/发布 考试，需要判断persionids
        ///     如果examId!=0 并且 persionids ==""那么是 列表中的发布考试，无需判断persions  strUser从数据库中获取
        /// </summary>
        /// <param name="examId"></param>
        /// <param name="approveIds"></param>
        /// <param name="personids"></param>
        /// <returns></returns>
        private int JudgeCanPublishExam(int examId, string approveIds, string personids = "")
        {
            string strUser = "";
            if (examId != 0)
            {
                if (personids != "")
                {
                    strUser = personids;
                }
                else
                {
                    strUser = string.Join(",",
                                          _examinationBL.GettbExamSendStudent(examId).Select(p => p.UserID).Distinct());
                }
            }
            else
            {
                strUser = personids;
            }
            //参加考试的人员

            if (!string.IsNullOrEmpty(strUser))
            {
                if (strUser.EndsWith(","))
                {
                    strUser.Remove(strUser.Length);
                }
                if (_userBL.GetList(" UserId in(" + strUser + ")").Count <= 0)
                {
                    return 1; //考试考生不存在！
                }
            }
            else
            {
                return 1;
            }


            if (!string.IsNullOrEmpty(approveIds))
            {
                if (approveIds.EndsWith(","))
                {
                    approveIds.Remove(strUser.Length);
                }
                if (_userBL.GetList(" UserId in(" + approveIds + ")").Count <= 0)
                {
                    return 2; //考试批阅人不存在！       
                }
            }
            else
            {
                return 2;
            }

            return 0;
        }

        #endregion

        #region 新增考试

        /// <summary>
        ///     新增或修改考试
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult EditExamination(int? Id)
        {
            var exam = new tbExamination();
            if (Id.HasValue)
            {
                exam = _examinationBL.GetExamination(Id.Value);
                if (exam != null)
                {
                    exam.ExamBeginTime = exam.ExamBeginTime.ToLocalTime();
                    exam.ExamEndTime = exam.ExamEndTime.ToLocalTime();
                    exam.LastUpdateTime = exam.LastUpdateTime.ToLocalTime();
                }
            }
            string ApprovalUserIds = exam.ApprovalUser == null
                                         ? ""
                                         : exam.ApprovalUser.Aggregate("", (current, id) => current + (id + ","));
            if (!string.IsNullOrEmpty(ApprovalUserIds))
                ApprovalUserIds = ApprovalUserIds.Remove(ApprovalUserIds.Length - 1);
            if (exam != null)
            {
                tbExampaper paper = _exampaperBL.GetExampaper(exam.PaperID);
                if (paper != null)
                {
                    ViewBag.PaperName = paper.ExampaperTitle;
                }
            }

            ViewBag.ApprovalUserIds = ApprovalUserIds;
            return View(exam);
        }

        /// <summary>
        ///     考试详细
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ExaminationDetail(int Id)
        {
            tbExamination exam = _examinationBL.GetExamination(Id);
            if (exam != null)
            {
                exam.ExamBeginTime = exam.ExamBeginTime.ToLocalTime();
                exam.ExamEndTime = exam.ExamEndTime.ToLocalTime();
                exam.LastUpdateTime = exam.LastUpdateTime.ToLocalTime();

                string ApprovalUserIds = exam.ApprovalUser == null
                                             ? ""
                                             : exam.ApprovalUser.Aggregate("", (current, id) => current + (id + ","));
                if (!string.IsNullOrEmpty(ApprovalUserIds))
                    ApprovalUserIds = ApprovalUserIds.Remove(ApprovalUserIds.Length - 1);
                tbExampaper paper = _exampaperBL.GetExampaper(exam.PaperID);
                if (paper != null)
                {
                    ViewBag.PaperName = paper.ExampaperTitle;
                }
                ViewData["personids"] = string.Join(",",
                                                    _examinationBL.GettbExamSendAllStudent(Id)
                                                                  .Select(p => p.UserID)
                                                                  .Distinct());
                ViewBag.ApprovalUserIds = ApprovalUserIds;
            }
            else
            {
                exam = new tbExamination();
                ViewBag.Msg = "对不起，该考试不存在或已被删除！";
            }
            return View(exam);
        }

        /// <summary>
        ///     Add Or Modify Examination
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitEditExamination(tbExamination exam, string personIds, string ApprovalUser)
        {
            string content = "";
            int result = 0;
            int examId = exam._id > 0 ? exam._id : 0;
            if (exam.ExamBeginTime > DateTime.Now)
            {
                int flag = JudgeCanPublishExam(examId, ApprovalUser, personIds);
                if (flag == 0)
                {
                    if (string.IsNullOrEmpty(ApprovalUser))
                        exam.ApprovalUser = new List<int>();
                    else
                        exam.ApprovalUser =
                            ApprovalUser.Split(',')
                                        .Distinct()
                                        .Where(p => !string.IsNullOrEmpty(p))
                                        .Select(int.Parse)
                                        .ToList();
                    if (exam._id > 0)
                    {
                        tbExamination tempExam = _examinationBL.GetExamination(exam._id);
                        examId = exam._id;
                        if (tempExam != null)
                        {
                            tempExam.ApprovalUser = exam.ApprovalUser;
                            tempExam.ExamBeginTime = exam.ExamBeginTime;
                            tempExam.ExamEndTime = exam.ExamEndTime;
                            tempExam.ExaminationTitle = exam.ExaminationTitle;
                            tempExam.ExamLength = exam.ExamLength;
                            tempExam.PaperID = exam.PaperID;
                            tempExam.PassScore = exam.PassScore;
                            tempExam.PercentFlag = exam.PercentFlag;
                            tempExam.PublishedFlag = exam.PublishedFlag;
                            tempExam.RadomOrderFlag = exam.RadomOrderFlag;
                            tempExam.ShowType = exam.ShowType;
                            tempExam.TestTimes = exam.TestTimes;
                            // tempExam.UserID = exam.UserID;不更新创建人
                            result = _examinationBL.ModifyExamination(tempExam) ? 1 : 0;
                            content = "修改成功！";
                        }
                    }
                    else
                    {
                        exam.CreateTime = DateTime.Now;
                        exam.UserID = Session["userID"].StringToInt32();
                        result = _examinationBL.AddExamination(exam) > 0 ? 1 : 0;
                        content = "新增成功！";
                        examId = exam._id;
                    }
                    if (exam.PublishedFlag == 1)
                    {
                        content = "发布成功！";
                    }
                    ChangtbExamSendStudent(personIds, exam._id); //授权参与考试的人员
                }
                else
                {
                    result = 3;
                    content = flag == 1 ? "该考试的所授权的考生无效！" : "该考试所添加的批阅人无效！";
                }
            }
            else
            {
                result = 3;
                content = "考试开始时间不能小于服务器系统时间！";
            }
            return Json(new
            {
                result,
                content,
                examId
            }, JsonRequestBehavior.DenyGet);
        }


        /// <summary>
        ///     发布考试，如果考试已经开始则不允许发布
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitPublishExam(int Id)
        {
            int result = 0;
            string content = "";
            tbExamination exam = _examinationBL.GetExamination(Id);
            if (exam != null)
            {
                int flag = JudgeCanPublishExam(Id, string.Join(",", exam.ApprovalUser));
                if (flag == 0)
                {
                    if (exam.ExamEndTime.ToLocalTime() > DateTime.Now)
                    {
                        exam.PublishedFlag = 1;
                        result = _examinationBL.ModifyExamination(exam) ? 1 : 0;
                        content = "发布成功！";
                    }
                    else
                    {
                        content = "对不起，该考试时间已经结束，不能发布！";
                    }
                }
                else
                {
                    result = 3;
                    content = flag == 1 ? "发布失败！<br/> 原因：该考试的所授权的考生无效！" : "发布失败！<br/> 原因：该考试所添加的批阅人无效！";
                }
            }
            return Json(new
            {
                result,
                content
            }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        ///     删除考试
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteExam(int Id)
        {
            int result = 0;
            string content = "";
            tbExamination exam = _examinationBL.GetExamination(Id);
            if (exam != null)
            {
                if (exam.PublishedFlag == 0)
                {
                    exam.Status = 1;
                    result = _examinationBL.ModifyExamination(exam) ? 1 : 0;
                    content = "删除成功！";
                }
                else
                {
                    content = "对不起，该考试已经发布，不能删除！";
                }
            }
            else
            {
                content = "该考试已在其他地方被删除！";
            }
            return Json(new
            {
                result,
                content
            }, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        ///     考试名称是否存在
        /// </summary>
        /// <param name="ExamTitle"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsExistExaminationTitle(string ExamTitle, int? id)
        {
            return Json(_examinationBL.IsExistExamTitle(ExamTitle, id.HasValue ? id.Value : 0),
                        JsonRequestBehavior.DenyGet);
        }


        public JsonResult GetUserByIds(string ids)
        {
            //ids.Trim().Split(',').Distinct().Where(id => !string.IsNullOrEmpty(id)).Select(int.Parse).ToList();
            if (string.IsNullOrEmpty(ids))
                return Json(new List<Sys_User>(), JsonRequestBehavior.AllowGet);

            List<Sys_User> result = _userBL.GetList(" UserId in (" + ids + ") ").ToList();

            return Json(new
                {
                    data = result,
                    ids = result.Select(p => p.UserId).Aggregate("", (current, id) => current + (id + ",")).TrimEnd(',')
                }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 考试复评

        public ActionResult ReEvaluationList()
        {
            return View();
        }

        public ActionResult ReEvaluation(int Id)
        {
            ViewBag.ExamId = Id;
            return View();
        }


        /// <summary>
        ///     获得可以复评的考试：考试已经结束、考试已经发布
        /// </summary>
        /// <param name="examBeginTimeStart"></param>
        /// <param name="examBeginTimeEnd"></param>
        /// <param name="examinationTitle"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetReEvaluationList(string examBeginTimeStart, string examBeginTimeEnd,
                                              string examinationTitle, int pageSize = 20, int pageIndex = 1)
        {
            IEnumerable<tbExamination> result = _examinationBL.GetAllExamination().Where(p =>
                                                                                         p.ExamEndTime.ToLocalTime() <
                                                                                         DateTime.Now &&
                                                                                         p.PublishedFlag == 1 &&
                                                                                         p.ExaminationTitle.Contains(
                                                                                             examinationTitle) &&
                                                                                         p.ApprovalUser.Contains(
                                                                                             CurrentUser.UserId));
            if (!string.IsNullOrEmpty(examBeginTimeStart))
            {
                result = result.Where(p => p.ExamBeginTime.ToLocalTime() >= examBeginTimeStart.StringToDate(0));
            }
            if (!string.IsNullOrEmpty(examBeginTimeEnd))
            {
                result = result.Where(p => p.ExamBeginTime.ToLocalTime() <= examBeginTimeEnd.StringToDate(1));
            }

            int totalCount = result.Count();
            result = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var itemArray = new object[result.Count()];
            int n = 0;
            foreach (tbExamination item in result)
            {
                int intPassScore = 0;
                string examTotalScore = "";
                if (item.PercentFlag == 1)
                {
                    tbExampaper paper = _exampaperBL.GetExampaper(item.PaperID);
                    intPassScore = Convert.ToInt32(paper.ExampaperScore * item.PassScore * 0.01);
                    examTotalScore = paper.ExampaperScore.ToString();
                }
                else
                {
                    examTotalScore = "100";
                    intPassScore = Convert.ToInt32(item.PassScore);
                }

                var temp = new
                    {
                        item._id,
                        item.ExaminationTitle,
                        item.ExamLength,
                        ExamBeginTime = item.ExamBeginTime.ToLocalTime().ToString(),
                        ExamEndTime = item.ExamEndTime.ToLocalTime().ToString(),
                        item.TestTimes,
                        PassScore = intPassScore,
                        examTotalScore,
                        PublishResultHtml =
                            item.PublishResult == 1
                                ? "disabled='disabled'"
                                : " onclick='PublishResultExam(" + item._id + ")' ",
                        ReEvaluationHtml =
                            item.PublishResult == 1
                                ? "disabled='disabled'"
                                : " onclick='ReEvaluationExam(" + item._id + ")' "
                    };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray.ToList(),
                recordCount = totalCount
            },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     发布考试成绩
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PublishExamResult(int Id)
        {
            int result = 0;
            string content = "";
            tbExamination exam = _examinationBL.GetExamination(Id);
            if (exam != null)
            {
                if (exam.PublishedFlag == 1)
                {
                    exam.PublishResult = 1;
                    result = _examinationBL.ModifyExamination(exam) ? 1 : 0;
                    content = "发布成绩成功！！";
                }
                else
                {
                    content = "对不起，该考试尚未对学员发布，不能发布成绩！";
                }
            }
            else
            {
                content = "该考试已在其他地方被删除！";
            }
            return Json(new
            {
                result,
                content
            }, JsonRequestBehavior.DenyGet);
        }


        /// <summary>
        ///     获取某门考试下的学生列表
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userName"></param>
        /// <param name="deptName"></param>
        /// <returns></returns>
        public JsonResult GetAllSendStudentByExamId(int Id, string userName, string deptName, int pageSize = 20,
                                                    int pageIndex = 1)
        {
            var listStudentId = new List<int>();
            var listDeptId = new List<int>();
            List<tbExamSendStudent> examSendStudentList = _examinationBL.GetAllStudentByExamId(Id);
            //if (examSendStudentList.Count > 0)
            //{
            examSendStudentList.ForEach(e => listStudentId.Add(e.UserID));
            //} 
            //var departmentList = _departmentBL.GetListByDeptName(deptName);
            //if (departmentList.Count > 0)
            //{
            //departmentList.ForEach(d => listDeptId.Add(d.DepartmentId));
            //}


            string strWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(userName))
            {
                strWhere += string.Format(" AND Sys_User.Realname like '%{0}%' ", userName);
            }
            if (!string.IsNullOrEmpty(deptName))
            {
                strWhere += string.Format(" and Sys_Department.DeptName like '%{0}%'", deptName);
            }

            var userList = _userBL.GetList(strWhere);

            var result = userList.Where(u => listStudentId.Contains(u.UserId));
            var totalCount = result.Count();
            result = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var itemArray = new object[result.Count()];
            var n = 0;
            foreach (Sys_User item in result)
            {
                var tempExamSend = examSendStudentList.Where(e => e.UserID == item.UserId).FirstOrDefault();
                var temp = new
                    {
                        item.UserId,
                        Username = item.Realname, //RealName在此处修改 View页面不变
                        examUserId = tempExamSend._id,
                        doExamStatus = tempExamSend.DoExamStatus,
                    };
                itemArray[n] = temp;
                n++;
            }

            var exam = _examinationBL.GetExamination(Id);

            return
                Json(
                    new
                        {
                            result = 1,
                            dataList = itemArray.ToList(),
                            recordCount = totalCount,
                            examTitle = exam.ExaminationTitle
                        }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     根据 考生考试关联ID 获取考生答题详情
        /// </summary>
        /// <returns></returns>
        public JsonResult GetReStudentExamAnswerByExamUserId(int examUserId)
        {
            tbExamSendStudent examSendStudent = _examinationBL.GetExamSendStudent(examUserId);
            ExamBaseInforShow exam = _examTestBL.GetExamBaseInforShow(examUserId);
            tbExampaper examPaper = _exampaperBL.GetExampaper(examSendStudent.ExamPaperID);
            var itemArray = new object[examSendStudent.StudentAnswerList.Count];
            int n = 0;

            string questionTypeHtml = "";
            int q1Count = examSendStudent.StudentAnswerList.Count(q => q.QType == 1);
            int q2Count = examSendStudent.StudentAnswerList.Count(q => q.QType == 2);
            int q3Count = examSendStudent.StudentAnswerList.Count(q => q.QType == 3);
            int q4Count = examSendStudent.StudentAnswerList.Count(q => q.QType == 4);
            int q5Count = examSendStudent.StudentAnswerList.Count(q => q.QType == 5);
            int q6Count = examSendStudent.StudentAnswerList.Count(q => q.QType == 6);

            questionTypeHtml += q1Count > 0 ? "<span>问答题 ( " + q1Count + " )</span>" : "";
            questionTypeHtml += q2Count > 0 ? "<span>单选题 ( " + q2Count + " )</span>" : "";
            questionTypeHtml += q3Count > 0 ? "<span>多选题 ( " + q3Count + " )</span>" : "";
            questionTypeHtml += q4Count > 0 ? "<span>判断题 ( " + q4Count + " )</span>" : "";
            questionTypeHtml += q5Count > 0 ? "<span>填空题 ( " + q5Count + " )</span>" : "";
            questionTypeHtml += q6Count > 0 ? "<span>多媒体题 ( " + q6Count + " )</span>" : "";

            string answerCaseHtml = "";
            answerCaseHtml += "<span>答错: " + examSendStudent.StudentAnswerList.Count(p => p.GetScore == 0 && p.Answer != "") + "</span>";
            answerCaseHtml += "<span>未答: " + examSendStudent.StudentAnswerList.Count(p => p.Answer == "") + "</span>";
            answerCaseHtml += "<span>正确: " + examSendStudent.StudentAnswerList.Count(p => p.GetScore > 0) + "</span>";
            int totalScore = 0;
            foreach (ReStudentExamAnswer item in examSendStudent.StudentAnswerList)
            {
                //根据Qid 查Question表
                tbQuestion question = _questionBL.GetSingleByID(item.Qid);
                string multi_mediaHtml = "";
                if (question.QuestionType == 6)
                {
                    string name = question.FileUpload[0].FileName;
                    switch (question.FileUpload[0].FileType)
                    {
                        case 0:
                            {
                                multi_mediaHtml = "    <table class='all80 cen'>" +
                                                  "<tr>" +
                                                  "<td class='all20' align='center' valign='middle'>" +
                                                  " <a id='k-prev' style='position:relative;' onclick='turnToNext(this,\"left\");' ></a>" +
                                                  "</td>" +
                                                  "<td id='imageCollection' align='center' style='height: 300px;'>" +
                                                  "<input type='hidden' value='1' />";
                                for (int i = 0; i < question.FileUpload.Count; i++)
                                {
                                    multi_mediaHtml += "<img src='../../ClientBin/UploadFile/" +
                                                       question.FileUpload[i].FileName +
                                                       "' style='width:250px; height:250px; " +
                                                       (i == 0 ? " display:block; " : " display:none; ") + "' />";
                                }
                                multi_mediaHtml += "     </td>" +
                                                   "<td class='all20' align='center' valign='middle'>" +
                                                   "<a id='k-next' style='position:relative;' onclick='turnToNext(this,\"right\");' ></a>" +
                                                   "</td>" +
                                                   "</tr>" +
                                                   "</table>";
                            }
                            break;
                        case 1:
                            {
                                multi_mediaHtml +=
                                   @"<embed class='mLeft_2' src='../../Scripts/mp3player/player.swf?url=../../ClientBin/UploadFile/"+name+"&amp;autoplay=0;autostart=0' type='application/x-shockwave-flash' wmode='transparent'  allowscriptaccess='always' height='25' width='400'></embed>"; 
                            }
                            break;
                        case 2:
                            {
                                var id = name.Substring(0, name.Length - 4);
                                multi_mediaHtml +=
                                   @"<input name='FlvName' value='" + name +
                                   @"' type='hidden' /><div class='mLeft_2'><div id='" + id + "'></div></div>";
                            }
                            break;
                    }
                }

                ReStudentExamAnswer reStuExamAnswer =
                    examSendStudent.StudentAnswerList.Where(a => a.Qid == item.Qid).FirstOrDefault();
                string qUserAnswerHtml = "";
                string qRightAnswerHtml = "";
                switch (item.QType)
                {
                    case 1:
                        qUserAnswerHtml = "<p><textarea class='Boxarea all60' disabled='disabled'>" + item.Answer + "</textarea></p>";
                        qRightAnswerHtml = question.QuestionAnswer[0].Answer;
                        break;
                    case 2:
                    case 3:
                        foreach (QuestionAnswer itemQueAnswer in question.QuestionAnswer.OrderBy(q => q.Order))
                        {
                            qRightAnswerHtml += itemQueAnswer.AnswerFlag == 1
                                                    ? (qRightAnswerHtml == ""
                                                           ? ((char)(itemQueAnswer.Order + 64)).ToString()
                                                           : (". " + ((char)(itemQueAnswer.Order + 64))))
                                                    : "";

                            qUserAnswerHtml += " <p><input disabled='disabled' type='" +
                                               (item.QType == 2 ? "radio" : "checkbox") + "'" +
                                               (("," + item.Answer + ",").IndexOf("," + itemQueAnswer.Order + ",") >= 0
                                                    ? "checked='checked'"
                                                    : "") + "name='answer_" + question._id + "' />" +
                                               ((char)(itemQueAnswer.Order + 64)) + ". " + itemQueAnswer.Answer + "</p>";
                        }
                        break;
                    case 4:
                        qRightAnswerHtml = question.QuestionAnswer[0].Answer == "0" ? "A. 正确" : "B. 错误";
                        // 判断题
                        qUserAnswerHtml = "<p><input disabled='disabled' type='radio' name='answer_" + question._id +
                                          "'" + (item.Answer == "0" ? "checked='checked'" : "") +
                                          "/>A. 正确</p> <p><input disabled='disabled' type='radio' name='answer_" +
                                          question._id + "'" + (item.Answer == "1" ? "checked='checked'" : "") +
                                          " />B. 错误</p>";
                        break;
                    case 5:
                        qRightAnswerHtml = question.QuestionAnswer[0].Answer.Replace("!!%%!!", " ");
                        // 填空题 
                        qUserAnswerHtml = " <p>学员答案：" + (item.Answer.Replace("##**##", " ")) + "</p>";

                        break;
                    case 6:
                        int type = question.QuestionAnswer[0].AnswerType;
                        if (type == 0)
                        {
                            qRightAnswerHtml = question.QuestionAnswer[0].Answer;
                            //  问答题
                            qUserAnswerHtml = "<p><textarea class='Boxarea all60' disabled='disabled'>" + (item.Answer) +
                                              "</textarea></p>";
                        }
                        else
                        {
                            //单选题 
                            foreach (QuestionAnswer an in question.QuestionAnswer.OrderBy(p => p.Order))
                            {
                                qRightAnswerHtml += an.AnswerFlag == 1
                                                        ? (qRightAnswerHtml == ""
                                                               ? ((char)(an.Order + 64)).ToString()
                                                               : (". " + ((char)(an.Order + 64))))
                                                        : "";
                                qUserAnswerHtml += " <p><input disabled='disabled' type='" +
                                                   (type == 1 ? "radio" : "checkbox") + "'" +
                                                   (("," + item.Answer + ",").IndexOf("," + an.Order + ",") >= 0
                                                        ? "checked=checked"
                                                        : "") + " name='answer_" + question._id + "' />" +
                                                   ((char)(an.Order + 64)) + ". " + (an.Answer) + "</p>";
                            }
                        }

                        break;
                    default:
                        break;
                }
                var temp = new
                    {
                        item.Qid,
                        QOrder = item.Order,
                        QuestionContent = "<h5>" + question.QuestionContent + "</h5><div class='db'>" + multi_mediaHtml + "</div>",
                        UserAnswer = qUserAnswerHtml,
                        QuestionAnswer = qRightAnswerHtml,
                        UserGetScore = reStuExamAnswer.GetScore,
                        QuestionScore = item.Score
                    };
                totalScore += reStuExamAnswer.GetScore;
                itemArray[n] = temp;
                n++;
            }

            return
                Json(
                    new
                        {
                            result = 1,
                            dataList = itemArray.ToList(),
                            questionTypeHtml,
                            answerCaseHtml,
                            examTitle = exam.ExamTitle,
                            totalScore
                        }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     复评 保存修改后的成绩
        /// </summary>
        /// <param name="examUserId"></param>
        /// <param name="qorderScore"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitSaveReEvaluation(int examUserId, string qorderScore)
        {
            int totalScore = 0;
            string answerCaseHtml = "";
            int result = 0;
            string content = "";
            tbExamSendStudent examSendStudent = _examinationBL.GetExamSendStudent(examUserId);
            if (examSendStudent != null)
            {
                if (qorderScore.EndsWith(";"))
                {
                    qorderScore = qorderScore.Substring(0, qorderScore.Length - 1);
                }
                List<string> list = qorderScore.Split(new[] { ';' }).ToList();
                foreach (string itemStr in list)
                {
                    string[] strArray = itemStr.Split(new[] { ',' });
                    foreach (ReStudentExamAnswer item in examSendStudent.StudentAnswerList)
                    {
                        if (item.Order == int.Parse(strArray[0]))
                        {
                            item.GetScore = int.Parse(strArray[1]);
                        }
                    }
                }
                //通过还是不通过
                var gScore = examSendStudent.StudentAnswerList.Sum(p => p.GetScore);
                var tScore = examSendStudent.StudentAnswerList.Sum(p => p.Score);
                if (tScore == 0)
                {
                    examSendStudent.IsPass = 0;
                }
                else
                {
                    examSendStudent.IsPass = gScore >= examSendStudent.PaperScore ? 1 : 0;
                }
                if (_examinationBL.ModifyExamSendStudent(examSendStudent))
                {
                    result = 1;
                    answerCaseHtml += "答错:" +
                                      examSendStudent.StudentAnswerList.Count(p => p.GetScore == 0 && p.Answer != "");
                    answerCaseHtml += "未答:" + examSendStudent.StudentAnswerList.Count(p => p.Answer == "");
                    answerCaseHtml += "正确:" + examSendStudent.StudentAnswerList.Count(p => p.GetScore > 0);
                    totalScore = examSendStudent.StudentAnswerList.Sum(p => p.GetScore);
                    content = "复评成功！";
                }
                else
                {
                    content = "复评失败！";
                }
            }
            else
            {
                content = "该条数据已被删除，无法进行复评操作！";
            }

            return Json(new
            {
                result,
                content,
                totalScore,
                answerCaseHtml
            }, JsonRequestBehavior.DenyGet);
        }

        #endregion
    }
}