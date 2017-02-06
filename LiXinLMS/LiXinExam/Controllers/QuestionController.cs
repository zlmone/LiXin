using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinBLL.User;
using LiXinCommon;
using LiXinControllers;
using LiXinInterface.Examination;
using LiXinInterface.User;
using LiXinLanguage;
using LiXinModels.Examination;
using LiXinModels.Examination.DBModel;
using LiXinModels.Examination.ShowModel;
using LiXinModels.User;
using System.Web;

namespace LiXinExam.Controllers
{
    public partial class QuestionController : BaseController
    {
        protected IQuestion EQuestionBL;
        protected IQuestionSort ESortBL;

        protected IKnowledgeKey KnowBL;
        protected IExampaper PaperBL;
        protected IUser UserBL;

        public QuestionController(IQuestion eQuestionBL, IQuestionSort eSortBL, IUser userBL, IExampaper paperBL,
                                  IKnowledgeKey knowBL)
        {
            EQuestionBL = eQuestionBL;
            ESortBL = eSortBL;
            UserBL = userBL;
            PaperBL = paperBL;
            KnowBL = knowBL;
        }

        #region 页面呈现

        /// <summary>
        ///     题库管理
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionList()
        {
            return View();
        }


        /// <summary>
        ///     创建分类的页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult SortEdit()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            tbQuestionSort sort = id > 0 ? ESortBL.GetSingleByID(id) : new tbQuestionSort();
            ViewData["model"] = sort;

