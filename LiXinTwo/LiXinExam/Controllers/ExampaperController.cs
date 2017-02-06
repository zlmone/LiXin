using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinBLL.User;
using LiXinCommon;
using LiXinControllers;
using LiXinInterface.Examination;
using LiXinLanguage;
using LiXinModels.Examination.DBModel;
using LiXinModels.Examination.ShowModel;
using LiXinModels.User;
using System.Data;
using LiXinControllers.Filter;

namespace LiXinExam.Controllers
{
    public class ExampaperController : BaseController
    {
        protected IExampaper EBL;
        protected IExampaperSort eSortBL;
        protected IQuestion qBL;
        protected IQuestionSort qSortBL;

        public ExampaperController(IExampaper _EBL, IExampaperSort _eSortBL, IQuestion _qBL, IQuestionSort _qSortBL)
        {
            EBL = _EBL;
            eSortBL = _eSortBL;
            qBL = _qBL;
            qSortBL = _qSortBL;
        }

        #region 呈现页面

        /// <summary>
        ///     试卷列表呈现
        /// </summary>
        public ViewResult ExampaperList()
        {
            ViewBag.Flag = 0;//事务所编辑试卷
            return View();
        }

        /// <summary>
        ///     试卷列表呈现
        /// </summary>
        public ViewResult DepExampaperList()
        {
            ViewBag.Flag = 1;//部门分所编辑试卷
            ViewBag.Departments = GetAllSubDepartments();
            return View("ExampaperList");
        }
        /// <summary>
        ///     编辑试卷呈现
        /// </summary>
        public ViewResult ExampaperEdit(int id)
        {
            ViewBag.Flag = 0;//事务所编辑试卷
            ViewData["id"] = id;
            if (id > 0)
            {
                var expape = EBL.GetExampaper(id);
                var questionList = expape.QuestionList;
                var ruleList = expape.QuestionRule;
                var itemArray = new List<tbQuestion>();
                if (questionList.Count > 0)
                {
                    //遍历试卷问题ID,获取题目
                    foreach (var baseInfor in questionList.Select(pquestion => qBL.GetSingleByID(pquestion.Qid)))
                    {
                        baseInfor.QuestionContent = baseInfor.QuestionContent;
                        itemArray.Add(baseInfor);
                    }
                }

                var itemArray1 = new List<MExamRuleShow>();
                if (ruleList.Count > 0)
                {
                    foreach (var qRule in ruleList)
                    {
                        var eq = new MExamRuleShow { QuestingScore = qRule.QScore };
                        switch (qRule.Qtype)
                        {
                            case 1: //问答题
                                eq.QuestionType = "问答题";
                                break;
                            case 2: //单选题
                                eq.QuestionType = "单选题";
                                break;
                            case 3: //多选题
                                eq.QuestionType = "多选题";
                                break;
                            case 4: //判断题
                                eq.QuestionType = "判断题";
                                break;
                            case 5: //填空题
                                eq.QuestionType = "填空题";
                                break;
                            case 6: //情景题
                                eq.QuestionType = "情景题";
                                break;
                        }

                        var qSort = qSortBL.GetSingleByID(qRule.QSort);
                        eq.QuestionSort = qSort.Title;
                        var questionLevel = qRule.QLevelStr.Split(';');
                        var Easy = questionLevel[0].Split(':');
                        var Common = questionLevel[1].Split(':');
                        var Hard = questionLevel[2].Split(':');
                        eq.Leveleasy = Convert.ToInt32(Easy[1]);
                        eq.Levelcommon = Convert.ToInt32(Common[1]);
                        eq.Levelhard = Convert.ToInt32(Hard[1]);
                        var qit1 = qRule.Qtype + "|" + qRule.QSort;
                        var qit = qRule.Qtype + "|" + qRule.QSort + "|" + qRule.QScore + "|" + Easy[1] + "|" +
                                     Common[1] + "|" + Hard[1];
                        eq.qita = qit;
                        eq.qitaone = qit1;

                        itemArray1.Add(eq);
                    }
                }
                ViewData["expape"] = expape;
                ViewData["expapeQuestion"] = itemArray;
                ViewData["expapeRule"] = itemArray1;
            }
            if (Request.QueryString["sortID"] != null)
            {
                ViewData["fatherModel"] = Request.QueryString["SortID"] == "0"
                                              ? Exampaper.NO
                                              : eSortBL.GetAllExampaperSortDictionary()[
                                                  Convert.ToInt32(Request.QueryString["sortID"])].Title;
            }
            else
                ViewData["fatherModel"] = Exampaper.NO;
            return View();
        }
        /// <summary>
        ///     编辑试卷呈现
        /// </summary>
        public ViewResult DepExampaperEdit(int id)
        {
            ViewBag.Flag = 1;//部门分所编辑试卷
            ViewData["id"] = id;
            if (id > 0)
            {
                var expape = EBL.GetExampaper(id);
                var questionList = expape.QuestionList;
                var ruleList = expape.QuestionRule;
                var itemArray = new List<tbQuestion>();
                if (questionList.Count > 0)
                {
                    //遍历试卷问题ID,获取题目
                    foreach (var baseInfor in questionList.Select(pquestion => qBL.GetSingleByID(pquestion.Qid)))
                    {
                        baseInfor.QuestionContent = baseInfor.QuestionContent;
                        itemArray.Add(baseInfor);
                    }
                }

                var itemArray1 = new List<MExamRuleShow>();
                if (ruleList.Count > 0)
                {
                    foreach (var qRule in ruleList)
                    {
                        var eq = new MExamRuleShow { QuestingScore = qRule.QScore };
                        switch (qRule.Qtype)
                        {
                            case 1: //问答题
                                eq.QuestionType = "问答题";
                                break;
                            case 2: //单选题
                                eq.QuestionType = "单选题";
                                break;
                            case 3: //多选题
                                eq.QuestionType = "多选题";
                                break;
                            case 4: //判断题
                                eq.QuestionType = "判断题";
                                break;
                            case 5: //填空题
                                eq.QuestionType = "填空题";
                                break;
                            case 6: //情景题
                                eq.QuestionType = "情景题";
                                break;
                        }

                        var qSort = qSortBL.GetSingleByID(qRule.QSort);
                        eq.QuestionSort = qSort.Title;
                        var questionLevel = qRule.QLevelStr.Split(';');
                        var Easy = questionLevel[0].Split(':');
                        var Common = questionLevel[1].Split(':');
                        var Hard = questionLevel[2].Split(':');
                        eq.Leveleasy = Convert.ToInt32(Easy[1]);
                        eq.Levelcommon = Convert.ToInt32(Common[1]);
                        eq.Levelhard = Convert.ToInt32(Hard[1]);
                        var qit1 = qRule.Qtype + "|" + qRule.QSort;
                        var qit = qRule.Qtype + "|" + qRule.QSort + "|" + qRule.QScore + "|" + Easy[1] + "|" +
                                     Common[1] + "|" + Hard[1];
                        eq.qita = qit;
                        eq.qitaone = qit1;

                        itemArray1.Add(eq);
                    }
                }
                ViewData["expape"] = expape;
                ViewData["expapeQuestion"] = itemArray;
                ViewData["expapeRule"] = itemArray1;
            }
            if (Request.QueryString["sortID"] != null)
            {
                ViewData["fatherModel"] = Request.QueryString["SortID"] == "0"
                                              ? Exampaper.NO
                                              : eSortBL.GetAllExampaperSortDictionary()[
                                                  Convert.ToInt32(Request.QueryString["sortID"])].Title;
            }
            else
                ViewData["fatherModel"] = Exampaper.NO;
            return View("ExampaperEdit");
        }

