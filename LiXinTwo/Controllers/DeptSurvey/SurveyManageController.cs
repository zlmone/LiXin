using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinModels.Survey;
using LiXinBLL.DeptSurvey;
using LiXinInterface.DeptSurvey;
using LiXinLanguage;
using LiXinCommon;
using LiXinModels;
using LiXinModels.User;
using LiXinBLL.DeptSystemManage;
using LiXinControllers.Filter;

namespace LiXinControllers.DeptSurvey
{
    public partial class DeptSurveyController : BaseController
    {
        #region 呈现
        public ActionResult DeptSurveyManage()
        {
            ViewBag.DepartList = GetAllSubDepartments();
            return View();
        }
        public ActionResult DeptSurveyEdit(int DeptID, int id = 0)
        {
            Survey_Survey survey = new Survey_Survey();
            ViewBag.DeptID = DeptID;
            ViewBag.id = id;
            if (id > 0)
            {
                survey = surveyBL.Get(id);

                if (survey.OpenGroupFlag == 1)//群组
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + survey.OpenGroup + "')) ";
                    ViewBag.Groups = sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                }
                if (survey.OpenDepartFlag == 1)//组织结构
                {
                    string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + survey.OpenDepart + "')) ";
                    ViewBag.Departs = deptBL.GetAllList(strWhere);
                }
            }
            string star = "";
            string end = "";
            var list = new ParamConfigBl().GetConfig(CurrentUser.DeptId, 1);
            var TrainGradeTime = trainBL.IsUpdateTrain(out star, out end, list, DateTime.Now.Date);
            ViewBag.start = star == "" ? "" : Convert.ToDateTime(star).ToString("yyyy-MM-dd");
            ViewBag.end = end == "" ? "" : Convert.ToDateTime(end).ToString("yyyy-MM-dd");
            return View(survey);
        }

        /// <summary>
        /// 调查问题浏览
        /// </summary>
        /// <param name="surveyID">ID</param>
        /// <returns></returns>
        public ActionResult DeptBrowseSurvey(int surveyID, int paperID = 0, string backurl = "")
        {
            ViewBag.backurl = backurl;

            var survey = surveyBL.Get(surveyID);
            var examPaper = SeBl.GetSurveyExampaper(survey.PaperID);
            survey.examPaper = examPaper;
            if (paperID > 0)
            {
                var replayAnswer = replayAnswerBL.GetAnswerBySurvey(surveyID, paperID, CurrentUser.UserId);
                foreach (var item in examPaper.Questions)
                {
                    item.ReplyAnswer = replayAnswer.Where(p => p.QuestionID == item.QuestionID).FirstOrDefault();
                }
            }
            ViewBag.Flag = 0;
            return View(survey);
        }

        public ActionResult DeptDoSurveyList()
        {
            return View();
        }


        public ActionResult DeptDoSurvey(int surveyID, int isdo, int flag = 0)
        {

            ViewBag.isdo = isdo;
            var survey = surveyBL.Get(surveyID);
            var examPaper = SeBl.GetSurveyExampaper(survey.PaperID);
            survey.examPaper = examPaper;
            if (survey.PaperID > 0)
            {
                var replayAnswer = replayAnswerBL.GetAnswerBySurvey(surveyID, survey.PaperID, CurrentUser.UserId);
                foreach (var item in examPaper.Questions)
                {
                    item.ReplyAnswer = replayAnswer.Where(p => p.QuestionID == item.QuestionID).FirstOrDefault();
                }
            }
            ViewBag.Flag = flag;
            return View(survey);
        }

        /// <summary>
        /// 调查问题浏览
        /// </summary>
        /// <param name="surveyID">ID</param>
        /// <returns></returns>
        public ActionResult DeptBrowseSurveyResult(int surveyID, int paperID = 0, string backurl = "", int flag = 0)
        {
            ViewBag.backurl = backurl;

            var survey = surveyBL.Get(surveyID);
            var examPaper = SeBl.GetSurveyExampaper(survey.PaperID);
            survey.examPaper = examPaper;
            if (paperID > 0)
            {
                var replayAnswer = replayAnswerBL.GetAnswerBySurvey(surveyID, paperID, CurrentUser.UserId);
                foreach (var item in examPaper.Questions)
                {
                    item.ReplyAnswer = replayAnswer.Where(p => p.QuestionID == item.QuestionID).FirstOrDefault();
                }
            }
            ViewBag.Flag = flag;
            return View("DeptBrowseSurvey", survey);
        }

        public ActionResult SurveyReport()
        {
            return View();
        }

        public ActionResult SurveyReportDeatils(int userNum, int surveyID, int paperID)
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

        public ActionResult studentResult(int type = 0)
        {
            ViewBag.TrainGrade = trainBL.GetAllTrainGrade();
            ViewBag.type = type;
            return View();
        }

