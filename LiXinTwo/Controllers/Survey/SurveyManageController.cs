using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.Survey;
using LiXinLanguage;
using LiXinModels.Survey;
using LiXinBLL.Survey;
using LiXinModels.CourseManage;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public partial class SurveyController
    {

        #region 呈现
        public ActionResult SurveyManage()
        {
            return View();
        }
        public ActionResult SurveyEdit(int id = 0)
        {
            Survey_Survey survey = new Survey_Survey();
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
            else
            {
                string star;
                string end;

                var TrainGradeTime = trainBL.IsUpdateTrain(out star, out end, AllSystemConfigs.Where(p => p.ConfigType == 11).FirstOrDefault(), DateTime.Now.Date);
                ViewBag.start = star == "" ? "" : Convert.ToDateTime(star).ToString("yyyy-MM-dd");
                ViewBag.end = end == "" ? "" : Convert.ToDateTime(end).ToString("yyyy-MM-dd");
            }
            return View(survey);
        }

        /// <summary>
        /// 调查问题浏览
        /// </summary>
        /// <param name="surveyID">ID</param>
        /// <returns></returns>
        public ActionResult BrowseSurvey(int surveyID, int paperID = 0, string backurl = "")
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

        /// <summary>
        /// 调查问题浏览
        /// </summary>
        /// <param name="surveyID">ID</param>
        /// <returns></returns>
        public ActionResult BrowseSurveyResult(int surveyID, int paperID = 0, string backurl = "",int flag=0)
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
            ViewBag.Flag = 1;
            return View("BrowseSurvey", survey);
        }

        /// <summary>
        /// 调查问题浏览(新进员工)
        /// </summary>
        /// <param name="surveyID">ID</param>
        /// <returns></returns>
        public ActionResult NewBrowseSurveyResult(int surveyID, int paperID = 0, string backurl = "")
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
            ViewBag.Flag = 2; 
            return View("BrowseSurvey", survey);
        }

        /// <summary>
        /// 调查问卷浏览
        /// </summary>
        /// <param name="paperID">ID</param>
        /// <returns></returns>
        public ActionResult BrowsePaper(int id)
        {
            var examPaper = SeBl.GetSurveyExampaper(id);
            return View(examPaper);
        }

        /// <summary>
        /// 我的需求
        /// </summary>
        /// <returns></returns>
        public ActionResult DoSurveyList()
        {
            ViewBag.Flag = 0;
            return View();
        }
        /// <summary>
        /// 新员工-我的需求
        /// </summary>
        /// <returns></returns>
        public ActionResult MySurveyList()
        {
            ViewBag.Flag = 1;
            return View("DoSurveyList");
        }

        public ActionResult DoSurvey(int surveyID, int isdo, int flag = 0)
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
        /// 新进员工做需求调查
        /// </summary>
        /// <returns></returns>
        public ActionResult NewDoSurvey(int surveyID, int isdo)
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
            ViewBag.Flag = 1;
            return View("DoSurvey", survey);
        }


        /// <summary>
        /// 调查问卷浏览
        /// </summary>
        /// <param name="paperID">ID</param>
        /// <returns></returns>
        public ActionResult NewBrowsePaper(int id)
        {
            var examPaper = SeBl.GetSurveyExampaper(id);
            return View(examPaper);
        }
        #endregion

        #region 调查管理
        /// <summary>
        /// 查询调查管理的基本信息
        /// </summary>
        public JsonResult GetSurveyList(string Name = "", string startTime = "", string endTime = "", int status = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " PublishTime desc")
        {
            try
            {
                int totalcount = 0;
                var list = surveyBL.GetSurveyList(out totalcount, Name.ReplaceSql(), startTime, endTime, status, pageIndex, pageSize, jsRenderSortField);
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
        [Filter.SystemLog("编辑调查", LogLevel.Info)] 
        public JsonResult InsertSurvey_Survey(Survey_Survey model)
        {
            try
            {
                string star;
                string end;
              
                var TrainGradeTime = trainBL.IsUpdateTrain(out star, out end, AllSystemConfigs.Where(p => p.ConfigType == 11).FirstOrDefault(), DateTime.Now.Date);
                model.StartTime = model.StartTime == Convert.ToDateTime("0001/1/1 0:00:00") ? Convert.ToDateTime(star) : model.StartTime;
                model.EndTime = model.EndTime == Convert.ToDateTime("0001/1/1 0:00:00") ? Convert.ToDateTime(end) : model.EndTime;
               // model.PublishFlag = 0;
                model.OpenGroup = model.OpenGroup == null ? "" : model.OpenGroup;
                model.OpenDepart = model.OpenDepart == null ? "" : model.OpenDepart;
                model.Memo = string.IsNullOrEmpty(model.Memo) ? "" : model.Memo.Trim();
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
                model.UserID = CurrentUser.UserId;
                if (model.Id > 0)
                {
                    surveyBL.UpdateSurvey_Survey(model);
                }
                else
                {
                    model.PublishFlag = 0;
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

        public JsonResult SaveReplyAnswer(int isdo)
        {
            try
            {
                var data = Request.Form["data"];
                dynamic replyAnswer = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                int deptID = surveyBL.GetDeptID(CurrentUser.UserId);
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
                        DeptID = deptID
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

        //public JsonResult GetAnswerBySurvey(int surveyID, int paperID)
        //{
        //    try
        //    {
        //        var data = replayAnswerBL.GetAnswerBySurvey(surveyID, paperID, CurrentUser.UserId);
        //        return Json(new
        //        {
        //            result = 0,
        //            data = data
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json(new
        //        {
        //            result = 0,
        //            data = new List<Survey_ReplyAnswer>()
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        #endregion

    }
}