        /// <summary>
        ///     查看试卷呈现
        /// </summary>
        public ViewResult ExampaperDetail(int flag = 0)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            tbExampaper expape = EBL.GetExampaper(id);
            List<ReExampaperQuestion> questionList = expape.QuestionList;
            List<ReRuleQuestion> ruleList = expape.QuestionRule;

            var itemArray = new List<tbQuestion>();
            if (questionList.Count > 0)
            {
                //遍历试卷问题ID,获取题目
                foreach (ReExampaperQuestion Pquestion in questionList)
                {
                    tbQuestion baseInfor = qBL.GetSingleByID(Pquestion.Qid);
                    baseInfor.QuestionAnswer = baseInfor.QuestionAnswer.OrderByDescending(p => p.AnswerFlag).ToList();
                    itemArray.Add(baseInfor);
                }
            }

            var itemArray1 = new List<MExamRuleShow>();
            if (ruleList.Count > 0)
            {
                foreach (ReRuleQuestion qRule in ruleList)
                {
                    var eq = new MExamRuleShow();
                    eq.QuestingScore = qRule.QScore;
                    switch (qRule.Qtype)
                    {
                        case 1: //问答题
                            eq.QuestionType = "问答题";
                            break;
                        case 2: //单选题
                            eq.QuestionType = "单选题";
                            break;
                        case 3: //多选题
                            eq.QuestionType = "多选题";
                            break;
                        case 4: //判断题
                            eq.QuestionType = "判断题";
                            break;
                        case 5: //填空题
                            eq.QuestionType = "填空题";
                            break;
                        case 6: //情景题
                            eq.QuestionType = "情景题";
                            break;
                    }

                    tbQuestionSort qSort = qSortBL.GetSingleByID(qRule.QSort);
                    eq.QuestionSort = qSort.Title;
                    string[] questionLevel = qRule.QLevelStr.Split(';');
                    string[] Easy = questionLevel[0].Split(':');
                    string[] Common = questionLevel[1].Split(':');
                    string[] Hard = questionLevel[2].Split(':');
                    eq.Leveleasy = Convert.ToInt32(Easy[1]);
                    eq.Levelcommon = Convert.ToInt32(Common[1]);
                    eq.Levelhard = Convert.ToInt32(Hard[1]);
                    string qit1 = qRule.Qtype + "|" + qRule.QSort;
                    string qit = qRule.Qtype + "|" + qRule.QSort + "|" + qRule.QScore + "|" + Easy[1] + "|" + Common[1] +
                                 "|" + Hard[1];
                    eq.qita = qit;
                    eq.qitaone = qit1;

                    itemArray1.Add(eq);
                }
            }