        #endregion

        #region 调查管理
        /// <summary>
        /// 查询调查管理的基本信息
        /// </summary>
        public JsonResult GetSurveyList(int DeptID, string Name = "", string startTime = "", string endTime = "", int status = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " PublishTime desc")
        {
            try
            {
                int totalcount = 0;
                var list = surveyBL.GetSurveyList(out totalcount, DeptID, Name.ReplaceSql(), startTime, endTime, status, pageIndex, pageSize, jsRenderSortField);
                foreach (var item in list)
                {
                    item.Name = item.Name.HtmlXssEncode();

                }
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
        /// 添加调查
        /// </summary>
        /// <param name="model"></param>
         [Filter.SystemLog("添加调查", LogLevel.Info)]
        public JsonResult InsertSurvey_Survey(int DeptID, Survey_Survey model)
        {
            try
            {
                string star = "";
                string end = "";
               // var TrainGradeTime = trainBL.IsUpdateTrain(out star, out end, AllSystemConfigs.Where(p => p.ConfigType == 11).FirstOrDefault(), DateTime.Now.Date);
                var list = new ParamConfigBl().GetConfig(CurrentUser.DeptId, 1);
                var TrainGradeTime = trainBL.IsUpdateTrain(out star, out end, list, DateTime.Now.Date);
                model.StartTime = model.StartTime == Convert.ToDateTime("0001/1/1 0:00:00") ? Convert.ToDateTime(star) : model.StartTime;
                model.EndTime = model.EndTime == Convert.ToDateTime("0001/1/1 0:00:00") ? Convert.ToDateTime(end) : model.EndTime;
                model.PublishFlag = 0;
                model.OpenGroup = model.OpenGroup == null ? "" : model.OpenGroup;
                model.OpenDepart = model.OpenDepart == null ? "" : model.OpenDepart + "," + CurrentUser.DeptId;
                model.Memo = string.IsNullOrEmpty(model.Memo) ? "" : model.Memo.Trim();
                model.UserID = CurrentUser.UserId;
                model.DeptID = DeptID;
                if (model.chbOpenFlag == 1)
                {
                    model.OpenGroupFlag = 1;
                }
                else if (model.chbOpenFlag == 2)
                {
                    model.OpenDepartFlag = 1;
                }
                else
                {
                    model.OpenGroupFlag = 1;
                    model.OpenDepartFlag = 1;
                }
                if (model.Id > 0)
                {
                    surveyBL.UpdateSurvey_Survey(model);
                }
                else
                {
                    surveyBL.InsertSurvey_Survey(model);
                }

                return Json(new
                {
                    result = 1,
                    Content = "添加成功"
                });
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "添加失败"
                });
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Filter.SystemLog("删除调查", LogLevel.Info)]
        public JsonResult Delete(int id)
        {
            try
            {
                surveyBL.Delete(id);

                return Json(new
                {
                    result = 1,
                    Content = "删除成功"
                });
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "删除失败"
                });
            }
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         [Filter.SystemLog("发布调查", LogLevel.Info)]
        public JsonResult Publish(int id)
        {
            try
            {
                surveyBL.Publish(id);

                return Json(new
                {
                    result = 1,
                    Content = "发布成功"
                });
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    Content = "发布失败"
                });
            }
        }
        #endregion

        #region 参与需求调查
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0,未做 1已做</param>
        /// <returns></returns>
        public JsonResult GetDoSurveylist(int type, string Name = "", string startTime = "", string endTime = "", int IsAccessed = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " starttime asc")
        {
            try
            {

                var list = surveyBL.GetDoSurveyList(CurrentUser.UserId, Name.ReplaceSql(), startTime, endTime);
                if (IsAccessed != -1)
                {
                    switch (IsAccessed)
                    {
                        case 0:
                            list = list.Where(p => p.DoStatu == "未开始").ToList();
                            break;
                        case 1:
                            list = list.Where(p => p.DoStatu == "进行中").ToList();
                            break;
                        case 2:
                            list = list.Where(p => p.DoStatu == "已关闭").ToList();
                            break;
                    }
                }


                if (type == 0)
                {
                    list = list.Where(p => p.IsDo == 0).ToList();
                }
                else
                {
                    list = list.Where(p => p.IsDo > 0).ToList();
                }


                var totalcount = list.Count;
                if (jsRenderSortField.Contains(" desc"))
                {
                    list = list.OrderByDescending(p => p.StartTime).ToList();
                }
                else
                {
                    list = list.OrderBy(p => p.StartTime).ToList();
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
        /// 保存答案
        /// </summary>
        /// <param name="isdo"></param>
        /// <returns></returns>
        public JsonResult SaveReplyAnswer(int isdo)
        {
            try
            {
                var data = Request.Form["data"];
                dynamic replyAnswer = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                //  int deptID = surveyBL.GetDeptID(CurrentUser.UserId);
                for (int i = 0; i < replyAnswer.questionAnswer.Count; i++)
                {
                    var replayAnswer = new Survey_ReplyAnswer()
                    {
                        ObjectID = replyAnswer.ObjectID,
                        ExampaperID = replyAnswer.ExampaperID,
                        QuestionID = replyAnswer.questionAnswer[i].QuestionID,
                        ObjectiveAnswer = replyAnswer.questionAnswer[i].ObjectiveAnswer,
                        SubjectiveAnswer = replyAnswer.questionAnswer[i].SubjectiveAnswer,
                        QuestionReply = replyAnswer.questionAnswer[i].QuestionReply,
                        UserID = CurrentUser.UserId,
                        SourceFrom = 1,
                        IsDeptAccessed = 0,
                        IsOfficeAccessed = 0,
                        IsMaster = 0,
                        Status = 1,
                        DeptID = CurrentUser.DeptId
                    };
                    if (isdo == 0)
                    {
                        replayAnswerBL.InsertSurvey_ReplyAnswer(replayAnswer);
                    }
                    else
                    {
                        replayAnswerBL.UpdateReaplyAnswer(replayAnswer);
                    }
                }
                return Json(new
                {
                    result = 1,
                    content = "添加成功"
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "添加失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region  问卷审核
        /// <summary>
        /// 问卷审核首页（事务所）
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="isInDate"></param>
        /// <returns></returns>
        public JsonResult GetSurveyReport(string Name = "", int isInDate = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " StartTime desc")
        {
            try
            {
                int totalCount = 0;
                string where = " 1=1";
                if (isInDate != -1)
                {
                    if (isInDate == 1)
                    {
                        where += string.Format("AND  '{0}'  BETWEEN StartTime and  EndTime", DateTime.Now.Date);
                    }
                    else
                    {
                        where += string.Format(" AND ('{0}' <StartTime  OR '{0}'> EndTime)", DateTime.Now.Date);
                    }
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    where += string.Format(" and Name like '%{0}%'", Name.ReplaceSql());
                }

                var list = surveyBL.GetSurveyReport(out totalCount,CurrentUser.DeptId, CurrentUser.UserId, pageIndex, pageSize, where, jsRenderSortField);

                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount
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
        public JsonResult GetQuestionSurveyByDeptID(int surveyID, int paperID, int deptID)
        {
            try
            {
                var list = surveyBL.GetQuestionSurveyByDeptID(surveyID, paperID, deptID);
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
                var questionAnswer = SeBl.GetSurvey_QuestionAnswerByQuestionID(questionID);
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
        ///  获取主观题回答的基本情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSubjectOfficeAnswer(int surveyID, int paperID, int questionID, int deptID, string where = "")
        {
            try
            {
                var query = string.Format(" SubjectiveAnswer LIKE '%{0}%'", where.ReplaceSql());
                var list = surveyBL.GetOfficeReplayAnswer(surveyID, paperID, questionID, CurrentUser.UserId, deptID, query);
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
        ///  获取星评题回答的基本情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetXinPAnswersOffice(int surveyID, int paperID, int questionID,int DeptID)
        {
            try
            {
                var list = surveyBL.GetXinPAnswersOffice(questionID, paperID, surveyID, DeptID);
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
        /// 获取回答此问卷的所有人（按部门）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="grade"></param>
        /// <returns></returns>
        public JsonResult GetStudentByDept(int surveyID, int paperID, int OpenDepartFlag, string name = "", string grade = "")
        {
            try
            {
                var list = surveyBL.GetStudentByDept(surveyID, paperID, CurrentUser.DeptId, OpenDepartFlag, name.ReplaceSql(), grade.ReplaceSql());
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

                stuAnswerList = surveyBL.GetAnswerForStu(surveyID, paperID, userID);
                questionList = surveyBL.GetStuQuestion(surveyID, paperID, userID);

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

        #region 部门树的方法
        public ActionResult GetDeptTree(int surveyID, int paperID, int OpenDepartFlag)
        {
            int depID = 0;
            var treeStr = GetDeptTreeStr(out depID, surveyID, paperID, OpenDepartFlag);
            return Json(new
            {
                depID = depID,
                dataList = treeStr.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public StringBuilder GetDeptTreeStr(out int depID, int surveyID, int paperID, int OpenDepartFlag)
        {
            string prefix = "";

            var treeStr = new StringBuilder();

            treeStr.AppendFormat("<ul id=\"navigation\" class=\"treeview\"><li id=\"0\"><span title='{0}'  class=\"rootNote\">{0}</span><ul>", "立信组织结构");

            var deptlist = surveyBL.GetDeptForDepartment(surveyID, paperID, CurrentUser.DeptId, OpenDepartFlag).OrderByDescending(p => p.IsSubmit).ToList();
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

