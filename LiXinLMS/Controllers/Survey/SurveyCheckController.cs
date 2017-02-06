using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinModels.Survey;
using LiXinModels;
using LiXinCommon;
using LiXinModels.User;

namespace LiXinControllers
{
    public partial class SurveyController
    {
        #region 呈现
        public ActionResult SurveyCheckForDept()
        {
            return View();
        }

        public ActionResult SurveyCheckDeatils(int userNum, int surveyID, int paperID)
        {
            var survey = surveyBL.Get(surveyID);
            survey.useNum = userNum;
            ViewBag.paperID = paperID;
            ViewBag.surveyID = surveyID;
            return View(survey);
        }

        /// <summary>
        /// 0部门负责人的 1事务所的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult surveyResult(int type = 0)
        {
            ViewBag.type = type;
            return View();
        }

        public ActionResult AccessedForDept()
        {
            return View();
        }

        public ActionResult studentResult(int type = 0)
        {
            ViewBag.TrainGrade = trainBL.GetAllTrainGrade();
            ViewBag.type = type;
            return View();
        }

        public ActionResult SurveyCheckForOffice()
        {
            return View();
        }

        public ActionResult SurveyCheckDeatilsForOffice(int userNum, int surveyID, int paperID)
        {
            var survey = surveyBL.Get(surveyID);
            survey.useNum = userNum;
            ViewBag.paperID = paperID;
            ViewBag.surveyID = surveyID;
            return View(survey);
        }
        #endregion