            if (Request.QueryString["sortID"] != null)
            {
                if (flag == 1)
                {
                    ViewData["fatherModel"] = eSortBL.GetAllExampaperSortDictionary().Keys.Contains(expape.ExamSortID)
                                                  ? eSortBL.GetAllExampaperSortDictionary()[expape.ExamSortID].Title
                                                  : "无";
                }
                else
                {
                    ViewData["fatherModel"] = Request.QueryString["SortID"] == "0"
                                                  ? Exampaper.NO
                                                  : eSortBL.GetAllExampaperSortDictionary()[
                                                      Convert.ToInt32(Request.QueryString["sortID"])].Title;
                }
            }
            else
                ViewData["fatherModel"] = Exampaper.NO;
            ViewData["expape"] = expape;
            ViewData["expapeQuestion"] = itemArray;
            ViewData["expapeRule"] = itemArray1;
            return View();
        }

        /// <summary>
        ///     题库列表呈现
        /// </summary>
        public ViewResult ExamQuestionList()
        {
            return View();
        }

        /// <summary>
        ///     试卷库呈现
        /// </summary>
        public ViewResult ExampaperSort()
        {
            return View();
        }

        /// <summary>
        ///     自由组卷呈现
        /// </summary>
        public ViewResult ExampaperRandom()
        {
            List<tbQuestionSort> sortList = qSortBL.GetAllQuestionSortList();
            ViewData["sort"] = sortList;
            return View();
        }

        /// <summary>
        ///     组卷规则呈现
        /// </summary>
        public ViewResult ExampaperRule()
        {
            List<tbQuestionSort> sortList = qSortBL.GetAllQuestionSortList();
            ViewData["sort"] = sortList;
            return View();
        }

        /// <summary>
        ///     导入试题呈现
        /// </summary>
        public ViewResult ImportQuestions()
        {
            int id = Request.QueryString["id"].StringToInt32();
            if (id > 0)
            {
                //var qu = EQuestionBL.GetSingleByID(id);
                ViewData["fatherModel"] = qSortBL.GetAllQuestionSortDictionary()[id].Title;
            }
            else
            {
                if (Request.QueryString["sortID"] != null)
                {
                    ViewData["fatherModel"] = Request.QueryString["sortID"] == "0"
                                                  ? "无"
                                                  : qSortBL.GetAllQuestionSortDictionary()[
                                                      Request.QueryString["sortID"].StringToInt32()].Title;
                }
                else
                    ViewData["fatherModel"] = "无";
            }
            return View();
        }

        /// <summary>
        ///     选择试卷呈现
        /// </summary>
        public ViewResult ExampaperSelect()
        {

            return View();
        }