            if (Request.QueryString["fatherID"] != null)
            {
                ViewData["fatherModel"] = Request.QueryString["fatherID"] == "0"
                                              ? CommonLanguage.Common_TreeFootName
                                              : ESortBL.GetAllQuestionSortDictionary()[
                                                  Request.QueryString["fatherID"].StringToInt32()].Title;
                ViewData["fatherID"] = Request.QueryString["fatherID"];
            }
            else
            {
                if (id > 0)
                {
                    ViewData["fatherModel"] = sort.FatherID == 0
                                                  ? CommonLanguage.Common_TreeFootName
                                                  : ESortBL.GetAllQuestionSortDictionary()[sort.FatherID].Title;
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
        ///     导入试题页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportQuestions()
        {
            int id = Request.QueryString["id"].StringToInt32();
            if (id > 0)
            {
                //var qu = EQuestionBL.GetSingleByID(id);
                ViewData["fatherModel"] = ESortBL.GetAllQuestionSortDictionary()[id].Title;
            }
            else
            {
                if (Request.QueryString["sortID"] != null)
                {
                    ViewData["fatherModel"] = Request.QueryString["sortID"] == "0"
                                                  ? "无"
                                                  : ESortBL.GetAllQuestionSortDictionary()[
                                                      Request.QueryString["sortID"].StringToInt32()].Title;
                }
                else
                    ViewData["fatherModel"] = "无";
            }
            return View();
        }

        public ActionResult BrowseQuestion()
        {
            Response.Expires = 0;
            string ids = Request.QueryString["ids"];
            //基础信息
            List<tbQuestion> baseInfor = EQuestionBL.GetQuestionList(ids.Substring(1, ids.Length - 2));
            //知识点分类字典
            Dictionary<int, tbKnowledgeKey> konwDic = KnowBL.GetAllQuestionSortDictionary();
            var listQuestion = new List<MQuestionShow>();

            foreach (tbQuestion item in baseInfor)
            {
                var mq = new MQuestionShow
                    {
                        QuestionContent = item.QuestionContent,
                        QuestionKey = konwDic[item.QuestionKey].KeyName,
                        QuestionType = item.QuestionType,
                        QuestionTypeStr = ((QuestionType)item.QuestionType).ToString(),
                        QuestionAnswerKeys = item.QuestionAnswerKeys,
                        QuestionAnalysis = item.QuestionAnalysis,
                        QuestionAnswer = item.QuestionAnswer,
                        FileUpload =
                            (item.FileUpload == null || item.FileUpload.Count == 0)
                                ? new List<FileUpload>()
                                : item.FileUpload
                    };
                listQuestion.Add(mq);
            }

            // ViewData["model"] = listQuestion;
            ViewBag.model = listQuestion;
            return View();
        }

        #endregion

        #region 试题库的操作

        /// <summary>
        ///     获取树形结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllQuestionSortTree()
        {
            List<tbQuestionSort> questionSort = ESortBL.GetAllQuestionSortList().OrderBy(p => p._id).ToList();
            var treeStr = new StringBuilder();
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview\">");
            treeStr.Append("<li id=\"0\" class='pNote'>");
            treeStr.AppendFormat("<a id=\"0\" title=\"{0}\" onclick=\"selectSort(0,'{0}');\">{0}</a>", "立信");
            AddChild(0, questionSort, treeStr);
            treeStr.Append("</li>");
            treeStr.AppendLine("</ul>");
            return Json(treeStr.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQSortTree()
        {
            int divid = Request.QueryString["divid"].StringToInt32();
            List<tbQuestionSort> questionSort = ESortBL.GetAllQuestionSortList().OrderBy(p => p._id).ToList();
            var treeStr = new StringBuilder();
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\">");
            treeStr.Append("<li id=\"0\" class='pNote'>");
            treeStr.AppendFormat("<a id=\"0\" title=\"{0}\" onclick=\"selectSort(0,'{0}',{1});\">{0}</a>", "立信", divid);
            AddChild1(0, questionSort, treeStr, divid);
            treeStr.Append("</li>");
            treeStr.AppendLine("</ul>");
            return Json(treeStr.ToString(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     子节点的处理
        /// </summary>
        /// <param name="fSortID"></param>
        /// <param name="sortList"></param>
        /// <param name="treeStr"></param>
        private void AddChild(int fSortID, List<tbQuestionSort> sortList, StringBuilder treeStr)
        {
            if (sortList.Count(p => p.FatherID == fSortID) > 0)
            {
                treeStr.Append("<ul>");
                foreach (tbQuestionSort sort in sortList.Where(p => p.FatherID == fSortID))
                {
                    treeStr.AppendFormat(
                        "<li id='{0}' class='pNote'><a title=\"{1}\" onclick=\"selectSort({0},this);\" id=\"{0}\">{1}</a>",
                        sort._id, sort.Title);
                    treeStr.AppendLine();
                    AddChild(sort._id, sortList, treeStr);
                    treeStr.AppendLine();
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
            }
        }

        private void AddChild1(int fSortID, List<tbQuestionSort> sortList, StringBuilder treeStr, int divid)
        {
            if (sortList.Count(p => p.FatherID == fSortID) > 0)
            {
                treeStr.Append("<ul>");
                foreach (tbQuestionSort sort in sortList.Where(p => p.FatherID == fSortID))
                {
                    treeStr.AppendFormat(
                        "<li id='{0}' class='pNote'><a title=\"{1}\" onclick=\"selectSort({0},this,{2});\" id=\"{0}\">{1}</a>",
                        sort._id, sort.Title, divid);
                    treeStr.AppendLine();
                    AddChild1(sort._id, sortList, treeStr, divid);
                    treeStr.AppendLine();
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
            }
        }

        /// <summary>
        ///     保存问题分类
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitQuestionSort()
        {
            int id = Request.QueryString["id"].StringToInt32();
            int pId = Request.Form["hidfatherID"].StringToInt32();
            if (ESortBL.IsExsitName(pId, id, Request.Form["txtSortName"].Trim()))
            {
                if (id == 0)
                {
                    //添加
                    int newid = ESortBL.Insert(new tbQuestionSort
                        {
                            Description = Request.Form["txtMemo"],
                            Title = Request.Form["txtSortName"],
                            FatherID = pId,
                            Status = 0,
                            _id = 0
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
                                content = "提交失败"
                            }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //修改
                    tbQuestionSort sort = ESortBL.GetSingleByID(id);
                    sort.Description = Request.Form["txtMemo"];
                    sort.Title = Request.Form["txtSortName"];
                    int newid = ESortBL.ModifyByID(id, sort);
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
                                content = "提交失败"
                            }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                    {
                        result = 0,
                        content = "已存在此题库名称"
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///     根据ID删除实体
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteQuestionSortByID()
        {
            int id = Request.QueryString["id"].StringToInt32();
            if (id > 0)
            {
                int count = 0;
                List<tbQuestion> list = EQuestionBL.GetListByQuery(ref count, id, "", "", int.MaxValue, 1, 0, 0);
                if (list.Count() == 0 && ESortBL.GetAllQuestionSortList().Where(p => p.FatherID == id).Count() == 0)
                {
                    bool result = ESortBL.Delete(id);
                    if (result)
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
                else
                    return Json(new
                        {
                            result = 0,
                            content = "请先删除本题库下的试题及子题库"
                        }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new
                    {
                        result = 0,
                        content = "删除失败"
                    }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 试题操作

        public JsonResult GetAllQuestion(int pageSize = 10, int pageIndex = 1)
        {
            int sortID = Convert.ToInt32(Request.QueryString["sortID"]);
            int qType = Request.QueryString["type"].StringToInt32();
            int qlevel = Request.QueryString["level"].StringToInt32();
            string qKey = Request.QueryString["key"];
            string qContent = Request.QueryString["content"];

            //要补全
            int totalCount = 0;

            //获取所有的试题   
            List<tbQuestion> qList = EQuestionBL.GetListByQuery(ref totalCount, sortID, "", qKey, int.MaxValue,
                                                                1, qType, qlevel);
            qList = qList.Where(p => HttpUtility.HtmlDecode(p.QuestionContent).NoHtml().Contains(qContent)).ToList();
            totalCount = qList.Count;
            qList = qList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            //获取所有的人员信息
            List<Sys_User> userList = UserBL.GetList("1=1");
            //获取所有试卷
            List<tbExampaper> paperlist = PaperBL.GetAllExampaperList();
            //知识点分类字典
            Dictionary<int, tbKnowledgeKey> konwDic = KnowBL.GetAllQuestionSortDictionary();

            var ListQuestion = new List<MQuestionShow>();
            foreach (tbQuestion item in qList)
            {
                //调用次数
                int count = 0;
                paperlist.ForEach(p =>
                    {
                        if (p.QuestionList.Count(subp => subp.Qid == item._id) > 0)
                        {
                            count++;
                        }
                    });
                var Question = new MQuestionShow();
                Question.id = item._id;
                Question.QuestionContent = item.QuestionContent.NoHtml();
                Question.QuestionTypeStr = ((QuestionType)item.QuestionType).ToString();
                Question.QuestionLevelStr = ((QuestionLevel)item.QuestionLevel).ToString();
                Question.QuestionKey = konwDic.Keys.Contains(item.QuestionKey) ? konwDic[item.QuestionKey].KeyName : "";
                Question.VoidTimes = count;
                Question.Creater = (userList.Count == 0 || item.UserID == 0 || userList.All(p => p.UserId != item.UserID))
                                       ? "管理员"
                                       : userList.FirstOrDefault(p => p.UserId == item.UserID).Realname;
                Question.CreatLocalTime = item.CreateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
                ListQuestion.Add(Question);
            }
            return Json(new
                {
                    dataList = ListQuestion,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     删除题库
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteSelectQuestions()
        {
            string delStr = Request.QueryString["ids"];
            if (EQuestionBL.Delete(delStr))
                return Json(new
                    {
                        result = 1,
                        content = CommonLanguage.Common_DeleteSuccess
                    }, JsonRequestBehavior.AllowGet);
            else
                return Json(new
                    {
                        result = 0,
                        content = CommonLanguage.Common_AddFailed
                    }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     导入试题模板
        /// </summary>
        /// <returns></returns>
        public JsonResult SubmitImportQuestion()
        {
            var itemArray = new List<MQuestionShow>();
            int sortID = Request.QueryString["sortID"].StringToInt32(); //题库ID
            int key = Request.QueryString["key"].StringToInt32(); //知识点
            string strFileName = Request.Files[0].FileName;
            var ran = new Random();
            if (strFileName.Substring(strFileName.Length - 3, 3).ToUpper() != "XLS")
            {
                return Json(new
                    {
                        content = Question.Question_QuestionManage_Tip_FileFormat,
                        result = 0
                    }, "text/html", JsonRequestBehavior.AllowGet);
            }
            strFileName = System.Web.HttpContext.Current.Request.MapPath("~/ClientBin/UploadFile/") +
                          DateTime.Now.ToString(@"yyyyMMddHHmmss") + ran.Next(100, 999).ToString() + ".xls";
            Request.Files[0].SaveAs(strFileName);

            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFileName +
            //                 ";Extended Properties=Excel 8.0;";
            //var tmpcn = new OleDbConnection(strConn);
            //tmpcn.Open();
            //DataTable dt = tmpcn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            int count = 0; //错误的个数
            int totalCount = 0; //总数
            string message = "";
            bool templateFlag = false;
            List<DataTable> dtList = new Spreadsheet().LoadExcel(strFileName, 1);
            var dt = dtList.Count == 0 ? new DataTable() : dtList[0];
            if (IsTrueTemplate(dt))
            {
                templateFlag = true;
                itemArray = ImportQuestion(sortID, key, dt, ref message, ref count, ref totalCount);
            }
            else
            {
                return Json(new
                            {
                                content = Question.Question_QuestionManage_FileDataWrong,
                                result = 0
                            }, "text/html", JsonRequestBehavior.AllowGet);
            }
            //foreach (DataRow drr in dt.Rows)
            //{
            //    string strExcelTableName = drr["TABLE_NAME"].ToString(); //EXCEL中的工作表名
            //    if (strExcelTableName == "模板$")
            //    {
            //        templateFlag = true;
            //        var tmpda = new OleDbDataAdapter("select * from [" + strExcelTableName + "]", tmpcn);
            //        var ds = new DataSet();
            //        tmpda.Fill(ds);
            //        bool flag = IsTrueTemplate(ds);
            //        if (flag)
            //        {
            //            itemArray = ImportQuestion(sortID, key, ds, ref message, ref count, ref totalCount);
            //        }
            //        else
            //            return Json(new
            //                {
            //                    content = Question.Question_QuestionManage_FileDataWrong,
            //                    result = 0
            //                }, "text/html", JsonRequestBehavior.AllowGet);
            //    }
            //}


            if (templateFlag)
                return Json(new
                    {
                        itemArray,
                        content =
                                ((count == 0 && message == "")
                                     ? CommonLanguage.Common_ImportSuccess
                                     : Question.Question_QuestionManage_TotalImport + "：" + (totalCount - count) + "，" +
                                       "行号为" + message + "的格式有误，导入失败"),
                        result = 1
                    }, "text/html", JsonRequestBehavior.AllowGet);
            else
                return Json(new
                    {
                        content = Question.Question_QuestionManage_TempleteFormat,
                        result = 0
                    }, "text/html", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     导入试题
        /// </summary>
        /// <param name="sortID">分类ID</param>
        /// <param name="ds">试题DataSet</param>
        /// <param name="message">错误信息</param>
        /// <param name="count">错误数目</param>
        /// <param name="totalCount">导入的总题数</param>
        /// <returns></returns>
        public List<MQuestionShow> ImportQuestion(int sortID, int key, DataTable dt, ref string message, ref int count,
                                                  ref int totalCount)
        {
            var newlist = new List<MQuestionShow>();
            var questionList = new List<tbQuestion>();
            var question = new tbQuestion();
            var rowList = new List<int>();
            int rowCount = 0; //记录行号
            Dictionary<int, tbQuestionSort> dicsort = ESortBL.GetAllQuestionSortDictionary();
            foreach (DataRow dr in dt.Rows)
            {
                rowCount++;
                //如果是空行跳过
                //if (dr[1].ToString() == "")
                //{
                //    message += message == "" ? (rowCount + 1).ToString() : ("、" + (rowCount + 1).ToString());
                //    continue;
                //}
                //为空的时候是答案
                if (dr[3].ToString() == string.Empty && dr[1].ToString() != "")
                {
                    if (question.QuestionType > 0)
                    {
                        questionList[questionList.Count - 1].QuestionAnswer.Add(new QuestionAnswer
                            {
                                Answer =
                                    (question.QuestionType == 4
                                         ? (dr[1].ToString() == "正确" ? "0" : "1")
                                         : dr[1].ToString().Trim()),
                                AnswerFlag = dr[2].ToString() == "是" ? 1 : 0,
                            });
                    }
                }
                //保存试题,初始化model
                else
                {
                    if ((dr[3].ToString() == "问答题" || dr[3].ToString() == "填空题") && dr[6].ToString() == "")
                    {
                        message += message == "" ? (rowCount + 1).ToString() : ("、" + (rowCount + 1).ToString());
                    }
                    else
                    {
                        if (dr[1].ToString() != "" && dr[3].ToString() != "" && dr[4].ToString() != "")
                        {
                            question = new tbQuestion
                                {
                                    QuestionAnswer = new List<QuestionAnswer>(),
                                    QuestionKey = key,
                                    QuestionAnswerKeys = dr[6].ToString(),
                                    QuestionAnalysis = dr[5].ToString(),
                                    QuestionContent =
                                        dr[1].ToString()
                                             .Replace("（", "(")
                                             .Replace("（", "(")
                                             .Replace("（", "(")
                                             .Replace("）", ")")
                                             .Replace("）", ")")
                                             .Replace("）", ")"),
                                    QuestionLevel =
                                        (int)((QuestionLevel)Enum.Parse(typeof(QuestionLevel), dr[4].ToString())),
                                    QuestionType =
                                        (int)((QuestionType)Enum.Parse(typeof(QuestionType), dr[3].ToString())),
                                    Status = 0,
                                    LastUpdateTime = DateTime.Now,
                                    CreateTime = DateTime.Now,
                                    QuestionSortID = sortID,
                                    UserID = CurrentUser.UserId,
                                    FileUpload = new List<FileUpload>(),
                                };
                            questionList.Add(question);
                            rowList.Add(rowCount + 1);
                        }
                        else
                        {
                            message += message == "" ? (rowCount + 1).ToString() : ("、" + (rowCount + 1).ToString());
                        }
                    }
                }
            }

            int quCount = 0;
            foreach (tbQuestion qu in questionList)
            {
                quCount++;
                int qID = 0;
                if ((qu.QuestionType == 1 && qu.QuestionAnswer.Count == 1) //问答
                    ||
                    (qu.QuestionType == 2 && qu.QuestionAnswer.Count > 1 && qu.QuestionAnswer.Count < 27 &&
                     qu.QuestionAnswer.Where(p => p.AnswerFlag == 1).Count() == 1) //单选
                    ||
                    (qu.QuestionType == 3 && qu.QuestionAnswer.Count > 1 && qu.QuestionAnswer.Count < 27 &&
                     qu.QuestionAnswer.Where(p => p.AnswerFlag == 1).Count() > 0) //多选
                    ||
                    (qu.QuestionType == 4 && qu.QuestionAnswer.Count == 2 &&
                     qu.QuestionAnswer.Where(p => p.AnswerFlag == 1).Count() == 1) //判断
                    )
                {
                    int c = 0;
                    qu.QuestionAnswer.ForEach(pz =>
                    {
                        pz.Order = ++c;
                    });
                    qID = EQuestionBL.Insert(qu);
                    var newQu = new MQuestionShow();
                    newQu.FileUpload = new List<FileUpload>();
                    newQu.QuestionAnswer = qu.QuestionAnswer;
                    newQu.QuestionContent = qu.QuestionContent;
                    newQu.id = qu._id;
                    newQu.QuestionLevelStr = ((QuestionLevel)qu.QuestionLevel).ToString();
                    newQu.QuestionTypeStr = ((QuestionType)qu.QuestionType).ToString();
                    newQu.QuestionType = qu.QuestionType;
                    newQu.QuestionLevel = qu.QuestionLevel;
                    newQu.SortName = dicsort[sortID].Title;
                    newlist.Add(newQu);
                }
                else if (qu.QuestionType == 5)
                {
                    //判断（）的个数和答案数是否一致，
                    var answerNumber = qu.QuestionContent.Split(new[] { "()" }, StringSplitOptions.None).Length - 1;
                    if (qu.QuestionAnswer.Count == answerNumber)
                    {
                        //关键词的个数是否一样
                        if (qu.QuestionAnswerKeys.Split(' ').Length == answerNumber)
                        {


                            string anStr = "";
                            qu.QuestionAnswer.ForEach(p =>
                            {
                                anStr += anStr == "" ? p.Answer : ("!!%%!!" + p.Answer);
                            });
                            var answer = new QuestionAnswer
                                {
                                    Answer = anStr,
                                    AnswerFlag = qu.QuestionAnswer[0].AnswerFlag
                                };
                            var list = new List<QuestionAnswer>();
                            list.Add(answer);
                            qu.QuestionAnswer = list;
                            qID = EQuestionBL.Insert(qu);
                            var newQu = new MQuestionShow();
                            newQu.FileUpload = new List<FileUpload>();
                            newQu.QuestionAnswer = qu.QuestionAnswer;
                            newQu.QuestionContent = qu.QuestionContent;
                            newQu.id = qu._id;
                            newQu.QuestionLevelStr = ((QuestionLevel)qu.QuestionLevel).ToString();
                            newQu.QuestionTypeStr = ((QuestionType)qu.QuestionType).ToString();
                            newQu.QuestionType = qu.QuestionType;
                            newQu.QuestionLevel = qu.QuestionLevel;
                            newQu.SortName = dicsort[sortID].Title;
                            newQu.Creater = CurrentUser.Realname;
                            newlist.Add(newQu);
                        }
                        else
                        {
                            count++;
                            message += message == ""
                                           ? ((rowList[quCount - 1]).ToString())
                                           : ("、" + (rowList[quCount - 1]).ToString());
                        }
                    }
                    else
                    {
                        count++;
                        message += message == ""
                                       ? ((rowList[quCount - 1] + 1).ToString())
                                       : ("、" + (rowList[quCount - 1] + 1).ToString());
                    }
                }
                else
                {
                    count++;
                    message += message == ""
                                   ? ((rowList[quCount - 1] + 1).ToString())
                                   : ("、" + (rowList[quCount - 1] + 1).ToString());
                }

                totalCount++;
            }
            return newlist;
        }


        //判断模板是否正确
        private bool IsTrueTemplate(DataTable dt)
        {
            if (dt.Columns.Count >= 7)
            {
                if (dt.Columns[1].ColumnName.Trim() != "题干及答案")
                    return false;
                if (dt.Columns[1].DataType.FullName != "System.String")
                    return false;
                if (dt.Columns[2].ColumnName.Trim() != "正确答案")
                    return false;
                if (dt.Columns[3].ColumnName.Trim() != "题型")
                    return false;
                if (dt.Columns[4].ColumnName.Trim() != "难度")
                    return false;
                if (dt.Columns[5].ColumnName.Trim() != "试题分析")
                    return false;
                if (dt.Columns[6].ColumnName.Trim() != "关键词")
                    return false;
            }
            else
                return false;
            return true;
        }

        #endregion
    }
}