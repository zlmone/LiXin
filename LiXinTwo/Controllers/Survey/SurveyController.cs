using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.Survey;
using LiXinLanguage;
using LiXinModels.Survey;
using LiXinInterface;
using LiXinInterface.SystemManage;
using LiXinInterface.User;
using System.Web;
using System.IO;
using System.Data;
using LiXinModels;
using System.Configuration;
using System.Web.Script.Serialization;
using LiXinControllers.Filter;
namespace LiXinControllers
{
    public partial class SurveyController : BaseController
    {
        protected ISurveyExampaper SeBl;
        protected ISurvey_Survey surveyBL;
        protected ISys_TrainGrade trainBL;
        protected ISys_Group sys_GroupBL;
        protected IDepartment deptBL;
        protected ISurveyReplyAnswer replayAnswerBL;
        protected ISurveyQuestionAnswer questionAnswerBL;
        public SurveyController(ISurveyExampaper seBl, ISurvey_Survey _surveyBL, ISys_TrainGrade _trainBL, ISys_Group _sys_GroupBL, IDepartment _deptBL, ISurveyReplyAnswer _replayAnswerBL, ISurveyQuestionAnswer _questionAnswerBL)
        {
            SeBl = seBl;
            surveyBL = _surveyBL;
            trainBL = _trainBL;
            sys_GroupBL = _sys_GroupBL;
            deptBL = _deptBL;
            replayAnswerBL = _replayAnswerBL;
            questionAnswerBL = _questionAnswerBL;
        }

        #region 页面呈现

        /// <summary>
        /// 问卷列表呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyExampaperList()
        {
            return View();
        }
        /// <summary>
        /// 问卷编辑呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyExampaperEdit(int id, int sortID)
        {
            ViewBag.sortName = sortID > 0 ? SeBl.GetPaperSort(sortID).CategoryName : "";
            ViewBag.paperID = id;
            ViewBag.sortID = sortID;
            return View();
        }

        /// <summary>
        /// 创建分类的页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult SortEdit()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            var sort = id > 0 ? SeBl.GetPaperSort(id) : new Survey_ExampaperCategory();
            ViewData["model"] = sort;

            if (Request.QueryString["fatherID"] != null)
            {
                ViewData["fatherModel"] = Request.QueryString["fatherID"] == "0"
                                              ? "问卷分类"
                                              : SeBl.GetPaperSortList().FirstOrDefault(p => p.CategoryId == Request.QueryString["fatherID"].StringToInt32()).CategoryName;
                ViewData["fatherID"] = Request.QueryString["fatherID"];
            }
            else
            {
                if (id > 0)
                {
                    ViewData["fatherModel"] = sort.ParentId == 0
                                                  ? "问卷分类"
                                                  : SeBl.GetPaperSortList().FirstOrDefault(p => p.CategoryId == sort.ParentId).CategoryName;
                    ViewData["fatherID"] = sort.ParentId;
                }
                else
                {
                    ViewData["fatherModel"] = CommonLanguage.Common_TreeFootName;
                    ViewData["fatherID"] = 0;
                }
            }
            return View();
        }


        public ActionResult AddSurveyQuestion()
        {
            return View();
        }

        public ActionResult ImportQuestion()
        {
            return View();
        }
        #endregion

        #region 问卷管理
        /// <summary>
        /// 根据一个父级分类获取下面所有的子分类
        /// </summary>
        public void GetSortByFatherID(List<Survey_ExampaperCategory> list, int sortID, ref List<int> sortlist)
        {
            var newlist = list.Where(p => p.ParentId == sortID);
            foreach (var item in newlist)
            {
                sortlist.Add(item.CategoryId);
                GetSortByFatherID(list, item.CategoryId, ref sortlist);
            }
        }