        #endregion

        #region 试卷操作

        /// <summary>
        ///     获取当前题库及其子题库
        /// </summary>
        /// <param name="pID">当前ID</param>
        /// <param name="sortList">题库集合</param>
        /// <param name="sortIDs"></param>
        private static void GetSortChildren(int pID, List<tbExampaperSort> sortList, ICollection<int> sortIDs)
        {
            sortIDs.Add(pID);
            if (sortList.Any(p => p.FatherID == pID))
            {
                sortList.Where(p => p.FatherID == pID).ToList().ForEach(p => GetSortChildren(p._id, sortList, sortIDs));
            }
        }

        /// <summary>
        ///     获取所有试卷列表
        /// </summary>
        public JsonResult GetAllExampaperList(int deptId = 0, int pageSize = 10, int pageIndex = 1)
        {
            var etitle = Request.QueryString["eTitle"].Trim(); //试卷标题
            var eSortID = Convert.ToInt32(Request.QueryString["eSortID"]); //试卷库分类ID
            var eUser = Request.QueryString["eUser"] ?? ""; //创建者
            var eExamType = Convert.ToInt32(Request.QueryString["eExamType"]); //试卷类型
            try
            {
                //分类
                var sortList = eSortBL.GetAllExampaperSortDictionary();
                var childrenSortIDs = new List<int>();
                if (eSortID != 0)
                    GetSortChildren(eSortID, sortList.Values.ToList(), childrenSortIDs);

                var userList = new UserBL().GetList("1=1");
                userList = userList.Where(p => p.Realname.ToLower().Contains(eUser.ToLower())).ToList();
                var temp = new int[userList.Count];
                for (var r = 0; r < userList.Count; r++)
                {
                    temp[r] = userList[r].UserId;
                }
                var examList = EBL.GetAllExampaperList().Where(p => p.DeptId == deptId && (eSortID == 0 || p.ExamSortID == eSortID || childrenSortIDs.Contains(p.ExamSortID)));
                examList = examList.Where(p => p.ExampaperTitle.ToLower().Contains(etitle.ToLower()));
                examList = examList.Where(p => temp.Contains(p.UserID));
                examList = examList.Where(p => (eExamType == 3 || p.ExamType == eExamType)).OrderByDescending(p => p._id);

                var list1 = examList.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();

                var alluserid = list1.Aggregate<tbExampaper, string>(null, (current, itemid) => current + (itemid.UserID + ","));
                alluserid = alluserid.Remove(alluserid.LastIndexOf(",", StringComparison.Ordinal), 1);
                var where = string.Format("UserId IN ({0})", alluserid);
                var userlist = new UserBL().GetList(where);

                var listExampaper = new List<MExampaperShow>();
                for (var j = 0; j < list1.Count(); j++)
                {
                    var str = "";
                    if (!string.IsNullOrEmpty(list1[j].Distribution.Trim()))
                    {
                        var dislist = list1[j].Distribution.Split(',');
                        foreach (var disstr in dislist.Select(t => t.Split(':')))
                        {
                            switch (disstr[0])
                            {
                                case "1":
                                    str += "问答" + disstr[1] + " ";
                                    break;
                                case "2":
                                    str += "单选" + disstr[1] + " ";
                                    break;
                                case "3":
                                    str += "多选" + disstr[1] + " ";
                                    break;
                                case "4":
                                    str += "判断" + disstr[1] + " ";
                                    break;
                                case "5":
                                    str += "填空" + disstr[1] + " ";
                                    break;
                                case "6":
                                    str += "情景" + disstr[1] + " ";
                                    break;
                            }
                        }
                    }
                    var exampaper = new MExampaperShow
                                        {
                                            id = list1[j]._id,
                                            ExampaperTitle = list1[j].ExampaperTitle,
                                            ExamType = (list1[j].ExamType == 0) ? "普通试卷" : "随机试卷",
                                            ExampaperScore = list1[j].ExampaperScore,
                                            ExamSortID = list1[j].ExamSortID,
                                            ExamSortTitle = sortList[list1[j].ExamSortID].Title,
                                            QuestionTypeList = str,
                                            Distribution = list1[j].Distribution,
                                            Description = list1[j].Description,
                                            LastUpdateTime = list1[j].LastUpdateTime.ToString("yyyy-MM-dd"),
                                            Creater = userlist.Any(p => p.UserId == list1[j].UserID)
                                                          ? userlist.First(p => p.UserId == list1[j].UserID)
                                                                .Realname.Replace("'", "’")
                                                          : "无"
                                        };
                    listExampaper.Add(exampaper);
                }
                return Json(new
                    {
                        dataList = listExampaper,
                        recordCount = examList.Count()
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
        ///     添加试卷&&修改试卷
        /// </summary>
        //[Filter.SystemLog("保存或修改试卷", LogLevel.Info)]
        [LiXinControllers.Filter.SystemLog("添加试卷&&修改试卷", LogLevel.Info)]
        public JsonResult SubmitExampaer(int id,int deptId=0)
        {
            int sID = Convert.ToInt32(Request.Form["Sortid"]);
            string desum = Request.Form["desum"];
            string typeorder = Request.Form["typeorder"];
            typeorder = typeorder.Remove(typeorder.LastIndexOf(","), 1);
            int examType = Convert.ToInt32(Request.Form["examType"]);
            int qp1 = 0;
            int qp2 = 0;
            int qp3 = 0;
            int qp4 = 0;
            int qp5 = 0;
            int qp6 = 0;
            //添加试卷问题            
            var paperquestion = new List<ReExampaperQuestion>();
            var rulequestion = new List<ReRuleQuestion>();
            string questionID = Request.Form["Queid"];
            if (!string.IsNullOrEmpty(questionID))
            {
                questionID = questionID.Remove(questionID.LastIndexOf(","), 1);
                string[] qIDlist = questionID.Split(',');
                string[] tolist = typeorder.Split(',');

                if (examType < 1)
                {
                    int abc = 1;
                    for (int i = 0; i < tolist.Length; i++)
                    {
                        for (int j = 0; j < qIDlist.Length; j++)
                        {
                            string[] qid = qIDlist[j].Split('|');
                            if (tolist[i] == qid[1])
                            {
                                var pq = new ReExampaperQuestion();
                                pq.Qid = Convert.ToInt32(qid[0]);
                                pq.QType = Convert.ToInt32(qid[1]);
                                string Scoreid = "Score" + qid[0];
                                pq.QOrder = abc;
                                pq.QScore = Convert.ToInt32(Request.Form[Scoreid]);
                                paperquestion.Add(pq);
                                abc++;
                            }
                        }
                    }
                }
                else
                {
                    for (int e = 0; e < tolist.Length; e++)
                    {
                        for (int s = 0; s < qIDlist.Length; s++)
                        {
                            var pq = new ReRuleQuestion();
                            string[] rulelist = qIDlist[s].Split('|');
                            if (tolist[e] == rulelist[0])
                            {
                                pq.QLevelStr = "1:" + rulelist[3] + ";2:" + rulelist[4] + ";3:" + rulelist[5];
                                pq.QScore = Convert.ToInt32(rulelist[2]);
                                pq.QSort = Convert.ToInt32(rulelist[1]);
                                pq.Qtype = Convert.ToInt32(rulelist[0]);
                                rulequestion.Add(pq);
                                switch (Convert.ToInt32(rulelist[0]))
                                {
                                    case 1:
                                        qp1 += Convert.ToInt32(rulelist[3]) + Convert.ToInt32(rulelist[4]) +
                                               Convert.ToInt32(rulelist[5]);
                                        break;
                                    case 2:
                                        qp2 += Convert.ToInt32(rulelist[3]) + Convert.ToInt32(rulelist[4]) +
                                               Convert.ToInt32(rulelist[5]);
                                        break;
                                    case 3:
                                        qp3 += Convert.ToInt32(rulelist[3]) + Convert.ToInt32(rulelist[4]) +
                                               Convert.ToInt32(rulelist[5]);
                                        break;
                                    case 4:
                                        qp4 += Convert.ToInt32(rulelist[3]) + Convert.ToInt32(rulelist[4]) +
                                               Convert.ToInt32(rulelist[5]);
                                        break;
                                    case 5:
                                        qp5 += Convert.ToInt32(rulelist[3]) + Convert.ToInt32(rulelist[4]) +
                                               Convert.ToInt32(rulelist[5]);
                                        break;
                                    case 6:
                                        qp6 += Convert.ToInt32(rulelist[3]) + Convert.ToInt32(rulelist[4]) +
                                               Convert.ToInt32(rulelist[5]);
                                        break;
                                }
                            }
                        }
                    }
                }
                if (examType > 0)
                {
                    if (qp1 > 0)
                    {
                        desum += "1:" + qp1 + ",";
                    }
                    if (qp2 > 0)
                    {
                        desum += "2:" + qp2 + ",";
                    }
                    if (qp3 > 0)
                    {
                        desum += "3:" + qp3 + ",";
                    }
                    if (qp4 > 0)
                    {
                        desum += "4:" + qp4 + ",";
                    }
                    if (qp5 > 0)
                    {
                        desum += "5:" + qp5 + ",";
                    }
                    if (qp6 > 0)
                    {
                        desum += "6:" + qp6 + ",";
                    }
                }
            }
            if (!string.IsNullOrEmpty(desum))
            {
                desum = desum.Remove(desum.LastIndexOf(","), 1);
            }
            else
            {
                desum = "";
            }
            if (id == 0)
            {
                //添加试卷
                int newid = EBL.InsertExampaper(new tbExampaper
                    {
                        ExampaperTitle = Request.Form["tbExamTitle"],
                        UserID = CurrentUser == null ? 0 : CurrentUser.UserId,
                        ExamSortID = sID,
                        ExamType = examType,
                        QuestionTypeOrder = typeorder,
                        ExampaperScore = Convert.ToInt32(Request.Form["question_sum"]),
                        LastUpdateTime = DateTime.Now,
                        CreateTime = DateTime.Now,
                        Description = Request.Form["tbRemark"],
                        Distribution = desum,
                        Status = 0,
                        QuestionRule = rulequestion,
                        QuestionList = paperquestion,
                        DeptId = deptId
                    });
                if (newid > 0)
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功"
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //修改
                var oldExam = EBL.GetExampaper(id);
                oldExam.ExampaperTitle = Request.Form["tbExamTitle"];
                oldExam.UserID = CurrentUser == null ? 0 : CurrentUser.UserId;
                oldExam.ExamSortID = sID;
                oldExam.ExamType = examType;
                oldExam.QuestionTypeOrder = typeorder;
                oldExam.ExampaperScore = Convert.ToInt32(Request.Form["question_sum"]);
                oldExam.LastUpdateTime = DateTime.Now;
                oldExam.Description = Request.Form["tbRemark"];
                oldExam.Distribution = desum;
                oldExam.QuestionRule = rulequestion;
                oldExam.QuestionList = paperquestion;
                var newid = EBL.UpdateExampaper(oldExam);
                if (newid)
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功"
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///     删除试卷
        /// </summary>
        [LiXinControllers.Filter.SystemLog("删除试卷", LogLevel.Info)]
        public JsonResult DeleteExampaer()
        {
            string idlist = Request.QueryString["IDlist"];
            if (EBL.DeleteExampapers(idlist))
                return Json(new
                {
                    result = 1,
                    content = "删除成功"
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new
                {
                    result = 0,
                    content = "删除失败"
                }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     添加多个题库
        /// </summary>
        public JsonResult GetQuestion()
        {
            string id = Request.QueryString["id"];
            var itemArray = new List<tbQuestion>();
            string[] idlist = id.Split(',');
            //获取题库信息
            for (int i = 0; i < idlist.Length - 1; i++)
            {
                tbQuestion QuestionInfor = qBL.GetSingleByID(Convert.ToInt32(idlist[i]));
                QuestionInfor.QuestionContent = QuestionInfor.QuestionContent;
                itemArray.Add(QuestionInfor);
            }
            return Json(itemArray, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     自由组卷
        /// </summary>
        public JsonResult GetRandomQuestion(string randonid, int deptId = 0)
        {
            var itemArray = new List<tbQuestion>();
            var randonlist = randonid.Split(',');

            //获取题库信息
            var questionitem = new List<tbQuestion>();
            for (var i = 0; i < randonlist.Length - 1; i++)
            {
                var question = randonlist[i];
                if (!string.IsNullOrEmpty(question))
                {
                    var qlist = question.Split('|');
                    var qType = Convert.ToInt32(qlist[0] == "" ? "0" : qlist[0]);
                    var qSort = Convert.ToInt32(qlist[1] == "" ? "0" : qlist[1]);
                    var qeasy = Convert.ToInt32(qlist[2] == "" ? "0" : qlist[2]);
                    var qcommon = Convert.ToInt32(qlist[3] == "" ? "0" : qlist[3]);
                    var qhard = Convert.ToInt32(qlist[4] == "" ? "0" : qlist[4]);

                    var allQuestion = qBL.GetQuestionList().Where(p => p.DeptId == deptId && (qType == 0 || p.QuestionType == qType) && (qSort == 0 || p.QuestionSortID == qSort)).OrderBy(p => p._id);
                    var alleasyQuestion = allQuestion.Where(p => (p.QuestionLevel == 1)).OrderBy(p => p._id);
                    var allcommonQuestion = allQuestion.Where(p => (p.QuestionLevel == 2)).OrderBy(p => p._id);
                    var allhardQuestion = allQuestion.Where(p => (p.QuestionLevel == 3)).OrderBy(p => p._id);
                    if (qeasy > 0)
                    {
                        if (alleasyQuestion.Any())
                        {
                            questionitem.AddRange(alleasyQuestion.ToList().RandomGetSome(qeasy));
                        }
                    }
                    if (qcommon > 0)
                    {
                        if (allcommonQuestion.Any())
                        {
                            questionitem.AddRange(allcommonQuestion.ToList().RandomGetSome(qcommon));
                        }
                    }
                    if (qhard > 0)
                    {
                        if (allhardQuestion.Any())
                        {
                            questionitem.AddRange(allhardQuestion.ToList().RandomGetSome(qhard));
                        }
                    }
                }
            }
            questionitem.ForEach(p =>
            {
                p.QuestionContent = p.QuestionContent;
            });
            return Json(new
            {
                Data = questionitem,
                result = 0
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     自由组卷ajax条件判断
        /// </summary>
        public JsonResult CheckRandomQuestion(int did, int qType, int qSort, int deptId = 0)
        {
            var qList = qBL.GetQuestionList().Where(p => p.DeptId == deptId);
            var qEasyList = qList.Where(p => (qType == 0 || p.QuestionType == qType) && (qSort == 0 || p.QuestionSortID == qSort) && (p.QuestionLevel == 1));
            var qCommonList = qList.Where(p => (qType == 0 || p.QuestionType == qType) && (qSort == 0 || p.QuestionSortID == qSort) && (p.QuestionLevel == 2));
            var qHardList = qList.Where(p => (qType == 0 || p.QuestionType == qType) && (qSort == 0 || p.QuestionSortID == qSort) && (p.QuestionLevel == 3));

            return Json(new
                {
                    result = 0,
                    DivId = did,
                    Easy = qEasyList.Count(),
                    Common = qCommonList.Count(),
                    Hard = qHardList.Count(),
                }, "text/html", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     验证试卷
        /// </summary>
        public JsonResult CheckExampaper(string etitle, string eid)
        {
            bool isValidate = false;
            IEnumerable<tbExampaper> exampaper = EBL.GetAllExampaperList().Where(s => (s.ExampaperTitle == etitle) && (s._id != Convert.ToInt32(eid)) && (s.Status == 0));
            if (exampaper.Count() > 0)
            {
                isValidate = false;
            }
            else
            {
                isValidate = true;
            }
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 试卷库

        /// <summary>
        ///     获取树形结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllExampaperSort(int deptId = 0)
        {
            var exampaperSort = eSortBL.GetAllExampaperSortList().Where(p => p.DeptId == deptId).OrderBy(p => p._id).ToList();
            var treeStr = new StringBuilder();
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview\">");
            treeStr.Append("<li id=\"0\" class='pNote'>");
            treeStr.AppendFormat("<a id=\"0\" title=\"{0}\" onclick=\"selectSort(0,this);\">{0}</a>", "立信");
            AddChild(0, exampaperSort, treeStr);
            treeStr.Append("</li>");
            treeStr.AppendLine("</ul>");
            return Json(treeStr.ToString(), JsonRequestBehavior.AllowGet);
        }

        private void AddChild(int fSortID, IEnumerable<tbExampaperSort> sorts, StringBuilder treeStr)
        {
            if (sorts.Count(p => p.FatherID == fSortID) > 0)
            {
                treeStr.Append("<ul>");
                foreach (tbExampaperSort sort in sorts.Where(p => p.FatherID == fSortID))
                {
                    treeStr.AppendFormat(
                        "<li id='{0}' class='pNote'><a title=\"{1}\" onclick=\"selectSort({0},this);\" id=\"{0}\">{1}</a>",
                        sort._id, sort.Title);
                    treeStr.AppendLine();
                    AddChild(sort._id, sorts, treeStr);
                    treeStr.AppendLine();
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
            }
        }

        /// <summary>
        ///  保存试卷分类
        /// </summary>
        /// <returns></returns>
        [LiXinControllers.Filter.SystemLog("编辑试卷分类", LogLevel.Info)]
        public JsonResult SubmitExampaperSort(int id, int deptId = 0)
        {
            var fatherID = Convert.ToInt32(Request.Form["hidfatherID"]);
            var title = Request.Form["txtSortName"];
            if (id == 0)
            {
                //添加
                var newid = eSortBL.Insert(new tbExampaperSort
                    {
                        Description = Request.Form["txtMemo"],
                        Title = title,
                        FatherID = fatherID,
                        Status = 0,
                        DeptId = deptId,
                        _id = 0
                    });
                if (newid > 0)
                    return Json(new
                        {
                            result = 1,
                            content = Exampaper.SaveSuccess
                        }, JsonRequestBehavior.AllowGet);
                return Json(new
                                {
                                    result = 0,
                                    content = Exampaper.SaveFail
                                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //修改
                var sort = eSortBL.GetSingleByID(id);
                sort.Description = Request.Form["txtMemo"];
                sort.Title = Request.Form["txtSortName"];
                var newid = eSortBL.ModifyByID(sort);
                if (newid > 0)
                    return Json(new
                        {
                            result = 1,
                            content = Exampaper.SaveSuccess
                        }, JsonRequestBehavior.AllowGet);
                return Json(new
                                {
                                    result = 0,
                                    content = Exampaper.SaveFail
                                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///     根据ID删除实体
        /// </summary>
        /// <returns></returns>
        [LiXinControllers.Filter.SystemLog("删除试卷分类", LogLevel.Info)]
        public JsonResult DeleteExampaperSortByID()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            if (id > 0)
            {
                if (EBL.GetAllExampaperList().Where(p => p.ExamSortID == id).Count() == 0 &&
                    eSortBL.GetAllExampaperSortList().Where(p => p.FatherID == id).Count() == 0)
                {
                    bool result = eSortBL.DeleteSingleByID(id, false);
                    if (result)
                        return Json(new
                            {
                                result = 1,
                                content = Exampaper.DeleteSuccess
                            }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new
                            {
                                result = 0,
                                content = Exampaper.DeleteFail
                            }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new
                        {
                            result = 0,
                            content = Exampaper.Prompt_Ten
                        }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new
                    {
                        result = 0,
                        content = Exampaper.DeleteFail
                    }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     创建分类的页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult ExampaperSortEdit()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            tbExampaperSort sort = id > 0 ? eSortBL.GetSingleByID(id) : new tbExampaperSort();
            ViewData["model"] = sort;
            if (Request.QueryString["fatherID"] != null)
            {
                ViewData["fatherModel"] = Request.QueryString["fatherID"] == "0"
                                              ? CommonLanguage.Common_TreeFootName
                                              : eSortBL.GetAllExampaperSortDictionary()[
                                                  Convert.ToInt32(Request.QueryString["fatherID"])].Title;
                ViewData["fatherID"] = Request.QueryString["fatherID"];
            }
            else
            {
                if (id > 0)
                {
                    ViewData["fatherModel"] = sort.FatherID == 0
                                                  ? CommonLanguage.Common_TreeFootName
                                                  : eSortBL.GetAllExampaperSortDictionary()[sort.FatherID].Title;
                    ViewData["fatherID"] = sort.FatherID;
                }
                else
                {
                    ViewData["fatherModel"] = CommonLanguage.Common_TreeFootName;
                    ViewData["fatherID"] = 0;
                }
            }
            return View();
        }

        /// <summary>
        ///     验证试卷分类
        /// </summary>
        public JsonResult CheckExampaperSort(string etitle, string eid, string fid, int deptId = 0)
        {
            var exampaperSort = eSortBL.GetAllExampaperSortList().Where(s => s.DeptId == deptId &&
                           (s.Title == etitle) && (s._id != Convert.ToInt32(eid)) &&
                           (s.FatherID == Convert.ToInt32(fid)) && (s.Status == 0));
            var isValidate = !exampaperSort.Any();
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}