        #region 问卷审核（部门负责人）
        /// <summary>
        /// 问卷审核首页
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="isInDate"></param>
        /// <returns></returns>
        public JsonResult GetSurveyCheckForSurvey(string Name = "", string isInDate = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " StartTime desc")
        {
            try
            {
                var list = surveyBL.GetSurveyCheckForDept(CurrentUser.UserId);
                if (isInDate != "")
                {
                    list = list.Where(p => p.IsInDate == isInDate).ToList();
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    list = list.Where(p => p.Name.ToLower().Contains(Name.Trim().ToLower())).ToList();
                }
                int totalcount = list.Count;

                if (jsRenderSortField.ToLower().Contains("starttime"))
                {
                    list = list.OrderByDescending(p => p.StartTime).ToList();
                    if (jsRenderSortField.ToLower().Contains("asc"))
                    {
                        list = list.OrderBy(p => p.StartTime).ToList();
                    }
                }
                else if (jsRenderSortField.Contains("useNum"))
                {
                    list = list.OrderByDescending(p => p.useNum).ToList();
                    if (jsRenderSortField.ToLower().Contains("asc"))
                    {
                        list = list.OrderBy(p => p.useNum).ToList();
                    }
                }
                list = list.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Survey_Survey>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取问题基本情况
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public JsonResult GetQuestionBySurvey(int surveyID, int paperID)
        {
            try
            {
                var list = surveyBL.GetQuestionBySurvey(surveyID, paperID, CurrentUser.UserId);
                var questionID = 0;
                var qtype = -1;
                var sumresult = "";
                if (list.Count > 0)
                {
                    questionID = list.FirstOrDefault().QuestionID;
                    qtype = list.FirstOrDefault().QuestionType;
                    sumresult = list.FirstOrDefault().sumresult;
                }
                return Json(new
                {
                    result = 1,
                    questionID = questionID,
                    qtype = qtype,
                    dataList = list,
                    sumresult = sumresult
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Survey_Question>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  获取主观题回答的基本情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSubjectReplayAnswer(int surveyID, int paperID, int questionID, string where = "")
        {
            try
            {
                var query = string.Format(" SubjectiveAnswer LIKE '%{0}%'", where.ReplaceSql());
                var list = surveyBL.GetReplayAnswer(surveyID, paperID, questionID, CurrentUser.UserId, query);
                //var sumresult = list.Count(p => p.IsMaster == 1) == 0 ? "" : list.Where(p => p.IsMaster == 1).FirstOrDefault().SubjectiveAnswer;
                return Json(new
                {
                    result = 1,
                    dataList = list
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Survey_ReplyAnswer>(),
                    sumresult = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  获取客观题回答的基本情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetObjectReplayAnswer(int surveyID, int paperID, int questionID, string where = "")
        {
            try
            {
                var query = string.Format(" QuestionReply LIKE '%{0}%'", where.ReplaceSql());
                var pie = new ChartModel();
                var pieSer = new List<pieSeries>();
                var questionAnswer = questionAnswerBL.GetSurvey_QuestionAnswerByQuestionID(questionID);
                var replayAnswer = surveyBL.GetReplayAnswer(surveyID, paperID, questionID, CurrentUser.UserId, query);

                #region 饼图

                foreach (var item in questionAnswer)
                {
                    pieSer.Add(new pieSeries
                    {
                        name = item.ShowOrder,
                        y = replayAnswer.Count(p => p.objectAnswerList.Contains(item.AnswerId.ToString()))
                    });
                }

                pie.DivID = "showpie";
                pie.title = "人数";
                pie.pieseries = pieSer;
                #endregion

                //   replayAnswer = replayAnswer.Where(p => string.IsNullOrEmpty(p.QuestionReply) ? false : p.QuestionReply.Contains(where)).ToList();
                // var sumresult = replayAnswer.Count(p => p.IsMaster == 1) == 0 ? "" : replayAnswer.Where(p => p.IsMaster == 1).FirstOrDefault().SubjectiveAnswer;

                return Json(new
                {
                    result = 1,
                    qAnswer = questionAnswer,
                    rAnswer = replayAnswer,
                    pie = pie
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
                {
                    result = 0,
                    qAnswer = new List<Survey_QuestionAnswer>(),
                    rAnswer = new List<Survey_ReplyAnswer>(),
                    sumresult = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///  获取主观题回答的基本情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetXinPAnswers(int surveyID, int paperID, int questionID)
        {
            try
            {
                var list = surveyBL.GetXinPAnswers(questionID, paperID, surveyID, CurrentUser.UserId);
                var sum = list.Sum(p => p.SubjectiveAnswer == "" ? 0 : Convert.ToInt32(p.SubjectiveAnswer));
                var userNum = list.Count();
                var avgXinP = Math.Round(sum / Convert.ToDouble(userNum), 1, MidpointRounding.AwayFromZero);
                return Json(new
                {
                    result = 1,
                    list = list,
                    avgXinP = avgXinP
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    list = new List<Survey_ReplyAnswer>(),
                    avgXinP = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 插入针对问题总结
        /// </summary>
        /// <param name="subjectanswer"></param>
        /// <returns></returns>
        public JsonResult InsertMasert(int questionID, int surveyID, int paperID, string subjectanswer)
        {
            try
            {

                Survey_ReplyAnswer model = new Survey_ReplyAnswer()
                {
                    ObjectID = surveyID,
                    ExampaperID = paperID,
                    QuestionID = questionID,
                    SubjectiveAnswer = subjectanswer,
                    ObjectiveAnswer = "",
                    UserID = CurrentUser.UserId,
                    IsMaster = 1,
                    SourceFrom = 1,
                    QuestionReply = "",
                    DeptID=CurrentUser.DeptId
                };
                replayAnswerBL.InsertSurvey_ReplyAnswer(model);
                surveyBL.InsertSurvey_DeptSurvey(CurrentUser.DeptId, surveyID);
                return Json(new
                {
                    result = 1,
                    content = "保存成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "保存失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 插入针对问题总结
        /// </summary>
        /// <param name="subjectanswer"></param>
        /// <returns></returns>
        public JsonResult updateMaster(int questionID, int surveyID, int paperID, string subjectanswer)
        {
            try
            {

                Survey_ReplyAnswer model = new Survey_ReplyAnswer()
                {
                    ObjectID = surveyID,
                    ExampaperID = paperID,
                    QuestionID = questionID,
                    SubjectiveAnswer = subjectanswer,
                    ObjectiveAnswer = "",
                    UserID = CurrentUser.UserId,
                    IsMaster = 1,
                    SourceFrom = 1,
                    QuestionReply = "",
                    DeptID = CurrentUser.DeptId
                };
                replayAnswerBL.UpdateReaplyAnswer(model);
                return Json(new
                {
                    result = 1,
                    content = "保存成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "保存失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 采纳或者推荐
        /// </summary>
        /// <param name="answerId"></param>
        /// <param name="type">1部门采纳 2部门推荐给事务所 3事务所采纳</param>
        /// <returns></returns>
        public JsonResult AccessOrOffice(int answerId, int type)
        {
            try
            {
                switch (type)
                {
                    case 1:
                        surveyBL.DeptAccessed(answerId);
                        break;
                    case 2:
                        surveyBL.DeptForOffice(answerId);
                        break;
                    case 3:
                        surveyBL.OfficeAccessed(answerId);
                        break;
                }
                return Json(new
                {
                    result = 1,
                    content = "采纳成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "采纳失败"
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 获取此部门负责人下面的人员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public JsonResult GetStudent(int surveyID, int paperID, string name = "", string grade = "")
        {
            try
            {
                var list = surveyBL.GetDeptUser(surveyID, paperID, CurrentUser.UserId, name.ReplaceSql(),grade.ReplaceSql());
                var userID = list.Count == 0 ? 0 : list.FirstOrDefault().UserId;
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    userID = userID
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_User>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取学员答卷情况
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public JsonResult GetAnswerForStu(int surveyID, int paperID, int userID, int type)
        {
            try
            {
                var stuAnswerList = new List<Survey_ReplyAnswer>();
                var questionList = new List<Survey_Question>();
                if (type == 0)
                {
                    stuAnswerList = surveyBL.GetAnswerForStu(surveyID, paperID, userID);
                    questionList = surveyBL.GetStuQuestion(surveyID, paperID, userID);
                }
                else
                {
                    stuAnswerList = surveyBL.GetAnswerForStu(surveyID, paperID, userID, " IsOfficeAccessed<>0");
                    questionList = surveyBL.GetStuQuestion(surveyID, paperID, userID, " IsOfficeAccessed<>0");
                }
                return Json(new
                {
                    result = 1,
                    questionList = questionList,
                    stuAnswer = stuAnswerList
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
               {
                   result = 0,
                   dataList = new List<Survey_ReplyAnswer>()
               }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 问卷审核（事务所）
        /// <summary>
        /// 问卷审核首页（事务所）
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="isInDate"></param>
        /// <returns></returns>
        public JsonResult GetSurveyCheckForOffice(string Name = "", string isInDate = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " StartTime desc")
        {
            try
            {
                var list = surveyBL.GetSurveyCheckForOffice();
                if (isInDate != "")
                {
                    list = list.Where(p => p.IsInDate == isInDate).ToList();
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    list = list.Where(p => p.Name.ToLower().Contains(Name.Trim().ToLower())).ToList();
                }
                int totalcount = list.Count;
                if (jsRenderSortField.ToLower().Contains("starttime"))
                {
                    list = list.OrderByDescending(p => p.StartTime).ToList();
                    if (jsRenderSortField.ToLower().Contains("asc"))
                    {
                        list = list.OrderBy(p => p.StartTime).ToList();
                    }
                }
                else if (jsRenderSortField.Contains("useNum"))
                {
                    list = list.OrderByDescending(p => p.useNum).ToList();
                    if (jsRenderSortField.ToLower().Contains("asc"))
                    {
                        list = list.OrderBy(p => p.useNum).ToList();
                    }
                }
                list = list.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Survey_Survey>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取问题基本情况
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public JsonResult GetQuestionSurveyForOffice(int surveyID, int paperID, int deptID)
        {
            try
            {
                var list = surveyBL.GetQuestionSurveyForOffice(surveyID, paperID, deptID);
                var questionID = 0;
                var qtype = -1;
                var sumresult = "";
                if (list.Count > 0)
                {
                    questionID = list.FirstOrDefault().QuestionID;
                    qtype = list.FirstOrDefault().QuestionType;
                    sumresult = list.FirstOrDefault().sumresult;
                }

                return Json(new
                {
                    result = 1,
                    questionID = questionID,
                    qtype = qtype,
                    dataList = list,
                    sumresult = sumresult
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Survey_Question>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  获取主观题回答的基本情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSubjectOfficeAnswer(int surveyID, int paperID, int questionID, int deptID, string where = "")
        {
            try
            {
                var query = string.Format(" SubjectiveAnswer LIKE '%{0}%'", where.ReplaceSql());
                var list = surveyBL.GetOfficeReplayAnswer(surveyID, paperID, questionID, CurrentUser.UserId, deptID, query);
                // var sumresult = list.Count(p => p.IsMaster == 1) == 0 ? "" : list.Where(p => p.IsMaster == 1).FirstOrDefault().SubjectiveAnswer;
                return Json(new
                {
                    result = 1,
                    dataList = list
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Survey_ReplyAnswer>(),
                    sumresult = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///  获取客观题回答的基本情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetObjectOfficeAnswer(int surveyID, int paperID, int questionID, int deptID, string where = "")
        {
            try
            {
                var query = string.Format(" QuestionReply LIKE '%{0}%'", where.ReplaceSql());
                var pie = new ChartModel();
                var pieSer = new List<pieSeries>();
                var questionAnswer = questionAnswerBL.GetSurvey_QuestionAnswerByQuestionID(questionID);
                var replayAnswer = surveyBL.GetOfficeReplayAnswer(surveyID, paperID, questionID, CurrentUser.UserId, deptID, query);

                #region 饼图
                foreach (var item in questionAnswer)
                {
                    pieSer.Add(new pieSeries
                    {
                        name = item.ShowOrder,
                        y = replayAnswer.Count(p => p.objectAnswerList.Contains(item.AnswerId.ToString()))
                    });
                }
                pie.DivID = "showpie";
                pie.title = "人数";
                pie.pieseries = pieSer;
                #endregion
                return Json(new
                {
                    result = 1,
                    qAnswer = questionAnswer,
                    rAnswer = replayAnswer,
                    pie = pie
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
                {
                    result = 0,
                    qAnswer = new List<Survey_QuestionAnswer>(),
                    rAnswer = new List<Survey_ReplyAnswer>(),
                    sumresult = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取此部门负责人下面的人员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public JsonResult GetStudentForOffice(int surveyID, int paperID, string name = "", string grade = "", int deptID = 0)
        {
            try
            {
                var list = surveyBL.GetStudentForOffice(surveyID, paperID, name.ReplaceSql(), grade.ReplaceSql(), deptID);
                var userID = list.Count == 0 ? 0 : list.FirstOrDefault().UserId;
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    userID = userID
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_User>()
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///  获取主观题回答的基本情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetXinPAnswersOffice(int surveyID, int paperID, int questionID)
        {
            try
            {
                var list = surveyBL.GetXinPAnswersOffice(questionID, paperID, surveyID);
                var sum = list.Sum(p => p.SubjectiveAnswer == "" ? 0 : Convert.ToInt32(p.SubjectiveAnswer));
                var userNum = list.Count();
                var avgXinP = Math.Round(sum / Convert.ToDouble(userNum), 1, MidpointRounding.AwayFromZero);
                return Json(new
                {
                    result = 1,
                    list = list,
                    avgXinP = avgXinP
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    list = new List<Survey_ReplyAnswer>(),
                    avgXinP = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 部门树的方法
        public ActionResult DeptTree(int surveyID, int paperID)
        {
            int depID = 0;
            var treeStr = GetDeptTreeStr(out depID, surveyID, paperID);
            return Json(new
            {
                depID = depID,
                dataList = treeStr.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public StringBuilder GetDeptTreeStr(out int depID, int surveyID, int paperID)
        {
            string prefix = "";

            var treeStr = new StringBuilder();

            treeStr.AppendFormat("<ul id=\"navigation\" class=\"treeview\"><li id=\"0\"><span title='{0}'  class=\"rootNote\">{0}</span><ul>", "立信组织结构");

            var deptlist = surveyBL.GetDeptForDepartment(surveyID, paperID).OrderByDescending(p => p.IsSubmit).ToList();
            depID = deptlist.Count == 0 ? 0 : (deptlist.FirstOrDefault().IsSubmit == 0 ? 0 : deptlist.FirstOrDefault().DepartmentId);
            // GetDeptTreeChild(deptlist, treeStr, prefix);
            foreach (var v in deptlist)
            {
                if (v.IsSubmit > 0)
                {
                    treeStr.AppendFormat(
                      "<li class='treeChild'>" + prefix +
                      "<a title='{2}'  class='pNote fl' id='{1}' onclick='ShowQuestionForOffice(this)' >{2} </a> <span class='ml5'>已提交</span>", v.DepartmentId, v.DepartmentId, v.DeptName);
                }
                else
                {
                    treeStr.AppendFormat(
                      "<li class='treeChild'>" + prefix +
                      "<span title='{2}'  class='pNote' id='{1}'  >{2} </span><span style='color:Red'>未提交</span>", v.DepartmentId, v.DepartmentId, v.DeptName, v.IsSubmitStr);
                }

            }
            treeStr.AppendLine("</ul>");
            return treeStr;
        }

        #endregion
    }
}