        /// <summary>
        /// 获取树形结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllPaperSortTree()
        {
            var paperSort = SeBl.GetPaperSortList().ToList();
            var treeStr = new StringBuilder();
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\">");
            treeStr.Append("<li id=\"0\" class='pNote'>");
            treeStr.AppendFormat("<a id=\"0\" title=\"{0}\" onclick=\"selectSort(0,'{0}');\">{0}</a>", "问卷分类");
            AddChild(0, paperSort, treeStr);
            treeStr.Append("</li>");
            treeStr.AppendLine("</ul>");
            return Json(treeStr.ToString(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 子节点的处理
        /// </summary>
        /// <param name="fSortID"></param>
        /// <param name="sortList"></param>
        /// <param name="treeStr"></param>
        private static void AddChild(int fSortID, List<Survey_ExampaperCategory> sortList, StringBuilder treeStr)
        {
            if (sortList.Count(p => p.ParentId == fSortID) > 0)
            {
                treeStr.Append("<ul>");
                foreach (var sort in sortList.Where(p => p.ParentId == fSortID))
                {
                    treeStr.AppendFormat(
                        "<li id='{0}' class='pNote'><a title=\"{1}\" onclick=\"selectSort({0},this);\" id=\"{0}\">{1}</a>",
                        sort.CategoryId, sort.CategoryName.HtmlXssEncode());
                    treeStr.AppendLine();
                    AddChild(sort.CategoryId, sortList, treeStr);
                    treeStr.AppendLine();
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
            }
        }

        /// <summary>
        /// 类别是否重复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult IsExsitPaperSort(int id, int pId, string name)
        {
            try
            {
                return Json(SeBl.IsExsitPaperSort(name, id, pId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
          
        }

        /// <summary>
        /// 保存问卷分类
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("编辑问卷分类", LogLevel.Info)] 
        public JsonResult SubmitPaperSort()
        {

            var id = Request.QueryString["id"].StringToInt32();
            var pId = Request.Form["hidfatherID"].StringToInt32();
            if (SeBl.IsExsitPaperSort(Request.Form["txtSortName"].Trim(), id, pId))
            {
                if (id == 0)
                {
                    //添加
                    var falg = SeBl.AddPaperSort(new Survey_ExampaperCategory
                    {
                        CategoryName = Request.Form["txtSortName"].Trim(),
                        ParentId = pId,
                        IsDelete = 0,
                        CategoryId = 0
                    });
                    if (falg)
                        return Json(new
                        {
                            result = 1,
                            content = "保存成功"
                        }, JsonRequestBehavior.AllowGet);
                    return Json(new
                                    {
                                        result = 0,
                                        content = "提交失败"
                                    }, JsonRequestBehavior.AllowGet);
                }
                //修改
                var sort = SeBl.GetPaperSort(id);
                sort.CategoryName = Request.Form["txtSortName"].Trim();
                if (SeBl.EditPaperSort(sort))
                    return Json(new
                                    {
                                        result = 1,
                                        content = "保存成功"
                                    }, JsonRequestBehavior.AllowGet);
                return Json(new
                                {
                                    result = 0,
                                    content = "提交失败"
                                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
                            {
                                result = 0,
                                content = "已存在此分类名称"
                            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("删除问卷分类", LogLevel.Info)]
        public JsonResult DeletePaperSortByID(int id)
        {
            if (id > 0)
            {
                var list = SeBl.GetServeyExamPaperList(string.Format(" Survey_Exampaper.IsDelete=0 and SortID={0} ", id));
                if (!list.Any() && SeBl.GetPaperSortList().All(p => p.ParentId != id))
                {
                    if (SeBl.DeletePaperSort(id))
                        return Json(new
                        {
                            result = 1,
                            content = "删除成功"
                        }, JsonRequestBehavior.AllowGet);
                    return Json(new
                                    {
                                        result = 0,
                                        content = "删除失败"
                                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                                {
                                    result = 0,
                                    content = "请先删除此分类下的问卷及子分类"
                                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
                            {
                                result = 0,
                                content = "删除失败"
                            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取问卷集合
        /// </summary>
        /// <param name="sortID">分类ID</param>
        /// <param name="name">搜索关键字</param>
        /// <param name="type">类型（0：普通问卷）</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页数目</param>
        /// <returns></returns>
        public JsonResult GetAllPaper(int sortID = 0, string name = "", int type = -1, int pageIndex = 1, int pageSize = 20, string jsRenderSortField = " LastUpdateTime desc")
        {
            var sortList = new List<int> { sortID };
            GetSortByFatherID(SeBl.GetPaperSortList(), sortID, ref sortList);
            var sortstr = string.Join(",", sortList);
            int totalCount;
            var paperList = SeBl.GetServeyExamPaperListPaging(out totalCount, string.Format(" Survey_Exampaper.SortID in ({0}) and Survey_Exampaper.IsDelete=0 and Survey_Exampaper.ExamTitle like '%{1}%' and {2}", sortstr, name.ReplaceSql(), (type == -1 ? "1=1" : ("Survey_Exampaper.ExamType=" + type))), pageIndex, pageSize, jsRenderSortField);
            foreach (var item in paperList)
            {
                item.ExamTitle = item.ExamTitle.HtmlXssEncode();
                item.SortName = item.SortName.HtmlXssEncode();
            }
            var objList = new List<object>();
            paperList.ForEach(p => objList.Add(new{
                                                      p.ExampaperID,
                                                      p.ExamTitle,
                                                      p.ExamTypeShow,
                                                      p.SortName,
                                                      p.LastUpdateTimeShow,
                                                      p.SortID,
                                                      p.ExamType
                                                  }));
           
            return Json(new
            {
                dataList = objList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="ids">问卷ID集合</param>
        /// <returns></returns>
        [Filter.SystemLog("删除问卷", LogLevel.Info)]
        public JsonResult DeleteSelectPapers(string ids)
        {
            return SeBl.DeleteServeyExamPaper(ids) ? Json(new
            {
                result = 1,
                content = "删除成功"
            }, JsonRequestBehavior.AllowGet) : Json(new
            {
                result = 1,
                content = "删除成功"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存问卷的
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("编辑问卷", LogLevel.Info)] 
        public JsonResult SaveExampaper()
        {
            try
            {
                string data = Request.Form["exampaper"];
                dynamic examdata = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                Survey_Exampaper exampaper;
                int examId = examdata.examId;
                if (examId > 0)
                {
                    exampaper = SeBl.GetSurveyExampaper(examId);
                }

                exampaper = new Survey_Exampaper
                {
                    ExamDescription = examdata.examDesc,
                    ExamTitle = examdata.examTitle,
                    ExamType = examdata.examType,
                    ExampaperID = examdata.examId,
                    SortID = examdata.sortID,
                    LastUpdateTime = DateTime.Now,
                    Questions = new List<Survey_Question>()
                };

                if (exampaper.ExampaperID == 0)
                {
                    exampaper.UserID = CurrentUser.UserId;
                }
                for (int i = 0; i < examdata.questions.Count; i++)
                {
                    var question = new Survey_Question
                    {
                        QuestionContent = examdata.questions[i].title,
                        QuestionOrder = examdata.questions[i].order,
                        QuestionType = examdata.questions[i].type,
                        Status = 0,
                        UpdateTime = DateTime.Now,
                        UserID = CurrentUser.UserId,
                        LinkSortPayGrade = examdata.questions[i].sortpayGrade,
                        Answers = new List<Survey_QuestionAnswer>(),
                        ObjectType=examdata.questions[i].objectType
                    };

                    question.QuestionContent = question.QuestionContent.HtmlDecode();
                    foreach (var answer in examdata.questions[i].answers)
                    {
                        var a = new Survey_QuestionAnswer
                        {
                            AnswerContent = answer.content,
                            ShowOrder = answer.order
                        };
                        question.Answers.Add(a);
                    }
                    exampaper.Questions.Add(question);
                }
                SeBl.AddExampaper(exampaper);

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

        /// <summary>
        /// 获取问卷
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public JsonResult GetSurveyExampaper(int paperID)
        {
            try
            {
                var examPaper = SeBl.GetSurveyExampaper(paperID);
                examPaper.SingleCount = examPaper.Questions.Count(p => p.QuestionType == 0);
                examPaper.MultipeCount = examPaper.Questions.Count(p => p.QuestionType == 1);
                examPaper.zhuguanCount = examPaper.Questions.Count(p => p.QuestionType == 2);
                examPaper.xingpCount = examPaper.Questions.Count(p => p.QuestionType == 3);
                return Json(new
                {
                    result = 1,
                    dataList = examPaper
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new Survey_Exampaper()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 问卷管理----导入
        // public JsonResult SubmitImportQuestion(HttpPostedFileBase FileData, string folder, int order)
        [ValidateInput(false)]
        public JsonResult SubmitImportQuestion(int order)
        {
            string folder = ConfigurationManager.AppSettings["SurveyUrl"];
            string filename = "";
            string resultName = "";
            string result = "0";
            try
            {
                HttpFileCollectionBase FileData = Request.Files;
                filename = Path.GetFileName(FileData[0].FileName); //获得文件名
                string fullPathname = Path.Combine(folder, filename);//文件后缀名
                string suffix = FileData[0].FileName.Substring(FileData[0].FileName.LastIndexOf(".") + 1).ToLower();
                resultName = Guid.NewGuid() + "." + suffix;
                saveFile(FileData, folder, resultName);
                List<DataTable> dtList = new Spreadsheet().LoadExcel(HttpContext.Server.MapPath(folder) + resultName);
                var questionlist = new List<Survey_Question>();
                var answerList = new Survey_QuestionAnswer();
                var newQuestionList = new List<Survey_Question>();

                //题型数
                var single = 0;
                var Multi = 0;
                var subject = 0;
                var xingp = 0;

                var flag = true;
                int rowcount = 0;
                var message = "";

                if (IsTrueTemplate(dtList[0]))
                {
                    foreach (DataRow row in dtList[0].Rows)
                    {
                        rowcount++;
                        var question = new Survey_Question();
                        //不为空的时候是题目，否则是选项
                        if (row[0].ToString() != "")
                        {
                            if (row[1].ToString() != "")
                            {
                                order = order + 1;
                                question.QuestionType = (int)((Enums.QuestionType)Enum.Parse(typeof(Enums.QuestionType), row[0].ToString()));
                                switch (question.QuestionType)
                                {
                                    case 0:
                                        single++;
                                        break;
                                    case 1:
                                        Multi++;
                                        break;
                                    case 2:
                                        subject++;
                                        break;
                                    case 3:
                                        xingp++;
                                        break;
                                }
                                question.QuestionContent = row[1].ToString();
                                question.QuestionOrder = order;
                                questionlist.Add(question);
                            }
                            else
                            {
                                flag = false;
                                message = message + "," + rowcount;
                            }

                        }
                        else
                        {

                            if (questionlist[questionlist.Count - 1].QuestionType != 2)
                            {

                                var ordr = questionlist[questionlist.Count - 1].Answers.Count == 0 ? 64 : (int)(System.Text.Encoding.ASCII.GetBytes(questionlist[questionlist.Count - 1].Answers.LastOrDefault().ShowOrder)[0]);
                                questionlist[questionlist.Count - 1].Answers.Add(new Survey_QuestionAnswer()
                                {
                                    AnswerContent = row[1].ToString(),
                                    ShowOrder = ((char)(ordr + 1)).ToString()
                                });

                            }

                        }
                    }
                    message = message == "" ? "" : message.TrimStart(',') + "行，题目为空";

                    foreach (var item in questionlist)
                    {

                        if (item.QuestionType == 0 || item.QuestionType == 1)
                        {
                            if (item.Answers.Count != 0 && item.Answers.Count(p => p.AnswerContent != "") > 2)
                            {
                                newQuestionList.Add(item);
                            }
                            else
                            {
                                flag = false;
                                var mess = "题目：" + item.QuestionContent + "导入失败，客观题至少需要2个答案";
                                message = message == "" ? mess : message + ";" + mess;
                            }
                        }
                        else
                        {
                            newQuestionList.Add(item);
                        }
                    }
                    //questionlist = questionlist.OrderBy(p => p.QuestionOrder).ToList();
                    message = " 导入成功" + newQuestionList.Count + "题" + message;

                }
                else
                {
                    return Json(new
                    {
                        result = 0,
                        content = "请使用正确的模板"
                    }, "text/html", JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    questionlist = newQuestionList,
                    single = single,
                    Multi = Multi,
                    subject = subject,
                    xingp = xingp,
                    result = 1,
                    content = message
                }, "text/html", JsonRequestBehavior.AllowGet);


            }
            catch
            {
                return Json(new
                {
                    questionlist = new List<Survey_Question>(),
                    result = 0,
                    content = "导入失败"
                }, "text/html", JsonRequestBehavior.AllowGet);
            }
        }
        public bool saveFile(HttpFileCollectionBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            if (!Directory.Exists(HttpContext.Server.MapPath(filepath)))
            {
                Directory.CreateDirectory(filepath);
            }
            try
            {
                string a = HttpContext.Server.MapPath(filepath);
                postedFile[0].SaveAs(a + "\\" + saveName);
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }

        //判断模板是否正确
        private bool IsTrueTemplate(DataTable dt)
        {
            if (dt.Columns.Count >= 2)
            {
                if (dt.Columns[0].ColumnName.Trim() != "题型")
                    return false;
                if (dt.Columns[0].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[1].ColumnName.Trim() != "题目")
                    return false;
                if (dt.Columns[1].DataType.FullName != "System.String")
                    return false;
            }
            else
                return false;
            return true;
        }
        #endregion
    }
}
